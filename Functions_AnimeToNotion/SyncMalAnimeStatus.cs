using Azure.Data.AppConfiguration;
using Business_AnimeToNotion.Integrations.Internal;
using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.MAL;
using Data_AnimeToNotion.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions_AnimeToNotion
{
    public class SyncMalAnimeStatus
    {
        #region Config

        private static string MAL_Header = Environment.GetEnvironmentVariable("MAL_Header");
        private static string MAL_ApiKey = Environment.GetEnvironmentVariable("MALApiKey");
        private static string MAL_BaseURL = Environment.GetEnvironmentVariable("MAL_BaseURL");
        private ConfigurationClient configClient = new ConfigurationClient(Environment.GetEnvironmentVariable("AppConfiguration-ConnectionString"));

        private int currentPage;

        #region DI

        private readonly ILogger _logger;
        private readonly IMAL_Integration _malIntegration;
        private readonly ISyncToNotionRepository _syncToNotionRepository;

        #endregion

        #endregion

        public SyncMalAnimeStatus(ILoggerFactory loggerFactory, ISyncToNotionRepository syncToNotionRepository, IMAL_Integration malIntegration)
        {
            _logger = loggerFactory.CreateLogger<SyncMalAnimeStatus>();
            _syncToNotionRepository = syncToNotionRepository;
            _malIntegration = malIntegration;
        }

        [Function("SyncMalAnimeStatus")]
        public async Task Run([TimerTrigger("0 30 3 * * *"
            ,
            #if DEBUG
                RunOnStartup= true
            #endif
            )] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            #region Delete

            var toDelete = await _syncToNotionRepository.GetMalListToUpdate("Delete");

            foreach(var item in toDelete)
            {
                try
                {
                    await _malIntegration.DeleteListStatus(MAL_Header, MAL_ApiKey, BuildUrl(item.AnimeShow.MalId));
                    await _syncToNotionRepository.SetMalListSynced(item.Id);

                    _logger.LogInformation($"Mal List item {item.AnimeShow.NameEnglish} - {item.AnimeShow.MalId}: deleted");
                }
                catch (Exception ex)
                {
                    await _syncToNotionRepository.SetMalListError(item, ex.Message);
                    _logger.LogInformation($"Mal List item {item.AnimeShow.NameEnglish} - {item.AnimeShow.MalId}: error while deleting");
                }
            }

            #endregion

            #region Update

            var toUpdate = await _syncToNotionRepository.GetMalListToUpdate("Add", "Edit");

            foreach (var item in toUpdate)
            {
                try
                {
                    MAL_AnimeUpdateStatus malUpdate = Mapping.Mapper.Map<MAL_AnimeUpdateStatus>(item.AnimeShow);
                    await _malIntegration.UpdateListStatus(MAL_Header, MAL_ApiKey, BuildUrl(malUpdate.anime_id), malUpdate);
                    await _syncToNotionRepository.SetMalListSynced(item.Id);

                    _logger.LogInformation($"Mal List item {item.AnimeShow.NameEnglish} - {item.AnimeShow.MalId}: updated");
                }
                catch (Exception ex)
                {
                    await _syncToNotionRepository.SetMalListError(item, ex.Message);
                    _logger.LogInformation($"Mal List item {item.AnimeShow.NameEnglish} - {item.AnimeShow.MalId}: error while updating");
                }
            }

            #endregion

            #region Fix Errors

            var toSync = await _syncToNotionRepository.GetMalSyncErrors();

            foreach(var item in toSync)
            {
                try
                {
                    MAL_AnimeUpdateStatus malUpdate = Mapping.Mapper.Map<MAL_AnimeUpdateStatus>(item.AnimeShow);

                    if(item.Action == "Delete")
                        await _malIntegration.DeleteListStatus(MAL_Header, MAL_ApiKey, BuildUrl(item.MalId));
                    else
                        await _malIntegration.UpdateListStatus(MAL_Header, MAL_ApiKey, BuildUrl(malUpdate.anime_id), malUpdate);

                    await _syncToNotionRepository.SetMalListSynced(item.Id, malListError: item.Id);

                    _logger.LogInformation($"Resyncing attempt for Mal List item {item.AnimeShow.NameEnglish} - {item.AnimeShow.MalId}: sync error fixed");
                }
                catch (Exception ex)
                {                    
                    _logger.LogInformation($"Resyncing attempt for Mal List item {item.AnimeShow.NameEnglish} - {item.AnimeShow.MalId}: error persisted");
                }
            }

            #endregion
        }

        private string BuildUrl(int id)
        {
            return $"{MAL_BaseURL}anime/{id}/my_list_status";
        }
    }
}
