using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Newtonsoft.Json;

namespace BrokenGlassTests
{
    internal class TestUtils
    {
        public static void SetApiControllerContextAndRequest(ApiController apiController, string requestUrl)
        {
            var httpConfiguiration = new HttpConfiguration();
            var httpReqest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var route = httpConfiguiration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary() { { "controller", "Claims" } });

            apiController.ControllerContext = new HttpControllerContext(httpConfiguiration, routeData, httpReqest);
            apiController.Request = httpReqest;
            apiController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = httpConfiguiration;
        }
    }
}
