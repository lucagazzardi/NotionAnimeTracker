using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    [Index(nameof(MalId), IsUnique = true)]
    [Index(nameof(NotionPageId), IsUnique = true)]
    public class AnimeShow
    {
        [Column(Order = 0)]
        public Guid Id { get; set; }

        [Column(Order = 1)]
        public string NotionPageId { get; set; }

        [Column(Order = 2)]
        public int MalId { get; set; }

        [Column(Order = 3)]
        public string NameOriginal { get; set; }

        [Column(Order = 4)]
        public string NameEnglish { get; set; }

        [Column(Order = 5)]
        public string Format { get; set; }

        [Column(Order = 6)]
        public int? Episodes { get; set; }

        [Column(Order = 7)]
        public string Status { get; set; }

        [Column(Order = 8)]
        public DateTime? StartedAiring { get; set; }

        [Column(Order = 9)]
        public string Cover { get; set; }

        [Column(Order = 10)]
        public Guid? ScoreId { get; set; }

        [Column(Order = 11)]
        public Guid? WatchingTimeId { get; set; }

        [Column(Order = 12)]
        public Guid? NoteId { get; set; }


        public ICollection<GenreOnAnimeShow> GenreOnAnimeShows { get; set; }
        public ICollection<StudioOnAnimeShow> StudioOnAnimeShows { get; set; }
        public ICollection<Relation> Relations { get; set; }
        public Score Score { get; set; }
        public WatchingTime WatchingTime { get; set; }
        public Note Note { get; set; }
    }
}
