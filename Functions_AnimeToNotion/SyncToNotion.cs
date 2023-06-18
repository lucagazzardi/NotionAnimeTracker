using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Notion.Client;

namespace Functions_AnimeToNotion
{
    public class SyncToNotion
    {
        #region Configuration

        private static string Notion_Auth_Token = Environment.GetEnvironmentVariable("NotionSecretKey");
        private string DataBaseId = Environment.GetEnvironmentVariable("Notion-DataBaseId");
        private readonly ILogger _logger;

        private NotionClient NotionClient;

        #endregion

        public SyncToNotion(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SyncToNotion>();
        }

        [Function("SyncToNotion")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            #region Notion Client

            NotionClient = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = Notion_Auth_Token
            });

            #endregion



            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }

        #region Private and Mapping



        #endregion
    }
}
