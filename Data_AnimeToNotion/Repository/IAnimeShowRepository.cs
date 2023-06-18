using Data_AnimeToNotion.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_AnimeToNotion.Repository
{
    public interface IAnimeShowRepository
    {
        List<AnimeShow> GetAll();
        AnimeShow Add(AnimeShow animeShow);
        void Update(AnimeShow animeShow);
        void Remove(AnimeShow animeShow);


        void AddOrUpdateStudios(Dictionary<int, string> animeStudios, AnimeShow animeShow);
        void AddOrUpdateGenres(Dictionary<int, string> animeGenres, AnimeShow animeShow);
        void AddOrUpdateRelations(List<Relation> animeRelations, AnimeShow animeShow);
        void AddWatchingTime(WatchingTime watchingTime, AnimeShow animeShow);
        void AddScore(Score score, AnimeShow animeShow);
        void AddNote(Note note, AnimeShow animeShow);
        int GetCompletedYearValue(WatchingTime watchingTime);
        Guid GetCompletedYearId(string notionPageId);
        bool Exists(int malId);

        #region Sync MAL

        AnimeShow GetByNotionPageId(string notionPageId);
        void SyncFromMal(AnimeShow animeShow, Dictionary<int, string> studios, Dictionary<int, string> genres, List<Relation> relations);

        #endregion

        #region Demo
        void AddFromNotion(AnimeShow animeShow, Dictionary<int, string> studios, Dictionary<int, string> genres, List<Relation> relations);

        #endregion

    }
}
