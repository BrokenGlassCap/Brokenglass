﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BrokenGlassWebApp.Infostracture;
using Microsoft.Owin.Security.OAuth;

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
            ConfigureOAuth(app);
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new CustomOAuthAuthorizationServerProvider()
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

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