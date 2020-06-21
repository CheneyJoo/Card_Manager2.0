using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TjFzryModel
    {
        #region Model
        private int _id;
        private int _fzid;
        private int _qyygid;
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
        public int fzid
        {
            set { _fzid = value; }
            get { return _fzid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int qyygid
        {
            set { _qyygid = value; }
            get { return _qyygid; }
        }
        #endregion Model
    }
}
