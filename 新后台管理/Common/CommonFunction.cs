
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common
{
   public static class CommonFunction
    {
        /// <summary>  
        　/// Base64加密  
        　/// </summary>  
        　/// <param name="Message"></param>  
        　/// <returns></returns>  
        public static string Base64Code(string Message)
        {
            byte[] bytes = Encoding.Default.GetBytes(Message);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>  
        　/// Base64解密  
        　/// </summary>  
        　/// <param name="Message"></param>  
        　/// <returns></returns>  
        public static string Base64Decode(string Message)
        {
            byte[] bytes = Convert.FromBase64String(Message);
            return Encoding.Default.GetString(bytes);
        }
        /// <summary>
        /// NameValueCollection转json
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToDictionary(NameValueCollection source)
        {
            IDictionary<string, string> dict = source.AllKeys.ToDictionary(k => k, k => source[k]);
            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }
        /// 某个年月有多少天 
        /// </summary> 
        /// <param name="y"></param> 
        /// <param name="m"></param> 
        /// <returns></returns> 
        public static int HowMonthDay(int y, int m)
        {
            int mnext;
            int ynext;
            if (m < 12)
            {
                mnext = m + 1;
                ynext = y;
            }
            else
            {
                mnext = 1;
                ynext = y + 1;
            }
            DateTime dt1 = System.Convert.ToDateTime(y + "-" + m + "-1");
            DateTime dt2 = System.Convert.ToDateTime(ynext + "-" + mnext + "-1");
            TimeSpan diff = dt2 - dt1;
            return diff.Days;
        }
        /// <summary>
        /// ///数字转字母,要转换成字母的数字（数字范围在闭区间[1,26]）
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NumToChar(int number)
        {
            if (number > 26 || number < 1) return string.Empty;
            number += 64;
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            byte[] btNumber = new byte[] { (byte)number };
            return asciiEncoding.GetString(btNumber);

        }
    }
}
