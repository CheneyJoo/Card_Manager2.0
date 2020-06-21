using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common
{
    /// <summary>
    /// Hashtable帮助类
    /// </summary>
    public class HashtableHelper
    {
        /// <summary>
        /// 字符串 分割转换 Hashtable   ≌; ☻
        /// </summary>
        public static Hashtable String_Key_ValueToHashtable(string str, Controller c = null)
        {
            Hashtable ht = new Hashtable();
            if (!string.IsNullOrEmpty(str))
            {
                string[] arrayParm_Key_Value = str.Split('≌');
                foreach (string item in arrayParm_Key_Value)
                {
                    if (item.Length > 0)
                    {
                        string[] Key_Value = item.Split('☻');
                        ht[Key_Value[0]] = Key_Value[1];
                        if (c != null)
                        {
                            c.ViewData[Key_Value[0].ToString()] = Key_Value[1];
                        }

                    }
                }
            }
            return ht;
        }
    }
}
