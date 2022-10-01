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
using System.Runtime.CompilerServices;
using Business_AnimeToNotion.Main_Integration.Exceptions;

namespace Business_AnimeToNotion.Main_Integration
{
    public class MAL_Integration : IMAL_Integration
    {
        #region fields

        private readonly IConfiguration Configuration;

        #endregion

        internal class MAL_ExceptionMessages
        {
            public const string SearchByNameException = "Error: couldn't find shows with input \"[searchTerm]\"";
            public const string SearchByIdException = "Error: show with id \"[id]\" not found";
        }

        public MAL_Integration(IConfiguration configuration)
        {
            Configuration = configuration;

            // Add MAL Secret Api Key as header
            if(!StaticHttpClient.MALHttpClient.DefaultRequestHeaders.Contains(Configuration["MAL_ApiConfig:MAL_Header"]))
                StaticHttpClient.MALHttpClient.DefaultRequestHeaders.Add(Configuration["MAL_ApiConfig:MAL_Header"], Configuration["MAL-ApiKey"]);
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

        #region Private

        private string BuildMALUrl_SearchByName(string searchTerm)
        {
            string result = Configuration["MAL_ApiConfig:MAL_BaseURL"];

            // Join with query string space and substring to 80 maximum chars
            searchTerm = string.Join("%20", searchTerm.Split(" "));
            searchTerm = searchTerm.Length > 80 ? searchTerm.Substring(0, 79) : searchTerm;


            // QueryString build
            result += Configuration["MAL_ApiConfig:MAL_SearchAnime"];
            result += "?";
            result += Configuration["MAL_ApiConfig:MAL_SearchByName"];
            result += searchTerm;
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

        private MAL_AnimeModel DeserializeResponse(string httpResponse)
        {
            MAL_AnimeModel result = JsonSerializer.Deserialize<MAL_AnimeModel>(httpResponse);
            result.genresJoined = string.Join(", ", result.genres.OrderByDescending(x => x.id).Select(x => x.name).Take(4).ToList());
            result.studiosJoined = string.Join(", ", result.studios.OrderByDescending(x => x.id).Select(x => x.name).Take(4).ToList());
            return result;
        }

        private List<MAL_AnimeModel> DeserializeResponseList(string httpResponse)
        {
            MAL_ApiResponseModel response = JsonSerializer.Deserialize<MAL_ApiResponseModel>(httpResponse);

            foreach(var show in response.data)
            {
                show.node.genresJoined = string.Join(", ", show.node.genres.OrderByDescending(x => x.id).Select(x => x.name).Take(4).ToList());
                show.node.studiosJoined = string.Join(", ", show.node.studios.OrderByDescending(x => x.id).Select(x => x.name).Take(4).ToList());
            }

            return response.data.Select(x => x.node).ToList();
        }

        #endregion

    }
}
