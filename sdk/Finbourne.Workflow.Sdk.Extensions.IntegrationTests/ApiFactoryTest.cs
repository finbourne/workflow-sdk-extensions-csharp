using Finbourne.Notifications.Sdk.Api;
using Finbourne.Notifications.Sdk.Client;
using Finbourne.Notifications.Sdk.Model;
using NUnit.Framework;
using System;

namespace Finbourne.Notifications.Sdk.Extensions.IntegrationTests
{
    public class ApiFactoryTest
    {
        private IApiFactory _factory;

        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = IntegrationTestApiFactoryBuilder.CreateApiFactory("secrets.json");
        }

        [Test]
        public void Create_ApplicationMetadataApi()
        {
            var api = _factory.Api<ApplicationMetadataApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<ApplicationMetadataApi>());
        }

        [Test]
        public void Create_EventsApi()
        {
            var api = _factory.Api<EventsApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<EventsApi>());
        }

        [Test]
        public void Create_EventTypesApi()
        {
            var api = _factory.Api<EventTypesApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<EventTypesApi>());
        }

        [Test]
        public void Create_NotificationsApi()
        {
            var api = _factory.Api<NotificationsApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<NotificationsApi>());
        }

        [Test]
        public void Create_SubscriptionsApi()
        {
            var api = _factory.Api<SubscriptionsApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<SubscriptionsApi>());
        }

        [Test]
        public void Api_From_Interface()
        {
            var api = _factory.Api<IApplicationMetadataApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<ApplicationMetadataApi>());
        }

        [Test]
        public void Api_From_Implementation()
        {
            var api = _factory.Api<ApplicationMetadataApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<ApplicationMetadataApi>());
        }

        [Test]
        public void Invalid_Requested_Api_Throws()
        {
            Assert.That(() => _factory.Api<InvalidApi>(), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ApplicationMetadataApi_ListAccessControlledResources()
        {
            var config = IntegrationTestApiFactoryBuilder.CreateApiConfiguration("secrets.json");
            ITokenProvider tokenProvider;
            if (config.MissingSecretVariables)
            {
                tokenProvider = new PersonalAccessTokenProvider(config.PersonalAccessToken);
            }
            else 
            {
                tokenProvider = new ClientCredentialsFlowTokenProvider(ApiConfigurationBuilder.Build("secrets.json")); 
            }

            var factory = ApiFactoryBuilder.Build(config.NotificationsUrl, tokenProvider);
            var api = factory.Api<ApplicationMetadataApi>();
            ResourceListOfAccessControlledResource resources = api.ListAccessControlledResources();
            Assert.IsNotNull(resources);
        }

        class InvalidApi : IApiAccessor
        {
            public IReadableConfiguration Configuration { get; set; }
            public string GetBasePath()
            {
                throw new NotImplementedException();
            }

            public ExceptionFactory ExceptionFactory { get; set; }
        }
    }
}
