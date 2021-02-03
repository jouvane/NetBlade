using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Movies.Infrastructure.Repository.Context
{
    [ExcludeFromCodeCoverage]
    internal sealed class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoviesDbContext).Assembly);
        }
    }
}
