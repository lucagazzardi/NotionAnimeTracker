using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Model.Query.Filter
{
    public class PersonalScoreFilter : Filter
    {
        public override FilterType Type => FilterType.PersonalScore;
        public int Term { get; set; }
        public int RangeTerm { get; set; }

        public override IQueryable<AnimeShow> ApplyFilter(IQueryable<AnimeShow> data)
        {
            return data.Where(x => x.AnimeShowProgress.PersonalScore >= Term && x.AnimeShowProgress.PersonalScore <= RangeTerm);
        }
    }
}
