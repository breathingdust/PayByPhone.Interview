using System;

namespace PayByPhoneInterview.Proxies
{
    public class Tweet
    {
        public DateTime created_at { get; set; }
        public string Text { get; set; }
        public Entities entities { get; set; }
        public User user { get; set; }
    }
}