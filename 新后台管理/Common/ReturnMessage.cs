using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 接口返回数据类
    /// </summary>
    public class ReturnMessage
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object result { get; set; }
    }


    /// <summary>
    /// 基础消息
    /// </summary>
    public class MsgModel
    {
        public string Code { get; set; }
        public string Msg { get; set; }

    }
}
