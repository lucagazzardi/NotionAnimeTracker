using Data_AnimeToNotion.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.Context
{
    public class AnimeShowContext : DbContext
    {
        public AnimeShowContext(DbContextOptions<AnimeShowContext> options) : base(options)
        {
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
        public DbSet<Year> Year { get; set; }


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

        }
    }
}
