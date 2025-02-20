using Microsoft.AspNet.SignalR;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using YKPortal.Models.Dto;

namespace YKPortal.Areas.E.Hubs
{
    public class ETicaretBildirimHub : Hub
    {
        private KullaniciEkleDto Kullanici { get; set; }

        public ETicaretBildirimHub()
        {
            
        }
        // GET: E/BildirimHub
        public void BildirimGonder(int count)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    string[] roles = new string[] { "Profil" };
                    var identity = new FormsIdentity(authTicket);

                    var kullanici = JsonSerializer.Deserialize<KullaniciEkleDto>(authTicket.UserData);

                    Kullanici = kullanici;

                    if (!string.IsNullOrEmpty(Kullanici.ID))
                    {
                        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ETicaretBildirimHub>();
                        context.Clients.Group(Kullanici.ID).ReceiveNotification(count);
                    }

                }
            }
            
        }


        public override Task OnConnected()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    string[] roles = new string[] { "Profil" };
                    var identity = new FormsIdentity(authTicket);

                    var kullanici = JsonSerializer.Deserialize<KullaniciEkleDto>(authTicket.UserData);

                    Kullanici = kullanici;

                    if (!string.IsNullOrEmpty(Kullanici.ID))
                    {
                        Groups.Add(Context.ConnectionId, Kullanici.ID);
                    }

                }
            }
           
           

            return base.OnConnected();
        }
    }
}