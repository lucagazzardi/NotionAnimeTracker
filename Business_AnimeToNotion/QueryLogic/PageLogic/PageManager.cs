using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Pagination;
using Business_AnimeToNotion.Model.Query;
using Data_AnimeToNotion.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Business_AnimeToNotion.QueryLogic.PageLogic
{
    internal class PageManager : IPageManager
    {
        private int _totalCount;
        public PageManager() { }

        public async Task<IQueryable<AnimeShow>> ApplyPaging(IQueryable<AnimeShow> data, PageIn page)
        {
            _totalCount = page.TotalCount ?? await data.CountAsync();
            return data.Skip((page.CurrentPage - 1) * page.PerPage).Take(page.PerPage);
        }

        public PaginatedResponse<AnimeShowFull> GeneratePaginatedResponse(List<AnimeShowFull> data, PageIn page)
        {
            int lastPage = (int)Math.Ceiling(_totalCount / (double)page.PerPage);

            return new PaginatedResponse<AnimeShowFull>()
            {
                PageInfo = new PageInfo()
                {
                    LastPage = lastPage,
                    PerPage = page.PerPage,
                    CurrentPage = page.CurrentPage,
                    TotalCount = _totalCount,
                    HasNextPage = page.CurrentPage < lastPage
                },                
                Data = data
            };

        }
    }
}
