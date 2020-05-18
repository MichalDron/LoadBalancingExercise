using System.Collections.Generic;
using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing.Abstractions
{
    public interface IProviderStoreService
    {
        IList<IProvider> GetAvailableProviders();

        IList<IProvider> GetAllProviders();

        void Register(IProvider provider);

        int MaxProviderLimit { get; }

        void ExcludeProvider(string id);

        void IncludeProvider(string id);
    }
}
