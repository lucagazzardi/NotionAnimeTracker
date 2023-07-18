using Business_AnimeToNotion.Model.Query;
using Business_AnimeToNotion.Model.Query.Filter;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.QueryLogic.FilterLogic
{
    public enum FilterType
    {
        Search,
        Status,
        Format,
        Year,
        FavoritesOnly,
        PlanToWatchOnly,
        MalScore,
        PersonalScore,
        Episodes
    }

    public class FilterManager : IFilterManager
    {
        private readonly IFilterLauncher _filterLauncher;
        public FilterManager(FilterIn filters)
        {
            _filterLauncher = new FilterLauncher();

            if (filters.Search != null)
                _filterLauncher.AddFilter(new SearchFilter() { Term = filters.Search });

            if (filters.Status != null)
                _filterLauncher.AddFilter(new StatusFilter() { Term = filters.Status });

            if (filters.Format != null)
                _filterLauncher.AddFilter(new FormatFilter() { Term = filters.Format });

            if (filters.Year != null)
                _filterLauncher.AddFilter(new YearFilter() { Term = filters.Year.Value });

            if (filters.MalScoreGreater != null && filters.MalScoreLesser != null)
                _filterLauncher.AddFilter(new MALScoreFilter() { Term = filters.MalScoreGreater.Value, RangeTerm = filters.MalScoreLesser.Value });

            if (filters.PersonalScoreGreater != null && filters.PersonalScoreLesser != null)
                _filterLauncher.AddFilter(new PersonalScoreFilter() { Term = filters.PersonalScoreGreater.Value, RangeTerm = filters.PersonalScoreLesser.Value });

            if (filters.EpisodesGreater != null && filters.EpisodesLesser != null)
                _filterLauncher.AddFilter(new EpisodesFilter() { Term = filters.EpisodesGreater.Value, RangeTerm = filters.EpisodesLesser.Value });

            if (filters.FavoritesOnly != null)
                _filterLauncher.AddFilter(new FavoritesOnlyFilter() { Term = filters.FavoritesOnly.Value });

            if (filters.PlanToWatchOnly != null)
                _filterLauncher.AddFilter(new PlanToWatchOnlyFilter() { Term = filters.PlanToWatchOnly.Value });
        }

        public IQueryable<AnimeShow> ApplyFilters(IQueryable<AnimeShow> data)
        {
            return _filterLauncher.FilterLaunch(data);
        }
    }
}
