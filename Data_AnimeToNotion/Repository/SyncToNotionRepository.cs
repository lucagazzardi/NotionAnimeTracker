using Data_AnimeToNotion.Context;
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

        public async Task Add(SyncToNotionLog log)
        {
            await _animeShowContext.SyncToNotionLogs.AddAsync(log);
            await _animeShowContext.SaveChangesAsync();
        }

        public async Task UpdateNotionPageId(int malId, string notionPageId)
        {
            await _animeShowContext.AnimeShows.Where(x => x.MalId == malId).ExecuteUpdateAsync(setters => setters.SetProperty(b => b.NotionPageId, notionPageId));
        }

    }
}
