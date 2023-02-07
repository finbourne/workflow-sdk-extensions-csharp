using System.Collections.Generic;

namespace Finbourne.Workflow.Sdk.Extensions
{
    /// <summary>
    /// Configuration for the ClientCredentialsFlowTokenProvider, usually sourced from a "secrets.json" file
    /// </summary>
    public class ApiConfiguration
    {
        /// <summary>
        /// Url for the token provider
        /// </summary>
        public string TokenUrl { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// OAuth2 Client ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// OAuth2 Client Secret
        /// </summary>
        public string ClientSecret { get;  set; }

        /// <summary>
        /// Workflow Api Url
        /// </summary>
        public string WorkflowUrl { get; set; }

        /// <summary>
        /// Client Application name
        /// </summary>
        public string ApplicationName { get; set; }
        
        /// <summary>
        /// Configurable via FBN_ACCESS_TOKEN env variable - get the value from LUSID web 'Your Profile'->'Personal access tokens'.
        /// Takes precedence over other authentication factors if set.
        /// </summary>
        public string PersonalAccessToken { get; set; }

        internal bool MissingPersonalAccessTokenVariables =>
            string.IsNullOrWhiteSpace(PersonalAccessToken) ||
            string.IsNullOrWhiteSpace(WorkflowUrl);

        internal bool MissingSecretVariables =>
            string.IsNullOrWhiteSpace(TokenUrl) ||
            string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(ClientId) ||
            string.IsNullOrWhiteSpace(ClientSecret) ||
            string.IsNullOrWhiteSpace(WorkflowUrl);

        /// <summary>
        /// Checks if any of the required configuration values are missing
        /// </summary>
        /// <returns>true if there is any configuration details missing, call <see cref="GetMissingConfig"/> to obtain details of the missing configuration details</returns>
        public bool HasMissingConfig()
        {
            return MissingPersonalAccessTokenVariables && MissingSecretVariables;
        }

        /// <summary>
        /// Returns a list of the missing required configuration values
        /// </summary>
        /// <returns>List of missing configuration values or empty list if all configuration values are present</returns>
        public List<string> GetMissingConfig()
        {
            var missingConfig = new List<string>();

            if (!string.IsNullOrWhiteSpace(PersonalAccessToken))
            {
                if (MissingPersonalAccessTokenVariables)
                {
                    missingConfig.Add(nameof(WorkflowUrl));
                }
                return missingConfig; // in case PAC is to be used we don't care about the other properties
            }

            if (string.IsNullOrWhiteSpace(TokenUrl))
            {
                missingConfig.Add(nameof(TokenUrl));
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                missingConfig.Add(nameof(Username));
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                missingConfig.Add(nameof(Password));
            }

            if (string.IsNullOrWhiteSpace(ClientId))
            {
                missingConfig.Add(nameof(ClientId));
            }

            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                missingConfig.Add(nameof(ClientSecret));
            }

            if (string.IsNullOrWhiteSpace(WorkflowUrl))
            {
                missingConfig.Add(nameof(WorkflowUrl));
            }

            return missingConfig;
        }  
    }
}