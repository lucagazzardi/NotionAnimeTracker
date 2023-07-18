using Business_AnimeToNotion.Model.Query.Filter;
using Business_AnimeToNotion.QueryLogic.SortLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Query
{
    public class QueryIn
    {
        public FilterIn filters { get; set; }
        public SortIn? sort { get; set; }
        public PageIn page { get; set; }
    }
}
