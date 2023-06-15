using Data_AnimeToNotion.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_AnimeToNotion.Repository
{
    public interface IRelationRepository
    {
        void AddRelation(Relation rel);
        //Guid? SearchParentRelation(List<int> involvedRelationMalIds);
        //void UpdateParentAnimeShowId(int malId, int malRelatedId, Guid animeShowParentId);
    }
}
