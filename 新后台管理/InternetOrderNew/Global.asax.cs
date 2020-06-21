using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Common;

namespace InternetOrder
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception exception = Server.GetLastError();
            //Log.WriteLog(exception.ToString()); //记录日志信息  
            //var httpStatusCode = (exception as HttpException)?.GetHttpCode() ?? 700; //如果为空则走自定义
            //var httpContext = ((Global)sender).Context;
            //httpContext.ClearError();

            //#region 直接跳转到对应错误页面
            //switch (httpStatusCode)
            //{
            //    case 404:
            //        httpContext.Response.Redirect("/Error/404.html");
            //        break;
            //    default:
            //        httpContext.Response.Redirect("/Error/500.html");
            //        break;
            //}
            //#endregion

        }
    }
}