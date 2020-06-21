using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    /// <summary>
    /// 排期-基本设置
    /// </summary>
    public class PqJbszModel
    {
        #region Model
        private int _id;
        private string _tjjgid = "";
        private int _zdjd = 0;
        private int _skyl = 0;
        private int _qtyl = 0;
        private int _tjyl = 0;
        private string _xxr = "";
        private string _tsky = "";
        private string _ztyy = "";
        private int _qdyl = 0;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 体检机构编号
        /// </summary>
        public string tjjgid
        {
            set { _tjjgid = value; }
            get { return _tjjgid; }
        }
        /// <summary>
        /// 最大接待量
        /// </summary>
        public int zdjd
        {
            set { _zdjd = value; }
            get { return _zdjd; }
        }


        /// <summary>
        /// 散客预留量
        /// </summary>
        public int skyl
        {
            set { _skyl = value; }
            get { return _skyl; }
        }
        /// <summary>
        /// 其他预留量
        /// </summary>
        public int qtyl
        {
            set { _qtyl = value; }
            get { return _qtyl; }
        }
        /// <summary>
        /// 团检预留量
        /// </summary>
        public int tjyl
        {
            set { _tjyl = value; }
            get { return _tjyl; }
        }
        /// <summary>
        /// 休息日
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string xxr
        {
            set { _xxr = value; }
            get { return _xxr; }
        }
        /// <summary>
        /// 特殊可约日期
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string tsky
        {
            set { _tsky = value; }
            get { return _tsky; }
        }
        /// <summary>
        /// 暂停预约日期
        /// </summary>
        //[System.ComponentModel.DefaultValue("")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ztyy
        {
            set { _ztyy = value; }
            get { return _ztyy; }
        }

        /// <summary>
        /// 渠道预留量
        /// </summary>
        public int qdyl
        {
            set { _qdyl = value; }
            get { return _qdyl; }
        }
        #endregion Model

    }
}
