using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Wjjg
    {
        /// <summary>
        /// 主键，自增Id
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string YYBH { get; set; }
        /// <summary>
        /// 问卷状态 0.停用 1.启用
        /// </summary>
        public int WJZT { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CJR { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CJSJ { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string JGMC { get; set; }
    }
}