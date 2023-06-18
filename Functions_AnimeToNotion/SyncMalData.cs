using System.Text.Json;
using Azure.Data.AppConfiguration;
using Business_AnimeToNotion.Functions.Static;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.MAL;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Notion.Client;

namespace Functions_AnimeToNotion
{
    public class SyncMalData
    {
        #region Config

        private static string Notion_Auth_Token = Environment.GetEnvironmentVariable("NotionSecretKey");
        private static string MAL_Header = Environment.GetEnvironmentVariable("MAL_Header");
        private static string MAL_ApiKey = Environment.GetEnvironmentVariable("MALApiKey");
        private static string MAL_NotionNeededFields = Environment.GetEnvironmentVariable("MAL_NotionNeededFields");
        private static string MAL_BaseURL = Environment.GetEnvironmentVariable("MAL_BaseURL");
        private static string NextCursor = Environment.GetEnvironmentVariable("NextCursor");
        private string DataBaseId = Environment.GetEnvironmentVariable("Notion-DataBaseId");
        private int PageSize = Convert.ToInt32(Environment.GetEnvironmentVariable("PageSize"));
        private ConfigurationClient configClient = new ConfigurationClient(Environment.GetEnvironmentVariable("AppConfiguration-ConnectionString"));

        private NotionClient NotionClient;
        private HttpClient MALClient;

        #region DI
        private readonly ILogger _logger;
        private readonly IAnimeShowRepository _animeRepository;
        #endregion

        #endregion

        public SyncMalData(ILoggerFactory loggerFactory, IAnimeShowRepository animeRepository)
        {
            _logger = loggerFactory.CreateLogger<SyncMalData>();
            _animeRepository = animeRepository;
        }

        [Function("SyncMalData")]
        public async Task Run([TimerTrigger("0 0 1 * * *"
            //#if DEBUG
            //    RunOnStartup= true
            //#endif
            )] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            _logger.LogInformation($"{configClient.GetConfigurationSetting("AnimeToNotion-NextCursor").Value.Value}");

            #region Notion Client

            NotionClient = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = Notion_Auth_Token
            });

            #endregion Notion Client

            #region MAL Client

            MALClient = new HttpClient();
            MALClient.DefaultRequestHeaders.Add(MAL_Header, MAL_ApiKey);

            #endregion

            List<Page> notionEntries = await GetNotionEntries(_logger);

            foreach (var notionEntry in notionEntries)
            {
                // Retrieve MAL anime record by Id
                MAL_AnimeShow MALEntry = await GetMALById(Mapping.Mapper.Map<string>(notionEntry.Properties["MAL Id"]));

                // Check if there are differences
                var differences = CheckDifferences(MALEntry, notionEntry);

                // Update if there are differences
                await UpdateItem(MALEntry.title, differences, notionEntry, MALEntry, _logger);
            }
        }

        #region Private & Mapping

        private async Task<List<Page>> GetNotionEntries(ILogger log)
        {
            var notionEntries = await NotionClient.Databases.QueryAsync(
                DataBaseId,
                new DatabasesQueryParameters()
                {
                    // Ordered by the oldest modified items
                    Sorts = new List<Sort>() { new Sort() { Timestamp = Timestamp.LastEditedTime, Direction = Direction.Ascending } },
                    StartCursor = !string.Equals(configClient.GetConfigurationSetting("AnimeToNotion-NextCursor").Value, "no_cursor") ? NextCursor : null,
                    PageSize = PageSize
                }
            );

            log.LogInformation($"--- Next Cursor: {notionEntries.NextCursor}");

            if (notionEntries.NextCursor != null)
            {
                configClient.SetConfigurationSetting("AnimeToNotion-NextCursor", notionEntries.NextCursor);
            }
            else
            {
                configClient.SetConfigurationSetting("AnimeToNotion-NextCursor", "no_cursor");
            }

            return notionEntries.Results;
        }

        private async Task<MAL_AnimeShow> GetMALById(string id)
        {
            var response = await MALClient.GetStringAsync(BuildMALUrl_SearchById(id));
            return JsonSerializer.Deserialize<MAL_AnimeShow>(response);
        }

        private string BuildMALUrl_SearchById(string id)
        {
            return $"{MAL_BaseURL}anime/{id}?{MAL_NotionNeededFields}";
        }

        private Dictionary<string, PropertyValue> CheckDifferences(MAL_AnimeShow MALEntry, Page notionEntry)
        {
            var differences = new Dictionary<string, PropertyValue>();
            Common_Utilities.Equals(MALEntry, notionEntry, out differences);
            return differences;
        }

        private async Task UpdateItem(string title, Dictionary<string, PropertyValue> differences, Page notionEntry, MAL_AnimeShow malAnimeShow, ILogger log)
        {
            if (differences.Count > 0)
            {
                // Update SQL Database
                UpdateDatabase(notionEntry.Id, differences, malAnimeShow);

                // Update Notion entry
                await NotionClient.Pages.UpdateAsync(notionEntry.Id, new PagesUpdateParameters() { Properties = differences });

                LogDifferences(title, differences, notionEntry, log);
            }
            else
            {
                log.LogInformation($"___ {title} ___ No changes");
            }
        }

        private void LogDifferences(string title, Dictionary<string, PropertyValue> differences, Page notionEntry, ILogger log)
        {
            log.LogInformation($"*** {title} ***");
            foreach (var difference in differences)
            {
                log.LogInformation($"{difference.Key}: {Mapping.Mapper.Map<string>(notionEntry.Properties[difference.Key])} ----> {Mapping.Mapper.Map<string>(difference.Value)}");
            }
            log.LogInformation($"***************");
        }

        private void UpdateDatabase(string notionPageId, Dictionary<string, PropertyValue> differences, MAL_AnimeShow malAnimeShow)
        {
            // Retrieve AnimeShow by NotionPageId and maps the differences to the entity
            AnimeShow animeShow = Common_Utilities.MapFromNotionToAnimeShow(_animeRepository.GetByNotionPageId(notionPageId), differences);

            // Update anime show with the current info retrieved from MAL
            _animeRepository.SyncFromMal(
                animeShow,
                studios: Mapping.Mapper.Map<Dictionary<int, string>>(malAnimeShow.studios),
                genres: Mapping.Mapper.Map<Dictionary<int, string>>(malAnimeShow.genres),
                relations: Mapping.Mapper.Map<List<Relation>>(malAnimeShow));
        }

        #endregion Private & Mapping
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
