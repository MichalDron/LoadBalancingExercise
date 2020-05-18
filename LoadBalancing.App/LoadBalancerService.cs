using LoadBalancing.Abstractions;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoadBalancing.App
{
    internal class LoadBalancerService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IProviderStoreService _providerStoreService;
        private readonly ILoadBalancer _loadBalancer;

        public LoadBalancerService(
            ILoadBalancer loadBalancer,
            IProviderStoreService providerStoreService)
        {
            _loadBalancer = loadBalancer;
            _providerStoreService = providerStoreService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.UtcNow} - {nameof(LoadBalancerService)}: Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine($"{DateTime.UtcNow} - {nameof(LoadBalancerService)}: Service runs job");

            try
            {
                var result = _loadBalancer.get();

                Console.WriteLine($"{DateTime.UtcNow} - {nameof(LoadBalancerService)}: Request was handled by provider with id = '{result}'");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.UtcNow} - {nameof(LoadBalancerService)}: Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
