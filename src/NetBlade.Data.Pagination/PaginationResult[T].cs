using NetBlade.Core.Pagination;
using System.Collections.Generic;

namespace NetBlade.Data.Pagination
{
    public class PaginationResult<T> : PaginationResult, IPaginationResult<T>
    {
        public PaginationResult()
            : base(null, 1, 50, "ASC")
        {
        }

        public PaginationResult(IPaginationResult page)
            : this(null, page?.SortField, page?.ActualPage ?? 1, page?.PageSize ?? 50, 0, page?.SortDirection)
        {
        }

        public PaginationResult(List<T> entities, string sortField, int actualPage = 1, int pageSize = 50, int rowsCount = 0, string sortDirection = "ASC")
            : base(sortField, actualPage, pageSize, sortDirection)
        {
            this.Entities = entities;
            this.RowsCount = rowsCount;
        }

        public List<T> Entities { get; set; }

        public int RowsCount { get; set; }
    }
}