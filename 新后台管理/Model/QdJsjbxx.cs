using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 渠道结算基本信息
    /// </summary>
    public class QdJsjbxx
    {
        /// <summary>
        /// 主键自增Id
        /// </summary>
        public int id { get; set;}
        /// <summary>
        /// 结算编号
        /// </summary>
        public string jsbh { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string yybh { get; set; }
        /// <summary>
        /// 渠道Id
        /// </summary>
        public int qdid { get; set; }
        /// <summary>
        /// 状态 1.审核中2.付款中3.被拒绝4.已完成5.已付款
        /// </summary>
        public int zt { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime tjsj { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public int tjr { get; set; }
        /// <summary>
        /// 原始结算总额
        /// </summary>
        public  decimal ysjsze { get; set; }
        /// <summary>
        /// 实际结算总额
        /// </summary>
        public decimal sjjsze { get; set; }
        /// <summary>
        /// 渠道名称(第三方标识)
        /// </summary>
        public string dsfbz { get; set; }
        /// <summary>
        /// 渠道对接人
        /// </summary>
        public string lxr { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string lxdh { get; set; }
        /// <summary>
        /// 退款总额
        /// </summary>
        public decimal tkze { get; set; }
        /// <summary>
        /// 调整总额
        /// </summary>
        public decimal tzze { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string jgmc { get; set; }
        /// <summary>
        /// 有结算调整的订单数量
        /// </summary>
        public int ddtzsl { get; set; }
        public byte[] fkpz { get; set; }
        public string jjyy { get; set; }
    }
}
