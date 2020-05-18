using LoadBalancing.Providers.Abstractions;
using System.Collections.Generic;

namespace LoadBalancing.Algorithms.Abstractions
{
    public interface IInvocationAlgorithm
    {
        IProvider GetProvider(IList<IProvider> providers);
    }
}
