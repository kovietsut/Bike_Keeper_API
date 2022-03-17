using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace bikekeeper.AutoMapper
{
    public class MappingConfiguration
    {
        public static void ConfigurationService(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<MappingProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
