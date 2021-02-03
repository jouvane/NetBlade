using NetBlade.Core.Pagination;

namespace NetBlade.Data.Pagination
{
    public class PaginationResult : IPaginationResult
    {
        public PaginationResult()
            : this(null)
        {
        }

        public PaginationResult(IPaginationResult page)
            : this(page?.SortField, page?.ActualPage ?? 1, page?.PageSize ?? 50, page?.SortDirection ?? "ASC")
        {
        }

        public PaginationResult(string sortField, int actualPage = 1, int pageSize = 50, string sortDirection = "ASC")
        {
            this.ActualPage = actualPage;
            this.PageSize = pageSize;
            this.SortDirection = sortDirection;
            this.SortField = sortField;
        }

        public int ActualPage { get; set; }

        public int PageSize { get; set; }

        public string SortDirection { get; set; }

        public string SortField { get; set; }

        public static PaginationResult<TEntitie> Cast<TEntitie>(IPaginationResult pagination, int rowsCount = 0)
        {
            return new PaginationResult<TEntitie>(null, pagination.SortField, pagination.ActualPage, pagination.PageSize, rowsCount, pagination.SortDirection);
        }
    }
}
