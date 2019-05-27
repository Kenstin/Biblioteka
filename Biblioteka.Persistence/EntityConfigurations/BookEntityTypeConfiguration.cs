using System;
using System.Collections.Generic;
using System.Text;
using Biblioteka.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteka.Persistence.EntityConfigurations
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Title).IsRequired();
            builder.Property(b => b.Author).IsRequired();
            builder.Property(b => b.Publisher).IsRequired();
            builder.HasOne(b => b.Rental).WithOne(r => r.Book).OnDelete(DeleteBehavior.Cascade).HasForeignKey<BookRental>("RentedBookId");
        }
    }
}
