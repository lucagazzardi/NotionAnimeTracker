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
        /// Retrieve seasonal anime
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        public async Task<List<INT_AnimeShowBase>> GetSeasonalAnimeShow(int year, Season season)
        {
            // Gets seasonal from Jikan and takes 12 items
            List<INT_AnimeShowBase> seasonEntries = Mapping.Mapper.ProjectTo<INT_AnimeShowBase>(
                    (await _jikan.GetSeasonAsync(year, season)).Data.AsQueryable().Take(12)
                )
                .ToList();

            // Retrieves only the items existing in the DB
            var animeShowPartial = _animeRepository.GetAllByIds(seasonEntries.Select(x => x.MalId).ToList()).Select(x => new { x.Id, x.MalId, x.Status });

            // Sets the basic DB info 
            foreach (var anime in animeShowPartial)
            {
                seasonEntries.Single(x => x.MalId == anime.MalId).Info = new INT_AnimeShowPersonal() { Id = anime.Id, Status = anime.Status };
            }

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

            // Retrieves only the items existing in the DB
            var animeShowPartial = _animeRepository.GetAllByIds(foundEntries.Select(x => x.MalId).ToList()).Select(x => new { x.Id, x.MalId, x.Status });

            // Sets the basic DB info 
            foreach (var anime in animeShowPartial)
            {
                foundEntries.Single(x => x.MalId == anime.MalId).Info = new INT_AnimeShowPersonal() { Id = anime.Id, Status = anime.Status };
            }

            return foundEntries;
        }

        public async Task<INT_AnimeShowFull> SearchAnimeById(int malId)
        {
            var anime = _jikan.GetAnimeAsync(malId);
            var relations = StaticHttpClient.MALHttpClient.GetStringAsync(BuildMALRelationsUrl(malId));
            await Task.WhenAll(anime, relations);

            var malRelations = JsonConvert.DeserializeObject<MAL_AnimeShowRelations>(await relations);

            INT_AnimeShowFull found = Mapping.Mapper.Map<INT_AnimeShowFull>((await anime).Data);
            found.Relations = Mapping.Mapper.ProjectTo<INT_AnimeShowRelation>(malRelations.related_anime.AsQueryable()).ToList();

            return found;
        }


        private string BuildMALRelationsUrl(int malId)
        {
            return string.Concat(_configuration["MAL_ApiConfig:MAL_BaseURL"], "anime/", malId.ToString(), "?fields=related_anime");
        }
    }
}
