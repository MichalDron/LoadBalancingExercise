using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing.Tests.Fakes
{
    internal class ProviderFake : IProvider
    {
        private readonly string _id;
        private readonly bool _check;

        public ProviderFake(string id, bool check = true)
        {
            _id = id;
            _check = check;
        }

        public string Id => _id;

        public bool check()
        {
            return _check;
        }

        public string get()
        {
            return _id;
        }
    }
}
