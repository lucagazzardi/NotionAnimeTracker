﻿using Data_AnimeToNotion.DataModel;

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
        Task<AnimeShow> AddInternalAnimeShow(AnimeShow animeShow, List<Studio> studios, List<Genre> genres);
        Task RemoveInternalAnimeShow(Guid id);
        Task SetAnimeFavorite(Guid id, bool isFavorite);
        Task SetPlanToWatch(Guid id);
        Task<bool> Exists(int malId);
        Task<Guid> AddEpisode(Guid animeId, int episodeNumber, DateTime watchedOn);
        Task<List<AnimeEpisode>> GetAnimeEpisodes(Guid animeId);
        Task EditEpisode(Guid id, DateTime watchedOn);
        Task DeleteAnimeEpisodes(Guid id);

        #region Sync MAL
        Task SyncFromMal(AnimeShow animeShow, List<Studio> studios, List<Genre> genres);

        #endregion

    }
}
