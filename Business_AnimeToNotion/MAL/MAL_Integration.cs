using Business_AnimeToNotion.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace Business_AnimeToNotion.MAL
{
    public class MAL_Exception : Exception
    {
        public MAL_Exception(string customMessage) : base(customMessage) { }
    }

    internal class MAL_ExceptionMessages
    {
        public const string SearchByNameException = "Error: couldn't find shows with input \"[searchTerm]\"";
        public const string SearchByIdException = "Error: show with id \"[id]\" not found";
    }

    public class MAL_Integration
    {
        #region DI

        private readonly IConfiguration _configuration;

        #endregion

        public MAL_Integration(
                IConfiguration configuration
            )
        {
            #region DI

            _configuration = configuration;

            #endregion            

            // Add MAL Secret Api Key as header
            if (!StaticHttpClient.MALHttpClient.DefaultRequestHeaders.Contains(_configuration["MAL_ApiConfig:MAL_Header"]))
                StaticHttpClient.MALHttpClient.DefaultRequestHeaders.Add(_configuration["MAL_ApiConfig:MAL_Header"], _configuration["MAL-ApiKey"]);
        }

        public async Task<MAL_AnimeModel> MAL_SearchAnimeByIdAsync(int id)
        {
            MAL_AnimeModel result = null;

            try
            {
                var httpResponse = await StaticHttpClient.MALHttpClient.GetStringAsync(BuildMALUrl_SearchById(id));
                result = DeserializeResponse(httpResponse);
            }
            catch
            {
                throw new MAL_Exception(MAL_ExceptionMessages.SearchByIdException.Replace("[id]", id.ToString()));
            }

            return result;
        }

        public async Task<List<MAL_AnimeModel>> MAL_SearchAnimeByNameAsync(string searchTerm)
        {
            List<MAL_AnimeModel> result = new List<MAL_AnimeModel>();
            try
            {
                var httpResponse = await StaticHttpClient.MALHttpClient.GetStringAsync(BuildMALUrl_SearchByName(searchTerm));
                result = DeserializeResponseList(httpResponse);
            }
            catch
            {
                throw new MAL_Exception(MAL_ExceptionMessages.SearchByNameException.Replace("[searchTerm]", searchTerm));
            }

            return result;
        }

        #region Private

        private string BuildMALUrl_SearchByName(string searchTerm)
        {
            string result = _configuration["MAL_ApiConfig:MAL_BaseURL"];

            // Join with query string space and substring to 80 maximum chars
            searchTerm = string.Join("%20", searchTerm.Split(" "));
            searchTerm = searchTerm.Length > 80 ? searchTerm.Substring(0, 79) : searchTerm;


            // QueryString build
            result += _configuration["MAL_ApiConfig:MAL_SearchAnime"];
            result += "?";
            result += _configuration["MAL_ApiConfig:MAL_SearchByName"];
            result += searchTerm;
            result += "&";
            result += _configuration["MAL_ApiConfig:MAL_NotionNeededFields"];
            result += "&";
            result += _configuration["MAL_ApiConfig:MAL_NSFW"];
            result += "&limit=6";

            return result;
        }

        private string BuildMALUrl_SearchById(int id)
        {
            string result = _configuration["MAL_ApiConfig:MAL_BaseURL"];

            result += _configuration["MAL_ApiConfig:MAL_SearchAnime"];
            result += "/";
            result += id;
            result += "?";
            result += _configuration["MAL_ApiConfig:MAL_NotionNeededFields"];
            result += "&";
            result += _configuration["MAL_NSFW"];

            return result;
        }

        private MAL_AnimeModel DeserializeResponse(string httpResponse)
        {
            MAL_AnimeModel result = JsonSerializer.Deserialize<MAL_AnimeModel>(httpResponse);
            return result;
        }

        private List<MAL_AnimeModel> DeserializeResponseList(string httpResponse)
        {
            MAL_ApiResponseModel response = JsonSerializer.Deserialize<MAL_ApiResponseModel>(httpResponse);

            return response.data.Select(x => x.node).ToList();
        }

        #endregion
    }
}
