using System.Collections.Generic;

namespace PayByPhoneInterview.Models
{
    public class TweetAggregation
    {
        public TweetAggregation()
        {
            Tweets = new List<Tweet>();
            NumberOfMentionsByAccount = new Dictionary<string, int>();
            NumberOfTweetsByAccount = new Dictionary<string, int>();
        }
        public List<Tweet> Tweets { get; set; } 
        public Dictionary<string, int> NumberOfTweetsByAccount { get; set; }
        public Dictionary<string,int> NumberOfMentionsByAccount { get; set; } 
    }
}