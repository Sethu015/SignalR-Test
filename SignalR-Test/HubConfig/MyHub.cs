using Microsoft.AspNetCore.SignalR;
using SignalR_Test.EFModels;

namespace SignalR_Test.HubConfig
{
    public class MyHub(SignalrDbContext context) : Hub
    {
        public async Task AskServer(string textFromClient)
        {
            string tempString = string.Empty;
            if(textFromClient.Equals("hey",StringComparison.OrdinalIgnoreCase))
            {
                tempString = "Hello from server";
            }
            else
            {
                tempString = "I don't understand you";
            }
            await Clients.Client(Context.ConnectionId).SendAsync("AskServerResponse", tempString);
        }

        public async Task AuthMe(PersonInfo personInfo)
        {
            string currentSignalrId = Context.ConnectionId;
            var person = context.Persons.SingleOrDefault(p => p.UserName == personInfo.UserName
            && p.Password == personInfo.Password);
            if(person != null)
            {
                Connections connections = new Connections()
                {
                    PersonId = person.Id,
                    SignalrId = currentSignalrId,
                    Timestamp = DateTime.Now
                };

                await context.Connections.AddAsync(connections);
                await context.SaveChangesAsync();

                //Bettter Method
                await Clients.Caller.SendAsync("AuthMeResponseSuccess", person);
            }
            else
            {
                await Clients.Client(currentSignalrId).SendAsync("AuthMeResponseFail");
            }
        }
    }
}
