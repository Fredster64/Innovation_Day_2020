using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using News_Emailer.Models;
using News_Emailer.Services;

namespace News_Emailer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlagheapController : ControllerBase
    {
        private const int DefaultStoriesPerFeed = 3;

        [HttpGet("stories/most-recent")]
        public Task<List<NewsItem>> GetMostRecentFeedItems()
        {
            return SlagheapService.GetMostRecentFeedItems(DefaultStoriesPerFeed);
        }

        [HttpGet("stories/most-recent/{storiesPerFeed}")]
        public Task<List<NewsItem>> GetMostRecentFeedItems(int storiesPerFeed)
        {
            return SlagheapService.GetMostRecentFeedItems(storiesPerFeed);
        }
    }
}