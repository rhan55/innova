using System.Web.Mvc;

namespace YKPortal.Areas.Depo
{
    public class DepoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Depo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Depo_default",
                "Depo/{controller}/{action}/{id}",
                new { Contoller = "Depo", action = "AnaSayfa", id = UrlParameter.Optional }
            );
        }
    }
}