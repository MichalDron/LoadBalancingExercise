using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LoadBalancing.Providers.Abstractions;
using LoadBalancing.Exceptions;
using LoadBalancing.Algorithms.Abstractions;
using LoadBalancing.Abstractions;
using System.Threading.Tasks;
using LoadBalancing.Extensions;

namespace LoadBalancing.Services
{
    internal class LoadBalancer : ILoadBalancer
    {
        private readonly IProviderStoreService _providerStoreService;
        private readonly IInvocationAlgorithm _invocationAlgorithm;
        private readonly BlockingCollection<Guid> _requestQueue;
        private readonly int _maximumNumberOfParallelRequests;

        public LoadBalancer(IProviderStoreService providerStoreService, IInvocationAlgorithm invocationAlgorithm, LoadBalancerOptions loadBalancerOptions)
        {
            _providerStoreService = providerStoreService;
            _invocationAlgorithm = invocationAlgorithm;
            _requestQueue = new BlockingCollection<Guid>();
            _maximumNumberOfParallelRequests = loadBalancerOptions.MaximumNumberOfParallelRequests;
        }

        public Task<string> get()
        {
            var providers = _providerStoreService.GetAvailableProviders();

            if (_requestQueue.Count > ParallelRequestLimit(providers.Count))
            {
                throw new ClusterCapacityLimitExceededException();
            }

            _requestQueue.Add(Guid.NewGuid());

            var provider = GetProvider(providers);

            return provider.get().ContinueWith(task =>
            {
                _requestQueue.Take();
                return task.Result;
            });
        }

        private IProvider GetProvider(IList<IProvider> providers)
        {
            if (!providers.Any())
            {
                throw new NoProviderAvailableException();
            }

            return _invocationAlgorithm.GetProvider(providers);
        }

        private int ParallelRequestLimit(int providersCount) => _maximumNumberOfParallelRequests * providersCount;
    }
}
