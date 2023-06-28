using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    [Index(nameof(MalId), IsUnique = true)]
    [Index(nameof(NotionPageId), IsUnique = true)]
    public class AnimeShow
    {
        public Guid Id { get; set; }
        public string? NotionPageId { get; set; }
        public int MalId { get; set; }
        public string NameDefault { get; set; }
        public string NameEnglish { get; set; }
        public string NameJapanese { get; set; }
        public string? Format { get; set; }
        public int? Episodes { get; set; }
        public string Status { get; set; }
        public DateTime? StartedAiring { get; set; }            
        public string Cover { get; set; }
        public Guid? ScoreId { get; set; }
        public Guid? WatchingTimeId { get; set; }
        public Guid? NoteId { get; set; }


        public ICollection<GenreOnAnimeShow> GenreOnAnimeShows { get; set; }
        public ICollection<StudioOnAnimeShow> StudioOnAnimeShows { get; set; }
        public ICollection<Relation> Relations { get; set; }
        public Score Score { get; set; }
        public WatchingTime WatchingTime { get; set; }
        public Note Note { get; set; }
    }
}
