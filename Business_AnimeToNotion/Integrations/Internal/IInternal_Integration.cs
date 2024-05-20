using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.History;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Pagination;
using Business_AnimeToNotion.Model.Query;
using Business_AnimeToNotion.QueryLogic.SortLogic;

namespace Business_AnimeToNotion.Integrations.Internal
{
    public interface IInternal_Integration
    {
        #region Single Operativity
        Task<AnimeShowPersonal> AddNewAnimeBase(AnimeShowBase animeAdd);
        Task<AnimeShowPersonal> AddNewAnimeFull(AnimeShowFull animeAdd);
        Task<AnimeShowFull> GetAnimeFull(int MalId);
        Task<AnimeShowFull> GetAnimeForEdit(int Id);
        Task EditAnime(AnimeShowEdit animeEdit, bool skipSync = false);
        Task<bool> SetAnimeFavorite(int id, bool favorite);
        Task<bool> SetAnimePlanToWatch(int id, bool planToWatch);
        Task RemoveAnime(int id);
        Task<List<AnimeEpisode>> GetAnimeEpisodes(int malId);

        #endregion

        #region Library Operativity

        Task<PaginatedResponse<AnimeShowFull>> LibraryQuery(FilterIn filters, SortIn? sort, PageIn page);

        #endregion

        #region History

        Task<List<HistoryYear>> GetHistory();
        Task<PaginatedResponse<AnimeShowFull>> GetHistoryYear(int year, int page);
        Task<YearCount> GetHistoryCount(int year);

        #endregion

        #region Forms

        Task<List<KeyValue>> GetGenres();

        #endregion
    }
}
