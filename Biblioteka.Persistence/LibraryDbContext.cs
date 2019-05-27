using System.Threading;
using System.Threading.Tasks;
using Biblioteka.Domain.Entities;
using Biblioteka.Domain.SeedWork;
using Biblioteka.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.Persistence
{
    public class LibraryDbContext : DbContext, IUnitOfWork
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookRental> BookRentals { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await SaveChangesAsync(cancellationToken);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
        }
    }
}
