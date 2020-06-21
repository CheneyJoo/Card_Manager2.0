using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Wjtc
    {
        /// <summary>
        /// 主键，自增Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 套餐Id
        /// </summary>
        public string TCID { get; set; }
        /// <summary>
        /// 适用人群
        /// </summary>
        public string SYRQ { get; set; }
        /// <summary>
        /// 套餐作用
        /// </summary>
        public string TCZY { get; set; }
        /// <summary>
        /// 是否启用 0.停用 1.启用
        /// </summary>
        public int SFQY { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string YYBH { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal JG { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string TCMC { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// 标签ID列表，逗号隔开
        /// </summary>
        public string BQIDS { get; set; }
        /// <summary>
        /// 标签深度列表，逗号隔开
        /// </summary>
        public string BQSDS { get; set; }
        /// <summary>
        /// 套餐标签列表
        /// </summary>
        public List<Wjtcbq> ListBq { get; set; }
    }
}