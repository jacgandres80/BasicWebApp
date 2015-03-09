using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AppWebServer.App_Start;

namespace AppWebServer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles); 
        }

        protected void Session_Start()
        {
            Session["XmlPathFile"] = Server.MapPath("~/App_Data")+"\\Data.xml";
        }
    }
}
