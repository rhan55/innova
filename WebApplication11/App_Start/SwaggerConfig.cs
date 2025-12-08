using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(YKPortal.SwaggerConfig), "Register")]

namespace YKPortal
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var config = GlobalConfiguration.Configuration;

            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "YKPortal API");
                })
                .EnableSwaggerUi();
        }
    }
}
