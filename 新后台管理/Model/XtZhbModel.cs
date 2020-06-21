using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 账号
    /// </summary>
    public class XtZhbModel
    {
        public int id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string zh { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string mm { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string yybh { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string dh { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string lxr { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string ys { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public int jsid { get; set; }
       /// <summary>
        /// 渠道Id
        /// </summary>
        public int QdId { get; set; }
        /// <summary>
        /// 1 代表医院，2渠道
        /// </summary>
        public int Lx { get; set; } = 1;
        /// <summary>
        /// 角色名称
        /// </summary>
        public string jsmc { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int zt { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string ztmc { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string bz { get; set; }
        public int RowNum { get; set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        public string txlj { get; set; }

    }
}
