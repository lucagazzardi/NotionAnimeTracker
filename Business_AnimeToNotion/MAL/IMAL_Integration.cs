using Business_AnimeToNotion.Model;

namespace Business_AnimeToNotion.MAL
{
    public interface IMAL_Integration
    {
        Task<MAL_AnimeModel> MAL_SearchAnimeByIdAsync(int id);
        Task<List<MAL_AnimeModel>> MAL_SearchAnimeByNameAsync(string searchTerm);
    }
}
