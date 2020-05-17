using LoadBalancing.Providers.Abstractions;
using System.Linq;
using LoadBalancing.Exceptions;

namespace LoadBalancing.Services
{
    internal class LoadBalancer : ILoadBalancer
    {
        private readonly IProviderStoreService _providerStoreService;

        public LoadBalancer(IProviderStoreService providerStoreService)
        {
            _providerStoreService = providerStoreService;
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

            return providers.FirstOrDefault();
        }
    }
}
