using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class XtMenuModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string mc { get; set; }
        /// <summary>
        /// 上层
        /// </summary>
        public int scid { get; set; }
        /// <summary>
        /// 类型 1医院端,2渠道端
        /// </summary>
        public int lx { get; set; }

        public string url { get; set; }

        public string icon { get; set; }

        public decimal sort { get; set; }
        /// <summary>
        /// 是否为团检菜单，1是，0不是
        /// </summary>
        public int sftj { get; set; }


    }

}
