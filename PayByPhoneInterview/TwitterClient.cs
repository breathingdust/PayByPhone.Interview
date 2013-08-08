using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using PayByPhoneInterview.Proxies;

namespace PayByPhoneInterview
{
    public interface ITwitterClient
    {
        List<Proxies.Tweet> GetTwoWeeksOfTweets(string userId, string screenName);
    }

    public class TwitterClient : ITwitterClient
    {
        private readonly IAppSettingsService appSettingsService;
        private readonly IOAuthService oAuthService;
        private readonly IWebRequestWrapper webRequestWrapper;

        public TwitterClient(IAppSettingsService appSettingsService, IOAuthService oAuthService,
                             IWebRequestWrapper webRequestWrapper)
        {
            this.appSettingsService = appSettingsService;
            this.oAuthService = oAuthService;
            this.webRequestWrapper = webRequestWrapper;
        }

        public List<Proxies.Tweet> GetTwoWeeksOfTweets(string userId, string screenName)
        {
            var searchUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json?user_id={0}&screen_name={1}&count=100";

            searchUrl = string.Format(searchUrl, Uri.EscapeDataString(userId), Uri.EscapeDataString(screenName));

            var uri = new Uri(searchUrl);

            var jsonResult = webRequestWrapper.GetResponseAsString(uri, oAuthService.CreateAuthorizationHeader(
                uri,
                "GET",
                appSettingsService.Get(AppSettings.Twitter.ConsumerKey),
                appSettingsService.Get(AppSettings.Twitter.ConsumerSecret),
                appSettingsService.Get(AppSettings.Twitter.AccessToken),
                appSettingsService.Get(AppSettings.Twitter.AccessTokenSecret)
                                                                            ));

            return JsonConvert.DeserializeObject<List<Proxies.Tweet>>(jsonResult, new TwitterDateTimeConverter());
        }
    }
}

