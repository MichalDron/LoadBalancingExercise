using LoadBalancing.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LoadBalancing.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLoadBalancer(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IProviderStoreService, ProviderStoreService>()
                .AddSingleton<ILoadBalancer, LoadBalancer>();

            return serviceCollection;
        }
    }
}
