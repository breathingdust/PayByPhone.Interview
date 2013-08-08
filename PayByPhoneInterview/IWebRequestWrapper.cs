using System;

namespace PayByPhoneInterview
{
    public interface IWebRequestWrapper
    {
        string GetResponseAsString(Uri uri, string authorizationHeader);
    }
}