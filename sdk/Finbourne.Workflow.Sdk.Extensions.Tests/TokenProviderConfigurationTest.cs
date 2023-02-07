using NUnit.Framework;

namespace Finbourne.Workflow.Sdk.Extensions.Tests
{
    [TestFixture]
    public class TokenProviderConfigurationTest
    {
        [Test]
        public void Construct_WithNullTokenProvider_Returns_NonNull()
        {
            var config = new TokenProviderConfiguration(null);
            Assert.IsNotNull(config);
        }

        [Test]
        public void Construct_WithNullTokenProvider_Returns_BasePathSet()
        {
            var config = new TokenProviderConfiguration(null);
            StringAssert.StartsWith($"https://www.lusid.com/", config.BasePath);
        }
    }
}
