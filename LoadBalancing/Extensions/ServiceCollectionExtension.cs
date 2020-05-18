﻿using LoadBalancing.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using LoadBalancing.Algorithms.Abstractions;

namespace LoadBalancing.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLoadBalancer(this IServiceCollection serviceCollection, Action<LoadBalancerOptions> options = null)
        {
            serviceCollection
                .AddSingleton<IProviderStoreService, ProviderStoreService>()
                .AddSingleton<ILoadBalancer, LoadBalancer>();

            var loadBalancerOptions = new LoadBalancerOptions();
            if (options != null)
            {
                options.Invoke(loadBalancerOptions);
            }

            serviceCollection
                .AddSingleton(typeof(IInvocationAlgorithm), loadBalancerOptions.InvocationAlgorithmType);

            return serviceCollection;
        }
    }
}