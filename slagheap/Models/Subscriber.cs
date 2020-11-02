namespace slagheap.Models
{
    public class Subscriber
    {
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }

        public Subscriber(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }
    }
}