using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 排期体检时间
    /// </summary>
    public class PqTjsjModel
    {
        #region Model
        private int _id;
        private string _pqbh = "";
        private string _yybh = "";
        private string _qybh = "";
        private DateTime _rq = DateTime.Now;
        private DateTime _kssj = DateTime.Now;
        private DateTime _jssj = DateTime.Now;
        private int _tjrs = 0;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string pqbh
        {
            set { _pqbh = value; }
            get { return _pqbh; }
        }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string yybh
        {
            set { _yybh = value; }
            get { return _yybh; }
        }
        /// <summary>
        /// 企业id
        /// </summary>
        public string qybh
        {
            set { _qybh = value; }
            get { return _qybh; }
        }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime rq
        {
            set { _rq = value; }
            get { return _rq; }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime kssj
        {
            set { _kssj = value; }
            get { return _kssj; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime jssj
        {
            set { _jssj = value; }
            get { return _jssj; }
        }
        /// <summary>
        /// 体检人数
        /// </summary>
        public int tjrs
        {
            set { _tjrs = value; }
            get { return _tjrs; }
        }
        #endregion Model
    }
}
