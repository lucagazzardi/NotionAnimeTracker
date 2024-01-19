using Data_AnimeToNotion.DataModel;

namespace Data_AnimeToNotion.Repository
{
    public interface ISyncToNotionRepository
    {
        Task<List<NotionSync>> GetNotionSync(string action);
        Task<List<NotionSync>> GetMalListToUpdate(params string[] actions);
        Task AddNotionSync(AnimeShow anime);
        Task CreateNotionSyncs();
        Task CreateNotionSync(AnimeShow anime, string notionPageId);
        Task SetToSyncNotion(Guid animeId, string action, bool malListToSync = true);
        Task SetAdded(List<NotionSync> added, Dictionary<int, string> notionPages);
        Task SetEdited(List<NotionSync> edited);
        Task SetDeleted(List<NotionSync> deleted);
        Task SetError(NotionSync inError, string message);
        Task SetMalListSynced(int id);
        Task SetMalListError(int id, string message);
        Task<string> GetYear(int year);
    }
}
