using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing.Tests.Fakes
{
    internal class ProviderFake : IProvider
    {
        private readonly string _id;

        public ProviderFake(string id)
        {
            _id = id;
        }

        public string Id => _id;

        public string get()
        {
            return _id;
        }
    }
}
