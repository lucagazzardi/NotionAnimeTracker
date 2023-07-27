﻿using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.QueryLogic.SortLogic
{
    public enum SortIn
    {
        Title,
        Status,
        MalScore,
        PersonalScore,
        StartDate,
        FinishDate,
        StartedAiring
    }    

    public class SortManager : ISortManager
    {  
        public SortManager()
        {
            
        }      

        public IQueryable<AnimeShow> ApplySort(IQueryable<AnimeShow> data, SortIn? sort)
        {
            switch (sort)
            {
                case SortIn.Title:
                    return data.OrderBy(x => x.NameEnglish);
                case SortIn.Status:
                    return data.OrderByDescending(x => x.Status).ThenBy(x => x.NameEnglish);
                case SortIn.MalScore:
                    return data.OrderByDescending(x => x.Score.MalScore).ThenBy(x => x.NameEnglish);
                case SortIn.PersonalScore:
                    return data.OrderByDescending(x => x.Score.PersonalScore).ThenBy(x => x.NameEnglish);
                case SortIn.StartDate:
                    return data.OrderByDescending(x => x.WatchingTime.StartedOn).ThenBy(x => x.NameEnglish);
                case SortIn.FinishDate:
                    return data.OrderByDescending(x => x.WatchingTime.FinishedOn).ThenBy(x => x.NameEnglish);
                case SortIn.StartedAiring:
                    return data.OrderByDescending(x => x.StartedAiring).ThenBy(x => x.NameEnglish);
                default:
                    return data.OrderByDescending(x => x.Status).ThenBy(x => x.NameEnglish);
            }
        }
    }
}