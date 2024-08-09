using System.Web.Mvc;

namespace YKPortal.Areas.B2BLogo
{
    public class B2BLogoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "B2BLogo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "B2BLogo_default",
                "B2BLogo/{controller}/{action}/{id}",
                new { Contoller = "B2BLogo", action = "AnaSayfa", id = UrlParameter.Optional }
            );
        }
    }
}