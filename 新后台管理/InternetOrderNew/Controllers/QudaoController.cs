using Common;
using Model;
using Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;
namespace InternetOrder.Controllers
{
    /// <summary>
    /// 渠道
    /// </summary>
    public class QudaoController : BaseController
    {
        XttcbService _xttcbService;
        public QudaoController()
        {
            _xttcbService = new XttcbService();
        }
        /// <summary>
        /// 基本
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string mc = "")
        {
            List<QuDaoJbxx> li = new Service.QuDaoJbxxService().GetYyQudao(GLYadmin.YYID, mc);
            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexPart", li);
            }
            return View(li);
        }
        /// <summary>
        /// 更新渠道状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sfqy"></param>
        /// <returns></returns>
        public JsonResult UpdateQudaoSfqy(int id, int sfqy)
        {
            int i = new QuDaoJbxxService().UpdateYyQudaoStatus(sfqy, id);
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
        /// 更新渠道联系人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lxr"></param>
        /// <param name="lxdh"></param>
        /// <returns></returns>
        public JsonResult UpdateQudaoLxr(int id, string lxr, string lxdh)
        {
            int i = new QuDaoJbxxService().UpdateYyQudao(lxr, lxdh, id);
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
        /// 渠道套餐
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
            ht.Add("tclx", "2");
            List<XttcbModel> list = new XttcbService().GetYyQdTcNew(ht, page, PageSize, ref count);

            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("TcPart", list);
            }
            else
            {
                List<QuDaoJbxx> liQd = new QuDaoJbxxService().GetYyQudao(GLYadmin.YYID, "");

                List<KeyValueModel> li = new List<KeyValueModel>();
                foreach (QuDaoJbxx model in liQd)
                {
                    li.Add(new KeyValueModel() { key = model.Bh, values = model.Mc });
                }

                ViewBag.QdList = li;
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
        public ActionResult QdTcZhxmIndex(string yybh, string tcbh = "")
        {
            List<XtZhxmbModel> list = new XttcbService().GetTcZhxm(yybh, tcbh);
            return PartialView("TcDetailPart", list);
        }


        /// <summary>
        /// 更新价格
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jg"></param>
        /// <param name="jsj"></param>
        /// <returns></returns>
        public JsonResult UpdateQudaoTc(int id, decimal jg, decimal jsj, int sfxsjg)
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
        /// 渠道订单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sfqy"></param>
        /// <param name="qdbh"></param>
        /// <param name="tcmc"></param>
        /// <returns></returns>
        public ActionResult Order(int page = 1, string qdbh = "", string ddstart = "", string ddend = "", string yystart = "", string yyend = "", string xm = "", string dh = "", string tczt = "", string tcmc = "")
        {


            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID); ;
            ViewBag.Jg = jgModel;

            int count = 0;
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            ht.Add("ddly", "0");
            ht.Add("qdbh", qdbh);
            ht.Add("ddstart", ddstart);
            ht.Add("ddend", ddend);
            ht.Add("yystart", yystart);
            ht.Add("yyend", yyend);
            ht.Add("dh", dh);
            ht.Add("xm", xm);
            ht.Add("tczt", tczt);
            ht.Add("tcmc", tcmc);
            List<DdjbxxModel> list = new Service.DdyymxbService().OrderYyQdList(ht, page, PageSize, ref count);
            int recordCount = count;//总记录数

            ViewBag.Zdbz = jgModel.zdbz;
            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("OrderPart", list);
            }
            else
            {
                List<QuDaoJbxx> liQd = new QuDaoJbxxService().GetYyQudao(GLYadmin.YYID, "");
                List<KeyValueModel> li = new List<KeyValueModel>();
                foreach (QuDaoJbxx model in liQd)
                {
                    li.Add(new KeyValueModel() { key = model.Bh.ToString(), values = model.Mc });
                }

                ViewBag.QdList = li;
            }
            return View(list);
        }
        /// <summary>
        /// 渠道订单详情
        /// </summary>
        /// <param name="ddbh"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(string ddbh)
        {
            DdjbxxModel model = new Service.DdyymxbService().GetBydsfddxx(ddbh);
            return View(model);
        }

        /// <summary>
        /// 导出渠道订单
        /// </summary>
        /// <returns></returns>
        public FileResult ExportExcel(string qdbh = "", string ddstart = "", string ddend = "", string yystart = "", string yyend = "", string xm = "", string dh = "", string tczt = "")
        // public FileResult ExportExcel(string ddbh )
        {
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            ht.Add("ddly", "0");
            ht.Add("dsfbz", qdbh);
            ht.Add("ddstart", ddstart);
            ht.Add("ddend", ddend);
            ht.Add("yystart", yystart);
            ht.Add("yyend", yyend);
            ht.Add("dh", dh);
            ht.Add("xm", xm);
            ht.Add("tczt", tczt);

            //string ddbhs = "'" + ddbh.TrimEnd(',').Replace(",", "','") + "'";
            DataTable dtExport = new Service.DdyymxbService().ExportOrderYyQdList(ht);

            if (dtExport.Rows.Count > 0)
            {
                dtExport.TableName = "报表数据";
                MemoryStream ms = Common.ExcelTool.RenderToExcel(dtExport);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyy-MM-dd") + "渠道订单.xls");
            }
            else
            {
                //context.Response.Write("数据为空不能导出");
                return null;
            }
        }

        [HttpPost]
        public JsonResult BatchComplete(string ddbhs, int ddzt)
        {

            var result = new Service.DdyymxbService().BatchComplete(ddbhs, ddzt);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

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
                ExcelToList(excelPath, null, 2, out msg, ref li, ref lizhxm);

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
        public void ExcelToList(string fileName, string sheetName, int tclx, out string msg, ref List<XttcbModel> li, ref List<XttczhxmbModel> lizhxm)
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
                        XttcbModel tcModel = new XttcbModel();


                        tcModel.yybh = GLYadmin.YYID;
                        tcModel.tclx = tclx;
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

        private bool ValidateTemplate(IRow firstRow, int totalCol)
        {
            List<string> lstColName = new List<string>();
            lstColName.Add("套餐编号(软件里的编号)");
            lstColName.Add("套餐名称");
            lstColName.Add("单位编号(自营渠道：0000)");
            lstColName.Add("单位名称");
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

        /// <summary>
        /// 预约
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Yy(string id)
        {
            try
            {
                DdjbxxModel ddModel = new DdyymxbService().GetBydsfddxx(id);
                if (ddModel == null)
                {
                    return Json(new { isSuccess = 0, msg = "订单不存在" });
                }
                else
                {
                    if (ddModel.yybh.ToString() != GLYadmin.YYID)
                    {
                        return Json(new { isSuccess = 0, msg = "机构不对" });
                    }
                    DdyymxbService ddService = new DdyymxbService();
                    ddService.UpdatOrderdjlsh(ddModel.ddbh, "手动更新无");
                    return Json(new { isSuccess = 0, msg = "" });

                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = 0, msg = ex.Message });
            }
        }

        public ActionResult Update(XttcbModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (string.IsNullOrEmpty(model.jcyy))
                {
                    model.jcyy = string.Empty;
                }
                if (string.IsNullOrEmpty(model.tclxbh))
                {
                    model.tclxbh = string.Empty;
                }
                if (string.IsNullOrEmpty(model.tctp))
                {
                    model.tctp = string.Empty;
                }
                _xttcbService.Update(model);
                return Json(new { Code = 200, Msg = "修改成功." });
            }
            int id = 0;
            int.TryParse(Request.Params["item"], out id);
            model = _xttcbService.GetSingleById(id);
            if (model == null)
                return Redirect("/");

            ViewBag.MealTable = _xttcbService.GetMinMeal("DL201505201452130001");
            ViewBag.MealTable2 = _xttcbService.GetMinMeal("DL201505201452500001");
            return View(model);
        }
    }
}