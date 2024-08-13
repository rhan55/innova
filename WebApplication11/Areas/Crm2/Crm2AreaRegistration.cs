using System.Web.Mvc;

namespace YKPortal.Areas.Crm2
{
    public class Crm2AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Crm2";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Crm2_default",
                "Crm2/{controller}/{action}/{id}",
                new { Contoller = "Crm2", action = "AnaSayfa", id = UrlParameter.Optional }
            );
        }
    }
}