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
        Task AddNewAnimeBase(INT_AnimeShowBase animeAdd);
        Task AddNewAnimeFull(INT_AnimeShowFull animeAdd);
        Task<INT_AnimeShowFull> GetAnimeForEdit(Guid Id);
        Task RemoveAnime(Guid id);

        #region Demo

        Task<NotionSyncAdd> AddNewAnimeBaseDemo(INT_AnimeShowBase animeAdd);

        #endregion
    }
}
