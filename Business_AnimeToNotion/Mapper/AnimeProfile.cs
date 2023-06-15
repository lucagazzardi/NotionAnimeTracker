using AutoMapper;
using Business_AnimeToNotion.Functions.Static;
using Business_AnimeToNotion.Model;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Notion.Client;

namespace Business_AnimeToNotion.Mapper
{
    public class AnimeProfile : Profile
    {
        public AnimeProfile()
        {
            #region Demo

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
                                AnimeShow.WatchingTime.YearNotionPageId = ((RelationPropertyValue)prop.Value).Relation[0].Id;
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

            #endregion

            #region Dto to Entity

            CreateMap<AnimeShowDto, AnimeShow>();

            CreateMap<RelationDto, Data_AnimeToNotion.DataModel.Relation>();

            CreateMap<WatchingTimeDto, WatchingTime>();

            CreateMap<ScoreDto, Score>();

            CreateMap<NoteDto, Note>();

            #endregion

            #region FunctionMappings

            CreateMap<MAL_AnimeModel, Dictionary<string, PropertyValue>>().ConvertUsing((source, dst) =>
            {
                return new Dictionary<string, PropertyValue>
                {
                    { "Name English", Mapping.Mapper.Map<TitlePropertyValue>(string.IsNullOrEmpty(source.alternative_titles.en) ? source.title : source.alternative_titles.en) },
                    { "Name Original", Mapping.Mapper.Map<RichTextPropertyValue>(source.title) },
                    { "MAL Score", Mapping.Mapper.Map<NumberPropertyValue>(source.mean) },
                    { "Next To Watch", Mapping.Mapper.Map<CheckboxPropertyValue>(false) },
                    { "Format", Mapping.Mapper.Map<SelectPropertyValue>(Common_Utilities.MALToNotion_PropertyType(source.media_type)) },
                    { "Status", Mapping.Mapper.Map<SelectPropertyValue>("To Watch") },
                    { "Started Airing", Mapping.Mapper.Map<DatePropertyValue>(source.start_date) },
                    { "Cover", Mapping.Mapper.Map<FilesPropertyValue>(source.main_picture.medium) },
                    { "MAL Id", Mapping.Mapper.Map<NumberPropertyValue>(source.id) },
                    { "MAL Link", Mapping.Mapper.Map<UrlPropertyValue>(source.id) },
                    { "Episodes", Mapping.Mapper.Map<NumberPropertyValue>(source.num_episodes) },
                    { "Studios", Mapping.Mapper.Map<RichTextPropertyValue>(source.studios) },
                    { "Genres", Mapping.Mapper.Map<RichTextPropertyValue>(source.genres) }
                };
            });


            //Name English
            CreateMap<string, TitlePropertyValue>().ForMember(dto => dto.Title, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));

            //Name Original
            CreateMap<string, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));

            //MAL Rating
            CreateMap<decimal, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => Double.Parse(source.ToString())));

            //Next To Watch
            CreateMap<bool, CheckboxPropertyValue>().ForMember(dto => dto.Checkbox, map => map.MapFrom(source => false));

            //Type
            CreateMap<string, SelectPropertyValue>().ForMember(dto => dto.Select, map => map.MapFrom(source => new SelectOption() { Name = source }));

            //Watched
            CreateMap<string, SelectPropertyValue>().ForMember(dto => dto.Select, map => map.MapFrom(source => new SelectOption() { Name = source }));

            //Started Airing
            CreateMap<string, DatePropertyValue>().ConvertUsing((source, dest) =>
            {
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(source, out result))
                    return new DatePropertyValue() { Date = new Date() { Start = DateTime.Parse(source) } };
                else
                    return new DatePropertyValue() { Date = new Date() { Start = null } };
            });

            //Cover
            CreateMap<string, FilesPropertyValue>().ForMember(dto => dto.Files, map => map.MapFrom(source => new List<FileObjectWithName>() { new ExternalFileWithName() { Type = "external", Name = source, External = new ExternalFileWithName.Info() { Url = source } } }));

            //MAL Id
            CreateMap<int, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => source));

            //MAL Link
            CreateMap<int, UrlPropertyValue>().ForMember(dto => dto.Url, map => map.MapFrom(source => Common_Utilities.Property_MAL_Link(source.ToString())));

            //Episodes
            CreateMap<int, NumberPropertyValue>().ForMember(dto => dto.Number, map => map.MapFrom(source => source));

            //Studios
            CreateMap<List<MAL_GeneralObject>, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", source.Select(x => x.name)) } } }));

            //Genres
            CreateMap<List<MAL_GeneralObject>, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", source.Select(x => x.name)) } } }));

            //Show Hidden
            CreateMap<string, RichTextPropertyValue>().ForMember(dto => dto.RichText, map => map.MapFrom(source => new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = source } } }));



            // Reverse Maps
            CreateMap<PropertyValue, string>().ConvertUsing((src, dest) =>
            {
                switch (src.Type)
                {
                    case PropertyValueType.Title:
                        // Need to serialize to JSON the object and deserialize in the correct type
                        var serializedTitle = JsonConvert.SerializeObject(((TitlePropertyValue)src).Title[0]);
                        var deserializedTitle = JsonConvert.DeserializeObject<RichTextText>(serializedTitle);
                        return deserializedTitle.Text.Content;
                    case PropertyValueType.RichText:
                        // Need to serialize to JSON the object and deserialize in the correct type
                        var serializedText = JsonConvert.SerializeObject(((RichTextPropertyValue)src).RichText[0]);
                        var deserializedText = JsonConvert.DeserializeObject<RichTextText>(serializedText);
                        return deserializedText.Text.Content;
                    case PropertyValueType.Number:
                        return ((NumberPropertyValue)src).Number != null ? ((NumberPropertyValue)src).Number.Value.ToString() : "0";
                    case PropertyValueType.Checkbox:
                        return ((CheckboxPropertyValue)src).Checkbox.ToString();
                    case PropertyValueType.Select:
                        return ((SelectPropertyValue)src).Select?.Name;
                    case PropertyValueType.Date:
                        return ((DatePropertyValue)src).Date != null ? !string.IsNullOrEmpty(((DatePropertyValue)src).Date.Start.ToString()) ? ((DatePropertyValue)src).Date.Start.ToString() : null : null;
                    case PropertyValueType.Files:
                        return ((FilesPropertyValue)src).Files.First().Name;
                    case PropertyValueType.Url:
                        return ((UrlPropertyValue)src).Url;
                    default: return "";
                }
            });

            // Filter out the useless fields
            CreateMap<Dictionary<string, PropertyValue>, Compare_AnimeModel>().ConvertUsing((src, dest) =>
            {
                var usefullFields = new List<string>()
                {
                    "Name English",
                    "Name Original",
                    "MAL Score",
                    "Format",
                    "Started Airing",
                    "Cover",
                    "MAL Id",
                    "MAL Link",
                    "Episodes",
                    "Studios",
                    "Genres"
                };

                return new Compare_AnimeModel() { CompareProperties = src.Where(x => usefullFields.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value) };
            });

            #endregion

        }
    }


}
