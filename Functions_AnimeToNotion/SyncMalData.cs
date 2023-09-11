using Azure.Data.AppConfiguration;
using Business_AnimeToNotion.Integrations.Internal;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Query.Filter;
using Business_AnimeToNotion.Model.Query;
using Business_AnimeToNotion.QueryLogic.SortLogic;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Notion.Client;
using Functions_AnimeToNotion.Model;
using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Model.Pagination;

namespace Functions_AnimeToNotion
{
    public class SyncMalData
    {
        #region Config

        private static string MAL_Header = Environment.GetEnvironmentVariable("MAL_Header");
        private static string MAL_ApiKey = Environment.GetEnvironmentVariable("MALApiKey");
        private static string MAL_NotionNeededFields = Environment.GetEnvironmentVariable("MAL_NotionNeededFields");
        private static string MAL_BaseURL = Environment.GetEnvironmentVariable("MAL_BaseURL");
        private int PageSize = Convert.ToInt32(Environment.GetEnvironmentVariable("PageSize"));
        private ConfigurationClient configClient = new ConfigurationClient(Environment.GetEnvironmentVariable("AppConfiguration-ConnectionString"));

        private int currentPage;

        #region DI
        private readonly ILogger _logger;
        private readonly IAnimeShowRepository _animeRepository;
        private readonly IInternal_Integration _internalIntegration;
        private readonly IMAL_Integration _malIntegration;
        private readonly ISyncToNotionRepository _syncToNotionRepository;

        #endregion

        #endregion

        public SyncMalData(ILoggerFactory loggerFactory, IMAL_Integration malIntegration, IAnimeShowRepository animeRepository, IInternal_Integration internalIntegration, ISyncToNotionRepository syncToNotionRepository )
        {
            _logger = loggerFactory.CreateLogger<SyncMalData>();
            _animeRepository = animeRepository;
            _internalIntegration = internalIntegration;
            _malIntegration = malIntegration;
            _syncToNotionRepository = syncToNotionRepository;
        }

        [Function("SyncMalData")]
        public async Task Run([TimerTrigger("0 0 3 * * *"
            //,
            //#if DEBUG
            //    RunOnStartup= false
            //#endif
            )] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
                        
            currentPage = Convert.ToInt32(configClient.GetConfigurationSetting("AnimeToNotion-NextCursor").Value.Value);
            _logger.LogInformation($"Page: {currentPage}");

            var dbEntries = await GetFromDB();

            foreach (var dbEntry in dbEntries.Data)
            {
                try
                {
                    // Retrieve MAL anime record by Id
                    INT_AnimeShowFull MalEntry = await _malIntegration.GetAnimeById(MAL_Header, MAL_ApiKey, BuildMALUrl_SearchById(dbEntry.MalId));

                    // Check if there are differences
                    var changes = Utility.Utility.CheckDifferences(MalEntry, dbEntry);

                    // Update if there are differences
                    await UpdateItem(changes, dbEntry);

                    if(changes.Changes.Count > 0)
                        _logger.LogInformation($"{dbEntry.NameEnglish} - {dbEntry.MalId}: updated");
                    else
                        _logger.LogInformation($"{dbEntry.NameEnglish} - {dbEntry.MalId}: no differences");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{dbEntry.NameEnglish} - {dbEntry.MalId}: {ex.Message}");
                }
                
            }

            currentPage = dbEntries.PageInfo.HasNextPage ? currentPage + 1 : 1;
            configClient.SetConfigurationSetting("AnimeToNotion-NextCursor", (currentPage).ToString());
        }

        #region Private & Mapping

        private async Task<PaginatedResponse<INT_AnimeShowFull>> GetFromDB()
        {
            return (await _internalIntegration
                .LibraryQuery(new FilterIn(), SortIn.StartedAiring, new PageIn() { PerPage = PageSize, CurrentPage = currentPage }));
        }

        private string BuildMALUrl_SearchById(int id)
        {
            return $"{MAL_BaseURL}anime/{id}?{MAL_NotionNeededFields}";
        }

        private async Task UpdateItem(Changes_MalToInternal changes, INT_AnimeShowFull dbEntry)
        {
            if (changes.Changes.Count == 0)
                return;

            var anime = await _animeRepository.GetForEdit(dbEntry.Info.Id);

            anime = Utility.Utility.SetBasicChanges(anime, changes.ChangedAnime, changes.Changes);

            await _animeRepository.SyncFromMal(
                anime,
                changes.Changes.Contains("Studios") ? Mapping.Mapper.ProjectTo<Studio>(changes.ChangedAnime.Studios.AsQueryable()).ToList() : null,
                changes.Changes.Contains("Genres") ? Mapping.Mapper.ProjectTo<Genre>(changes.ChangedAnime.Genres.AsQueryable()).ToList() : null,
                changes.Changes.Contains("Relations") ? Mapping.Mapper.ProjectTo<Relation>(changes.ChangedAnime.Relations.AsQueryable()).ToList() : null
            );

            await _syncToNotionRepository.SetToSyncNotion(anime.Id, "Edit");
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
