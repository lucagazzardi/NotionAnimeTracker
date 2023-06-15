﻿// <auto-generated />
using System;
using Data_AnimeToNotion.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    [DbContext(typeof(AnimeShowContext))]
    [Migration("20230610192453_EDIT_RemovedKeys")]
    partial class EDIT_RemovedKeys
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.AnimeShow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<string>("Cover")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(9);

                    b.Property<int?>("Episodes")
                        .HasColumnType("int")
                        .HasColumnOrder(6);

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(5);

                    b.Property<int>("MalId")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(4);

                    b.Property<string>("NameOriginal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(3);

                    b.Property<Guid?>("NoteId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(13);

                    b.Property<string>("NotionPageId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(1);

                    b.Property<Guid>("SagaId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(10);

                    b.Property<Guid?>("ScoreId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(11);

                    b.Property<DateTime?>("StartedAiring")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(8);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(7);

                    b.Property<Guid?>("WatchingTimeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(12);

                    b.HasKey("Id");

                    b.HasIndex("NoteId")
                        .IsUnique()
                        .HasFilter("[NoteId] IS NOT NULL");

                    b.HasIndex("SagaId");

                    b.HasIndex("ScoreId")
                        .IsUnique()
                        .HasFilter("[ScoreId] IS NOT NULL");

                    b.HasIndex("WatchingTimeId")
                        .IsUnique()
                        .HasFilter("[WatchingTimeId] IS NOT NULL");

                    b.ToTable("AnimeShow", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(2);

                    b.Property<int>("MalId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("Genre", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.GenreOnAnimeShow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<Guid>("AnimeShowId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(1);

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.HasIndex("AnimeShowId");

                    b.HasIndex("GenreId");

                    b.ToTable("GenreOnAnimeShow", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Note", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("Note", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Saga", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("Saga", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<bool>("Favorite")
                        .HasColumnType("bit")
                        .HasColumnOrder(3);

                    b.Property<int>("MalScore")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int?>("PersonalScore")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.ToTable("Score", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Studio", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(2);

                    b.Property<int>("MalId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("Studio", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.StudioOnAnimeShow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<Guid>("AnimeShowId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(1);

                    b.Property<Guid>("StudioId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.HasIndex("AnimeShowId");

                    b.HasIndex("StudioId");

                    b.ToTable("StudioOnAnimeShow", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.WatchingTime", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<int>("CompletedYear")
                        .HasColumnType("int")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("FinishedOn")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(2);

                    b.Property<DateTime>("StartedOn")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("WatchingTime", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.AnimeShow", b =>
                {
                    b.HasOne("Data_AnimeToNotion.DataModel.Note", "Note")
                        .WithOne("AnimeShow")
                        .HasForeignKey("Data_AnimeToNotion.DataModel.AnimeShow", "NoteId");

                    b.HasOne("Data_AnimeToNotion.DataModel.Saga", "Saga")
                        .WithMany("AnimeShows")
                        .HasForeignKey("SagaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_AnimeToNotion.DataModel.Score", "Score")
                        .WithOne("AnimeShow")
                        .HasForeignKey("Data_AnimeToNotion.DataModel.AnimeShow", "ScoreId");

                    b.HasOne("Data_AnimeToNotion.DataModel.WatchingTime", "WatchingTime")
                        .WithOne("AnimeShow")
                        .HasForeignKey("Data_AnimeToNotion.DataModel.AnimeShow", "WatchingTimeId");

                    b.Navigation("Note");

                    b.Navigation("Saga");

                    b.Navigation("Score");

                    b.Navigation("WatchingTime");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.GenreOnAnimeShow", b =>
                {
                    b.HasOne("Data_AnimeToNotion.DataModel.AnimeShow", "AnimeShow")
                        .WithMany("GenreOnAnimeShows")
                        .HasForeignKey("AnimeShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_AnimeToNotion.DataModel.Genre", "Genre")
                        .WithMany("GenreOnAnimeShows")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnimeShow");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.StudioOnAnimeShow", b =>
                {
                    b.HasOne("Data_AnimeToNotion.DataModel.AnimeShow", "AnimeShow")
                        .WithMany("StudioOnAnimeShows")
                        .HasForeignKey("AnimeShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_AnimeToNotion.DataModel.Studio", "Studio")
                        .WithMany("StudioOnAnimeShows")
                        .HasForeignKey("StudioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnimeShow");

                    b.Navigation("Studio");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.AnimeShow", b =>
                {
                    b.Navigation("GenreOnAnimeShows");

                    b.Navigation("StudioOnAnimeShows");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Genre", b =>
                {
                    b.Navigation("GenreOnAnimeShows");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Note", b =>
                {
                    b.Navigation("AnimeShow")
                        .IsRequired();
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Saga", b =>
                {
                    b.Navigation("AnimeShows");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Score", b =>
                {
                    b.Navigation("AnimeShow")
                        .IsRequired();
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Studio", b =>
                {
                    b.Navigation("StudioOnAnimeShows");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.WatchingTime", b =>
                {
                    b.Navigation("AnimeShow")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
