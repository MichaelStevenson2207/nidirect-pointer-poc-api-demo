namespace nidirect_pointer_poc_infrastructure.Features.Common
{
    public class PaginationDetails
    {
        public string? ApiRoute { get; private set; }
        public int PageNumber { get; set; }
        public int PreviousPageNumber
        {
            get
            {
                var prev = PageNumber - 1;
                return prev >= 1 ? prev : 0;
            }
        }
        public int NextPageNumber => PageNumber + 1;
        public int PageSize { get; set; }
        public int Total { get; private set; }

        public PaginationDetails WithTotal(int total)
        {
            Total = total;
            return this;
        }

        internal PaginationDetails WithApiRoute(string route)
        {
            ApiRoute = route;
            return this;
        }
    }
}