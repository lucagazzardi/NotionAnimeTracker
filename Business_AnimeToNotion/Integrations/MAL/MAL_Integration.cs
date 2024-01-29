using Business_AnimeToNotion.Functions.Static;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Auth;
using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.MAL;
using Data_AnimeToNotion.Repository;
using JikanDotNet;
using JikanDotNet.Config;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;

namespace Business_AnimeToNotion.Integrations.MAL
{  
    public class MAL_Integration : IMAL_Integration
    {
        private readonly IJikan _jikan;
        private readonly HttpClient _malHttpClient;

        #region DI

        private readonly IConfiguration _configuration;
        private readonly IAnimeShowRepository _animeRepository;

        #endregion

        public MAL_Integration(
                IConfiguration configuration,
                IAnimeShowRepository animeRepository
            )
        {
            #region DI

            _configuration = configuration;
            _animeRepository = animeRepository;

            #endregion

            var config = new JikanClientConfiguration()
            {
                LimiterConfigurations = TaskLimiterConfiguration.None
            };
            _jikan = new Jikan(config);

            _malHttpClient = new HttpClient();            
        }

        /// <summary>
        /// Retrieve current season anime
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        public async Task<List<INT_AnimeShowBase>> GetCurrentSeasonAnimeShow()
        {
            // Gets seasonal from Jikan and takes 12 items
            List<INT_AnimeShowBase> seasonEntries = Mapping.Mapper.ProjectTo<INT_AnimeShowBase>(
                    (await _jikan.GetCurrentSeasonAsync()).Data.AsQueryable().Take(12)
                )
                .ToList();

            CheckSavedAnimeShow(seasonEntries);

            return seasonEntries;
        }

        /// <summary>
        /// Retrieve upcoming season anime
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        public async Task<List<INT_AnimeShowBase>> GetUpcomingSeasonAnimeShow()
        {
            // Calculate upcoming season
            Season season = Common_Utilities.RetrieveUpcomingSeason();

            // Gets seasonal from Jikan and takes 12 items
            List<INT_AnimeShowBase> seasonEntries = Mapping.Mapper.ProjectTo<INT_AnimeShowBase>(
                    (await _jikan.GetSeasonAsync(DateTime.Now.Year, season)).Data.AsQueryable().Take(12)
                )
                .ToList();

            CheckSavedAnimeShow(seasonEntries);

            return seasonEntries;
        }

        /// <summary>
        /// Search anime based on the term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<List<INT_AnimeShowBase>> SearchAnimeByName(string searchTerm)
        {
            AnimeSearchConfig config = new AnimeSearchConfig()
            {
                Query = searchTerm,
                PageSize = 20
            };

            try
            {
                List<INT_AnimeShowBase> foundEntries = Mapping.Mapper.ProjectTo<INT_AnimeShowBase>(
                        (await _jikan.SearchAnimeAsync(config)).Data.AsQueryable()
                    )
                    .ToList();

                CheckSavedAnimeShow(foundEntries);

                return foundEntries;
            }
            catch
            {
                return new List<INT_AnimeShowBase>();
            }            
        }

        /// <summary>
        /// Retrieve the anime from MAL by Id
        /// </summary>
        /// <param name="malId"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowFull> GetAnimeById(int malId)
        {
            var anime = _jikan.GetAnimeAsync(malId);
            var malRelations = GetRelationsFromMAL(malId);
            
            await Task.WhenAll(anime, malRelations);

            INT_AnimeShowFull found = Mapping.Mapper.Map<INT_AnimeShowFull>((await anime).Data);
            found.Relations = Mapping.Mapper.ProjectTo<INT_AnimeShowRelation>((await malRelations).related_anime.AsQueryable()).ToList();

            await CheckSavedAnimeShow(found);

            return found;
        }

        /// <summary>
        /// Retrieves anime from Mal
        /// </summary>
        /// <param name="header"></param>
        /// <param name="key"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowFull> GetAnimeById(string header, string key, string url)
        {
            var malOb = await GetAnimeFromMal(header, key, url);
            return Mapping.Mapper.Map<INT_AnimeShowFull>(malOb);
        }

        /// <summary>
        /// Add or update status on Mal
        /// </summary>
        /// <param name="header"></param>
        /// <param name="key"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<MAL_AnimeUpdateStatus> UpdateListStatus(string token, string url, MAL_AnimeUpdateStatus item)
        {
            Dictionary<string, string> reqData = new Dictionary<string, string>
            {
                { "status", item.status },
                { "start_date", item.start_date },
                { "finish_date", item.finish_date },
                { "num_watched_episodes", item.num_watched_episodes.ToString() },
                { "score", item.score?.ToString() }
            };

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);

            var req = new HttpRequestMessage(HttpMethod.Patch, url) { Content = new FormUrlEncodedContent(reqData) };
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.SendAsync(req);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MAL_AnimeUpdateStatus>(result);
        }

        /// <summary>
        /// Delete status on Mal
        /// </summary>
        /// <param name="token"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task DeleteListStatus(string token, string url)
        {
            SetHttpHeader(token);
            var response = await _malHttpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Retrieves relations for an anime
        /// </summary>
        /// <param name="malId"></param>
        /// <returns></returns>
        public async Task<MAL_AnimeShowRelations> GetRelationsFromMAL(int malId)
        {
            SetHttpHeader(_configuration["MAL_ApiConfig:MAL_Header"], _configuration["MAL-ApiKey"]);
            string relations = await _malHttpClient.GetStringAsync(BuildMALRelationsUrl(malId));
            return JsonConvert.DeserializeObject<MAL_AnimeShowRelations>(relations);
        }

        #region Private

        private List<INT_AnimeShowBase> CheckSavedAnimeShow(List<INT_AnimeShowBase> animeShows)
        {
            // Retrieves only the items existing in the DB
            var animeShowPartial = _animeRepository.GetAllByIds(animeShows.Select(x => x.MalId).ToList()).Select(x => new { x.Id, x.MalId, x.Status });

            // Sets the basic DB info 
            foreach (var anime in animeShowPartial)
            {
                animeShows.Single(x => x.MalId == anime.MalId).Info = new INT_AnimeShowPersonal() { Id = anime.Id, Status = anime.Status };
            }

            return animeShows;
        }

        private async Task<INT_AnimeShowFull> CheckSavedAnimeShow(INT_AnimeShowFull animeShow)
        {
            // Retrieves only the items existing in the DB
            var anime = await _animeRepository.GetByMalId(animeShow.MalId);

            if (anime == null)
                return null;

            animeShow.Info = new INT_AnimeShowPersonal() { Id = anime.Id, Status = anime.Status };

            return animeShow;
        }

        private async Task<MAL_AnimeShowRaw> GetAnimeFromMal(string header, string key, string url)
        {
            SetHttpHeader(header, key);
            var response = await _malHttpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<MAL_AnimeShowRaw>(response);
        }

        private string BuildMALRelationsUrl(int malId)
        {
            return string.Concat(_configuration["MAL_ApiConfig:MAL_BaseURL"], "anime/", malId.ToString(), "?fields=related_anime");
        }

        private void SetHttpHeader(string header, string key)
        { 
            // Add MAL Secret Api Key as header
            if (!_malHttpClient.DefaultRequestHeaders.Contains(header))
                _malHttpClient.DefaultRequestHeaders.Add(header, key);
        }

        private void SetHttpHeader(string token)
        {
            // Add Access Token as header
            if (!_malHttpClient.DefaultRequestHeaders.Contains("Authorization"))
                _malHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        #endregion
    }
}
