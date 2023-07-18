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
    [Migration("20230714145020_Edit_Year_ToInt")]
    partial class Edit_Year_ToInt
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
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cover")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Episodes")
                        .HasColumnType("int");

                    b.Property<bool>("Favorite")
                        .HasColumnType("bit");

                    b.Property<string>("Format")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MalId")
                        .HasColumnType("int");

                    b.Property<string>("NameDefault")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameJapanese")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("NoteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NotionPageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("PlanToWatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid?>("ScoreId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("StartedAiring")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("WatchingTimeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MalId")
                        .IsUnique();

                    b.HasIndex("NoteId")
                        .IsUnique()
                        .HasFilter("[NoteId] IS NOT NULL");

                    b.HasIndex("NotionPageId")
                        .IsUnique()
                        .HasFilter("[NotionPageId] IS NOT NULL");

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
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("Note", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Relation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<int>("AnimeRelatedMalId")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<Guid>("AnimeShowId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(1);

                    b.Property<string>("Cover")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelationType")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnOrder(3);

                    b.HasKey("Id");

                    b.HasIndex("AnimeShowId");

                    b.ToTable("Relation", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

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

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.SyncToNotionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MalId")
                        .HasColumnType("int");

                    b.Property<string>("Result")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SyncToNotionLog", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.WatchingTime", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("FinishedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("WatchingTime", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Year", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NotionPageId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("YearValue")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Year", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.AnimeShow", b =>
                {
                    b.HasOne("Data_AnimeToNotion.DataModel.Note", "Note")
                        .WithOne("AnimeShow")
                        .HasForeignKey("Data_AnimeToNotion.DataModel.AnimeShow", "NoteId");

                    b.HasOne("Data_AnimeToNotion.DataModel.Score", "Score")
                        .WithOne("AnimeShow")
                        .HasForeignKey("Data_AnimeToNotion.DataModel.AnimeShow", "ScoreId");

                    b.HasOne("Data_AnimeToNotion.DataModel.WatchingTime", "WatchingTime")
                        .WithOne("AnimeShow")
                        .HasForeignKey("Data_AnimeToNotion.DataModel.AnimeShow", "WatchingTimeId");

                    b.Navigation("Note");

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
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AnimeShow");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Relation", b =>
                {
                    b.HasOne("Data_AnimeToNotion.DataModel.AnimeShow", "AnimeShow")
                        .WithMany("Relations")
                        .HasForeignKey("AnimeShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnimeShow");
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
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AnimeShow");

                    b.Navigation("Studio");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.AnimeShow", b =>
                {
                    b.Navigation("GenreOnAnimeShows");

                    b.Navigation("Relations");

                    b.Navigation("StudioOnAnimeShows");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Genre", b =>
                {
                    b.Navigation("GenreOnAnimeShows");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Note", b =>
                {
                    b.Navigation("AnimeShow");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Score", b =>
                {
                    b.Navigation("AnimeShow");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Studio", b =>
                {
                    b.Navigation("StudioOnAnimeShows");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.WatchingTime", b =>
                {
                    b.Navigation("AnimeShow");
                });
#pragma warning restore 612, 618
        }
    }
}
