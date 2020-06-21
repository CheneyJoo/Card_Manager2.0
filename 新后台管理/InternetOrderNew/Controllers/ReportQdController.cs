using Common;
using Model;
using Model.Api;
using Model.Dto.ReportQd;
using Newtonsoft.Json;
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
    /// <summary>
    /// 
    /// </summary>
    public class ReportQdController : BaseController
    {
        //PagingService paging = new PagingService();
        QyjbxxService qyjbxxService = new QyjbxxService();
      
        PqQyRqService pqqyrqSevice = new PqQyRqService();
        DdJbxxService ddService = new DdJbxxService();


       
 
        /// <summary>
        /// 营收报表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public ActionResult YsList(string start="",string end="")
        {
            Service.ReportQdService sr = new ReportQdService();
            if (Request.IsAjaxRequest())
            {
                List<Model.Dto.ReportQd.YyYsReport> li = sr.ReporYyYsList(start, end, GLYadmin.YYID);
                return PartialView("YsPartList", li);
            }
            else
            {
                List<Model.Dto.ReportQd.YyYsReport> li = sr.ReporYyYsList(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), GLYadmin.YYID);
                ViewBag.Yye30 = sr.Get30Yye(GLYadmin.YYID);
                ViewBag.Yye = sr.GetdrYye(GLYadmin.YYID);
                ViewBag.Yyedy = sr.GetdyYye(GLYadmin.YYID);

                List<QuDaoJbxx> liQd = new QuDaoJbxxService().GetYyQudao(GLYadmin.YYID, "");

                List<KeyValueModel> liQdReturn = new List<KeyValueModel>();
                foreach (QuDaoJbxx model in liQd)
                {
                    liQdReturn.Add(new KeyValueModel() { key = model.Bh.ToString(), values = model.Mc });
                }

                ViewBag.QdList = liQdReturn;

                return View(li);
            }          
         
        }
        public JsonResult Ys(int qdId=0)
        {
            Service.ReportQdService sr = new ReportQdService();      
            string Yye30 = sr.Get30Yye(GLYadmin.YYID,qdId);
            string Yye = sr.GetdrYye(GLYadmin.YYID,qdId);
            string Yyedy = sr.GetdyYye(GLYadmin.YYID,qdId);

        
            return Json(new { flag = true, Yye30 = Yye30, Yye= Yye, Yyedy= Yyedy }, JsonRequestBehavior.AllowGet);
           
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public FileResult ExportExcel(int qdid,string start = "", string end = "")
        {
            Service.ReportQdService sr = new ReportQdService();
            DataTable dtExport = sr.GetYsExport(start, end, GLYadmin.YYID,qdid);         
            if (dtExport.Rows.Count > 0)
            {               
                dtExport.TableName = "报表数据";
                MemoryStream ms = Common.ExcelTool.RenderToExcel(dtExport);              
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel",  "订单名细.xls");
            }
            else
            {            
                return null;
            }
        }
        

        /// <summary>
        /// 预约报表
        /// </summary>
        /// <returns></returns>
        public ActionResult Appointment()
        {
            return View();
        }
        /// <summary>
        /// 医院排期报表
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public string AppoingmentData(int year, int month)
        {
            //渠道基本排期设置
            var pqjbsz = new PqJbszService().GetModel(GLYadmin.YYID);
            var zrs = pqjbsz == null ? 0 : pqjbsz.zdjd;
           
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

           //渠道每日预约量
            var yyList = ddService.GetQdDayYyrsByMonth(GLYadmin.YYID, year, month);
            //var pqList = pqqyrqSevice.GetList(GLYadmin.YYID,startDate,endDate);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            for (int i = 1; i <= endDate.Subtract(startDate).TotalDays; i++)
            {
                var date = new DateTime(year, month, i);


                int flag = 0;//0:默认1:休息日
                int yyrs = 0;
                var item = yyList.Where(x => x.day == i).FirstOrDefault();
                //当天预约人数
                if (item != null)
                {
                    yyrs = item.yyrs; ;

                }

                //暂停预约校验
                if (pqjbsz.ztyy.Split(',').Contains(date.ToString("yyyy-MM-dd")) && !pqjbsz.tsky.Split(',').Contains(date.ToString("yyyy-MM-dd")))
                {
                    flag = 1;
                }
                //休息日校验
                if (pqjbsz.xxr.Split('|').Contains(Convert.ToInt32(date.DayOfWeek).ToString()) && !pqjbsz.tsky.Split(',').Contains(date.ToString("yyyy-MM-dd")))
                {
                    flag = 1;
                }

              
                dic.Add(string.Format("DT{0}", i), new { day = i, zrs = zrs, yyrs = yyrs, flag = flag });
            }
            return JsonConvert.SerializeObject(dic);
        }
        /// <summary>
        /// 预约百分比
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public string AppoingmentDayData (int year,int month,int day)
        {
            var date = new DateTime(year,month,day);
            List<KeyValueModel> qdList =qyjbxxService.GetqyqdList(GLYadmin.YYID);         
         
            List<dynamic> yyrsList = ddService.GetYyrsByDay(GLYadmin.YYID, date);
            List<PqQyRqModel> pqList = pqqyrqSevice.GetList(GLYadmin.YYID, date);

            var query = from a in qdList
                        join b in yyrsList on a.key equals b.qybh into bb
                        from bbb in bb.DefaultIfEmpty()
                        join c in pqList on a.key equals c.qybh into cc
                        from ccc in cc.DefaultIfEmpty()
                        select new OrderDayReport
                        {
                            Qdmc = a.values,
                            Yyrs = bbb == null ? 0 : bbb.yyrs,
                            Zrs = ccc == null ? 0 : ccc.tjrs,
                            color="red"
                      
                        };
            var title = string.Format("年月日  ");

            return JsonConvert.SerializeObject(query.ToList());
        }
    }
}