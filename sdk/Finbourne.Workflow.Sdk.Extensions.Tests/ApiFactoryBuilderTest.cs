using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Finbourne.Workflow.Sdk.Extensions.Tests
{
    [TestFixture]
    public class ApiFactoryBuilderTest
    {
        private string _secretsFile;
        [OneTimeSetUp]
        public void CreateDummySecretsFile()
        {
            _secretsFile = Path.GetTempFileName();
            var secrets = new Dictionary<string, object>
            {
                ["api"] = new Dictionary<string, string>()
                {
                    {string.Format("{0}Url", "workflow"), "https://sub-domain.lusid.com/api"},
                    {"tokenUrl", "https://sub-domain.okta.com/oauth2/abcd123/v1/token"},
                    {"clientId", "<clientId>"},
                    {"clientSecret", "<clientSecret>"},
                    {"applicationName", "<applicationName>"},                    
                    {"username", "<username>"},
                    {"password", "<password>"},                    
                }
            };
            var json = JsonSerializer.Serialize(secrets);
            File.WriteAllText(_secretsFile, json);
        }

        [Test]
        public void Build_From_Secrets_Returns_NonNull_ApiFactory()
        {
            var apiConfig = ApiConfigurationBuilder.Build(_secretsFile);
            var apiFactory = new ApiFactory(apiConfig);
            Assert.IsNotNull(apiFactory);
        }

        //Test requires [assembly: InternalsVisibleTo("Finbourne.Workflow.Sdk.Extensions.Tests")] in ClientCredentialsFlowTokenProvider
        [Test]
        public void Build_From_Configuration_Returns_NonNull_ApiFactory()
        {
            var config = new TokenProviderConfiguration(new ClientCredentialsFlowTokenProvider(ApiConfigurationBuilder.Build(_secretsFile)))
            {
                BasePath = "base path"
            };
            var apiFactory = new ApiFactory(config);
            Assert.IsNotNull(apiFactory);
        }
    }
}
