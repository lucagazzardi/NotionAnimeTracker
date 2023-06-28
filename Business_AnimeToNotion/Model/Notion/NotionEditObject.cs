using Business_AnimeToNotion.Model.Common;
using Business_AnimeToNotion.Model.Notion.Base;

namespace Business_AnimeToNotion.Model.Notion
{
    public class NotionEditObject : NotionBaseObject
    {
        public bool NextToWatch { get; set; }
        public string? Status { get; set; }
        public int PersonalScore { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime FinishedOn { get; set;}
        public bool Favorite { get; set; }
        public string? Notes { get; set; }
        public CompletedYear? CompletedYear { get; set; }
    }
}
