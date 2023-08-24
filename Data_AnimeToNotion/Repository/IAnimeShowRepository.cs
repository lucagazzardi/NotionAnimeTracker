using Data_AnimeToNotion.DataModel;

namespace Data_AnimeToNotion.Repository
{
    public interface IAnimeShowRepository
    {
        Task<AnimeShow> GetByMalId(int malId);
        IQueryable<AnimeShow> GetAllByIds(List<int> malIds);
        IQueryable<AnimeShow> GetAsQueryable();
        Task Save();
        Task<AnimeShow> GetFull(int MalId);
        Task<AnimeShow> GetFull(Guid Id);
        Task<AnimeShow> GetForEdit(Guid Id);
        Task<AnimeShow> AddInternalAnimeShow(AnimeShow animeShow, List<Studio> studios, List<Genre> genres, List<Relation> relations);
        Task RemoveInternalAnimeShow(Guid id);
        Task SetAnimeFavorite(Guid id);
        Task SetPlanToWatch(Guid id);
        Task<bool> Exists(int malId);

        #region Sync MAL
        Task SyncFromMal(AnimeShow animeShow, List<Studio> studios, List<Genre> genres, List<Relation> relations);

        #endregion

    }
}
