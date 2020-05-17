using System.Collections.Generic;
using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing
{
    public interface IProviderStoreService
    {
        IList<IProvider> GetProviders();

        void Register(IProvider provider);

        int MaxProviderLimit { get; }
    }
}
