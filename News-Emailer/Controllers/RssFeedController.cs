using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using News_Emailer.Models;
using News_Emailer.Services;

namespace News_Emailer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RssFeedController : ControllerBase
    {
        public RssFeedService rssFeedService { get; private set; }

        private const int DefaultStoriesPerFeed = 3;

        public RssFeedController()
        {
            rssFeedService = new RssFeedService();
        }
        
        [HttpGet("titles")]
        public List<string> GetFeedTitles()
        {
            return RssFeedService.GetFeedTitles();
        }
        
        [HttpGet("stories/most-recent")]
        public List<NewsItem> GetMostRecentFeedItems()
        {
            return RssFeedService.GetMostRecentFeedItems(DefaultStoriesPerFeed);
        }

        [HttpGet("stories/most-recent/{storiesPerFeed}")]
        public List<NewsItem> GetMostRecentFeedItems(int storiesPerFeed)
        {
            return RssFeedService.GetMostRecentFeedItems(storiesPerFeed);
        }
    }
}