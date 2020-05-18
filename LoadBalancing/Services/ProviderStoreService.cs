using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LoadBalancing.Exceptions;
using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing.Services
{
    internal class ProviderStoreService : IProviderStoreService
    {
        private const int MAX_PROVIDERS_LIMIT = 10;
        private readonly IDictionary<string, IProvider> _providerStore;
        private readonly ProviderComparer _providerComparer = new ProviderComparer();
        private readonly ISet<string> _excludedProvidersIds;

        public ProviderStoreService()
        {
            _providerStore = new Dictionary<string, IProvider>();
            _excludedProvidersIds = new HashSet<string>();
        }

        public void Register(IProvider provider)
        {
            if (provider.Id == null)
            {
                throw new ArgumentNullException();
            }

            if (_providerStore.Count >= MAX_PROVIDERS_LIMIT)
            {
                throw new MaxProviderCountExceededException();
            }

            if (_providerStore.ContainsKey(provider.Id))
            {
                throw new ProviderIdNotUniqueException();
            }

            _providerStore.Add(provider.Id, provider);
        }

        public IList<IProvider> GetProviders()
        {
            return _providerStore.Values.ToImmutableSortedSet(_providerComparer);
        }

        public IList<IProvider> GetAvailableProviders()
        {
            return _providerStore
                    .Where(x => !_excludedProvidersIds.Contains(x.Key))
                    .Select(x => x.Value)
                    .ToImmutableSortedSet(_providerComparer);
        }

        public void ExcludeProvider(string id)
        {
            if (!_providerStore.ContainsKey(id))
            {
                throw new ProviderWithIdNotRegisteredException();
            }

            _excludedProvidersIds.Add(id);
        }

        public void IncludeProvider(string id)
        {
            if (!_providerStore.ContainsKey(id))
            {
                throw new ProviderWithIdNotRegisteredException();
            }

            _excludedProvidersIds.Remove(id);
        }

        public int MaxProviderLimit => MAX_PROVIDERS_LIMIT;
    }
}
