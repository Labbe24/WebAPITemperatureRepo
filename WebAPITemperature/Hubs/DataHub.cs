using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebAPITemperature.Hubs
{
    public class DataHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendTemp(List<Temperature> temp)
        {
            await Clients.All.SendAsync("ReceiveTemp", temp);
        }
    }
}
