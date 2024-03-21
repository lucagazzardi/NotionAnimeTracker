using Data_AnimeToNotion.DataModel;

namespace Data_AnimeToNotion.Repository
{
    public interface IAnimeShowRepository
    {
        Task<AnimeShow> GetByMalId(int malId);
        IQueryable<AnimeShow> GetAllByIds(List<int> malIds);
        IQueryable<AnimeShow> GetAsQueryable();
        Task Save();
        Task<AnimeShow> GetFullByMalId(int MalId);
        Task<AnimeShow> GetFull(int Id);
        Task<AnimeShow> GetForEdit(int Id);
        Task<AnimeShow> AddInternalAnimeShow(AnimeShow animeShow, List<Studio> studios, List<Genre> genres);
        Task RemoveInternalAnimeShow(int id);
        Task SetAnimeFavorite(int id, bool isFavorite);
        Task SetPlanToWatch(int id);
        Task<bool> Exists(int malId);
        Task<int> AddEpisode(int animeId, int episodeNumber, DateTime watchedOn);
        Task<List<AnimeEpisode>> GetAnimeEpisodes(int animeId);
        Task EditEpisode(int id, DateTime watchedOn);
        Task DeleteAnimeEpisodes(int id);

        #region Sync MAL
        Task SyncFromMal(AnimeShow animeShow, List<Studio> studios, List<Genre> genres);

        #endregion

    }
}
