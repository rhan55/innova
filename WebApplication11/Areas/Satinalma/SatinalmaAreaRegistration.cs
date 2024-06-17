using System.Web.Mvc;

namespace YKPortal.Areas.Satinalma
{
    public class SatinalmaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Satinalma";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Satinalma_default",
                "Satinalma/{controller}/{action}/{id}",
                new { Contoller = "YKSatinalma", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}