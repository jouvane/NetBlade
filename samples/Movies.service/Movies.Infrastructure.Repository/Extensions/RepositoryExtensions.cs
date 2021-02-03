using NetBlade.Core.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Movieslections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Movies.Infrastructure.Repository.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class RepositoryExtensions
    {
        public static async Task Page<TResult, TEntity>(this DbSet<TEntity> entitySet, string tableName, string tableMoviesumnId, string whereFilter, Dictionary<string, string> dicSortField, IPaginationResult<TResult> paginationResult, Expression<Func<TEntity, TResult>> selector, params object[] parameters)
            where TEntity : class
        {
            string sortField = (paginationResult.SortField ?? tableMoviesumnId).ToUpper();
            sortField = dicSortField.ContainsKey(sortField) ? dicSortField[sortField] : tableMoviesumnId;

            paginationResult.PageSize = paginationResult.PageSize < 1 ? 10 : paginationResult.PageSize;
            paginationResult.ActualPage = paginationResult.ActualPage < 1 ? 1 : paginationResult.ActualPage;
            paginationResult.SortDirection = (paginationResult.SortDirection ?? "ASC").ToUpper().Equals("ASC") ? "ASC" : "DESC";

            var sql = entitySet
              .FromSqlRaw($@"SELECT * FROM {tableName} WHERE {tableMoviesumnId} IN 
                (
                    SELECT
                        {tableMoviesumnId}
                    FROM
                    (
                        SELECT ROW_NUMBER() OVER(ORDER BY {sortField} {paginationResult.SortDirection}) AS RowNr, {tableMoviesumnId}
                        FROM {tableName}
                        WHERE {whereFilter}
                    ) as TResult 
                    WHERE RowNr > {(paginationResult.ActualPage - 1) * paginationResult.PageSize}
                )", parameters)
              .Take(paginationResult.PageSize)
              .Select(selector);

            paginationResult.Entities = await sql.ToListAsync();
        }
    }
}
