using Business_AnimeToNotion.QueryLogic.FilterLogic;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Model.Query.Filter
{
    public abstract class Filter
    {
        public virtual FilterType Type { get; set; }

        public abstract IQueryable<AnimeShow> ApplyFilter(IQueryable<AnimeShow> data);
    }
}
