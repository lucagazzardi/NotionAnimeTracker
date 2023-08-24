using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Model.Query.Filter
{
    public class YearFilter : Filter
    {
        public override FilterType Type => FilterType.Year;
        public int Term { get; set; }

        public override IQueryable<AnimeShow> ApplyFilter(IQueryable<AnimeShow> data)
        {
            return data.Where(x => x.AnimeShowProgress.CompletedYear == Term);
        }
    }
}
