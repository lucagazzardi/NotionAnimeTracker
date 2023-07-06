using Business_AnimeToNotion.Integrations.MAL;
using Business_AnimeToNotion.Integrations.Notion;
using Business_AnimeToNotion.Mapper.Config;
using Business_AnimeToNotion.Model.Entities;
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
        public async Task<INT_AnimeShowPersonal> AddNewAnimeBase(INT_AnimeShowBase animeAdd)
        {
            if (animeAdd.Info != null)
                return null;

            var relations = Mapping.Mapper.ProjectTo<INT_AnimeShowRelation>((await _mal.GetRelationsFromMAL(animeAdd.MalId)).related_anime.AsQueryable()).ToList();

            var added = await _animeRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(animeAdd),
                Mapping.Mapper.ProjectTo<Studio>(animeAdd.Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(animeAdd.Genres.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Relation>(relations.AsQueryable()).ToList()
            );

            if(_hostEnvironment.IsProduction())
                _notion.SendSyncToNotion(new NotionSyncAdd() { Type = OperationType.Add, NotionAddObject = Mapping.Mapper.Map<NotionAddObject>(animeAdd) });

            return new INT_AnimeShowPersonal() { Id = added.Id, Status = added.Status };
        }

        /// <summary>
        /// Add new anime with relations
        /// </summary>
        /// <param name="animeAdd"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowPersonal> AddNewAnimeFull(INT_AnimeShowFull animeAdd)
        {
            var added = await _animeRepository.AddInternalAnimeShow(
                Mapping.Mapper.Map<AnimeShow>(animeAdd),
                Mapping.Mapper.ProjectTo<Studio>(animeAdd.Studios.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Data_AnimeToNotion.DataModel.Genre>(animeAdd.Genres.AsQueryable()).ToList(),
                Mapping.Mapper.ProjectTo<Relation>(animeAdd.Relations.AsQueryable()).ToList()
            );

            if (_hostEnvironment.IsProduction())
                _notion.SendSyncToNotion(new NotionSyncAdd() { Type = OperationType.Add, NotionAddObject = Mapping.Mapper.Map<NotionAddObject>(animeAdd) });

            return new INT_AnimeShowPersonal() { Id = added.Id, Status = added.Status };
        }

        /// <summary>
        /// Get anime full for editing
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowFull> GetAnimeFull(int malId)
        {
            var animeInt = await _animeRepository.GetFull(malId);

            if (animeInt == null)
                return Mapping.Mapper.Map<INT_AnimeShowFull>(await _mal.GetAnimeById(malId));

            return Mapping.Mapper.Map<INT_AnimeShowFull>(animeInt);
        }

        /// <summary>
        /// Get anime for editing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<INT_AnimeShowFull> GetAnimeForEdit(Guid id)
        {
            return Mapping.Mapper.Map<INT_AnimeShowFull>(await _animeRepository.GetFull(id));
        }

        /// <summary>
        /// Edit anime properties
        /// </summary>
        /// <param name="animeEdit"></param>
        /// <returns></returns>
        public async Task EditAnime(INT_AnimeShowEdit animeEdit)
        {
            var anime = await _animeRepository.GetForEdit(animeEdit.Id.Value);

            anime.Status = animeEdit.Status;

            if (animeEdit.PersonalScore != null)
                SetPersonalScore(anime, animeEdit);

            if (animeEdit.StartedOn != null || animeEdit.FinishedOn != null)
                await SetWatchingtime(anime, animeEdit);

            if (!string.IsNullOrEmpty(animeEdit.Notes))
                await SetNotes(anime, animeEdit);

            await _animeRepository.Update(anime);
        }

        /// <summary>
        /// Switch anime favorite
        /// </summary>
        /// <param name="id"></param>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public async Task<bool> SetAnimeFavorite(Guid id, bool favorite)
        {
            await _animeRepository.SetAnimeFavorite(id);
            return !favorite;
        }

        /// <summary>
        /// Switch anime plan to watch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public async Task<bool> SetAnimePlanToWatch(Guid id, bool planToWatch)
        {
            await _animeRepository.SetPlanToWatch(id);
            return !planToWatch;
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

        public async Task<List<INT_AnimeShowRelation>> GetAnimeRelations(int malId)
        {
            return Mapping.Mapper.ProjectTo<INT_AnimeShowRelation>((await _mal.GetRelationsFromMAL(malId)).related_anime.AsQueryable()).ToList();
        }

        #region Private

        private void SetPersonalScore(AnimeShow anime, INT_AnimeShowEdit edit)
        {
            if(anime.Score != null)
                anime.Score.PersonalScore = edit.PersonalScore;
        }

        private async Task SetWatchingtime(AnimeShow anime, INT_AnimeShowEdit edit)
        {
            if (anime.WatchingTime != null)
            {
                anime.WatchingTime.StartedOn = edit.StartedOn.Value;
                anime.WatchingTime.FinishedOn = edit.FinishedOn;
            }
            else
            {
                var watchingTime = new WatchingTime()
                {
                    Id = Guid.NewGuid(),
                    StartedOn = edit.StartedOn.Value,
                    FinishedOn = edit.FinishedOn,
                    CompletedYear = edit.CompletedYear?.Id ?? null
                };
                await _animeRepository.AddWatchingTime(watchingTime, anime);
            }

        }

        private async Task SetNotes(AnimeShow anime, INT_AnimeShowEdit edit)
        {
            if (anime.Note != null)
                anime.Note.Notes = edit.Notes;
            else
            {
                var note = new Note() { Id = Guid.NewGuid(), Notes = edit.Notes };
                await _animeRepository.AddNote(note, anime);
            }
        }

        #endregion

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
