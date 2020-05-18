using System.Linq;
using LoadBalancing.Providers.Abstractions;
using LoadBalancing.Exceptions;
using LoadBalancing.Algorithms.Abstractions;

namespace LoadBalancing.Services
{
    internal class LoadBalancer : ILoadBalancer
    {
        private readonly IProviderStoreService _providerStoreService;
        private readonly IInvocationAlgorithm _invocationAlgorithm;

        public LoadBalancer(IProviderStoreService providerStoreService, IInvocationAlgorithm invocationAlgorithm)
        {
            _providerStoreService = providerStoreService;
            _invocationAlgorithm = invocationAlgorithm;
        }

        public string get()
        {
            var provider = GetProvider();

            return provider.get();
        }

        private IProvider GetProvider()
        {
            var providers = _providerStoreService.GetProviders();

            if (!providers.Any())
            {
                throw new NoProviderAvailableException();
            }

            return _invocationAlgorithm.GetProvider(providers);
        }
    }
}
