﻿using Business_AnimeToNotion.Model;
using Notion.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Main_Integration.Interfaces
{
    public interface INotion_Integration
    {
        //public Task Notion_CreateNewEntry(MAL_AnimeModel animeModel);

        public Task FromNotionToDB(string cursor);
    }
}
