using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.ReportQd
{
    /// <summary>
    /// 医院营收报表
    /// </summary>
    public class YyYsReport
    {
        public int xh { get; set; }
        public string qdmc { get; set; }
        /// <summary>
        /// 订单总数
        /// </summary>
        public int ddzs { get; set; }
        /// <summary>
        /// 退款订单数
        /// </summary>
        public int tkzs { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal ddze { get; set; }
        /// <summary>
        /// 退款总额
        /// </summary>
        public decimal tkze { get; set; }
        /// <summary>
        /// 渠道id
        /// </summary>
        public int qdid { get; set; }
    }
}
