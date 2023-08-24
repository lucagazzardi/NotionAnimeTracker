using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Model.Query.Filter
{
    public class SearchFilter : Filter
    {
        public override FilterType Type => FilterType.Search;
        public string Term { get; set; }

        public override IQueryable<AnimeShow> ApplyFilter(IQueryable<AnimeShow> data)
        {
            return data.Where(x => x.NameEnglish.Contains(Term) || x.NameDefault.Contains(Term));
        }
    }
}
