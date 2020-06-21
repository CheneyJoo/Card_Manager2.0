using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace InternetOrder
{
    public class GLYadmin
    {
        public static void GLYLoginOut()
        {
            if (HttpContext.Current.Session["GLYUserAccount"] != null)
            {
                HttpContext.Current.Session.Abandon();
            }
            if (HttpContext.Current.Request.Cookies["AdminInfo"] != null)
            {
                HttpCookie cookie = new HttpCookie("AdminInfo");
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.AppendCookie(cookie);

                HttpCookie firstcookie = new HttpCookie("firstInfo");
                firstcookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.AppendCookie(firstcookie);

            }
            GLYUserAccount = null;         
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GLYUserAccount
        {
            get
            {
                if (HttpContext.Current.Session["GLYUserAccount"] != null)
                {
                    string GLYUserAccount = HttpContext.Current.Session["GLYUserAccount"].ToString();

                    return Encoding.UTF8.GetString(Convert.FromBase64String(GLYUserAccount));
                }
                else if (HttpContext.Current.Request.Cookies["AdminInfo"] != null)
                {
                    string GLYUserAccount = HttpContext.Current.Request.Cookies["AdminInfo"]["GLYUserAccount"];
                    if (string.IsNullOrEmpty(GLYUserAccount))
                        return "";
                    else
                        return Encoding.UTF8.GetString(Convert.FromBase64String(GLYUserAccount));
                }
                return "";
            }
            set
            {


                HttpContext.Current.Session["GLYUserAccount"] = (value == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(value)));
                if (HttpContext.Current.Response.Cookies["AdminInfo"] == null)
                {
                    HttpCookie cookie = new HttpCookie("AdminInfo");
                    HttpContext.Current.Response.AppendCookie(cookie);
                }
                HttpContext.Current.Response.Cookies["AdminInfo"]["GLYUserAccount"] = (value == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(value)));
            }
       
        }
        /// <summary>
        /// 体检机构ID
        /// </summary>
        public static string YYID
        {
            get
            {
                if (HttpContext.Current.Session["YYID"] != null)
                {
                    return HttpContext.Current.Session["YYID"].ToString();
                }
                else if (HttpContext.Current.Request.Cookies["AdminInfo"] != null)
                {
                    return HttpContext.Current.Request.Cookies["AdminInfo"]["YYID"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["YYID"] = value;
                if (HttpContext.Current.Response.Cookies["AdminInfo"] == null)
                {
                    HttpCookie cookie = new HttpCookie("AdminInfo");
                    HttpContext.Current.Response.AppendCookie(cookie);
                }
                HttpContext.Current.Response.Cookies["AdminInfo"]["YYID"] = value;
            }
        }
        /// <summary>
        /// 角色Id
        /// </summary>
        public static string JsId
        {
            get
            {
                if (HttpContext.Current.Session["JsId"] != null)
                {
                    return HttpContext.Current.Session["JsId"].ToString();
                }
                else if (HttpContext.Current.Request.Cookies["AdminInfo"] != null)
                {
                    return HttpContext.Current.Request.Cookies["AdminInfo"]["JsId"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["JsId"] = value;
                if (HttpContext.Current.Response.Cookies["AdminInfo"] == null)
                {
                    HttpCookie cookie = new HttpCookie("AdminInfo");
                    HttpContext.Current.Response.AppendCookie(cookie);
                }
                HttpContext.Current.Response.Cookies["AdminInfo"]["JsId"] = value;
            }
        }

        public static string UserId
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return HttpContext.Current.Session["UserId"].ToString();
                }
                else if (HttpContext.Current.Request.Cookies["AdminInfo"] != null)
                {
                    return HttpContext.Current.Request.Cookies["AdminInfo"]["UserId"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;
                if (HttpContext.Current.Response.Cookies["AdminInfo"] == null)
                {
                    HttpCookie cookie = new HttpCookie("AdminInfo");
                    HttpContext.Current.Response.AppendCookie(cookie);
                }
                HttpContext.Current.Response.Cookies["AdminInfo"]["UserId"] = value;
            }
        }

        /// <summary>
        /// 验证码
        /// </summary>
        public static string VerifyCode
        {
            get
            {
                if (HttpContext.Current.Session["VerifyCode"] != null)
                {
                    return HttpContext.Current.Session["VerifyCode"].ToString();
                }
                else if (HttpContext.Current.Request.Cookies["AdminInfo"] != null)
                {
                    return HttpContext.Current.Request.Cookies["AdminInfo"]["VerifyCode"];
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["VerifyCode"] = value;
                if (HttpContext.Current.Response.Cookies["AdminInfo"] == null)
                {
                    HttpCookie cookie = new HttpCookie("AdminInfo");
                    HttpContext.Current.Response.AppendCookie(cookie);
                }
                HttpContext.Current.Response.Cookies["AdminInfo"]["VerifyCode"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="tjjgId">医院编号</param>
        /// <param name="strUserXM">联系人</param>
        /// <param name="userPassWord"></param>
        public static void GLYLoginIn(string account, string tjjgId,string strUserXM, int jsId,int userId)
        {
            GLYUserAccount = account;
            YYID = tjjgId;
            JsId = jsId.ToString();
            UserId = userId.ToString();
        }
    }
}