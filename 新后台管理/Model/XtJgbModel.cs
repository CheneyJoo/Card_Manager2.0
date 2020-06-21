using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 机构
    /// </summary>
    public class XtJgbModel
    {
        public int id { get; set; }
        public string jgmc { get; set; }
        /// <summary>
        /// 机构地址
        /// </summary>
        public string jgdz { get; set; }
        public int sfqy { get; set; }
        public DateTime createtime { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string lxdh { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string lxr { get; set; }
        /// <summary>
        /// 开放给第三方的标识用逗号隔开
        /// </summary>
        public string dsfbz { get; set; }
        /// <summary>
        /// 自动标识,1全自动接口落单,2半自动接口落单,3手动导出落单
        /// </summary>
        public int zdbz { get; set; }
        public string jgjkurl { get; set; }
        public string account { get; set; }
        public string pwd { get; set; }
        /// <summary>
        /// 是否为康康提供的标准接口,1是,0否,如果不是要在前端订制代码类对接 
        /// </summary>
        public int iskkservice { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string yybh { get; set; }
        /// <summary>
        /// 医院介绍
        /// </summary>
        public string yyjs { get; set; }
        /// <summary>
        /// 医院logog
        /// </summary>
        public string yylogoimage { get; set; }
        /// <summary>
        /// 医院图片
        /// </summary>
        public string yyimage { get; set; }
        /// <summary>
        /// 医院等级
        /// </summary>
        public string yydj { get; set; }

        public string CSBH { get; set; }
        public string dj { get; set; }
        public string bw { get; set; }
        public int zyfs { get; set; }
        public int jgxz { get; set; }
        public int SFBH { get; set; }
    }
}
