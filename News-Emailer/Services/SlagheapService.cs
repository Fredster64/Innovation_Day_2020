using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using MailKit.Security;
using Microsoft.AspNetCore.Html;
using MimeKit;
using MimeKit.Text;
using News_Emailer.Models;

namespace News_Emailer.Services
{
    public class SlagheapService
    {
        private static readonly string[] FeedUrls =
        {
            "http://feeds.bbci.co.uk/news/england/london/rss.xml",
            "https://www.bristolpost.co.uk/news/bristol-news/?service=rss",
            "https://www.theguardian.com/crosswords/series/quiptic/rss",
            "https://www.theguardian.com/artanddesign/rss"
        };

        private static readonly Recipient[] Recipients =
        {
            new Recipient("Freddie Payne", "freddie.payne@ghyston.com"), 
        };
        
        public static async Task<List<NewsItem>> EmailMostRecentFeedItems(int storiesPerFeed)
        {
            var items = new List<NewsItem>();

            foreach (var url in FeedUrls)
            {
                var reader = XmlReader.Create(url);
                var feed = SyndicationFeed.Load(reader);
                items.AddRange(feed.Items.Select(item => new NewsItem(item, feed)).Take(storiesPerFeed));
            }

            await SendEmailFromNewsItems(items);
            return items;
        }

        private static async Task SendEmailFromNewsItems(List<NewsItem> newsItems)
        {
            var message = new MimeMessage
            {
                Sender = new MailboxAddress("Slagheap-News", "slagheap-news@outlook.com"),
                Subject = "Your Daily Headlines"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };
            await smtp.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("slagheap-news@outlook.com", "SLAGHEAPnews");
            foreach (var recipient in Recipients)
            {
                message.To.Add(new MailboxAddress(recipient.Name, recipient.EmailAddress));
                message.Body = GetEmailBodyFromItems(newsItems, recipient);
                await smtp.SendAsync(message);
                message.To.Clear();
            }
            await smtp.DisconnectAsync(true);
        }

        private static MimeEntity GetEmailBodyFromItems(List<NewsItem> newsItems, Recipient recipient)
        {
            var emailBuilder = new BodyBuilder();
            emailBuilder.HtmlBody = string.Format(
                "<p>Hi {0},</p><p>Here are your news updates for today:</p>", 
                new[] {recipient.Name.Split(' ')[0]});

            var currentFeedName = "";
            foreach (var item in newsItems)
            {
                if (item.FeedName != currentFeedName)
                {
                    if (currentFeedName != "")
                    {
                        emailBuilder.HtmlBody += "<br/>";
                    }
                    emailBuilder.HtmlBody += string.Format("<h2><u>{0}</u></h2>", new[] {item.FeedName});
                    currentFeedName = item.FeedName;
                }
                
                emailBuilder.HtmlBody += string.Format(
                    "<div><div><h2>{0}</h2></div><p>{1}</p><div>({2})</div></div>",
                    new[] {item.Headline, item.Summary, item.Url.ToString()});
            }
            return emailBuilder.ToMessageBody();
        }
    }
}