using Finbourne.Workflow.Sdk.Client;
using NUnit.Framework;
using Polly;
using RestSharp;

namespace Finbourne.Workflow.Sdk.Extensions.Tests
{
    [TestFixture]
    public class PollyApiRetryHandlerTest
    {
        [Test]
        public void CreateApiFactory_WhenRetryPolicyIsNull_AssignsDefaultRetryPolicy()
        {
            RetryConfiguration.RetryPolicy = null;

            new ApiFactory(new Client.Configuration());

            Assert.That(RetryConfiguration.RetryPolicy, Is.Not.Null);
            Assert.That(RetryConfiguration.RetryPolicy, Is.EqualTo(PollyApiRetryHandler.DefaultRetryPolicyWithFallback));
        }

        [Test]
        public void CreateApiFactory_WhenRetryPolicyIsAlreadyAssigned_ExistingRetryPolicyIsUsed()
        {
            var testPolicy = Policy.HandleResult<IRestResponse>(response => true).Retry();

            RetryConfiguration.RetryPolicy = testPolicy;
            var newFactory = new ApiFactory(new Client.Configuration());

            Assert.That(RetryConfiguration.RetryPolicy, Is.EqualTo(testPolicy));
        }

        [Test]
        public void CreateApiFactory_WhenRetryPolicyIsNull_AssignsDefaultAsyncRetryPolicy()
        {
            RetryConfiguration.AsyncRetryPolicy = null;

            new ApiFactory(new Client.Configuration());

            Assert.That(
                RetryConfiguration.AsyncRetryPolicy,
                Is.EqualTo(PollyApiRetryHandler.DefaultRetryPolicyWithFallbackAsync));
        }

        [Test]
        public void CreateApiFactory_WhenRetryPolicyIsAlreadyAssigned_ExistingAsyncRetryPolicyIsUsed()
        {
            var testPolicy = Policy.HandleResult<IRestResponse>(response => true).RetryAsync();

            RetryConfiguration.AsyncRetryPolicy = testPolicy;
            var newFactory = new ApiFactory(new Client.Configuration());

            Assert.That(RetryConfiguration.AsyncRetryPolicy, Is.EqualTo(testPolicy));
        }
    }
}
