namespace SignalR_Test.HubConfig
{
    public class PersonInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConnId { get; set; }
        public User(Guid id,string name,string connId)
        {
            Id = id;
            Name = name;
            ConnId = connId;
        }
    }
}