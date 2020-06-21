using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 渠道结算基本信息明细
    /// </summary>
    public class QdJsjbxxmx
    {
        /// <summary>
        /// 主键自增Id
        /// </summary>
        public int id { get; set;}
        /// <summary>
        /// 结算基本信息Id
        /// </summary>
        public int jbxxid { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string ddbh { get; set; }
        /// <summary>
        /// 原始结算价
        /// </summary>
        public  decimal ysjsj { get; set; }
        /// <summary>
        /// 退款总额
        /// </summary>
        public  decimal tkze { get; set; }
        /// <summary>
        /// 结算调整
        /// </summary>
        public  decimal jstz { get; set; }
        /// <summary>
        /// 实际结算价
        /// </summary>
        public decimal sjjsj { get; set; }
        /// <summary>
        /// 第三方标识
        /// </summary>
        public string dsfbz { get; set; }
        /// <summary>
        /// 医院订单号
        /// </summary>
        public string dsfdd { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string tcmc { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string xm { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string dh { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int zt { get; set; }
        /// <summary>
        /// 计划结算调整
        /// </summary>
        public decimal jhjstz { get; set; }
        /// <summary>
        /// 调整原因
        /// </summary>
        public string tzyy { get; set; }
        public string jgmc { get; set; }
    }
}
