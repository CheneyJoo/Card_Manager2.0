using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PqQyRqModel
    {
        public PqQyRqModel()
        { }
        #region Model
        private int _id;
        private string _pqbh = "";
        private string _yybh = "";      
        private DateTime _rq = DateTime.Now;
        private int _tjrs = 0;
        private int _flag = 0;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 排期编号
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
       public string qybh { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime rq
        {
            set { _rq = value; }
            get { return _rq; }
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
        /// 状态
        /// </summary>
        public int flag
        {
            set { _flag = value; }
            get { return _flag; }
        }
        #endregion Model
    }
}
