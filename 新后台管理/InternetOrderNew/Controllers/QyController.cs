using Common;
using Model;
using Model.Dto;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Service;
using Service.HosHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetOrder.Controllers
{
    /// <summary>
    /// 企业
    /// </summary>
    public class QyController : BaseController
    {
        public ActionResult QyJbxx(int page = 1,string sfqy="",string qybh="",string qymc= "")
        {         
            int count = 0;
            Hashtable ht = new Hashtable();
            ht.Add("qybh", qybh);
            ht.Add("qymc", qymc);
            ht.Add("sfqy", sfqy);
            ht.Add("yybh", GLYadmin.YYID);
            List<QyJbxxModel> list = new QyjbxxService().GetqyjbxxList(ht, page, PageSize, ref count);

            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("QyJbxxPart", list);
            }
            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID);
            ViewBag.Zdbz = jgModel.zdbz;
            return View(list);
        }
        /// <summary>
        /// 更新企业联系人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lxr"></param>
        /// <param name="lxdh"></param>
        /// <returns></returns>
        public JsonResult UpdateQyLxr(int id, string lxr, string lxdh,int sfxstcje,string zh,string mm)
        {
            int i = new QyjbxxService().UpdateQy(lxr, lxdh, id, sfxstcje,zh,mm);
            if (i > 0)
            {
                return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false, msg = "更新失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 企业开关      
        /// <param name="yybh"></param>
        /// <param name="bh"></param>
        /// <param name="sfqy"></param>
        /// <returns></returns>
        public JsonResult QySfqy(int id, int sfqy)
        {
            new QyjbxxService().UpdateSfqy(id, sfqy);

            return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 企业批量同步
        /// </summary>
        /// <returns></returns>
        public JsonResult QyTb()
        {
            //如果中间库成功，再调用医院接口，分3种情况，手动，全自动，半自动
            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID);
            if (jgModel.iskkservice == 1)
            {
                string msg = "";
                //if (Service.HosHelper.PublicKkService.QyjbxxTb(jgModel, ref msg))
                //{
                    return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(new { flag = false, msg = msg }, JsonRequestBehavior.AllowGet);
                //}
            }
            else
            {
                //定制开发
                return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 企业套餐
        /// </summary>
        /// <returns></returns>
        public ActionResult Tc(int page = 1, string sfqy = "", string qdbh = "", string tcmc = "")
        {
            int count = 0;
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            ht.Add("tcmc", tcmc);
            ht.Add("sfqy", sfqy);
            ht.Add("qdbh", qdbh);
            ht.Add("tclx", 1);
            List<XttcbModel> list = new XttcbService().GetYyQyTcNew(ht, page, PageSize, ref count);

            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("TcPart", list);
            }
            else
            {
                List<KeyValueModel> liQd = new QyjbxxService().GetqyList(GLYadmin.YYID);
                ViewBag.QdList = liQd;
            }
            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID);
            ViewBag.Zdbz = jgModel.zdbz;
            return View(list);
        }
        /// <summary>
        /// 套餐组合项目查看
        /// </summary>
        /// <param name="jgid"></param>
        /// <param name="tcbh"></param>
        /// <returns></returns>
        public ActionResult QyTcZhxmIndex(string yybh, string tcbh = "")
        {
            List<XtZhxmbModel> list = new XttcbService().GetTcZhxm(yybh, tcbh);
            return PartialView("TcDetailPart", list);
        }
        /// <summary>
        /// 企业套餐同步
        /// </summary>
        /// <param name="qdbh"></param>
        /// <returns></returns>
        public JsonResult QyTcTb(string dwbh)
        {
            //如果中间库成功，再调用医院接口，分3种情况，手动，全自动，半自动
            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID);
            if (jgModel.iskkservice == 1)
            {
                string msg = "";             
                //if (Service.HosHelper.PublicKkService.QyTcTb(jgModel, dwbh, 1,0, ref msg))
                //{
                    return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(new { flag = false, msg = msg }, JsonRequestBehavior.AllowGet);
                //}
            }
            else
            {
                //定制开发
                return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 更新价格
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jg"></param>
        /// <param name="jsj"></param>
        /// <returns></returns>
        public JsonResult UpdateQyTc(int id, decimal jg, decimal jsj, int sfxsjg)
        {

            int i = new XttcbService().UpdateTcJgJsj(id, jsj, jg, sfxsjg);
            if (i > 0)
            {
                return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false, msg = "更新失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 更新员工密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mm"></param>
        /// <returns></returns>
        public JsonResult UpdateQyrymm(int id,string mm)
        {
            int i= new QyygjbxxService().UpdateQyygMm(id, mm);
            if (i > 0)
            {
                return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false, msg = "更新失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteQyry(int id)
        {
            int i = new QyygjbxxService().DeleteQyyg(id);
            if (i > 0)
            {
                return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 企业人员List
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Qyrylist(int page = 1, string dwbh = "", string dh = "",string xm="")
        {
            int count = 0;
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            ht.Add("dwbh", dwbh);
            ht.Add("dh", dh);
            ht.Add("xm", xm);
            List<QyygxxModel> list = new QyygjbxxService().GetQyygjbxxList(ht, page, PageSize, ref count);

            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("QyryPart", list);
            }
            else
            {
                List<KeyValueModel> liQy = new QyjbxxService().GetqyList(GLYadmin.YYID);
                ViewBag.QyList = liQy;
            }
            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID);
            ViewBag.Zdbz = jgModel.zdbz;
            return View(list);
        }
        /// <summary>
        /// 同步企业人员
        /// </summary>
        /// <param name="dwbh"></param>
        /// <returns></returns>
        public JsonResult TbQyry(string dwbh)
        {

            //如果中间库成功，再调用医院接口，分3种情况，手动，全自动，半自动
            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID);
            if (jgModel.iskkservice == 1)
            {
                string msg = "";
                //if (Service.HosHelper.PublicKkService.QyryTb(jgModel, dwbh, ref msg))
                //{
                    return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(new { flag = false, msg = msg }, JsonRequestBehavior.AllowGet);
                //}
            }
            else
            {
                //定制开发
                return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 企业团检订单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sfqy"></param>
        /// <param name="qdbh"></param>
        /// <param name="tcmc"></param>
        /// <returns></returns>
        public ActionResult Order(int page = 1, string dwbh = "", string ddstart = "", string ddend = "", string yystart = "", string yyend = "", string xm = "", string dh = "", string tczt = "")
        {


            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID); ;
            ViewBag.Jg = jgModel;

            int count = 0;
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            ht.Add("ddly", "1");
            ht.Add("dwbh", dwbh);
            ht.Add("ddstart", ddstart);
            ht.Add("ddend", ddend);
            ht.Add("yystart", yystart);
            ht.Add("yyend", yyend);
            ht.Add("dh", dh);
            ht.Add("xm", xm);
            ht.Add("tczt", tczt);
            List<DdjbxxModel> list = new Service.DdyymxbService().OrderYyQyList(ht, page, PageSize, ref count);
            int recordCount = count;//总记录数

            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("OrderPart", list);
            }
            else
            {
                List<KeyValueModel> liQy = new QyjbxxService().GetqyList(GLYadmin.YYID);
                ViewBag.QyList = liQy;
            }
            return View(list);
        }
        /// <summary>
        /// 企业团检订单导出
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dwbh"></param>
        /// <param name="ddstart"></param>
        /// <param name="ddend"></param>
        /// <param name="yystart"></param>
        /// <param name="yyend"></param>
        /// <param name="xm"></param>
        /// <param name="dh"></param>
        /// <param name="tczt"></param>
        /// <returns></returns>
        public FileResult ExportExcel(int page = 1, string dwbh = "", string ddstart = "", string ddend = "", string yystart = "", string yyend = "", string xm = "", string dh = "", string tczt = "")
        {
          
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            ht.Add("ddly", "1");
            ht.Add("dwbh", dwbh);
            ht.Add("ddstart", ddstart);
            ht.Add("ddend", ddend);
            ht.Add("yystart", yystart);
            ht.Add("yyend", yyend);
            ht.Add("dh", dh);
            ht.Add("xm", xm);
            ht.Add("tczt", tczt);           
            DataTable dtExport = new Service.DdyymxbService().ExportOrderYyQyList(ht);

            if (dtExport.Rows.Count > 0)
            {
                dtExport.TableName = "报表数据";
                MemoryStream ms = Common.ExcelTool.RenderToExcel(dtExport);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyy-MM-dd") + "企业订单.xls");
            }
            else
            {
                //context.Response.Write("数据为空不能导出");
                return null;
            }
        }
        /// <summary>
        /// 企业订单详情
        /// </summary>
        /// <param name="ddbh"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(string ddbh)
        {
            DdjbxxModel model = new Service.DdyymxbService().GetBydsfddxxQy(ddbh);
            return View(model);
        }
        /// <summary>
        /// 企业预约管理
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dwbh"></param>
        /// <param name="pqstart"></param>
        /// <param name="pqend"></param>
        /// <returns></returns>
        public ActionResult YyGl(int page = 1, string dwbh = "", string pqstart = "", string pqend = "")
        {
            ViewBag.website=commonModel.website; 
            QyyyService qs = new QyyyService();
            int count = 0;          
            List<QyyyModel> list = qs.GetQyygjbxxList(GLYadmin.YYID, dwbh,pqstart,pqend, page, PageSize, ref count);
            int recordCount = count;//总记录数

            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("YyGlPart", list);
            }
            else
            {
                List<KeyValueModel> liQy = new QyjbxxService().GetqyList(GLYadmin.YYID);
                ViewBag.QyList = liQy;
            }
            return View(list);           
        }
        /// <summary>
        /// 预约详情
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="yykssj"></param>
        /// <param name="yyjssj"></param>
        /// <param name="sfyy"></param>
        /// <param name="dept"></param>
        /// <param name="xm"></param>
        /// <param name="dh"></param>
        /// <param name="sfzh"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Yyxq(string dwbh,string yykssj = "",string yyjssj = "",string sfyy= "",string dept="",string xm="",string dh="",string sfzh="", int page=1)
        {
            int count = 0;//总记录数
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            ht.Add("dwbh", dwbh);
            ht.Add("dh", dh);
            ht.Add("xm", xm);
            ht.Add("sfyy", sfyy);
            ht.Add("sfzh", sfzh);
            ht.Add("dept", dept);
            ht.Add("yykssj", yykssj);
            ht.Add("yyjssj", yyjssj);
            List<QyygxxModel> list = new QyygjbxxService().GetQyygYyxqList(ht, page, PageSize, ref count);


            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("YyxqPart", list);
            }
            else
            {
                List<QyJbxxModel> li = new QyjbxxService().GetqyjbxxdeptList(GLYadmin.YYID, dwbh);
                List<KeyValueModel> lidept = new List<KeyValueModel>();
                foreach (QyJbxxModel model in li)
                {
                    lidept.Add(new KeyValueModel() { key = model.bh, values = model.mc });
                }
                ViewBag.deptList = lidept;
                ViewBag.dwbh = dwbh;
                ViewBag.dwmc = new QyjbxxService().GetqyjbxxOne(GLYadmin.YYID, dwbh).mc;

                PqQyszModel modelpq = new PqQyszService().GetModelByRq(GLYadmin.YYID, dwbh);
                List<Sjd> liSjd = new List<Sjd>();
                if (modelpq != null && !string.IsNullOrEmpty(modelpq.sjd))
                {
                    liSjd = JsonConvert.DeserializeObject<List<Sjd>>(modelpq.sjd);
                }
                else
                {
                    liSjd.Add(new Sjd() { jssj = "08:30", kssj = "08:00" });
                }
                ViewBag.liSjd = liSjd;
            }

            return View(list);
        }
      
        /// <summary>
        /// 预约详情,导出
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public FileResult YyxqExportExcel(string dwbh, string yykssj = "", string yyjssj = "", string sfyy = "", string dept = "", string xm = "", string dh = "", string sfzh = "")
        {
            ReportQdService sr = new ReportQdService();
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            ht.Add("dwbh", dwbh);
            ht.Add("dh", dh);
            ht.Add("xm", xm);
            ht.Add("sfyy", sfyy);
            ht.Add("sfzh", sfzh);
            ht.Add("dept", dept);
            ht.Add("yykssj", yykssj);
            ht.Add("yyjssj", yyjssj);
            DataTable dtExport = new QyygjbxxService().GetQyygYyxqListExcel(ht);
            if (dtExport.Rows.Count > 0)
            {
                dtExport.TableName = "报表数据";
                MemoryStream ms = Common.ExcelTool.RenderToExcel(dtExport);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "订单名细.xls");
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 团检代预约
        /// </summary>
        /// <param name="ydjh"></param>
        /// <param name="tjrq"></param>
        /// <param name="kssj"></param>
        /// <param name="jssj"></param>
        /// <returns></returns>
        public JsonResult Dyy(string ydjh, string tjrq, string kssj)
        {
            try
            {
                DdJbxxService ddService = new DdJbxxService();
                string ddbh = ddService.GetDdbh();

                QyygxxModel ygModel = new QyygjbxxService().GetModel(GLYadmin.YYID, ydjh);


                XtYhbModel model = new XtYhbModel { dh = ygModel.dh, xm = ygModel.xm, mm = ygModel.mm, yybh = GLYadmin.YYID };
                string zh = ygModel.ygzh;
                XtYhbService yhbServcie = new XtYhbService();
                model = yhbServcie.GetModel(model);
                if (model == null)
                {
                    model = new XtYhbModel() { dh = ygModel.dh, xm = ygModel.xm, mm = ygModel.mm, yybh = GLYadmin.YYID, zh = yhbServcie.GetZH() };
                    yhbServcie.Add(model);
                    zh = model.zh;
                }


                DdjbxxModel ddModel = new DdjbxxModel();
                ddModel.ddbh = ddbh;
                ddModel.ddly = 1;
                ddModel.dsfdd = ygModel.ydjh;
                ddModel.yybh = ygModel.yybh;
                ddModel.xb = ygModel.xb;
                ddModel.xm = ygModel.xm;
                ddModel.zffs = "";
                ddModel.sfjx = 0;
                ddModel.jxlist = "";
                ddModel.dwbh = ygModel.dwbh;
                ddModel.dwmc = ygModel.dwmc;
                ddModel.tcid = ygModel.tcbh;
                ddModel.tcjg = ygModel.jg;
                ddModel.jxbjg = 0;
                ddModel.ddze = ddModel.tcjg + ddModel.jxbjg;
                ddModel.tcmc = ygModel.tcmc;
                ddModel.dh = ygModel.dh;

                ddModel.hz = ygModel.hz;
                ddModel.zjlx = 1;
                ddModel.zjhm = ygModel.sfzh;
                ddModel.yykssj = Convert.ToDateTime(string.Format("{0} {1}", tjrq, kssj.Split('-')[0]));
                ddModel.yyjssj = Convert.ToDateTime(string.Format("{0} {1}", tjrq, kssj.Split('-')[1]));


                ddModel.ddzt = 3;
                ddModel.jsbz = 0;
                ddModel.ygzh = zh;
                ddModel.csrq = ygModel.csrq;
                var zhxmList = new XtzhxmService().GetListByTcbh(ygModel.yybh, ygModel.tcbh);//套餐组合项目
                var ddZhxmList = new List<DdZhxmModel>();
                zhxmList.ForEach(x =>
                {
                    ddZhxmList.Add(new DdZhxmModel
                    {
                        ddbh = ddbh,
                        jg = x.zhxmjg,
                        sfjx = 0,
                        zhxmbh = x.zhxmbh,
                        zhxmmc = x.zhxmmc
                    });
                });
                ygModel.sfyy = 1;
                ddService.ConfirmOrder(ddModel, ddZhxmList, ygModel);
                new QyygjbxxService().UpdateSfyy(ddModel.yybh, ygModel.ydjh);
                //如果中间库成功，再调用医院接口，分3种情况，手动，全自动，半自动
                XtJgbModel jgModel = new Service.XtjgbService().GetJg(ddModel.yybh);//预约到医院
                if (jgModel.iskkservice == 1)
                {
                    string msg = "";
                    //bool res = PublicKkService.TjDdyy(ddModel.ddbh, 1, ref msg);
                }
                return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Log.WriteLog(ex.ToString()); //记录日志信息  
                return Json(new { flag = false, msg = "预约失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 取消预约
        /// </summary>
        /// <param name="ydjh"></param>
        /// <returns></returns>
        public JsonResult Cancelyy(string ydjh)
        {

            DdJbxxService ddService = new DdJbxxService();
            int i=ddService.DeleteTjOrder(GLYadmin.YYID, ydjh);
            if (i > 0)
            {
                return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { flag = false, msg = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 预约详情
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="yykssj"></param>
        /// <param name="yyjssj"></param>
        /// <param name="sfyy"></param>
        /// <param name="dept"></param>
        /// <param name="xm"></param>
        /// <param name="dh"></param>
        /// <param name="sfzh"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult YyxqAll(string dwbh="", string yykssj = "", string yyjssj = "", string sfyy = "", string dept = "", string xm = "", string dh = "", string sfzh = "",int page = 1)
        {
            int count = 0;//总记录数
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            ht.Add("dwbh", dwbh);
            ht.Add("dh", dh);
            ht.Add("xm", xm);
            ht.Add("sfyy", sfyy);
            ht.Add("sfzh", sfzh);
            ht.Add("dept", dept);
            ht.Add("yykssj", yykssj);
            ht.Add("yyjssj", yyjssj);
            List<QyygxxModel> list = new QyygjbxxService().GetQyygYyxqList(ht, page, PageSize, ref count);


            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("YyxqAllPart", list);
            }
            else
            {
                List<KeyValueModel> liQy = new QyjbxxService().GetqyList(GLYadmin.YYID);
                ViewBag.QyList = liQy;

                List<QyJbxxModel> li = new QyjbxxService().GetqyjbxxdeptList(GLYadmin.YYID, dwbh);
                List<KeyValueModel> lidept = new List<KeyValueModel>();
                foreach (QyJbxxModel model in li)
                {
                    lidept.Add(new KeyValueModel() { key = model.bh, values = model.mc });
                }
                ViewBag.deptList = lidept;
                ViewBag.dwbh = dwbh;

                PqQyszModel modelpq = new PqQyszService().GetModelByRq(GLYadmin.YYID, dwbh);
                List<Sjd> liSjd = new List<Sjd>();
                if (modelpq != null && !string.IsNullOrEmpty(modelpq.sjd))
                {
                    liSjd = JsonConvert.DeserializeObject<List<Sjd>>(modelpq.sjd);
                }
                else
                {
                    liSjd.Add(new Sjd() { jssj = "08:30", kssj = "08:00" });
                }
                ViewBag.liSjd = liSjd;
            }

            return View(list);
        }


        #region 企业导入

        public JsonResult ImportQy()
        {
            try
            {
                //校验文件是否存在 保存文件到服务器 校验文件类型 上传文件格式
                string excelPath = string.Empty;
                string msg = string.Empty;
                HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                if (hfc.Count > 0)
                {
                    string ext = Path.GetExtension(hfc[0].FileName);//文件后缀
                    excelPath = Server.MapPath("~/Content/customer/" + DateTime.Now.ToString("yyyy-MM") + "/");
                    if (!Directory.Exists(excelPath))
                    {
                        Directory.CreateDirectory(excelPath);
                    }

                    string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                    excelPath = excelPath + filename;
                    hfc[0].SaveAs(excelPath);
                }
                else
                {
                    return Json(new ReturnModel { Code = 201, Msg = "请选择excel文件再提交上传" }, JsonRequestBehavior.AllowGet);
                }

              
                List<QyJbxxModel> li = new List<QyJbxxModel>();
                ExcelToList(excelPath, null, out msg, ref li);
       
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    return Json(new ReturnModel { Code = 201, Msg = $"{msg}" }, JsonRequestBehavior.AllowGet);
                }
                new QyjbxxService().InsertOrUpdate(li);
                return Json(new ReturnModel { Code = 200, Msg = "导入成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return Json(new ReturnModel { Code = 201, Msg = "导入失败" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 将excel中的数据导入到list中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        private void ExcelToList(string fileName, string sheetName, out string msg, ref List<QyJbxxModel> li)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            int startRow = 1;
            msg = string.Empty;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                    int col = cellCount;//总列数
                    if (!ValidateTemplate(firstRow, col))
                    {
                        msg = "模板错误，请下载并使用正确的模板";
                        return;
                    }

                    //最后一行的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null
                        QyJbxxModel qyModel = new QyJbxxModel();


                        qyModel.yybh = GLYadmin.YYID;
                        qyModel.sfqd = 0;
                        qyModel.bh = GetCellValue(row, 0).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(qyModel.bh))
                        {
                            msg = $"第{i}行企业编号不能为空";
                            return;
                        }
                        qyModel.mc = GetCellValue(row, 1).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(qyModel.mc))
                        {
                            msg = $"第{i}行企业名称不能为空";
                            return;
                        }
                        qyModel.isdept = 0;
                        qyModel.dwfzr = GetCellValue(row, 2).ToString().Trim();
                        qyModel.lxdh = GetCellValue(row, 3).ToString().Trim();
                        qyModel.sfxstcje = 1;                     
                        li.Add(qyModel);
                    }
                }

                if (li.Count.Equals(0))
                {
                    msg = "模板不能为空";
                }
            }
            catch (Exception ex)
            {
                msg = "读取模板失败";
                Log.WriteLog(ex.Message);
            }
        }

        private bool ValidateTemplate(IRow firstRow, int totalCol)
        {
            List<string> lstColName = new List<string>();
            lstColName.Add("企业编号");
            lstColName.Add("企业名称");
            lstColName.Add("联系人");
            lstColName.Add("联系电话");         
            try
            {
                for (int i = 0; i < totalCol; i++)
                {
                    ICell cell = firstRow.GetCell(i);
                    if (cell == null)
                    {
                        return false;
                    }
                    string cellValue = cell.StringCellValue;
                    if (cellValue != lstColName[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return false;
            }
        }

        #endregion
      
        #region 套餐导入

        public JsonResult ImportTc()
        {
            try
            {
                //校验文件是否存在 保存文件到服务器 校验文件类型 上传文件格式
                string excelPath = string.Empty;
                string msg = string.Empty;
                HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                if (hfc.Count > 0)
                {
                    string ext = Path.GetExtension(hfc[0].FileName);//文件后缀
                    excelPath = Server.MapPath("~/Content/customer/" + DateTime.Now.ToString("yyyy-MM") + "/");
                    if (!Directory.Exists(excelPath))
                    {
                        Directory.CreateDirectory(excelPath);
                    }

                    string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                    excelPath = excelPath + filename;
                    hfc[0].SaveAs(excelPath);
                }
                else
                {
                    return Json(new ReturnModel { Code = 201, Msg = "请选择excel文件再提交上传" }, JsonRequestBehavior.AllowGet);
                }

                List<XttcbModel> li = new List<XttcbModel>();
                List<XttczhxmbModel> lizhxm = new List<XttczhxmbModel>();
                ExcelToListTc(excelPath, null, out msg, ref li, ref lizhxm);

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    return Json(new ReturnModel { Code = 201, Msg = $"{msg}" }, JsonRequestBehavior.AllowGet);
                }
                new XttcbService().InsertOrUpdate(li, lizhxm);

                return Json(new ReturnModel { Code = 200, Msg = "导入成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return Json(new ReturnModel { Code = 201, Msg = "导入失败" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 将excel中的数据导入到list中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public void ExcelToListTc(string fileName, string sheetName, out string msg, ref List<XttcbModel> li, ref List<XttczhxmbModel> lizhxm)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            int startRow = 1;
            msg = string.Empty;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                    int col = cellCount;//总列数
                    if (!ValidateTemplateTc(firstRow, col))
                    {
                        msg = "模板错误，请下载并使用正确的模板";
                        return;
                    }

                    //最后一行的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null
                        XttcbModel tcModel = new XttcbModel();


                        tcModel.yybh = GLYadmin.YYID;
                        tcModel.tclx =1;
                        tcModel.tcbh = GetCellValue(row, 0).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.tcbh))
                        {
                            msg = $"第{i}行套餐编号不能为空";
                            return;
                        }
                        tcModel.tcmc = GetCellValue(row, 1).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.tcmc))
                        {
                            msg = $"第{i}行套餐名称不能为空";
                            return;
                        }
                        tcModel.dwbh = GetCellValue(row, 2).ToString().Trim();

                        if (string.IsNullOrWhiteSpace(tcModel.dwbh))
                        {
                            msg = $"第{i}行单位编号不能为空";
                            return;
                        }
                        tcModel.dsfbzid = int.Parse(tcModel.dwbh);
                        tcModel.dwmc = GetCellValue(row, 3).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.dwmc))
                        {
                            msg = $"第{i}行单位名称不能为空";
                            return;
                        }
                        tcModel.jg = Convert.ToDecimal(GetCellValue(row, 4));
                        string sex = GetCellValue(row, 5).ToString();
                        switch (sex)
                        {
                            case "男":
                                tcModel.xb = 1;
                                break;
                            case "女":
                                tcModel.xb = 0;
                                break;
                            case "通用":
                                tcModel.xb = 2;
                                break;
                            default:
                                tcModel.xb = -1;
                                break;
                        }

                        if (tcModel.xb.Equals(-1))
                        {
                            msg = $"第{i}行性别不正确(仅支持:男,女,通用)";
                            return;
                        }
                        string sfqy = GetCellValue(row, 6).ToString();
                        switch (sfqy)
                        {
                            case "是":
                                tcModel.sfqy = 1;
                                break;
                            case "否":
                                tcModel.sfqy = 0;
                                break;
                            default:
                                tcModel.sfqy = -1;
                                break;
                        }
                        if (tcModel.sfqy.Equals(-1))
                        {
                            msg = $"第{i}行是否启用不正确(仅支持:是,否)";
                            return;
                        }
                        string hyzt = GetCellValue(row, 7).ToString();
                        switch (hyzt)
                        {
                            case "已婚":
                                tcModel.hz = 1;
                                break;
                            case "未婚":
                                tcModel.hz = 0;
                                break;
                            case "不限":
                                tcModel.hz = 2;
                                break;
                            default:
                                tcModel.hz = -1;
                                break;
                        }
                        if (tcModel.hz.Equals(-1))
                        {
                            msg = $"第{i}行婚姻状态不正确(仅支持:已婚,未婚,不限)";
                            return;
                        }
                        string zhxmbh = GetCellValue(row, 8).ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(zhxmbh))
                        {
                            try
                            {
                                List<string> liZhxmbh = zhxmbh.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                foreach (var strZhxm in liZhxmbh)
                                {
                                    XttczhxmbModel tczhxmmodel = new XttczhxmbModel();
                                    tczhxmmodel.zhxmbh = strZhxm.Trim();
                                    tczhxmmodel.yybh = GLYadmin.YYID;
                                    tczhxmmodel.dwbh = tcModel.dwbh;
                                    tczhxmmodel.tcbh = tcModel.tcbh;
                                    tczhxmmodel.createtime = DateTime.Now;
                                    lizhxm.Add(tczhxmmodel);
                                }
                            }
                            catch (Exception e)
                            {
                                msg = $"第{i}行组合项目编号不正确(以逗号隔开)";
                                return;
                            }

                        }

                        li.Add(tcModel);
                    }
                }

                if (li.Count.Equals(0))
                {
                    msg = "模板不能为空";
                }
            }
            catch (Exception ex)
            {
                msg = "读取模板失败";
                Log.WriteLog(ex.Message);
            }
        }

        private bool ValidateTemplateTc(IRow firstRow, int totalCol)
        {
            List<string> lstColName = new List<string>();
            lstColName.Add("套餐编号(软件里的编号)");
            lstColName.Add("套餐名称");
            lstColName.Add("企业编号");
            lstColName.Add("企业名称");
            lstColName.Add("价格");
            lstColName.Add("性别(仅支持:男,女,通用)");
            lstColName.Add("是否启用(仅支持:是,否)");
            lstColName.Add("婚姻状态(仅支持:已婚,未婚,不限)");
            lstColName.Add("组合项目编号,用逗号隔开");
            try
            {
                for (int i = 0; i < totalCol; i++)
                {
                    ICell cell = firstRow.GetCell(i);
                    if (cell == null)
                    {
                        return false;
                    }
                    string cellValue = cell.StringCellValue;
                    if (cellValue != lstColName[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return false;
            }
        }

        #endregion

        #region 企业人员导入

        public JsonResult ImportQyyg()
        {
            try
            {
                //校验文件是否存在 保存文件到服务器 校验文件类型 上传文件格式
                string excelPath = string.Empty;
                string msg = string.Empty;
                HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                if (hfc.Count > 0)
                {
                    string ext = Path.GetExtension(hfc[0].FileName);//文件后缀
                    excelPath = Server.MapPath("~/Content/customer/" + DateTime.Now.ToString("yyyy-MM") + "/");
                    if (!Directory.Exists(excelPath))
                    {
                        Directory.CreateDirectory(excelPath);
                    }

                    string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                    excelPath = excelPath + filename;
                    hfc[0].SaveAs(excelPath);
                }
                else
                {
                    return Json(new ReturnModel { Code = 201, Msg = "请选择excel文件再提交上传" }, JsonRequestBehavior.AllowGet);
                }
               
                List<QyygxxModel> li = new List<QyygxxModel>();              
                ExcelToListQyyg(excelPath, null, out msg, ref li);

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    return Json(new ReturnModel { Code = 201, Msg = $"{msg}" }, JsonRequestBehavior.AllowGet);
                }
                new QyygjbxxService().InsertOrUpdate(li);

                return Json(new ReturnModel { Code = 200, Msg = "导入成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return Json(new ReturnModel { Code = 201, Msg = "导入失败" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 将excel中的数据导入到list中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public void ExcelToListQyyg(string fileName, string sheetName, out string msg, ref List<QyygxxModel> li)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            int startRow = 1;
            msg = string.Empty;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                    int col = cellCount;//总列数
                    if (!ValidateTemplateQyyg(firstRow, col))
                    {
                        msg = "模板错误，请下载并使用正确的模板";
                        return;
                    }

                    //最后一行的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null
                        QyygxxModel tcModel = new QyygxxModel();


                        tcModel.yybh = GLYadmin.YYID;                      
                        tcModel.ydjh = GetCellValue(row, 0).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.ydjh))
                        {
                            msg = $"第{i}行号软件预登记号不能为空";
                            return;
                        }
                        tcModel.tcbh = GetCellValue(row, 1).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.tcbh))
                        {
                            msg = $"第{i}行套餐编号不能为空";
                            return;
                        }

                        tcModel.tcmc = GetCellValue(row, 2).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.tcmc))
                        {
                            msg = $"第{i}行套餐名称不能为空";
                            return;
                        }

                        tcModel.dwbh = GetCellValue(row, 3).ToString().Trim();

                        if (string.IsNullOrWhiteSpace(tcModel.dwbh))
                        {
                            msg = $"第{i}行单位编号不能为空";
                            return;
                        }
                      
                        tcModel.dwmc = GetCellValue(row, 4).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.dwmc))
                        {
                            msg = $"第{i}行单位名称不能为空";
                            return;
                        }

                        tcModel.xm = GetCellValue(row, 5).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.xm))
                        {
                            msg = $"第{i}行姓名不能为空";
                            return;
                        }
                        tcModel.sfzh = GetCellValue(row, 6).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.sfzh))
                        {
                            msg = $"第{i}行身份证不能为空";
                            return;
                        }
                        tcModel.dh = GetCellValue(row, 7).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(tcModel.dh))
                        {
                            msg = $"第{i}行电话不能为空";
                            return;
                        }
                        string vip = GetCellValue(row, 8).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(vip))
                        {
                            msg = $"第{i}行vip为空";
                            return;
                        }
                        if(vip=="是")
                        {
                            tcModel.sfvip = 1;
                        }
                        else
                        {
                            tcModel.sfvip = 0;
                        }
                        li.Add(tcModel);
                    }
                }

                if (li.Count.Equals(0))
                {
                    msg = "模板不能为空";
                }
            }
            catch (Exception ex)
            {
                msg = "读取模板失败";
                Log.WriteLog(ex.Message);
            }
        }

        private bool ValidateTemplateQyyg(IRow firstRow, int totalCol)
        {
            List<string> lstColName = new List<string>();
            lstColName.Add("软件预登记号");
            lstColName.Add("套餐编号");
            lstColName.Add("套餐名称");
            lstColName.Add("单位编号");
            lstColName.Add("单位名称");
            lstColName.Add("姓名");
            lstColName.Add("身份证");
            lstColName.Add("电话号");
            lstColName.Add("是否vip（是，否)");       
            try
            {
                for (int i = 0; i < totalCol; i++)
                {
                    ICell cell = firstRow.GetCell(i);
                    if (cell == null)
                    {
                        return false;
                    }
                    string cellValue = cell.StringCellValue;
                    if (cellValue != lstColName[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return false;
            }
        }

        #endregion
    }
}