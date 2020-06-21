using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class WebHelper
    {
        public static string PostUrl(string url, string postData)
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            // 设置参数

            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json";
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
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受  
            return true;
        }

        /// <summary>
        /// post函数1 验证授权
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="postData">参数</param>
        /// <param name="Header">校验头部</param>
        /// <returns></returns>
        public static string PostJson_OAuth2(string url, string postJson, NameValueCollection Headers)
        {
            byte[] responseData = null;

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var webClient = new WebClient())
            {
                //添加表头验证信息
                for (int i = 0; i < Headers.Keys.Count; i++)
                {
                    webClient.Headers.Add(Headers.Keys[i].ToString(), Headers[i].ToString());
                }
                webClient.Headers.Add("Content-Type", "application/json");
                responseData = webClient.UploadData(url, "POST", Encoding.UTF8.GetBytes(postJson));
                string msg = System.Text.RegularExpressions.Regex.Unescape(Encoding.UTF8.GetString(responseData));
                return msg;
            }

        }
        /// <summary>
        /// post函数3 验证授权 post字符串
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="postData">参数</param>
        /// <param name="Header">校验头部</param>
        /// <returns></returns>
        public static string PostData_OAuth2_V3(string url,string jsonStr, NameValueCollection Headers)
        {
            byte[] responseData = null;

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var webClient = new WebClient())
            {
                //添加表头验证信息
                for (int i = 0; i < Headers.Keys.Count; i++)
                {
                    webClient.Headers.Add(Headers.Keys[i].ToString(), Headers[i].ToString());
                }

                webClient.Headers.Add("Content-Type", "application/json");
                responseData = webClient.UploadData(url, "POST", Encoding.UTF8.GetBytes(jsonStr));

                string msg = System.Text.RegularExpressions.Regex.Unescape(Encoding.UTF8.GetString(responseData));
                return msg;
            }

        }
        /// <summary>
        /// post函数1 验证授权
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="postData">参数</param>
        /// <param name="Header">校验头部</param>
        /// <returns></returns>
        public static string PostData_OAuth2_V1(string url, NameValueCollection parameters, NameValueCollection Headers,string type)
        {
            byte[] responseData = null;

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var webClient = new WebClient())
            {
                //添加表头验证信息
                for (int i = 0; i < Headers.Keys.Count; i++)
                {
                    webClient.Headers.Add(Headers.Keys[i].ToString(), Headers[i].ToString());
                }

                if (type == "json")
                {
                    webClient.Headers.Add("Content-Type", "application/json");
                    string jsonStr = CommonFunction.ToDictionary(parameters);
                    responseData = webClient.UploadData(url, "POST", Encoding.UTF8.GetBytes(jsonStr));

                }
                else
                {
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    responseData = webClient.UploadValues(url, "POST", parameters);

                }
                string msg = System.Text.RegularExpressions.Regex.Unescape(Encoding.UTF8.GetString(responseData));
                return msg;
            }

        }
        /// <summary>
        /// post函数2 验证授权
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="postData">参数</param>
        /// <param name="Header">校验头部</param>
        /// <returns></returns>
        public static string PostData_OAuth2_V2(string url, NameValueCollection parameters, NameValueCollection Headers, string type)
        {

            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            //添加参数
            string jsonStr = CommonFunction.ToDictionary(parameters);
            byte[] data = Encoding.UTF8.GetBytes(jsonStr);

            request.Method = "Post";
            request.ContentType = type=="json"?"application/json": "application/x-www-form-urlencoded";
            request.Timeout = 1000 * 600;
            request.PreAuthenticate = true;

            ////时间
            //DateTime timeStamp = new DateTime(1970, 1, 1); //得到1970年的时间戳
            //long a = (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000; //注意这里有时区问题，用now就要减掉8个小时 
            //request.Headers.Add("god-portal-timestamp", a.ToString());
            //request.Headers.Add("god-portal-request-id", Guid.NewGuid().ToString());

            //添加表头验证信息
            for (int i = 0; i < Headers.Keys.Count; i++)
            {
                request.Headers.Add(Headers.Keys[i].ToString(), Headers[i].ToString());
            }
            //request.Headers.Add(HttpRequestHeader.Authorization, Headers[0].ToString());

            request.ContentLength = data.Length;
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            string responseString = string.Empty;
            using ( StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                responseString = sr.ReadToEnd();
                //result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseString);
            }

            return responseString;
        }

        public static string PostJsonTopskyApi(NameValueCollection parameters, string apiName)
        {
            string responseString = string.Empty;
            try
            {
                var api = ConfigurationManager.AppSettings["TopskyApiUrl"];
                string url = api + apiName;
                HttpWebRequest request = null;
                //如果是发送HTTPS请求  
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
                request.Method = "Post";
                request.ContentType = "application/json";

                StringBuilder buffer = new StringBuilder();
                int i = 0;
                parameters.Add("PlatForm", "1");
                parameters.Add("Token", "topsky");

                foreach (string key in parameters.Keys)
                {
                    buffer.AppendFormat("{0}:'{1}',", key, parameters[key]);
                }
                request.Timeout = 1000 * 600;
                string str = "{" + buffer.ToString().TrimEnd(',') + "}";
                byte[] data = Encoding.UTF8.GetBytes(str);

                request.ContentLength = data.Length;
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();


                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    responseString = sr.ReadToEnd();
                    //result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseString);
                }
            }
            catch (Exception ex)
            {

            }
            return responseString;
        }


    }
}
