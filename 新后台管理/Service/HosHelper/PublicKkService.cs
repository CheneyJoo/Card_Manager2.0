using Common;
using Model;
using Model.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.HosHelper
{

    /// <summary>
    /// 由康康网提供的公共接口
    /// </summary>
    //public static class PublicKkService
    //{
    //    /// <summary>
    //    /// 同步组合项目
    //    /// </summary>
    //    /// <param name="jgModel"></param>
    //    /// <param name="msg"></param>
    //    /// <returns></returns>
    //    public static bool ZhxmTb(XtJgbModel jgModel, ref string msg)
    //    {
    //        UserRequest model = new UserRequest();
    //        //调用医院接口
    //        model.username = jgModel.account;
    //        model.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //        model.sign = Common.MD5.MD5Encrypt32(jgModel.account + jgModel.pwd + model.timestamp.Trim());

    //        try
    //        {
    //            string resultStr = WebHelper.PostUrl(jgModel.jgjkurl + "api/Tc/Zhxmtb", Newtonsoft.Json.JsonConvert.SerializeObject(model));
               
    //            if (!string.IsNullOrEmpty(resultStr))
    //            {

    //                dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultStr));
    //                if ((string)resultObj.state == "200")
    //                {
    //                    List<XtZhxmbModel> li = Newtonsoft.Json.JsonConvert.DeserializeObject<List<XtZhxmbModel>>((string)resultObj.result);
    //                    foreach (XtZhxmbModel item in li)
    //                    {
    //                        item.yybh = jgModel.yybh;
    //                        item.zhxmbh = string.IsNullOrEmpty(item.zhxmbh) ? "" : item.zhxmbh.Trim();
    //                        item.zhxmmc = string.IsNullOrEmpty(item.zhxmmc) ? "" : item.zhxmmc.Trim();
    //                        item.zhxmksbh = string.IsNullOrEmpty(item.zhxmksbh) ? "" : item.zhxmksbh.Trim();
    //                        item.zhxmksmc = string.IsNullOrEmpty(item.zhxmksmc) ? "" : item.zhxmksmc.Trim();
    //                        item.zhxmms = string.IsNullOrEmpty(item.zhxmms) ? "" : item.zhxmms.Trim();
    //                        item.sxrs = 0;
    //                    }
                      
    //                    new XtzhxmService().InsertOrUpdate(li);
    //                    return true;

    //                }
    //                else
    //                {
    //                    msg = (string)resultObj.message;
    //                    return false;
    //                }
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogApiHelper.AddLog(ex.Message);
    //            msg = ex.Message;
    //            return false;
    //        }

    //    }
    //    /// <summary>
    //    /// 企业基本信息同步查询
    //    /// </summary>
    //    /// <param name="jgModel"></param>
    //    /// <param name="start"></param>
    //    /// <param name="end"></param>
    //    /// <param name="msg"></param>
    //    /// <returns></returns>
    //    public static bool QyjbxxTb(XtJgbModel jgModel, ref string msg)
    //    {
    //        List<QyJbxxModel> li = new List<QyJbxxModel>();
    //        UserRequest model = new UserRequest();
    //        //调用医院接口
    //        model.username = jgModel.account;
    //        model.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //        model.sign = Common.MD5.MD5Encrypt32(jgModel.account + jgModel.pwd + model.timestamp.Trim());
    //        model.page = 1;
    //        model.pagesize = 999999999;
    //        model.publicvalue = "";
    //        try
    //        {
    //            string resultStr = WebHelper.PostUrl(jgModel.jgjkurl + "api/Tc/Qytb", Newtonsoft.Json.JsonConvert.SerializeObject(model));
    //            if (!string.IsNullOrEmpty(resultStr))
    //            {
    //                dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultStr));
    //                if ((string)resultObj.state == "200")
    //                {
    //                    li = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QyJbxxModel>>((string)resultObj.result);
    //                    if (li != null && li.Count > 0)
    //                    {
    //                        foreach (QyJbxxModel item in li)
    //                        {
    //                            item.yybh = jgModel.yybh;
    //                            item.istb = 0;
    //                            item.bh = item.bh.Trim();
    //                            item.mc = item.mc.Trim();
    //                            item.lxdh = string.IsNullOrEmpty(item.lxdh) ? "" : item.lxdh.Trim();
    //                            item.lxdz = string.IsNullOrEmpty(item.lxdz) ? "" : item.lxdz.Trim();
    //                            if(jgModel.yybh== "YY201512241012120001")//郑州人民医院
    //                            {
    //                                item.isdept = 0;
    //                            }
    //                             else
    //                            {
    //                                item.isdept = item.bh.Length == 4 ? 0 : 1;//妙手企业，部门做法
    //                            }
                               
    //                        }

    //                        new QyjbxxService().InsertOrUpdate(li);
    //                        return true;
    //                    }
    //                    else
    //                    {
    //                        return false;
    //                    }
    //                }
    //                else
    //                {
    //                    return false;
    //                }
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogApiHelper.AddLog(ex.Message);
    //            msg = ex.Message;
    //            return false;
    //        }

    //    }
    //    /// <summary>
    //    /// 企业套餐同步
    //    /// </summary>
    //    /// <param name="jgModel"></param>
    //    /// <param name="dwbh"></param>
    //    /// <param name="msg"></param>
    //    /// <returns></returns>
    //    public static bool QyTcTb(XtJgbModel jgModel, string dwbh,int tclx,int dsfbzid, ref string msg)
    //    {
    //        UserRequest model = new UserRequest();
    //        //调用医院接口
    //        model.username = jgModel.account;
    //        model.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //        model.sign = Common.MD5.MD5Encrypt32(jgModel.account + jgModel.pwd + model.timestamp.Trim());
    //        model.publicvalue = dwbh;
    //        try
    //        {
    //            string resultStr = WebHelper.PostUrl(jgModel.jgjkurl + "api/Tc/QyTctb", Newtonsoft.Json.JsonConvert.SerializeObject(model));
    //            if (!string.IsNullOrEmpty(resultStr))
    //            {

    //                dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultStr));
    //                if ((string)resultObj.state == "200")
    //                {
    //                    List<XttcbModel> li = Newtonsoft.Json.JsonConvert.DeserializeObject<List<XttcbModel>>((string)resultObj.result);
    //                    List<XttczhxmbModel> lizhxm = Newtonsoft.Json.JsonConvert.DeserializeObject<List<XttczhxmbModel>>((string)resultObj.result2);
    //                    if (li != null)
    //                    {
    //                        foreach (XttcbModel item in li)
    //                        {
    //                            item.yybh = jgModel.yybh;
    //                            item.tcbh = string.IsNullOrEmpty(item.tcbh) ? "" : item.tcbh.Trim();
    //                            item.tcmc = string.IsNullOrEmpty(item.tcmc) ? "" : item.tcmc.Trim();
    //                            item.dwbh = string.IsNullOrEmpty(item.dwbh) ? "" : item.dwbh.Trim();
    //                            item.dwmc = string.IsNullOrEmpty(item.dwmc) ? "" : item.dwmc.Trim();
    //                            item.tclx = tclx;
    //                            item.dsfbzid = dsfbzid;
    //                        }
    //                        foreach (XttczhxmbModel item in lizhxm)
    //                        {
    //                            item.yybh = jgModel.yybh;
    //                            item.dwbh = string.IsNullOrEmpty(li[0].dwbh) ? "" : li[0].dwbh.Trim();
    //                            item.zhxmbh = string.IsNullOrEmpty(item.zhxmbh) ? "" : item.zhxmbh.Trim();
    //                            item.tcbh = string.IsNullOrEmpty(item.tcbh) ? "" : item.tcbh.Trim();
    //                            item.createtime = DateTime.Now;
    //                        }

    //                        new XttcbService().InsertOrUpdate(li, lizhxm);
    //                    }
    //                    return true;

    //                }
    //                else
    //                {
    //                    msg = (string)resultObj.message;
    //                    return false;
    //                }
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            msg = ex.Message;
    //            return false;
    //        }

    //    }
    //    /// <summary>
    //    /// 个人套餐同步
    //    /// </summary>
    //    /// <param name="jgModel"></param>
    //    /// <param name="dwbh"></param>
    //    /// <param name="msg"></param>
    //    /// <returns></returns>
    //    public static bool GrTcTb(XtJgbModel jgModel, ref string msg)
    //    {
    //        UserRequest model = new UserRequest();
    //        //调用医院接口
    //        model.username = jgModel.account;
    //        model.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //        model.sign = Common.MD5.MD5Encrypt32(jgModel.account + jgModel.pwd + model.timestamp.Trim());

    //        try
    //        {
    //            string resultStr = WebHelper.PostUrl(jgModel.jgjkurl + "api/Tc/GrTctb", Newtonsoft.Json.JsonConvert.SerializeObject(model));
    //            if (!string.IsNullOrEmpty(resultStr))
    //            {

    //                dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultStr));
    //                if ((string)resultObj.state == "200")
    //                {
    //                    List<XttcbModel> li = Newtonsoft.Json.JsonConvert.DeserializeObject<List<XttcbModel>>((string)resultObj.result);
    //                    List<XttczhxmbModel> lizhxm = Newtonsoft.Json.JsonConvert.DeserializeObject<List<XttczhxmbModel>>((string)resultObj.result2);
    //                    if (li != null)
    //                    {
    //                        foreach (XttcbModel item in li)
    //                        {
    //                            item.yybh = jgModel.yybh;
    //                            item.tcbh = string.IsNullOrEmpty(item.tcbh) ? "" : item.tcbh.Trim();
    //                            item.tcmc = string.IsNullOrEmpty(item.tcmc) ? "" : item.tcmc.Trim();
    //                            item.dwbh = "0";
    //                            item.dwmc = "个人套餐";
    //                            item.tclx = 0;                            
    //                            item.dsfbzid = 0;
    //                        }
    //                        foreach (XttczhxmbModel item in lizhxm)
    //                        {
    //                            item.yybh = jgModel.yybh;
    //                            item.dwbh = "0";
    //                            item.zhxmbh = string.IsNullOrEmpty(item.zhxmbh) ? "" : item.zhxmbh.Trim();
    //                            item.tcbh = string.IsNullOrEmpty(item.tcbh) ? "" : item.tcbh.Trim();
    //                            item.createtime = DateTime.Now;
    //                        }

    //                        new XttcbService().InsertOrUpdate(li, lizhxm);
    //                    }
    //                    return true;

    //                }
    //                else
    //                {
    //                    msg = (string)resultObj.message;
    //                    return false;
    //                }
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            msg = ex.Message;
    //            return false;
    //        }

    //    }
    //    /// <summary>
    //    /// 企业套餐下人员同步
    //    /// </summary>
    //    /// <param name="jgModel"></param>
    //    /// <param name="dwbh"></param>
    //    /// <param name="msg"></param>
    //    /// <returns></returns>
    //    public static bool QyryTb(XtJgbModel jgModel, string dwbh, ref string msg)
    //    {
    //        bool res = false;
    //        for(int i=1;i<200;i++)
    //        {
    //            UserRequest model = new UserRequest();
    //            //调用医院接口
    //            model.username = jgModel.account;
    //            model.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //            model.sign = Common.MD5.MD5Encrypt32(jgModel.account + jgModel.pwd + model.timestamp.Trim());
    //            model.publicvalue = dwbh;
    //            model.page = i;
    //            model.pagesize = 500;
    //            try
    //            {
    //                string resultStr = WebHelper.PostUrl(jgModel.jgjkurl + "api/Tc/Qyrytb", Newtonsoft.Json.JsonConvert.SerializeObject(model));
    //                if (!string.IsNullOrEmpty(resultStr))
    //                {

    //                    dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultStr));
    //                    if ((string)resultObj.state == "200")
    //                    {
    //                        List<QyygxxModel> li = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QyygxxModel>>((string)resultObj.result);
    //                        LogApiHelper.AddLog((string)resultObj.result);
    //                        if (li != null)
    //                        {
    //                            foreach (QyygxxModel item in li)
    //                            {
    //                                item.yybh = jgModel.yybh;
    //                                item.ydjh = string.IsNullOrEmpty(item.ydjh) ? "" : item.ydjh.Trim();
    //                                item.tcbh = string.IsNullOrEmpty(item.tcbh) ? "" : item.tcbh.Trim();
    //                                item.tcmc = string.IsNullOrEmpty(item.tcmc) ? "" : item.tcmc.Trim();
    //                                item.dwbh = string.IsNullOrEmpty(item.dwbh) ? "" : item.dwbh.Trim();
    //                                item.dwmc = string.IsNullOrEmpty(item.dwmc) ? "" : item.dwmc.Trim();
    //                                item.xm = string.IsNullOrEmpty(item.xm) ? "" : item.xm.Trim();
    //                                item.sfzh = string.IsNullOrEmpty(item.sfzh) ? "" : item.sfzh.Trim();
    //                                item.dh = string.IsNullOrEmpty(item.dh) ? "" : item.dh.Trim();
    //                                item.rybh = string.IsNullOrEmpty(item.rybh) ? "" : item.rybh.Trim(); 
    //                                if (item.sfzh.Length >= 6)
    //                                {
    //                                    item.mm = item.sfzh.Substring(item.sfzh.Length - 6, 6);
    //                                }
    //                                else
    //                                {
    //                                    item.mm = "000666";
    //                                }
    //                            }

    //                            new QyygjbxxService().InsertOrUpdate(li, dwbh);
    //                        }
    //                        else
    //                        {
    //                            break;
    //                        }
    //                        res= true;

    //                    }
    //                    else
    //                    {
                          
    //                        msg = (string)resultObj.message;
    //                        res= false;
    //                    }
    //                }
    //                else
    //                {
    //                    res= false;
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                LogApiHelper.AddLog(ex.Message);
    //                msg = ex.Message;
    //                res= false;
    //            }
    //        }
    //        return res;
    //    }

    //    /// <summary>
    //    /// 预约到医院,渠道个检，团检家属使用
    //    /// </summary>
    //    /// <param name="ddbh"></param>
    //    /// <param name="msg"></param>
    //    /// <returns></returns>
    //    public static bool Ddyy(string ddbh ,ref string msg)
    //    {
    //        bool res = true;
    //        try
    //        {
              
    //            DdjbxxModel ddModel = new DdyymxbService().GetBydsfddxx(ddbh);
    //            if (ddModel == null)
    //            {
    //                msg = "订单不存在";
    //                res= false;
    //            }
    //            else
    //            {                 
    //                //如果中间库成功，再调用医院接口，分3种情况，手动，全自动，半自动
    //                XtJgbModel jgModel = new Service.XtjgbService().GetJg(ddModel.yybh);
    //                if (jgModel.iskkservice == 1)
    //                {
    //                    PreOrder order = new PreOrder();
    //                    //调用医院接口
    //                    order.username = jgModel.account;
    //                    order.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //                    order.sign = Common.MD5.MD5Encrypt32(jgModel.account + jgModel.pwd + order.timestamp.Trim());
    //                    order.appointmentDate = ddModel.yykssj.ToString("yyy-MM-dd");
    //                    order.appointmentPackageCode = ddModel.tcid;
    //                    order.customerAge = ddModel.nl;
    //                    order.customerBirthday = ddModel.csrq;
    //                    order.customerGender = ddModel.xb;
    //                    order.customerIDCard = ddModel.zjhm;
    //                    order.customerIDType = ddModel.zjlx;
    //                    order.customerMedicalStatus = ddModel.hz;
    //                    order.customerMobilePhone = ddModel.dh;
    //                    order.deptID = ddModel.dwbh;
    //                    order.customerName = ddModel.xm;
    //                    order.outOrderCode = ddModel.dsfdd;
    //                    order.tradeNo = ddModel.trade_no;    
    //                    if(ddModel.yybh== "YY201512241012120001")//郑州人民医院本部
    //                    {
    //                        order.yuanqu = "本部";
    //                    }
    //                    if(ddModel.yybh== "YY201912091748440001")//郑州人民医院（郑东院区）
    //                    {
    //                        order.yuanqu = "东区";
    //                    }                
    //                    try
    //                    {
    //                        string resultStr = WebHelper.PostUrl(jgModel.jgjkurl + "api/PreOrder/PreOrder", Newtonsoft.Json.JsonConvert.SerializeObject(order));
    //                        if (!string.IsNullOrEmpty(resultStr))
    //                        {

    //                            dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultStr));
    //                            if ((string)resultObj.state == "200")
    //                            {

    //                                new DdyymxbService().UpdatOrderdjlsh(ddModel.ddbh, (string)resultObj.result.orderCode);
    //                                return true;
    //                            }
    //                            else
    //                            {
    //                                msg = (string)resultObj.message;
    //                                res = false;

    //                            }
    //                        }
    //                        else
    //                        {
    //                            msg = "接口返回错误"; ;
    //                            res = false;
    //                        }
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        msg = ex.Message;
    //                         res = false;
    //                    }

    //                }                  
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            msg = ex.Message;
    //            res = false;
               
    //        }
    //        return res;
    //    }
    //    /// <summary>
    //    /// 取消订单
    //    /// </summary>
    //    /// <param name="ddbh"></param>
    //    /// <param name="msg"></param>
    //    /// <returns></returns>
    //    public static bool CancelDdyy(string ddbh, ref string msg)
    //    {
    //        bool res = true;
    //        try
    //        {

    //            DdjbxxModel ddModel = new DdyymxbService().GetBydsfddxx(ddbh);
    //            if (ddModel == null)
    //            {
    //                msg = "订单不存在";
    //                res = false;
    //            }
    //            else
    //            {
    //                PreOrder order = new PreOrder();
    //                XtJgbModel jgModel = new Service.XtjgbService().GetJg(ddModel.yybh);
    //                if (jgModel.iskkservice == 1)
    //                {

    //                    //调用医院接口
    //                    order.username = jgModel.account;
    //                    order.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //                    order.sign = Common.MD5.MD5Encrypt32(jgModel.account + jgModel.pwd + order.timestamp.Trim());

    //                    try
    //                    {
    //                        if (!string.IsNullOrEmpty(ddModel.djlsh))
    //                        {
    //                            order.orderCode = ddModel.djlsh;
    //                            string resultStr = WebHelper.PostUrl(jgModel.jgjkurl + "api/PreOrder/CancelOrder", Newtonsoft.Json.JsonConvert.SerializeObject(order));
    //                            if (!string.IsNullOrEmpty(resultStr))
    //                            {

    //                                dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultStr));
    //                                if ((string)resultObj.state == "200")
    //                                {
    //                                    new DdyymxbService().Delete(ddbh);

    //                                }
    //                                else
    //                                {
    //                                    msg = (string)resultObj.message;
    //                                    LogApiHelper.AddLog(ddbh + "取消失败" + msg);
    //                                    res = false;

    //                                }
    //                            }
    //                            else
    //                            {
    //                                msg = "接口返回数据为空";
    //                                res = false;

    //                            }
    //                        }
    //                        else
    //                        {
    //                            new DdyymxbService().Delete(ddbh);
    //                        }
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        LogApiHelper.AddLog(ddbh + "取消失败" + msg);
    //                        msg = ex.Message;
    //                        res = false;

    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogApiHelper.AddLog(ddbh + "取消失败" + msg);
    //            msg = ex.Message;
    //            res = false;
    //        }
    //        return res;
    //    }

    //    /// <summary>
    //    /// 团检预约使用
    //    /// </summary>
    //    /// <param name="ddbh"></param>
    //    /// <param name="type">1,第一次预约，2第二次修改</param>
    //    /// <param name="msg"></param>
    //    /// <returns></returns>
    //    public static bool TjDdyy(string ddbh,int type, ref string msg)
    //    {
    //        bool res = true;
    //        try
    //        {

    //            DdjbxxModel ddModel = new DdyymxbService().GetBydsfddxx(ddbh);
    //            if (ddModel == null)
    //            {
    //                msg = "订单不存在";
    //                res = false;
    //            }
    //            else
    //            {
    //                //如果中间库成功，再调用医院接口，分3种情况，手动，全自动，半自动
    //                XtJgbModel jgModel = new Service.XtjgbService().GetJg(ddModel.yybh);
    //                if (jgModel.iskkservice == 1)
    //                {
    //                    ChangeTjOrder order = new ChangeTjOrder();
    //                    //调用医院接口
    //                    order.username = jgModel.account;
    //                    order.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //                    order.sign = Common.MD5.MD5Encrypt32(jgModel.account + jgModel.pwd + order.timestamp.Trim());
    //                    order.yysj = ddModel.yykssj.ToString("yyyy-MM-dd");
    //                    order.yybh = ddModel.dsfdd;
    //                    order.phone = ddModel.dh;
    //                    order.sfzh = ddModel.zjhm;
    //                    if(ddModel.sfjx==1)
    //                    {
    //                        if (type == 1)
    //                        {
    //                            List<PreOrderItems> li = new List<PreOrderItems>();
    //                            List<DdZhxmModel> lizhxm = new DdyymxbService().GetOrderZhxmJx(ddbh);
    //                            foreach (DdZhxmModel item in lizhxm)
    //                            {
    //                                li.Add(new PreOrderItems() { isAdd = 1, itemCode = item.zhxmbh });
    //                            }
    //                            order.items = li;
    //                        }
    //                    }
    //                    try
    //                    {
    //                        string resultStr = WebHelper.PostUrl(jgModel.jgjkurl + "api/PreOrder/ChangeTjOrder", Newtonsoft.Json.JsonConvert.SerializeObject(order));
    //                        if (!string.IsNullOrEmpty(resultStr))
    //                        {

    //                            dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultStr));
    //                            if ((string)resultObj.state == "200")
    //                            {
    //                                new DdyymxbService().UpdatOrderdjlsh(ddModel.ddbh, (string)resultObj.result.orderCode);
    //                                return true;
    //                            }
    //                            else
    //                            {
    //                                msg = (string)resultObj.message;
    //                                res = false;

    //                            }
    //                        }
    //                        else
    //                        {
    //                            msg = "接口返回错误"; ;
    //                            res = false;
    //                        }
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        LogApiHelper.AddLog(ex.Message);
    //                        msg = ex.Message;
    //                        res = false;
    //                    }

    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            msg = ex.Message;
    //            res = false;

    //        }
    //        return res;
    //    }
    //}
}
