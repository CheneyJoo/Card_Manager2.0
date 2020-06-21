using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DdTkjlbModel
    {
        #region Model
        private int _id;
        private string _tklsh = "";
        private string _jylsh = "";
        private string _zffs = "";
        private string _ddbh = "";
        private int _tkzt = 0;
        private int _sqtkid = 0;
        private string _yybh = "";
        private DateTime _tjsj = DateTime.Now;
        private string _refund_no = "";
        private DateTime _tksj = Convert.ToDateTime("1970-01-01");
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 退款流水号
        /// </summary>
        public string tklsh
        {
            set { _tklsh = value; }
            get { return _tklsh; }
        }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string jylsh
        {
            set { _jylsh = value; }
            get { return _jylsh; }
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string zffs
        {
            set { _zffs = value; }
            get { return _zffs; }
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
        /// 退款状态
        /// </summary>
        public int tkzt
        {
            set { _tkzt = value; }
            get { return _tkzt; }
        }
        /// <summary>
        /// 退款申请表id
        /// </summary>
        public int sqtkid
        {
            set { _sqtkid = value; }
            get { return _sqtkid; }
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
        /// 添加时间
        /// </summary>
        public DateTime tjsj
        {
            set { _tjsj = value; }
            get { return _tjsj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string refund_no
        {
            set { _refund_no = value; }
            get { return _refund_no; }
        }


        public DateTime tksj
        {
            set { _tksj = value; }
            get { return _tksj; }
        }
        #endregion Model
    }
}
