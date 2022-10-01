using System;
using System.Collections.Generic;
using System.Text;

namespace Business_AnimeToNotion.Model
{
    public class Notion_RatingsUpdate
    {
        public string title { get;set;}
        public decimal oldRating { get; set;}
        public decimal newRating { get; set;}
    }
}
