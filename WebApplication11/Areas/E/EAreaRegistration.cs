using System.Web.Mvc;

namespace YKPortal.Areas.E
{
    public class IAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "E";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "E_default",
                "E/{controller}/{action}/{id}",
                new { Contoller = "Site", action = "AnaSayfa", id = UrlParameter.Optional }
            );
        }
    }
}