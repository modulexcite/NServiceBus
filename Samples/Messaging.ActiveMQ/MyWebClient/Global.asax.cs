﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyWebClient
{
    using Injection;
    using NServiceBus;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Configure.With()
               .DefaultBuilder()
               .ForMvc()
               .XmlSerializer(dontWrapSingleMessages:true)
               .ActiveMqTransport()
                   .DontUseTransactions()
                   .PurgeOnStartup(true)
               .UnicastBus()
                   .ImpersonateSender(false)
               .CreateBus()
               .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
        }
    }
}