using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayByPhoneInterview.Web.Controllers
{
    public class TweetController : Controller
    {
        private readonly ITweetAggregatorService tweetAggregatorService;
        private readonly IAppSettingsService appSettingsService;

        public TweetController(ITweetAggregatorService tweetAggregatorService, IAppSettingsService appSettingsService)
        {
            this.tweetAggregatorService = tweetAggregatorService;
            this.appSettingsService = appSettingsService;
        }

        public ActionResult Index()
        {
            var accountNames = appSettingsService.Get(AppSettings.TwitterAccountNames).Split(',').ToList();

            var aggregatedTweets = tweetAggregatorService.AggregateTweets(accountNames);

            return Json(aggregatedTweets,JsonRequestBehavior.AllowGet);
        }
    }
}
