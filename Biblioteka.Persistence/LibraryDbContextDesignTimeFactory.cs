using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Biblioteka.Persistence
{
    public class LibraryDbContextDesignTimeFactory : IDesignTimeDbContextFactory<LibraryDbContext>
    {
        public LibraryDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LibraryDbContext>();
            builder.UseSqlite($"Data Source={Environment.CurrentDirectory}\\database.db;");
            return new LibraryDbContext(builder.Options);
        }
    }
}
