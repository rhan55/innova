using System.Web.Mvc;

namespace YKPortal.Areas.Entegrasyon
{
    public class EntegrasyonAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Entegrasyon";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Entegrasyon_default",
                "Entegrasyon/{controller}/{action}/{id}",
                new { Contoller = "QuickSigorta", action = "AnaSayfa", id = UrlParameter.Optional }
            );
        }
    }
}