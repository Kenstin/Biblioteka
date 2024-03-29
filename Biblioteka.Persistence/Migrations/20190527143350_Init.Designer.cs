﻿// <auto-generated />
using System;
using Biblioteka.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Biblioteka.Persistence.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20190527143350_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("Biblioteka.Domain.Entities.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired();

                    b.Property<bool>("IsAvailable");

                    b.Property<string>("Publisher")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("YearPublished");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Biblioteka.Domain.Entities.BookRental", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("RentDate");

                    b.Property<Guid?>("RentedBookId");

                    b.Property<DateTime>("ReturnDate");

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RentedBookId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("BookRentals");
                });

            modelBuilder.Entity("Biblioteka.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HashedPassword")
                        .IsRequired();

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Biblioteka.Domain.Entities.BookRental", b =>
                {
                    b.HasOne("Biblioteka.Domain.Entities.Book", "Book")
                        .WithOne("Rental")
                        .HasForeignKey("Biblioteka.Domain.Entities.BookRental", "RentedBookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Biblioteka.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
