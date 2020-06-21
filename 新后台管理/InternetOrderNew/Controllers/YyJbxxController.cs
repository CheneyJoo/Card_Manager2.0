using Common;
using Model;
using Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace InternetOrder.Controllers
{
    /// <summary>
    /// 医院基本信息
    /// </summary>
    public class YyJbxxController : BaseController
    {
        /// <summary>
        /// 基本设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {       
            XtJgbModel model=new XtjgbService().GetJg(GLYadmin.YYID);
            return View(model);
        }
        /// <summary>
        /// 医院更新
        /// </summary>
        /// <param name="lxr"></param>
        /// <param name="lxdh"></param>
        /// <param name="yyjs"></param>
        /// <returns></returns>
        public JsonResult UpdateYy(string lxr, string lxdh,string yyjs)
        {
            XtJgbModel model = new XtjgbService().GetJg(GLYadmin.YYID);
            model.yyjs = yyjs;
            model.lxr = lxr;
            model.lxdh = lxdh;
            int i = new XtjgbService().UpdateJg(model);
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
        /// 组合项目
        /// </summary>
        /// <returns></returns>
        public ActionResult Zhxm(int page = 1, string sfqy = "", string sxrs = "", string mc = "")
        {
            int count = 0;
            Hashtable ht = new Hashtable();
            ht.Add("yybh", GLYadmin.YYID);
            if (!string.IsNullOrEmpty(sfqy))
            {
                ht.Add("sfqy", sfqy);
            }
            if (!string.IsNullOrEmpty(sxrs))
            {
                ht.Add("sxrs", sxrs);
            }
            if (!string.IsNullOrEmpty(mc))
            {
                ht.Add("mc", mc);
            }
            var list = new Service.XtzhxmService().GetZhxmList(ht, page, PageSize, ref count);
            int recordCount = count;//总记录数

            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, recordCount);//生成分页条
            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID);
            ViewBag.Zdbz = jgModel.zdbz;
            if (Request.IsAjaxRequest())
            {
                return PartialView("ZhxmPart", list);
            }
            
            return View(list);

        }
        /// <summary>
        /// 同步组合项目
        /// </summary>
        /// <returns></returns>
        public JsonResult Tb()
        {
          
            //如果中间库成功，再调用医院接口，分3种情况，手动，全自动，半自动
            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID);          
            if (jgModel.iskkservice == 1)
            {
                string msg = "";
                //if (Service.HosHelper.PublicKkService.ZhxmTb(jgModel, ref msg))
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
        /// 组合项目上限人数设置
        /// </summary>
        /// <param name="zhxmbh"></param>
        /// <param name="sxrs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeSxrs(string zhxmbh, int sxrs)
        {
            new Service.XtzhxmService().UpdateSxrs(zhxmbh, sxrs);
            return Json(new { code = 200, msg = "" });
        }

        public JsonResult ImportZhxm()
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
                
                List<XtZhxmbModel> lstModels = ExcelToList(excelPath, null, out msg);

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    return Json(new ReturnModel { Code = 201, Msg = $"{msg}" }, JsonRequestBehavior.AllowGet);
                }
                new XtzhxmService().InsertOrUpdate(lstModels);
                
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
        public List<XtZhxmbModel> ExcelToList(string fileName, string sheetName, out string msg)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            int startRow = 1;
            msg = string.Empty;
            List<XtZhxmbModel> lstModels = new List<XtZhxmbModel>();
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
                        return null;
                    }

                    //最后一行的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null
                        XtZhxmbModel model = new XtZhxmbModel();
                        
                        model.yybh = GLYadmin.YYID;
                        model.zhxmbh = GetCellValue(row, 0).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(model.zhxmbh))
                        {
                            msg = $"第{i}行组合项目编号不能为空";
                            return null;
                        }
                        model.zhxmmc = GetCellValue(row, 1).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(model.zhxmmc))
                        {
                            msg = $"第{i}行组合项目名称不能为空";
                            return null;
                        }
                        model.zhxmms = GetCellValue(row, 2).ToString().Trim();
                        model.zhxmjg = Convert.ToDecimal(GetCellValue(row, 3));
                        string sex = GetCellValue(row, 4).ToString();
                        switch (sex)
                        {
                            case "男":
                                model.xb = 1;
                                break;
                            case "女":
                                model.xb = 0;
                                break;
                            case "通用":
                                model.xb = 2;
                                break;
                            default:
                                model.xb = -1;
                                break;
                        }
                        
                        if (model.xb.Equals(-1))
                        {
                            msg = $"第{i}行性别不正确(仅支持:男,女,通用)";
                            return null;
                        }
                        string sffk = GetCellValue(row, 5).ToString();
                        switch (sffk)
                        {
                            case "是":
                                model.sffk = 1;
                                break;
                            case "否":
                                model.sffk = 0;
                                break;
                            default:
                                model.sffk = -1;
                                break;
                        }
                        if (model.sffk.Equals(-1))
                        {
                            msg = $"第{i}行是否妇科不正确(仅支持:是,否)";
                            return null;
                        }
                        string sfqy = GetCellValue(row, 6).ToString();
                        switch (sfqy)
                        {
                            case "是":
                                model.sfqy = 1;
                                break;
                            case "否":
                                model.sfqy = 0;
                                break;
                            default:
                                model.sfqy = -1;
                                break;
                        }
                        if (model.sfqy.Equals(-1))
                        {
                            msg = $"第{i}行是否启用不正确(仅支持:是,否)";
                            return null;
                        }
                        model.zhxmksbh = GetCellValue(row, 7).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(model.zhxmksbh))
                        {
                            msg = $"第{i}行组合项目科室编号不能为空";
                            return null;
                        }
                        model.zhxmksmc = GetCellValue(row, 8).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(model.zhxmksmc))
                        {
                            msg = $"第{i}行组合项目科室名称不能为空";
                            return null;
                        }
                        model.sxrs = Convert.ToInt32(GetCellValue(row, 9));
                        lstModels.Add(model);
                    }
                }

                if (lstModels.Count.Equals(0))
                {
                    msg = "模板不能为空";
                    return null;
                }
                return lstModels;
            }
            catch (Exception ex)
            {
                msg = "读取模板失败";
                Log.WriteLog(ex.Message);
                return null;
            }
        }

        private bool ValidateTemplate(IRow firstRow, int totalCol)
        {
            List<string> lstColName = new List<string>();
            lstColName.Add("组合项目编号");
            lstColName.Add("组合项目名称");
            lstColName.Add("组合项目描述");
            lstColName.Add("组合项目价格");
            lstColName.Add("性别(仅支持:男，女，通用)");
            lstColName.Add("是否妇科(仅支持:是，否)");
            lstColName.Add("是否启用(仅支持:是，否)");
            lstColName.Add("科室编号");
            lstColName.Add("科室名称");
            lstColName.Add("上限人数");
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
        
    }
}