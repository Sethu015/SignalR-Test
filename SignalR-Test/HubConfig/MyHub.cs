using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR_Test.EFModels;
using System;

namespace SignalR_Test.HubConfig
{
    public partial class MyHub(SignalrDbContext context) : Hub
    {

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Guid currentUserId = context.Connections.Where(c => c.SignalrId == Context.ConnectionId)
                .Select(c => c.PersonId)
                .FirstOrDefault();
            context.Connections.RemoveRange(context.Connections.Where(c => c.PersonId == currentUserId));
            await context.SaveChangesAsync();
            await Clients.Others.SendAsync("UserOff", currentUserId);
            await base.OnDisconnectedAsync(exception);
        }
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

                User newUser = new User(person.Id, person.Name, currentSignalrId);

                //Bettter Method
                await Clients.Caller.SendAsync("AuthMeResponseSuccess", newUser);
                await Clients.AllExcept(currentSignalrId).SendAsync("UserOn", newUser);
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
                User newUser = new User(existingPerson.Id, existingPerson.Name, connectionId);
                await Clients.Caller.SendAsync("ReAuthMeResponseSuccess", newUser);
                await Clients.Others.SendAsync("UserOn", newUser);
            }
            else
            {
                await Clients.Client(connectionId).SendAsync("ReAuthMeResponseFail");
            }
        }

        public async Task LogOut(Guid personId)
        {
            var connectionId = Context.ConnectionId;
            context.Connections.RemoveRange(context.Connections.Where(c => c.PersonId == personId));
            await context.SaveChangesAsync();
            await Clients.Caller.SendAsync("LogOutResponse");
            await Clients.Others.SendAsync("UserOff", personId);
        }
    }
}
