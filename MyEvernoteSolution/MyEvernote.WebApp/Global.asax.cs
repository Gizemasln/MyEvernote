using MyEvernote.WebApp.Init;
using MyEvernoteCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyEvernote.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //proje aya�a kald���nda �al��an k�s�m
            App.Common = new WebCommon();
        }
    }
}
