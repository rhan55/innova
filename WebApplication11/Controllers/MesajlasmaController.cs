//using Microsoft.AspNet.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using Microsoft.AspNet.SignalR;
//using System.Threading.Tasks;
//using System.Threading;
//using System.Web.Http.Results;

//namespace YKPortal.Controllers
//{
//    public sealed class MesajlasmaController (IHubContext<MesajHub> hubContext): Controller
//    {

//        [HttpGet]
//        public ActionResult Chat()
//        {
//            return View();
//        }

//        [HttpGet]
//        public async Task<IActionResult> MesajAl(Guid KullaniciID, Guid KarsiKullaniciID, CancellationToken cancellationToken)
//        {
//            List<Mesaj> mesajlar = 
//                await context
//                .mesajlar.
//                Where
//                (p=>
//                p.KullaniciID == kullaniciID && p.KarsiKullaniciID == karsiKullaniciID || 
//                p.KullaniciID == KarsiKullaniciID && p.KullaniciID == karsiKullaniciID)
//                .OrderBy(p=>p.Tarih)
//                .ToListAsync(cancellationToken);
//            return Ok(Mesajlar);
//        }

//        [HttpPost]
//        public async Task<IActionResult> MesajGonder(MesajlasmaDto request, CancellationToken cancellationToken)
//        {
//            MesajAl mesaj = new()
//            {
//                KullaniciID = request.KullaniciID,
//                KarsiKullaniciID = request.KarsiKullaniciID,
//                Mesaj = request.Mesaj,
//                Date = DateTime.Now,
              
//            };
//            await context.AddAsync(mesaj, cancellationToken);
//            await context.SaveChangesAsync(cancellationToken);
//            string connectionId = MesajHub.Kullanicilar.First(p => p.Value == mesaj.KarsiKullanicilarID).Key;

//            await hubContext.Clients.Client(connectionId).SendAsync("Messsages", Chat);

//            await hubContext.Clients.Client()
//           return Ok();
//        }

//    }

//}
