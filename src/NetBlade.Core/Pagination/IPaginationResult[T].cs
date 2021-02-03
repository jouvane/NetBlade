using System.Collections.Generic;

namespace NetBlade.Core.Pagination
{
    public interface IPaginationResult<T> : IPaginationResult
    {
        List<T> Entities { get; set; }

        int RowsCount { get; set; }
    }
}