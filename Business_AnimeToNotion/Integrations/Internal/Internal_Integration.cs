using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.History;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Mixed;
using Business_AnimeToNotion.Model.Pagination;
using Business_AnimeToNotion.Model.Query;
using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Business_AnimeToNotion.QueryLogic.PageLogic;
using Business_AnimeToNotion.QueryLogic.SortLogic;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Repository;
using JikanDotNet;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Business_AnimeToNotion.Integrations.Internal
{
    public class Internal_Integration : IInternal_Integration
    {
        private readonly IJikan _jikan;

        private readonly IAnimeShowRepository _animeRepository;
        private readonly ISyncToNotionRepository _syncToNotionRepository;
        private readonly IMAL_Integration _mal;

        public Internal_Integration(IAnimeShowRepository animeRepository, IMAL_Integration mal, ISyncToNotionRepository syncToNotionRepository)
        {
            _animeRepository = animeRepository;
            _mal = mal;
            _syncToNotionRepository = syncToNotionRepository;

            _jikan = new Jikan();
        }

        #region Single Operativity

        /// <summary>
        /// Add new anime base
        /// </summary>
        /// <param name="animeAdd"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowPersonal> AddNewAnimeBase(INT_AnimeShowBase animeAdd)
        {
            if (animeAdd.Info != null)
                return null;

            var added = await _animeRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(animeAdd),
                Mapping.Mapper.ProjectTo<Studio>(animeAdd.Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(animeAdd.Genres.AsQueryable()).ToList()
            );

            await _syncToNotionRepository.AddNotionSync(added);

            return new INT_AnimeShowPersonal() { Id = added.Id, Status = added.Status };
        }

        /// <summary>
        /// Add new anime with relations
        /// </summary>
        /// <param name="animeAdd"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowPersonal> AddNewAnimeFull(INT_AnimeShowFull animeAdd)
        {
            var added = await _animeRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(animeAdd),
                Mapping.Mapper.ProjectTo<Studio>(animeAdd.Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(animeAdd.Genres.AsQueryable()).ToList()
            );

            await _syncToNotionRepository.AddNotionSync(added);

            return new INT_AnimeShowPersonal() { Id = added.Id, Status = added.Status };
        }

        /// <summary>
        /// Get anime full for editing
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowFull> GetAnimeFull(int malId)
        {
            var animeInt = await _animeRepository.GetFull(malId);

            if (animeInt == null)
                return Mapping.Mapper.Map<INT_AnimeShowFull>(await _mal.GetAnimeById(malId));

            return Mapping.Mapper.Map<INT_AnimeShowFull>(animeInt);
        }

        /// <summary>
        /// Get anime for editing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowFull> GetAnimeForEdit(int id)
        {
            return Mapping.Mapper.Map<INT_AnimeShowFull>(await _animeRepository.GetFull(id));
        }

        /// <summary>
        /// Edit anime properties
        /// </summary>
        /// <param name="animeEdit"></param>
        /// <returns></returns>
        public async Task EditAnime(INT_AnimeShowEdit animeEdit, bool skipSync = false)
        {
            var anime = await _animeRepository.GetForEdit(animeEdit.Id.Value);

            anime.Status = animeEdit.Status;

            anime.AnimeShowProgress.PersonalScore = animeEdit.PersonalScore;
            anime.AnimeShowProgress.StartedOn = animeEdit.StartedOn;
            anime.AnimeShowProgress.FinishedOn = animeEdit.FinishedOn;
            anime.AnimeShowProgress.Notes = animeEdit.Notes;
            anime.AnimeShowProgress.CompletedYear = animeEdit.CompletedYear;

            if(!skipSync)
                await _syncToNotionRepository.SetToSyncNotion(anime.Id, "Edit");

            await _animeRepository.Save();
        }

        /// <summary>
        /// Switch anime favorite
        /// </summary>
        /// <param name="id"></param>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public async Task<bool> SetAnimeFavorite(int id, bool favorite)
        {
            await _animeRepository.SetAnimeFavorite(id, favorite);

            await _syncToNotionRepository.SetToSyncNotion(id, "Edit", malListToSync: false);
            return favorite;
        }

        /// <summary>
        /// Switch anime plan to watch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public async Task<bool> SetAnimePlanToWatch(int id, bool planToWatch)
        {
            await _animeRepository.SetPlanToWatch(id);
            return planToWatch;
        }

        /// <summary>
        /// Remove anime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveAnime(int id)
        {
            await _syncToNotionRepository.SetToSyncNotion(id, "Delete");

            await _animeRepository.RemoveInternalAnimeShow(id);            
        }

        /// <summary>
        /// Adds new anime episode record
        /// </summary>
        /// <param name="animeEpisode"></param>
        /// <returns></returns>
        public async Task<int> AddAnimeEpisode(INT_AnimeEpisode animeEpisode)
        {
            return await _animeRepository.AddEpisode(animeEpisode.AnimeShowId, animeEpisode.EpisodeNumber, animeEpisode.WatchedOn);
        }

        /// <summary>
        /// Edits an episode already watched
        /// </summary>
        /// <param name="animeEpisode"></param>
        /// <returns></returns>
        public async Task EditAnimeEpisode(INT_AnimeEpisode animeEpisode)
        {
            await _animeRepository.EditEpisode(animeEpisode.Id.Value, animeEpisode.WatchedOn);
        }

        /// <summary>
        /// Retrieves every watched episode for an anime
        /// </summary>
        /// <param name="animeShowId"></param>
        /// <returns></returns>
        public async Task<List<INT_AnimeEpisode>> GetAnimeEpisodes(int animeShowId)
        {
            return (await _animeRepository.GetAnimeEpisodes(animeShowId)).Select(x => new INT_AnimeEpisode()
            {
                Id = x.Id,
                AnimeShowId = x.AnimeShowId,
                EpisodeNumber = x.EpisodeNumber,
                WatchedOn = x.WatchedOn
            }).ToList();
        }

        /// <summary>
        /// Deletes an anime episode
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAnimeEpisode(int id)
        {
            await _animeRepository.DeleteAnimeEpisodes(id);
        }

        public async Task<AnimeEpisodesRecord> GetAnimeEpisodesRecord(int animeShowId, int malId)
        {
            // Collect watched episodes
            var watchedEpisodes = (await _animeRepository.GetAnimeEpisodes(animeShowId)).ToDictionary(x => x.EpisodeNumber, x => x);

            // Collect all the episodes of the anime
            var episodes = (await _jikan.GetAnimeEpisodesAsync(malId)).Data;


            var result = new AnimeEpisodesRecord()
            {
                AnimeShowId = animeShowId,
                Episodes = episodes.Select(x => new AnimeSingleEpisode()
                {
                    TitleEnglish = x.Title,
                    TitleJapanese = x.TitleJapanese,
                    EpisodeNumber = (int)x.MalId,
                    EpisodeId = watchedEpisodes.TryGetValue((int)x.MalId, out var watched) ? watched.Id : null,
                    WatchedOn = watched?.WatchedOn
                }).ToList()
            };

            return result;
        }

        #endregion

        #region Library Operativity

        /// <summary>
        /// Gets a page of shows based on filters, sort and page number specified
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PaginatedResponse<INT_AnimeShowFull>> LibraryQuery(FilterIn filters, SortIn? sort, PageIn page)
        {
            IQueryable<AnimeShow> data = _animeRepository.GetAsQueryable();

            FilterManager filterManager = new FilterManager(filters);
            data = filterManager.ApplyFilters(data);

            SortManager sortManager = new SortManager();
            data = sortManager.ApplySort(data, sort);

            PageManager pageManager = new PageManager();
            data = await pageManager.ApplyPaging(data, page);

            return pageManager.GeneratePaginatedResponse(await Mapping.Mapper.ProjectTo<INT_AnimeShowFull>(data).ToListAsync(), page);
        }

        #endregion

        #region History

        /// <summary>
        /// Retrieves watched animes grouped by years
        /// </summary>
        /// <returns></returns>
        public async Task<List<HistoryYear>> GetHistory()
        {
            var data = _animeRepository.GetAsQueryable().Select(x => new { x.AnimeShowProgress.CompletedYear, x.AnimeShowProgress.FinishedOn, x.MalId, x.NameEnglish, x.Favorite, x.Cover });

            var result = await data.Where(x => x.CompletedYear != null).GroupBy(x => x.CompletedYear ?? 0).Select(x => new HistoryYear()
            {
                Year = x.Key,
                WatchedNumber = x.Count(),
                FavoritesNumber = x.Where(x => x.Favorite).Count(),
                Data = x.OrderByDescending(x => x.FinishedOn).Take(8).OrderBy(x => x.FinishedOn).Select(y => new HistoryYearPreview() { MalId = y.MalId, NameEnglish = y.NameEnglish, Cover = y.Cover }).ToList()
            }).OrderByDescending(x => x.Year).ToListAsync();

            return result;
        }

        /// <summary>
        /// Retrieves list of animes watched based on year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PaginatedResponse<INT_AnimeShowFull>> GetHistoryYear(int year, int page)
        {
            return await LibraryQuery(new FilterIn() { Year = year }, SortIn.FinishDate, new PageIn() { CurrentPage = page, PerPage = 20 });
        }

        /// <summary>
        /// Retrieves the count of watched and favorite anime for a year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<INT_YearCount> GetHistoryCount(int year)
        {
            var data = _animeRepository.GetAsQueryable()
                .Where(x => x.AnimeShowProgress.CompletedYear == year)
                .Select(x => x.Favorite);

            return new INT_YearCount() { Year = year, Completed = await data.CountAsync(), Favorite = await data.Where(x => x).CountAsync() };
        }

        #endregion
        
    }
}
