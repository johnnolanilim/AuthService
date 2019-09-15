using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Configuration;
using System.Web.Http.Cors;

namespace AuthService
{
    /// <summary>
    /// Web API configuration.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Web API registration method.
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            EnableCors(config);

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}"
            );
        }

        private static void EnableCors(HttpConfiguration config)
        {
            string origins = "*";
            string headers = "*";
            string methods = "*";

            var cors = new EnableCorsAttribute(origins, headers, methods);

            config.EnableCors(cors);
        }
    }
}
