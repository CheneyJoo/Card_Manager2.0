using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DdZhxmModel
    {
        #region Model
        private int _id;
        private string _ddbh="";
        private string _zhxmbh = "";
        private string _zhxmmc = "";
        private decimal _jg = 0M;
        private int _sfjx = 0;
        private int _sfdj = 0;
        /// <summary>
        /// id
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string ddbh
        {
            set { _ddbh = value; }
            get { return _ddbh; }
        }
        /// <summary>
        /// 组合项目编号
        /// </summary>
        public string zhxmbh
        {
            set { _zhxmbh = value; }
            get { return _zhxmbh; }
        }
        /// <summary>
        /// 组合项目名称
        /// </summary>
        public string zhxmmc
        {
            set { _zhxmmc = value; }
            get { return _zhxmmc; }
        }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal jg
        {
            set { _jg = value; }
            get { return _jg; }
        }
        /// <summary>
        /// 是否加项
        /// </summary>
        public int sfjx
        {
            set { _sfjx = value; }
            get { return _sfjx; }
        }
        /// <summary>
        /// 是否到检
        /// </summary>
        public int sfdj
        {
            set { _sfdj = value; }
            get { return _sfdj; }
        }
        public string zhxmksbh { get; set; }
        public string zhxmksmc { get; set; }
        #endregion Model
    }
}
