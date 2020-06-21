using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Wjgzmx
    {
        /// <summary>
        /// 主键，自增Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 规则Id
        /// </summary>
        public int GZID { get; set; }
        /// <summary>
        /// 标签Id
        /// </summary>
        public int BQID { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string BQMC { get; set; }
    }
}