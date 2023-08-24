using Business_AnimeToNotion.Mapper.Config;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Notion.Client;

namespace Functions_AnimeToNotion
{
    public class SyncNotionData
    {
        private readonly ILogger _logger;
        private readonly ISyncToNotionRepository _syncToNotionRepository;

        private static string Notion_Auth_Token = Environment.GetEnvironmentVariable("NotionSecretKey");
        private string DataBaseId = Environment.GetEnvironmentVariable("Notion-DataBaseId");
        private NotionClient NotionClient;

        public SyncNotionData(ILoggerFactory loggerFactory, ISyncToNotionRepository syncToNotionRepository)
        {
            _logger = loggerFactory.CreateLogger<SyncNotionData>();
            _syncToNotionRepository = syncToNotionRepository;
        }

        [Function("SyncNotionData")]
        public async Task Run([TimerTrigger("0 0 4 * * *",
            #if DEBUG
                RunOnStartup= true
            #endif
            )] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            #region Notion Client

            NotionClient = NotionClientFactory.Create(new Notion.Client.ClientOptions
            {
                AuthToken = Notion_Auth_Token
            });

            #endregion

            #region ADD

            var toAdd = await _syncToNotionRepository.GetNotionSync("Add");
            Dictionary<int, string> notionPages = new Dictionary<int, string>();
            

            foreach (var entry in toAdd)
            {
                try
                {
                    var properties = Mapping.Mapper.Map<Dictionary<string, PropertyValue>>(entry.AnimeShow);
                    await CheckAndRetrieveCompletedYear(entry.AnimeShow.AnimeShowProgress.CompletedYear, properties);

                    var added = await NotionClient.Pages.CreateAsync(new PagesCreateParameters()
                    {
                        Parent = new DatabaseParentInput() { DatabaseId = DataBaseId },
                        Properties = properties
                    });

                    notionPages.Add(entry.Id, added.Id);
                }
                catch(Exception ex)
                {
                    toAdd.Remove(entry);
                    await _syncToNotionRepository.SetError(entry, ex.Message);
                }
            }

            if(toAdd.Count > 0)
                await _syncToNotionRepository.SetAdded(toAdd, notionPages);

            #endregion

            #region EDIT

            var toEdit = await _syncToNotionRepository.GetNotionSync("Edit");

            foreach (var entry in toEdit)
            {
                try
                {
                    var properties = Mapping.Mapper.Map<Dictionary<string, PropertyValue>>(entry.AnimeShow);
                    await CheckAndRetrieveCompletedYear(entry.AnimeShow.AnimeShowProgress.CompletedYear, properties);

                    await NotionClient.Pages.UpdateAsync(entry.NotionPageId, new PagesUpdateParameters()
                    {
                        Properties = properties
                    });
                }
                catch(Exception ex)
                {
                    toEdit.Remove(entry);
                    await _syncToNotionRepository.SetError(entry, ex.Message);
                }
            }

            if(toEdit.Count > 0)
                await _syncToNotionRepository.SetEdited(toEdit);

            #endregion

            #region DELETE

            var toDelete = await _syncToNotionRepository.GetNotionSync("Delete");

            foreach (var entry in toDelete)
            {
                try
                {
                    await NotionClient.Pages.UpdateAsync(entry.NotionPageId, new PagesUpdateParameters() { Archived = true });
                }
                catch(Exception ex)
                {
                    toDelete.Remove(entry);
                    await _syncToNotionRepository.SetError(entry, ex.Message);
                }
            }

            if(toDelete.Count > 0)
                await _syncToNotionRepository.SetDeleted(toDelete);

            #endregion
        }

        private async Task<Dictionary<string, PropertyValue>> CheckAndRetrieveCompletedYear(int? year, Dictionary<string, PropertyValue> properties)
        {
            if(year != null)
            {
                properties.Add("Completed Year", Mapping.Mapper.Map<RelationPropertyValue>(await _syncToNotionRepository.GetYear(year.Value)));
            }

            return properties;
        }
    }
}
