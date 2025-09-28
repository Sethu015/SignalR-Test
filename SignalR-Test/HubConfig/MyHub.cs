using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

        public async Task ReAuthMe(string personId)
        {
            var connectionId = Context.ConnectionId;
            var existingPerson = await context.Persons.SingleOrDefaultAsync(p => p.Id.ToString() == personId);
            if(existingPerson != null)
            {
                var connection = new Connections()
                {
                    PersonId = existingPerson.Id,
                    SignalrId = connectionId,
                    Timestamp = DateTime.Now
                };
                await context.Connections.AddAsync(connection);
                await context.SaveChangesAsync();
                await Clients.Caller.SendAsync("ReAuthMeResponseSuccess", existingPerson);
            }
            else
            {
                await Clients.Client(connectionId).SendAsync("ReAuthMeResponseFail");
            }
        }
    }
}
