using Microsoft.AspNetCore.SignalR;

namespace SignalR_Test.HubConfig
{
    public class MyHub : Hub
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
    }
}
