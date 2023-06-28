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
        IQueryable<AnimeShow> GetAllByIds(List<int> malIds);
        Task<AnimeShow> Add(AnimeShow animeShow);
        Task Update(AnimeShow animeShow);
        Task Remove(AnimeShow animeShow);


        Task<AnimeShow> AddInternalAnimeShow(AnimeShow animeShow, List<Studio> studios, List<Genre> genres, List<Relation> relations);
        Task RemoveInternalAnimeShow(Guid id);

        Task AddOrUpdateStudios(Dictionary<int, string> animeStudios, AnimeShow animeShow);
        Task AddOrUpdateGenres(Dictionary<int, string> animeGenres, AnimeShow animeShow);
        Task AddOrUpdateRelations(List<Relation> animeRelations, AnimeShow animeShow);
        Task AddWatchingTime(WatchingTime watchingTime, AnimeShow animeShow);
        Task AddScore(Score score, AnimeShow animeShow);
        Task AddNote(Note note, AnimeShow animeShow);
        Task<int> GetCompletedYearValue(WatchingTime watchingTime);
        Task<Guid> GetCompletedYearId(string notionPageId);
        Task<bool> Exists(int malId);

        #region Sync MAL

        Task<AnimeShow> GetByNotionPageId(string notionPageId);
        Task SyncFromMal(AnimeShow animeShow, Dictionary<int, string> studios, Dictionary<int, string> genres, List<Relation> relations);

        #endregion

        #region Demo
        Task AddFromNotion(AnimeShow animeShow, Dictionary<int, string> studios, Dictionary<int, string> genres, List<Relation> relations);

        #endregion

    }
}
