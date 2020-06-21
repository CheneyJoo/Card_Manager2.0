using Common;
using Model;
using Model.Dto;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetOrder.Controllers
{
    public class JsXxController : BaseController
    {
        public ActionResult QyJs(int page = 1, string dwbh = "", string pqstart = "", string pqend = "")
        {
            QyyyService qs = new QyyyService();
            int count = 0;
            List<QyyyModel> list = qs.GetQyygjbxxList(GLYadmin.YYID, dwbh, pqstart, pqend, page, PageSize, ref count);
            int recordCount = count;//总记录数

            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("QyJsPart", list);
            }
            else
            {
                List<KeyValueModel> liQy = new QyjbxxService().GetqyList(GLYadmin.YYID);
                ViewBag.QyList = liQy;
            }
            return View(list);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public FileResult JsExportExcel(string dwbh)
        {
            ReportQyService sr = new ReportQyService();
            DataTable dtExport = sr.GetTjJsExport(GLYadmin.YYID, dwbh);
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
    }
}