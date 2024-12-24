using System.Web.Mvc;
using YKPortal.Areas.E.Models.Dto;

namespace YKPortal.Areas.E.Controllers
{
    public class ETicaretSiteController : BaseController
    {
        [HttpGet]
        public ActionResult AnaSayfa()
        {
            ViewBag.Stoklar = StokGetir(new ETicaretStokDto.ETicaretStokSorguDto { });
            return View();
        }

       
    }

}
