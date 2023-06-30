using Business_AnimeToNotion.Functions.Static;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.MAL;
using Business_AnimeToNotion.Model.MAL.MAL_BasicObjects;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Repository;
using JikanDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Notion.Client;
using System.Runtime.CompilerServices;

namespace Business_AnimeToNotion.Integrations.MAL
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

    public class MAL_Integration : IMAL_Integration
    {
        private readonly IJikan _jikan;

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

            _jikan = new Jikan();

            // Add MAL Secret Api Key as header
            if (!StaticHttpClient.MALHttpClient.DefaultRequestHeaders.Contains(_configuration["MAL_ApiConfig:MAL_Header"]))
                StaticHttpClient.MALHttpClient.DefaultRequestHeaders.Add(_configuration["MAL_ApiConfig:MAL_Header"], _configuration["MAL-ApiKey"]);
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
            List<INT_AnimeShowBase> foundEntries = Mapping.Mapper.ProjectTo<INT_AnimeShowBase>(
                    (await _jikan.SearchAnimeAsync(searchTerm)).Data.AsQueryable()
                )
                .ToList();

            CheckSavedAnimeShow(foundEntries);

            return foundEntries;
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

            return found;
        }        

        /// <summary>
        /// Retrieve relations for an anime
        /// </summary>
        /// <param name="malId"></param>
        /// <returns></returns>
        public async Task<MAL_AnimeShowRelations> GetRelationsFromMAL(int malId)
        {
            string relations = await StaticHttpClient.MALHttpClient.GetStringAsync(BuildMALRelationsUrl(malId));
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

        private string BuildMALRelationsUrl(int malId)
        {
            return string.Concat(_configuration["MAL_ApiConfig:MAL_BaseURL"], "anime/", malId.ToString(), "?fields=related_anime");
        }        

        #endregion
    }
}
