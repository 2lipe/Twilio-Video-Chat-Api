using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Api.CrossCutting.Notification
{
    public class NotificationHub : Hub
    {
        public async Task RoomsUpdated(bool flag)
        {
            try
            {
                await Clients.Others.SendAsync("RoomsUpdated", flag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
