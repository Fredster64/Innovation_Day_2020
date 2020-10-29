using System;
using System.ServiceModel.Syndication;

namespace News_Emailer.Models
{
    public class NewsItem
    {
        public string FeedName { get; private set; }
        public Uri Url { get; private set; }
        public string Headline { get; private set; }
        public string Summary { get; private set; }

        public NewsItem(SyndicationItem syndicationItem, SyndicationFeed baseFeed)
        {
            FeedName = baseFeed.Title.Text;
            Url = syndicationItem.Links[0].Uri;
            Headline = syndicationItem.Title.Text;
            Summary = syndicationItem.Summary.Text;
        }
    }
}