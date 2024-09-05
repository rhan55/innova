using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace YKPortal.Hubs
{
    public class MessageHub : Hub
    {
        // Kullanıcılar sisteme bağlandığında ID'lerini kaydederiz.
        public static Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();

        // Kullanıcı bağlantı kurduğunda ID'sini kaydediyoruz
        public override Task OnConnected()
        {
            string userName = Context.QueryString["userName"]; // Kullanıcı adını alıyoruz
            string connectionId = Context.ConnectionId;

            if (!ConnectedUsers.ContainsKey(userName))
            {
                ConnectedUsers.Add(userName, connectionId);
            }

            return base.OnConnected();
        }

        // Kullanıcıdan kullanıcıya mesaj gönderme
        public void SendMessage(string fromUser, string toUser, string message)
        {
            // Alıcı kullanıcının bağlantı ID'sini bul
            if (ConnectedUsers.ContainsKey(toUser))
            {
                string toConnectionId = ConnectedUsers[toUser];

                // Alıcıya mesajı gönder
                Clients.Client(toConnectionId).receiveMessage(fromUser, message);
            }
        }

        // Kullanıcı bağlantısını kapattığında kullanıcıyı sil
        public override Task OnDisconnected(bool stopCalled)
        {
            string connectionId = Context.ConnectionId;
            var user = ConnectedUsers.FirstOrDefault(x => x.Value == connectionId);

            if (user.Key != null)
            {
                ConnectedUsers.Remove(user.Key);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}