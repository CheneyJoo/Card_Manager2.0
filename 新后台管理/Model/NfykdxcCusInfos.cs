using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 南医三院体检
    /// </summary>
    public class NfykdxcCusInfos
    {

    }

    /// <summary>
    /// 订单改期对象
    /// </summary>
    public class ChangeOrderDateModel {
        /// <summary>
        /// 医院订单号
        /// </summary>
        public string regNo { set; get; }
        /// <summary>
        /// 改期时间
        /// </summary>
        public string RegDate { set; get; }
    }
    /// <summary>
    /// 第三方订单对象
    /// </summary>
    public class ExternalReserveModel {
        /// <summary>
        /// 体检人姓名
        /// </summary>
        public string patName { get; set; }
        /// <summary>
        /// 体检人电话
        /// </summary>
        public string patTel { set; get; }
        /// <summary>
        /// 体检人身份证
        /// </summary>
        public string id_card { set; get; }
        /// <summary>
        /// 体检日期
        /// </summary>
        public string RegDate { set; get; }
        /// <summary>
        /// 体检人性别 1男 2女
        /// </summary>
        public string patSex { set; get; }
        /// <summary>
        /// 是否为VIP（“T”:是“F”:否）
        /// </summary>
        public string IsVip { set; get; }
        /// <summary>
        /// 金额  “单位’分’”
        /// </summary>
        public string PayAmount { set; get; }
        /// <summary>
        /// 第三方平台订单号
        /// 体检网订单号
        /// </summary>
        public string ExternalOrder { set; get; }
        /// <summary>
        /// 体检时段代码
        /// </summary>
        public string ArrCode { set; get; }
        /// <summary>
        /// 套餐编号
        /// </summary>
        public string ClusA { set; get; }
        /// <summary>
        /// 额外体检项目信息
        /// </summary>
        public string ItemList { set; get; }

        /// <summary>
        /// “1”:入职体检套餐信息
        ///“2”:驾驶员体检套餐信息
        ///“3”:团检套餐信息,
        ///“4”个人健康
        /// </summary>
        public string regType { set; get; }
        /// <summary>
        /// 给定第三方标识码
        /// 非必填
        /// </summary>
        public string ExternalCode { set; get; }

        /// <summary>
        /// 1身份证 2其他证件
        /// </summary>
        public string cardType { get; set; }
        /// <summary>
        /// “yyyy.MM.dd”
        /// </summary>
        public string patBirthday { get; set; }

    }

    #region 套餐对象
    /// <summary>
    /// 南医三院体检套餐对象
    /// </summary>
    public class returnData
    {
        /// <summary>
        /// 注意事项
        /// </summary>
        public string neednote { set; get; }
        /// <summary>
        /// 体检须知
        /// </summary>
        public string tjnote { set; get; }
        /// <summary>
        /// 套餐列表
        /// </summary>
        public List<_clusInfos> clusInfos;
    }
    /// <summary>
    /// 套餐
    /// </summary>
    public class _clusInfos
    {
        /// <summary>
        ///套餐码
        /// </summary>
        public string clus_code { set; get; }
        /// <summary>
        /// 套餐名字
        /// </summary>
        public string clus_name { set; get; }
        /// <summary>
        /// 套餐价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 1男 2女 3 通用
        /// </summary>
        public string sex { set; get; }
        /// <summary>
        /// 备注说明
        /// </summary>
        public string note { set; get; }
        public string clus_note { set; get; }//
        public string compareFlag { set; get; }//
        /// <summary>
        /// 套餐组合项目列表
        /// </summary>
        public List<_itemInfos> itemInfos;
    }
    /// <summary>
    /// 套餐组合项目列表
    /// </summary>
    public class _itemInfos
    {
        /// <summary>
        /// 套餐码
        /// </summary>
        public string clus_code { get; set; }
        /// <summary>
        /// 组合码
        /// </summary>
        public string comb_code { get; set; }
        /// <summary>
        /// 组合名字
        /// </summary>
        public string comb_name { get; set; }//
        /// <summary>
        /// 类型
        /// </summary>
        public string cls_code { get; set; }//
        /// <summary>
        /// 区分是否是可自选项目（*标志不是自选项目）
        /// </summary>
        public string check_cls { get; set; }//
        /// <summary>
        /// 项目价格
        /// </summary>
        public string price { get; set; }//
        /// <summary>
        /// 备注说明
        /// </summary>
        public string note { get; set; }//
        /// <summary>
        /// 检查标识
        /// </summary>
        public string checkFlag { set; get; }//
    }
    #endregion

    #region 号源对象
    /// <summary>
    /// 号源列表
    /// </summary>
    public class SumNoModel
    {
        /// <summary>
        /// 号源对象
        /// </summary>
        public List<_arrCodeItems> arrCodeItems;
        /// <summary>
        /// 号源日期
        /// </summary>
        public string book_date { set; get; }
        public string shift_no { set; get; }
        public string book_flag { set; get; }
    }
    /// <summary>
    /// 号源对象
    /// </summary>
    public class _arrCodeItems
    {
        /// <summary>
        /// 时段编码
        /// </summary>
        public string arr_code { set; get; }
        /// <summary>
        /// 时段信息
        /// </summary>
        public string ShiftName { set; get; }
        /// <summary>
        /// 该时段总号源（有LncCode时获取我方平台的号源）
        /// </summary>
        public string sum_num { set; get; }
        /// <summary>
        /// 已约人数（有LncCode时获取我方平台的号源）
        /// </summary>
        public string book_num { set; get; }
        /// <summary>
        /// 剩余号源（有LncCode时获取我方平台的号源）
        /// </summary>
        public string left_num { set; get; }
        /// <summary>
        /// 暂未启用
        /// </summary>
        public string add_num { set; get; }
    } 
    #endregion

}
