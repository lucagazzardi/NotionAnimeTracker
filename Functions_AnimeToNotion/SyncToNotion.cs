using System.Net;
using Business_AnimeToNotion.Model.Notion;
using Business_AnimeToNotion.Model.Notion.Base;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Notion.Client;
using Business_AnimeToNotion.Mapper.Config;
using Newtonsoft.Json;
using Data_AnimeToNotion.Repository;
using Data_AnimeToNotion.DataModel;

namespace Functions_AnimeToNotion
{
    public class SyncToNotion
    {
        #region Configuration

        private static string Notion_Auth_Token = Environment.GetEnvironmentVariable("NotionSecretKey");
        private string DataBaseId = Environment.GetEnvironmentVariable("Notion-DataBaseId");
        private readonly ILogger _logger;

        private readonly ISyncToNotionRepository _syncToNotionRepository;

        private NotionClient NotionClient;

        #endregion

        public SyncToNotion(ILoggerFactory loggerFactory, ISyncToNotionRepository syncToNotionRepository)
        {
            _logger = loggerFactory.CreateLogger<SyncToNotion>();
            _syncToNotionRepository = syncToNotionRepository;
        }

        [Function("SyncToNotion")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            #region Notion Client

            NotionClient = NotionClientFactory.Create(new Notion.Client.ClientOptions
            {
                AuthToken = Notion_Auth_Token
            });

            #endregion

            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            try
            {
                NotionSync notionSync = JsonConvert.DeserializeObject<NotionSync>(requestBody);

                switch (notionSync.Type)
                {
                    case OperationType.Add:
                        await Add(JsonConvert.DeserializeObject<NotionSyncAdd>(requestBody));
                        break;

                    case OperationType.Edit:
                        await Edit(JsonConvert.DeserializeObject<NotionSyncEdit>(requestBody));
                        break;

                    case OperationType.Remove:
                        await Remove(JsonConvert.DeserializeObject<NotionSyncRemove>(requestBody));
                        break;
                }                
            }
            catch(Exception ex)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                badResponse.WriteString(ex.Message);
                badResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                return badResponse;
            }            

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            return response;
        }

        #region Private and Mapping

        private async Task Add(NotionSyncAdd add)
        {
            try
            {
                var toAdd = Mapping.Mapper.Map<Dictionary<string, PropertyValue>>(add.NotionAddObject);
                var notionPage = await NotionClient.Pages.CreateAsync(new PagesCreateParameters()
                {
                    Properties = toAdd,
                    Parent = new DatabaseParentInput() { DatabaseId = DataBaseId }
                });
                await _syncToNotionRepository.UpdateNotionPageId(add.NotionAddObject.MalId, notionPage.Id);

                await AddLog(add.NotionAddObject, "OK");
            }
            catch
            {
                await AddLog(add.NotionAddObject, "KO");
                throw;
            }            
        }

        private async Task Edit(NotionSyncEdit edit)
        {
            try
            {
                var toEdit = Mapping.Mapper.Map<Dictionary<string, PropertyValue>>(edit.NotionEditObject);
                await NotionClient.Pages.UpdateAsync(edit.NotionEditObject.NotionPageId, new PagesUpdateParameters() { Properties = toEdit });

                await EditLog(edit, "OK");
            }
            catch
            {
                await EditLog(edit, "KO");
                throw;
            }            
        }

        private async Task Remove(NotionSyncRemove remove)
        {
            try
            {
                await NotionClient.Pages.UpdateAsync(remove.NotionRemoveObject.NotionPageId, new PagesUpdateParameters() { Archived = true });

                await RemoveLog(remove, "OK");
            }
            catch
            {
                await RemoveLog(remove, "KO");
                throw;
            }
        }

        private async Task AddLog(NotionAddObject add, string result)
        {
            await _syncToNotionRepository.Add(new SyncToNotionLog()
            {
                Title = add.NameEnglish,
                MalId = add.MalId,
                Type = "Add",
                Result = result
            });
        }

        private async Task EditLog(NotionSyncEdit edit, string result)
        {
            await _syncToNotionRepository.Add(new SyncToNotionLog()
            {
                Title = edit.Title,
                MalId = edit.MalId,
                Type = "Edit",
                Result = result
            });
        }

        private async Task RemoveLog(NotionSyncRemove remove, string result)
        {
            await _syncToNotionRepository.Add(new SyncToNotionLog()
            {
                Title = remove.Title,
                MalId = remove.MalId,
                Type = "Remove",
                Result = result
            });
        }

        #endregion
    }
}
