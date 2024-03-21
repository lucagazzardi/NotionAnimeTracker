using Business_AnimeToNotion.Integrations.Demo.DemoModels;
using Business_AnimeToNotion.Integrations.Internal;
using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Internal;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Repository;
using JikanDotNet;
using JikanDotNet.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Notion.Client;

namespace Business_AnimeToNotion.Integrations.Demo
{
    public class Demo_Integration : IDemo_Integration
    {
        #region DI

        private readonly IJikan _jikan;

        private readonly IConfiguration _configuration;
        private readonly IAnimeShowRepository _animeShowRepository;
        private readonly IMAL_Integration _malIntegration;
        private readonly IInternal_Integration _internalIntegration;
        private readonly ISyncToNotionRepository _syncToNotionRepository;

        #endregion

        private NotionClient Client { get; set; }
        private string DataBaseId { get; set; } = null;

        public Demo_Integration(
                IConfiguration configuration,
                IAnimeShowRepository animeShowRepository,
                IMAL_Integration malIntegration,
                IInternal_Integration internalIntegration,
                ISyncToNotionRepository syncToNotionRepository
            )
        {
            #region DI
            _configuration = configuration;
            _animeShowRepository = animeShowRepository;
            _malIntegration = malIntegration;
            _internalIntegration = internalIntegration;
            _syncToNotionRepository = syncToNotionRepository;
            #endregion

            Client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = configuration["Notion-AuthToken"]
            });

            var config = new JikanClientConfiguration()
            {
                LimiterConfigurations = TaskLimiterConfiguration.None
            };
            _jikan = new Jikan(config);
        }

        /// <summary>
        /// Add Notion entries to database
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public async Task FromNotionToDB(string cursor)
        {
            await Notion_GetDataBaseId();

            await AddToDB(DataBaseId, cursor);
        }

        /// <summary>
        /// Creates missing NotionSyncs
        /// </summary>
        /// <returns></returns>
        public async Task CreateNotionSync()
        {
            await _syncToNotionRepository.CreateNotionSyncs();
        }

        /// <summary>
        /// Updates specific list of anime entries from Mal
        /// </summary>
        /// <returns></returns>
        public async Task UpdateMassiveFromMal(List<int> MalIds)
        {
            foreach(var id in MalIds)
            {
                var dbEntry = Mapping.Mapper.Map<INT_AnimeShowFull>(await _animeShowRepository.GetFull(id));

                // Retrieve MAL anime record by Id
                INT_AnimeShowFull MalEntry = await _malIntegration.GetAnimeById(_configuration["MAL_ApiConfig:MAL_Header"], _configuration["MAL-ApiKey"], $"{_configuration["MAL_ApiConfig:MAL_BaseURL"]}anime/{id}?{_configuration["MAL_ApiConfig:MAL_NotionNeededFields"]}");

                // Check if there are differences
                var changes = CheckDifferences(MalEntry, dbEntry);

                // Update if there are differences
                await UpdateItem(changes, dbEntry);
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
                    Query = _configuration["Notion_ApiConfig:Notion_DatabaseName"]
                });
                DataBaseId = database.Results.Single(x => ((Database)x).Title[0].PlainText == _configuration["Notion_ApiConfig:Notion_DatabaseName"]).Id;
            }
        }

        private async Task AddToDB(string DataBaseId, string cursor)
        {
            List<Page> result = new List<Page>();
            PaginatedList<Page> retrievedNotionPages = null;
            //Retrieve the pages that need to be updated
            do
            {
                retrievedNotionPages = await Client.Databases.QueryAsync(DataBaseId, new DatabasesQueryParameters()
                {
                    PageSize = 10,
                    StartCursor = cursor != "null" ? cursor : null
                });
                cursor = retrievedNotionPages.NextCursor;
                result.AddRange(retrievedNotionPages.Results);

                foreach (var show in result)
                {
                    try
                    {
                        await MappingAndAdding(show);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error for: " + show.Id + ": " + ex);
                    }

                }
            }
            while (retrievedNotionPages.HasMore);
        }

        private async Task MappingAndAdding(Page page)
        {
            int animeId = (int)((NumberPropertyValue)page.Properties["MAL Id"]).Number.Value;

            if (await _animeShowRepository.Exists(animeId))
                return;

            var anime = await _jikan.GetAnimeAsync(animeId);
            await Task.Delay(1000);

            var added = await _animeShowRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(Mapping.Mapper.Map<INT_AnimeShowFull>(anime.Data)),
                Mapping.Mapper.ProjectTo<Studio>(Mapping.Mapper.Map<INT_AnimeShowFull>(anime.Data).Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(Mapping.Mapper.Map<INT_AnimeShowFull>(anime.Data).Genres.AsQueryable()).ToList()
            );

            await _syncToNotionRepository.CreateNotionSync(added, page.Id);

            var edit = Mapping.Mapper.Map<INT_AnimeShowEdit>(page);
            edit.Id = added.Id;
            await _internalIntegration.EditAnime(edit, skipSync: true);
        }

        #endregion

        #region Private Update From Mal

        private Demo_Changes_MalToInternal CheckDifferences(INT_AnimeShowFull malShow, INT_AnimeShowFull internalShow)
        {
            Demo_Changes_MalToInternal result = new Demo_Changes_MalToInternal();

            result.Changes = EvaluateChanges(malShow, internalShow);
            result.ChangedAnime = malShow;
            return result;
        }

        private AnimeShow SetBasicChanges(AnimeShow show, INT_AnimeShowFull source, List<string> changes)
        {
            foreach (var change in changes)
            {
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

        private List<string> EvaluateChanges(INT_AnimeShowFull mappedMalShow, INT_AnimeShowFull internalShow)
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

        private async Task UpdateItem(Demo_Changes_MalToInternal changes, INT_AnimeShowFull dbEntry)
        {
            if (changes.Changes.Count == 0)
                return;

            var anime = await _animeShowRepository.GetForEdit(dbEntry.Info.Id);

            anime = SetBasicChanges(anime, changes.ChangedAnime, changes.Changes);

            await _animeShowRepository.SyncFromMal(
                anime,
                changes.Changes.Contains("Studios") ? Mapping.Mapper.ProjectTo<Studio>(changes.ChangedAnime.Studios.AsQueryable()).ToList() : null,
                changes.Changes.Contains("Genres") ? Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(changes.ChangedAnime.Genres.AsQueryable()).ToList() : null
            );

            await _syncToNotionRepository.SetToSyncNotion(anime.Id, "Edit", malListToSync: false);
        }

        #endregion
    }
}
