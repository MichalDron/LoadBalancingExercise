using LoadBalancing.Extensions;
using Microsoft.Extensions.DependencyInjection;
using LoadBalancing.Algorithms.RoundRobin;
using LoadBalancing.HeartBeatChecker.Extensions;
using System.Threading.Tasks;
using LoadBalancing.Abstractions;
using Microsoft.Extensions.Hosting;
using System;
using LoadBalancing.App.SampleProviders;
using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing.App
{
    public class Startup
    {
        public static Task RunAsync()
        {
            var host = new HostBuilder().ConfigureServices((hostContext, services) =>
           {
               services
                   .AddLoadBalancer(options =>
                   {
                       options.SetInvocationAlgorithm<RoundRobinInvocationAlgorithm>();
                       options.MaximumNumberOfParallelRequests = 2;
                   })
                   .AddLoadBalancerHeartBeatCheckService(options => options.CheckPeriodInSeconds = 3)
                   .AddHostedService<LoadBalancerService>();
           }).Build();

            RegisterProviders(host);

            return host.RunAsync();
        }

        private static void RegisterProviders(IHost host)
        {
            var providerStoreService = host.Services.GetService<IProviderStoreService>();

            int i = 0;
            for (; i < 4; i++)
            {
                IProvider provider = new EveryNthRequestUnavailableProvider($"providerId{i}", i * 2, i * 2);
                providerStoreService.Register(provider);
                Console.WriteLine($"Registered provider with id = '{provider.Id}'");
            }

            for (; i >= 4 && i < 8; i++)
            {
                IProvider provider = new NeverAvailableProvider($"providerId{i}");
                providerStoreService.Register(provider);
                Console.WriteLine($"Registered provider with id = '{provider.Id}'");
            }

            for (; i >= 8 && i < 10; i++)
            {
                IProvider provider = new AlwaysAvailableProvider($"providerId{i}");
                providerStoreService.Register(provider);
                Console.WriteLine($"Registered provider with id = '{provider.Id}'");
            }
        }
    }
}
