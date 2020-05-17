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
                .AddLoadBalancer()
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
        public void GivenMultipleProvidersRegistered_WhenRunGet_ReturnsProviderValue()
        {
            // Arrange
            var provider1 = new ProviderFake("providerId1");
            var provider2 = new ProviderFake("providerId2");
            _providerStoreService.Register(provider1);
            _providerStoreService.Register(provider2);

            // Act
            var firstResult = _loadBalancer.get();
            var secondResult = _loadBalancer.get();
            var thirdResult = _loadBalancer.get();

            // Assert
            Assert.AreEqual(provider1.get(), firstResult);
            Assert.AreEqual(provider1.get(), secondResult);
            Assert.AreEqual(provider1.get(), thirdResult);
        }
    }
}
