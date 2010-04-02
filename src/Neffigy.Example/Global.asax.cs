using System.Web.Mvc;
using System.Web.Routing;

namespace Neffigy.Example
{
    public class ExampleApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapRoute("Default", "{controller}/{action}/{id}");
        }
    }
}