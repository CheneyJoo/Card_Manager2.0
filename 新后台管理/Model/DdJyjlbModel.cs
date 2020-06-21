using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DdJyjlbModel
    {
        #region Model
        private int _id;
        private string _ddbh = "";
        private string _jyjlbh = "";
        private string _zffs = "";
        private decimal _jyje;
        private bool _sfzfcg = false;
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
        /// 交易记录编号
        /// </summary>
        public string jyjlbh
        {
            set { _jyjlbh = value; }
            get { return _jyjlbh; }
        }
        /// <summary>
        /// 支付方式  alipay weixin
        /// </summary>
        public string zffs
        {
            set { _zffs = value; }
            get { return _zffs; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal jyje
        {
            set { _jyje = value; }
            get { return _jyje; }
        }
        /// <summary>
        /// 是否支付成功
        /// </summary>
        public bool sfzfcg
        {
            set { _sfzfcg = value; }
            get { return _sfzfcg; }
        }
        #endregion Model


    }
}
