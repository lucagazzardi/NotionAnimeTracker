using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class AnimeShow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MalId { get; set; }
        public string NameDefault { get; set; }
        public string NameEnglish { get; set; }
        public string NameJapanese { get; set; }
        public string? Format { get; set; }
        public int? Episodes { get; set; }
        public string Status { get; set; }
        public bool PlanToWatch { get; set; }
        public bool Favorite { get; set; }
        public DateTime? StartedAiring { get; set; }            
        public string Cover { get; set; }
        public int? Score { get; set; }
        public DateTime AddedOn { get; set; }

        public AnimeShowProgress AnimeShowProgress { get; set; }
        public ICollection<AnimeEpisode> AnimeEpisodes { get; set; }
        public ICollection<GenreOnAnimeShow> GenreOnAnimeShows { get; set; }
        public ICollection<StudioOnAnimeShow> StudioOnAnimeShows { get; set; }
        public NotionSync NotionSync { get; set; }
    }
}
