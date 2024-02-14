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
                .Include(x => x.Relations)
                .AsSplitQuery()
                .Where(x => x.MalId == MalId).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves full anime by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<AnimeShow> GetFull(Guid Id)
        {
            return await _animeShowContext.AnimeShows
                .Include(x => x.AnimeShowProgress)
                .Include(x => x.GenreOnAnimeShows)
                .Include(x => x.StudioOnAnimeShows)
                .Include(x => x.Relations)
                .AsSplitQuery()
                .Where(x => x.Id == Id).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Retrives anime specifically for editing
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<AnimeShow> GetForEdit(Guid Id)
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
        public async Task<AnimeShow> AddInternalAnimeShow(AnimeShow animeShow, List<Studio> studios, List<Genre> genres, List<Relation> relations)
        {
            using var transaction = _animeShowContext.Database.BeginTransaction();

            AnimeShow added = (await _animeShowContext.AddAsync(animeShow)).Entity;

            await HandleStudios(studios, animeShow);
            await HandleGenres(genres, animeShow);
            await HandleRelations(relations, animeShow);

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
        public async Task RemoveInternalAnimeShow(Guid id)
        {
            using var transaction = _animeShowContext.Database.BeginTransaction();

            await _animeShowContext.Relations.Where(x => x.AnimeShowId == id).ExecuteDeleteAsync();
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
        public async Task SetAnimeFavorite(Guid id, bool isFavorite)
        {
            await _animeShowContext.AnimeShows.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(a => a.Favorite, a => isFavorite));
        }

        /// <summary>
        /// Toggle plan to watch for anime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetPlanToWatch(Guid id)
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

        #region Sync MAL

        /// <summary>
        /// Sync the current info in MAL with the show saved to DB
        /// </summary>
        /// <param name="animeShow"></param>
        /// <param name="studios"></param>
        /// <param name="genres"></param>
        /// <param name="relations"></param>
        public async Task SyncFromMal(AnimeShow animeShow, List<Studio> studios, List<Genre> genres, List<Relation> relations)
        {
            using var transaction = _animeShowContext.Database.BeginTransaction();

            if (studios != null)
                await HandleStudios(studios, animeShow);

            if (genres != null)
                await HandleGenres(genres, animeShow);

            if (relations != null)
                await HandleRelations(relations, animeShow);

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
            foreach (var studio in syncedStudios)
            {
                studioOnAnime.Add(new StudioOnAnimeShow() { Id = Guid.NewGuid(), Description = studio.Description, AnimeShowId = show.Id, StudioId = studio.Id });
            }

            // Bug with the function SyncMalData because deletes genres already present and adds delta only, so everytime the studios are different
            // Decomment if something wrong comes up
            //await _animeShowContext.StudioOnAnimeShows.Where(x => x.AnimeShowId == show.Id).ExecuteDeleteAsync();
            await _animeShowContext.StudioOnAnimeShows.AddRangeAsync(studioOnAnime);
        }

        private async Task HandleGenres(List<Genre> genres, AnimeShow show)
        {
            var syncedGenres = await FillMissingGenres(genres);

            List<GenreOnAnimeShow> genreOnAnime = new List<GenreOnAnimeShow>();
            foreach (var genre in syncedGenres)
            {
                genreOnAnime.Add(new GenreOnAnimeShow() { Id = Guid.NewGuid(), Description = genre.Description, AnimeShowId = show.Id, GenreId = genre.Id });
            }

            // Bug with the function SyncMalData because deletes genres already present and adds delta only, so everytime the genres are different
            // Decomment if something wrong comes up
            //await _animeShowContext.GenreOnAnimeShows.Where(x => x.AnimeShowId == show.Id).ExecuteDeleteAsync();
            await _animeShowContext.GenreOnAnimeShows.AddRangeAsync(genreOnAnime);
        }

        private async Task HandleRelations(List<Relation> relations, AnimeShow show)
        {
            foreach (var relation in relations) { relation.AnimeShowId = show.Id; }
            await _animeShowContext.Relations.AddRangeAsync(relations);
        }

        #endregion
    }

}
