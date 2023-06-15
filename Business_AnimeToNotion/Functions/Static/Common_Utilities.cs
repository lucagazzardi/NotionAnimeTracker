using AutoMapper;
using Business_AnimeToNotion.Mapper;
using Business_AnimeToNotion.Model;
using Notion.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Functions.Static
{
    public static class Common_Utilities
    {
        public static string MALToNotion_PropertyType(string MAL_Type)
        {
            switch (MAL_Type)
            {
                case "tv":
                    return "TV";
                case "ova":
                    return "OVA";
                case "movie":
                    return "Movie";
                case "special":
                    return "Special";
                default:
                    return "TV";
            }
        }

        public static string Property_MAL_Link(string MAL_Id)
        {
            return $"https://myanimelist.net/anime/{MAL_Id}";
        }

        public static bool Equals(MAL_AnimeModel MAL_Entry, Page Notion_Entry, out Dictionary<string, PropertyValue> differences)
        {
            differences = new Dictionary<string, PropertyValue>();
            bool isEquals = true;

            // Map the MAL Anime to Notion
            var Mapped_MAL_Entry = Mapping.Mapper.Map<Dictionary<string, PropertyValue>>(MAL_Entry);
            var MAL_CompareModel = Mapping.Mapper.Map<Compare_AnimeModel>(Mapped_MAL_Entry);

            // Map the Notion entry to the right fields
            var Notion_CompareModel = Mapping.Mapper.Map<Compare_AnimeModel>(Notion_Entry.Properties);

            // For every Notion property
            foreach (KeyValuePair<string, PropertyValue> property in Notion_CompareModel.CompareProperties)
            {
                var MAL_Field = Mapping.Mapper.Map<string>(property.Value);
                var Notion_Field = Mapping.Mapper.Map<string>(MAL_CompareModel.CompareProperties[property.Key]);

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
