using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.MAL;

namespace Business_AnimeToNotion.Integrations.MAL
{
    public interface IMAL_Integration
    {
        Task<List<AnimeShowBase>> GetCurrentSeasonAnimeShow();
        Task<List<AnimeShowBase>> GetUpcomingSeasonAnimeShow();
        Task<List<AnimeShowBase>> SearchAnimeByName(string searchTerm);
        Task<AnimeShowFull> GetAnimeById(int malId);
        Task<AnimeShowFull> GetAnimeById(string header, string key, string url);
        Task<AnimeUpdateStatus> UpdateListStatus(string token, string url, AnimeUpdateStatus item);
        Task DeleteListStatus(string token, string url);
        Task<AnimeSynopsis> GetSynopsisById(int malId);
    }
}
