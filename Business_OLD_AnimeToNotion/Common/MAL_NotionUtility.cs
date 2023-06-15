using Business_AnimeToNotion.Main_Integration;
using Business_AnimeToNotion.Model;
using Business_AnimeToNotion.Profile.AutoMapperConfig;
using Notion.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_AnimeToNotion.Common
{
    public static class MAL_NotionUtility
    {
        public static string Property_Type(string MAL_Type)
        {
            switch (MAL_Type)
            {
                case MAL_Properties_Mapping.TV:
                    return Notion_Properties_Mapping.TV;
                case MAL_Properties_Mapping.Movie:
                    return Notion_Properties_Mapping.Movie;
                case MAL_Properties_Mapping.Special:
                    return Notion_Properties_Mapping.Special;
                case MAL_Properties_Mapping.OVA:
                    return Notion_Properties_Mapping.OVA;
                default:
                    return Notion_Properties_Mapping.TV;
            }
        }
        public static string Property_MAL_Link(string MAL_Id)
        {
            return $"{MAL_Properties_Mapping.Base_MAL_URL}{MAL_Id}";
        }

        public static bool Equals(MAL_AnimeModel MAL_Entry, Page Notion_Entry, out Dictionary<string, PropertyValue> differences)
        {
            differences = new Dictionary<string, PropertyValue>();
            bool isEquals = true;

            // Map the MAL Anime to Notion
            var Mapped_MAL_Entry = MappingHandler.Mapper.Map<Dictionary<string, PropertyValue>>(MAL_Entry);
            var MAL_CompareModel = MappingHandler.Mapper.Map<CompareAnimeModel>(Mapped_MAL_Entry);
            
            // Map the Notion entry to the right fields
            var Notion_CompareModel = MappingHandler.Mapper.Map<CompareAnimeModel>(Notion_Entry.Properties);

            // For every Notion property
            foreach (KeyValuePair<string, PropertyValue> property in Notion_CompareModel.CompareProperties)
            {
                var MAL_Field = MappingHandler.Mapper.Map<string>(property.Value);
                var Notion_Field = MappingHandler.Mapper.Map<string>(MAL_CompareModel.CompareProperties[property.Key]);

                // Map the PropertyValue to a string and compare the values
                if (!string.Equals(MAL_Field, Notion_Field))
                {
                    isEquals = false;
                    differences.Add(property.Key, Mapped_MAL_Entry[property.Key]);
                }
            }

            // Return if equals or not
            // If not returns a list of properties that are not equals
            return isEquals;
        }
    }
}
