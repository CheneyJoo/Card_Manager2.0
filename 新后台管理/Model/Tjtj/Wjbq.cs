using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Wjbq
    {
        /// <summary>
        /// 主键，自增Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string BQMC { get; set; }
        /// <summary>
        /// 标签详情
        /// </summary>
        public string BQXQ { get; set; }
        /// <summary>
        /// 状态 -1.删除 0.停用  1.启用 
        /// </summary>
        public int SFQY { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum { get; set; }
    }
}