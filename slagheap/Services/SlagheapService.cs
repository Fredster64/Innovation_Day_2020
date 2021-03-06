﻿using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using MailKit.Security;
using MimeKit;
using slagheap.Models;

namespace slagheap.Services
{
    public class SlagheapService
    {
        private readonly List<string> _feedUrls;
        private readonly List<Subscriber> _subscribers;

        public SlagheapService()
        {
            var dataService = new DataService();
            _feedUrls = dataService.GetFeedUrls();
            _subscribers = dataService.GetSubscribers();
        }

        public async Task<List<NewsItem>> EmailMostRecentFeedItems(int storiesPerFeed)
        {
            var items = new List<NewsItem>();

            foreach (var url in _feedUrls)
            {
                var reader = XmlReader.Create(url);
                var feed = SyndicationFeed.Load(reader);
                items.AddRange(feed.Items.Select(item => new NewsItem(item, feed)).Take(storiesPerFeed));
            }

            await SendEmailFromNewsItems(items);
            return items;
        }

        private async Task SendEmailFromNewsItems(List<NewsItem> newsItems)
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
            foreach (var subscriber in _subscribers)
            {
                message.To.Add(new MailboxAddress(subscriber.Name, subscriber.EmailAddress));
                message.Body = GetEmailBodyFromItems(newsItems, subscriber);
                await smtp.SendAsync(message);
                message.To.Clear();
            }
            await smtp.DisconnectAsync(true);
        }

        private static MimeEntity GetEmailBodyFromItems(List<NewsItem> newsItems, Subscriber subscriber)
        {
            var emailBuilder = new BodyBuilder();
            emailBuilder.HtmlBody = string.Format(
                "<p>Hi {0},</p><p>Here are your news updates for today:</p>",
                new[] {subscriber.Name.Split(' ')[0]});

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