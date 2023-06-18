using Business_AnimeToNotion.Model.Notion.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Notion
{
    public class NotionEditObject : NotionBaseObject
    {
        public bool NextToWatch { get; set; }
        public int PersonalScore { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime FinishedOn { get; set;}
        public bool Favorite { get; set; }
        public string Notes { get; set; }

        //TODO: probably i'll need to modify this type later
        public int CompletedYear { get; set; }

    }
}
