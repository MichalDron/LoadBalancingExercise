using LoadBalancing.Providers.Abstractions;
using System;

namespace LoadBalancing.App
{
    public class SampleProvider : IProvider
    {
        private readonly string _id;

        public SampleProvider(string id)
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
