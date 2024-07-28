using System.Web.Mvc;

namespace YKPortal.Areas.D
{
    public class DAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "D";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "D_default",
                "D/{controller}/{action}/{id}",
                new { Contoller = "Destek", action = "AnaSayfa", id = UrlParameter.Optional }
            );
        }
    }
}