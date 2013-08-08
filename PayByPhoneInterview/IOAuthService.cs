using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace PayByPhoneInterview
{
    public interface IOAuthService
    {
        string GenerateNonce();
        string CreateAuthorizationHeader(Uri url, string httpMethod, string consumerKey, string consumerSecretKey, string token, string tokenSecret);
    }

    public class OAuthService : OAuthBase, IOAuthService
    {
        public string CreateAuthorizationHeader(Uri url, string httpMethod, string consumerKey, string consumerSecretKey, string token, string tokenSecret)
        {
            const string headerTemplate = "OAuth oauth_consumer_key=\"{0}\"," +
                                          "oauth_signature_method=\"HMAC-SHA1\"," +
                                          "oauth_timestamp=\"{3}\"," +
                                          "oauth_nonce=\"{1}\"," +
                                          "oauth_version=\"1.0\"," +
                                          "oauth_token=\"{4}\"," +
                                          "oauth_signature=\"{2}\"";

            string normalizedUrl;
            string normalizedParameters;

            string nonce = GenerateNonce();
            string timestamp = GenerateTimeStamp();

            return String.Format(headerTemplate,
                          consumerKey,
                          nonce,
                          Uri.EscapeDataString(GenerateSignature(url, consumerKey, consumerSecretKey, token, tokenSecret, httpMethod, timestamp, nonce, out normalizedUrl, out normalizedParameters)),
                          timestamp,
                          token
                );
        }

        public string GenerateNonce()
        {
            const string charsToUse = "abcdefghijklmnopqrstuvwxyz1234567890";
            var chars = charsToUse.ToCharArray();

            var result = String.Empty;

            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var buffer = new byte[32];
            rngCryptoServiceProvider.GetBytes(buffer);

            foreach (var @byte in buffer)
            {
                result += charsToUse[@byte % (chars.Length - 1)];
            }

            return result;
        }
    }
}