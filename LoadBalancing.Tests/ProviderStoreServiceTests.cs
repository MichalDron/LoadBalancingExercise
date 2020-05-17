using LoadBalancing.Exceptions;
using LoadBalancing.Extensions;
using LoadBalancing.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LoadBalancing.Tests
{
    public class ProviderStoreServiceTests
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
        public void GivenProvidersWithSameId_WhenRegisterSecondProvider_ThrowsException()
        {
            // Arrange
            string providerId = "nonUniqueProviderId";
            var provider1 = new ProviderFake(providerId);
            var provider2 = new ProviderFake(providerId);
            _providerStoreService.Register(provider1);

            // Act & Assert
            Assert.Throws<ProviderIdNotUniqueException>(() => _providerStoreService.Register(provider2));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void GivenEmptyProvidersStore_WhenRegisterUnderMaxLimit_ExecuteSuccessfully(int providersToRegisterCount)
        {
            // Arrange

            // Act
            for (int i = 0; i < providersToRegisterCount; i++)
            {
                _providerStoreService.Register(new ProviderFake($"providerId{i}"));
            }

            // Assert
            Assert.AreEqual(providersToRegisterCount, _providerStoreService.GetProviders().Count);
        }

        [Test]
        public void GivenEmptyProvidersStore_WhenRegisterMaxLimit_ExecuteSuccessfully()
        {
            // Arrange
            var maxProviderCount = _providerStoreService.MaxProviderLimit;

            // Act
            for (int i = 0; i < maxProviderCount; i++)
            {
                _providerStoreService.Register(new ProviderFake($"providerId{i}"));
            }

            // Assert
            Assert.AreEqual(maxProviderCount, _providerStoreService.GetProviders().Count);
        }

        [Test]
        public void GivenMaxProvidersWithUniqueId_WhenRegisterOverLimit_ThrowsException()
        {
            // Arrange
            var maxProviderCount = _providerStoreService.MaxProviderLimit;

            for (int i = 0; i < maxProviderCount; i++)
            {
                _providerStoreService.Register(new ProviderFake($"providerId{i}"));
            }

            // Act & Assert
            Assert.Throws<MaxProviderCountExceededException>(() => _providerStoreService.Register(new ProviderFake("providerOverLimit")));
        }
    }
}
