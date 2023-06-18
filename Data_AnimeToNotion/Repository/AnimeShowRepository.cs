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

        public List<AnimeShow> GetAll()
        {
            return _animeShowContext.AnimeShows.ToList();
        }        

        public AnimeShow Add(AnimeShow animeShow)
        {
            var anime =_animeShowContext.Add(animeShow);
            _animeShowContext.SaveChanges();
            return anime.Entity;
        }

        public void Update(AnimeShow animeShow)
        {
            _animeShowContext.Update(animeShow);
            _animeShowContext.SaveChanges();
        }

        public void Remove(AnimeShow animeShow)
        {
            _animeShowContext.Remove(animeShow);
            _animeShowContext.SaveChanges();
        }        

        /// <summary>
        /// Sync studios with the current studios from MAL
        /// </summary>
        /// <param name="animeStudios"></param>
        /// <param name="animeShow"></param>
        public void AddOrUpdateStudios(Dictionary<int, string> animeStudios, AnimeShow animeShow)
        {
            // Remove all the studios links that are present in DB but not in the studios passed as argument (that are always all the studios from MAL)
            var malIds = animeStudios.Keys;
            var toRemove = _animeShowContext.StudioOnAnimeShows.Include(x => x.Studio).Where(x => x.AnimeShowId == animeShow.Id && !malIds.Contains(x.Studio.MalId)).ToList();
            _animeShowContext.StudioOnAnimeShows.RemoveRange(toRemove);

            // Check if the Studio is already present in DB, if not adds it, then check if the link with the show is present, if not adds it
            foreach (var studio in animeStudios)
            {
                var studioId = _animeShowContext.Studios.SingleOrDefault(x => x.MalId == studio.Key)?.Id;

                if (studioId == null)
                    studioId = _animeShowContext.Studios.Add(new Studio() { Id = new Guid(), MalId = studio.Key, Description = studio.Value }).Entity.Id;                

                if (!_animeShowContext.StudioOnAnimeShows.Any(x => x.AnimeShowId == animeShow.Id && x.StudioId == studioId))
                    _animeShowContext.StudioOnAnimeShows.Add(new StudioOnAnimeShow() { Id = new Guid(), AnimeShowId = animeShow.Id, StudioId = studioId.Value });
            }
        }

        /// <summary>
        /// Sync genres with the current genres from MAL
        /// </summary>
        /// <param name="animeGenres"></param>
        /// <param name="animeShow"></param>
        public void AddOrUpdateGenres(Dictionary<int, string> animeGenres, AnimeShow animeShow)
        {
            // Remove all the genres links that are present in DB but not in the genres passed as argument (that are always all the genres from MAL)
            var malIds = animeGenres.Keys;
            var toRemove = _animeShowContext.GenreOnAnimeShows.Include(x => x.Genre).Where(x => x.AnimeShowId == animeShow.Id && !malIds.Contains(x.Genre.MalId)).ToList();
            _animeShowContext.GenreOnAnimeShows.RemoveRange(toRemove);

            // Check if the Genre is already present in DB, if not adds it, then check if the link with the show is present, if not adds it
            foreach (var genre in animeGenres)
            {
                var genreId = _animeShowContext.Genres.SingleOrDefault(x => x.MalId == genre.Key)?.Id;

                if (genreId == null)
                    genreId = _animeShowContext.Genres.Add(new Genre() { Id = new Guid(), MalId = genre.Key, Description = genre.Value }).Entity.Id;

                if(!_animeShowContext.GenreOnAnimeShows.Any(x => x.AnimeShowId == animeShow.Id && x.GenreId == genreId))
                    _animeShowContext.GenreOnAnimeShows.Add(new GenreOnAnimeShow() { Id = new Guid(), AnimeShowId = animeShow.Id, GenreId = genreId.Value });
            }
        }

        /// <summary>
        /// Sync relations with the current relations from MAL
        /// </summary>
        /// <param name="animeRelations"></param>
        /// <param name="animeShow"></param>
        public void AddOrUpdateRelations(List<Relation> animeRelations, AnimeShow animeShow)
        {
            var malIds = animeRelations.Select(x => x.AnimeRelatedMalId);
            var toRemove = _animeShowContext.Relations.Where(x => x.AnimeShowId == animeShow.Id && !malIds.Contains(x.AnimeRelatedMalId)).ToList();
            _animeShowContext.RemoveRange(toRemove);

            foreach(var relation in animeRelations)
            {
                relation.AnimeShowId = animeShow.Id;
                if (!_animeShowContext.Relations.Any(x => x.AnimeShowId == animeShow.Id && relation.AnimeRelatedMalId == x.AnimeRelatedMalId))
                    _animeShowContext.Relations.Add(relation);
            }
        }

        public void AddWatchingTime(WatchingTime watchingTime, AnimeShow animeShow)
        {    
            _animeShowContext.WatchingTimes.Add(watchingTime);
            animeShow.WatchingTime = watchingTime;
            _animeShowContext.SaveChanges();
        }

        public void AddScore(Score score, AnimeShow animeShow)
        {
            _animeShowContext.Scores.Add(score);
            animeShow.Score = score;
            _animeShowContext.SaveChanges();            
        }

        public void AddNote(Note note, AnimeShow animeShow)
        {
            _animeShowContext.Notes.Add(note);
            animeShow.Note = note;
            _animeShowContext.SaveChanges();
        }

        public int GetCompletedYearValue(WatchingTime watchingTime)
        {
            return _animeShowContext.Year.Single(x => x.Id == watchingTime.CompletedYear).YearValue;
        }

        public Guid GetCompletedYearId(string notionPageId)
        {
            return _animeShowContext.Year.Single(x => x.NotionPageId == notionPageId).Id;
        }

        public bool Exists(int malId)
        {
            return _animeShowContext.AnimeShows.Any(x => x.MalId == malId);
        }

        #region Sync MAL
        public AnimeShow GetByNotionPageId(string notionPageId)
        {
            return _animeShowContext.AnimeShows
                .Include(x => x.Score)
                .Single(x => x.NotionPageId == notionPageId);
        }

        /// <summary>
        /// Sync the current info in MAL with the show saved to DB
        /// </summary>
        /// <param name="animeShow"></param>
        /// <param name="studios"></param>
        /// <param name="genres"></param>
        /// <param name="relations"></param>
        public void SyncFromMal(AnimeShow animeShow, Dictionary<int, string> studios, Dictionary<int, string> genres, List<Relation> relations)
        {
            _animeShowContext.Update(animeShow);

            AddOrUpdateStudios(studios, animeShow);
            AddOrUpdateGenres(genres, animeShow);
            AddOrUpdateRelations(relations, animeShow);
            AddWatchingTime(animeShow.WatchingTime, animeShow);
            AddScore(animeShow.Score, animeShow);
            AddNote(animeShow.Note, animeShow);


            _animeShowContext.SaveChanges();
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
        public void AddFromNotion(AnimeShow animeShow, Dictionary<int, string> studios, Dictionary<int, string> genres, List<Relation> relations)
        {
            _animeShowContext.Add(animeShow);

            AddOrUpdateStudios(studios, animeShow);
            AddOrUpdateGenres(genres, animeShow);
            AddOrUpdateRelations(relations, animeShow);
            

            _animeShowContext.SaveChanges();
        }

        #endregion
    }

}
