using AutoMapper;
using Business_AnimeToNotion.Model.Internal;
using Business_AnimeToNotion.Model.Notion;

namespace Business_AnimeToNotion.Mapper.Internal_Notion
{
    public class Internal_Notion_Profile : Profile
    {
        public Internal_Notion_Profile()
        {
            CreateMap<INT_AnimeShowAddBase, NotionAddObject>()
                .ForMember(dto => dto.NameOriginal, map => map.MapFrom(source => source.NameDefault))
                .ForMember(dto => dto.MalScore, map => map.MapFrom(source => source.Score != null ? source.Score : 0))
                .ForMember(dto => dto.Genres, map => map.MapFrom(source => source.Genres.Select(x => new NotionAddKeyValue() { Id = x.Id, Value = x.Value })))
                .ForMember(dto => dto.Studios, map => map.MapFrom(source => source.Studios.Select(x => new NotionAddKeyValue() { Id = x.Id, Value = x.Value })));
        }
    }
}
