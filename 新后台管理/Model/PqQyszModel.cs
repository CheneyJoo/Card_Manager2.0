using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    /// <summary>
    /// 排期-企业基本设置
    /// </summary>
    public class PqQyszModel
    {
        public PqQyszModel()
        {

        }

        #region Model
        //private int _id;
        private string _pqbh = "";
        private string _qybh = "";
        private string _yybh = "";
        private int _tqts = 0;
        private string _jzsj = "";
        private string _xxr = "";
        private DateTime _ksrq = Convert.ToDateTime("1970-01-01");
        private DateTime _jsrq = Convert.ToDateTime("1970-01-01");
        private DateTime _cjsj = Convert.ToDateTime("1970-01-01");
        private string _sjd = "";
        private int _tjrs = 0;
        private int _tjrs_dy = 0;
        ///// <summary>
        ///// 
        ///// </summary>
        //public int id
        //{
        //    set { _id = value; }
        //    get { return _id; }
        //}
        /// <summary>
        /// 排期编号
        /// </summary>
        public string pqbh
        {
            set { _pqbh = value; }
            get { return _pqbh; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string qybh
        {
            set { _qybh = value; }
            get { return _qybh; }
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
        /// 提前天数
        /// </summary>
        public int tqts
        {
            set { _tqts = value; }
            get { return _tqts; }
        }
        /// <summary>
        /// 预约截止时间
        /// </summary>
        public string jzsj
        {
            set { _jzsj = value; }
            get { return _jzsj; }
        }
        /// <summary>
        /// 休息日
        /// </summary>
        public string xxr
        {
            set { _xxr = value; }
            get { return _xxr; }
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime ksrq
        {
            set { _ksrq = value; }
            get { return _ksrq; }
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime jsrq
        {
            set { _jsrq = value; }
            get { return _jsrq; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime cjsj
        {
            set { _cjsj = value; }
            get { return _cjsj; }
        }
        /// <summary>
        /// 时间段
        /// </summary>
        public string sjd
        {
            set { _sjd = value; }
            get { return _sjd; }
        }
        /// <summary>
        /// 体检人数
        /// </summary>
        public int tjrs
        {
            set { _tjrs = value; }
            get { return _tjrs; }
        }
        /// <summary>
        /// 体检人数_到院
        /// </summary>
        public int tjrs_dy
        {
            set { _tjrs_dy = value; }
            get { return _tjrs_dy; }
        }
        #endregion Model

        /// <summary>
        /// 是否为渠道，1是,0否
        /// </summary>
        public int sfqd { get; set; }
        /// <summary>
        /// 时间段
        /// </summary>
        public List<Sjd> SjdList = new List<Sjd>();
    }

    public class Sjd
    {
        public string kssj { get; set; }

        public string jssj { get; set; }

        public int tjrs { get; set; }

        public int tjrs_dy { get; set; }
    }
}
