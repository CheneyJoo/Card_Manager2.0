using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class XtYhbModel
    {
        #region Model
        private string _zh = "";
        private string _xm = "";
        private string _dh = "";
        private string _mm = "";
        private DateTime _cjsj = DateTime.Now;
        private string _yybh;
        /// <summary>
        /// 
        /// </summary>
        public string zh
        {
            set { _zh = value; }
            get { return _zh; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string xm
        {
            set { _xm = value; }
            get { return _xm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string dh
        {
            set { _dh = value; }
            get { return _dh; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mm
        {
            set { _mm = value; }
            get { return _mm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime cjsj
        {
            set { _cjsj = value; }
            get { return _cjsj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string yybh
        {
            set { _yybh = value; }
            get { return _yybh; }
        }
        #endregion Model

    }
}
