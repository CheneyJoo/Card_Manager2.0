using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class PayHelper
    {

        public static string h5pay_url = ConfigurationManager.AppSettings["h5pay_url"];
        public static string notify_url = "http://" + HttpContext.Current.Request.Url.Authority + "/pay/notify";
        public static string return_url = "http://" + HttpContext.Current.Request.Url.Authority + "/pay/return";
        public static string appid = ConfigurationManager.AppSettings["appid"];
        public static string secret = ConfigurationManager.AppSettings["secret"];
        public static string alipay_refund_url = ConfigurationManager.AppSettings["alipay_refund_url"];
        public static string weixin_refund_url = ConfigurationManager.AppSettings["weixin_refund_url"];
        public static string refund_notify_url = "http://" + HttpContext.Current.Request.Url.Authority + "/refund/notify";


        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string GetParam(NameValueCollection dic, string secret)
        {
            string[] arrKeys = dic.AllKeys.ToArray();
            Array.Sort(arrKeys, string.CompareOrdinal);
            StringBuilder str = new StringBuilder();
            StringBuilder param = new StringBuilder();
            foreach (var key in arrKeys)
            {
                param.AppendFormat("{0}={1}&", key, HttpUtility.UrlEncode(dic[key]));
                str.AppendFormat("{0}={1}&", key, dic[key]);
            }

            string sign = Encryptor.MD5(str.ToString() + "secret=" + secret);
            return param.ToString() + "sign=" + sign;
        }

        /// <summary>
        /// 请求Url，发送数据
        /// </summary>
        public static string PostUrl(string url, string postData)
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);

            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream outstream = request.GetRequestStream();
            outstream.Write(data, 0, data.Length);
            outstream.Close();
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
    }
}
