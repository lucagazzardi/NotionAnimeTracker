using Business_AnimeToNotion.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Business_AnimeToNotion.HTTPClient;
using Microsoft.Extensions.Configuration;
using Business_AnimeToNotion.Main_Integration.Interfaces;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace Business_AnimeToNotion.Main_Integration
{
    public class MAL_Integration : IMAL_Integration
    {
        #region fields

        private readonly IConfiguration Configuration;

        #endregion

        public MAL_Integration(IConfiguration configuration)
        {
            Configuration = configuration;

            // Add MAL Secret Api Key as header
            StaticHttpClient.MALHttpClient.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", Configuration["MAL_ApiConfig:MAL_ApiKey"]);
        }

        public async Task<List<MAL_AnimeModel>> MAL_SearchAnimeByNameAsync(string searchTerm)
        {
            var httpResponse = await StaticHttpClient.MALHttpClient.GetStringAsync(BuildMALUrl_SearchByName(searchTerm));
            MAL_ApiResponseModel malResponse = JsonSerializer.Deserialize<MAL_ApiResponseModel>(httpResponse);

            List<MAL_AnimeModel> result = malResponse.data.Select(x => x.node).ToList();

            return result;
        }

        public async Task<MAL_AnimeModel> MAL_SearchAnimeByIdAsync(int id)
        {
            var httpResponse = await StaticHttpClient.MALHttpClient.GetStringAsync(BuildMALUrl_SearchById(id));
            MAL_AnimeModel result = JsonSerializer.Deserialize<MAL_AnimeModel>(httpResponse);

            return result;
        }

        #region Private

        private string BuildMALUrl_SearchByName(string searchTerm)
        {
            string result = Configuration["MAL_ApiConfig:MAL_BaseURL"];

            // QueryString build
            result += Configuration["MAL_ApiConfig:MAL_SearchAnime"];
            result += "?";
            result += Configuration["MAL_ApiConfig:MAL_SearchByName"];
            result += string.Join("%20", searchTerm.Split(" "));
            result += "&";
            result += Configuration["MAL_ApiConfig:MAL_NotionNeededFields"];

            return result;
        }

        private string BuildMALUrl_SearchById(int id)
        {
            string result = Configuration["MAL_ApiConfig:MAL_BaseURL"];

            result += Configuration["MAL_ApiConfig:MAL_SearchAnime"];
            result += "/";
            result += id;
            result += "?";
            result += Configuration["MAL_ApiConfig:MAL_NotionNeededFields"];

            return result;
        }

        #endregion

    }
}
