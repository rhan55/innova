using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Areas.E.Models.Dto;

namespace YKPortal.Areas.E.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Kategoriler için veri kaynağını burada doldurun
            var kategoriler = new List<KategorilerDto>
        {
            new KategorilerDto { Kategori1 = "Category 1", Kategori2 = "Subcategory 1" },
            new KategorilerDto { Kategori1 = "Category 2", Kategori2 = "Subcategory 2" }
        };

            // ViewBag ile Layout'a taşınacak veri
            ViewBag.Kategoriler = kategoriler;

            base.OnActionExecuting(filterContext);
        }
    }
}