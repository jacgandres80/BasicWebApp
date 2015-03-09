using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace SumServer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {
            //var process = new ProcessStartInfo();
            //process.FileName = Server.MapPath("Console") + "\\SumServerServerSocket.exe";
            //process.Arguments = " " + System.Configuration.ConfigurationManager.AppSettings["initialPort"];
            //process.UseShellExecute = true;

            //Process proc = Process.Start(process);

        }
    }
}
