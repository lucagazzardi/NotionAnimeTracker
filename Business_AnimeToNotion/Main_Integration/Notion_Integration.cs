using Business_AnimeToNotion.Common;
using Business_AnimeToNotion.Main_Integration.Exceptions;
using Business_AnimeToNotion.Main_Integration.Interfaces;
using Business_AnimeToNotion.Model;
using Business_AnimeToNotion.Profile.AutoMapperConfig;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Notion.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Main_Integration
{
    internal class MAL_Properties_Mapping
    {
        public const string TV = "tv";
        public const string Movie = "movie";
        public const string Special = "special";
        public const string OVA = "ova";
        public const string Base_MAL_URL = "https://myanimelist.net/anime/";

        //Related Anime
        public const string Prequel = "prequel";
        public const string Sequel = "sequel";
        public const string ParentStory = "parent_story";
    }

    internal class Notion_Properties_Mapping
    {
        //Main Properties
        public const string Name_English = "Name English";
        public const string Name_Original = "Name Original";
        public const string MAL_Rating = "MAL Rating";
        public const string Next_To_Watch = "Next To Watch";
        public const string Distributor = "Distributor";
        public const string Type = "Type";
        public const string Season = "Season";
        public const string Watched = "Watched";
        public const string Started_Airing = "Started Airing";
        public const string Cover = "Cover";
        public const string MAL_Id = "MAL Id";
        public const string MAL_Link = "MAL Link";
        public const string Episodes = "Episodes";
        public const string Studios = "Studios";
        public const string Genres = "Genres";
        public const string Show_Hidden = "Show Hidden";

        //Minor properties
        public const string TV = "TV Show";
        public const string Movie = "Movie";
        public const string OVA = "OVA";
        public const string Special = "Special";
        public const string AnimeSaturn = "AnimeSaturn";
        public const string ToWatch = "To Watch";
        public const string External = "external"; 
    }

    internal class Notion_ExceptionMessages
    {
        public const string CreateNewEntryException = "Error: \"[title]\" is already present";
        public const string MultiSeasonIdentifierException = "Error: couldn't retrieve parent show for \"[title]\"";
        public const string GetLatestAdded = "Error: failed latest added retrieval";
        public const string UpdatePropertiesException = "Error: properties update failed";
    }

    public class Notion_Integration : INotion_Integration
    {
        #region fields

        private readonly IConfiguration Configuration;
        private readonly IMAL_Integration _malIntegration;

        #endregion

        private NotionClient Client { get; set; }
        private string DataBaseId { get; set; } = null;

        public Notion_Integration(IConfiguration configuration, IMAL_Integration malIntegration)
        {
            Configuration = configuration;
            _malIntegration = malIntegration;

            Client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = Configuration["Notion-AuthToken"]
            });
        }

        public async Task Notion_CreateNewEntry(MAL_AnimeModel animeModel)
        {
            await Notion_GetDataBaseId();

            var result = await ConsistencyChecks(animeModel);

            try
            {
                PagesCreateParameters pagesCreateParameters = ConvertMALResponseToNotionPage_V2(result, DataBaseId);

                await Notion_CreateNewEntry(pagesCreateParameters);
            }
            catch (Exception ex)
            {
                throw new Notion_Exception("Error: " + ex.Message);
            }
        }

        public async Task<List<Notion_LatestAddedModel>> Notion_GetLatestAdded()
        {
            List<Notion_LatestAddedModel> result = new List<Notion_LatestAddedModel>();

            await Notion_GetDataBaseId();

            try
            {
                var sortedLastAdded = await Notion_GetSortedAdded();

                foreach (var lastAdd in sortedLastAdded)
                {
                    result.Add(MapNotionPageToLatestAdded(lastAdd, sortedLastAdded.IndexOf(lastAdd)));
                }
            }
            catch
            {
                throw new Notion_Exception(Notion_ExceptionMessages.GetLatestAdded);
            }    

            return result;
        }

        public async Task<Dictionary<string, string>> Notion_UpdateProperties(List<string> propertiesToUpate)
        {
            await Notion_GetDataBaseId();

            Dictionary<string, string> differences = new Dictionary<string, string>();

            try
            {
                var toUpdate = await RetrieveAllPages(DataBaseId);

                //For every page, execute the update
                foreach (var page in toUpdate)
                {
                    try
                    {
                        int id = GetId(page).Value;
                        var malShow = await GetFromMAL(id);

                        differences.Add($"***** {id} - {GetTitle(page)} *****", "Starting");

                        //Update only if there are differences
                        Dictionary<string, PropertyValue> notionPropertiesDictionary = new Dictionary<string, PropertyValue>();
                        Notion_UpdateProperties(page, malShow, propertiesToUpate, notionPropertiesDictionary, differences);

                        if (notionPropertiesDictionary.Count > 0)
                            await Client.Pages.UpdatePropertiesAsync(page.Id, notionPropertiesDictionary);

                        differences.Add($"******* {GetTitle(page)} ******", "Finished");
                    }
                    catch
                    {
                        differences.Add(GetTitle(page), "Error!");
                    }
                }
            }
            catch
            {
                throw new Notion_Exception(Notion_ExceptionMessages.UpdatePropertiesException);
            }

            

            return differences;
        }

        #region Demo

        public async Task<List<Notion_RatingsUpdate>> Notion_GetRatingsToUpdate()
        {
            List<Notion_RatingsUpdate> result = new List<Notion_RatingsUpdate>();
            await Notion_GetDataBaseId();

            var toUpdate = await RetrieveAllPages(DataBaseId);

            foreach(var page in toUpdate)
            {
                var MALShow = await GetFromMAL(GetId(page).Value);

                decimal currentRating = GetRating(page);

                if (MALShow.mean != currentRating)
                    result.Add(new Notion_RatingsUpdate()
                    {
                        title = GetTitle(page),
                        oldRating = currentRating,
                        newRating = MALShow.mean
                    });
            }


            return result;
        }

        #endregion

        #region Private

        private async Task Notion_GetDataBaseId()
        {
            if (string.IsNullOrEmpty(DataBaseId))
            {
                var database = await Client.Search.SearchAsync(new SearchParameters()
                {
                    Filter = new SearchFilter() { Value = SearchObjectType.Database },
                    Query = Configuration["Notion_ApiConfig:Notion_DatabaseName"]
                });
                DataBaseId = database.Results[0].Id;
            }
        }

        private async Task Notion_CreateNewEntry(PagesCreateParameters pagesCreateParameters)
        {
            await Client.Pages.CreateAsync(pagesCreateParameters);
        }

        private async Task<List<Page>> Notion_GetSortedAdded()
        {
            var sortedLatestAdded = await Client.Databases.QueryAsync(DataBaseId, new DatabasesQueryParameters()
            {
                Sorts = new List<Sort>() { new Sort() { Timestamp = Timestamp.CreatedTime, Direction = Direction.Descending } },
                PageSize = 3
            });

            return sortedLatestAdded.Results.Take(3).ToList();
        }

        private void Notion_UpdateProperties(Page toUpdate, MAL_AnimeModel showFrom, List<string> propertiesToUpate, Dictionary<string, PropertyValue> properties, Dictionary<string, string> differences)
        {
            foreach(var property in propertiesToUpate)
            {
                string newValue = string.Empty;
                string oldValue = string.Empty;
                switch (property)
                { 
                    #region Properties Building

                    case Notion_Properties_Mapping.Name_English:

                        newValue = string.IsNullOrEmpty(showFrom.alternative_titles.en) ? showFrom.title : showFrom.alternative_titles.en;
                        oldValue = GetPropertyValue(toUpdate.Properties[property]);
                        if (string.Equals(oldValue, newValue)) break;

                        differences.Add(property, $"{oldValue} ---> {newValue}");
                        
                        PropertyValue Name_English = new TitlePropertyValue()
                        {
                            Title = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = newValue } } }
                        };
                        properties.Add(Notion_Properties_Mapping.Name_English, Name_English);
                        break;

                    case Notion_Properties_Mapping.Name_Original:

                        newValue = showFrom.title;
                        oldValue = GetPropertyValue(toUpdate.Properties[property]);
                        if (string.Equals(oldValue, newValue)) break;

                        differences.Add(property, $"{oldValue} ---> {newValue}");

                        PropertyValue Name_Original = new RichTextPropertyValue()
                        {
                            RichText = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = newValue } } }
                        };
                        properties.Add(Notion_Properties_Mapping.Name_Original, Name_Original);
                        break;

                    case Notion_Properties_Mapping.MAL_Rating:

                        newValue = showFrom.mean.ToString();
                        oldValue = GetPropertyValue(toUpdate.Properties[property]);
                        if (string.Equals(oldValue, newValue)) break;

                        differences.Add(property, $"{oldValue} ---> {newValue}");

                        PropertyValue MAL_Rating = new NumberPropertyValue()
                        {
                            Number = Double.Parse(newValue)
                        };
                        properties.Add(Notion_Properties_Mapping.MAL_Rating, MAL_Rating);
                        break;

                    case Notion_Properties_Mapping.Type:

                        newValue = MAL_NotionUtility.Property_Type(showFrom.media_type);
                        oldValue = GetPropertyValue(toUpdate.Properties[property]);
                        if (string.Equals(oldValue, newValue)) break;

                        differences.Add(property, $"{oldValue} ---> {newValue}");

                        SelectPropertyValue Type = new SelectPropertyValue()
                        {
                            Select = new SelectOption() { Name = newValue }
                        };
                        properties.Add(Notion_Properties_Mapping.Type, Type);
                        break;

                    case Notion_Properties_Mapping.Episodes:

                        newValue = showFrom.num_episodes.ToString();
                        oldValue = GetPropertyValue(toUpdate.Properties[property]);
                        if (string.Equals(oldValue, newValue)) break;

                        differences.Add(property, $"{oldValue} ---> {newValue}");

                        NumberPropertyValue Episodes = new NumberPropertyValue()
                        {
                            Number = int.Parse(newValue)
                        };
                        properties.Add(Notion_Properties_Mapping.Episodes, Episodes);
                        break;

                    case Notion_Properties_Mapping.Genres:

                        newValue = string.Join(", ", showFrom.genres.Select(x => x.name));
                        oldValue = GetPropertyValue(toUpdate.Properties[property]);
                        if (string.Equals(oldValue, newValue)) break;

                        differences.Add(property, $"{oldValue} ---> {newValue}");

                        RichTextPropertyValue Genres = new RichTextPropertyValue()
                        {
                            RichText = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = newValue } } }
                        };
                        properties.Add(Notion_Properties_Mapping.Genres, Genres);
                        break;

                    case Notion_Properties_Mapping.MAL_Id:

                        newValue = showFrom.id.ToString();
                        oldValue = GetPropertyValue(toUpdate.Properties[property]);
                        if (string.Equals(oldValue, newValue)) break;

                        differences.Add(property, $"{oldValue} ---> {newValue}");

                        NumberPropertyValue MAL_Id = new NumberPropertyValue()
                        {
                            Number = Double.Parse(newValue)
                        };
                        properties.Add(Notion_Properties_Mapping.MAL_Id, MAL_Id);
                        break;

                    case Notion_Properties_Mapping.MAL_Link:

                        newValue = MAL_NotionUtility.Property_MAL_Link(showFrom.id.ToString());
                        oldValue = GetPropertyValue(toUpdate.Properties[property]);
                        if (string.Equals(oldValue, newValue)) break;

                        differences.Add(property, $"{oldValue} ---> {newValue}");

                        UrlPropertyValue MAL_Link = new UrlPropertyValue()
                        {
                            Url = newValue
                        };
                        properties.Add(Notion_Properties_Mapping.MAL_Link, MAL_Link);
                        break;

                    case Notion_Properties_Mapping.Studios:

                        newValue = string.Join(", ", showFrom.studios.Select(x => x.name));
                        oldValue = GetPropertyValue(toUpdate.Properties[property]);
                        if (string.Equals(oldValue, newValue)) break;

                        differences.Add(property, $"{oldValue} ---> {newValue}");

                        RichTextPropertyValue Studios = new RichTextPropertyValue()
                        {
                            RichText = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = newValue } } }
                        };
                        properties.Add(Notion_Properties_Mapping.Studios, Studios);
                        break;

                    #endregion
                }
            }
        }

        private string GetPropertyValue(PropertyValue propertyValue)
        {
            if (propertyValue is RichTextPropertyValue)
            {
                var richTextPropertyValue = propertyValue as RichTextPropertyValue;
                return richTextPropertyValue.RichText[0].PlainText;
            }
            else if (propertyValue is TitlePropertyValue)
            {
                var titlePropertyValue = propertyValue as TitlePropertyValue;
                return titlePropertyValue.Title[0].PlainText;
            }
            else if (propertyValue is NumberPropertyValue)
            {
                var numberPropertyValue = propertyValue as NumberPropertyValue;
                return numberPropertyValue.Number.ToString();
            }
            else if (propertyValue is SelectPropertyValue)
            {
                var selectPropertyValue = propertyValue as SelectPropertyValue;
                return selectPropertyValue.Select.Name;
            }
            else if (propertyValue is UrlPropertyValue)
            {
                var urlPropertyValue = propertyValue as UrlPropertyValue;
                return urlPropertyValue.Url.ToString();
            }
            else return string.Empty;
        } 

        private async Task<MAL_AnimeModel> ConsistencyChecks(MAL_AnimeModel animeModel)
        {
            ReplaceMissingTitle(animeModel);

            await CheckForDuplicates(animeModel);

            return await SetMultiSeasonIdentifier(animeModel);
        }

        private async Task CheckForDuplicates(MAL_AnimeModel animeModel)
        {
            //If a page with the same show id already exists, nothing is done
            var checkAnimeDuplicate = await Client.Databases.QueryAsync(DataBaseId, new DatabasesQueryParameters()
            {
                Filter = new NumberFilter(Notion_Properties_Mapping.MAL_Id, animeModel.id)
            });

            if(checkAnimeDuplicate.Results.Count > 0 )
                throw new Notion_Exception(Notion_ExceptionMessages.CreateNewEntryException.Replace("[title]", animeModel.alternative_titles.en));
        }

        private void ReplaceMissingTitle(MAL_AnimeModel animeModel)
        {
            //When english and original title match "alternative_titles.en" is empty
            if (string.IsNullOrEmpty(animeModel.alternative_titles.en))
                animeModel.alternative_titles.en = animeModel.title;
        }

        private async Task<MAL_AnimeModel> SetMultiSeasonIdentifier(MAL_AnimeModel animeModel)
        {
            //// If related_anime is populated but has no items, it means that the search is by ID but this is the first season.
            //if(animeModel.related_anime.Count == 0)
            //{
            //    animeModel.related_anime = new List<MAL_RelatedShow>();
            //    return animeModel;
            //}

            animeModel.showHidden = animeModel.alternative_titles.en;

            //If the show comes from the search by name, MAL doesn't populate the field "related_anime" so a new call is needed to recover that field
            animeModel = animeModel.related_anime.Count == 0 ? await GetFromMAL(animeModel.id) : animeModel;
            ReplaceMissingTitle(animeModel);

            try
            {
                int? prequelId = GetPrequelId(animeModel);

                //Recursive check to get the first show in a series
                while (prequelId != null)
                {
                    var prequel = await _malIntegration.MAL_SearchAnimeByIdAsync(prequelId.Value);
                    ReplaceMissingTitle(prequel);
                    animeModel.showHidden = prequel.alternative_titles.en;
                    prequelId = GetPrequelId(prequel);
                }
            }
            catch
            {
                throw new Notion_Exception(Notion_ExceptionMessages.MultiSeasonIdentifierException.Replace("[title]", animeModel.alternative_titles.en));
            }

            return animeModel;

            //Internal method
            int? GetPrequelId(MAL_AnimeModel animeModel)
            {
                return animeModel.related_anime?
                    .FirstOrDefault(x => string.Equals(x.relation_type, MAL_Properties_Mapping.ParentStory) || string.Equals(x.relation_type, MAL_Properties_Mapping.Prequel))?
                    .node.id;
            }
        }

        private async Task<List<Page>> RetrieveAllPages(string DataBaseId)
        {
            List<Page> result = new List<Page>();
            PaginatedList<Page> retrievedNotionPages = null;
            string cursor = null;
            // Retrieve the pages that need to be updated
            do
            {
                retrievedNotionPages = await Client.Databases.QueryAsync(DataBaseId, new DatabasesQueryParameters()
                {
                    //Filter = new NumberFilter(Notion_Properties_Mapping.MAL_Id, equal: 268),
                    StartCursor = cursor
                });
                cursor = retrievedNotionPages.NextCursor;
                result.AddRange(retrievedNotionPages.Results);
            }
            while (retrievedNotionPages.HasMore);

            return result;
        }

        [Obsolete("This method is obsolete. Use ConvertMALResponseToNotionPage_V2 instead.")]
        private PagesCreateParameters ConvertMALResponseToNotionPage(MAL_AnimeModel animeModel, string databaseId)
        {
            Dictionary<string, PropertyValue> properties = new Dictionary<string, PropertyValue>();

            #region Properties Building

            //Name English
            TitlePropertyValue Name_English = new TitlePropertyValue()
            {
                Title = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = animeModel.alternative_titles.en } } }
            };
            properties.Add(Notion_Properties_Mapping.Name_English, Name_English);

            //Name Original
            RichTextPropertyValue Name_Original = new RichTextPropertyValue()
            {
                RichText = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = animeModel.title } } }
            };
            properties.Add(Notion_Properties_Mapping.Name_Original, Name_Original);

            //MAL Rating
            if (animeModel.mean != 0)
            {
                NumberPropertyValue MAL_Rating = new NumberPropertyValue()
                {
                    Number = Double.Parse(animeModel.mean.ToString())
                };
                properties.Add(Notion_Properties_Mapping.MAL_Rating, MAL_Rating);
            }

            //Next To Watch
            CheckboxPropertyValue Next_To_Watch = new CheckboxPropertyValue()
            {
                Checkbox = false
            };
            properties.Add(Notion_Properties_Mapping.Next_To_Watch, Next_To_Watch);

            //Distributor
            MultiSelectPropertyValue Distributor = new MultiSelectPropertyValue()
            {
                MultiSelect = new List<SelectOption>() { new SelectOption() { Name = Notion_Properties_Mapping.AnimeSaturn } }
            };
            properties.Add(Notion_Properties_Mapping.Distributor, Distributor);

            //Type
            SelectPropertyValue Type = new SelectPropertyValue()
            {
                Select = new SelectOption() { Name = MAL_NotionUtility.Property_Type(animeModel.media_type) }
            };
            properties.Add(Notion_Properties_Mapping.Type, Type);

            //Watched
            SelectPropertyValue Watched = new SelectPropertyValue()
            {
                Select = new SelectOption() { Name = Notion_Properties_Mapping.ToWatch }
            };
            properties.Add(Notion_Properties_Mapping.Watched, Watched);

            if (animeModel.start_date != null)
            {
                //Started Airing
                DatePropertyValue Started_Airing = new DatePropertyValue()
                {
                    Date = new Date() { Start = DateTime.Parse(animeModel.start_date) }
                };
                properties.Add(Notion_Properties_Mapping.Started_Airing, Started_Airing);
            }

            //Cover
            FilesPropertyValue Cover = new FilesPropertyValue()
            {
                Files = new List<FileObjectWithName>() { new ExternalFileWithName() { Type = Notion_Properties_Mapping.External, Name = animeModel.title, External = new ExternalFileWithName.Info() { Url = animeModel.main_picture.medium } } }
            };
            properties.Add(Notion_Properties_Mapping.Cover, Cover);

            //MAL Id
            NumberPropertyValue MAL_Id = new NumberPropertyValue()
            {
                Number = animeModel.id
            };
            properties.Add(Notion_Properties_Mapping.MAL_Id, MAL_Id);

            //MAL Link
            UrlPropertyValue MAL_Link = new UrlPropertyValue()
            {
                Url = MAL_NotionUtility.Property_MAL_Link(animeModel.id.ToString())
            };
            properties.Add(Notion_Properties_Mapping.MAL_Link, MAL_Link);

            if (animeModel.num_episodes != 0)
            {
                //Episodes
                NumberPropertyValue Episodes = new NumberPropertyValue()
                {
                    Number = animeModel.num_episodes
                };
                properties.Add(Notion_Properties_Mapping.Episodes, Episodes);
            }

            //Studios
            RichTextPropertyValue Studios = new RichTextPropertyValue()
            {
                RichText = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", animeModel.studios.Select(x => x.name)) } } }
            };
            properties.Add(Notion_Properties_Mapping.Studios, Studios);

            //Genres
            RichTextPropertyValue Genres = new RichTextPropertyValue()
            {
                RichText = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = string.Join(", ", animeModel.genres.Select(x => x.name)) } } }
            };
            properties.Add(Notion_Properties_Mapping.Genres, Genres);

            //Show Hidden
            RichTextPropertyValue Show_Hidden = new RichTextPropertyValue()
            {
                RichText = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = animeModel.showHidden ?? string.Empty } } }
            };
            properties.Add(Notion_Properties_Mapping.Show_Hidden, Show_Hidden);

            #endregion

            PagesCreateParameters result = new PagesCreateParameters()
            {
                Properties = properties,
                Parent = new DatabaseParentInput() { DatabaseId = databaseId }
            };
            return result;
        }

        private PagesCreateParameters ConvertMALResponseToNotionPage_V2(MAL_AnimeModel animeModel, string databaseId)
        {
            Dictionary<string, PropertyValue> properties = MappingHandler.Mapper.Map<Dictionary<string, PropertyValue>>(animeModel);            

            PagesCreateParameters result = new PagesCreateParameters()
            {
                Properties = properties,
                Parent = new DatabaseParentInput() { DatabaseId = databaseId }
            };
            return result;
        }

        private Notion_LatestAddedModel MapNotionPageToLatestAdded(Page notionPage, int orderIndex)
        {  
            Notion_LatestAddedModel result = new Notion_LatestAddedModel()
            {
                orderIndex = orderIndex,
                title = GetTitle(notionPage),
                cover = GetCover(notionPage),
                createdTime = notionPage.CreatedTime.ToString("yyyy-MM-dd")
            };

            return result;
        }

        private string GetTitle(Page page)
        {
            PropertyValue nameOriginalPropertyValue = null; RichTextPropertyValue titleRichText = null;
            page.Properties.TryGetValue(Notion_Properties_Mapping.Name_Original, out nameOriginalPropertyValue);
            titleRichText = nameOriginalPropertyValue as RichTextPropertyValue;
            return titleRichText.RichText[0].PlainText;
        }

        private int? GetId(Page page)
        {
            PropertyValue idPropValue = null; NumberPropertyValue id = null;
            page.Properties.TryGetValue(Notion_Properties_Mapping.MAL_Id, out idPropValue);
            id = idPropValue as NumberPropertyValue;
            return Convert.ToInt32(id.Number);
        }

        private string GetCover(Page page)
        {
            PropertyValue coverPropertyValue = null; FilesPropertyValue filesProperty = null; ExternalFileWithName externalFile = null;
            page.Properties.TryGetValue(Notion_Properties_Mapping.Cover, out coverPropertyValue);
            filesProperty = coverPropertyValue as FilesPropertyValue;
            externalFile = filesProperty.Files[0] as ExternalFileWithName;
            return externalFile.External.Url;
        }

        private decimal GetRating(Page page)
        {
            PropertyValue ratingPropValue = null; NumberPropertyValue rating = null;
            page.Properties.TryGetValue(Notion_Properties_Mapping.MAL_Rating, out ratingPropValue);
            rating = ratingPropValue as NumberPropertyValue;
            return Convert.ToDecimal(rating.Number);
        }

        private async Task<MAL_AnimeModel> GetFromMAL(int id)
        {
            return await _malIntegration.MAL_SearchAnimeByIdAsync(id);
        }

        #endregion
    }
}
