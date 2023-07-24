using AutoMapper;
using Business_AnimeToNotion.Mapper.Entity_History;
using Business_AnimeToNotion.Mapper.Entity_Internal;
using Business_AnimeToNotion.Mapper.FromMal;
using Business_AnimeToNotion.Mapper.Internal_Entity;
using Business_AnimeToNotion.Mapper.Internal_Notion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                cfg.AddProfile<AnimeShowDto_AnimeShow_Profile>();
                cfg.AddProfile<MAL_AnimeShow_NotionProperties_Profile>();
                cfg.AddProfile<MAL_AnimeShow_AnimeShowDto_Profile>();
                cfg.AddProfile<MAL_AnimeShow_AnimeShow_Profile>();
                cfg.AddProfile<NotionSync_NotionProperties_Profile>();
                cfg.AddProfile<String_NotionProperty_Reverse_Profile>();
                cfg.AddProfile<FrontEnd_NotionProperties_Profile>();
                cfg.AddProfile<JIKAN_Internal_Profile>();
                cfg.AddProfile<Internal_Entity_Profile>();
                cfg.AddProfile<Internal_Notion_Profile>();
                cfg.AddProfile<Entity_Internal_Profile>();
                cfg.AddProfile<Entity_History_Profile>();

            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
