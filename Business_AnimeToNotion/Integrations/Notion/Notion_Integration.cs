using Business_AnimeToNotion.Model.Notion.Base;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text;

namespace Business_AnimeToNotion.Integrations.Notion
{
    public class Notion_Integration : INotion_Integration
    {
        private readonly IConfiguration _configuration;

        private HttpClient _httpClient;

        public Notion_Integration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendSyncToNotion(NotionSync notionSync)
        {
            _ = Task.Run(async () =>
                {
                    if (_httpClient == null)
                        _httpClient = new HttpClient();

                    string json = JsonConvert.SerializeObject(notionSync);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    await _httpClient.PostAsync(_configuration["SyncToNotion-FunctionURI"], data);
                }
            );
        }
    }
}
