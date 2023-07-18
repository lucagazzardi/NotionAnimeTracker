using Business_AnimeToNotion.Model.Query.Filter;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.QueryLogic.FilterLogic
{
    public class FilterLauncher : IFilterLauncher
    {
        private readonly List<Filter> FilterList = new List<Filter>();

        public FilterLauncher()
        {

        }

        public void AddFilter(Filter filter)
        {
            FilterList.Add(filter);
        }

        public IQueryable<AnimeShow> FilterLaunch(IQueryable<AnimeShow> data)
        {
            foreach (var filter in FilterList)
            {
                data = filter.ApplyFilter(data);
            }

            return data;
        }
    }
}
