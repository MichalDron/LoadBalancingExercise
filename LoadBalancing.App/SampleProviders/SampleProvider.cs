using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing.App.SampleProviders
{
    public class SampleProvider : IProvider
    {
        private readonly string _id;
        private readonly bool _check;

        public SampleProvider(string id, bool check = true)
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
