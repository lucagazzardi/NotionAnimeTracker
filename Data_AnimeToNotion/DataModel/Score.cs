using System.ComponentModel.DataAnnotations.Schema;

namespace Data_AnimeToNotion.DataModel
{
    public class Score
    {
        [Column(Order = 0)]
        public Guid Id { get; set; }

        [Column(Order = 1)]
        public int MalScore { get; set; }

        [Column(Order = 2)]
        public int? PersonalScore { get; set; }


        public AnimeShow AnimeShow { get; set; }
    }
}
