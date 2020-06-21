using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class JsModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Jsmc { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Bz { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string Yybh { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime Cjrq { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Cjr { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime Gxrq { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string Gxr { get; set; }
        /// <summary>
        /// 1,医院，2渠道
        /// </summary>
        public int Lx { get; set; } = 1;
        /// <summary>
        /// 渠道Id
        /// </summary>
        public int QdId { get; set; } = 0;
        /// <summary>
        /// 序号
        /// </summary>
        public int RowNum { get; set; }

    }
    /// <summary>
    /// 角色权限
    /// </summary>
    public class JsqxModel
    {
        public int MenuId { get; set; }
        public int JsId { get; set; }
        public int Id { get; set; }
        public string Yybh { get; set; }
    }
}
