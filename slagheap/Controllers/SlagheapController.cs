using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using slagheap.Models;
using slagheap.Services;

namespace slagheap.Controllers
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