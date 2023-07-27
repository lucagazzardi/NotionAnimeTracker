using Business_AnimeToNotion.Integrations.Internal;
using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Internal;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Model;
using Data_AnimeToNotion.Repository;
using JikanDotNet;
using JikanDotNet.Config;
using Microsoft.Extensions.Configuration;
using Notion.Client;

namespace Business_AnimeToNotion.Integrations.Demo
{
    public class Demo_Integration : IDemo_Integration
    {
        #region DI

        private readonly IJikan _jikan;

        private readonly IConfiguration _configuration;
        private readonly IAnimeShowRepository _animeShowRepository;
        private readonly IMAL_Integration _malIntegration;
        private readonly IInternal_Integration _internalIntegration;

        #endregion

        private NotionClient Client { get; set; }
        private string DataBaseId { get; set; } = null;

        public Demo_Integration(
                IConfiguration configuration,
                IAnimeShowRepository animeShowRepository,
                IMAL_Integration malIntegration,
                IInternal_Integration internalIntegration
            )
        {
            #region DI
            _configuration = configuration;
            _animeShowRepository = animeShowRepository;
            _malIntegration = malIntegration;
            _internalIntegration = internalIntegration;
            #endregion

            Client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = configuration["Notion-AuthToken"]
            });

            var config = new JikanClientConfiguration()
            {
                LimiterConfigurations = TaskLimiterConfiguration.None
            };
            _jikan = new Jikan(config);
        }

        /// <summary>
        /// Add Notion entries to database
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public async Task FromNotionToDB(string cursor)
        {
            await Notion_GetDataBaseId();

            Dictionary<string, string> differences = new Dictionary<string, string>();

            await AddToDB(DataBaseId, cursor);
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
                DataBaseId = database.Results.Single(x => ((Database)x).Title[0].PlainText == _configuration["Notion_ApiConfig:Notion_DatabaseName"]).Id;
            }
        }

        private async Task AddToDB(string DataBaseId, string cursor)
        {
            List<Page> result = new List<Page>();
            PaginatedList<Page> retrievedNotionPages = null;
            //Retrieve the pages that need to be updated
            do
            {
                retrievedNotionPages = await Client.Databases.QueryAsync(DataBaseId, new DatabasesQueryParameters()
                {
                    PageSize = 10,
                    StartCursor = cursor != "null" ? cursor : null
                });
                cursor = retrievedNotionPages.NextCursor;
                result.AddRange(retrievedNotionPages.Results);

                foreach (var show in result)
                {
                    try
                    {
                        await MappingAndAdding(show);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error for: " + show.Id + ": " + ex);
                    }

                }
            }
            while (retrievedNotionPages.HasMore);
        }

        private async Task MappingAndAdding(Page page)
        {
            int animeId = (int)((NumberPropertyValue)page.Properties["MAL Id"]).Number.Value;

            if (await _animeShowRepository.Exists(animeId))
                return;

            var anime = _jikan.GetAnimeAsync(animeId);            
            var malRelations = _malIntegration.GetRelationsFromMAL(animeId);
            await Task.Delay(1000);
            await Task.WhenAll(anime, malRelations);

            var added = await _animeShowRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(Mapping.Mapper.Map<INT_AnimeShowFull>((await anime).Data)),
                Mapping.Mapper.ProjectTo<Studio>(Mapping.Mapper.Map<INT_AnimeShowFull>((await anime).Data).Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(Mapping.Mapper.Map<INT_AnimeShowFull>((await anime).Data).Genres.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Relation>(Mapping.Mapper.ProjectTo<INT_AnimeShowRelation>((await malRelations).related_anime.AsQueryable())).ToList()
            );

            var edit = Mapping.Mapper.Map<INT_AnimeShowEdit>(page);
            edit.Id = added.Id;
            await _internalIntegration.EditAnime(edit);
        }

        /// <summary>
        /// Maps a DTO to the actual AnimeShow entity
        /// </summary>
        /// <param name="showDto"></param>
        /// <returns></returns>
        private async Task<AnimeShow> MapAnimeShow(AnimeShowDto showDto)
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
            result.NameDefault = showDto.NameOriginal;
            result.StartedAiring = showDto.StartedAiring;
            result.Score = showDto.Score != null ? Mapping.Mapper.Map<Score>(showDto.Score) : null;
            result.WatchingTime = showDto.WatchingTime != null ? Mapping.Mapper.Map<WatchingTime>(showDto.WatchingTime) : null;
            result.Note = showDto.Note != null ? Mapping.Mapper.Map<Note>(showDto.Note) : null;

            if (showDto.WatchingTime != null && showDto.WatchingTime.CompletedYear != null)
                result.WatchingTime.CompletedYear = showDto.WatchingTime.CompletedYear;

            return result;
        }

        #endregion
    }
}
