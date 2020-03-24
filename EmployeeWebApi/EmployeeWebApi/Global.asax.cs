using EmployeeWebApi.DAL.Interface;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using di = EmployeeWebApi.DependencyInjection;
namespace EmployeeWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly ILog _log = LogManager.GetLogger("WebApiApplication");
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            XmlConfigurator.Configure();
            _log.Info("******Application started******");

            di.DependencyResolver resolver = new di.DependencyResolver();
            IoC.Initialize();

            IoC.Resolve<ISqlDatabase>().InitialiseConnectionString("Server=" + ConfigurationManager.AppSettings["DBServerName"] + ";Database=" + ConfigurationManager.AppSettings["DBName"] + ";User Id=" + ConfigurationManager.AppSettings["DBUserName"] + ";Password=" + ConfigurationManager.AppSettings["Password"] + ";");

            AreaRegistration.RegisterAllAreas();
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_End()
        {
            _log.Info("******Application End******");
        }
    }
}
