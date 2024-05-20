using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.History;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Pagination;
using Business_AnimeToNotion.Model.Query;
using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Business_AnimeToNotion.QueryLogic.PageLogic;
using Business_AnimeToNotion.QueryLogic.SortLogic;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Repository;
using JikanDotNet;
using Microsoft.EntityFrameworkCore;

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
        public async Task<AnimeShowPersonal> AddNewAnimeBase(AnimeShowBase animeAdd)
        {
            if (animeAdd.Info != null)
                return null;

            var added = await _animeRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(animeAdd),
                Mapping.Mapper.ProjectTo<Studio>(animeAdd.Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(animeAdd.Genres.AsQueryable()).ToList()
            );

            await _syncToNotionRepository.AddNotionSync(added);

            return new AnimeShowPersonal() { Id = added.Id, Status = added.Status };
        }

        /// <summary>
        /// Add new anime with relations
        /// </summary>
        /// <param name="animeAdd"></param>
        /// <returns></returns>
        public async Task<AnimeShowPersonal> AddNewAnimeFull(AnimeShowFull animeAdd)
        {
            var added = await _animeRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(animeAdd),
                Mapping.Mapper.ProjectTo<Studio>(animeAdd.Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(animeAdd.Genres.AsQueryable()).ToList()
            );

            await _syncToNotionRepository.AddNotionSync(added);

            return new AnimeShowPersonal() { Id = added.Id, Status = added.Status };
        }

        /// <summary>
        /// Get anime full for editing
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<AnimeShowFull> GetAnimeFull(int malId)
        {
            var animeInt = await _animeRepository.GetFull(malId);

            if (animeInt == null)
                return Mapping.Mapper.Map<AnimeShowFull>(await _mal.GetAnimeById(malId));

            return Mapping.Mapper.Map<AnimeShowFull>(animeInt);
        }

        /// <summary>
        /// Get anime for editing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AnimeShowFull> GetAnimeForEdit(int id)
        {
            return Mapping.Mapper.Map<AnimeShowFull>(await _animeRepository.GetFull(id));
        }

        /// <summary>
        /// Edit anime properties
        /// </summary>
        /// <param name="animeEdit"></param>
        /// <returns></returns>
        public async Task EditAnime(AnimeShowEdit animeEdit, bool skipSync = false)
        {
            var anime = await _animeRepository.GetForEdit(animeEdit.Id.Value);

            anime.Status = animeEdit.Status;

            anime.AnimeShowProgress.PersonalScore = animeEdit.PersonalScore;
            anime.AnimeShowProgress.EpisodesProgress = animeEdit.EpisodesProgress;
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

        public async Task<List<Model.Internal.AnimeEpisode>> GetAnimeEpisodes(int malId)
        {
            // Collect all the episodes of the anime from Jikan
            var episodes = (await _jikan.GetAnimeEpisodesAsync(malId)).Data;

            return episodes.Select(x => new Model.Internal.AnimeEpisode()
            {
                TitleEnglish = x.Title,
                TitleJapanese = x.TitleJapanese,
                EpisodeNumber = (int)x.MalId,
            }).ToList();
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
        public async Task<PaginatedResponse<AnimeShowFull>> LibraryQuery(FilterIn filters, SortIn? sort, PageIn page)
        {
            IQueryable<AnimeShow> data = _animeRepository.GetAsQueryable();

            FilterManager filterManager = new FilterManager(filters);
            data = filterManager.ApplyFilters(data);

            SortManager sortManager = new SortManager();
            data = sortManager.ApplySort(data, sort);

            PageManager pageManager = new PageManager();
            data = await pageManager.ApplyPaging(data, page);

            return pageManager.GeneratePaginatedResponse(await Mapping.Mapper.ProjectTo<AnimeShowFull>(data).ToListAsync(), page);
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
        public async Task<PaginatedResponse<AnimeShowFull>> GetHistoryYear(int year, int page)
        {
            return await LibraryQuery(new FilterIn() { Year = year }, SortIn.FinishDate, new PageIn() { CurrentPage = page, PerPage = 20 });
        }

        /// <summary>
        /// Retrieves the count of watched and favorite anime for a year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<YearCount> GetHistoryCount(int year)
        {
            var data = _animeRepository.GetAsQueryable()
                .Where(x => x.AnimeShowProgress.CompletedYear == year)
                .Select(x => x.Favorite);

            return new YearCount() { Year = year, Completed = await data.CountAsync(), Favorite = await data.Where(x => x).CountAsync() };
        }

        #endregion

        #region Forms

        public async Task<List<KeyValue>> GetGenres()
        {
            return (await _animeRepository.GetGenres()).Select(x => new KeyValue(x.Id, x.Description)).ToList();
        }

        #endregion

    }
}
