using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace YKPortal.Controllers
{
    public class MesajlasmaController : Controller
    {

        [HttpGet]
        public ActionResult Chat()
        {
            return View();
        }

    }

}
