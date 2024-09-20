using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YKPortal.Models.Dto;
using YKPortal.Models;

namespace YKPortal.Hubs
{
    public class BildirimHub : Hub
    {
        public static void BildirimGonder(string kullaniciID) 
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<BildirimHub>();
            context.Clients.Group(kullaniciID).ReceiveNotification($"");          
        }

        public override Task OnConnected()
        {
            string kullaniciID = Context.RequestCookies["KullaniciID"]?.Value;
            if (!string.IsNullOrEmpty(kullaniciID)) {
                Groups.Add(Context.ConnectionId, kullaniciID);
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string kullaniciID = Context.RequestCookies["KullaniciID"]?.Value;
            if (!string.IsNullOrEmpty(kullaniciID)) {
                Groups.Remove(Context.ConnectionId, kullaniciID);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}