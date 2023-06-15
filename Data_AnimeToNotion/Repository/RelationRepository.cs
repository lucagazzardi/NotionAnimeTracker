using Data_AnimeToNotion.Context;
using Data_AnimeToNotion.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_AnimeToNotion.Repository
{
    public class RelationRepository : IRelationRepository
    {
        private readonly AnimeShowContext _animeShowContext;

        public RelationRepository(AnimeShowContext animeShowContext)
        {
            _animeShowContext = animeShowContext;
        }   

        public void AddRelation(Relation rel)
        {
            rel.Id = new Guid();
            _animeShowContext.Relations.Add(rel);
            _animeShowContext.SaveChanges();
        }

        //public Guid? SearchParentRelation(List<int> involvedRelationMalIds)
        //{
        //    return _animeShowContext.Relations.FirstOrDefault(x => x.AnimeShowParentId != null && (involvedRelationMalIds.Contains(x.AnimeMalId) || involvedRelationMalIds.Contains(x.AnimeRelatedMalId)))?.AnimeShowParentId;
        //}

        //public void UpdateParentAnimeShowId(int malId, int malRelatedId, Guid animeShowParentId)
        //{
        //    _animeShowContext.Relations.FromSql($"[dbo].[RecursiveRelationParentAnime] @MalId = {malId}, @MalRelatedId = {malRelatedId}, @AnimeParentId = {animeShowParentId}");
        //}
    }
}
