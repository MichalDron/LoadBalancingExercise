using System.Collections.Generic;
using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing
{
    public interface IProviderStoreService
    {
        IList<IProvider> GetAvailableProviders();

        IList<IProvider> GetProviders();

        void Register(IProvider provider);

        int MaxProviderLimit { get; }

        void ExcludeProvider(string id);

        void IncludeProvider(string id);
    }
}
