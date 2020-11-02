using System.Collections.Generic;
using System.Data;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using slagheap.Models;

namespace slagheap.Services
{
    public class DataService
    {
        private const string FeedUrlsFilePath = @"Data\feedUrls.csv";
        private const string SubscribersFilePath = @"Data\subscribers.csv";

        private readonly CsvReader _feedUrlsCsvReader;
        private readonly CsvReader _subscribersCsvReader;

        public DataService()
        {
            _feedUrlsCsvReader = new CsvReader(new StreamReader(File.OpenRead(FeedUrlsFilePath)));
            _subscribersCsvReader = new CsvReader(new StreamReader(File.OpenRead(SubscribersFilePath)));
        }

        public List<string> GetFeedUrls()
        {
            var feedUrls = new List<string>();
            var csvTable = new DataTable();
            csvTable.Load(_feedUrlsCsvReader);

            for(int i=0; i<csvTable.Rows.Count; i++)
            {
                feedUrls.Add(csvTable.Rows[i][0].ToString());
            }

            return feedUrls;
        }
        
        public List<Subscriber> GetSubscribers()
        {
            var recipients = new List<Subscriber>();
            var csvTable = new DataTable();
            csvTable.Load(_subscribersCsvReader);

            for(int i=0; i<csvTable.Rows.Count; i++)
            {
                recipients.Add(new Subscriber(
                    csvTable.Rows[i][0].ToString(), 
                    csvTable.Rows[i][1].ToString()));
            }

            return recipients;
        }
    }
}