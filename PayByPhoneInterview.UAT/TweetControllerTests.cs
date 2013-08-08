using System.Net;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PayByPhoneInterview.UAT
{
    [TestFixture]
    public class TweetControllerTests
    {
         [Test]
         public void when_calling_the_tweet_controller_http_status_OK_should_be_returned()
         {
             HttpWebResponse response = null;
             var request = (HttpWebRequest)WebRequest.Create("http://localhost:51473/Tweet");
             response = (HttpWebResponse)request.GetResponse();

             Assert.AreEqual(HttpStatusCode.OK,response.StatusCode);
        }
    }            
}