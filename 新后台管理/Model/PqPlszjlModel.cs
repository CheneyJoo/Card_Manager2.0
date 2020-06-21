using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    /// <summary>
	/// 批量设置记录
	/// </summary>
    public class PqPlszjlModel
    {
        #region Model
        private int _id;
        private string _yybh = "";
        private string _qybh ="";
        private DateTime _kssj = DateTime.Now;
        private DateTime _jssj = DateTime.Now;
        private DateTime _cjsj = DateTime.Now;
        private string _mx = "[]";
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
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
        /// 企业编号
        /// </summary>
        public string qybh
        {
            set { _qybh = value; }
            get { return _qybh; }
        }
        /// <summary>
        /// 开始时间
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
        /// 创建时间
        /// </summary>
        public DateTime cjsj
        {
            set { _cjsj = value; }
            get { return _cjsj; }
        }
        /// <summary>
        /// 明细
        /// </summary>
        public string mx
        {
            set { _mx = value; }
            get { return _mx; }
        }
        #endregion Model

        private List<PlszMxModel> _mxlist = new List<PlszMxModel>();
        public List<PlszMxModel> mxList
        {
            set { _mxlist = value; }
            get { return _mxlist; }
        }

    }


    public class PlszMxModel
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string kssj { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string jssj { get; set; }
        /// <summary>
        /// 预留人数
        /// </summary>
        public int tjrs { get; set; }
    }
}
