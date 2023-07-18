using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Data_AnimeToNotion.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Query.Filter
{
    public class FormatFilter : Filter
    {
        public override FilterType Type => FilterType.Format;
        public string Term { get; set; }

        public override IQueryable<AnimeShow> ApplyFilter(IQueryable<AnimeShow> data)
        {
            return data.Where(x => x.Format == Term);
        }
    }
}
