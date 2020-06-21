using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 渠道退款MOdel
    /// </summary>
    public class QuDaoTkModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string ddbh { get; set; }
        public decimal tkje { get; set; }
        /// <summary>
        /// 申请退款时间
        /// </summary>
        public DateTime sqtksj { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string tkyy { get; set; }
        /// <summary>
        /// 退款状态 1,退款中, 2退款成功,3驳回,0作废
        /// </summary>
        public int tkzt { get; set; }
        /// <summary>
        /// 渠道id
        /// </summary>
        public int qdid { get; set; }
        /// <summary>
        /// 申请人id
        /// </summary>
        public int sqrid { get; set; }
        /// <summary>
        /// 审核原因
        /// </summary>
        public string shyy { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>

        public int shrid { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime shsj { get; set; }
    }
}
