using System;
using System.Collections.Generic;
using System.Linq;
using PayByPhoneInterview.Models;

namespace PayByPhoneInterview
{
    public interface ITweetAggregatorService
    {
        TweetAggregation AggregateTweets(List<string> accountNames);
    }

    public class TweetAggregatorService : ITweetAggregatorService
    {
        private readonly ITwitterClient twitterClient;

        public TweetAggregatorService(ITwitterClient twitterClient)
        {
            this.twitterClient = twitterClient;
        }

        Tweet MapProxyToTweet(Proxies.Tweet tweetProxy)
        {
            return new Tweet{AccountName = tweetProxy.user.screen_name, Text = tweetProxy.Text, Timestamp = tweetProxy.created_at};
        }

        public TweetAggregation AggregateTweets(List<string> accountNames)
        {
            var tweetAggregaton = new TweetAggregation();

            foreach (var accountName in accountNames)
            {
                var tweetProxies = twitterClient.GetTwoWeeksOfTweets(accountName, accountName).Where(o => o.created_at > DateTime.Now.AddDays(-14)).ToList();

                tweetAggregaton.NumberOfTweetsByAccount.Add(accountName,tweetProxies.Count);


                if (tweetAggregaton.NumberOfMentionsByAccount.ContainsKey(accountName) == false)
                    tweetAggregaton.NumberOfMentionsByAccount[accountName] = 0;

                
                tweetAggregaton.NumberOfMentionsByAccount[accountName] = GetSumOfUserMentions(tweetProxies);

                tweetAggregaton.Tweets.AddRange(tweetProxies.Select(MapProxyToTweet));
            }

            tweetAggregaton.Tweets = tweetAggregaton.Tweets.OrderBy(o => o.Timestamp).ToList();

            return tweetAggregaton;
        }

        private int GetSumOfUserMentions(IEnumerable<Proxies.Tweet> tweetProxies)
        {
            var result = 0;
            foreach (var tweetProxy in tweetProxies)
            {
                if (tweetProxy.entities != null)
                {
                    if (tweetProxy.entities.user_mentions != null)
                    {
                        result += tweetProxy.entities.user_mentions.Count;
                    }
                }
            }
            return result;
        }
    }
}