using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data.Common;
using System.Web.Script.Serialization;

namespace Common
{
    public class ConvertJson
    {
        #region 私有方法
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string StringFormat(object obj, Type type)
        {
            string str = string.Empty;
            if (type == typeof(string))
            {
                str = String2Json(obj.ToString());
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                    str = "null";
                else
                {
                    DateTime dt = Convert.ToDateTime(obj);
                    //str = "\"" + dt.ToString("yyyy/MM/dd hh:mm:ss") + "\"";
                    str = "\"" + dt.ToString("yyyy/MM/dd HH:mm:ss") + "\"";
                }
            }
            else if (type == typeof(bool))
            {
                str = obj.ToString().ToLower();
            }
            else if (type == typeof(decimal) || type == typeof(System.Int64) || type == typeof(float) || type == typeof(byte))
            {
                str = obj.ToString();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(Convert.ToString(obj)))
            {
                str = "\"" + str + "\"";
            }
            else
            {
                str = obj.ToString();
            }
            return str;
        }

        #endregion

        #region list转换成JSON
        /// <summary>
        /// list转换为Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }
        /// <summary>
        /// list转换为json
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="list"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string ListToJson<T>(IList<T> list, string JsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(JsonName))
                JsonName = list[0].GetType().Name;
            Json.Append("{\"" + JsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type = pi[j].GetValue(list[i], null).GetType();
                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));
                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        #endregion

        #region 对象转换为Json
        /// <summary>
        /// 对象转换为json
        /// </summary>
        /// <param name="jsonObject">json对象</param>
        /// <returns>json字符串</returns>
        public static string ToJson(object jsonObject)
        {
            string jsonString = "{";
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                string value = string.Empty;
                if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                {
                    value = "'" + objectValue.ToString() + "'";
                }
                else if (objectValue is string)
                {
                    value = "'" + ToJson(objectValue.ToString()) + "'";
                }
                else if (objectValue is IEnumerable)
                {
                    value = ToJson((IEnumerable)objectValue);
                }
                else
                {
                    value = ToJson(objectValue.ToString());
                }
                jsonString += "\"" + ToJson(propertyInfo[i].Name) + "\":" + value + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "}";
        }

        /// <summary>  
        /// 将对象转化为Json字符串   
        /// </summary>  
        /// <typeparam name="T">源类型</typeparam>  
        /// <param name="obj">源类型实例</param>  
        /// <returns>Json字符串</returns>  
        public static string ToJson<T>(T obj)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, obj);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>  
        /// 将Json字符串转化为对象  
        /// </summary>  
        /// <typeparam name="T">目标类型</typeparam>  
        /// <param name="strJson">Json字符串</param>  
        /// <returns>目标类型的一个实例</returns>  
        public static T GetObjFromJson<T>(string strJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson)))
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
                return (T)jsonSerializer.ReadObject(ms);
            }
        }

        #endregion

        #region 对象集合转换为json
        /// <summary>
        /// 对象集合转换为json
        /// </summary>
        /// <param name="array">对象集合</param>
        /// <returns>json字符串</returns>
        public static string ToJson(IEnumerable array)
        {
            string jsonString = "{";
            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region 普通集合转换Json
        /// <summary>    
        /// 普通集合转换Json   
        /// </summary>   
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns>  
        public static string ToArrayString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region  DataSet转换为Json
        /// <summary>    
        /// DataSet转换为Json   
        /// </summary>    
        /// <param name="dataSet">DataSet对象</param>   
        /// <returns>Json字符串</returns>    
        public static string ToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }
        #endregion

        #region Datatable转换为Json
        /// <summary>     
        /// Datatable转换为Json     
        /// </summary>    
        /// <param name="table">Datatable对象</param>     
        /// <returns>Json字符串</returns>     
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    //string strValue = drc[i][j].ToString();
                    object objValue = drc[i][j];
                    string strValue = string.Empty;
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(objValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.Length > 1)
                jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            string p1 = @"\d{4}/\d{2}/\d{2}\s\d{2}:\d{2}:\d{2}";
            string p2 = @"\d{4}/\d{2}/\d{2}\s\d{1}:\d{2}:\d{2}";
            string p3 = @"\d{4}/\d{1}/\d{1}\s\d{1}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(StringUtil.ConvertDateStringToJsonDate);
            Regex reg1 = new Regex(p1);
            Regex reg2 = new Regex(p2);
            Regex reg3 = new Regex(p3);
            string jsonStr = reg1.Replace(jsonString.ToString(), matchEvaluator);
            jsonStr = reg2.Replace(jsonStr, matchEvaluator);
            jsonStr = reg3.Replace(jsonStr, matchEvaluator);
            return jsonStr;
        }
        /// <summary>     
        /// Datatable转换为Json     
        /// </summary>    
        /// <param name="table">Datatable对象</param>     
        /// <returns>Json字符串</returns>     
        public static string ToJsontest(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    //string strValue = drc[i][j].ToString();
                    object objValue = drc[i][j];
                    string strValue = string.Empty;
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(objValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>    
        /// DataTable转换为Json     
        /// </summary>    
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>
        ///  DataTable转换为Json,判断是否转换时间格式  
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="changetime"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt, bool changetime)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    //string strValue = drc[i][j].ToString();
                    object objValue = drc[i][j];
                    string strValue = string.Empty;
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(objValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.Length > 1)
                jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            string jsonStr = jsonString.ToString();
            if (changetime)
            {
                string p1 = @"\d{4}/\d{2}/\d{2}\s\d{2}:\d{2}:\d{2}";
                string p2 = @"\d{4}/\d{2}/\d{2}\s\d{1}:\d{2}:\d{2}";
                string p3 = @"\d{4}/\d{1}/\d{1}\s\d{1}:\d{2}:\d{2}";
                MatchEvaluator matchEvaluator = new MatchEvaluator(StringUtil.ConvertDateStringToJsonDate);
                Regex reg1 = new Regex(p1);
                Regex reg2 = new Regex(p2);
                Regex reg3 = new Regex(p3);
                jsonStr = reg1.Replace(jsonStr, matchEvaluator);
                jsonStr = reg2.Replace(jsonStr, matchEvaluator);
                jsonStr = reg3.Replace(jsonStr, matchEvaluator);
            }
            return jsonStr;
        }
        #endregion

        #region DataReader转换为Json
        /// <summary>     
        /// DataReader转换为Json     
        /// </summary>     
        /// <param name="dataReader">DataReader对象</param>     
        /// <returns>Json字符串</returns>  
        public static string ToJson(DbDataReader dataReader)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            while (dataReader.Read())
            {
                jsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            dataReader.Close();
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        #endregion

        #region 将实体转化成Json
        /// <summary>
        /// 实体对象转换成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string EntityToJson(object obj)
        {
            StringBuilder jsonStr = new StringBuilder();
            PropertyInfo[] pInfos = obj.GetType().GetProperties();
            string pValue = string.Empty;
            jsonStr.Append("{");
            foreach (PropertyInfo p in pInfos)
            {
                if (!(p.GetValue(obj, null) == null))
                {
                    //转义掉Json格式特殊字符 ‘\’,‘"’
                    pValue = p.GetValue(obj, null).ToString().Replace("\\", "\\\\").Replace("\"", "\\\"");
                }
                else
                {
                    pValue = string.Empty;
                }
                jsonStr.Append(string.Format("\"{0}\":\"{1}\",", p.Name, pValue));
            }
            jsonStr.Remove(jsonStr.Length - 1, 1);
            jsonStr.Append("}");
            return jsonStr.ToString();
        }
        #endregion

        #region Json转化成List
        /// <summary>
        /// Json转化成List
        /// </summary>
        /// <param name="responseStr"></param>
        /// <returns></returns>
        public static List<T> ResponseList<T>(string responseStr)
        {
            List<T> list = new List<T>();
            DataContractJsonSerializer _json = new DataContractJsonSerializer(list.GetType());
            byte[] _using = System.Text.Encoding.UTF8.GetBytes(responseStr);
            System.IO.MemoryStream memoryStream = new MemoryStream(_using);
            memoryStream.Position = 0;
            list = (List<T>)_json.ReadObject(memoryStream);
            return list;
        }

        /// <summary>
        /// JSON转换成Dictionary
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDictionary(string json)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();

            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Dictionary<string, object>>(json);
            }
            catch (Exception ex)
            {
                DataTable dt = ConvertJson.JsonToDataTable("[" + json + "]");
                //DataTable dt = ConvertJson.JsonToDataTable("[" + json + "]");
                //DataTable dt = (DataTable)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(json, typeof(DataTable));
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dic.Add(dt.Columns[i].ColumnName, dt.Rows[0][i]);
                    //dic.Add(dt.Columns[i].ColumnName, "");
                }
                return dic;


                throw new Exception(ex.Message);
            }

        }
        #endregion
        /// <summary>
        /// 实体对象转Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetPropertiesDictionary<T>(T t) where T : class
        {
            Type type = t.GetType();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            PropertyInfo[] props = type.GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                dict.Add(props[i].Name, props[i].GetValue(t));
            }
            return dict;
        }

        #region json转实体类
        public static T JsonToModel<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer dcj = new DataContractJsonSerializer(typeof(T));
                return (T)dcj.ReadObject(ms);
            }
        }
        #endregion

        /// <summary>
        /// 将实体对象列表转换成功JSON字符串
        /// </summary>
        /// <param name="objList">对象列表</param>
        /// <returns></returns>
        public static string EntityListToJson<T>(List<T> objList)
        {
            if (objList == null || objList.Count == 0)
                return "[]";
            string resList = "[";

            for (int i = 0; i < objList.Count; i++)
            {
                if (i == 0)
                    resList += EntityToJson(objList[i]);
                else
                    resList += "," + EntityToJson((object)objList[i]);
            }

            resList += "]";

            return resList;
        }

        /// <summary>
        /// Json to datatable 满足 格式  [{"A","a"}]
        /// add by ryan.ruan
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTableForDic(string strJson)
        {
            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.LastIndexOf("]"));

            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');

                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        if (str == "\"")
                        {
                            continue;
                        }
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');

                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                ////增加内容   
                DataRow dr = tb.NewRow();
                int rindex = 0;
                for (int r = 0; r < strRows.Length; r++)
                {
                    if (strRows[r] == "\"")
                    {
                        continue;
                    }
                    dr[rindex] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                    rindex++;
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }

        /// <summary>
        /// Json to datatable 满足 格式  [{"A","a"}]
        /// add by ryan.ruan
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            strJson = strJson.Replace("*", "$");
            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"や").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.LastIndexOf("]"));

            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');

                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('や');

                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    string newStr = strRows[r].Split('や')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                    newStr = newStr.Replace("$", "*");


                    dr[r] = Security.UnEscape(newStr);
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }

        public static string Escape(string s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] ba = System.Text.Encoding.Unicode.GetBytes(s);
            for (int i = 0; i < ba.Length; i += 2)
            {
                if (ba[i + 1] == 0)
                {
                    //数字,大小写字母,以及"+-*/._"不变
                    if (
                          (ba[i] >= 48 && ba[i] <= 57)
                        || (ba[i] >= 64 && ba[i] <= 90)
                        || (ba[i] >= 97 && ba[i] <= 122)
                        || (ba[i] == 42 || ba[i] == 43 || ba[i] == 45 || ba[i] == 46 || ba[i] == 47 || ba[i] == 95)
                        )//保持不变
                    {
                        sb.Append(Encoding.Unicode.GetString(ba, i, 2));

                    }
                    else//%xx形式
                    {
                        sb.Append("%");
                        sb.Append(ba[i].ToString("X2"));
                    }
                }
                else
                {
                    sb.Append("%u");
                    sb.Append(ba[i + 1].ToString("X2"));
                    sb.Append(ba[i].ToString("X2"));
                }
            }
            return sb.ToString();
        }
    }
}
