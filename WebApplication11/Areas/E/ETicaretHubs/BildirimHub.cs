using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace YKPortal.Areas.E.ETicaretHubs
{
    public class BildirimHub : Hub
    {
        // GET: E/BildirimHub
        public static void BildirimGonder(string kullaniciID, string mesaj)
        {  
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<BildirimHub>();
            context.Clients.Group(kullaniciID).ReceiveNotification(mesaj);
        }


        public override Task OnConnected()
        {
            string kullaniciID = Context.RequestCookies["KullaniciID"]?.Value;
            if (!string.IsNullOrEmpty(kullaniciID))
            {
                Groups.Add(Context.ConnectionId, kullaniciID);
            }
            return base.OnConnected();
        }
    }
}