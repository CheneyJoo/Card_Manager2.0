using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Wjb
    {
        /// <summary>
        /// 主键，自增Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 问卷名称
        /// </summary>
        public string WJMC { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string YYBH { get; set; }
        /// <summary>
        /// 是否启用 0.停用 1.启用
        /// </summary>
        public int SFQY { get; set; }
    }
}