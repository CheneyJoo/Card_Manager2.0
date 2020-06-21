using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 企业基本信息
    /// </summary>
    public class QyJbxxModel
    {
        public int id { get; set; }
        public string yybh { get; set; }
        public string bh { get; set; }
        public string mc { get; set; }
        public string dwfzr { get; set; }
        public string lxdh { get; set; }
        public string lxdz { get; set; }
        /// <summary>
        /// 0企业，1部门
        /// </summary>
        public int isdept { get; set; }
        public DateTime createtime { get; set; }
        public DateTime updatetime { get; set; }
        /// <summary>
        /// 软件中最后修改时间
        /// </summary>
        public DateTime zhxgrq { get; set; }
        /// <summary>
        /// 是否已经同步到中间平台
        /// </summary>
        public int istb { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int sfqy { get; set; }
        /// <summary>
        /// 是否为渠道
        /// </summary>
        public int sfqd { get; set; }
        /// <summary>
        /// 是否显示套餐价格
        /// </summary>
        public int sfxstcje { get; set; }
        public string zh { get; set; }
        public string mm { get; set; }
    }
}
