using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 订单基本信息
    /// </summary>
    public class DdjbxxModel
    {
        #region Model
        private string _ddbh = "";
        private string _dsfdd = "";
        private int _dsfbzid = 0;
        private int _ddzt = 0;
        private string _tcid = "";
        private string _tcmc = "";
        private string _dwbh = "";
        private decimal _tcjg = 0M;
        private decimal _jxbjg = 0M;
        private decimal _ddze = 0M;
        private DateTime _intime = DateTime.Now;
        private int _sfout = 0;
        private DateTime _outtime = Convert.ToDateTime("1970-01-01");
        private string _dh = "";
        private string _xm = "";
        private int _xb = -1;
        private int _hz = -1;
        private int _zjlx = 0;
        private string _zjhm = "";
        private DateTime _yykssj = DateTime.Now;
        private DateTime _yyjssj = DateTime.Now;
        private int _sfdj = 0;
        private DateTime _djtime = Convert.ToDateTime("1970-01-01");
        private int _sfbg = 0;
        private DateTime _bgtime = Convert.ToDateTime("1970-01-01");
        private string _djlsh = "";
        private int _sfjx = 0;
        private string _jxlist = "";
        private int _sfjs = 0;
        private string _csrq = "";
        private int _nl = 0;
        private string _remark = "";
        private string _yybh = "";
        private int _ddly = 0;
        private string _dwmc = "";
        private int _jsbz = 0;
        private string _zffs = "";
        private string _trade_no = "";
        private string _ygzh = "";
        private int _jxbzt = 0;
        /// <summary>
        /// 订单编号
        /// </summary>
        public string ddbh
        {
            set { _ddbh = value; }
            get { return _ddbh; }
        }
        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string dsfdd
        {
            set { _dsfdd = value; }
            get { return _dsfdd; }
        }
        /// <summary>
        /// 第三方标识 1,康康网,2中康,3袋鼠,4,每天
        /// </summary>
        public int dsfbzid
        {
            set { _dsfbzid = value; }
            get { return _dsfbzid; }
        }
        /// <summary>
        /// 订单状态 2,预约中,3,预约成功,6,已到检,7,已完成,9已出报告,4,退款中,5,退款完成,8已取消
        /// </summary>
        public int ddzt
        {
            set { _ddzt = value; }
            get { return _ddzt; }
        }
        /// <summary>
        /// 医院套餐编号
        /// </summary>
        public string tcid
        {
            set { _tcid = value; }
            get { return _tcid; }
        }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string tcmc
        {
            set { _tcmc = value; }
            get { return _tcmc; }
        }
        /// <summary>
        /// 在体检软件中的单位编号
        /// </summary>
        public string dwbh
        {
            set { _dwbh = value; }
            get { return _dwbh; }
        }
        /// <summary>
        /// 套餐价格
        /// </summary>
        public decimal tcjg
        {
            set { _tcjg = value; }
            get { return _tcjg; }
        }
        /// <summary>
        /// 加项包价格
        /// </summary>
        public decimal jxbjg
        {
            set { _jxbjg = value; }
            get { return _jxbjg; }
        }
        /// <summary>
        ///  订单总金额
        /// </summary>
        public decimal ddze
        {
            set { _ddze = value; }
            get { return _ddze; }
        }
        /// <summary>
        /// 进入中间平台时间
        /// </summary>
        public DateTime intime
        {
            set { _intime = value; }
            get { return _intime; }
        }
        /// <summary>
        /// 是否送达医院
        /// </summary>
        public int sfout
        {
            set { _sfout = value; }
            get { return _sfout; }
        }
        /// <summary>
        /// 送达医院时间
        /// </summary>
        public DateTime outtime
        {
            set { _outtime = value; }
            get { return _outtime; }
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string dh
        {
            set { _dh = value; }
            get { return _dh; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string xm
        {
            set { _xm = value; }
            get { return _xm; }
        }
        /// <summary>
        /// 性别1男0女
        /// </summary>
        public int xb
        {
            set { _xb = value; }
            get { return _xb; }
        }
        /// <summary>
        /// 婚姻0:未婚，1：已婚
        /// </summary>
        public int hz
        {
            set { _hz = value; }
            get { return _hz; }
        }
        /// <summary>
        /// 证件类型 1,身份证,2其他 
        /// </summary>
        public int zjlx
        {
            set { _zjlx = value; }
            get { return _zjlx; }
        }
        /// <summary>
        /// 证件号
        /// </summary>
        public string zjhm
        {
            set { _zjhm = value; }
            get { return _zjhm; }
        }
        /// <summary>
        /// 预约开始时间
        /// </summary>
        public DateTime yykssj
        {
            set { _yykssj = value; }
            get { return _yykssj; }
        }
        /// <summary>
        /// 预约结束时间
        /// </summary>
        public DateTime yyjssj
        {
            set { _yyjssj = value; }
            get { return _yyjssj; }
        }
        /// <summary>
        /// 是否到检
        /// </summary>
        public int sfdj
        {
            set { _sfdj = value; }
            get { return _sfdj; }
        }
        /// <summary>
        /// 到检时间
        /// </summary>
        public DateTime djtime
        {
            set { _djtime = value; }
            get { return _djtime; }
        }
        /// <summary>
        /// 是否出报告
        /// </summary>
        public int sfbg
        {
            set { _sfbg = value; }
            get { return _sfbg; }
        }
        /// <summary>
        /// 报告时间
        /// </summary>
        public DateTime bgtime
        {
            set { _bgtime = value; }
            get { return _bgtime; }
        }
        /// <summary>
        /// 医院订单号,登记流水号
        /// </summary>
        public string djlsh
        {
            set { _djlsh = value; }
            get { return _djlsh; }
        }
        /// <summary>
        /// 是否加项
        /// </summary>
        public int sfjx
        {
            set { _sfjx = value; }
            get { return _sfjx; }
        }
        /// <summary>
        /// 加项列表,用逗号隔开
        /// </summary>
        public string jxlist
        {
            set { _jxlist = value; }
            get { return _jxlist; }
        }
        /// <summary>
        /// 是否结算
        /// </summary>
        public int sfjs
        {
            set { _sfjs = value; }
            get { return _sfjs; }
        }
        /// <summary>
        /// 出身日期
        /// </summary>
        public string csrq
        {
            set { _csrq = value; }
            get { return _csrq; }
        }
        /// <summary>
        /// 年龄
        /// </summary>
        public int nl
        {
            set { _nl = value; }
            get { return _nl; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 机构编号
        /// </summary>
        public string yybh
        {
            set { _yybh = value; }
            get { return _yybh; }
        }
        /// <summary>
        /// 订单来源,0个检,1团检
        /// </summary>
        public int ddly
        {
            set { _ddly = value; }
            get { return _ddly; }
        }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string dwmc
        {
            set { _dwmc = value; }
            get { return _dwmc; }
        }
        /// <summary>
        /// 家属标识1家属,0自己
        /// </summary>
        public int jsbz
        {
            set { _jsbz = value; }
            get { return _jsbz; }
        }
        /// <summary>
        /// 支付方式(weixin，alipay)
        /// </summary>
        public string zffs
        {
            set { _zffs = value; }
            get { return _zffs; }
        }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string trade_no
        {
            set { _trade_no = value; }
            get { return _trade_no; }
        }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string ygzh
        {
            set { _ygzh = value; }
            get { return _ygzh; }
        }

        /// <summary>
        /// 加项包状态 0默认 1退款中 2已退款
        /// </summary>
        public int jxbzt
        {
            set { _jxbzt = value; }
            get { return _jxbzt; }
        }

        #endregion Model

        /// <summary>
        /// 订单状态描述 2,预约中,3,预约成功,6,已到检,7,已完成,9已出报告,4,退款中,5,退款完成,8已取消
        /// </summary>
        public string ddztms
        {
            get
            { 
                if(ddzt==1)
                {
                    return "待付款";
                }
                else if (ddzt == 2)
                {
                    return "预约中";
                }
                else if (ddzt == 3)
                {
                    return "预约成功";
                }
                else if (ddzt == 4)
                {
                    return "退款中";
                }
                else if (ddzt == 5)
                {
                    return "退款完成";
                }
                else if (ddzt == 6)
                {
                    return "已到检";
                }
                else if (ddzt == 7)
                {
                    return "已完成";
                }
                else if (ddzt == 8)
                {
                    return "已取消";
                }
                else
                {
                    return "已出报告";
                }
            }

        }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string qdmc
        {

            get;set;
        }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string jgmc { get; set; }
        public string account { get; set; }

        public string jgjkurl { get; set; }
        public string pwd { get; set; }
        public int iskkservice { get; set; }
        /// <summary>
        /// 组合项目
        /// </summary>
        public List<DdZhxmModel> zhxmlist { get; set; }

        /// <summary>
        /// 退款表，退款金额
        /// </summary>
        public decimal tkje { get; set; }
        /// <summary>
        /// 订单表，退款金额
        /// </summary>
        public decimal tkze { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string tkyy { get; set; }

        /// <summary>
        /// 退款状态1,退款中, 2退款成功,3驳回,0作废
        /// </summary>
        public int tkzt { get; set; }

        /// <summary>
        /// 原始结算价
        /// </summary>
        public decimal ysjsj { get; set; }
        /// <summary>
        /// 结算调整
        /// </summary>
        public decimal jstz { get; set; }
        /// <summary>
        /// 调整原因
        /// </summary>
        public string tzyy { get; set; }
        /// <summary>
        /// 实际结算价
        /// </summary>
        public decimal sjjsj { get; set; }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string jjyy { get; set; }
        /// <summary>
        /// 退款拒绝原因
        /// </summary>

        public string tkjjyy { get; set; }
        /// <summary>
        /// 自动标识
        /// </summary>
        public int zdbz { get; set; }

    }

}
