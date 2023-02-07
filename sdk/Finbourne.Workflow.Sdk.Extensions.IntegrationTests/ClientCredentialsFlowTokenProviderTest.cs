using NUnit.Framework;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Finbourne.Notifications.Sdk.Extensions.Tests
{
    //Test requires [assembly: InternalsVisibleTo("Finbourne.Notifications.Sdk.Extensions.Tests")] in ClientCredentialsFlowTokenProvider
    [TestFixture]
    public class ClientCredentialsFlowTokenProviderTest
    {
        private static readonly Lazy<ApiConfiguration> ApiConfig =
            new Lazy<ApiConfiguration>(() => ApiConfigurationBuilder.Build("secrets.json"));

        [Test]
        public void GetAuthenticationHeaderAsync_Returns_NonNull()
        {
            if (ApiConfig.Value.MissingSecretVariables)
            {
                Assert.Inconclusive();
            }

            var tokenProvider = new ClientCredentialsFlowTokenProvider(ApiConfigurationBuilder.Build("secrets.json"));
            Task<AuthenticationHeaderValue> result = tokenProvider.GetAuthenticationHeaderAsync();
            result.Wait();
            Assert.IsNotNull(result.Result);
        }

        [Test]
        public void GetAuthenticationTokenAsync_Returns_NonNull()
        {
            if (ApiConfig.Value.MissingSecretVariables)
            {
                Assert.Inconclusive();
            }

            var tokenProvider = new ClientCredentialsFlowTokenProvider(ApiConfigurationBuilder.Build("secrets.json"));
            Task<string> result = tokenProvider.GetAuthenticationTokenAsync();
            result.Wait();
            Assert.IsNotNull(result.Result);
        }

        [Test]
        public void GetLastToken_Returns_NonNull()
        {
            if (ApiConfig.Value.MissingSecretVariables)
            {
                Assert.Inconclusive();
            }

            var tokenProvider = new ClientCredentialsFlowTokenProvider(ApiConfigurationBuilder.Build("secrets.json"));
            Task<string> result = tokenProvider.GetAuthenticationTokenAsync();
            result.Wait();
            ClientCredentialsFlowTokenProvider.AuthenticationToken token = tokenProvider.GetLastToken();
            Assert.IsNotNull(token);
        }

        [Test]
        public void GetLastToken_RefreshExpiresOn_Has_Expired_After_ExpireRefreshToken_Called()
        {
            if (ApiConfig.Value.MissingSecretVariables)
            {
                Assert.Inconclusive();
            }

            var tokenProvider = new ClientCredentialsFlowTokenProvider(ApiConfigurationBuilder.Build("secrets.json"));
            Task<string> result = tokenProvider.GetAuthenticationTokenAsync();
            result.Wait();
            tokenProvider.ExpireRefreshToken();
            ClientCredentialsFlowTokenProvider.AuthenticationToken token = tokenProvider.GetLastToken();
            Assert.That(DateTimeOffset.Compare(DateTimeOffset.UtcNow, token.RefreshExpiresOn) > 0);
        }

        [Test]
        public void GetLastToken_ExpiresOn_Has_Expired_After_ExpireToken_Called()
        {
            if (ApiConfig.Value.MissingSecretVariables)
            {
                Assert.Inconclusive();
            }

            var tokenProvider = new ClientCredentialsFlowTokenProvider(ApiConfigurationBuilder.Build("secrets.json"));
            Task<string> result = tokenProvider.GetAuthenticationTokenAsync();
            result.Wait();
            tokenProvider.ExpireToken();
            ClientCredentialsFlowTokenProvider.AuthenticationToken token = tokenProvider.GetLastToken();
            Assert.That(DateTimeOffset.Compare(DateTimeOffset.UtcNow, token.ExpiresOn) > 0);
        }
    }
}
