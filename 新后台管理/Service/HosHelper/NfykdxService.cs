using Common;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Service.HosHelper
{
    /// <summary>
    /// 南方医科大学第三附属医院业务对接
    /// </summary>
    public static class NfykdxService
    {
        public static XtJgbModel model = null;
        /// <summary>
        /// 本地医院编号
        /// </summary>
        public static string nfykdxYYBH =ConfigurationManager.AppSettings["nfykdxYYBH"];
        /// <summary>
        /// 第三方平台编号
        /// </summary>
        public static string ExternalCode = ConfigurationManager.AppSettings["ExternalCode"];
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static NfykdxService (){
            model = new XtjgbService().GetJg(nfykdxYYBH);
        }
        /// <summary>
        /// 校验机构配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ReturnMessage ckeckJG(XtJgbModel model)
        {
            ReturnMessage Message = new ReturnMessage();
            if (model == null)
            {
                Message.state = "401";
                Message.message = "未查到机构";
                Message.result = "";
                LogApiHelper.AddErrorLog("getToke-预约下单成功,医院失败，未查到机构");
                return Message;
            }
            //没有过期则直接返回
            if (string.IsNullOrEmpty(model.jgjkurl) || string.IsNullOrEmpty(model.account) || string.IsNullOrEmpty(model.pwd))
            {
                Message.state = "402";
                Message.message = "请检查机构接口配置表";
                Message.result = "";
                LogApiHelper.AddErrorLog("getToke-预约下单成功,医院失败，请检查机构接口配置表");
                return Message;
            }
            Message.state = "200";
            Message.message = "";
            Message.result = "";
            return Message;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static ReturnMessage getToke()
        {
            ReturnMessage Message = ckeckJG(model);

            if (Message.state != "200")
            {
                return Message;
            }

            string apiUrl = model.jgjkurl;//请求地址
            apiUrl += "Token";

            try
            {
                var authBasic = Common.CommonFunction.Base64Code("hospital:1234");
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Basic " + authBasic);

                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("grant_type", "password");
                parameters.Add("Password", model.pwd);
                parameters.Add("username", model.account);
                string resultStr = Common.WebHelper.PostData_OAuth2_V1(apiUrl, parameters, Headers,"html");
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "getToke-获取Toke返回值为空";
                    Message.result = "";
                    return Message;
                }
                else
                {
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    string access_token = Convert.ToString(resultObject.access_token);
                    if (!string.IsNullOrEmpty(access_token))
                    {
                        Message.state = "200";
                        Message.message = "成功";
                        Message.result = access_token;
                        return Message;
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = resultObject.msg;
                        Message.result = "";
                        LogApiHelper.AddErrorLog("getToke-获取token返回错误=" + resultObject.msg);
                        return Message;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.state = "407";
                Message.message = "获取Toke异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("getToke-获取token报错=" + ex.ToString());
                return Message;
            }
        }

        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static returnData GetClusInfo(string access_token,out ReturnMessage Message) {
            Message = ckeckJG(model);

            if (Message.state != "200")
            {
                return null;
            }

            try
            {
                string apiUrl = model.jgjkurl;//请求地址
                apiUrl += "api/External/GetClusInfo";
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Bearer " + access_token);
                
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("regType", "4");
                string resultStr = Common.WebHelper.PostData_OAuth2_V1(apiUrl, parameters, Headers,"json");
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "GetClusInfo-获取套餐信息失败";
                    Message.result = "";
                    return null;
                }
                else
                {
                    resultStr = resultStr.Replace("\r\n", "").Replace("\n", "");
                    resultStr = resultStr.Replace("\"\"", "\"").Replace(":\",", ":\"\",");
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    if (resultObject.success== true)
                    {
                        returnData TCList = JsonConvert.DeserializeObject<returnData>(Convert.ToString(resultObject.returnData));
                        Message.state = "200";
                        Message.message = "";
                        Message.result = "";
                        return TCList;
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = resultObject.returnMsg;
                        Message.result = "";
                        LogApiHelper.AddErrorLog("GetClusInfo-获取套餐信息不成功=" + resultObject.msg);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                Message.state = "407";
                Message.message = "获取套餐信息异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("GetClusInfo-获取套餐信息异常=" + ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// 获取耗材费用
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static decimal GetOrderMaterialsFee(string comb_code, out ReturnMessage Message)
        {
            Message = ckeckJG(model);

            string access_token = "";

            #region 获取token
            string nysykey = "nysy_access_token";
            if (AspNetCache.Exist(nysykey) == false)
            {
                ReturnMessage tokenModel = Service.HosHelper.NfykdxService.getToke();
                if (tokenModel.state != "200")
                {
                    Message.message = tokenModel.message;
                    Message.state = tokenModel.state;
                    Message.result = "";
                    LogApiHelper.AddErrorLog("GetOrderMaterialsFee-获取耗材价格异常=" + tokenModel.message);
                    return 0;
                }
                access_token = tokenModel.result.ToString();
                AspNetCache.Insert(nysykey, access_token, DateTime.Now.AddSeconds(7000));
            }
            else
            {
                access_token = AspNetCache.Get<string>(nysykey);
            }
            #endregion

 
            try
            {
                string apiUrl = model.jgjkurl;//请求地址
                apiUrl += "api/External/OrderMaterialsFee";
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Bearer " + access_token);

                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("comb_code", comb_code);
                string resultStr = Common.WebHelper.PostData_OAuth2_V1(apiUrl, parameters, Headers, "json");
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "GetOrderMaterialsFee-获取耗材价格异常";
                    Message.result = "";
                    return 0;
                }
                else
                {
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    if (resultObject.success == true)
                    {
                        dynamic returnData = resultObject.returnData;
                        string price = returnData.price;
                        Message.state = "200";
                        Message.message = "";
                        Message.result = "";
                        return string.IsNullOrEmpty(price) ? 0 : decimal.Parse(price);
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = resultObject.returnMsg;
                        Message.result = "";
                        LogApiHelper.AddErrorLog("GetClusInfo-获取耗材价格异常=" + resultObject.msg);
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {

                Message.state = "407";
                Message.message = "获取套餐信息异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("GetClusInfo-获取耗材价格异常=" + ex.ToString());
                return 0;
            }

        }

        /// <summary>
        /// 获取号源
        /// ExternalCode 为必填参数，LncCode为空时，返回第三方的号源，LncCode不为空时返回该单位的号源
        /// </summary>
        /// <param name="LncCode">团检单位编码</param>
        /// <param name="access_token"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static List<SumNoModel> GetNo(string LncCode, string access_token, out ReturnMessage Message)
        {

            Message = ckeckJG(model);

            if (Message.state != "200")
            {
                return null;
            }

            try
            {
                string apiUrl = model.jgjkurl;//请求地址
                apiUrl += "api/External/GetNo";
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Bearer " + access_token);

                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("ExternalCode", ExternalCode);
                if (string.IsNullOrEmpty(LncCode))
                {
                    parameters.Add("LncCode", LncCode);
                }
                string resultStr = Common.WebHelper.PostData_OAuth2_V1(apiUrl, parameters, Headers, "json");
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "GetClusInfo-获取套餐信息失败";
                    Message.result = "";
                    return null;
                }
                else
                {
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    if (resultObject.success == true)
                    {
                        //dynamic rtData = resultObject.returnData;
                        List<SumNoModel> NoList = JsonConvert.DeserializeObject<List<SumNoModel>>(Convert.ToString(resultObject.returnData.SumNo));
                        Message.state = "200";
                        Message.message = "";
                        Message.result = "";
                        return NoList;
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = resultObject.returnMsg;
                        Message.result = "";
                        LogApiHelper.AddErrorLog("GetClusInfo-获取套餐信息不成功=" + resultObject.msg);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                Message.state = "407";
                Message.message = "获取套餐信息异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("GetClusInfo-获取套餐信息异常=" + ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// 调用第三方下单
        /// 返回第三方订单号
        /// </summary>
        /// <param name="order"></param>
        /// <param name="access_token"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string createorder(ExternalReserveModel order, string access_token, out ReturnMessage Message)
        {

            Message = ckeckJG(model);

            if (Message.state != "200")
            {
                return null;
            }

            try
            {
                string apiUrl = model.jgjkurl;//请求地址
                apiUrl += "api/External/ExternalReserve";
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Bearer " + access_token);
              
                string jsonStr= JsonConvert.SerializeObject(order);
                //Log.writeLog("createorderjsonStr=" + jsonStr);
                string resultStr = Common.WebHelper.PostJson_OAuth2(apiUrl, jsonStr, Headers);
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "GetClusInfo-获取套餐信息失败";
                    Message.result = "";
                    return null;
                }
                else
                {
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    if (resultObject.success == true)
                    {
                        string regNo = Convert.ToString(resultObject.returnData.regNo);
                        Message.state = "200";
                        Message.message = "";
                        Message.result = "";
                        return regNo;
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = resultObject.returnMsg;
                        Message.result = "";
                        LogApiHelper.AddErrorLog("GetClusInfo-获取套餐信息不成功=" + resultObject.msg);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                Message.state = "407";
                Message.message = "获取套餐信息异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("GetClusInfo-获取套餐信息异常=" + ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// 调用第三方订单改期
        /// </summary>
        /// <param name="order"></param>
        /// <param name="access_token"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static void changeorder(ChangeOrderDateModel ChangeOrder, string access_token, out ReturnMessage Message)
        {

            Message = ckeckJG(model);

            if (Message.state != "200")
            {
                return;
            }

            try
            {
                string apiUrl = model.jgjkurl;//请求地址
                apiUrl += "api/External/ChangeOrderDate";
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Bearer " + access_token);

                string jsonStr = JsonConvert.SerializeObject(ChangeOrder);
                string resultStr = Common.WebHelper.PostJson_OAuth2(apiUrl, jsonStr, Headers);
                //Log.writeLog("jsonStr="+ jsonStr);
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "changeorder-改期异常";
                    Message.result = "";
                    return ;
                }
                else
                {
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    if (resultObject.success == true)
                    {
                        Message.state = "200";
                        Message.message = "";
                        Message.result = "";
                        return ;
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = Convert.ToString(resultObject.returnMsg);
                        Message.result = "";
                        LogApiHelper.AddErrorLog("changeorder-改期异常=" + resultObject.msg);
                        return ;
                    }
                }
            }
            catch (Exception ex)
            {

                Message.state = "407";
                Message.message = "改期异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("changeorder-改期异常=" + ex.ToString());
                return ;
            }

        }

        /// <summary>
        /// 调用第三方订单取消订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="access_token"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static void CancelExternalOrder(ChangeOrderDateModel ChangeOrder, string access_token, out ReturnMessage Message)
        {

            Message = ckeckJG(model);

            if (Message.state != "200")
            {
                return;
            }

            try
            {
                string apiUrl = model.jgjkurl;//请求地址
                apiUrl += "api/External/CancelExternalOrder";
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Bearer " + access_token);

                string jsonStr = JsonConvert.SerializeObject(new { regNo =ChangeOrder.regNo});
                string resultStr = Common.WebHelper.PostJson_OAuth2(apiUrl, jsonStr, Headers);
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "CancelExternalOrder-取消异常,返回空信息";
                    Message.result = "";
                    return;
                }
                else
                {
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    if (resultObject.success == true|| resultObject.returnMsg== "找不到该订单或已退单")
                    {
                        Message.state = "200";
                        Message.message = "";
                        Message.result = "";
                        return;
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = Convert.ToString(resultObject.returnMsg);
                        Message.result = "";
                        LogApiHelper.AddErrorLog("CancelExternalOrder-取消异常，返回信息=" + resultStr);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

                Message.state = "407";
                Message.message = "取消异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("CancelExternalOrder-取消异常=" + ex.ToString());
                return;
            }

        }

        /// <summary>
        /// 个人套餐同步南医三院
        /// </summary>
        /// <param name="jgModel"></param>
        /// <param name="dwbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool GrTcTb_nysy(XtJgbModel jgModel,int qdbh, ref string msg)
        {
            try
            {
                ReturnMessage Message = new ReturnMessage();
                //获取南医三院的套餐列表
                returnData TCModelList = GetClusInfo();
                if (TCModelList != null)
                {
                    //套餐
                    List<XttcbModel> TClList = new List<XttcbModel>();
                    //套餐下组合项目关联
                    List<XttczhxmbModel> TCGLlList = new List<XttczhxmbModel>();
                    //套餐下组合项目
                    List<XttczhxmbmxModel> TCZHXMlList = new List<XttczhxmbmxModel>();
                    foreach (_clusInfos itemTC in TCModelList.clusInfos)
                    {
                        XttcbModel TCModel = new XttcbModel();
                        TCModel.yybh = jgModel.yybh;
                        TCModel.tcbh = string.IsNullOrEmpty(itemTC.clus_code) ? "" : itemTC.clus_code.Trim();
                        TCModel.tcmc = string.IsNullOrEmpty(itemTC.clus_name) ? "" : itemTC.clus_name.Trim();
                        TCModel.dwbh = "0";
                        TCModel.dwmc = "南医三院个人套餐";
                        TCModel.tclx = 2;
                        TCModel.jg = itemTC.price;
                        TCModel.sfqy = 1;
                        TCModel.createtime = DateTime.Now;
                        TCModel.xb = getNysySEX(itemTC.sex);
                        TCModel.dsfbzid = qdbh;
                        TClList.Add(TCModel);
                        string tcbh = TCModel.tcbh;
                        decimal OrderMaterialsFee = 0;
                        string zhxmbhList = "";
                        foreach (_itemInfos itemZHXM in itemTC.itemInfos)//组合项目
                        {
                            string zhxmbh = string.IsNullOrEmpty(itemZHXM.comb_code) ? "" : itemZHXM.comb_code.Trim();
                            zhxmbhList += zhxmbh+";";
                           
                            //去重
                            List<XttczhxmbmxModel> TCZHXMlCheck = TCZHXMlList.Where(p => p.zhxmbh == zhxmbh).ToList();
                            if (TCZHXMlCheck.Count == 0)
                            {
                                XttczhxmbmxModel TCZHXMModel = new XttczhxmbmxModel();
                                TCZHXMModel.createtime = DateTime.Now;
                                TCZHXMModel.sffk = 0;
                                TCZHXMModel.sfqy = 1;
                                TCZHXMModel.updatetime = DateTime.Now;
                                TCZHXMModel.xb = getNysySEX(itemTC.sex);
                                TCZHXMModel.yybh = jgModel.yybh;
                                TCZHXMModel.zhxmbh = zhxmbh;
                                TCZHXMModel.zhxmjg = itemZHXM.price;
                                //TCZHXMModel.zhxmksbh = "";
                                //TCZHXMModel.zhxmksmc = "";
                                TCZHXMModel.zhxmmc = itemZHXM.comb_name;
                                TCZHXMModel.zhxmms = itemZHXM.note;
                                TCZHXMlList.Add(TCZHXMModel);

                               
                            }

                            //去重
                            List<XttczhxmbModel> TCGLCheck = TCGLlList.Where(p => p.zhxmbh == zhxmbh&p.tcbh== tcbh).ToList();
                            if (TCGLCheck.Count == 0)
                            {
                                XttczhxmbModel TCGLModel = new XttczhxmbModel();
                                TCGLModel.yybh = jgModel.yybh;
                                TCGLModel.dwbh = "0";
                                TCGLModel.zhxmbh = zhxmbh;
                                TCGLModel.tcbh = tcbh;
                                TCGLModel.createtime = DateTime.Now;
                                TCGLlList.Add(TCGLModel);
                                
                            }

                            OrderMaterialsFee = 0;

                        }

                        if (!string.IsNullOrEmpty(zhxmbhList))//同步耗材信息
                        {
                            OrderMaterialsFee = GetOrderMaterialsFee(zhxmbhList, out Message);
                            if (Message.state != "200")
                            {
                                msg = Message.message;
                                return false;
                            }

                            if (OrderMaterialsFee != 0)
                            {
                                XttczhxmbmxModel TCZHXMModel1 = new XttczhxmbmxModel();
                                TCZHXMModel1.createtime = DateTime.Now;
                                TCZHXMModel1.sffk = 0;
                                TCZHXMModel1.sfqy = 1;
                                TCZHXMModel1.updatetime = DateTime.Now;
                                TCZHXMModel1.xb = getNysySEX(itemTC.sex);
                                TCZHXMModel1.yybh = jgModel.yybh;
                                TCZHXMModel1.zhxmbh = tcbh + "_HC";
                                TCZHXMModel1.zhxmjg = OrderMaterialsFee.ToString();

                                TCZHXMModel1.zhxmmc = TCModel.tcmc + "_耗材费";
                                TCZHXMModel1.zhxmms = "耗材费";
                                TCZHXMlList.Add(TCZHXMModel1);
                            }
                            if (OrderMaterialsFee != 0)
                            {
                                XttczhxmbModel TCGLModel1 = new XttczhxmbModel();
                                TCGLModel1.yybh = jgModel.yybh;
                                TCGLModel1.dwbh = "0";
                                TCGLModel1.zhxmbh = tcbh + "_HC";
                                TCGLModel1.tcbh = tcbh;
                                TCGLModel1.createtime = DateTime.Now;
                                TCGLlList.Add(TCGLModel1);
                            }

                        }
                        TCModel.jg = TCModel.jg+ OrderMaterialsFee;
                    }
                    new XttcbService().InsertOrUpdate(TClList, TCGLlList, TCZHXMlList);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 获取南医三院套餐列表
        /// </summary>
        /// <returns></returns>
        public static returnData GetClusInfo()
        {
            ReturnMessage Message = new ReturnMessage();

            string access_token = "";

            #region 获取token
            string nysykey = "nysy_access_token";
            if (AspNetCache.Exist(nysykey) == false)
            {
                ReturnMessage tokenModel = Service.HosHelper.NfykdxService.getToke();
                if (tokenModel.state != "200")
                {
                    Message.message = tokenModel.message;
                    Message.state = tokenModel.state;
                    Message.result = "";
                    return null;
                }
                access_token = tokenModel.result.ToString();
                AspNetCache.Insert(nysykey, access_token, DateTime.Now.AddSeconds(7000));
            }
            else
            {
                access_token = AspNetCache.Get<string>(nysykey);
            }
            #endregion

            #region 获取套餐

            returnData TCList =GetClusInfo(access_token, out Message);
            if (Message.state != "200" || TCList == null)
            {
                return null;
            }
            #endregion
            return TCList;
        }

        /// <summary>
        /// 体检人员到检信息反馈
        /// </summary>
        /// <returns></returns>
        public static ReturnMessage BookingOrderInfo(string regNo,string id_card)
        {

            ReturnMessage Message = new ReturnMessage();
            Message = ckeckJG(model);

            if (Message.state != "200")
            {
                Message.state = "402";
                Message.message = "BookingOrderInfo-0机构校验失败";
                Message.result = "";
                return Message;
            }

            string access_token = "";

            #region 获取token
            string nysykey = "nysy_access_token";
            if (AspNetCache.Exist(nysykey) == false)
            {
                ReturnMessage tokenModel = getToke();
                if (tokenModel.state != "200")
                {
                    Message.message = tokenModel.message;
                    Message.state = tokenModel.state;
                    Message.result = "";
                    return Message;
                }
                access_token = tokenModel.result.ToString();
                AspNetCache.Insert(nysykey, access_token, DateTime.Now.AddSeconds(7000));
            }
            else
            {
                access_token = AspNetCache.Get<string>(nysykey);
            }
            #endregion

            #region 获取套餐

            try
            {
                string apiUrl = model.jgjkurl;//请求地址
                apiUrl += "api/External/BookingOrderInfo";
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Bearer " + access_token);

                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("regno", regNo);
                parameters.Add("id_card", id_card);
                string resultStr = Common.WebHelper.PostData_OAuth2_V1(apiUrl, parameters, Headers, "json");
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "BookingOrderInfo-1到检信息反馈失败";
                    Message.result = "";
                    return Message;
                }
                else
                {
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    if (resultObject.success == true)
                    {
                        Message.state = "200";
                        Message.message = "";
                        Message.result = resultObject.returnData;
                        return Message;
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = "BookingOrderInfo-1到检信息反馈失败"+ Convert.ToString(resultObject.returnMsg); 
                        Message.result = "";
                        LogApiHelper.AddErrorLog("BookingOrderInfo-2到检信息反馈失败=" + Convert.ToString(resultObject.returnMsg));
                        return Message;
                    }
                }
            }
            catch (Exception ex)
            {

                Message.state = "407";
                Message.message = "获取套餐信息异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("BookingOrderInfo-3到检信息反馈失败=" + ex.ToString());
                return Message;
            }

            #endregion
          
        }

        /// <summary>
        /// 第三方平台体检人员到检信息反馈 作废
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="regno">第三方平台流水号</param>
        /// <param name="id_card"></param>
        /// <param name="Message"></param>
        /// <returns>state=200 成功</returns>
        public static string getBookingOrderInfo(string access_token,string regno,string id_card, out ReturnMessage Message)
        {
            Message = ckeckJG(model);

            if (Message.state != "200")
            {
                return null;
            }

            try
            {
                string apiUrl = model.jgjkurl;//请求地址
                apiUrl += "api/External/BookingOrderInfo";
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Bearer " + access_token);

                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("regno", regno);
                parameters.Add("id_card", id_card);
                string resultStr = Common.WebHelper.PostData_OAuth2_V1(apiUrl, parameters, Headers, "json");
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "getBookingOrderInfo-1到检信息反馈";
                    Message.result = "";
                    return null;
                }
                else
                {
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    if (resultObject.success == true)
                    {
                        Message.state = "200";
                        Message.message = "";
                        Message.result = "";
                        return Convert.ToString(resultObject.returnData);
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = resultObject.returnMsg;
                        Message.result = "";
                        LogApiHelper.AddErrorLog("getBookingOrderInfo-2到检信息反馈=" + resultObject.msg);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                Message.state = "407";
                Message.message = "获取套餐信息异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("getBookingOrderInfo-3到检信息反馈=" + ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// 第三方平台按项目取消部分费用
        /// </summary>
        /// <param name="regno">第三方平台流水号</param>
        /// <param name="type">C项目退费 R取消项目退费</param>
        /// <param name="ItemList">第三方平台体检人员到检信息反馈] 接口所返回的comb_code（待退费组合码）数组</param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static ReturnMessage CancelPartFeeByItem( string regno, string type,string[] ItemList)
        {
            ReturnMessage Message = ckeckJG(model);

            if (Message.state != "200")
            {
                Message.state = "402";
                Message.message = "CancelPartFeeByItem-0机构校验失败";
                Message.result = "";
                return Message;
            }

            string access_token = "";

            #region 获取token
            string nysykey = "nysy_access_token";
            if (AspNetCache.Exist(nysykey) == false)
            {
                ReturnMessage tokenModel = getToke();
                if (tokenModel.state != "200")
                {
                    Message.message = tokenModel.message;
                    Message.state = tokenModel.state;
                    Message.result = "";
                    return Message;
                }
                access_token = tokenModel.result.ToString();
                AspNetCache.Insert(nysykey, access_token, DateTime.Now.AddSeconds(7000));
            }
            else
            {
                access_token = AspNetCache.Get<string>(nysykey);
            }
            #endregion

            try
            {
                string apiUrl = model.jgjkurl;//请求地址
                apiUrl += "api/External/CancelPartFeeByItem";
                NameValueCollection Headers = new NameValueCollection();
                Headers.Add("Authorization", "Bearer " + access_token);

                //NameValueCollection parameters = new NameValueCollection();
                //parameters.Add("regno", regno);
                //parameters.Add("type", type);
                //parameters.Add("ItemList", ItemList.ToString());

                string jsonStr = "{'regno':'" + regno + "','type':'" + type + "','ItemList':" + JsonConvert.SerializeObject(ItemList) +"}";
                LogApiHelper.AddLog("CancelPartFeeByItem jsonStr=" + jsonStr);
                string resultStr = Common.WebHelper.PostData_OAuth2_V3(apiUrl, jsonStr, Headers);
                if (string.IsNullOrEmpty(resultStr))
                {
                    Message.state = "403";
                    Message.message = "CancelPartFeeByItem-1取消部分费用失败";
                    Message.result = "";
                    return Message;
                }
                else
                {
                    dynamic resultObject = JsonConvert.DeserializeObject<dynamic>(resultStr);
                    if (resultObject.success == true)
                    {
                        returnData TCList = JsonConvert.DeserializeObject<returnData>(Convert.ToString(resultObject.returnData));
                        Message.state = "200";
                        Message.message = "";
                        Message.result = resultObject.returnData;
                        return Message;
                    }
                    else
                    {
                        Message.state = "406";
                        Message.message = resultObject.returnMsg;
                        Message.result = "";
                        LogApiHelper.AddErrorLog("CancelPartFeeByItem-2取消部分费用失败=" + resultObject.msg);
                        return Message;
                    }
                }
            }
            catch (Exception ex)
            {

                Message.state = "407";
                Message.message = "获取套餐信息异常：" + ex.ToString();
                Message.result = "";
                LogApiHelper.AddErrorLog("CancelPartFeeByItem-3取消部分费用失败=" + ex.ToString());
                return Message;
            }

        }

        /// <summary>
        /// 识别性别
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static int getNysySEX(string sex)
        {
            int xb = 2;
            switch (sex)
            {
                case "1":
                    xb = 1;
                    break;
                case "2":
                    xb = 0;
                    break;
                case "3":
                    xb = 2;
                    break;
                default:
                    break;
            }
            return xb;
        }
    }

}
