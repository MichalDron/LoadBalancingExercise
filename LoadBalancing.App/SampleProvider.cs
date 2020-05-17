using LoadBalancing.Providers.Abstractions;
using System;

namespace LoadBalancing.App
{
    public class SampleProvider : IProvider
    {
        private readonly string _id;

        public SampleProvider()
        {
            _id = Guid.NewGuid().ToString();
        }

        public string Id => _id;

        public string get()
        {
            return _id;
        }
    }
}
