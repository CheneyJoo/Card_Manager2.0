using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 企业加项包
    /// </summary>
    public class QyJxbJbxxModel
    {
        public int id { get; set; }
        public string yybh { get; set; }
        public string qybh { get; set; }
        public string qymc { get; set; }
        /// <summary>
        /// 加项包名称
        /// </summary>
        public string jxbmc { get; set; }
        /// <summary>
        /// 体检内容介绍
        /// </summary>
        public string lrjs { get; set; }
        /// <summary>
        /// 适宜人群
        /// </summary>
        public string syrq { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int sfqy { get; set; }
        public decimal jg { get; set; }
        public int xb { get; set; }
        public decimal jsj { get; set; }
        public DateTime createtime { get; set; }
        public DateTime updatetime { get; set; }
        /// <summary>
        /// 组合项目，用逗号隔开
        /// </summary>
        public string zhxms { get; set; }
        public string zhxmbhs { get; set; }
        /// <summary>
        /// 套餐，用逗号隔开
        /// </summary>
        public string tcmcs { get; set; }
        public string tcbhs { get; set; }
        public List<QyJxbZhxmModel> lZhxm { get; set; }
        public List<QyJxbTcModel> ltc { get; set; }
    }
    /// <summary>
    /// 加项包组合项目
    /// </summary>
    public class QyJxbZhxmModel
    {
        public int id { get; set; }
        public int jxbid { get; set; }
        public string yybh { get; set; }
        public string zhxmbh { get; set; }
        public string zhxmmc { get; set; }
        public decimal jg { get; set; }

    }
    /// <summary>
    /// 加项包适用套餐
    /// </summary>
    public class QyJxbTcModel
    {
        public int id { get; set; }
        public int jxbid { get; set; }
        public string yybh { get; set; }
        public string tcbh { get; set; }
        public string tcmc { get; set; }
    }
}
