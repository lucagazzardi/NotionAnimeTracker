using AutoMapper;
using Business_AnimeToNotion.Mapper.Entity_History;
using Business_AnimeToNotion.Mapper.Entity_Internal;
using Business_AnimeToNotion.Mapper.Entity_Notion;
using Business_AnimeToNotion.Mapper.FromMal;
using Business_AnimeToNotion.Mapper.Internal_Entity;
using Business_AnimeToNotion.Mapper.MAL_Internal;
using Business_AnimeToNotion.Mapper.NotionSync_MAL;
using Business_AnimeToNotion.Mapper.String_Notion;

namespace Business_AnimeToNotion.Mapper.Config
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<Demo_Profile>();
                cfg.AddProfile<String_NotionProperty_Reverse_Profile>();
                cfg.AddProfile<JIKAN_Internal_Profile>();
                cfg.AddProfile<Internal_Entity_Profile>();
                cfg.AddProfile<Entity_Internal_Profile>();
                cfg.AddProfile<Entity_History_Profile>();
                cfg.AddProfile<MAL_Internal_Profile>();
                cfg.AddProfile<Entity_Notion_Profile>();
                cfg.AddProfile<NotionSync_MAL_Profile>();                
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
