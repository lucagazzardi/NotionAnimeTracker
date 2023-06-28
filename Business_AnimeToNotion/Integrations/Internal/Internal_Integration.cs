using Business_AnimeToNotion.Integrations.Notion;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Notion;
using Business_AnimeToNotion.Model.Notion.Base;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Repository;
using JikanDotNet;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Integrations.Internal
{
    public class Internal_Integration : IInternal_Integration
    {
        private readonly IJikan _jikan;
        private readonly IAnimeShowRepository _animeRepository;
        private readonly INotion_Integration _notion;
        private readonly IHostEnvironment _hostEnvironment;

        public Internal_Integration(IAnimeShowRepository animeRepository, INotion_Integration notion, IHostEnvironment hostEnvironment)
        {
            _animeRepository = animeRepository;
            _notion = notion;
            _hostEnvironment = hostEnvironment;

            _jikan = new Jikan();
        }

        /// <summary>
        /// Add new anime 
        /// </summary>
        /// <param name="animeAdd"></param>
        /// <returns></returns>
        public async Task AddNewAnimeBase(INT_AnimeShowAddBase animeAdd)
        { 
            var relations = Mapping.Mapper.Map<List<INT_AnimeShowRelation>>((await _jikan.GetAnimeRelationsAsync(animeAdd.MalId)).Data.ToList());

            await _animeRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(animeAdd),
                Mapping.Mapper.ProjectTo<Studio>(animeAdd.Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(animeAdd.Genres.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Relation>(relations.AsQueryable()).ToList()
            );

            if(_hostEnvironment.IsProduction())
                _notion.SendSyncToNotion(new NotionSyncAdd() { Type = OperationType.Add, NotionAddObject = Mapping.Mapper.Map<NotionAddObject>(animeAdd) });
        }

        /// <summary>
        /// Add new anime with relations
        /// </summary>
        /// <param name="animeAdd"></param>
        /// <returns></returns>
        public async Task AddNewAnimeFull(INT_AnimeShowAddFull animeAdd)
        {
            await _animeRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(animeAdd),
                Mapping.Mapper.ProjectTo<Studio>(animeAdd.Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(animeAdd.Genres.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Relation>(animeAdd.Relations.AsQueryable()).ToList()
            );

            if (_hostEnvironment.IsProduction())
                _notion.SendSyncToNotion(new NotionSyncAdd() { Type = OperationType.Add, NotionAddObject = Mapping.Mapper.Map<NotionAddObject>(animeAdd) });
        }

        /// <summary>
        /// Remove anime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveAnime(Guid id)
        {
            await _animeRepository.RemoveInternalAnimeShow(id);
        }

        #region Demo

        public async Task<NotionSyncAdd> AddNewAnimeBaseDemo(INT_AnimeShowAddBase animeAdd)
        {
            var relations = Mapping.Mapper.Map<List<INT_AnimeShowRelation>>((await _jikan.GetAnimeRelationsAsync(animeAdd.MalId)).Data.ToList());

            await _animeRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(animeAdd),
                Mapping.Mapper.ProjectTo<Studio>(animeAdd.Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(animeAdd.Genres.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Relation>(relations.AsQueryable()).ToList()
            );

            var result = new NotionSyncAdd() { Type = OperationType.Add, NotionAddObject = Mapping.Mapper.Map<NotionAddObject>(animeAdd) };
            result.NotionAddObject.NotionEditObject = new NotionEditObject() { Status = "To Watch" };
            return result;
        }

        #endregion
    }
}
