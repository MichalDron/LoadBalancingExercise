using LoadBalancing.Providers.Abstractions;
using System.Collections.Generic;

namespace LoadBalancing.Services
{
    internal class ProviderComparer : IComparer<IProvider>
    {
        public int Compare(IProvider x, IProvider y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
}
