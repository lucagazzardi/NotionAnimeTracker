using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.QueryLogic.SortLogic
{
    public interface ISortManager
    {
        IQueryable<AnimeShow> ApplySort(IQueryable<AnimeShow> data, SortIn? sort);
    }
}
