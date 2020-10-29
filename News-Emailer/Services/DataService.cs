using System.Collections.Generic;
using System.Data;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using News_Emailer.Models;

namespace News_Emailer.Services
{
    public class DataService
    {
        private const string FeedUrlsFilePath = @"Data\feedUrls.csv";
        private const string RecipientsFilePath = @"Data\recipients.csv";

        private readonly CsvReader _feedUrlsCsvReader;
        private readonly CsvReader _recipientsCsvReader;

        public DataService()
        {
            _feedUrlsCsvReader = new CsvReader(new StreamReader(File.OpenRead(FeedUrlsFilePath)));
            _recipientsCsvReader = new CsvReader(new StreamReader(File.OpenRead(RecipientsFilePath)));
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
        
        public List<Recipient> GetRecipients()
        {
            var recipients = new List<Recipient>();
            var csvTable = new DataTable();
            csvTable.Load(_recipientsCsvReader);

            for(int i=0; i<csvTable.Rows.Count; i++)
            {
                recipients.Add(new Recipient(
                    csvTable.Rows[i][0].ToString(), 
                    csvTable.Rows[i][1].ToString()));
            }

            return recipients;
        }
    }
}