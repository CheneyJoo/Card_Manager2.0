using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 压力科室组合项目报表
    /// </summary>
   public class ReportYaLi
    {
        public string yysj { get; set; }
        public string zhxmksbh { get; set; }
        public string zhxmksmc { get; set; }
        public string zhxmmc { get; set; }
        public string zhxmbh { get; set; }
        /// <summary>
        /// 上限人次
        /// </summary>
        public int sxrs { get; set; }
        public int total { get; set; }
    }
}
