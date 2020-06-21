using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace Model
{
    public class Wjtm
    {
        /// <summary>
        /// 主键，自增Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 问卷Id
        /// </summary>
        public int WJID { get; set; }
        /// <summary>
        /// 题目内容
        /// </summary>
        public string TMNR { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int PX { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string XGR { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public System.DateTime XGSJ { get; set; }
        /// <summary>
        /// 题目类型 1.单选 2.多选
        /// </summary>
        public int TMLX { get; set; }
        /// <summary>
        /// 问卷-题目选项列表
        /// </summary>
        public List<Wjtmxx> ListTMXX { get; set; }

    }
}