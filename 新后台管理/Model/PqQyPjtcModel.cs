using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PqQyPjtcModel
    {
        #region Model
        private int _id;
        private string _pqbh = "";
        private string _tcbh;
        private int _tjrs;
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
        /// 套餐编号
        /// </summary>
        public string tcbh
        {
            set { _tcbh = value; }
            get { return _tcbh; }
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
