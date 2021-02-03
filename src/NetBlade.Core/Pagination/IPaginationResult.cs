namespace NetBlade.Core.Pagination
{
    public interface IPaginationResult
    {
        int ActualPage { get; set; }

        int PageSize { get; set; }

        string SortDirection { get; set; }

        string SortField { get; set; }
    }
}
