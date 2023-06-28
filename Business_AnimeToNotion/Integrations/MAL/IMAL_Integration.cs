using Business_AnimeToNotion.Model.Entities;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.MAL;
using JikanDotNet;

namespace Business_AnimeToNotion.Integrations.MAL
{
    public interface IMAL_Integration
    {
        Task<List<INT_AnimeShowBase>> GetSeasonalAnimeShow(int year, Season season);
        Task<List<INT_AnimeShowBase>> SearchAnimeByName(string searchTerm);
        Task<INT_AnimeShowFull> SearchAnimeById(int malId);
    }
}
