using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;

namespace DataService
{
    public static class WebApiConfig
    {
        
        public static void Register(HttpConfiguration config)
        {
            System.Net.Http.HttpMethod[] allowedMethods = { System.Net.Http.HttpMethod.Get, System.Net.Http.HttpMethod.Post };

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                            name: "route1",
                            routeTemplate: "api/{controller}/{action}/{id}",
                            defaults: new { controller = "DataService", action = "GetPropertiesData", id = RouteParameter.Optional } ,
                            constraints: new { httpMethod = new HttpMethodConstraint(allowedMethods) }
                        );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
