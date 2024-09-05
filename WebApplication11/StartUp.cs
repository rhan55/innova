using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(YKPortal.Startup))]


namespace YKPortal
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // SignalR'ı başlatıyoruz
            app.MapSignalR();
        }
    }
}