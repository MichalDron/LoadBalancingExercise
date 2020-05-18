using LoadBalancing.Algorithms.Abstractions;
using LoadBalancing.Providers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoadBalancing.Algorithms.RoundRobin
{
    public class RoundRobinInvocationAlgorithm : IInvocationAlgorithm
    {
        private int _providerIndex = 0;

        public IProvider GetProvider(IList<IProvider> providers)
        {
            if (providers == null || !providers.Any())
            {
                throw new ArgumentException("Providers list should not be empty.");
            }

            bool shouldRestartIndexCounting = _providerIndex >= providers.Count - 1;
            if (shouldRestartIndexCounting)
            {
                _providerIndex = 0;
            }

            var provider = providers[_providerIndex];

            _providerIndex++;

            return provider;
        }
    }
}
