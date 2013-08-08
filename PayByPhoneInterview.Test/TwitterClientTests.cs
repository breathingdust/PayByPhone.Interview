using System;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap.AutoMocking;

namespace PayByPhoneInterview.Test
{
    [TestFixture]
    public class TwitterClientTests
    {
        [Test]
        public void client_when_queried_should_generate_an_authorization_header()
        {
            var autoMocker = new RhinoAutoMocker<TwitterClient>();

            const string consumerKey = "consumerKey";
            const string consumerKeySecret = "consumerKeySecret";
            const string accessKey = "accessKey";
            const string accessKeySecret = "accessKeySecret";

            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerKey))
                   .Return(consumerKey);

            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerSecret))
                   .Return(consumerKeySecret);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessToken))
                   .Return(accessKey);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessTokenSecret))
                   .Return(accessKeySecret);

            autoMocker.Get<IWebRequestWrapper>()
                      .Stub(o => o.GetResponseAsString(null, null))
                      .IgnoreArguments()
                      .Return(String.Empty);

            autoMocker.ClassUnderTest.GetTwoWeeksOfTweets("@payByPhone", "@payByPhone");

            autoMocker.Get<IOAuthService>()
                .AssertWasCalled(o => o.CreateAuthorizationHeader(Arg<Uri>.Is.Anything, Arg.Is("GET"), Arg.Is(consumerKey), Arg.Is(consumerKeySecret), Arg.Is(accessKey), Arg.Is(accessKeySecret)));
        }

        [Test]
        public void client_when_queried_returns_status()
        {
            var tweet_status = "@martialb41 les comptes Twitter sont de plus en plus pirat\u00e9s. Ceci n'a rien \u00e0 voir avec notre s\u00e9curit\u00e9 totalement ind\u00e9pendante de Twitter";

            #region twitter_json
            string reponseFromTwitterApi = @"[{
      ""created_at"":""Sat Jul 27 21:28:03 +0000 2013"",
      ""id"":361236564522700800,
      ""id_str"":""361236564522700800"",
      ""text"":""{0}"",
      ""source"":""\u003ca href=\""http:\/\/twitter.com\/download\/iphone\"" rel=\""nofollow\""\u003eTwitter for iPhone\u003c\/a\u003e"",
      ""truncated"":false,
      ""in_reply_to_status_id"":null,
      ""in_reply_to_status_id_str"":null,
      ""in_reply_to_user_id"":738483991,
      ""in_reply_to_user_id_str"":""738483991"",
      ""in_reply_to_screen_name"":""martialb41"",
      ""user"":{
         ""id"":31749548,
         ""id_str"":""31749548"",
         ""name"":""PayByPhone France"",
         ""screen_name"":""PayByPhone"",
         ""location"":""Paris"",
         ""description"":""Le paiement du stationnement par mobile et des services de mobilit\u00e9 urbaine : Nice, Boulogne, Issy, Le Havre, Londres, New-York, San Francisco ..."",
         ""url"":""https:\/\/t.co\/ALu15SjIv6"",
         ""entities"":{
            ""url"":{
               ""urls"":[
                  {
                     ""url"":""https:\/\/t.co\/ALu15SjIv6"",
                     ""expanded_url"":""https:\/\/www.paybyphone.fr"",
                     ""display_url"":""paybyphone.fr"",
                     ""indices"":[
                        0,
                        23
                     ]
                  }
               ]
            },
            ""description"":{
               ""urls"":[

               ]
            }
         },
         ""protected"":false,
         ""followers_count"":787,
         ""friends_count"":870,
         ""listed_count"":59,
         ""created_at"":""Thu Apr 16 12:24:56 +0000 2009"",
         ""favourites_count"":1,
         ""utc_offset"":7200,
         ""time_zone"":""Paris"",
         ""geo_enabled"":true,
         ""verified"":false,
         ""statuses_count"":331,
         ""lang"":""fr"",
         ""contributors_enabled"":false,
         ""is_translator"":false,
         ""profile_background_color"":""1A1B1F"",
         ""profile_background_image_url"":""http:\/\/a0.twimg.com\/profile_background_images\/747772723\/8e41dc058b909220da517c824f3f320f.jpeg"",
         ""profile_background_image_url_https"":""https:\/\/si0.twimg.com\/profile_background_images\/747772723\/8e41dc058b909220da517c824f3f320f.jpeg"",
         ""profile_background_tile"":false,
         ""profile_image_url"":""http:\/\/a0.twimg.com\/profile_images\/264234694\/PaybyPhone_element_normal.jpg"",
         ""profile_image_url_https"":""https:\/\/si0.twimg.com\/profile_images\/264234694\/PaybyPhone_element_normal.jpg"",
         ""profile_link_color"":""71BE44"",
         ""profile_sidebar_border_color"":""FFFFFF"",
         ""profile_sidebar_fill_color"":""252429"",
         ""profile_text_color"":""FAFAFA"",
         ""profile_use_background_image"":true,
         ""default_profile"":false,
         ""default_profile_image"":false,
         ""following"":null,
         ""follow_request_sent"":false,
         ""notifications"":null
      },
      ""geo"":null,
      ""coordinates"":null,
      ""place"":null,
      ""contributors"":null,
      ""retweet_count"":1,
      ""favorite_count"":0,
      ""entities"":{
         ""hashtags"":[

         ],
         ""symbols"":[

         ],
         ""urls"":[

         ],
         ""user_mentions"":[
            {
               ""screen_name"":""martialb41"",
               ""name"":""Martial \u271e"",
               ""id"":738483991,
               ""id_str"":""738483991"",
               ""indices"":[
                  0,
                  11
               ]
            }
         ]
      },
      ""favorited"":false,
      ""retweeted"":false,
      ""lang"":""fr""
   }]";
            #endregion

            reponseFromTwitterApi = reponseFromTwitterApi.Replace("{0}",tweet_status);

            var autoMocker = new RhinoAutoMocker<TwitterClient>();

            const string consumerKey = "consumerKey";
            const string consumerKeySecret = "consumerKeySecret";
            const string accessKey = "accessKey";
            const string accessKeySecret = "accessKeySecret";

            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerKey))
                   .Return(consumerKey);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerSecret))
                   .Return(consumerKeySecret);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessToken))
                   .Return(accessKey);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessTokenSecret))
                   .Return(accessKeySecret);

            autoMocker.Get<IWebRequestWrapper>()
                      .Stub(o => o.GetResponseAsString(null, null))
                      .IgnoreArguments()
                      .Return(reponseFromTwitterApi);

            var actual = autoMocker.ClassUnderTest.GetTwoWeeksOfTweets("@payByPhone", "@payByPhone");

            Assert.AreEqual(tweet_status,actual[0].Text);
        }

        [Test]
        public void client_when_queried_returns_date()
        {
            var tweet_date = new DateTime(2013, 7, 27, 21, 28, 03);

            #region twitter_json
            string reponseFromTwitterApi = @"[{
      ""created_at"":""Sat Jul 27 21:28:03 +0000 2013"",
      ""id"":361236564522700800,
      ""id_str"":""361236564522700800"",
      ""text"":""{0}"",
      ""source"":""\u003ca href=\""http:\/\/twitter.com\/download\/iphone\"" rel=\""nofollow\""\u003eTwitter for iPhone\u003c\/a\u003e"",
      ""truncated"":false,
      ""in_reply_to_status_id"":null,
      ""in_reply_to_status_id_str"":null,
      ""in_reply_to_user_id"":738483991,
      ""in_reply_to_user_id_str"":""738483991"",
      ""in_reply_to_screen_name"":""martialb41"",
      ""user"":{
         ""id"":31749548,
         ""id_str"":""31749548"",
         ""name"":""PayByPhone France"",
         ""screen_name"":""PayByPhone"",
         ""location"":""Paris"",
         ""description"":""Le paiement du stationnement par mobile et des services de mobilit\u00e9 urbaine : Nice, Boulogne, Issy, Le Havre, Londres, New-York, San Francisco ..."",
         ""url"":""https:\/\/t.co\/ALu15SjIv6"",
         ""entities"":{
            ""url"":{
               ""urls"":[
                  {
                     ""url"":""https:\/\/t.co\/ALu15SjIv6"",
                     ""expanded_url"":""https:\/\/www.paybyphone.fr"",
                     ""display_url"":""paybyphone.fr"",
                     ""indices"":[
                        0,
                        23
                     ]
                  }
               ]
            },
            ""description"":{
               ""urls"":[

               ]
            }
         },
         ""protected"":false,
         ""followers_count"":787,
         ""friends_count"":870,
         ""listed_count"":59,
         ""created_at"":""Thu Apr 16 12:24:56 +0000 2009"",
         ""favourites_count"":1,
         ""utc_offset"":7200,
         ""time_zone"":""Paris"",
         ""geo_enabled"":true,
         ""verified"":false,
         ""statuses_count"":331,
         ""lang"":""fr"",
         ""contributors_enabled"":false,
         ""is_translator"":false,
         ""profile_background_color"":""1A1B1F"",
         ""profile_background_image_url"":""http:\/\/a0.twimg.com\/profile_background_images\/747772723\/8e41dc058b909220da517c824f3f320f.jpeg"",
         ""profile_background_image_url_https"":""https:\/\/si0.twimg.com\/profile_background_images\/747772723\/8e41dc058b909220da517c824f3f320f.jpeg"",
         ""profile_background_tile"":false,
         ""profile_image_url"":""http:\/\/a0.twimg.com\/profile_images\/264234694\/PaybyPhone_element_normal.jpg"",
         ""profile_image_url_https"":""https:\/\/si0.twimg.com\/profile_images\/264234694\/PaybyPhone_element_normal.jpg"",
         ""profile_link_color"":""71BE44"",
         ""profile_sidebar_border_color"":""FFFFFF"",
         ""profile_sidebar_fill_color"":""252429"",
         ""profile_text_color"":""FAFAFA"",
         ""profile_use_background_image"":true,
         ""default_profile"":false,
         ""default_profile_image"":false,
         ""following"":null,
         ""follow_request_sent"":false,
         ""notifications"":null
      },
      ""geo"":null,
      ""coordinates"":null,
      ""place"":null,
      ""contributors"":null,
      ""retweet_count"":1,
      ""favorite_count"":0,
      ""entities"":{
         ""hashtags"":[

         ],
         ""symbols"":[

         ],
         ""urls"":[

         ],
         ""user_mentions"":[
            {
               ""screen_name"":""martialb41"",
               ""name"":""Martial \u271e"",
               ""id"":738483991,
               ""id_str"":""738483991"",
               ""indices"":[
                  0,
                  11
               ]
            }
         ]
      },
      ""favorited"":false,
      ""retweeted"":false,
      ""lang"":""fr""
   }]";
            #endregion

            var autoMocker = new RhinoAutoMocker<TwitterClient>();

            const string consumerKey = "consumerKey";
            const string consumerKeySecret = "consumerKeySecret";
            const string accessKey = "accessKey";
            const string accessKeySecret = "accessKeySecret";

            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerKey))
                   .Return(consumerKey);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerSecret))
                   .Return(consumerKeySecret);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessToken))
                   .Return(accessKey);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessTokenSecret))
                   .Return(accessKeySecret);

            autoMocker.Get<IWebRequestWrapper>()
                      .Stub(o => o.GetResponseAsString(null, null))
                      .IgnoreArguments()
                      .Return(reponseFromTwitterApi);

            var actual = autoMocker.ClassUnderTest.GetTwoWeeksOfTweets("@payByPhone", "@payByPhone");

            Assert.AreEqual(tweet_date, actual[0].created_at.ToUniversalTime());
        }

        [Test]
        public void client_when_queried_returns_account()
        {
            var twitter_account = "PayByPhone";

            #region twitter_json
            string reponseFromTwitterApi = @"[{
      ""created_at"":""Sat Jul 27 21:28:03 +0000 2013"",
      ""id"":361236564522700800,
      ""id_str"":""361236564522700800"",
      ""text"":""@martialb41 les comptes Twitter sont de plus en plus pirat\u00e9s. Ceci n'a rien \u00e0 voir avec notre s\u00e9curit\u00e9 totalement ind\u00e9pendante de Twitter"",
      ""source"":""\u003ca href=\""http:\/\/twitter.com\/download\/iphone\"" rel=\""nofollow\""\u003eTwitter for iPhone\u003c\/a\u003e"",
      ""truncated"":false,
      ""in_reply_to_status_id"":null,
      ""in_reply_to_status_id_str"":null,
      ""in_reply_to_user_id"":738483991,
      ""in_reply_to_user_id_str"":""738483991"",
      ""in_reply_to_screen_name"":""martialb41"",
      ""user"":{
         ""id"":31749548,
         ""id_str"":""31749548"",
         ""name"":""PayByPhone France"",
         ""screen_name"":""{0}"",
         ""location"":""Paris"",
         ""description"":""Le paiement du stationnement par mobile et des services de mobilit\u00e9 urbaine : Nice, Boulogne, Issy, Le Havre, Londres, New-York, San Francisco ..."",
         ""url"":""https:\/\/t.co\/ALu15SjIv6"",
         ""entities"":{
            ""url"":{
               ""urls"":[
                  {
                     ""url"":""https:\/\/t.co\/ALu15SjIv6"",
                     ""expanded_url"":""https:\/\/www.paybyphone.fr"",
                     ""display_url"":""paybyphone.fr"",
                     ""indices"":[
                        0,
                        23
                     ]
                  }
               ]
            },
            ""description"":{
               ""urls"":[

               ]
            }
         },
         ""protected"":false,
         ""followers_count"":787,
         ""friends_count"":870,
         ""listed_count"":59,
         ""created_at"":""Thu Apr 16 12:24:56 +0000 2009"",
         ""favourites_count"":1,
         ""utc_offset"":7200,
         ""time_zone"":""Paris"",
         ""geo_enabled"":true,
         ""verified"":false,
         ""statuses_count"":331,
         ""lang"":""fr"",
         ""contributors_enabled"":false,
         ""is_translator"":false,
         ""profile_background_color"":""1A1B1F"",
         ""profile_background_image_url"":""http:\/\/a0.twimg.com\/profile_background_images\/747772723\/8e41dc058b909220da517c824f3f320f.jpeg"",
         ""profile_background_image_url_https"":""https:\/\/si0.twimg.com\/profile_background_images\/747772723\/8e41dc058b909220da517c824f3f320f.jpeg"",
         ""profile_background_tile"":false,
         ""profile_image_url"":""http:\/\/a0.twimg.com\/profile_images\/264234694\/PaybyPhone_element_normal.jpg"",
         ""profile_image_url_https"":""https:\/\/si0.twimg.com\/profile_images\/264234694\/PaybyPhone_element_normal.jpg"",
         ""profile_link_color"":""71BE44"",
         ""profile_sidebar_border_color"":""FFFFFF"",
         ""profile_sidebar_fill_color"":""252429"",
         ""profile_text_color"":""FAFAFA"",
         ""profile_use_background_image"":true,
         ""default_profile"":false,
         ""default_profile_image"":false,
         ""following"":null,
         ""follow_request_sent"":false,
         ""notifications"":null
      },
      ""geo"":null,
      ""coordinates"":null,
      ""place"":null,
      ""contributors"":null,
      ""retweet_count"":1,
      ""favorite_count"":0,
      ""entities"":{
         ""hashtags"":[

         ],
         ""symbols"":[

         ],
         ""urls"":[

         ],
         ""user_mentions"":[
            {
               ""screen_name"":""martialb41"",
               ""name"":""Martial \u271e"",
               ""id"":738483991,
               ""id_str"":""738483991"",
               ""indices"":[
                  0,
                  11
               ]
            }
         ]
      },
      ""favorited"":false,
      ""retweeted"":false,
      ""lang"":""fr""
   }]";
            #endregion

            reponseFromTwitterApi = reponseFromTwitterApi.Replace("{0}", twitter_account);

            var autoMocker = new RhinoAutoMocker<TwitterClient>();

            const string consumerKey = "consumerKey";
            const string consumerKeySecret = "consumerKeySecret";
            const string accessKey = "accessKey";
            const string accessKeySecret = "accessKeySecret";

            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerKey))
                   .Return(consumerKey);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerSecret))
                   .Return(consumerKeySecret);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessToken))
                   .Return(accessKey);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessTokenSecret))
                   .Return(accessKeySecret);

            autoMocker.Get<IWebRequestWrapper>()
                      .Stub(o => o.GetResponseAsString(null, null))
                      .IgnoreArguments()
                      .Return(reponseFromTwitterApi);

            var actual = autoMocker.ClassUnderTest.GetTwoWeeksOfTweets("@payByPhone", "@payByPhone");

            Assert.AreEqual(twitter_account, actual[0].user.screen_name);
        }

        [Test]
        public void client_when_queried_returns_mentions()
        {
            var number_of_mentions = 1;

            #region twitter_json
            string reponseFromTwitterApi = @"[{
      ""created_at"":""Sat Jul 27 21:28:03 +0000 2013"",
      ""id"":361236564522700800,
      ""id_str"":""361236564522700800"",
      ""text"":""@martialb41 les comptes Twitter sont de plus en plus pirat\u00e9s. Ceci n'a rien \u00e0 voir avec notre s\u00e9curit\u00e9 totalement ind\u00e9pendante de Twitter"",
      ""source"":""\u003ca href=\""http:\/\/twitter.com\/download\/iphone\"" rel=\""nofollow\""\u003eTwitter for iPhone\u003c\/a\u003e"",
      ""truncated"":false,
      ""in_reply_to_status_id"":null,
      ""in_reply_to_status_id_str"":null,
      ""in_reply_to_user_id"":738483991,
      ""in_reply_to_user_id_str"":""738483991"",
      ""in_reply_to_screen_name"":""martialb41"",
      ""user"":{
         ""id"":31749548,
         ""id_str"":""31749548"",
         ""name"":""PayByPhone France"",
         ""screen_name"":""PayByPhone"",
         ""location"":""Paris"",
         ""description"":""Le paiement du stationnement par mobile et des services de mobilit\u00e9 urbaine : Nice, Boulogne, Issy, Le Havre, Londres, New-York, San Francisco ..."",
         ""url"":""https:\/\/t.co\/ALu15SjIv6"",
         ""entities"":{
            ""url"":{
               ""urls"":[
                  {
                     ""url"":""https:\/\/t.co\/ALu15SjIv6"",
                     ""expanded_url"":""https:\/\/www.paybyphone.fr"",
                     ""display_url"":""paybyphone.fr"",
                     ""indices"":[
                        0,
                        23
                     ]
                  }
               ]
            },
            ""description"":{
               ""urls"":[

               ]
            }
         },
         ""protected"":false,
         ""followers_count"":787,
         ""friends_count"":870,
         ""listed_count"":59,
         ""created_at"":""Thu Apr 16 12:24:56 +0000 2009"",
         ""favourites_count"":1,
         ""utc_offset"":7200,
         ""time_zone"":""Paris"",
         ""geo_enabled"":true,
         ""verified"":false,
         ""statuses_count"":331,
         ""lang"":""fr"",
         ""contributors_enabled"":false,
         ""is_translator"":false,
         ""profile_background_color"":""1A1B1F"",
         ""profile_background_image_url"":""http:\/\/a0.twimg.com\/profile_background_images\/747772723\/8e41dc058b909220da517c824f3f320f.jpeg"",
         ""profile_background_image_url_https"":""https:\/\/si0.twimg.com\/profile_background_images\/747772723\/8e41dc058b909220da517c824f3f320f.jpeg"",
         ""profile_background_tile"":false,
         ""profile_image_url"":""http:\/\/a0.twimg.com\/profile_images\/264234694\/PaybyPhone_element_normal.jpg"",
         ""profile_image_url_https"":""https:\/\/si0.twimg.com\/profile_images\/264234694\/PaybyPhone_element_normal.jpg"",
         ""profile_link_color"":""71BE44"",
         ""profile_sidebar_border_color"":""FFFFFF"",
         ""profile_sidebar_fill_color"":""252429"",
         ""profile_text_color"":""FAFAFA"",
         ""profile_use_background_image"":true,
         ""default_profile"":false,
         ""default_profile_image"":false,
         ""following"":null,
         ""follow_request_sent"":false,
         ""notifications"":null
      },
      ""geo"":null,
      ""coordinates"":null,
      ""place"":null,
      ""contributors"":null,
      ""retweet_count"":1,
      ""favorite_count"":0,
      ""entities"":{
         ""hashtags"":[

         ],
         ""symbols"":[

         ],
         ""urls"":[

         ],
         ""user_mentions"":[
            {
               ""screen_name"":""martialb41"",
               ""name"":""Martial \u271e"",
               ""id"":738483991,
               ""id_str"":""738483991"",
               ""indices"":[
                  0,
                  11
               ]
            }
         ]
      },
      ""favorited"":false,
      ""retweeted"":false,
      ""lang"":""fr""
   }]";
            #endregion

            var autoMocker = new RhinoAutoMocker<TwitterClient>();

            const string consumerKey = "consumerKey";
            const string consumerKeySecret = "consumerKeySecret";
            const string accessKey = "accessKey";
            const string accessKeySecret = "accessKeySecret";

            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerKey))
                   .Return(consumerKey);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.ConsumerSecret))
                   .Return(consumerKeySecret);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessToken))
                   .Return(accessKey);


            autoMocker.Get<IAppSettingsService>()
                   .Stub(o => o.Get(AppSettings.Twitter.AccessTokenSecret))
                   .Return(accessKeySecret);

            autoMocker.Get<IWebRequestWrapper>()
                      .Stub(o => o.GetResponseAsString(null, null))
                      .IgnoreArguments()
                      .Return(reponseFromTwitterApi);

            var actual = autoMocker.ClassUnderTest.GetTwoWeeksOfTweets("@payByPhone", "@payByPhone");

            Assert.AreEqual(number_of_mentions, actual[0].entities.user_mentions.Count);
        }
    }
}