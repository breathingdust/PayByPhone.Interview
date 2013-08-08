using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PayByPhoneInterview.Test
{
    [TestFixture]
    public class IOAuthServiceTests
    {
        [Test]
        public void when_generating_an_nonce_value_result_should_be_random()
        {
            var oAuthService = new OAuthService();

            var actuals = new List<string>();

            // loop 100 times to ensure all values returned are unique

            for (int i = 0; i < 100; i++)
            {
                actuals.Add(oAuthService.GenerateNonce());

                var bum = actuals.GroupBy(x => x).Any(o => o.Count() > 1);

                Assert.IsFalse(bum);
            }
        }

        [Test]
        public void when_generating_an_authorization_header_consumer_key_should_be_assigned()
        {
            var oAuthService = new OAuthService();

            const string consumer_key = "consumerKey";

            var actual = oAuthService.CreateAuthorizationHeader(new Uri("http://test.com"), "httpMethod", consumer_key, "consumerSecretKey", "token", "tokenSecret");

            actual.Contains("oauth_consumer_key=\"" + consumer_key + "\"");
        }
    }
}