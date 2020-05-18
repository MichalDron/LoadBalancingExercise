using System.Collections.Generic;
using LoadBalancing.Algorithms.RandomInvocationAlgorithm;
using LoadBalancing.Exceptions;
using LoadBalancing.Extensions;
using LoadBalancing.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LoadBalancing.Tests
{
    public class LoadBalancingTests
    {
        private ILoadBalancer _loadBalancer;

        private IProviderStoreService _providerStoreService;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddLoadBalancer(options => options.SetInvocationAlgorithm<RandomInvocationAlgorithm>())
                .BuildServiceProvider();

            _loadBalancer = serviceProvider.GetService<ILoadBalancer>();

            _providerStoreService = serviceProvider.GetService<IProviderStoreService>();
        }

        [Test]
        public void GivenNoProvidersRegistered_WhenRunGet_ThrowsException()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<NoProviderAvailableException>(() => _loadBalancer.get());
        }

        [Test]
        [Ignore("Non-deterministic, using Random function")]
        public void GivenMultipleProvidersRegistered_WhenRunGet_ReturnsDifferentProviderValue()
        {
            // Arrange
            var provider1 = new ProviderFake("providerId1");
            var provider2 = new ProviderFake("providerId2");
            _providerStoreService.Register(provider1);
            _providerStoreService.Register(provider2);

            // Act
            var result = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                result.Add(_loadBalancer.get());
            }

            // Assert
            Assert.True(result.Contains(provider1.Id));
            Assert.True(result.Contains(provider2.Id));
        }

        [Test]
        public void GivenMultipleProvidersRegistered_WhenRunGet_DoesNotFail()
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
    }
}
