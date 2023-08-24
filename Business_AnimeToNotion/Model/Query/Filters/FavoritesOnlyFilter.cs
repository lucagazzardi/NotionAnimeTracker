using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Model.Query.Filter
{
    public class FavoritesOnlyFilter : Filter
    {
        public override FilterType Type => FilterType.FavoritesOnly;
        public bool Term { get; set; }

        public override IQueryable<AnimeShow> ApplyFilter(IQueryable<AnimeShow> data)
        {
            return data.Where(x => x.Favorite == Term);
        }
    }
}
