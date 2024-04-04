using Data_AnimeToNotion.Context;
using Data_AnimeToNotion.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.Repository
{
    public class AnimeShowRepository : IAnimeShowRepository
    {
        private readonly AnimeShowContext _animeShowContext;

        public AnimeShowRepository(AnimeShowContext animeShowContext)
        {
            _animeShowContext = animeShowContext;
        }

        /// <summary>
        /// Retrieves anime from MalId
        /// </summary>
        /// <param name="malId"></param>
        /// <returns></returns>
        public async Task<AnimeShow> GetByMalId(int malId)
        {
            return await _animeShowContext.AnimeShows.Where(x => x.MalId == malId).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all animes with passed ids
        /// </summary>
        /// <param name="malIds"></param>
        /// <returns></returns>
        public IQueryable<AnimeShow> GetAllByIds(List<int> malIds)
        {
            return _animeShowContext.AnimeShows.Where(x => malIds.Contains(x.MalId)).AsNoTracking().AsQueryable();
        }

        /// <summary>
        /// Get basic queryable for anime
        /// </summary>
        /// <returns></returns>
        public IQueryable<AnimeShow> GetAsQueryable()
        {
            return _animeShowContext.AnimeShows.AsNoTracking().AsSplitQuery().AsQueryable();
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        public async Task Save()
        {            
            await _animeShowContext.SaveChangesAsync();
        }

        #region Internal GET

        /// <summary>
        /// Retrieves full anime by MalId
        /// </summary>
        /// <param name="MalId"></param>
        /// <returns></returns>
        public async Task<AnimeShow> GetFull(int MalId)
        {
            return await _animeShowContext.AnimeShows
                .Include(x => x.AnimeShowProgress)
                .Include(x => x.GenreOnAnimeShows)
                .Include(x => x.StudioOnAnimeShows)
                .AsSplitQuery()
                .Where(x => x.MalId == MalId).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves full anime by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<AnimeShow> GetFullByMalId(int Id)
        {
            return await _animeShowContext.AnimeShows
                .Include(x => x.AnimeShowProgress)
                .Include(x => x.GenreOnAnimeShows)
                .Include(x => x.StudioOnAnimeShows)
                .AsSplitQuery()
                .Where(x => x.Id == Id).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Retrives anime specifically for editing
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<AnimeShow> GetForEdit(int Id)
        {
            return await _animeShowContext.AnimeShows
                .Include(x => x.AnimeShowProgress)
                .Where(x => x.Id == Id).SingleOrDefaultAsync();
        }

        #endregion

        #region Internal ADD

        /// <summary>
        /// Add anime
        /// </summary>
        /// <param name="animeShow"></param>
        /// <param name="studios"></param>
        /// <param name="genres"></param>
        /// <param name="relations"></param>
        /// <returns></returns>
        public async Task<AnimeShow> AddInternalAnimeShow(AnimeShow animeShow, List<Studio> studios, List<Genre> genres)
        {
            using var transaction = _animeShowContext.Database.BeginTransaction();

            AnimeShow added = (await _animeShowContext.AddAsync(animeShow)).Entity;

            await HandleStudios(studios, animeShow);
            await HandleGenres(genres, animeShow);

            await _animeShowContext.SaveChangesAsync();

            transaction.Commit();

            return added;
        }

        #endregion

        #region Internal REMOVE

        /// <summary>
        /// Remove anime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveInternalAnimeShow(int id)
        {
            using var transaction = _animeShowContext.Database.BeginTransaction();

            await _animeShowContext.GenreOnAnimeShows.Where(x => x.AnimeShowId == id).ExecuteDeleteAsync();
            await _animeShowContext.StudioOnAnimeShows.Where(x => x.AnimeShowId == id).ExecuteDeleteAsync();
            await _animeShowContext.AnimeShows.Where(x => x.Id == id).ExecuteDeleteAsync();

            transaction.Commit();
        }

        #endregion        

        /// <summary>
        /// Toggle favorite for anime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetAnimeFavorite(int id, bool isFavorite)
        {
            await _animeShowContext.AnimeShows.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(a => a.Favorite, a => isFavorite));
        }

        /// <summary>
        /// Toggle plan to watch for anime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetPlanToWatch(int id)
        {
            await _animeShowContext.AnimeShows.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(a => a.PlanToWatch, a => !a.PlanToWatch));
        }        

        /// <summary>
        /// Check if the anime with the specified MalId is already existing
        /// </summary>
        /// <param name="malId"></param>
        /// <returns></returns>
        public async Task<bool> Exists(int malId)
        {
            return await _animeShowContext.AnimeShows.AsNoTracking().AnyAsync(x => x.MalId == malId);
        }

        /// <summary>
        /// Adds new anime episode record
        /// </summary>
        /// <param name="animeId"></param>
        /// <param name="episodeNumber"></param>
        /// <param name="watchedOn"></param>
        /// <returns></returns>
        public async Task<int> AddEpisode(int animeId, int episodeNumber, DateTime watchedOn)
        {
            var added = await _animeShowContext.AnimeEpisodes.AddAsync(new AnimeEpisode() 
            { 
                AnimeShowId = animeId, EpisodeNumber = episodeNumber, WatchedOn = watchedOn 
            });
            await _animeShowContext.SaveChangesAsync();
            return added.Entity.Id;
        }

        /// <summary>
        /// Edits an episode already watched
        /// </summary>
        /// <param name="animeId"></param>
        /// <param name="episodeNumber"></param>
        /// <param name="watchedOn"></param>
        /// <returns></returns>
        public async Task EditEpisode(int id, DateTime watchedOn)
        {
            await _animeShowContext.AnimeEpisodes.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(a => a.WatchedOn, a => watchedOn));
        }

        /// <summary>
        /// Retrieves every watched episode for an anime
        /// </summary>
        /// <param name="animeId"></param>
        /// <returns></returns>
        public async Task<List<AnimeEpisode>> GetAnimeEpisodes(int animeId)
        {
            return await _animeShowContext.AnimeEpisodes.Where(x => x.AnimeShowId == animeId).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Deletes an anime episode
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAnimeEpisodes(int id)
        {
            await _animeShowContext.AnimeEpisodes.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Genre>> GetGenres()
        {
            return await _animeShowContext.Genres.AsNoTracking().ToListAsync();
        }

        #region Sync MAL

        /// <summary>
        /// Sync the current info in MAL with the show saved to DB
        /// </summary>
        /// <param name="animeShow"></param>
        /// <param name="studios"></param>
        /// <param name="genres"></param>
        /// <param name="relations"></param>
        public async Task SyncFromMal(AnimeShow animeShow, List<Studio> studios, List<Genre> genres)
        {
            using var transaction = _animeShowContext.Database.BeginTransaction();

            if (studios != null)
                await HandleStudios(studios, animeShow);

            if (genres != null)
                await HandleGenres(genres, animeShow);

            await _animeShowContext.SaveChangesAsync();

            transaction.Commit();
        }

        #endregion       

        #region Private

        private async Task<List<Studio>> FillMissingStudios(List<Studio> studios)
        {
            var studioMalIds = studios.Select(x => x.Id).ToList();
            var existingStudios = await _animeShowContext.Studios.AsNoTracking().Where(x => studioMalIds.Contains(x.Id)).ToListAsync();

            if (existingStudios.Any() && studioMalIds.All(x => existingStudios.Select(x => x.Id).Contains(x)))
                return existingStudios;

            List<Studio> newStudios = studios.Where(x => !existingStudios.Select(x => x.Id).Contains(x.Id)).ToList();
            await _animeShowContext.AddRangeAsync(newStudios);

            return existingStudios.Union(newStudios).ToList();
        }

        private async Task<List<Genre>> FillMissingGenres(List<Genre> genres)
        {
            var genreMalIds = genres.Select(x => x.Id).ToList();
            var existingGenres = await _animeShowContext.Genres.AsNoTracking().Where(x => genreMalIds.Contains(x.Id)).ToListAsync();

            if (existingGenres.Any() && genreMalIds.All(x => existingGenres.Select(x => x.Id).Contains(x)))
                return existingGenres;

            List<Genre> newGenres = genres.Where(x => !existingGenres.Select(x => x.Id).Contains(x.Id)).ToList();
            await _animeShowContext.AddRangeAsync(newGenres);

            return existingGenres.Union(newGenres).ToList();
        }

        private async Task HandleStudios(List<Studio> studios, AnimeShow show)
        {
            var syncedStudios = await FillMissingStudios(studios);

            List<StudioOnAnimeShow> studioOnAnime = new List<StudioOnAnimeShow>();
            foreach(var studio in syncedStudios)
            {
                studioOnAnime.Add(new StudioOnAnimeShow() { Description = studio.Description, AnimeShow = show, StudioId = studio.Id });
            }
            await _animeShowContext.StudioOnAnimeShows.AddRangeAsync(studioOnAnime);
        }

        private async Task HandleGenres(List<Genre> genres, AnimeShow show)
        {
            var syncedGenres = await FillMissingGenres(genres);

            List<GenreOnAnimeShow> genreOnAnime = new List<GenreOnAnimeShow>();

            foreach (var genre in syncedGenres)
            {
                genreOnAnime.Add(new GenreOnAnimeShow() { Description = genre.Description, AnimeShow = show, GenreId = genre.Id });
            }
            await _animeShowContext.GenreOnAnimeShows.AddRangeAsync(genreOnAnime);
        }

        #endregion
    }

}
