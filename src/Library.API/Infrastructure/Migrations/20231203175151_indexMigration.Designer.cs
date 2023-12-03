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
    [Migration("20231203175151_indexMigration")]
    partial class indexMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Library.API.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("a_id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("a_firstname")
                        .UseCollation("utf8mb4_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("FirstName"), "utf8mb4");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("a_lastname")
                        .UseCollation("utf8mb4_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("LastName"), "utf8mb4");

                    b.Property<string>("MiddleName")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("a_middlename")
                        .UseCollation("utf8mb4_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("MiddleName"), "utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("FirstName");

                    b.ToTable("authors", (string)null);

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

            modelBuilder.Entity("Library.API.Models.BookEdition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("be_id");

                    b.Property<string>("Description")
                        .HasColumnType("longtext")
                        .HasColumnName("be_description");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("be_isbn")
                        .UseCollation("utf8mb4_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ISBN"), "utf8mb4");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("be_title")
                        .UseCollation("utf8mb4_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Title"), "utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ISBN")
                        .IsUnique();

                    b.ToTable("book_editions", (string)null);
                });

            modelBuilder.Entity("Library.API.Models.BookInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("bi_id");

                    b.Property<int>("BookEditionId")
                        .HasColumnType("int")
                        .HasColumnName("bi_book_edition_id");

                    b.Property<DateOnly?>("DateOfReturn")
                        .HasColumnType("date")
                        .HasColumnName("bi_date_of_return");

                    b.Property<DateOnly?>("DateOfTaken")
                        .HasColumnType("date")
                        .HasColumnName("bi_date_of_taken");

                    b.HasKey("Id");

                    b.HasIndex("BookEditionId");

                    b.ToTable("book_instances", (string)null);
                });

            modelBuilder.Entity("Library.API.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("g_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("g_name");

                    b.HasKey("Id");

                    b.ToTable("genres", (string)null);

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

            modelBuilder.Entity("m2m_editions_authors", b =>
                {
                    b.Property<int>("author_id")
                        .HasColumnType("int");

                    b.Property<int>("edition_id")
                        .HasColumnType("int");

                    b.HasKey("author_id", "edition_id");

                    b.HasIndex("edition_id");

                    b.ToTable("m2m_editions_authors");
                });

            modelBuilder.Entity("m2m_editions_genres", b =>
                {
                    b.Property<int>("genre_id")
                        .HasColumnType("int");

                    b.Property<int>("edition_id")
                        .HasColumnType("int");

                    b.HasKey("genre_id", "edition_id");

                    b.HasIndex("edition_id");

                    b.ToTable("m2m_editions_genres");
                });

            modelBuilder.Entity("Library.API.Models.BookInstance", b =>
                {
                    b.HasOne("Library.API.Models.BookEdition", "Book")
                        .WithMany()
                        .HasForeignKey("BookEditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("m2m_editions_authors", b =>
                {
                    b.HasOne("Library.API.Models.Author", null)
                        .WithMany()
                        .HasForeignKey("author_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.API.Models.BookEdition", null)
                        .WithMany()
                        .HasForeignKey("edition_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("m2m_editions_genres", b =>
                {
                    b.HasOne("Library.API.Models.BookEdition", null)
                        .WithMany()
                        .HasForeignKey("edition_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.API.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("genre_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
