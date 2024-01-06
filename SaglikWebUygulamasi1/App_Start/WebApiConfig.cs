using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SaglikWebUygulamasi1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API yapılandırması ve hizmetler

            // Web API yolları
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{folder}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
