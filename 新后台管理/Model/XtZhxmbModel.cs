using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 组合项目
    /// </summary>
    public class XtZhxmbModel
    {
        public int id { get; set; }
        public string yybh { get; set; }
        public string zhxmbh { get; set; }
        public string zhxmmc { get; set; }
        public string zhxmms { get; set; }
        public decimal zhxmjg { get; set; }
        /// <summary>
        /// 性别 0女,1男,2通用
        /// </summary>
        public int xb { get; set; }
        /// <summary>
        /// 是否妇科
        /// </summary>
        public int sffk { get; set; }
        public int sfqy { get; set; }
        public string zhxmksbh { get; set; }
        public string zhxmksmc { get; set; }
        public DateTime createtime { get; set; }
        public DateTime updatetime { get; set; }

        private int _sxrs = 0;
        public int sxrs
        {
            get { return _sxrs; }
            set { _sxrs = value; }
        }
        public string jgmc { get; set; }
    }


    /// <summary>
    /// 科室
    /// </summary>
    public class XtZhxmKs
    {
        public string yybh { get; set; }
        public string ksbh { get; set; }

        public string ksmc { get; set; }
    }
   
}
