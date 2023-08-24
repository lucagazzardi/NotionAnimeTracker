using JikanDotNet;

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
    }
}
