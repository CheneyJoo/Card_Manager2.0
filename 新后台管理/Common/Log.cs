using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class Log
    {
        /// <summary>
        /// 新的日志记录方法。记录在同一文件。
        /// </summary>
        /// <param name="postString"></param>
        public static void WriteLog(string postString)
        {
            var logPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/APPDATA/temp_{0}/", DateTime.Now.ToString("yyyy-MM-dd")));

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string logfile = Path.Combine(logPath, string.Format("{0}.txt", DateTime.Now.ToString("yyyy-MM-dd")));

            try
            {
                postString += "\r\n";
                using (StreamWriter sw = new StreamWriter(logfile, true, Encoding.UTF8))
                {
                    sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + postString);
                    sw.Close();
                }
            }
            catch { }
        }
    }
}
