using Azure.Data.AppConfiguration;
using Business_AnimeToNotion.Integrations.Internal;
using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.MAL;
using Data_AnimeToNotion.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static Grpc.Core.Metadata;

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
        private readonly IAnimeShowRepository _animeRepository;
        private readonly IInternal_Integration _internalIntegration;
        private readonly IMAL_Integration _malIntegration;
        private readonly ISyncToNotionRepository _syncToNotionRepository;

        #endregion

        #endregion

        public SyncMalAnimeStatus(ILoggerFactory loggerFactory, ISyncToNotionRepository syncToNotionRepository)
        {
            _logger = loggerFactory.CreateLogger<SyncMalAnimeStatus>();
            _syncToNotionRepository = syncToNotionRepository;
        }

        [Function("SyncMalAnimeStatus")]
        public async Task Run([TimerTrigger("0 30 3 * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            var toUpdate = await _syncToNotionRepository.GetMalListToUpdate("Add", "Edit");

            foreach (var item in toUpdate)
            {
                try
                {
                    MAL_AnimeUpdateStatus malUpdate = Mapping.Mapper.Map<MAL_AnimeUpdateStatus>(item);
                    await _malIntegration.UpdateListStatus(MAL_Header, MAL_ApiKey, BuildUrl(malUpdate.anime_id), malUpdate);
                    await _syncToNotionRepository.SetMalListSynced(item.Id);

                    _logger.LogInformation($"Mal List item {item.AnimeShow.NameEnglish} - {item.AnimeShow.MalId}: updated");
                }
                catch (Exception ex)
                {
                    await _syncToNotionRepository.SetMalListError(item.Id, ex.Message);
                    _logger.LogInformation($"Mal List item {item.AnimeShow.NameEnglish} - {item.AnimeShow.MalId}: error while updating");
                }
            }
        }

        private string BuildUrl(int id)
        {
            return $"{MAL_BaseURL}anime/{id}/my_list_status";
        }
    }
}
