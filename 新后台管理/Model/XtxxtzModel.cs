using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 系统信息通知
    /// </summary>
    public class XtxxtzModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public int Sfyd { get; set; }
        /// <summary>
        /// 渠道Id
        /// </summary>
        public int Qdid { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string Yybh { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Lr { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Bt { get; set; }
        /// <summary>
        /// 1医院，2渠道
        /// </summary>
        public int Lx { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cjsj { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime Xgsj { get; set; }
        /// <summary>
        /// 时间间隔
        /// </summary>
        public string TimeSpan
        {
            get
            {
                TimeSpan ts = DateTime.Now - Cjsj;
                if(ts.Days > 1)
                {
                    return ts.Days.ToString() + "天前";
                 
                }
                else if(ts.Hours>0)
                {
                    return ts.Hours + "小时前";
                }
             
                else
                {
                    return (ts.Minutes+1).ToString() + "分前";
                }
            }
        }
    }
}
