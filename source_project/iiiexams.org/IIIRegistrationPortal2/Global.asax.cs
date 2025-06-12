using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Configuration;
using System.Web.Optimization;
using System.Reflection;

namespace IIIRegistrationPortal2
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            MvcHandler.DisableMvcResponseHeader = true;

            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);//Commented by Mangesh 

            String EnableLogging = ConfigurationManager.AppSettings.Get("EnableLogging");
            String LogDirectory = ConfigurationManager.AppSettings.Get("LogDirectory");

            Errorlogger.Initialize(LogDirectory, (EnableLogging == "Y"));

            //Initialize Masters.
            try
            {
                Errorlogger.LogInfo(String.Empty, String.Empty, "Initializing Application");
                PortalApplication.LoadMasterData();
                Errorlogger.LogInfo(String.Empty, String.Empty, "Application Initialized");
            }
            catch(Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {

            }
        }

        protected void Application_EndRequest()
        {
            //var context = new HttpContextWrapper(Context);
            //if (context.Request.IsAjaxRequest() && context.Response.StatusCode == 401)
            //{
            //    Context.Response.Clear();
            //    Context.Response.Write("USER_SESSION_EXPIRED");
            //    Context.Response.StatusCode = 401;
            //}
        }
    }
}