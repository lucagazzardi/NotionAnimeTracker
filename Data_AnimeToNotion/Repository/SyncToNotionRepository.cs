﻿using Data_AnimeToNotion.Context;
using Data_AnimeToNotion.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.Repository
{
    public class SyncToNotionRepository : ISyncToNotionRepository
    {
        private readonly AnimeShowContext _animeShowContext;

        public SyncToNotionRepository(AnimeShowContext animeShowContext)
        {
            _animeShowContext = animeShowContext;
        }
        
        /// <summary>
        /// Retrieves all the anime to sync based on action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<List<NotionSync>> GetNotionSync(string action)
        {
            switch (action)
            {
                case "Add":
                    // Gets all the Add syncs and Edit not present in Notion
                    return await _animeShowContext.NotionSyncs
                        .Include(x => x.AnimeShow)
                        .Include(x => x.AnimeShow.AnimeShowProgress)
                        .Include(x => x.AnimeShow.GenreOnAnimeShows)
                        .Include(x => x.AnimeShow.StudioOnAnimeShows)
                        .AsSplitQuery()
                        .Where(x => x.ToSync && (x.Action == "Add" || (x.Action == "Edit" && x.NotionPageId == null)) )
                        .ToListAsync();
                case "Edit":
                    // Gets all the Edit syncs that are present in Notion
                    return await _animeShowContext.NotionSyncs
                        .Include(x => x.AnimeShow)
                        .Include(x => x.AnimeShow.AnimeShowProgress)
                        .Include(x => x.AnimeShow.GenreOnAnimeShows)
                        .Include(x => x.AnimeShow.StudioOnAnimeShows)
                        .AsSplitQuery()
                        .Where(x => x.ToSync && x.Action == "Edit" && x.NotionPageId != null)
                        .ToListAsync();
                default:
                    return await _animeShowContext.NotionSyncs
                        .Include(x => x.AnimeShow)
                        .Include(x => x.AnimeShow.AnimeShowProgress)
                        .Include(x => x.AnimeShow.GenreOnAnimeShows)
                        .Include(x => x.AnimeShow.StudioOnAnimeShows)
                        .AsSplitQuery()
                        .Where(x => x.ToSync && x.Action == action)
                        .ToListAsync();
            }            
        } 

        /// <summary>
        /// Creates NotionSync for every anime that's missing
        /// </summary>
        /// <returns></returns>
        public async Task CreateNotionSyncs()
        {
            List<Guid> animeIds = await _animeShowContext.AnimeShows.Include(x => x.NotionSync).Where(x => x.NotionSync == null).Select(x => x.Id).ToListAsync();

            List<NotionSync> syncs = new List<NotionSync>();
            foreach(var id in animeIds)
            {
                syncs.Add(new NotionSync()
                {
                    AnimeShowId = id,
                    ToSync = false,
                    Action = string.Empty
                });
            }

            await _animeShowContext.NotionSyncs.AddRangeAsync(syncs);
            await _animeShowContext.SaveChangesAsync();
        }

        /// <summary>
        /// Creates single NotionSync for an anime
        /// </summary>
        /// <param name="anime"></param>
        /// <returns></returns>
        public async Task CreateNotionSync(AnimeShow anime, string notionPageId)
        {
            await _animeShowContext.AddAsync(new NotionSync()
            {
                AnimeShowId = anime.Id,
                ToSync = false,
                Action = string.Empty,
                NotionPageId = notionPageId
            });
            await _animeShowContext.SaveChangesAsync();
        }

        /// <summary>
        /// Creates and set to Add a NotionSync
        /// </summary>
        /// <param name="anime"></param>
        /// <returns></returns>
        public async Task AddNotionSync(AnimeShow anime)
        {
            await _animeShowContext.AddAsync(new NotionSync()
            {
                AnimeShowId = anime.Id,
                ToSync = true,
                Action = "Add"
            });
            await _animeShowContext.SaveChangesAsync();
        }

        /// <summary>
        /// Set a NotionSync to be synced based on action
        /// </summary>
        /// <param name="animeId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task SetToSyncNotion(Guid animeId, string action)
        {
            await _animeShowContext.NotionSyncs.Where(x => x.AnimeShowId == animeId).ExecuteUpdateAsync(x => x
                .SetProperty(a => a.ToSync, a => true)
                .SetProperty(a => a.Action, a => action)
            );
        }

        /// <summary>
        /// Set the passed NotionSync to Added
        /// </summary>
        /// <param name="added"></param>
        /// <param name="notionPages"></param>
        /// <returns></returns>
        public async Task SetAdded(List<NotionSync> added, Dictionary<int, string> notionPages)
        {
            foreach(var sync in await _animeShowContext.NotionSyncs.Where(x => added.Contains(x)).ToListAsync())
            {
                sync.ToSync = false;
                sync.Action = string.Empty;
                sync.LastModified = DateTime.Now;
                sync.NotionPageId = notionPages[sync.Id];
                sync.Error = null;
            }
            await _animeShowContext.SaveChangesAsync();
        }

        /// <summary>
        /// Set the passed NotionSync to Edited
        /// </summary>
        /// <param name="edited"></param>
        /// <returns></returns>
        public async Task SetEdited(List<NotionSync> edited)
        {
            await _animeShowContext.NotionSyncs.Where(x => edited.Contains(x)).ExecuteUpdateAsync(x => x
                .SetProperty(a => a.ToSync, a => false)
                .SetProperty(a => a.Action, a => string.Empty)
                .SetProperty(a => a.LastModified, a => DateTime.Now)
                .SetProperty(a => a.Error, a => string.Empty)
            );
        }

        /// <summary>
        /// Set the passed NotionSync to Deleted
        /// </summary>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public async Task SetDeleted(List<NotionSync> deleted)
        {
            await _animeShowContext.NotionSyncs.Where(x => deleted.Contains(x)).ExecuteDeleteAsync();
        }

        /// <summary>
        /// Set the error message in case of exception
        /// </summary>
        /// <param name="inError"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SetError(NotionSync inError, string message)
        {
            await _animeShowContext.NotionSyncs.Where(x => x.Id == inError.Id).ExecuteUpdateAsync(x => x
                .SetProperty(a => a.Error, a => message)
            );
        }

        /// <summary>
        /// Retrieves the year notion page Id
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<string> GetYear(int year)
        {
            return await _animeShowContext.Years.AsNoTracking().Where(x => x.YearValue == year).Select(x => x.NotionPageId).SingleOrDefaultAsync();
        }

    }
}
