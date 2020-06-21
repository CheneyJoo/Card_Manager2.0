using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 套餐
    /// </summary>
    public class XttcbModel
    {
        public XttcbModel()
        {
            this.jcyy = string.Empty;
            this.tclxbh = string.Empty;
            this.tctp = string.Empty;
        }
        public int id { get; set; }
        public string tcbh { get; set; }
        public string tcmc { get; set; }
        public string dwbh { get; set; }
        public string dwmc { get; set; }
        public decimal jg { get; set; }
        public decimal jsj { get; set; }
        public int sfqy { get; set; }
        public DateTime createtime { get; set; }
        /// <summary>
        /// 渠道id
        /// </summary>
        public int dsfbzid { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string qdmc { get; set; }
        /// <summary>
        /// 性别1男,0女，2通用
        /// </summary>
        public int xb { get; set; }
        /// <summary>
        /// 0未婚,1已婚,2不限
        /// </summary>
        public int hz { get; set; }
        public string yybh { get; set; }
        /// <summary>
        /// 开放平台
        /// </summary>
        public string kfpt { get; set; }
        /// <summary>
        /// 套餐类型,1团体套餐，0个人套餐,2渠道套餐
        /// </summary>
        public int tclx { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string jgmc { get; set; }

        /// <summary>
        /// 是否显示真实价格
        /// </summary>
        public int sfxsjg { get; set; }




        public string jcyy { get; set; }
        public int tcpx { get; set; }
        public string tclxbh { get; set; }
        public string tctp { get; set; }

    }
    /// <summary>
    /// 套餐下的组合项目关联
    /// </summary>
    public class XttczhxmbModel
    {
        public int id { get; set; }
        /// <summary>
        /// 组合项目编号
        /// </summary>
        public string zhxmbh { get; set; }
        /// <summary>
        /// 套餐编号
        /// </summary>
        public string tcbh { get; set; }
        /// <summary>
        /// 机构id
        /// </summary>
        public string yybh { get; set; }
        /// <summary>
        /// 单位编号
        /// </summary>
        public string dwbh { get; set; }
        public DateTime createtime { get; set; }
    }
    /// <summary>
    /// 套餐下的组合项目明细
    /// </summary>
    public class XttczhxmbmxModel
    {
        public int id { get; set; }
        /// <summary>
        /// 机构编号
        /// </summary>
        public string yybh { get; set; }
        /// <summary>
        /// 组合项目编号
        /// </summary>
        public string zhxmbh { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string zhxmmc { get; set; }
        /// <summary>
        /// 描述意义
        /// </summary>
        public string zhxmms { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string zhxmjg { get; set; }
        /// <summary>
        /// 性别 0女,1男,2通用
        /// </summary>
        public int xb { get; set; }
        /// <summary>
        /// 是否妇科,1 是0 否
        /// </summary>
        public int sffk { get; set; }
        /// <summary>
        /// 是否启用,1启,0关
        /// </summary>
        public int sfqy { get; set; }
        ///// <summary>
        ///// 科室类型
        ///// </summary>
        //public string zhxmksbh { get; set; }
        ///// <summary>
        ///// 科室名称
        ///// </summary>
        //public string zhxmksmc { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime updatetime { get; set; }

    }
}
