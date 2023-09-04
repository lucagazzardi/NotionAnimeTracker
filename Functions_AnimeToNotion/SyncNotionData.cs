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

            #region DELETE

            var toDelete = await _syncToNotionRepository.GetNotionSync("Delete");
            var delToRemove = new List<NotionSync>();

            foreach (var entry in toDelete)
            {
                try
                {
                    if (entry.NotionPageId != null)
                        await NotionClient.Pages.UpdateAsync(entry.NotionPageId, new PagesUpdateParameters() { Archived = true });

                    _logger.LogInformation($"Notion page {entry.NotionPageId}: deleted");
                }
                catch (Exception ex)
                {
                    delToRemove.Add(entry);
                    await _syncToNotionRepository.SetError(entry, ex.Message);

                    _logger.LogError($"Notion page {entry.NotionPageId}: error while deleting");
                }
            }

            toDelete = toDelete.Except(delToRemove).ToList();
            if (toDelete.Count > 0)
                await _syncToNotionRepository.SetDeleted(toDelete);

            #endregion

            #region ADD

            var toAdd = await _syncToNotionRepository.GetNotionSync("Add");
            var addToRemove = new List<NotionSync>();
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

                    _logger.LogInformation($"{entry.AnimeShow.NameEnglish} - {entry.AnimeShow.MalId}: added");
                }
                catch(Exception ex)
                {
                    addToRemove.Add(entry);
                    await _syncToNotionRepository.SetError(entry, ex.Message);

                    _logger.LogError($"{entry.AnimeShow.NameEnglish} - {entry.AnimeShow.MalId}: error while adding");
                }
            }

            toAdd = toAdd.Except(addToRemove).ToList();
            if(toAdd.Count > 0)
                await _syncToNotionRepository.SetAdded(toAdd, notionPages);

            #endregion

            #region EDIT

            var toEdit = await _syncToNotionRepository.GetNotionSync("Edit");
            var editToRemove = new List<NotionSync>();

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

                    _logger.LogInformation($"{entry.AnimeShow.NameEnglish} - {entry.AnimeShow.MalId}: updated");
                }
                catch(Exception ex)
                {
                    editToRemove.Add(entry);
                    await _syncToNotionRepository.SetError(entry, ex.Message);

                    _logger.LogError($"{entry.AnimeShow.NameEnglish} - {entry.AnimeShow.MalId}: error while updating");
                }
            }

            toEdit = toEdit.Except(editToRemove).ToList();
            if(toEdit.Count > 0)
                await _syncToNotionRepository.SetEdited(toEdit);

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
