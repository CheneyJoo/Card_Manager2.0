using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    /// <summary>
    /// 企业预约管理Model
    /// </summary>
    public class QyyyModel
    {
        public int id { get; set; }
        public string dwmc { get; set; }
        /// <summary>
        /// 排期时间段
        /// </summary>
        public DateTime ksrq { get; set; }
        /// <summary>
        /// 排期结束时间
        /// </summary>
        public DateTime jsrq { get; set; }
        /// <summary>
        /// 体检人数
        /// </summary>
        public int tjrs { get; set; }
        /// <summary>
        /// 预留名额
        /// </summary>
        public int ylme { get; set; }
        /// <summary>
        /// 预约人数
        /// </summary>
        public int yyrs { get; set; }
        /// <summary>
        /// 到检人数
        /// </summary>
        public int djrs { get; set; }
        /// <summary>
        /// 预约率
        /// </summary>
        public string yyl { get; set; }
        /// <summary>
        /// 到检进度
        /// </summary>
        public string djjd { get; set; }
        /// <summary>
        /// 医院编号 
        /// </summary>
        public string yybh { get; set; }
        /// <summary>
        /// 单位编号
        /// </summary>
        public string dwbh { get; set; }
        /// <summary>
        /// 体检状态
        /// </summary>
        public string tjzt { get; set; }
    }
}
