using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace YKPortal.Hubs
{
    public class MesajHub : Hub
    {
        private static ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        public void RegisterUser(string kullaniciID)
        {
            UserConnections.AddOrUpdate(kullaniciID, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);
        }

        public void SendMessage(string aliciKullaniciID, string gonderenkullaniciID, string mesaj)
        {
            if (UserConnections.TryGetValue(aliciKullaniciID, out var connectionId))
            {
                Clients.Client(connectionId).ReceiveMessage(gonderenkullaniciID, mesaj);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var kullaniciID = UserConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

            if (!string.IsNullOrEmpty(kullaniciID))
            {
                UserConnections.TryRemove(kullaniciID, out _);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}