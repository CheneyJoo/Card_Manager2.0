using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReturnModel
    {
        /// <summary>
        /// 返回代码 200:成功 201:失败
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回结果集
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }
    }
}
