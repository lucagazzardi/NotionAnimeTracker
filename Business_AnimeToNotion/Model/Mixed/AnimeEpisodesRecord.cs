using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Mixed
{
    public class AnimeEpisodesRecord
    {
        public int AnimeShowId { get; set; }
        public List<AnimeSingleEpisode> Episodes { get; set; }
    }

    public class AnimeSingleEpisode
    {       
        public string TitleEnglish { get; set; }
        public string TitleJapanese { get; set; }
        public int EpisodeNumber { get; set; }
        public int? EpisodeId { get; set; }
        public DateTime? WatchedOn { get; set; } 
    }
}
