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
        public void GivenProvidersWithSameId_WhenRegisterSecondProvider_ThenThrowsException()
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
        public void GivenEmptyProvidersStore_WhenRegisterUnderMaxLimit_ThenExecuteSuccessfully(int providersToRegisterCount)
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
        public void GivenEmptyProvidersStore_WhenRegisterMaxLimit_ThenExecuteSuccessfully()
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
        public void GivenMaxProvidersWithUniqueId_WhenRegisterOverLimit_ThenThrowsException()
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

        [Test]
        public void GivenEmptyProvidersStore_WhenExcludeProvider_ThenThrowsException()
        {
            // Act & Assert
            Assert.Throws<ProviderWithIdNotRegisteredException>(() => _providerStoreService.ExcludeProvider("providerNotRegistered"));
        }

        [Test]
        public void GivenEmptyProvidersStore_WhenIncludeProvider_ThenThrowsException()
        {
            // Act & Assert
            Assert.Throws<ProviderWithIdNotRegisteredException>(() => _providerStoreService.IncludeProvider("providerNotRegistered"));
        }

        [Test]
        public void GivenMultipleProvidersRegistered_WhenProvidersIncludedTwice_ThenDoesNotThrow()
        {
            // Arrange
            string providerId1 = "providerIdExcluded";
            var provider1 = new ProviderFake(providerId1);
            _providerStoreService.Register(provider1);
            _providerStoreService.ExcludeProvider(providerId1);

            _providerStoreService.IncludeProvider(providerId1);

            // Act & Assert
            Assert.DoesNotThrow(() => _providerStoreService.IncludeProvider(providerId1));
            Assert.AreEqual(1, _providerStoreService.GetAvailableProviders().Count);
        }

        [Test]
        public void GivenMultipleProvidersRegistered_WhenProvidersExcludedTwice_ThenDoesNotThrow()
        {
            // Arrange
            string providerId1 = "providerIdExcluded";
            var provider1 = new ProviderFake(providerId1);
            _providerStoreService.Register(provider1);
            _providerStoreService.ExcludeProvider(providerId1);

            // Act & Assert
            Assert.DoesNotThrow(() => _providerStoreService.ExcludeProvider(providerId1));
            Assert.AreEqual(0, _providerStoreService.GetAvailableProviders().Count);
        }

        [Test]
        public void GivenMultipleProvidersRegistered_WhenProvidersExcludedAndIncludedAgain_ThenProvidersUsed()
        {
            // Arrange
            string providerId1 = "providerId1";
            string providerId2 = "providerId2";
            var provider1 = new ProviderFake(providerId1);
            var provider2 = new ProviderFake(providerId2);
            _providerStoreService.Register(provider1);
            _providerStoreService.Register(provider2);

            Assert.AreEqual(2, _providerStoreService.GetAvailableProviders().Count);

            // Act
            _providerStoreService.ExcludeProvider(providerId1);

            // Assert
            Assert.AreEqual(1, _providerStoreService.GetAvailableProviders().Count);

            // Act
            _providerStoreService.IncludeProvider(providerId1);

            // Assert
            Assert.AreEqual(2, _providerStoreService.GetAvailableProviders().Count);
        }
    }
}
