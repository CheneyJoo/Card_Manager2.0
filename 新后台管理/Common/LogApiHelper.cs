using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class LogApiHelper
    {

        /// <summary>
        /// 是否开启记录日志 Y记录 N不记录
        /// </summary>
        public static string RecordLog = "Y";
        public static string LogFile
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogApi");
                //return Path.Combine(@"D:\ATMKKTJ_LOG\APPDATA", "Log");
            }
        }
        public static string TemporaryFile
        {
            get
            {
                return Path.Combine(LogFile, "Temporary");
                //return Path.Combine(@"D:\ATMKKTJ_LOG\APPDATA", "Log");
            }
        }

        public static string DayFile
        {
            get
            {
                return Path.Combine(LogFile, DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }
        public static string HourPath
        {
            get
            {
                return Path.Combine(DayFile, DateTime.Now.ToString("yyyyMMdd-HH") + ".txt");
            }
        }
        public static string ErrorLogFile
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrirLog");
                //return Path.Combine(@"D:\ATMKKTJ_LOG\ErrorLog", "ErrirLog");
            }
        }
        public static string ErrorDayFile
        {
            get
            {
                return Path.Combine(ErrorLogFile, DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }
        public static string ErrorHourPath
        {
            get
            {
                return Path.Combine(ErrorDayFile, DateTime.Now.ToString("yyyyMMdd-HH") + ".txt");
            }
        }

        /// <summary>
        /// 记录错误日志ErrorLog文件夹
        /// </summary>
        /// <param name="msg"></param>
        public static void AddErrorLog(string msg)
        {
            try
            {
                CreateDirectory(ErrorLogFile);
                CreateDirectory(ErrorDayFile);
                Write(ErrorHourPath, msg);
            }
            catch { }

        }

        /// <summary>
        /// 记录日志文本Log文件夹
        /// </summary>
        /// <param name="msg"></param>
        public static void AddLog(string msg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RecordLog) == false && RecordLog == "Y")
                {
                    CreateDirectory(LogFile);
                    CreateDirectory(DayFile);
                    Write(HourPath, msg);
                }
            }
            catch
            { }
        }


        public static void AddTemporary(string msg, string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RecordLog) == false && RecordLog == "Y")
                {
                    CreateDirectory(LogFile);
                    CreateDirectory(TemporaryFile);
                    string path = Path.Combine(TemporaryFile, fileName);
                    File.AppendAllText(path, msg + "  " + DateTime.Now.ToString() + "\r\n");
                }
            }
            catch
            { }
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="directoryName"></param>
        private static void CreateDirectory(string directoryName)
        {
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        private static void Write(string fileName, string msg)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Append))
            {
                string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
                text += msg;
                text += Environment.NewLine;
                //获得字节数组
                byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
            }
        }
    }
}
