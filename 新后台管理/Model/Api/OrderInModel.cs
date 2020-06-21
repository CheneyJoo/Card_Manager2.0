using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Api
{
    /// <summary>
    /// 订单模型
    /// </summary>
    public class PreOrder : UserRequest
    {

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string customerName { get; set; }
        /// <summary>
        /// 证件类型1身份证，2其他
        /// </summary>
        public int customerIDType { get; set; }
        /// <summary>
        /// 客户证件号码
        /// </summary>
        public string customerIDCard { get; set; }
        /// <summary>
        /// 客户性别：1：男，0：女
        /// </summary>
        public int customerGender { get; set; }
        /// <summary>
        /// 出生日期yyyy-MM-dd
        /// </summary>
        public string customerBirthday { get; set; }
        /// <summary>
        /// 客户年龄
        /// </summary>
        public int customerAge { get; set; }
        /// <summary>
        /// 婚姻状况,0:已婚，1：未婚
        /// </summary>
        public int customerMedicalStatus { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string customerMobilePhone { get; set; }
        /// <summary>
        /// 分组编号，或者个检套餐编号
        /// </summary>
        public string appointmentPackageCode { get; set; }
        /// <summary>
        /// 预约日期yyyy-MM-dd
        /// </summary>
        public string appointmentDate { get; set; }
        /// <summary>
        /// 外部订单编号即：调用方订单编号
        /// </summary>
        public string outOrderCode { get; set; }
        /// <summary>
        /// 部门编号，单位编号(合作单位)
        /// </summary>
        public string deptID { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal ddJe { get; set; }
        /// <summary>
        /// 体检项目，加项
        /// </summary>
        public List<PreOrderItems> items { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string yybh { get; set; }
        /// <summary>
        /// 预约编号，下单成功返回的值,体检软件值
        /// </summary>
        public string orderCode { get; set; }
        public string tradeNo { get; set; }
        /// <summary>
        /// 院区
        /// </summary>
        public string yuanqu { get; set; }
      
    }

    public class OrderModel
    {
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string customerName { get; set; }
        /// <summary>
        /// 证件类型1身份证，2其他
        /// </summary>
        public int customerIDType { get; set; }
        /// <summary>
        /// 客户证件号码
        /// </summary>
        public string customerIDCard { get; set; }
        /// <summary>
        /// 客户性别：1：男，0：女
        /// </summary>
        public int customerGender { get; set; }
        /// <summary>
        /// 出生日期yyyy-MM-dd
        /// </summary>
        public string customerBirthday { get; set; }
        /// <summary>
        /// 客户年龄
        /// </summary>
        public int customerAge { get; set; }
        /// <summary>
        /// 婚姻状况,0:已婚，1：未婚
        /// </summary>
        public int customerMedicalStatus { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string customerMobilePhone { get; set; }
        /// <summary>
        /// 分组编号，或者个检套餐编号
        /// </summary>
        public string appointmentPackageCode { get; set; }
        public string appointmentPackageName { get; set; }
        /// <summary>
        /// 预约日期yyyy-MM-dd
        /// </summary>
        public string appointmentDate { get; set; }
        /// <summary>
        /// 外部订单编号即：调用方订单编号
        /// </summary>
        public string outOrderCode { get; set; }
        /// <summary>
        /// 部门编号，单位编号(合作单位)
        /// </summary>
        public string deptID { get; set; }
        public string deptNm { get; set; }
        public int dsfbzId { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal ddJe { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string yybh { get; set; }
        /// <summary>
        /// 原始结算价
        /// </summary>
        public decimal ysjsj { get; set; }


    }

    public class PreOrderItems
    {
        /// <summary>
        /// 体检项目编号
        /// </summary>
        public string itemCode { get; set; }
        /// <summary>
        /// 体检项目编号
        /// </summary>
        public string itemName { get; set; }
        /// <summary>
        /// 体检项目原来价格
        /// </summary>
        public decimal originalPrice { get; set; }
        /// <summary>
        /// 体检项目折后价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 是否加项 0-常规 1-加项
        /// </summary>
        public int isAdd { get; set; }
    }

    /// <summary>
    /// 团检修改参数
    /// </summary>
    public class ChangeTjOrder : UserRequest
    {
        /// <summary>
        /// 预约编号，下单成功返回的值，即djlsh
        /// </summary>
        public string yybh { get; set; }

        /// <summary>
        /// 体检日期
        /// </summary>
        public string yysj { get; set; }
        public string phone { get; set; }
        public string sfzh { get; set; }

        /// <summary>
        /// 体检加项项目
        /// </summary>
        public List<PreOrderItems> items { get; set; }
    }
}
