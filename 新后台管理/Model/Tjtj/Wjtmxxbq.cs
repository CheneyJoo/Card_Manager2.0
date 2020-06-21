using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Wjtmxxbq
    {
        /// <summary>
        /// 主键，自增Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 选项Id
        /// </summary>
        public int XXID { get; set; }
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