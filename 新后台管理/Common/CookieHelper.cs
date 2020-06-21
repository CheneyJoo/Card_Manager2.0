using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class CookieHelper
    {
        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        public static void ClearCookie(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            string str = string.Empty;
            if (cookie != null)
            {
                str = HttpUtility.UrlDecode(cookie.Value, Encoding.GetEncoding("UTF-8"));
            }
            return str;
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void SetCookie(string cookiename, string cookievalue, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookiename)
            {
                Value = HttpUtility.UrlEncode(cookievalue, Encoding.GetEncoding("UTF-8")),
                Expires = expires
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        /// <returns></returns>
        public static T Get<T>(string cookiename)
        {
            string value = HttpUtility.UrlDecode(GetCookieValue(cookiename));
            return JsonConvert.DeserializeObject<T>(value);
        }

        #region  张凯 2016-03-16 增加cookie时间为0就默认浏览器关闭过期  同时cookie值加入编码  获取自动解码
        #region  增加一个Cookies
        /// <summary>
        /// 增加一个Cookies  编码cookie值
        /// </summary>
        /// <param name="name">cookie名</param>
        /// <param name="value">cookie值</param>
        /// <param name="day">过期天数</param>
        public static void AddCookies(string name, string value, int day)
        {
            HttpCookie hc = new HttpCookie(name);
            hc.Value = HttpUtility.UrlEncode(value);
            if (day > 0)
            {
                hc.Expires = DateTime.Now.AddDays(day);
            }
            HttpContext.Current.Response.Cookies.Add(hc);
        }
        #endregion


        #region 读取cooike的值 +string ReadCookies(string name)
        /// <summary>
        /// 读取cooike的值  解码cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ReadCookies(string name)
        {
            if (HttpContext.Current.Request.Cookies[name] != null)
            {
                return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[name].Value);
            }
            else
            {
                return "";
            }
        }
        #endregion

        #endregion
    }
}
