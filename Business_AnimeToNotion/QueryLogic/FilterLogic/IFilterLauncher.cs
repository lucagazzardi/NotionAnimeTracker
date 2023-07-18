using Business_AnimeToNotion.Model.Query.Filter;
using Data_AnimeToNotion.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.QueryLogic.FilterLogic
{
    public interface IFilterLauncher
    {
        void AddFilter(Filter filter);
        IQueryable<AnimeShow> FilterLaunch(IQueryable<AnimeShow> data);
    }
}
