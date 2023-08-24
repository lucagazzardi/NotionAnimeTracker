using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Model.Query.Filter
{
    public class MALScoreFilter : Filter
    {
        public override FilterType Type => FilterType.MalScore;
        public int Term { get; set; }
        public int RangeTerm { get; set; }

        public override IQueryable<AnimeShow> ApplyFilter(IQueryable<AnimeShow> data)
        {
            return data.Where(x => x.Score >= Term && x.Score <= RangeTerm);
        }
    }
}
