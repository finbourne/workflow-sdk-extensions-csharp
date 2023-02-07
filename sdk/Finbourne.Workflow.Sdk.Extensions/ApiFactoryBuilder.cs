namespace Finbourne.Workflow.Sdk.Extensions
{
    /// <summary>
    /// Builder class to build instances of IApiFactory
    /// </summary>
    public static class ApiFactoryBuilder
    {
        /// <summary>
        /// Create an IApiFactory using the specified configuration file.  
        /// </summary>
        public static IApiFactory Build(string apiSecretsFilename)
        {
            var apiConfig = ApiConfigurationBuilder.Build(apiSecretsFilename);
            return new ApiFactory(apiConfig);
        }

        /// <summary>
        /// Create an IApiFactory using the specified Url and Token Provider
        /// </summary>
        public static IApiFactory Build(string url, ITokenProvider tokenProvider)
        {
            // TokenProviderConfiguration.ApiClient is the client used by ApiFactory and is 
            // NOT thread-safe, so there needs to be a separate instance for each instance of ApiFactory.
            // Do NOT cache the ApiFactory instances (DEV-6922)
            var config = new TokenProviderConfiguration(tokenProvider)
            {
                BasePath = url
            };

            return new ApiFactory(config);
        }
    }
}