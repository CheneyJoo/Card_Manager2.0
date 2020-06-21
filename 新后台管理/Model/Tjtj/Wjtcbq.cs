using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Wjtcbq
    {
        /// <summary>
        /// 主键,自增Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 问卷套餐的关联ID
        /// </summary>
        public int GLID { get; set; }
        /// <summary>
        /// 标签Id
        /// </summary>
        public int BQID { get; set; }
        /// <summary>
        /// 检查深度 1.基础检查 2.中等检查 3.深度检查
        /// </summary>
        public int JCSD { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string BQMC { get; set; }
    }
}