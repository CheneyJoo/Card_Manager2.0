using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Api
{
    /// <summary>
    /// 用户请求参数
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 时间戳，秒（20180207142501）
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// MD5签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        ///接口权限 1代表康康体检网,2中康体检网 ,具体看xt_dsfbz表
        /// </summary>
        public int interfaceNum { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int pagesize { get; set; }
        /// <summary>
        /// 备用公共参数
        /// </summary>
        public string publicvalue { get; set; }

    }
}
