using AutoMapper;
using Business_AnimeToNotion.MAL;
using Business_AnimeToNotion.Mapper;
using Business_AnimeToNotion.Model;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Model;
using Data_AnimeToNotion.Repository;
using Microsoft.Extensions.Configuration;
using Notion.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Demo
{
    public class Demo_Integration : IDemo_Integration
    {
        #region DI

        private readonly IConfiguration _configuration;
        private readonly IAnimeShowRepository _animeShowRepository;
        private readonly IRelationRepository _relationRepository;
        private readonly IMAL_Integration _malIntegration;

        #endregion

        private NotionClient Client { get; set; }
        private string DataBaseId { get; set; } = null;

        public Demo_Integration(
                IConfiguration configuration,
                IAnimeShowRepository animeShowRepository,
                IRelationRepository relationRepository,
                IMAL_Integration malIntegration
            )
        {
            #region DI
            _configuration = configuration;
            _animeShowRepository = animeShowRepository;
            _relationRepository = relationRepository;
            _malIntegration = malIntegration;
            #endregion

            Client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = configuration["Notion-AuthToken"]
            });
        }

        public async Task FromNotionToDB(string cursor)
        {
            await Notion_GetDataBaseId();

            Dictionary<string, string> differences = new Dictionary<string, string>();

            await RetrieveAllPages(DataBaseId, cursor);
        }

        #region Private

        private async Task Notion_GetDataBaseId()
        {
            if (string.IsNullOrEmpty(DataBaseId))
            {
                var database = await Client.Search.SearchAsync(new SearchParameters()
                {
                    Filter = new SearchFilter() { Value = SearchObjectType.Database },
                    Query = _configuration["Notion_ApiConfig:Notion_DatabaseName"]
                });
                DataBaseId = database.Results[0].Id;
            }
        }

        private async Task RetrieveAllPages(string DataBaseId, string cursor)
        {
            List<Page> result = new List<Page>();
            PaginatedList<Page> retrievedNotionPages = null;
            //Retrieve the pages that need to be updated
            do
            {
                retrievedNotionPages = await Client.Databases.QueryAsync(DataBaseId, new DatabasesQueryParameters()
                {
                    PageSize = 100,
                    StartCursor = cursor != "null" ? cursor : null
                });
                cursor = retrievedNotionPages.NextCursor;
                result.AddRange(retrievedNotionPages.Results);

                foreach (var show in result)
                {
                    try
                    {
                        await MapDBProperties(show);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Show rotto: " + show.Id);
                    }

                }
            }
            while (retrievedNotionPages.HasMore);
        }

        private async Task MapDBProperties(Page page)
        {
            AnimeShowDto anime = Mapping.Mapper.Map<AnimeShowDto>(page);

            if (_animeShowRepository.IsAlreadyExisting(anime.MalId))
                return;

            var malAnime = await _malIntegration.MAL_SearchAnimeByIdAsync(anime.MalId);

            anime.Genres = new Dictionary<int, string>();
            foreach (var genre in malAnime.genres)
            {
                anime.Genres.Add(genre.id, genre.name);
            }

            anime.Studios = new Dictionary<int, string>();
            foreach (var studio in malAnime.studios)
            {
                anime.Studios.Add(studio.id, studio.name);
            }

            AnimeShow animeShow = MapAnimeShow(anime);
            var addedAnime = _animeShowRepository.Add(animeShow);

            _animeShowRepository.AddStudios(anime.Studios, addedAnime);
            _animeShowRepository.AddGenres(anime.Genres, addedAnime);
            if (anime.WatchingTime != null) _animeShowRepository.AddWatchingTime(Mapping.Mapper.Map<WatchingTime>(anime.WatchingTime), anime.WatchingTime.YearNotionPageId, animeShow);
            if (anime.Score != null) _animeShowRepository.AddScore(Mapping.Mapper.Map<Score>(anime.Score), animeShow);
            if (anime.Note != null) _animeShowRepository.AddNote(Mapping.Mapper.Map<Note>(anime.Note), animeShow);

            HandleRelations(malAnime.related_anime, addedAnime.Id);
        }

        private AnimeShow MapAnimeShow(AnimeShowDto showDto)
        {
            AnimeShow result = new AnimeShow();
            result.Id = Guid.NewGuid();
            result.MalId = showDto.MalId;
            result.NotionPageId = showDto.NotionPageId;
            result.Episodes = showDto.Episodes;
            result.Status = showDto.Status;
            result.Format = showDto.Format;
            result.Cover = showDto.Cover;
            result.NameEnglish = showDto.NameEnglish;
            result.NameOriginal = showDto.NameOriginal;
            result.StartedAiring = showDto.StartedAiring;

            return result;
        }

        private void HandleRelations(List<MAL_RelatedShow> relations, Guid animeShowId)
        {
            if (relations.Count == 0)
                return;

            //bool isAnimeParent = !relations.Any(x => x.relation_type == "parent_story" || x.relation_type == "prequel" || x.relation_type == "other");
            //var malIdInvolved = relations.Select(x => x.node.id).ToList();
            //malIdInvolved.Add(malId);

            //Guid? animeParentId = isAnimeParent ? animeShowId : _relationRepository.SearchParentRelation(malIdInvolved);


            foreach (var rel in relations)
            {
                RelationDto newRel = new RelationDto();
                newRel.AnimeShowId = animeShowId;
                newRel.AnimeRelatedMalId = rel.node.id;
                newRel.RelationType = rel.relation_type;

                var relation = Mapping.Mapper.Map<Data_AnimeToNotion.DataModel.Relation>(newRel);

                _relationRepository.AddRelation(relation);
            }

            //if (isAnimeParent)
            //{
            //    int randomMalRelatedId = relations[0].node.id;
            //    _relationRepository.UpdateParentAnimeShowId(malId, randomMalRelatedId, animeParentId.Value);
            //}

        }

        #endregion
    }
}
