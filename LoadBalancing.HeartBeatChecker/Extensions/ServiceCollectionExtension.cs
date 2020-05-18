using Microsoft.Extensions.DependencyInjection;
using System;

namespace LoadBalancing.HeartBeatChecker.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLoadBalancerHeartBeatCheckService(this IServiceCollection serviceCollection, Action<HeartBeatCheckServiceOptions> options = null)
        {
            serviceCollection
                .AddHostedService<HeartBeatCheckService>();

            var heartBeatCheckServiceOptions = new HeartBeatCheckServiceOptions();
            if (options != null)
            {
                options.Invoke(heartBeatCheckServiceOptions);
            }

            serviceCollection.AddSingleton<HeartBeatCheckServiceOptions>(heartBeatCheckServiceOptions);

            return serviceCollection;
        }
    }
}
