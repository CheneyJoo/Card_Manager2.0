using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TjFzModel
    {
        #region Model
        private int _id;
        private string _mc = "";
        private string _yybh = "";
        private bool _sfqy = true;
        private int _jsmds = 0;
        private DateTime _tjkssj = DateTime.Now;
        private DateTime _tjjssj = DateTime.Now;
        private DateTime _cjsj = DateTime.Now;
        private string _jstc="";
        private string _dwbh = "";
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string mc
        {
            set { _mc = value; }
            get { return _mc; }
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
        /// 是否启用
        /// </summary>
        public bool sfqy
        {
            set { _sfqy = value; }
            get { return _sfqy; }
        }
        /// <summary>
        /// 家属免单数
        /// </summary>
        public int jsmds
        {
            set { _jsmds = value; }
            get { return _jsmds; }
        }
        /// <summary>
        /// 体检开始时间
        /// </summary>
        public DateTime tjkssj
        {
            set { _tjkssj = value; }
            get { return _tjkssj; }
        }
        /// <summary>
        /// 体检结束时间
        /// </summary>
        public DateTime tjjssj
        {
            set { _tjjssj = value; }
            get { return _tjjssj; }
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
        /// 家属套餐
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string jstc
        {
            set { _jstc = value; }
            get { return _jstc; }
        }

        public string dwbh
        {
            set { _dwbh = value; }
            get { return _dwbh; }
        }

        #endregion Model

        /// <summary>
        /// 预登记号
        /// </summary>
        public string ydjh { get; set; }
    }
}
