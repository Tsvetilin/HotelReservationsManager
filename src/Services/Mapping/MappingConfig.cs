using AutoMapper;
using System.Reflection;

namespace Services.Mapping
{
    public static class MappingConfig
    {
        public static IMapper Instance { get; private set; }

        public static void RegisterMappings(params Assembly[] assemblies)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(assemblies);
            });

            Instance = new Mapper(config);
        }
    }
}
