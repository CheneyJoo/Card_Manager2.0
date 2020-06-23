using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Net;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Data;

namespace Common
{
    /// <summary>
    ///  The StringUtil class serves as an utility to deal with a string.
    /// </summary>
    public static class StringUtil
    {
        //public static ILog log = log4net.LogManager.GetLogger(typeof(StringUtil));
        #region Public methods

        /// <summary>
        ///  Checks whether the passed in string is empty.
        /// </summary>
        /// 
        /// <param name="inputStr">String to be checked</param>
        /// 
        /// <returns>true if the string is null or just contains spaces, otherwise false</returns>
        public static bool IsEmpty(string inputStr)
        {
            return String.IsNullOrEmpty(inputStr) || String.IsNullOrEmpty(inputStr.Trim());
        }

        /// <summary>
        ///  Trims the passed in string by removing the spaces at the start and end.
        /// </summary>
        /// 
        /// <param name="inputStr">String to be trimed</param>
        /// 
        /// <returns>New string after trimed</returns>
        public static string Trim(string inputStr)
        {
            return inputStr == null ? null : inputStr.Trim();
        }

        #endregion

        #region Json
        /// <summary>
        /// Json序列化,用于发送到客户端
        /// </summary>
        public static string ToJsJson(this object item)
        {
            if (null == item)
            {
                return "[]";
            }
            Type ty1 = typeof(DataTable);

            if (item.GetType() == ty1)
            {
                DataTable dt = item as DataTable;
                StringBuilder jsonBuilder = new StringBuilder();
                jsonBuilder.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName.ToLower());
                        jsonBuilder.Append("\":\"");
                        jsonBuilder.Append(HttpUtility.JavaScriptStringEncode(dt.Rows[i][j].ToString()));
                        jsonBuilder.Append("\",");
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("]");
                return jsonBuilder.ToString();
            }
            else
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, item);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                    //替换Json的Date字符串        
                    string p = @"\\/Date\((\d+)\+\d+\)\\/";
                    MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                    Regex reg = new Regex(p);
                    string jsonString = sb.ToString();
                    jsonString = reg.Replace(jsonString, matchEvaluator);
                    return jsonString;
                }
            }
        }


        /// <summary>
        /// Json反序列化,用于接收客户端Json后生成对应的对象
        /// </summary>
        public static T FromJsonToBase<T>(this string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            T jsonObject = (T)ser.ReadObject(ms);

            ms.Close();

            return jsonObject;

        }
        /// <summary>
        /// Json反序列化,用于接收客户端Json后生成对应的对象
        /// </summary>
        public static T FromJsonTo<T>(this string jsonString)
        {
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
            string p1 = @"\d{4}/\d{2}/\d{2}\s\d{2}:\d{2}:\d{2}";
            string p2 = @"\d{4}/\d{2}/\d{2}";
            string p3 = @"\d{4}/\d{1}/\d{2}\s\d{2}:\d{2}:\d{2}";
            string p4 = @"\d{4}/\d{2}/\d{1}\s\d{2}:\d{2}:\d{2}";
            string p5 = @"\d{4}/\d{1}/\d{2}\s\d{1}:\d{2}:\d{2}";
            string p6 = @"\d{4}/\d{2}/\d{1}\s\d{1}:\d{2}:\d{2}";
            string p7 = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            string p8 = @"\d{4}-\d{2}-\d{2}";
            string p9 = @"\d{4}-\d{1}-\d{1}";
            string p10 = @"\d{4}-\d{2}-\d{1}";
            string p11 = @"\d{4}-\d{1}-\d{2}";
            string p12 = @"\d{4}/\d{2}/\d{2}";
            string p13 = @"\d{4}/\d{1}/\d{1}";
            string p14 = @"\d{4}/\d{2}/\d{1}";
            string p15 = @"\d{4}/\d{1}/\d{2}";
            string p16 = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}";
            string p17 = @"\d{4}/\d{2}/\d{2}\s\d{1}:\d{2}:\d{2}";

            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg1 = new Regex(p1);
            Regex reg2 = new Regex(p2);
            Regex reg3 = new Regex(p3);
            Regex reg4 = new Regex(p4);
            Regex reg5 = new Regex(p5);
            Regex reg6 = new Regex(p6);
            Regex reg7 = new Regex(p7);
            Regex reg8 = new Regex(p8);
            Regex reg9 = new Regex(p9);
            Regex reg10 = new Regex(p10);
            Regex reg11 = new Regex(p11);
            Regex reg12 = new Regex(p12);
            Regex reg13 = new Regex(p13);
            Regex reg14 = new Regex(p14);
            Regex reg15 = new Regex(p15);
            Regex reg16 = new Regex(p16);
            Regex reg17 = new Regex(p17);
            jsonString = reg1.Replace(jsonString, matchEvaluator);
            jsonString = reg2.Replace(jsonString, matchEvaluator);
            jsonString = reg3.Replace(jsonString, matchEvaluator);
            jsonString = reg4.Replace(jsonString, matchEvaluator);
            jsonString = reg5.Replace(jsonString, matchEvaluator);
            jsonString = reg6.Replace(jsonString, matchEvaluator);
            jsonString = reg7.Replace(jsonString, matchEvaluator);
            jsonString = reg16.Replace(jsonString, matchEvaluator);
            jsonString = reg8.Replace(jsonString, matchEvaluator);
            jsonString = reg9.Replace(jsonString, matchEvaluator);
            jsonString = reg10.Replace(jsonString, matchEvaluator);
            jsonString = reg11.Replace(jsonString, matchEvaluator);
            jsonString = reg12.Replace(jsonString, matchEvaluator);
            jsonString = reg13.Replace(jsonString, matchEvaluator);
            jsonString = reg14.Replace(jsonString, matchEvaluator);
            jsonString = reg15.Replace(jsonString, matchEvaluator);
            jsonString = reg17.Replace(jsonString, matchEvaluator);

            string match = "\"__type\":\"([^\\\"]|\\.)*\",";
            Regex regex = new Regex(match, RegexOptions.Singleline);
            jsonString = regex.Replace(jsonString, "");
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            T jsonObject = (T)ser.ReadObject(ms);

            ms.Close();

            return jsonObject;

        }
        /// <summary>    
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串    
        /// </summary>    
        public static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy/MM/dd HH:mm:ss");
            return result;
        }
        /// <summary>    
        /// 将时间字符串转为Json时间    
        /// </summary>    
        public static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
        ///// <summary>
        ///// 过滤特殊字符
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static string String2Json(String s)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        char c = s.ToCharArray()[i];
        //        switch (c)
        //        {
        //            case '\"':
        //                sb.Append("\\\""); break;
        //            case '\\':
        //                sb.Append("\\\\"); break;
        //            case '/':
        //                sb.Append("\\/"); break;
        //            case '\b':
        //                sb.Append("\\b"); break;
        //            case '\f':
        //                sb.Append("\\f"); break;
        //            case '\n':
        //                sb.Append("\\n"); break;
        //            case '\r':
        //                sb.Append("\\r"); break;
        //            case '\t':
        //                sb.Append("\\t"); break;
        //            default:
        //                sb.Append(c); break;
        //        }
        //    }
        //    return sb.ToString();
        //}
        #endregion
        #region Code128
        public static string Get128CodeString(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return string.Empty;
            string result;
            int checksum = 104;
            for (int ii = 0; ii < inputData.Length; ii++)
            {
                if (inputData[ii] >= 32)
                {
                    checksum += (inputData[ii] - 32) * (ii + 1);
                }
                else
                {
                    checksum += (inputData[ii] + 64) * (ii + 1);
                }
            }
            checksum = checksum % 103;
            if (checksum < 95)
            {
                checksum += 32;
            }
            else
            {
                checksum += 100;
            }
            result = Convert.ToChar(136) + inputData.ToString() + Convert.ToChar(checksum) + Convert.ToChar(138);
            return result;
        }
        #endregion
        #region 计算中英文混合字符串的长度
        /// <summary> 
        /// 计算字符串的长度，包括中英混合字符串的长度； 
        /// </summary> 
        /// <param name="Str">字符串</param> 
        public static int Length(String Str)
        {
            int n;
            if (string.IsNullOrEmpty(Str))
            {
                n = 0;
            }
            else
            {
                n = Encoding.UTF8.GetByteCount(Str);
            }
            return n;
        }
        #endregion
        #region 中文字符(全角)转英文字符(半角)
        /// <summary>
        /// 中文字符(全角)转英文字符(半角)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CharConverter(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                source = string.Empty;
                return source;
            }
            System.Text.StringBuilder result = new System.Text.StringBuilder(source.Length, source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] >= 65281 && source[i] <= 65373)
                {
                    result.Append((char)(source[i] - 65248));
                }
                else if (source[i] == 12288)
                {
                    result.Append(' ');
                }
                else
                {
                    result.Append(source[i]);
                }
            }
            return result.ToString();
        }
        #endregion
        #region 获取指定长度的中英文混合字符串
        /// <summary>
        /// 获取指定长度的中英文混合字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="len">要截取的长度</param>
        /// <returns></returns>
        public static string K8GetStrLength(string str, int len)
        {
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.UTF8Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = str.Substring(0, pos);
            }
            else
                result = str;

            return result;
        }
        #endregion
        ///<summary> 
        ///依据连接串名字connectionName返回数据连接字符串  
        ///</summary> 
        ///<param name="connectionName"></param> 
        ///<returns></returns> 
        public static string GetConnectionStringsConfig(string connectionName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString.ToString();
            Console.WriteLine(connectionString);
            return connectionString;
        }
        ///<summary>
        ///更新连接字符串  
        ///</summary> 
        ///<param name="newName">连接字符串名称</param> 
        ///<param name="newConString">连接字符串内容</param> 
        ///<param name="newProviderName">数据提供程序名称</param>
        public static void UpdateConnectionStringsConfig(string newName, string newConString, string newProviderName)
        {
            bool isModified = false;
            //记录该连接串是否已经存在      
            //如果要更改的连接串已经存在      
            if (ConfigurationManager.ConnectionStrings[newName] != null)
            {
                isModified = true;
            }
            //新建一个连接字符串实例      
            ConnectionStringSettings mySettings = new ConnectionStringSettings(newName, newConString, newProviderName);
            // 打开可执行的配置文件*.exe.config      
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 如果连接串已存在，首先删除它      
            if (isModified)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            // 将新的连接串添加到配置文件中.      
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改      
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节      
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }
        ///<summary> 
        ///返回＊.exe.config文件中appSettings配置节的value项  
        ///</summary> 
        ///<param name="strKey"></param> 
        ///<returns></returns> 
        public static string GetAppConfig(string strKey)
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return null;
        }
        ///<summary>  
        ///在＊.exe.config文件中appSettings配置节增加一对键、值对  
        ///</summary>  
        ///<param name="newKey"></param>  
        ///<param name="newValue"></param>  
        public static void UpdateAppConfig(string newKey, string newValue)
        {
            bool isModified = false;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == newKey)
                {
                    isModified = true;
                }
            }
            // Open App.Config of executable      
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // You need to remove the old settings object before you can replace it      
            if (isModified)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            // Add an Application Setting.      
            config.AppSettings.Settings.Add(newKey, newValue);
            // Save the changes in App.config file.      
            config.Save(ConfigurationSaveMode.Modified);
            // Force a reload of a changed section.      
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 获得param参数value
        /// </summary>
        /// <param name="param"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetParamValue(string param, string name)
        {
            if (param.IndexOf(name) < 0)
            {
                return "";
            }
            string value = "";
            value = param.Substring(param.IndexOf(name));
            int x = value.IndexOf(name) + name.Length + 10;
            value = value.Substring(x);
            int y = value.IndexOf('}');
            value = value.Substring(0, y);
            return value.Replace("\"", "");
        }
        //end


        /// <summary>
        /// 对参数过滤掉单引号
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string LegalParam(string param)
        {
            if (string.IsNullOrEmpty(param))
                return string.Empty;
            return param.Replace("'", "''").Trim();
        }

   
    }

}