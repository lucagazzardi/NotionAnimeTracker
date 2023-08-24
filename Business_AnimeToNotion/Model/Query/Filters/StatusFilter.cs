using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Model.Query.Filter
{
    public class StatusFilter : Filter
    {
        public override FilterType Type => FilterType.Status;
        public string Term { get; set; }

        public override IQueryable<AnimeShow> ApplyFilter(IQueryable<AnimeShow> data)
        {
            return data.Where(x => x.Status == Term);
        }
    }
}
