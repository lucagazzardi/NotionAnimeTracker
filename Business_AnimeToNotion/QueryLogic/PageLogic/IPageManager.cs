using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Pagination;
using Business_AnimeToNotion.Model.Query;
using Data_AnimeToNotion.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.QueryLogic.PageLogic
{
    public interface IPageManager
    {
        Task<IQueryable<AnimeShow>> ApplyPaging(IQueryable<AnimeShow> data, PageIn page);
        PaginatedResponse<INT_AnimeShowFull> GeneratePaginatedResponse(List<INT_AnimeShowFull> data, PageIn page);
    }
}
