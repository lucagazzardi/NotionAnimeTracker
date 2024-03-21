using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.DataModel
{
    public class Year
    {
        public int Id { get; set; }
        public string NotionPageId { get; set; }
        public int YearValue { get; set; }
    }
}
