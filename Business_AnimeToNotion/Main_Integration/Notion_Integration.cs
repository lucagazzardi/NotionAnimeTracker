using Business_AnimeToNotion.Main_Integration.Interfaces;
using Business_AnimeToNotion.Model;
using Microsoft.Extensions.Configuration;
using Notion.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Main_Integration
{
    internal class MAL_Properties_Mapping
    {
        public const string TV = "tv";
        public const string Movie = "movie";
        public const string Special = "special";
        public const string Base_MAL_URL = "https://myanimelist.net/anime/";
    }

    internal class Notion_Properites_Mapping
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

        //Minor properties
        public const string TV = "TV Serie";
        public const string Movie = "Movie";
        public const string AnimeSaturn = "AnimeSaturn";
        public const string ToWatch = "To Watch";
        public const string External = "external";
    }

    public class Notion_Integration : INotion_Integration
    {
        #region fields

        private readonly IConfiguration Configuration;

        #endregion

        private NotionClient Client { get; set; }
        private string DataBaseId { get; set; } = null;

        public Notion_Integration(IConfiguration configuration)
        {
            Configuration = configuration;

            Client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = Configuration["Notion_ApiConfig:Notion_AuthToken"]
            });
        }

        public async Task<bool> Notion_CreateNewEntry(MAL_AnimeModel animeModel)
        {
            await Notion_GetDataBaseId();

            //If a page with the same show title already exists, nothing is done
            var checkAnimeDuplicate = await Client.Search.SearchAsync(new SearchParameters()
            {
                Filter = new SearchFilter() { Value = SearchObjectType.Page },
                Query = animeModel.alternative_titles.en
            });

            if (checkAnimeDuplicate.Results.Count == 0)
            {
                PagesCreateParameters pagesCreateParameters = ConvertMALResponseToNotionPage(animeModel, DataBaseId);
                await Notion_CreateNewEntry(pagesCreateParameters);
                return true;
            }
            else
            {
                return false;
            }

        }

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

        private PagesCreateParameters ConvertMALResponseToNotionPage(MAL_AnimeModel animeModel, string databaseId)
        {
            Dictionary<string, PropertyValue> properties = new Dictionary<string, PropertyValue>();

            #region Properties Building

            //Name English
            TitlePropertyValue Name_English = new TitlePropertyValue()
            {
                Title = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = animeModel.alternative_titles.en } } }
            };
            properties.Add(Notion_Properites_Mapping.Name_English, Name_English);

            //Name Original
            RichTextPropertyValue Name_Original = new RichTextPropertyValue()
            {
                RichText = new List<RichTextBase>() { new RichTextText() { Text = new Text() { Content = animeModel.title } } }
            };
            properties.Add(Notion_Properites_Mapping.Name_Original, Name_Original);

            //MAL Rating
            NumberPropertyValue MAL_Rating = new NumberPropertyValue()
            {
                Number = Double.Parse(animeModel.mean.ToString())
            };
            properties.Add(Notion_Properites_Mapping.MAL_Rating, MAL_Rating);

            //Next To Watch
            CheckboxPropertyValue Next_To_Watch = new CheckboxPropertyValue()
            {
                Checkbox = false
            };
            properties.Add(Notion_Properites_Mapping.Next_To_Watch, Next_To_Watch);

            //Distributor
            MultiSelectPropertyValue Distributor = new MultiSelectPropertyValue()
            {
                MultiSelect = new List<SelectOption>() { new SelectOption() { Name = Notion_Properites_Mapping.AnimeSaturn } }
            };
            properties.Add(Notion_Properites_Mapping.Distributor, Distributor);

            //Type
            SelectPropertyValue Type = new SelectPropertyValue()
            {
                Select = new SelectOption() { Name = Property_Type(animeModel.media_type) }
            };
            properties.Add(Notion_Properites_Mapping.Type, Type);

            //Watched
            SelectPropertyValue Watched = new SelectPropertyValue()
            {
                Select = new SelectOption() { Name = Notion_Properites_Mapping.ToWatch }
            };
            properties.Add(Notion_Properites_Mapping.Watched, Watched);

            //Started Airing
            DatePropertyValue Started_Airing = new DatePropertyValue()
            {
                Date = new Date() { Start = animeModel.start_date }
            };
            properties.Add(Notion_Properites_Mapping.Started_Airing, Started_Airing);

            //Cover
            FilesPropertyValue Cover = new FilesPropertyValue()
            {
                Files = new List<FileObjectWithName>() { new ExternalFileWithName() { Type = Notion_Properites_Mapping.External, Name = animeModel.title, External = new ExternalFileWithName.Info() { Url = animeModel.main_picture.medium } } }
            };
            properties.Add(Notion_Properites_Mapping.Cover, Cover);

            //MAL Id
            NumberPropertyValue MAL_Id = new NumberPropertyValue()
            {
                Number = animeModel.id
            };
            properties.Add(Notion_Properites_Mapping.MAL_Id, MAL_Id);

            //MAL Link
            UrlPropertyValue MAL_Link = new UrlPropertyValue()
            {
                Url = Property_MAL_Link(animeModel.id.ToString())
            };
            properties.Add(Notion_Properites_Mapping.MAL_Link, MAL_Link);

            #endregion

            PagesCreateParameters result = new PagesCreateParameters()
            {
                Properties = properties,
                Parent = new DatabaseParentInput() { DatabaseId = databaseId }
            };
            return result;
        }

        #region MAL to Notion Properties Mapping

        private string Property_Type(string MAL_Type)
        {
            switch (MAL_Type)
            {
                case MAL_Properties_Mapping.TV:
                    return Notion_Properites_Mapping.TV;
                case MAL_Properties_Mapping.Movie:
                    return Notion_Properites_Mapping.Movie;
                default:
                    return Notion_Properites_Mapping.TV;
            }
        }

        private string Property_MAL_Link(string MAL_Id)
        {
            return $"{MAL_Properties_Mapping.Base_MAL_URL}{MAL_Id}";
        }

        #endregion

        #endregion
    }
}
