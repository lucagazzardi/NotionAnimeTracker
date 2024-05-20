using Business_AnimeToNotion.Model.Internal;
using Data_AnimeToNotion.DataModel;
using Functions_AnimeToNotion.Model;

namespace Functions_AnimeToNotion.Utility
{
    public static class Utility
    {
        public static Changes_MalToInternal CheckDifferences(AnimeShowFull malShow, AnimeShowFull internalShow)
        {
            Changes_MalToInternal result = new Changes_MalToInternal();

            result.Changes = EvaluateChanges(malShow, internalShow);
            result.ChangedAnime = malShow;
            return result;
        }

        public static AnimeShow SetBasicChanges(AnimeShow show, AnimeShowFull source, List<string> changes)
        {
            foreach(var change in changes) {
                switch (change)
                {
                    case "NameDefault":
                        show.NameDefault = source.NameDefault; break;
                    case "NameEnglish":
                        show.NameEnglish = source.NameEnglish; break;
                    case "NameJapanese":
                        show.NameJapanese = source.NameJapanese; break;
                    case "Cover":
                        show.Cover = source.Cover; break;
                    case "Episodes":
                        show.Episodes = source.Episodes; break;
                    case "StartedAiring":
                        show.StartedAiring = source.StartedAiring; break;
                    case "Score":                        
                        show.Score = source.Score; break;
                }
            }
            return show;
        }

        private static List<string> EvaluateChanges(AnimeShowFull mappedMalShow, AnimeShowFull internalShow)
        {
            List<string> result = new List<string>();            

            if (mappedMalShow.NameDefault != internalShow.NameDefault)
                result.Add("NameDefault");
            if (mappedMalShow.NameEnglish != internalShow.NameEnglish)
                result.Add("NameEnglish");
            if (mappedMalShow.NameJapanese != internalShow.NameJapanese)
                result.Add("NameJapanese");
            if (mappedMalShow.Cover != internalShow.Cover)
                result.Add("Cover");
            if (mappedMalShow.Episodes != internalShow.Episodes)
                result.Add("Episodes");
            if (mappedMalShow.Score != internalShow.Score)
                result.Add("Score");
            if (mappedMalShow.StartedAiring != internalShow.StartedAiring)
                result.Add("StartedAiring");

            var deltaStudios = mappedMalShow.Studios.Select(x => x.Id).Where(x => !internalShow.Studios.Select(y => y.Id).Contains(x)).ToList();
            var deltaGenres = mappedMalShow.Genres.Select(x => x.Id).Where(x => !internalShow.Genres.Select(y => y.Id).Contains(x)).ToList();

            if (deltaStudios.Count > 0)
            {
                mappedMalShow.Studios = mappedMalShow.Studios.Where(x => deltaStudios.Contains(x.Id)).ToArray();
                result.Add("Studios");
            }

            if (deltaGenres.Count > 0)
            {
                mappedMalShow.Genres = mappedMalShow.Genres.Where(x => deltaGenres.Contains(x.Id)).ToArray();
                result.Add("Genres");
            }

            return result;
        }        
    }
}
