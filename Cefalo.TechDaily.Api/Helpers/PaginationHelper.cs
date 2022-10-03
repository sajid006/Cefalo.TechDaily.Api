using Cefalo.TechDaily.Api.Filter;
using Cefalo.TechDaily.Api.Wrappers;
using Cefalo.TechDaily.Service.Contracts;

namespace Cefalo.TechDaily.Api.Helpers
{

    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            var respose = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? uriService.GetPageUri(validFilter.PageNumber + 1, validFilter.PageSize, route)
                : null;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUri(validFilter.PageNumber - 1, validFilter.PageSize, route)
                : null;
            respose.FirstPage = uriService.GetPageUri(1, validFilter.PageSize, route);
            respose.LastPage = uriService.GetPageUri(roundedTotalPages, validFilter.PageSize, route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}
