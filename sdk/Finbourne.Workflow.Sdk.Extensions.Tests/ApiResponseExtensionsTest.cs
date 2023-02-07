using Finbourne.Workflow.Sdk.Client;
using NUnit.Framework;
using System;
using System.Net;

namespace Finbourne.Workflow.Sdk.Extensions.Tests
{
    [TestFixture]
    public class ApiResponseExtensionsTest
    {
        [Test]
        public void GetRequestDateTime_CanExtractAndParseAccurately_DateHeader()
        {
            var apiResponse = new ApiResponse<string>(
                statusCode: HttpStatusCode.OK,
                headers: new Multimap<string, string>()
                {
                    {"Date", "Tue, 09 Feb 2021 05:18:41 GMT"},
                },
                data: "data"
            );
            var date = apiResponse.GetRequestDateTime();
            Assert.That(date, Is.EqualTo(new DateTimeOffset(2021, 2, 9, 5, 18, 41, new TimeSpan())));
        }

        [Test]
        public void GetRequestDateTime_InvalidDateHeader_ReturnsNull_DateHeader()
        {
            var apiResponse = new ApiResponse<string>(
                statusCode: HttpStatusCode.OK,
                headers: new Multimap<string, string>()
                {
                    {"Date", "Tue, 09 Feb 2021 05:18:41 GMT"},
                },
                data: "data"
            );
            // Invalidate header containing access token
            apiResponse.Headers[ApiResponseExtensions.DateHeader] = new[] { "invalid" };
            var date = apiResponse.GetRequestDateTime();
            Assert.IsNull(date);
        }

        [Test]
        public void GetRequestDateTime_MissingHeader_ReturnsNull_DateHeader()
        {
            var apiResponse = new ApiResponse<string>(
                statusCode: HttpStatusCode.OK,
                headers: new Multimap<string, string>()
                {
                    {"Date", "Tue, 09 Feb 2021 05:18:41 GMT"},
                },
                data: "data"
            );
            // Remove header containing access token
            apiResponse.Headers.Remove(ApiResponseExtensions.DateHeader);
            var date = apiResponse.GetRequestDateTime();
            Assert.IsNull(date);
        }

        [Test]
        public void GetRequestDateTime_CanExtract_DateHeader()
        {
            var apiResponse = new ApiResponse<string>(
                statusCode: HttpStatusCode.OK,
                headers: new Multimap<string, string>()
                {
                    {"Date", "Tue, 09 Feb 2021 05:18:41 GMT"},
                },
                data: "data"
            );
            var date = apiResponse.GetRequestDateTime();
            Assert.IsNotNull(date);
        }

        [Test]
        public void GetRequestId_MissingHeader_ReturnsNull_RequestId()
        {
            var apiResponse = new ApiResponse<string>(
                statusCode: HttpStatusCode.OK,
                headers: new Multimap<string, string>()
                {
                    {"Date", "Tue, 09 Feb 2021 05:18:41 GMT"},
                },
                data: "data"                
            );
            // Remove header containing access token
            apiResponse.Headers.Remove(ApiResponseExtensions.RequestIdHeader);
            var requestId = apiResponse.GetRequestId();
            Assert.That(requestId, Is.Null);
        }

    }
}
