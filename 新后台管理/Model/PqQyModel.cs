using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PqQyModel
    {
        public PqQyModel()
        { }
        #region Model
        private string _pqbh;
        private string _yybh = "";
        private int _qdid = 0;
        private DateTime _ksrq = DateTime.Now;
        private DateTime _jsrq = DateTime.Now;
        private DateTime _cjsj = DateTime.Now;
        private string _jzsj = "";
        private string _xxr = "";
        private int _tqts = 0;
        private int _tjrs = 0;
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
        /// 渠道编号
        /// </summary>
        public int qdid
        {
            set { _qdid = value; }
            get { return _qdid; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime ksrq
        {
            set { _ksrq = value; }
            get { return _ksrq; }
        }
        /// <summary>
        /// 结束时间
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
        /// 提前天数
        /// </summary>
        public int tqts
        {
            set { _tqts = value; }
            get { return _tqts; }
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
        public string qdmc { get; set; }
        public string yymc { get; set; }
    }
}
