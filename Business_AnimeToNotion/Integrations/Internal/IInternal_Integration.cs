using Business_AnimeToNotion.Model.Common;
using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Notion.Base;
using Business_AnimeToNotion.Model.Pagination;
using Business_AnimeToNotion.Model.Query;
using Business_AnimeToNotion.Model.Query.Filter;
using Business_AnimeToNotion.QueryLogic.SortLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Integrations.Internal
{
    public interface IInternal_Integration
    {
        #region Single Operativity
        Task<INT_AnimeShowPersonal> AddNewAnimeBase(INT_AnimeShowBase animeAdd);
        Task<INT_AnimeShowPersonal> AddNewAnimeFull(INT_AnimeShowFull animeAdd);
        Task<INT_AnimeShowFull> GetAnimeFull(int MalId);
        Task<INT_AnimeShowFull> GetAnimeForEdit(Guid Id);
        Task EditAnime(INT_AnimeShowEdit animeEdit);
        Task<bool> SetAnimeFavorite(Guid id, bool favorite);
        Task<bool> SetAnimePlanToWatch(Guid id, bool planToWatch);
        Task RemoveAnime(Guid id);
        Task<List<INT_AnimeShowRelation>> GetAnimeRelations(int malId);

        #endregion

        #region Library Operativity

        Task<PaginatedResponse<INT_AnimeShowFull>> LibraryQuery(FilterIn filters, SortIn? sort, PageIn page);

        #endregion

        #region Demo

        Task<NotionSyncAdd> AddNewAnimeBaseDemo(INT_AnimeShowBase animeAdd);

        #endregion
    }
}
