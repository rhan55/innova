using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;

[assembly: OwinStartup(typeof(YKPortal.Startup))]


namespace YKPortal
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ÖNEMLİ!eğer KeepAlive = 10 saniye ise, DisconnectTimeout en az 30 saniye olmalıdır.Eğer KeepAlive = 20 saniye ise, DisconnectTimeout en az 60 saniye olmalıdır.

            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(60);
   
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(20);

            // SignalR rotasını belirleyin
            app.MapSignalR();
         
          }
    }
}