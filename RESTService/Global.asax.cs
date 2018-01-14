using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace RESTService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.Add(new ServiceRoute("RESTService", new WebServiceHostFactory(), typeof(Service1)));
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
        }
    }
}