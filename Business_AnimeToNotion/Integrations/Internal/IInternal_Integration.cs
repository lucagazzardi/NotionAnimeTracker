﻿using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.History;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Mixed;
using Business_AnimeToNotion.Model.Pagination;
using Business_AnimeToNotion.Model.Query;
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
        Task<INT_AnimeShowFull> GetAnimeForEdit(int Id);
        Task EditAnime(INT_AnimeShowEdit animeEdit, bool skipSync = false);
        Task<bool> SetAnimeFavorite(int id, bool favorite);
        Task<bool> SetAnimePlanToWatch(int id, bool planToWatch);
        Task RemoveAnime(int id);
        Task<int> AddAnimeEpisode(INT_AnimeEpisode animeEpisode);
        Task<List<INT_AnimeEpisode>> GetAnimeEpisodes(int animeShowId);
        Task EditAnimeEpisode(INT_AnimeEpisode animeEpisode);
        Task DeleteAnimeEpisode(int id);
        Task<AnimeEpisodesRecord> GetAnimeEpisodesRecord(int animeShowId, int malId);

        #endregion

        #region Library Operativity

        Task<PaginatedResponse<INT_AnimeShowFull>> LibraryQuery(FilterIn filters, SortIn? sort, PageIn page);

        #endregion

        #region History

        Task<List<HistoryYear>> GetHistory();
        Task<PaginatedResponse<INT_AnimeShowFull>> GetHistoryYear(int year, int page);
        Task<INT_YearCount> GetHistoryCount(int year);

        #endregion
    }
}
