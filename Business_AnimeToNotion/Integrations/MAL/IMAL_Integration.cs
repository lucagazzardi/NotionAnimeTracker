using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.MAL;
using JikanDotNet;

namespace Business_AnimeToNotion.Integrations.MAL
{
    public interface IMAL_Integration
    {
        Task<List<INT_AnimeShowBase>> GetCurrentSeasonAnimeShow();
        Task<List<INT_AnimeShowBase>> GetUpcomingSeasonAnimeShow();
        Task<List<INT_AnimeShowBase>> SearchAnimeByName(string searchTerm);
        Task<INT_AnimeShowFull> GetAnimeById(int malId);
        Task<INT_AnimeShowFull> GetAnimeById(string header, string key, string url);
        Task<MAL_AnimeUpdateStatus> UpdateListStatus(string token, string url, MAL_AnimeUpdateStatus item);
        Task DeleteListStatus(string token, string url);
        Task<MAL_AnimeShowRelations> GetRelationsFromMAL(int malId);
    }
}
