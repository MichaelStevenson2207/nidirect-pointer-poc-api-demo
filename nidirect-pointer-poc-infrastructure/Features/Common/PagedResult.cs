namespace nidirect_pointer_poc_infrastructure.Features.Common;

public class PagedResult<T>
{
    public IEnumerable<T>? Data { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? NextPage { get; set; }
    public string? PreviousPage { get; set; }
    public int? Total { get; set; }

    public static PagedResult<T> CreatePaginatedResponse(IUriService uriService, PaginationDetails paginationDetails, IReadOnlyList<T> response)
    {
        var nextPage = paginationDetails.PageNumber >= 1
            ? uriService.CreateNextPageUri(paginationDetails).ToString()
            : null;

        var previousPage = paginationDetails.PreviousPageNumber >= 0
            ? uriService.CreatePreviousPageUri(paginationDetails).ToString()
            : null;

        return new PagedResult<T>
        {
            Data = response,
            PageNumber = paginationDetails.PageNumber >= 1 ? paginationDetails.PageNumber : null,
            PageSize = paginationDetails.PageSize >= 1 ? paginationDetails.PageSize : null,
            NextPage = (response.Any() ? nextPage : null)!,
            PreviousPage = previousPage!,
            Total = paginationDetails.Total
        };
    }
}