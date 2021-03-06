﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ProjectManagerService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "ProjectManagerService",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
