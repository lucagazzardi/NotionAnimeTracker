using AutoMapper;
using Data_AnimeToNotion.Model;
using Notion.Client;

namespace Business_AnimeToNotion.Mapper
{
    public class Demo_Profile : Profile
    {
        public Demo_Profile()
        {
            CreateMap<Page, AnimeShowDto>().ConvertUsing((src, dest) =>
            {
                AnimeShowDto AnimeShow = new AnimeShowDto();
                AnimeShow.NotionPageId = src.Id;
                foreach (var prop in src.Properties)
                {
                    switch (prop.Key)
                    {
                        case "Name Original":
                            AnimeShow.NameOriginal = ((RichTextPropertyValue)prop.Value).RichText[0].PlainText;
                            break;

                        case "Name English":
                            AnimeShow.NameEnglish = ((TitlePropertyValue)prop.Value).Title[0].PlainText;
                            break;

                        case "MAL Id":
                            AnimeShow.MalId = Convert.ToInt32(((NumberPropertyValue)prop.Value).Number);
                            break;

                        case "Format":
                            AnimeShow.Format = ((SelectPropertyValue)prop.Value).Select?.Name;
                            break;

                        case "Episodes":
                            double? episodes = ((NumberPropertyValue)prop.Value).Number;
                            AnimeShow.Episodes = episodes == null ? null : Convert.ToInt32(episodes);
                            break;

                        case "Status":
                            AnimeShow.Status = ((SelectPropertyValue)prop.Value).Select?.Name;
                            break;

                        case "Started Airing":
                            AnimeShow.StartedAiring = ((DatePropertyValue)prop.Value).Date != null ? ((DatePropertyValue)prop.Value).Date.Start != null ? ((DatePropertyValue)prop.Value).Date.Start : null : null;
                            break;

                        case "Cover":
                            AnimeShow.Cover = ((FilesPropertyValue)prop.Value).Files[0].Name;
                            break;

                        case "MAL Score":
                            double? malScore = ((NumberPropertyValue)prop.Value).Number;
                            if (malScore != null && malScore > 0)
                            {
                                AnimeShow.Score = AnimeShow.Score ?? new ScoreDto();
                                AnimeShow.Score.MalScore = Convert.ToInt32(Math.Round(malScore.Value * 10));
                            }
                            break;

                        case "Personal Score":
                            double? persScore = ((NumberPropertyValue)prop.Value).Number;
                            if (persScore != null)
                            {
                                AnimeShow.Score = AnimeShow.Score ?? new ScoreDto();
                                AnimeShow.Score.PersonalScore = Convert.ToInt32(Math.Round(persScore.Value * 10));
                            }
                            break;

                        case "Favorite":
                            if (((SelectPropertyValue)prop.Value).Select?.Name != null)
                            {
                                AnimeShow.Score = AnimeShow.Score ?? new ScoreDto();
                                AnimeShow.Score.Favorite = true;
                            }
                            break;

                        case "Started On":
                            if (((DatePropertyValue)prop.Value).Date != null && ((DatePropertyValue)prop.Value).Date.Start != null)
                            {
                                AnimeShow.WatchingTime = AnimeShow.WatchingTime ?? new WatchingTimeDto();
                                AnimeShow.WatchingTime.StartedOn = ((DatePropertyValue)prop.Value).Date.Start.Value;
                            }
                            break;

                        case "Finished On":
                            if (((DatePropertyValue)prop.Value).Date != null && ((DatePropertyValue)prop.Value).Date.Start != null)
                            {
                                AnimeShow.WatchingTime = AnimeShow.WatchingTime ?? new WatchingTimeDto();
                                AnimeShow.WatchingTime.FinishedOn = ((DatePropertyValue)prop.Value).Date.Start.Value;
                            }
                            break;

                        case "Completed Year":
                            if (((RelationPropertyValue)prop.Value).Relation.Count > 0)
                            {
                                AnimeShow.WatchingTime = AnimeShow.WatchingTime ?? new WatchingTimeDto();
                                AnimeShow.WatchingTime.CompletedYear = AnimeShow.WatchingTime.FinishedOn!.Value.Year;
                            }
                            break;

                        case "Notes":
                            if (((RichTextPropertyValue)prop.Value).RichText.Count > 0)
                            {
                                AnimeShow.Note = new NoteDto() { Notes = ((RichTextPropertyValue)prop.Value).RichText[0].PlainText };
                            }
                            break;
                        default:
                            break;
                    }
                }

                return AnimeShow;
            });
        }
    }
}
