using Data_AnimeToNotion.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.QueryLogic.FilterLogic
{
    public interface IFilterManager
    {
        IQueryable<AnimeShow> ApplyFilters(IQueryable<AnimeShow> data);
    }
}
