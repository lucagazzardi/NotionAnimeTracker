using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Model.Query.Filter
{
    public class GenreFilter : Filter
    {
        public override FilterType Type => FilterType.Genre;

        public List<int> Term { get; set; }

        public override IQueryable<AnimeShow> ApplyFilter(IQueryable<AnimeShow> data)
        {
            return data.Where(x => x.GenreOnAnimeShows.Count(g => Term.Contains(g.GenreId)) == Term.Count);
        }
    }
}
