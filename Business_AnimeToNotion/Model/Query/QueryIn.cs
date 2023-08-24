using Business_AnimeToNotion.QueryLogic.SortLogic;

namespace Business_AnimeToNotion.Model.Query
{
    public class QueryIn
    {
        public FilterIn filters { get; set; }
        public SortIn? sort { get; set; }
        public PageIn page { get; set; }
    }
}
