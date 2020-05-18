using LoadBalancing.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using LoadBalancing.Algorithms.RandomInvocationAlgorithm;

namespace LoadBalancing.App
{
    public class Startup
    {
        public static void Run()
        {
            var serviceProvider = new ServiceCollection()
                .AddLoadBalancer(options => options.SetInvocationAlgorithm<RandomInvocationAlgorithm>())
                .BuildServiceProvider();

            ILoadBalancer loadBalancer = serviceProvider.GetService<ILoadBalancer>();

            IProviderStoreService providerStoreService = serviceProvider.GetService<IProviderStoreService>();

            for (int i = 0; i < 10; i++)
            {
                SampleProvider provider = new SampleProvider($"providerId{i}");
                providerStoreService.Register(provider);
                Console.WriteLine($"Registered provider with id = '{provider.Id}'");
            }

            for (int i = 0; i < 20; i++)
            {
                var result = loadBalancer.get();

                Console.WriteLine($"Request was handled by provider with id = '{result}'");
            }
        }
    }
}
