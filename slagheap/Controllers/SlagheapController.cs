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
        private readonly SlagheapService _slagheapService;

        public SlagheapController()
        {
            _slagheapService = new SlagheapService();
        }

        [HttpGet("email/most-recent")]
        public Task<List<NewsItem>> EmailMostRecentFeedItems()
        {
            return _slagheapService.EmailMostRecentFeedItems(DefaultStoriesPerFeed);
        }

        [HttpGet("email/most-recent/{storiesPerFeed}")]
        public Task<List<NewsItem>> EmailMostRecentFeedItems(int storiesPerFeed)
        {
            return _slagheapService.EmailMostRecentFeedItems(storiesPerFeed);
        }
    }
}