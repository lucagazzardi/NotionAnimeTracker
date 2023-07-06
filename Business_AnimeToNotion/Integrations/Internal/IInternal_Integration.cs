using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Notion.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Integrations.Internal
{
    public interface IInternal_Integration
    {
        Task<INT_AnimeShowPersonal> AddNewAnimeBase(INT_AnimeShowBase animeAdd);
        Task<INT_AnimeShowPersonal> AddNewAnimeFull(INT_AnimeShowFull animeAdd);
        Task<INT_AnimeShowFull> GetAnimeFull(int MalId);
        Task<INT_AnimeShowFull> GetAnimeForEdit(Guid Id);
        Task EditAnime(INT_AnimeShowEdit animeEdit);
        Task RemoveAnime(Guid id);
        Task<List<INT_AnimeShowRelation>> GetAnimeRelations(int malId);

        #region Demo

        Task<NotionSyncAdd> AddNewAnimeBaseDemo(INT_AnimeShowBase animeAdd);

        #endregion
    }
}
