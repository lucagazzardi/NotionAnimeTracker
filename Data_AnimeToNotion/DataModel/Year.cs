﻿using Microsoft.EntityFrameworkCore;

namespace Data_AnimeToNotion.DataModel
{
    [Index(nameof(NotionPageId))]
    public class Year
    {
        public Guid Id { get; set; }
        public string NotionPageId { get; set; }
        public int YearValue { get; set; }
    }
}
