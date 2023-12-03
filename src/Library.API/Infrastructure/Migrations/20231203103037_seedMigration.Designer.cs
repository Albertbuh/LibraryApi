﻿// <auto-generated />
using System;
using Library.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Library.API.Infrastructure.Migrations
{
    [DbContext(typeof(LibraryContext))]
    [Migration("20231203103037_seedMigration")]
    partial class seedMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.Property<int>("AuthorsId")
                        .HasColumnType("int");

                    b.Property<int>("BooksId")
                        .HasColumnType("int");

                    b.HasKey("AuthorsId", "BooksId");

                    b.HasIndex("BooksId");

                    b.ToTable("AuthorBook");
                });

            modelBuilder.Entity("BookGenre", b =>
                {
                    b.Property<int>("BooksId")
                        .HasColumnType("int");

                    b.Property<int>("GenresId")
                        .HasColumnType("int");

                    b.HasKey("BooksId", "GenresId");

                    b.HasIndex("GenresId");

                    b.ToTable("BookGenre");
                });

            modelBuilder.Entity("Library.API.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Authors", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "Пушкин",
                            LastName = "Александр",
                            MiddleName = "Сергеевич"
                        },
                        new
                        {
                            Id = 2,
                            FirstName = "Достоевский",
                            LastName = "Фёдор"
                        },
                        new
                        {
                            Id = 3,
                            FirstName = "Кинг",
                            LastName = "Стивен"
                        },
                        new
                        {
                            Id = 4,
                            FirstName = "Лермонтов",
                            LastName = "Михаил"
                        });
                });

            modelBuilder.Entity("Library.API.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Books", (string)null);
                });

            modelBuilder.Entity("Library.API.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Genres", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "комедия"
                        },
                        new
                        {
                            Id = 2,
                            Name = "драма"
                        },
                        new
                        {
                            Id = 3,
                            Name = "трагедия"
                        },
                        new
                        {
                            Id = 4,
                            Name = "ужасы"
                        });
                });

            modelBuilder.Entity("Library.API.Models.TakenBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("DateOfReturn")
                        .HasColumnType("date");

                    b.Property<DateOnly>("DateOfTaken")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("TakenBooks", (string)null);
                });

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.HasOne("Library.API.Models.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.API.Models.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookGenre", b =>
                {
                    b.HasOne("Library.API.Models.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.API.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Library.API.Models.TakenBook", b =>
                {
                    b.HasOne("Library.API.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });
#pragma warning restore 612, 618
        }
    }
}
