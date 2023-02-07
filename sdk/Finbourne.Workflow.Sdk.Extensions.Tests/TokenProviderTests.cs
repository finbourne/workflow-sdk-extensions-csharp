using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Finbourne.Workflow.Sdk.Extensions
{
    public class TokenProviderTests
    {
        private static readonly Lazy<ApiConfiguration> ApiConfig =
            new Lazy<ApiConfiguration>(() => ApiConfigurationBuilder.Build("secrets.json"));
        [Test]
        public async Task CanGetNewTokenWhenRefreshTokenExpired()
        {
            if (ApiConfig.Value.MissingSecretVariables)
            {
                Assert.Inconclusive();
            }

            var provider = new ClientCredentialsFlowTokenProvider(ApiConfig.Value);
            var _ = await provider.GetAuthenticationTokenAsync();
            var firstTokenDetails = provider.GetLastToken();
            Assert.That(firstTokenDetails.RefreshToken, Is.Not.Null.And.Not.Empty, "refresh_token not returned so unable to verify refresh behaviour.");
            Console.WriteLine($"Token expiring at {firstTokenDetails.ExpiresOn:o}");

            // WHEN we pretend to delay until both...
            // (1) the original token has expired (for expediency update the expires_on on the token)
            provider.ExpireToken();
            // (2) the refresh token has expired (for expediency update the refresh_token to an invalid value that will not be found)
            provider.GetLastToken().RefreshToken = "InvalidRefreshToken";
            Assert.That(DateTimeOffset.UtcNow, Is.GreaterThan(firstTokenDetails.ExpiresOn));
            var refreshedToken = await provider.GetAuthenticationTokenAsync();
            
            // THEN it should be populated, and the ExpiresOn should be in the future
            Assert.That(refreshedToken, Is.Not.Empty);
            Assert.That(provider.GetLastToken().ExpiresOn, Is.GreaterThan(DateTimeOffset.UtcNow));
        }
    }
}