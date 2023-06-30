using Data_AnimeToNotion.Context;
using Data_AnimeToNotion.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Data_AnimeToNotion.Repository
{
    public class AnimeShowRepository : IAnimeShowRepository
    {
        private readonly AnimeShowContext _animeShowContext;

        public AnimeShowRepository(AnimeShowContext animeShowContext)
        {
            _animeShowContext = animeShowContext;
        }

        public IQueryable<AnimeShow> GetAllByIds(List<int> malIds)
        {
            return _animeShowContext.AnimeShows.Where(x => malIds.Contains(x.MalId)).AsNoTracking().AsQueryable();
        }

        public async Task<AnimeShow> Add(AnimeShow animeShow)
        {
            var anime = await _animeShowContext.AddAsync(animeShow);
            await _animeShowContext.SaveChangesAsync();
            return anime.Entity;
        }

        public async Task Update(AnimeShow animeShow)
        {
            _animeShowContext.Update(animeShow);
            await _animeShowContext.SaveChangesAsync();
        }

        public async Task Remove(AnimeShow animeShow)
        {
            _animeShowContext.Remove(animeShow);
            await _animeShowContext.SaveChangesAsync();
        }

        #region Internal GET

        public async Task<AnimeShow> GetFull(Guid Id)
        {
            return await _animeShowContext.AnimeShows
                .Include(x => x.Score)
                .Include(x => x.WatchingTime)
                .ThenInclude(x => x.Year)
                .Include(x => x.Note)
                .Include(x => x.GenreOnAnimeShows)
                .ThenInclude(x => x.Genre)
                .Include(x => x.StudioOnAnimeShows)
                .ThenInclude(x => x.Studio)
                .Include(x => x.Relations)
                .AsSplitQuery()
                .Where(x => x.Id == Id).AsNoTracking().SingleOrDefaultAsync();
        }

        #endregion

        #region Internal ADD

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

        private async Task HandleStudios(List<Studio> studios, AnimeShow show)
        {
            var syncedStudios = await FillMissingStudios(studios);

            List<StudioOnAnimeShow> studioOnAnime = new List<StudioOnAnimeShow>();
            foreach(var studio in syncedStudios)
            {
                studioOnAnime.Add(new StudioOnAnimeShow() { Id = Guid.NewGuid(), AnimeShowId = show.Id, StudioId = studio.Id });
            }

            await _animeShowContext.StudioOnAnimeShows.AddRangeAsync(studioOnAnime);
        }

        private async Task HandleGenres(List<Genre> genres, AnimeShow show)
        {
            var syncedGenres = await FillMissingGenres(genres);

            List<GenreOnAnimeShow> genreOnAnime = new List<GenreOnAnimeShow>();
            foreach (var genre in syncedGenres)
            {
                genreOnAnime.Add(new GenreOnAnimeShow() { Id = Guid.NewGuid(), AnimeShowId = show.Id, GenreId = genre.Id });
            }

            await _animeShowContext.GenreOnAnimeShows.AddRangeAsync(genreOnAnime);
        }

        private async Task HandleRelations(List<Relation> relations, AnimeShow show)
        {
            foreach(var relation in relations) { relation.AnimeShowId = show.Id; }
            await _animeShowContext.Relations.AddRangeAsync(relations);
        }

        /// <summary>
        /// Sync studios with the current studios from MAL
        /// </summary>
        /// <param name="animeStudios"></param>
        /// <param name="animeShow"></param>
        public async Task AddOrUpdateStudios(Dictionary<int, string> animeStudios, AnimeShow animeShow)
        {
            // Remove all the studios links that are present in DB but not in the studios passed as argument (that are always all the studios from MAL)
            var malIds = animeStudios.Keys;
            var toRemove = await _animeShowContext.StudioOnAnimeShows.Include(x => x.Studio).Where(x => x.AnimeShowId == animeShow.Id && !malIds.Contains(x.Studio.MalId)).ToListAsync();
            _animeShowContext.StudioOnAnimeShows.RemoveRange(toRemove);

            // Check if the Studio is already present in DB, if not adds it, then check if the link with the show is present, if not adds it
            foreach (var studio in animeStudios)
            {
                var studioId = (await _animeShowContext.Studios.SingleOrDefaultAsync(x => x.MalId == studio.Key))?.Id;

                if (studioId == null)
                    studioId = (await _animeShowContext.Studios.AddAsync(new Studio() { Id = new Guid(), MalId = studio.Key, Description = studio.Value })).Entity.Id;

                if (!await _animeShowContext.StudioOnAnimeShows.AnyAsync(x => x.AnimeShowId == animeShow.Id && x.StudioId == studioId))
                    await _animeShowContext.StudioOnAnimeShows.AddAsync(new StudioOnAnimeShow() { Id = new Guid(), AnimeShowId = animeShow.Id, StudioId = studioId.Value });
            }
        }

        /// <summary>
        /// Sync genres with the current genres from MAL
        /// </summary>
        /// <param name="animeGenres"></param>
        /// <param name="animeShow"></param>
        public async Task AddOrUpdateGenres(Dictionary<int, string> animeGenres, AnimeShow animeShow)
        {
            // Remove all the genres links that are present in DB but not in the genres passed as argument (that are always all the genres from MAL)
            var malIds = animeGenres.Keys;
            var toRemove = await _animeShowContext.GenreOnAnimeShows.Include(x => x.Genre).Where(x => x.AnimeShowId == animeShow.Id && !malIds.Contains(x.Genre.MalId)).ToListAsync();
            _animeShowContext.GenreOnAnimeShows.RemoveRange(toRemove);

            // Check if the Genre is already present in DB, if not adds it, then check if the link with the show is present, if not adds it
            foreach (var genre in animeGenres)
            {
                var genreId = (await _animeShowContext.Genres.SingleOrDefaultAsync(x => x.MalId == genre.Key))?.Id;

                if (genreId == null)
                    genreId = (await _animeShowContext.Genres.AddAsync(new Genre() { Id = new Guid(), MalId = genre.Key, Description = genre.Value })).Entity.Id;

                if (!await _animeShowContext.GenreOnAnimeShows.AnyAsync(x => x.AnimeShowId == animeShow.Id && x.GenreId == genreId))
                    await _animeShowContext.GenreOnAnimeShows.AddAsync(new GenreOnAnimeShow() { Id = new Guid(), AnimeShowId = animeShow.Id, GenreId = genreId.Value });
            }
        }

        /// <summary>
        /// Sync relations with the current relations from MAL
        /// </summary>
        /// <param name="animeRelations"></param>
        /// <param name="animeShow"></param>
        public async Task AddOrUpdateRelations(List<Relation> animeRelations, AnimeShow animeShow)
        {
            var malIds = animeRelations.Select(x => x.AnimeRelatedMalId);
            var toRemove = await _animeShowContext.Relations.Where(x => x.AnimeShowId == animeShow.Id && !malIds.Contains(x.AnimeRelatedMalId)).ToListAsync();
            _animeShowContext.RemoveRange(toRemove);

            foreach (var relation in animeRelations)
            {
                relation.AnimeShowId = animeShow.Id;
                if (!await _animeShowContext.Relations.AnyAsync(x => x.AnimeShowId == animeShow.Id && relation.AnimeRelatedMalId == x.AnimeRelatedMalId))
                    await _animeShowContext.Relations.AddAsync(relation);
            }
        }

        public async Task AddWatchingTime(WatchingTime watchingTime, AnimeShow animeShow)
        {
            await _animeShowContext.WatchingTimes.AddAsync(watchingTime);
            animeShow.WatchingTime = watchingTime;
            //await _animeShowContext.SaveChangesAsync();
        }

        public async Task AddScore(Score score, AnimeShow animeShow)
        {
            await _animeShowContext.Scores.AddAsync(score);
            animeShow.Score = score;
            //await _animeShowContext.SaveChangesAsync();            
        }

        public async Task AddNote(Note note, AnimeShow animeShow)
        {
            await _animeShowContext.Notes.AddAsync(note);
            animeShow.Note = note;
            //await _animeShowContext.SaveChangesAsync();
        }

        public async Task<int> GetCompletedYearValue(WatchingTime watchingTime)
        {
            return await _animeShowContext.Year.AsNoTracking().Where(x => x.Id == watchingTime.CompletedYear).Select(x => x.YearValue).SingleOrDefaultAsync();
        }

        public async Task<Guid> GetCompletedYearId(string notionPageId)
        {
            return await _animeShowContext.Year.AsNoTracking().Where(x => x.NotionPageId == notionPageId).Select(x => x.Id).SingleOrDefaultAsync();
        }

        public async Task<bool> Exists(int malId)
        {
            return await _animeShowContext.AnimeShows.AsNoTracking().AnyAsync(x => x.MalId == malId);
        }

        #region Sync MAL
        public async Task<AnimeShow> GetByNotionPageId(string notionPageId)
        {
            return await _animeShowContext.AnimeShows
                .Include(x => x.Score)
                .SingleAsync(x => x.NotionPageId == notionPageId);
        }

        /// <summary>
        /// Sync the current info in MAL with the show saved to DB
        /// </summary>
        /// <param name="animeShow"></param>
        /// <param name="studios"></param>
        /// <param name="genres"></param>
        /// <param name="relations"></param>
        public async Task SyncFromMal(AnimeShow animeShow, Dictionary<int, string> studios, Dictionary<int, string> genres, List<Relation> relations)
        {
            _animeShowContext.Update(animeShow);

            await AddOrUpdateStudios(studios, animeShow);
            await AddOrUpdateGenres(genres, animeShow);
            await AddOrUpdateRelations(relations, animeShow);
            await AddWatchingTime(animeShow.WatchingTime, animeShow);
            await AddScore(animeShow.Score, animeShow);
            await AddNote(animeShow.Note, animeShow);

            await _animeShowContext.SaveChangesAsync();
        }

        #endregion

        #region Demo

        /// <summary>
        /// Method used to fill the database
        /// </summary>
        /// <param name="animeShow"></param>
        /// <param name="studios"></param>
        /// <param name="genres"></param>
        /// <param name="relations"></param>
        public async Task AddFromNotion(AnimeShow animeShow, Dictionary<int, string> studios, Dictionary<int, string> genres, List<Relation> relations)
        {
            await _animeShowContext.AddAsync(animeShow);

            await AddOrUpdateStudios(studios, animeShow);
            await AddOrUpdateGenres(genres, animeShow);
            await AddOrUpdateRelations(relations, animeShow);            

            await _animeShowContext.SaveChangesAsync();
        }

        #endregion

        #region Private

        private async Task<List<Studio>> FillMissingStudios(List<Studio> studios)
        {
            var studioMalIds = studios.Select(x => x.MalId).ToList();
            var existingStudios = await _animeShowContext.Studios.AsNoTracking().Where(x => studioMalIds.Contains(x.MalId)).ToListAsync();

            if (existingStudios.All(x => studioMalIds.Contains(x.MalId)))
                return existingStudios;

            List<Studio> newStudios = studios.Where(x => !existingStudios.Select(x => x.MalId).Contains(x.MalId)).ToList();
            await _animeShowContext.AddRangeAsync(newStudios);

            return existingStudios.Union(newStudios).ToList();
        }

        private async Task<List<Genre>> FillMissingGenres(List<Genre> genres)
        {
            var genreMalIds = genres.Select(x => x.MalId).ToList();
            var existingGenres = await _animeShowContext.Genres.AsNoTracking().Where(x => genreMalIds.Contains(x.MalId)).ToListAsync();

            if (existingGenres.All(x => genreMalIds.Contains(x.MalId)))
                return existingGenres;

            List<Genre> newGenres = genres.Where(x => !existingGenres.Select(x => x.MalId).Contains(x.MalId)).ToList();
            await _animeShowContext.AddRangeAsync(newGenres);

            return existingGenres.Union(newGenres).ToList();
        }

        #endregion
    }

}
