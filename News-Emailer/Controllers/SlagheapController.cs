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

        [HttpGet("email/most-recent")]
        public Task<List<NewsItem>> EmailMostRecentFeedItems()
        {
            return SlagheapService.EmailMostRecentFeedItems(DefaultStoriesPerFeed);
        }

        [HttpGet("email/most-recent/{storiesPerFeed}")]
        public Task<List<NewsItem>> EmailMostRecentFeedItems(int storiesPerFeed)
        {
            return SlagheapService.EmailMostRecentFeedItems(storiesPerFeed);
        }
    }
}