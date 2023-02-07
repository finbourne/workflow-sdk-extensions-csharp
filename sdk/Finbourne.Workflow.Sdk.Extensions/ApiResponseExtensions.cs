using System;
using Finbourne.Workflow.Sdk.Client;

namespace Finbourne.Workflow.Sdk.Extensions
{
    /// <summary>
    /// Extensions to the ApiResponse class which is returned when using the WithHttpInfo methods.
    /// </summary>
    public static class ApiResponseExtensions
    {
        /// <summary>
        /// The header that the request id is contained within.
        /// </summary>
        public const string RequestIdHeader = "workflow-meta-requestId";
        
        /// <summary>
        /// The header that the Date Time Offset is contained within.
        /// </summary>
        public const string DateHeader = "Date";
        
        /// <summary>
        /// Extracts the requestId from an ApiResponse
        /// </summary>
        public static string GetRequestId<T>(this ApiResponse<T> apiResponse)
        {
            // Extract requestId from Insights link contained in the Instance property
            return apiResponse.Headers.ContainsKey(RequestIdHeader) ? apiResponse.Headers[RequestIdHeader][0] : null;
        }
        
        /// <summary>
        /// Extracts the Date from an ApiResponse
        /// </summary>
        public static DateTimeOffset? GetRequestDateTime <T>(this ApiResponse<T> apiResponse)
        {
            if (!apiResponse.Headers.ContainsKey(DateHeader) ||
                !DateTimeOffset.TryParse(apiResponse.Headers[DateHeader][0], out var headerDateValue))
            {
                return null;
            }
            return headerDateValue;
        }
    }
}
