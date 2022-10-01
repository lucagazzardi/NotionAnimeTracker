using Business_AnimeToNotion.Model;
using Notion.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Main_Integration.Interfaces
{
    public interface INotion_Integration
    {
        public Task Notion_CreateNewEntry(MAL_AnimeModel animeModel);

        public Task<List<Notion_LatestAddedModel>> Notion_GetLatestAdded();

        public Task<Dictionary<string, string>> Notion_UpdateProperties(List<string> propertiesToUpate);

        public Task<List<Notion_RatingsUpdate>> GetRatingsToUpdate();
    }
}
