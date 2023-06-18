using Business_AnimeToNotion.MAL;
using Business_AnimeToNotion.Mapper.Config;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Model;
using Data_AnimeToNotion.Repository;
using Microsoft.Extensions.Configuration;
using Notion.Client;

namespace Business_AnimeToNotion.Demo
{
    public class Demo_Integration : IDemo_Integration
    {
        #region DI

        private readonly IConfiguration _configuration;
        private readonly IAnimeShowRepository _animeShowRepository;
        private readonly IMAL_Integration _malIntegration;

        #endregion

        private NotionClient Client { get; set; }
        private string DataBaseId { get; set; } = null;

        public Demo_Integration(
                IConfiguration configuration,
                IAnimeShowRepository animeShowRepository,
                IMAL_Integration malIntegration
            )
        {
            #region DI
            _configuration = configuration;
            _animeShowRepository = animeShowRepository;
            _malIntegration = malIntegration;
            #endregion

            Client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = configuration["Notion-AuthToken"]
            });
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
                DataBaseId = database.Results[0].Id;
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
                    PageSize = 100,
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
                        throw new Exception("Error for: " + show.Id);
                    }

                }
            }
            while (retrievedNotionPages.HasMore);
        }

        private async Task MappingAndAdding(Page page)
        {
            AnimeShowDto anime = Mapping.Mapper.Map<AnimeShowDto>(page);

            if (_animeShowRepository.Exists(anime.MalId))
                return;

            var malAnime = await _malIntegration.MAL_SearchAnimeByIdAsync(anime.MalId);

            AnimeShow animeShow = MapAnimeShow(anime);

            _animeShowRepository.AddFromNotion(
                animeShow, 
                genres: Mapping.Mapper.Map<Dictionary<int, string>>(malAnime.genres), 
                studios: Mapping.Mapper.Map<Dictionary<int, string>>(malAnime.studios), 
                relations: Mapping.Mapper.Map<List<Relation>>(malAnime.related_anime));
        }

        /// <summary>
        /// Maps a DTO to the actual AnimeShow entity
        /// </summary>
        /// <param name="showDto"></param>
        /// <returns></returns>
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
            result.Score = showDto.Score != null ? Mapping.Mapper.Map<Score>(showDto.WatchingTime) : null;
            result.WatchingTime = showDto.WatchingTime != null ? Mapping.Mapper.Map<WatchingTime>(showDto.Score) : null;
            result.Note = showDto.Note != null ? Mapping.Mapper.Map<Note>(showDto.Note) : null;

            if (showDto.WatchingTime != null && showDto.WatchingTime.YearNotionPageId != null)
                result.WatchingTime.CompletedYear = _animeShowRepository.GetCompletedYearId(showDto.WatchingTime.YearNotionPageId);

            return result;
        }

        #endregion
    }
}
