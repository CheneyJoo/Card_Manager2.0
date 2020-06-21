using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.ReportQd
{
    public class OrderDayReport
    {
        public string dwbh { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        public string Qdmc { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int Zrs { get; set; }
        /// <summary>
        /// 预约人数
        /// </summary>
        public int Yyrs { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public string Bfb
        {
            get
            {
                if (Zrs <= Yyrs)
                {
                    return "100%";
                }
                else
                {
                    return (Convert.ToDecimal(Yyrs) / Convert.ToDecimal(Zrs)).ToString("0.##%");
                }
              
            }
        }
    }
}
