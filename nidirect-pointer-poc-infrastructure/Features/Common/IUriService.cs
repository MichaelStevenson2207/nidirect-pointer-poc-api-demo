namespace nidirect_pointer_poc_infrastructure.Features.Common
{
    public interface IUriService
    {
        Uri CreateNextPageUri(PaginationDetails paginationDetails);
        Uri CreatePreviousPageUri(PaginationDetails paginationDetails);
    }
}