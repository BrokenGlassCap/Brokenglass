using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BrokenGlassWebApp.Infostracture;

[assembly: OwinStartup(typeof(BrokenGlassWebApp.App_Start.Startup))]
namespace BrokenGlassWebApp.App_Start
{
    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
        //public class Global : HttpApplication
        //{
        //    void Application_Start(object sender, EventArgs e)
        //    {
        //        // Код, выполняемый при запуске приложения
        //        AreaRegistration.RegisterAllAreas();
        //        GlobalConfiguration.Configure(WebApiConfig.Register);
        //        RouteConfig.RegisterRoutes(RouteTable.Routes);
        //        DependencyResolver.SetResolver(new NinjectDependencyResolver());
        //        HttpConfiguration config = GlobalConfiguration.Configuration;
        //        config.Formatters.JsonFormatter
        //                    .SerializerSettings
        //                    .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        //    }
        //}
    }
}