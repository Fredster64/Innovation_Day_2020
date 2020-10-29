using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using News_Emailer.Models;

namespace News_Emailer.Services
{
    public class RssFeedService
    {
        private static readonly string[] FeedUrls =
        {
            "http://feeds.bbci.co.uk/news/england/london/rss.xml",
            "https://www.bristolpost.co.uk/news/bristol-news/?service=rss",
            "https://www.theguardian.com/crosswords/series/quiptic/rss",
            "https://www.theguardian.com/artanddesign/rss"
        };

        public static List<string> GetFeedTitles()
        {
            var feedTitles = new List<string>();
            
            foreach (var url in FeedUrls)
            {
                var reader = XmlReader.Create(url);
                var feed = SyndicationFeed.Load(reader);
                feedTitles.Add(feed.Title.Text);
            }

            return feedTitles;
        }

        public static List<NewsItem> GetMostRecentFeedItems(int storiesPerFeed)
        {
            var items = new List<NewsItem>();

            foreach (var url in FeedUrls)
            {
                var reader = XmlReader.Create(url);
                var feed = SyndicationFeed.Load(reader);
                items.AddRange(feed.Items.Select(item => new NewsItem(item, feed)).Take(storiesPerFeed));
            }

            return items;
        }
    }
}