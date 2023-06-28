using Data_AnimeToNotion.DataModel;

namespace Data_AnimeToNotion.Repository
{
    public interface ISyncToNotionRepository
    {
        Task Add(SyncToNotionLog log);
        Task UpdateNotionPageId(int malId, string notionPageId);
    }
}
