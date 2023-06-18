using Business_AnimeToNotion.Model.MAL;

namespace Business_AnimeToNotion.MAL
{
    public interface IMAL_Integration
    {
        Task<MAL_AnimeShow> MAL_SearchAnimeByIdAsync(int id);
        Task<List<MAL_AnimeShow>> MAL_SearchAnimeByNameAsync(string searchTerm);
    }
}
