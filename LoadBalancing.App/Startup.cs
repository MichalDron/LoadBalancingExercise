using LoadBalancing.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using LoadBalancing.Algorithms.RoundRobin;

namespace LoadBalancing.App
{
    public class Startup
    {
        public static void Run()
        {
            var serviceProvider = new ServiceCollection()
                .AddLoadBalancer(options => options.SetInvocationAlgorithm<RoundRobinInvocationAlgorithm>())
                .BuildServiceProvider();

            ILoadBalancer loadBalancer = serviceProvider.GetService<ILoadBalancer>();

            IProviderStoreService providerStoreService = serviceProvider.GetService<IProviderStoreService>();

            for (int i = 0; i < 10; i++)
            {
                SampleProvider provider = new SampleProvider($"providerId{i}");
                providerStoreService.Register(provider);
                Console.WriteLine($"Registered provider with id = '{provider.Id}'");
            }

            Console.WriteLine("Before Exclusion");

            for (int i = 0; i < 15; i++)
            {
                var result = loadBalancer.get();

                Console.WriteLine($"Request was handled by provider with id = '{result}'");
            }

            providerStoreService.ExcludeProvider("providerId2");
            providerStoreService.ExcludeProvider("providerId4");
            providerStoreService.ExcludeProvider("providerId6");

            Console.WriteLine("With Exclusion");

            for (int i = 0; i < 15; i++)
            {
                var result = loadBalancer.get();

                Console.WriteLine($"Request was handled by provider with id = '{result}'");
            }

            providerStoreService.IncludeProvider("providerId4");

            Console.WriteLine("With Inclusion 1 excluded provider");

            for (int i = 0; i < 15; i++)
            {
                var result = loadBalancer.get();

                Console.WriteLine($"Request was handled by provider with id = '{result}'");
            }
        }
    }
}
