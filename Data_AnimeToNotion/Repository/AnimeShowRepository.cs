using Data_AnimeToNotion.Context;
using Data_AnimeToNotion.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            AnimeShow toEdit = _animeShowContext.AnimeShows.Single(x => x.Id == animeShow.Id);
            toEdit = animeShow;

            _animeShowContext.Update(toEdit);
            _animeShowContext.SaveChanges();
        }

        public void Remove(AnimeShow animeShow)
        {
            _animeShowContext.Remove(animeShow);
            _animeShowContext.SaveChanges();
        }

        
        public void AddStudios(Dictionary<int, string> animeStudios, AnimeShow animeShow)
        {
            foreach(var studio in animeStudios)
            {
                var studioId = _animeShowContext.Studios.SingleOrDefault(x => x.MalId == studio.Key)?.Id;

                if (studioId == null)
                    studioId = _animeShowContext.Studios.Add(new Studio() { Id = new Guid(), MalId = studio.Key, Description = studio.Value }).Entity.Id;

                _animeShowContext.StudioOnAnimeShows.Add(new StudioOnAnimeShow() { Id = new Guid(), AnimeShowId = animeShow.Id, StudioId = studioId.Value });
            }

            _animeShowContext.SaveChanges();
        }

        public void AddGenres(Dictionary<int, string> animeGenres, AnimeShow animeShow)
        {
            foreach (var genre in animeGenres)
            {
                var genreId = _animeShowContext.Genres.SingleOrDefault(x => x.MalId == genre.Key)?.Id;

                if (genreId == null)
                    genreId = _animeShowContext.Genres.Add(new Genre() { Id = new Guid(), MalId = genre.Key, Description = genre.Value }).Entity.Id;

                _animeShowContext.GenreOnAnimeShows.Add(new GenreOnAnimeShow() { Id = new Guid(), AnimeShowId = animeShow.Id, GenreId = genreId.Value });
            }

            _animeShowContext.SaveChanges();
        }

        public void AddWatchingTime(WatchingTime watchingTime, string notionPageId, AnimeShow animeShow)
        {
            if (!string.IsNullOrEmpty(notionPageId))
            {
                watchingTime.CompletedYear = _animeShowContext.Year.Single(x => x.NotionPageId == notionPageId).Id;
            }
            watchingTime.Id = Guid.NewGuid();

            _animeShowContext.WatchingTimes.Add(watchingTime);
            animeShow.WatchingTime = watchingTime;
            _animeShowContext.SaveChanges();
        }

        public void AddScore(Score score, AnimeShow animeShow)
        {
            score.Id = Guid.NewGuid(); 
            _animeShowContext.Scores.Add(score);
            animeShow.Score = score;
            _animeShowContext.SaveChanges();            
        }

        public void AddNote(Note note, AnimeShow animeShow)
        {
            note.Id = Guid.NewGuid();
            _animeShowContext.Notes.Add(note);
            animeShow.Note = note;
            _animeShowContext.SaveChanges();
        }

        public int GetCompletedYearValue(WatchingTime watchingTime)
        {
            return _animeShowContext.Year.Single(x => x.Id == watchingTime.CompletedYear).YearValue;
        }

        public bool IsAlreadyExisting(int malId)
        {
            return _animeShowContext.AnimeShows.Any(x => x.MalId == malId);
        }

        //public void UpdateRelationsToFill(int animeRelatedMalId, Guid animeShowParentId)
        //{


        //    var toUpdate = _animeShowContext.Relations
        //        .Where(x => x.AnimeShowParentId == null && (x.AnimeMalId == animeRelatedMalId || x.AnimeRelatedMalId == animeRelatedMalId))
        //        .ToList();

        //    foreach(var update in toUpdate)
        //    {
        //        update.AnimeShowParentId = animeShowParentId;
        //    }
        //}
    }

}
