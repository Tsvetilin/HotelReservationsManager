using AutoMapper;
using System.Reflection;

/// <summary>
/// Controller layer related to automated objects mapping
/// </summary>
namespace Services.Mapping
{
    /// <summary>
    /// Default AutoMapper configuration provider
    /// </summary>
    public static class MappingConfig
    {
        public static IMapper Instance { get; private set; }

        /// <summary>
        /// Register AutoMapper predefined maps from assemblies
        /// </summary>
        /// <param name="assemblies">Assemblies to search for mappings profiles from </param>
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
