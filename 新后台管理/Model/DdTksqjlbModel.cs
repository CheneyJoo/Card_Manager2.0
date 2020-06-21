using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DdTksqjlbModel
    {
        #region Model
        private int _id;
        private string _ddbh = "";
        private string _jylsbh = "";
        private decimal _tkje = 0M;
        private DateTime _sqtksj = DateTime.Now;
        private DateTime _tksj = DateTime.Now;
        private string _zffs = "";
        private string _tklsh = "";
        private string _trade_no = "";
        private string _refund_no = "";
        private string _tkyy = "";
        private int _tkzt = 0;
        private string _yybh = "";
        private string _dwbh = "";
        private decimal _ddje = 0M;
        /// <summary>
        /// 
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
        /// 交易流水号
        /// </summary>
        public string jylsbh
        {
            set { _jylsbh = value; }
            get { return _jylsbh; }
        }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal tkje
        {
            set { _tkje = value; }
            get { return _tkje; }
        }
        /// <summary>
        /// 申请退款时间
        /// </summary>
        public DateTime sqtksj
        {
            set { _sqtksj = value; }
            get { return _sqtksj; }
        }
        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime tksj
        {
            set { _tksj = value; }
            get { return _tksj; }
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
        /// 退款流水号
        /// </summary>
        public string tklsh
        {
            set { _tklsh = value; }
            get { return _tklsh; }
        }
        /// <summary>
        /// 支付交易号
        /// </summary>
        public string trade_no
        {
            set { _trade_no = value; }
            get { return _trade_no; }
        }
        /// <summary>
        /// 退款交易号
        /// </summary>
        public string refund_no
        {
            set { _refund_no = value; }
            get { return _refund_no; }
        }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string tkyy
        {
            set { _tkyy = value; }
            get { return _tkyy; }
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
        /// 医院编号
        /// </summary>
        public string yybh
        {
            set { _yybh = value; }
            get { return _yybh; }
        }
        /// <summary>
        /// 单位编号
        /// </summary>
        public string dwbh
        {
            set { _dwbh = value; }
            get { return _dwbh; }
        }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal ddje
        {
            set { _ddje = value; }
            get { return _ddje; }
        }
        #endregion Model
    }
}
