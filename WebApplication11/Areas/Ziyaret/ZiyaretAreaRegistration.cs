using System.Web.Mvc;

namespace YKPortal.Areas.Ziyaret
{
    public class ZiyaretAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Ziyaret";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Ziyaret_default",
                "Ziyaret/{controller}/{action}/{id}",
                new { Contoller = "Ziyaret", action = "AnaSayfa", id = UrlParameter.Optional }
            );
        }
    }
}