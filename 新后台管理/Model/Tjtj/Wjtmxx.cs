using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Wjtmxx
    {
        /// <summary>
        /// 主键，自增Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 题目Id
        /// </summary>
        public int TMID { get; set; }
        /// <summary>
        /// 选项内容
        /// </summary>
        public string XXNR { get; set; }
        /// <summary>
        /// 问卷-题目选项标签
        /// </summary>
        public List<Wjtmxxbq> ListTMXXBQ { get; set; }
        /// <summary>
        /// 选项标签列表
        /// </summary>
        public string XXBQS { get; set; }
    }
}