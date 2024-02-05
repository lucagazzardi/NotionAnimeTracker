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
    [Migration("20240119140735_ADD_MALLISTERRORS")]
    partial class ADD_MALLISTERRORS
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

                    b.Property<bool>("PlanToWatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int?>("Score")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartedAiring")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("MalId")
                        .IsUnique();

                    b.HasIndex("Status");

                    b.ToTable("AnimeShow", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.AnimeShowProgress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnimeShowId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CompletedYear")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FinishedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PersonalScore")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AnimeShowId")
                        .IsUnique();

                    b.ToTable("AnimeShowProgress");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Genre", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genre", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.GenreOnAnimeShow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnimeShowId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("AnimeShowId", "GenreId")
                        .IsUnique();

                    b.ToTable("GenreOnAnimeShow", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.MalSyncErrors", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("AnimeShowId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnimeShowId");

                    b.HasIndex("MalId");

                    b.ToTable("MalSyncError", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.NotionSync", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("AnimeShowId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<bool>("MalListToSync")
                        .HasColumnType("bit");

                    b.Property<string>("NotionPageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("ToSync")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AnimeShowId")
                        .IsUnique()
                        .HasFilter("[AnimeShowId] IS NOT NULL");

                    b.HasIndex("NotionPageId");

                    b.ToTable("NotionSync", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Relation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AnimeRelatedMalId")
                        .HasColumnType("int");

                    b.Property<Guid>("AnimeShowId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cover")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelationType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AnimeRelatedMalId");

                    b.HasIndex("AnimeShowId");

                    b.ToTable("Relation", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Studio", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Studio", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.StudioOnAnimeShow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnimeShowId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudioId");

                    b.HasIndex("AnimeShowId", "StudioId")
                        .IsUnique();

                    b.ToTable("StudioOnAnimeShow", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Year", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NotionPageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("YearValue")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NotionPageId");

                    b.ToTable("Year", (string)null);
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.AnimeShowProgress", b =>
                {
                    b.HasOne("Data_AnimeToNotion.DataModel.AnimeShow", "AnimeShow")
                        .WithOne("AnimeShowProgress")
                        .HasForeignKey("Data_AnimeToNotion.DataModel.AnimeShowProgress", "AnimeShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnimeShow");
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

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.MalSyncErrors", b =>
                {
                    b.HasOne("Data_AnimeToNotion.DataModel.AnimeShow", "AnimeShow")
                        .WithMany()
                        .HasForeignKey("AnimeShowId");

                    b.Navigation("AnimeShow");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.NotionSync", b =>
                {
                    b.HasOne("Data_AnimeToNotion.DataModel.AnimeShow", "AnimeShow")
                        .WithOne("NotionSync")
                        .HasForeignKey("Data_AnimeToNotion.DataModel.NotionSync", "AnimeShowId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("AnimeShow");
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
                    b.Navigation("AnimeShowProgress");

                    b.Navigation("GenreOnAnimeShows");

                    b.Navigation("NotionSync");

                    b.Navigation("Relations");

                    b.Navigation("StudioOnAnimeShows");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Genre", b =>
                {
                    b.Navigation("GenreOnAnimeShows");
                });

            modelBuilder.Entity("Data_AnimeToNotion.DataModel.Studio", b =>
                {
                    b.Navigation("StudioOnAnimeShows");
                });
#pragma warning restore 612, 618
        }
    }
}