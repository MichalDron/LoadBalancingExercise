using LoadBalancing.Abstractions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoadBalancing.HeartBeatChecker.Extensions;
using LoadBalancing.Providers.Abstractions;

namespace LoadBalancing.HeartBeatChecker
{
    public class HeartBeatCheckService : IHostedService, IDisposable
    {
        private const int SUCCESSFUL_RECHECKED_MIN_COUNT = 2;
        private Timer _timer;
        private readonly IProviderStoreService _providerStoreService;
        private readonly TimeSpan _workerPeriod;
        private readonly IDictionary<string, int> _providerAvailabilityState;

        public HeartBeatCheckService(IProviderStoreService providerStoreService, HeartBeatCheckServiceOptions heartBeatCheckServiceOptions)
        {
            _providerAvailabilityState = new Dictionary<string, int>();
            _providerStoreService = providerStoreService;
            _workerPeriod = TimeSpan.FromSeconds(heartBeatCheckServiceOptions.CheckPeriodInSeconds);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.UtcNow} - {nameof(HeartBeatCheckService)}: Service  is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, _workerPeriod);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine($"{DateTime.UtcNow} - {nameof(HeartBeatCheckService)}: Service runs job");

            var providers = _providerStoreService.GetAllProviders();

            foreach (var provider in providers)
            {
                CheckProvider(provider);
            }
        }

        private void CheckProvider(IProvider provider)
        {
            bool isProviderAvailable = false;
            try
            {
                isProviderAvailable = provider.check();
            }
            catch (Exception)
            {
                //Console.WriteLine(e);
                isProviderAvailable = false;
            }

            if (!isProviderAvailable)
            {
                _providerStoreService.ExcludeProvider(provider.Id);
                _providerAvailabilityState[provider.Id] = SUCCESSFUL_RECHECKED_MIN_COUNT;
            }
            else
            {
                if (_providerAvailabilityState.ContainsKey(provider.Id) && _providerAvailabilityState[provider.Id] > 0)
                {
                    _providerStoreService.IncludeProvider(provider.Id);
                    _providerAvailabilityState[provider.Id]--;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.UtcNow} - {nameof(HeartBeatCheckService)}: Service  is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
