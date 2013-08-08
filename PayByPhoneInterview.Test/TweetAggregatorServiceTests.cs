using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PayByPhoneInterview.Proxies;
using Rhino.Mocks;
using StructureMap.AutoMocking;

namespace PayByPhoneInterview.Test
{
    [TestFixture]
    public class TweetAggregatorServiceTests
    {
        [Test]
        public void when_aggregating_tweets_twitter_client_should_be_called_for_each_account()
        {
            var subject = new RhinoAutoMocker<TweetAggregatorService>();

            const string account_one = "account_one";
            const string account_two = "account_two";

            var list_of_account_names = new List<string> {account_one, account_two};

            subject.Get<ITwitterClient>()
                   .Stub(o => o.GetTwoWeeksOfTweets(account_one, account_one))
                   .Return(new List<Proxies.Tweet>());

            subject.Get<ITwitterClient>()
                   .Stub(o => o.GetTwoWeeksOfTweets(account_two, account_two))
                   .Return(new List<Proxies.Tweet>());

            subject.ClassUnderTest.AggregateTweets(list_of_account_names);

            subject.Get<ITwitterClient>()
                   .AssertWasCalled(o => o.GetTwoWeeksOfTweets(account_one, account_one));

            subject.Get<ITwitterClient>()
                    .AssertWasCalled(o => o.GetTwoWeeksOfTweets(account_two,account_two));

        }

        [Test]
        public void when_aggregating_tweets_tweets_per_account_should_be_correctly_summed()
        {
            var subject = new RhinoAutoMocker<TweetAggregatorService>();

            const string account_one = "account_one";

            var list_of_account_names = new List<string> { account_one };

            var tweet_one = new Proxies.Tweet { user = new User { screen_name = account_one }, created_at = DateTime.Now };
            var tweet_two = new Proxies.Tweet { user = new User { screen_name = account_one }, created_at = DateTime.Now };

            var list_of_tweets = new List<Proxies.Tweet>
                {
                    tweet_one, tweet_two
                };

            subject.Get<ITwitterClient>()
                   .Stub(o => o.GetTwoWeeksOfTweets(account_one, account_one))
                   .Return(list_of_tweets);

            var actual = subject.ClassUnderTest.AggregateTweets(list_of_account_names);
            
            Assert.AreEqual(2, actual.NumberOfTweetsByAccount[account_one]);
        }

        [Test]
        public void when_aggregating_tweets_number_of_tweets_should_be_zero_if_no_tweets_for_that_account_were_returned()
        {
            var subject = new RhinoAutoMocker<TweetAggregatorService>();

            const string account_one = "account_one";
            const string account_two = "account_two";

            var list_of_account_names = new List<string> { account_one, account_two };

            var tweet_one = new Proxies.Tweet { user = new User { screen_name = account_one },created_at = DateTime.Now};
            var tweet_two = new Proxies.Tweet { user = new User { screen_name = account_one }, created_at = DateTime.Now };

            var list_of_tweets = new List<Proxies.Tweet>
                {
                    tweet_one, tweet_two
                };

            subject.Get<ITwitterClient>()
                   .Stub(o => o.GetTwoWeeksOfTweets(account_one, account_one))
                   .Return(list_of_tweets);

            subject.Get<ITwitterClient>()
                   .Stub(o => o.GetTwoWeeksOfTweets(account_two, account_two))
                   .Return(new List<Proxies.Tweet>());

            var actual = subject.ClassUnderTest.AggregateTweets(list_of_account_names);

            Assert.AreEqual(0, actual.NumberOfTweetsByAccount[account_two]);
        }

        [Test]
        public void when_aggregating_tweets_mentions_per_account_should_be_correctly_summed()
        {
            var subject = new RhinoAutoMocker<TweetAggregatorService>();

            const string account_one = "account_one";
            const string account_two = "account_two";

            var list_of_account_names = new List<string> { account_one, account_two };

            var tweet_one = new Proxies.Tweet { user = new User { screen_name = account_one }, created_at = DateTime.Now, entities = new Entities() { user_mentions = new List<object> { new object(), new object() } } };
            var tweet_two = new Proxies.Tweet { user = new User { screen_name = account_two }, created_at = DateTime.Now, entities = new Entities() { user_mentions = new List<object> { new object(), new object() } } };

            var list_of_tweets_for_account_one = new List<Proxies.Tweet>
                {
                    tweet_one
                };

            var list_of_tweets_for_account_two = new List<Proxies.Tweet>
                {
                    tweet_two
                };

            subject.Get<ITwitterClient>()
                   .Stub(o => o.GetTwoWeeksOfTweets(account_one, account_one))
                   .Return(list_of_tweets_for_account_one);

            subject.Get<ITwitterClient>()
                   .Stub(o => o.GetTwoWeeksOfTweets(account_two, account_two))
                   .Return(list_of_tweets_for_account_two);

            var actual = subject.ClassUnderTest.AggregateTweets(list_of_account_names);

            Assert.AreEqual(2, actual.NumberOfMentionsByAccount[account_one]);
        }

        [Test]
        public void when_aggregating_tweets_tweet_proxies_should_be_correctly_mapped()
        {
            var subject = new RhinoAutoMocker<TweetAggregatorService>();

            const string account_one = "account_one";

            var list_of_account_names = new List<string> { account_one };

            var tweet_text = "tweet text";

            var now = DateTime.Now;

            var tweet_one = new Proxies.Tweet { user = new User { screen_name = account_one }, Text = tweet_text, created_at = now };

            var list_of_tweets = new List<Proxies.Tweet>
                {
                    tweet_one
                };

            subject.Get<ITwitterClient>()
                   .Stub(o => o.GetTwoWeeksOfTweets(account_one, account_one))
                   .Return(list_of_tweets);

            var actual = subject.ClassUnderTest.AggregateTweets(list_of_account_names);

            Assert.AreEqual(account_one, actual.Tweets[0].AccountName);
            Assert.AreEqual(tweet_text, actual.Tweets[0].Text);
            Assert.AreEqual(now, actual.Tweets[0].Timestamp);
        }

        [Test]
        public void when_aggregating_tweets_tweets_per_account_should_be_sorted_by_date()
        {
            var subject = new RhinoAutoMocker<TweetAggregatorService>();

            const string account_one = "account_one";

            var list_of_account_names = new List<string> { account_one };

            var now = DateTime.Now;

            var earliest_date = now.AddHours(-3);
            var middle_date = now.AddHours(-2);
            var latest_date = now.AddHours(-1);

            var tweet_one = new Proxies.Tweet { user = new User { screen_name = account_one },created_at = earliest_date};
            var tweet_two = new Proxies.Tweet { user = new User { screen_name = account_one }, created_at = middle_date };
            var tweet_three = new Proxies.Tweet { user = new User { screen_name = account_one }, created_at = latest_date };

            var list_of_tweets = new List<Proxies.Tweet>
                {
                    tweet_two, tweet_one, tweet_three
                };

            subject.Get<ITwitterClient>()
                   .Stub(o => o.GetTwoWeeksOfTweets(account_one, account_one))
                   .Return(list_of_tweets);

            var actual = subject.ClassUnderTest.AggregateTweets(list_of_account_names);

            Assert.AreEqual(earliest_date, actual.Tweets[0].Timestamp);
            Assert.AreEqual(latest_date, actual.Tweets[2].Timestamp);
        }
        
    }
}
