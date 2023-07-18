using Data_AnimeToNotion.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;

namespace Data_AnimeToNotion.Context
{
    public class AnimeShowContext : DbContext
    {
        public AnimeShowContext(DbContextOptions<AnimeShowContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<AnimeShow> AnimeShows { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreOnAnimeShow> GenreOnAnimeShows { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudioOnAnimeShow> StudioOnAnimeShows { get; set; }
        public DbSet<WatchingTime> WatchingTimes { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<Year> Years { get; set; }

        #region SyncToNotion

        public DbSet<SyncToNotionLog> SyncToNotionLogs { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnimeShow>().ToTable("AnimeShow");
            modelBuilder.Entity<Genre>().ToTable("Genre");
            modelBuilder.Entity<GenreOnAnimeShow>().ToTable("GenreOnAnimeShow");
            modelBuilder.Entity<Note>().ToTable("Note");
            modelBuilder.Entity<Score>().ToTable("Score");
            modelBuilder.Entity<Studio>().ToTable("Studio");
            modelBuilder.Entity<StudioOnAnimeShow>().ToTable("StudioOnAnimeShow");
            modelBuilder.Entity<WatchingTime>().ToTable("WatchingTime");
            modelBuilder.Entity<Relation>().ToTable("Relation");
            modelBuilder.Entity<Year>().ToTable("Year");
            modelBuilder.Entity<SyncToNotionLog>().ToTable("SyncToNotionLog");

            modelBuilder.Entity<AnimeShow>()
                .Property(t => t.PlanToWatch)
                .HasDefaultValue(false);

            modelBuilder
                .Entity<GenreOnAnimeShow>()
                .HasOne(e => e.Genre)
                .WithMany(e => e.GenreOnAnimeShows)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<StudioOnAnimeShow>()
                .HasOne(e => e.Studio)
                .WithMany(e => e.StudioOnAnimeShows)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
