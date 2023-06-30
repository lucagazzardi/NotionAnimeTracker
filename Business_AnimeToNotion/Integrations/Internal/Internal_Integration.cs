using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Integrations.Notion;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Notion;
using Business_AnimeToNotion.Model.Notion.Base;
using Data_AnimeToNotion.DataModel;
using Data_AnimeToNotion.Repository;
using JikanDotNet;
using Microsoft.EntityFrameworkCore.Metadata;
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
        private readonly IMAL_Integration _mal;

        public Internal_Integration(IAnimeShowRepository animeRepository, INotion_Integration notion, IHostEnvironment hostEnvironment, IMAL_Integration mal)
        {
            _animeRepository = animeRepository;
            _notion = notion;
            _hostEnvironment = hostEnvironment;
            _mal = mal;

            _jikan = new Jikan();
        }

        /// <summary>
        /// Add new anime base
        /// </summary>
        /// <param name="animeAdd"></param>
        /// <returns></returns>
        public async Task AddNewAnimeBase(INT_AnimeShowBase animeAdd)
        {
            if (animeAdd.Info != null)
                return;

            var relations = Mapping.Mapper.ProjectTo<INT_AnimeShowRelation>((await _mal.GetRelationsFromMAL(animeAdd.MalId)).related_anime.AsQueryable()).ToList();

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
        public async Task AddNewAnimeFull(INT_AnimeShowFull animeAdd)
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
        /// Get anime full for editing
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowFull> GetAnimeForEdit(Guid Id)
        {
            return Mapping.Mapper.Map<INT_AnimeShowFull>(await _animeRepository.GetFull(Id));
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

        public async Task<NotionSyncAdd> AddNewAnimeBaseDemo(INT_AnimeShowBase animeAdd)
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
