using System;
using System.IO;
using System.Net;
using System.Text;

namespace PayByPhoneInterview
{
    public class WebRequestWrapper : IWebRequestWrapper
    {
        public string GetResponseAsString(Uri uri, string authorizationHeader)
        {
            var webRequest = WebRequest.Create(uri);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            ServicePointManager.Expect100Continue = false;
            webRequest.Headers["Authorization"] = authorizationHeader;

            var response = webRequest.GetResponse() as HttpWebResponse;

            if (response != null)
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
            return String.Empty;
        }
    }
}