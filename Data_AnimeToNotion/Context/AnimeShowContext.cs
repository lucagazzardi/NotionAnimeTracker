using Data_AnimeToNotion.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.Context
{
    public class AnimeShowContext : DbContext
    {
        public AnimeShowContext(DbContextOptions<AnimeShowContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<AnimeShow> AnimeShows { get; set; }
        public DbSet<AnimeShowProgress> AnimeShowProgresses { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreOnAnimeShow> GenreOnAnimeShows { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudioOnAnimeShow> StudioOnAnimeShows { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<AnimeEpisode> AnimeEpisodes { get; set; }

        #region SyncToNotion

        public DbSet<NotionSync> NotionSyncs { get; set; }
        public DbSet<MalSyncError> MalSyncErrors { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnimeShow>().ToTable("AnimeShow");
            modelBuilder.Entity<AnimeShowProgress>().ToTable("AnimeShowProgress");
            modelBuilder.Entity<Genre>().ToTable("Genre");
            modelBuilder.Entity<GenreOnAnimeShow>().ToTable("GenreOnAnimeShow");
            modelBuilder.Entity<Studio>().ToTable("Studio");
            modelBuilder.Entity<StudioOnAnimeShow>().ToTable("StudioOnAnimeShow");
            modelBuilder.Entity<Year>().ToTable("Year");
            modelBuilder.Entity<NotionSync>().ToTable("NotionSync");
            modelBuilder.Entity<MalSyncError>().ToTable("MalSyncError");
            modelBuilder.Entity<AnimeEpisode>().ToTable("AnimeEpisode");


            modelBuilder.Entity<AnimeShow>()
                .Property(t => t.PlanToWatch)
                .HasDefaultValue(false);

            modelBuilder.Entity<AnimeShow>()
                .Property(t => t.AddedOn)
                .HasDefaultValueSql("getdate()");

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

            modelBuilder
                .Entity<AnimeShow>()
                .HasOne(e => e.NotionSync)
                .WithOne(e => e.AnimeShow)
                .OnDelete(DeleteBehavior.SetNull);

            #region Indexes

            modelBuilder.Entity<StudioOnAnimeShow>()
                .HasIndex(e => e.AnimeShowId)
                .IncludeProperties(
                    e => new { e.StudioId, e.Description });

            modelBuilder.Entity<GenreOnAnimeShow>()
                .HasIndex(e => e.AnimeShowId)
                .IncludeProperties(
                    e => new { e.GenreId, e.Description });

            modelBuilder.Entity<AnimeEpisode>()
                .HasIndex(e => e.AnimeShowId)
                .IncludeProperties(
                    e => new { e.EpisodeNumber, e.WatchedOn });

            #endregion
        }
    }
}
