using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Wjgz
    {
        /// <summary>
        /// 主键，自增Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string GZMC { get; set; }
        /// <summary>
        /// 最小年龄
        /// </summary>
        public int ZXNL { get; set; }
        /// <summary>
        /// 最大年龄
        /// </summary>
        public int ZDNL { get; set; }
        /// <summary>
        /// 是否启用 -1.删除 0.停用 1.启用
        /// </summary>
        public int SFQY { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// 选择的标签列表
        /// </summary>
        public List<Wjgzmx> ListMx { get; set; }
        /// <summary>
        /// 选择的标签,逗号隔开
        /// </summary>
        public string XZBQ { get; set; }
        /// <summary>
        /// 标签名称，逗号隔开
        /// </summary>
        public string BQXQ { get; set; }
        /// <summary>
        /// 性别 1.男 2.女
        /// </summary>
        public int XB { get; set; }
    }
}