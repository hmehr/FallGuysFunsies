using MediatonicFunsies.Common.DAL;
using MediatonicFunsies.Common.Logic;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace MediatonicFunsies.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            services.AddSingleton<IConnection, Connection>();
            return services;
        }

        public static IServiceCollection AddDataClasses(this IServiceCollection services)
        {
            services.AddRedis();
            services.AddSingleton<IAnimalsRepository, AnimalsRepository>();
            return services;
        }

        public static IServiceCollection AddLogicClasses(this IServiceCollection services)
        {
            services.AddSingleton<IAnimalService, AnimalService>();
            return services;
        }

    }
}