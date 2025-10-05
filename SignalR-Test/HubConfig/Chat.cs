using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace SignalR_Test.HubConfig
{
    public partial class MyHub
    {
        public async Task GetOnlineUsers()
        {
            var allConnections = await context.Connections.ToListAsync();
            var currentConnectedUser = allConnections
                .Where(c => c.SignalrId == Context.ConnectionId).Select(c => c.PersonId)
                .FirstOrDefault();
            var otherUsers = allConnections.Where(c => c.PersonId != currentConnectedUser)
                .Join(context.Persons, o => o.PersonId, i => i.Id, (o, i) => new User(i.Id, i.Name, o.SignalrId));
            await Clients.Others.SendAsync("GetOnlineUsersResponse", otherUsers);

        }
    }
}
