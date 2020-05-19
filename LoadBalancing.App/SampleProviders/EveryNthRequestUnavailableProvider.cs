using System;
using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing.App.SampleProviders
{
    public class EveryNthRequestUnavailableProvider : IProvider
    {
        private readonly string _id;
        private readonly int _nthDivisor;
        private readonly int _unavailableInRowCount;

        private int _counter = 0;
        private int _unavailableCounter = 0;

        public string Id => _id;

        public EveryNthRequestUnavailableProvider(string id, int nthDivisor, int unavailableInRowCount)
        {
            _id = id;
            _nthDivisor = nthDivisor;
            _unavailableInRowCount = unavailableInRowCount;
        }

        public bool check()
        {
            if (_counter < _nthDivisor)
            {
                _counter++;
            }
            else
            {
                if (_unavailableCounter < _unavailableInRowCount)
                {
                    _unavailableCounter++;
                    throw new Exception();
                }
                else
                {
                    _counter = 0;
                    _unavailableCounter = 0;
                }
            }

            return true;
        }

        public string get()
        {
            return _id;
        }
    }
}
