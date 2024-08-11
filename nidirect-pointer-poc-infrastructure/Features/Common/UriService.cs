using System.Text;
using Microsoft.AspNetCore.Http;

namespace nidirect_pointer_poc_infrastructure.Features.Common;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(IHttpContextAccessor httpContextAccessor)
    {
        _baseUri = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host.ToUriComponent(), httpContextAccessor.HttpContext.Request.Path);
    }

    public Uri CreateNextPageUri(PaginationDetails paginationDetails) =>
        CreateUriFor(paginationDetails.ApiRoute!, paginationDetails.NextPageNumber,
            paginationDetails.PageSize);

    public Uri CreatePreviousPageUri(PaginationDetails paginationDetails) =>
        CreateUriFor(paginationDetails.ApiRoute!, paginationDetails.PreviousPageNumber,
            paginationDetails.PageSize);

    private Uri CreateUriFor(string route, int pageNumber, int pageSize)
    {
        var builder = new StringBuilder(_baseUri + route);
        builder.Append("?pageNumber=").Append(pageNumber);
        builder.Append("&pageSize=").Append(pageSize);

        if (Uri.TryCreate(builder.ToString(), UriKind.Absolute, out Uri? uri))
        {
            return uri;
        }
        else
        {
            throw new ArgumentException("Invalid Uri detected.");
        }
    }
}