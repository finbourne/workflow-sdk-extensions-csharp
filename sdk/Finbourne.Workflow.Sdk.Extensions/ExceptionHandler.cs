using System;
using Finbourne.Workflow.Sdk.Client;

namespace Finbourne.Workflow.Sdk.Extensions
{
    /// <summary>
    /// Handles the generation of exceptions after receiving the ApiResponse
    /// </summary>
    internal static class ExceptionHandler
    {
        /// <summary>
        /// Generate exceptions from the ApiResponse when ResponseStatus is an Error,
        /// and StatusCode has no content or is less than 400
        /// </summary>
        /// <param name="methodName">The name of the method that was being called</param>
        /// <param name="response">The ApiResponse</param>
        public static Exception CustomExceptionFactory(string methodName, IApiResponse response)
        {
            // Use default exception handler first (only use subsequent checks if this returns null)
            Exception defaultException = Client.Configuration.DefaultExceptionFactory.Invoke(methodName, response);
            if (defaultException != null)
            {
                return defaultException;
            }

            // Throw if ErrorText has been populated:
            //  - Internal SDK deserialization errors will result in ErrorText to be not null.
            //  - Network-level errors will also result in ErrorText being populated.
            //  - OpenAPI generator for Drive SDK attempts to use an XmlDeserializer as default
            //    choice for content which fails generating ErrorText !=null so we also need to
            //    check content type != application/octet-stream
            if (response.ErrorText != null && !(response.Headers.ContainsKey(CONTENT_TYPE) && response.Headers[CONTENT_TYPE][0].Equals(OCTET_STREAM)))
            {
                return new ApiException(
                    (int)response.StatusCode,
                    $"Internal SDK error occurred when calling {methodName}: {response.ErrorText}",
                    response.ErrorText,
                    response.Headers
                );
            }

            return null;
        }

        private const string CONTENT_TYPE = "Content-Type";
        private const string OCTET_STREAM = "application/octet-stream";
    }
}