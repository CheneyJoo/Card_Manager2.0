using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 企业员工信息
    /// </summary>
    public class QyygxxModel
    {
        public int id { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string yybh { get; set; }
        /// <summary>
        /// 套餐编号
        /// </summary>
        public string tcbh { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string tcmc { get; set; }
        /// <summary>
        /// 单位编号
        /// </summary>
        public string dwbh { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string dwmc { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal jg { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string sfzh { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string xm { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime createtime { get; set; }

        /// <summary>
        /// 性别1男,0女
        /// </summary>
        public int xb { get; set; }
        /// <summary>
        /// 0未婚,1已婚,2不限
        /// </summary>
        public int hz { get; set; }
        /// <summary>
        /// 电话
        /// </summary>

        public string dh { get; set; }
        /// <summary>
        /// 单位固定金额
        /// </summary>
        public decimal dwgdje { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>

        public string csrq { get; set; }
        /// <summary>
        /// 是否已经预约
        /// </summary>
        public int sfyy { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string mm { get; set; }
        /// <summary>
        /// 体检软件预登记号(相当于订单号)
        /// </summary>
        public string ydjh { get; set; }

        /// <summary>
        /// 员工账号
        /// </summary>
        public string ygzh { get; set; }
        /// <summary>
        /// 软件人员编号，类似于工号
        /// </summary>
        public string rybh { get; set; }
        /// <summary>
        /// 是否vip
        /// </summary>
        public int sfvip { get; set; }
        /// <summary>
        /// 预约开始时间
        /// </summary>
        public DateTime yykssj { get; set; }
        /// <summary>
        /// 预约结束时间
        /// </summary>
        public DateTime yyjssj { get; set; }

    }

}
