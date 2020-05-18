using LoadBalancing.Algorithms.Abstractions;
using LoadBalancing.Providers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoadBalancing.Algorithms.RandomInvocation
{
    public class RandomInvocationAlgorithm : IInvocationAlgorithm
    {
        private static readonly Random _random = new Random();

        public IProvider GetProvider(IList<IProvider> providers)
        {
            if (providers == null || !providers.Any())
            {
                throw new ArgumentException("Providers list should not be empty.");
            }

            int providerIndex = _random.Next(0, providers.Count);

            var provider = providers[providerIndex];

            return provider;
        }
    }
}
