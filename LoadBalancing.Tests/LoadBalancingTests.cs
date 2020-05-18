using System.Collections.Generic;
using System.Linq;
using LoadBalancing.Algorithms.RoundRobin;
using LoadBalancing.Exceptions;
using LoadBalancing.Extensions;
using LoadBalancing.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LoadBalancing.Tests
{
    [Description("Load Balancing test with RoundRobin algorithm used.")]
    public class LoadBalancingTests
    {
        private ILoadBalancer _loadBalancer;

        private IProviderStoreService _providerStoreService;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddLoadBalancer(options => options.SetInvocationAlgorithm<RoundRobinInvocationAlgorithm>())
                .BuildServiceProvider();

            _loadBalancer = serviceProvider.GetService<ILoadBalancer>();

            _providerStoreService = serviceProvider.GetService<IProviderStoreService>();
        }

        [Test]
        public void GivenNoProvidersRegistered_WhenRunGet_ThenThrowsException()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<NoProviderAvailableException>(() => _loadBalancer.get());
        }

        [Test]
        public void GivenMultipleProvidersRegisteredWithRoundRobinAlgorithm_WhenRunGet_ThenReturnsDifferentProviderValue()
        {
            // Arrange
            var provider1 = new ProviderFake("providerId1");
            var provider2 = new ProviderFake("providerId2");
            var provider3 = new ProviderFake("providerId3");
            _providerStoreService.Register(provider1);
            _providerStoreService.Register(provider2);
            _providerStoreService.Register(provider3);

            // Act
            var result = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                result.Add(_loadBalancer.get());
            }

            // Assert
            Assert.AreEqual(3, result.Where(x => x == provider1.Id).Count());
            Assert.AreEqual(2, result.Where(x => x == provider2.Id).Count());
            Assert.AreEqual(2, result.Where(x => x == provider3.Id).Count());
        }

        [Test]
        public void GivenMultipleProvidersRegistered_WhenRunGet_ThenDoesNotFail()
        {
            // Arrange
            var provider1 = new ProviderFake("providerId1");
            var provider2 = new ProviderFake("providerId2");
            _providerStoreService.Register(provider1);
            _providerStoreService.Register(provider2);

            // Act & Assert
            for (int i = 0; i < 100; i++)
            {
                Assert.DoesNotThrow(() => _loadBalancer.get());
            }
        }

        [Test]
        public void GivenMultipleProvidersRegisteredWithFewExcluded_WhenRunGet_ThenProvidersNotUsed()
        {
            // Arrange
            var provider1 = new ProviderFake("providerId1");
            var provider2 = new ProviderFake("providerId2");
            var provider3 = new ProviderFake("providerId3");
            var provider4 = new ProviderFake("providerId4");
            var provider5 = new ProviderFake("providerId5");
            _providerStoreService.Register(provider1);
            _providerStoreService.Register(provider2);
            _providerStoreService.Register(provider3);
            _providerStoreService.Register(provider4);
            _providerStoreService.Register(provider5);

            _providerStoreService.ExcludeProvider(provider1.Id);
            _providerStoreService.ExcludeProvider(provider4.Id);

            // Act
            var result = new List<string>();
            for (int i = 0; i < 13; i++)
            {
                result.Add(_loadBalancer.get());
            }

            // Assert
            Assert.AreEqual(0, result.Where(x => x == provider1.Id).Count());
            Assert.AreEqual(5, result.Where(x => x == provider2.Id).Count());
            Assert.AreEqual(4, result.Where(x => x == provider3.Id).Count());
            Assert.AreEqual(0, result.Where(x => x == provider4.Id).Count());
            Assert.AreEqual(4, result.Where(x => x == provider5.Id).Count());
        }
    }
}
