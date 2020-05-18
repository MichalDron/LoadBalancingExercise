﻿using System.Collections.Generic;
using System.Linq;
using LoadBalancing.Exceptions;
using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing.Services
{
    internal class ProviderStoreService : IProviderStoreService
    {
        private const int MAX_PROVIDERS_LIMIT = 10;
        private readonly IDictionary<string, IProvider> _providerStore;

        public ProviderStoreService()
        {
            _providerStore = new Dictionary<string, IProvider>();
        }

        public void Register(IProvider provider)
        {
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
            return _providerStore.Values.ToList();
        }

        public int MaxProviderLimit => MAX_PROVIDERS_LIMIT;
    }
}