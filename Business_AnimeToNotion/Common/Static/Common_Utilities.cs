using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.MAL;
using Business_AnimeToNotion.Model.SyncMAL;
using Data_AnimeToNotion.DataModel;
using JikanDotNet;
using Notion.Client;
using System.ComponentModel;

namespace Business_AnimeToNotion.Functions.Static
{
    public static class Common_Utilities
    {
        public static string MALFormatToCommon(string MAL_Type)
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

        public static Season RetrieveUpcomingSeason()
        {
            DateTime now = DateTime.Now;
            if (now.Month >= 1 && now.Month < 4)
                return Season.Spring;
            else if (now.Month >= 4 && now.Month < 7)
                return Season.Summer;
            else if (now.Month >= 7 && now.Month < 10)
                return Season.Fall;
            else if (now.Month >= 10 && now.Month <= 12)
                return Season.Winter;

            return Season.Winter;
        }

        /// <summary>
        /// Get enum value from description
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }

        /// <summary>
        /// Checks differences for SyncMalData azure function
        /// </summary>
        /// <param name="MAL_Entry"></param>
        /// <param name="Notion_Entry"></param>
        /// <param name="differences"></param>
        /// <returns></returns>
        public static bool Equals(MAL_AnimeShowRaw MAL_Entry, Page Notion_Entry, out Dictionary<string, PropertyValue> differences)
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

        /// <summary>
        /// Maps the properties of a Notion entry to AnimeShow
        /// </summary>
        /// <param name="input"></param>
        /// <param name="notionPropsDifferences"></param>
        /// <returns></returns>
        public static AnimeShow MapFromNotionToAnimeShow(AnimeShow input, Dictionary<string, PropertyValue> notionPropsDifferences)
        {
            foreach(var prop in notionPropsDifferences)
            {
                switch (prop.Key)
                {
                    case "Name Original":
                        input.NameDefault = Mapping.Mapper.Map<string>(prop.Value);
                        break;

                    case "Name English":
                        input.NameEnglish = Mapping.Mapper.Map<string>(prop.Value);
                        break;

                    case "MAL Score":
                        if(input.ScoreId != null)
                        {
                            input.Score.MalScore = Int32.Parse(Mapping.Mapper.Map<string>(prop.Value));
                        }
                        break;

                    case "Format":
                        input.Format = Mapping.Mapper.Map<string>(prop.Value);
                        break;

                    case "Started Airing":
                        input.StartedAiring = DateTime.Parse(Mapping.Mapper.Map<string>(prop.Value));
                        break;

                    case "Cover":
                        input.Cover = Mapping.Mapper.Map<string>(prop.Value);
                        break;

                    case "Episodes":
                        input.Episodes = Int32.Parse(Mapping.Mapper.Map<string>(prop.Value));
                        break;
                }
            }

            return input;
        }
    }
}
