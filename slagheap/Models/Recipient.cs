namespace slagheap.Models
{
    public class Recipient
    {
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }

        public Recipient(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }
    }
}