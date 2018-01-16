using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JavaJamAjax
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Date", 
                routeTemplate: "api/musics/date/{year}/{month}/{day}", 
                defaults: new { controller = "Musics", 
                    month = RouteParameter.Optional, 
                    day = RouteParameter.Optional }, 
                    constraints: new { month = @"\d{0,2}", day = @"\d{0,2}" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
