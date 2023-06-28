using AutoMapper;
using Business_AnimeToNotion.Model.MAL;
using Data_AnimeToNotion.DataModel;

namespace Business_AnimeToNotion.Mapper
{
    public class MAL_AnimeShow_AnimeShow_Profile : Profile
    {
        public MAL_AnimeShow_AnimeShow_Profile()
        {
            CreateMap<MAL_AnimeShowRaw, List<Relation>>().ConvertUsing((src, dest) =>
            {
                dest = new List<Relation>();
                foreach (var relation in src.related_anime)
                {
                    dest.Add(new Relation()
                    {
                        Id = Guid.NewGuid(),
                        AnimeRelatedMalId = relation.node.id,
                        RelationType = relation.relation_type
                    });
                }
                return dest;
            });

        }
    }
}
