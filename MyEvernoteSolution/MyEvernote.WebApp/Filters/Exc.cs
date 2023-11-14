using MyEvernote.WebApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Filters
{
    public class Exc : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Controller.TempData["LastError"] = filterContext.Exception; //TempData 2 request kadar ömrü var
            filterContext.ExceptionHandled = true; // ExceptionHandled = true Hatayı ben yöneteceğim demek
            filterContext.Result = new RedirectResult("/Home/HasError");
    }
}
}


