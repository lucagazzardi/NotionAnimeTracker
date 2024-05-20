using AutoMapper;
using Business_AnimeToNotion.Model.Internal;
using Notion.Client;

namespace Business_AnimeToNotion.Mapper
{
    public class Demo_Profile : Profile
    {
        public Demo_Profile()
        {
            CreateMap<Page, AnimeShowEdit>().ConvertUsing((src, dest) =>
            {
                AnimeShowEdit edit = new AnimeShowEdit();                
                foreach (var prop in src.Properties)
                {
                    switch (prop.Key)
                    {   
                        case "Status":
                            edit.Status = ((SelectPropertyValue)prop.Value).Select?.Name;
                            break;

                        case "Episodes Progress":
                            double? episodeProgress = ((NumberPropertyValue)prop.Value).Number;
                            if (episodeProgress != null)
                            {
                                edit.EpisodesProgress = Convert.ToInt32(Math.Round(episodeProgress.Value));
                            }
                            break;

                        case "Personal Score":
                            double? persScore = ((NumberPropertyValue)prop.Value).Number;
                            if (persScore != null)
                            {
                                edit.PersonalScore = persScore.Value < 10 ? Convert.ToInt32(Math.Round(persScore.Value * 10)) : Convert.ToInt32(persScore);
                            }
                            break;

                        case "Started On":
                            if (((DatePropertyValue)prop.Value).Date != null && ((DatePropertyValue)prop.Value).Date.Start != null)
                            {
                                edit.StartedOn = ((DatePropertyValue)prop.Value).Date.Start.Value;
                            }
                            break;

                        case "Finished On":
                            if (((DatePropertyValue)prop.Value).Date != null && ((DatePropertyValue)prop.Value).Date.Start != null)
                            {
                                edit.FinishedOn = ((DatePropertyValue)prop.Value).Date.Start.Value;
                            }
                            break;

                        case "Notes":
                            if (((RichTextPropertyValue)prop.Value).RichText.Count > 0)
                            {
                                edit.Notes = ((RichTextPropertyValue)prop.Value).RichText[0].PlainText;
                            }
                            break;
                        default:
                            break;
                    };

                    edit.CompletedYear = edit.FinishedOn != null ? edit.FinishedOn.Value.Year : null;
                }

                return edit;
            });
        }
    }
}
