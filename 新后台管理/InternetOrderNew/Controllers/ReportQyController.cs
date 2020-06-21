using Common;
using Model;
using Model.Dto;
using Model.Dto.ReportQy;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetOrder.Controllers
{
    public class ReportQyController : BaseController
    {
        QyjbxxService qyjbxxService = new QyjbxxService();

     
        PqQyRqService pqqyrqSevice = new PqQyRqService();
        DdJbxxService ddService = new DdJbxxService();

        /// <summary>
        /// 预约进度表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dwbh"></param>
        /// <param name="pqstart"></param>
        /// <param name="pqend"></param>
        /// <returns></returns>
        public ActionResult YyJdb(int page = 1, string dwbh = "", string pqstart = "", string pqend = "")
        {
            ReportQyService rs = new ReportQyService();
            ViewBag.jsyrs = rs.GetHomejryrs(GLYadmin.YYID, 1);//首页今日预约人数
            List<string> jsList = rs.GetHometjzryyjd(GLYadmin.YYID);//今日预约总进度
            ViewBag.jsyyzjd = jsList[0];
            ViewBag.jsyyzjdsz = jsList[1];
            //今日到检总进度
            List<string> jsDjList = rs.GetHometjdjzjd(GLYadmin.YYID);
            ViewBag.jsDjzjd = jsDjList[0];
            ViewBag.jsDjzjdsz = jsDjList[1];

            QyyyService qs = new QyyyService();
            int count = 0;
            List<QyyyModel> list = qs.GetQyygjbxxList(GLYadmin.YYID, dwbh, pqstart, pqend, page, PageSize, ref count);
            int recordCount = count;//总记录数

            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("YyJdbPart", list);
            }
            else
            {
                List<KeyValueModel> liQy = new QyjbxxService().GetqyList(GLYadmin.YYID);
                ViewBag.QyList = liQy;
            }
            return View(list);
        }




        /// <summary>
        /// 企业预约日历表
        /// </summary>
        /// <returns></returns>
        public ActionResult Appointment()
        {
            return View();
        }
        /// <summary>
        /// 企业预约日历表,数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public string AppoingmentData(int year, int month)
        {
            var pqjbsz = new PqJbszService().GetModel(GLYadmin.YYID);
            var zrs = pqjbsz == null ? 0 : pqjbsz.zdjd;
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var yyList = ddService.GetQyDayYyrsByMonth(GLYadmin.YYID, year, month);
            //var pqList = pqqyrqSevice.GetList(GLYadmin.YYID,startDate,endDate);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            for (int i = 1; i <= endDate.Subtract(startDate).TotalDays; i++)
            {
                var date = new DateTime(year, month, i);

                int flag = 0;//0:默认1:休息日
                int yyrs = 0;
                var item = yyList.Where(x => x.day == i).FirstOrDefault();


                if (pqjbsz.ztyy.Split(',').Contains(date.ToString("yyyy-MM-dd")) && !pqjbsz.tsky.Split(',').Contains(date.ToString("yyyy-MM-dd")))
                {
                    flag = 1;
                }
                if (pqjbsz.xxr.Split('|').Contains(Convert.ToInt32(date.DayOfWeek).ToString()) && !pqjbsz.tsky.Split(',').Contains(date.ToString("yyyy-MM-dd")))
                {
                    flag = 1;
                }


                if (item != null)
                {
                    yyrs = item.yyrs; ;

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
        public string AppoingmentDayData(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var qyList = qyjbxxService.GetqypaList(GLYadmin.YYID, date.ToString("yyyy-MM-dd"));


            List<dynamic> yyrsList = ddService.GetTjYyrsByDay(GLYadmin.YYID, date);
            List<PqTjrqModel> pqList = new PqTjrqService().GetList(GLYadmin.YYID, date);

            var query = from a in qyList
                        join b in yyrsList on a.key equals b.dwbh into bb
                        from bbb in bb.DefaultIfEmpty()
                        join c in pqList on a.key equals c.qybh into cc
                        from ccc in cc.DefaultIfEmpty()
                        select new OrderDayReport
                        {
                            Qymc = a.values,
                            Yyrs = bbb == null ? 0 : bbb.yyrs,
                            Zrs = ccc == null ? 0 : ccc.tjrs
                        };

            return JsonConvert.SerializeObject(query.ToList());
        }
        /// <summary>
        /// 团检排期报表甘特图
        /// </summary>
        /// <returns></returns>
        public ActionResult TjPqGtt()
        {
            ReportQyService rs = new ReportQyService();
            ViewBag.QyList = rs.GetPqqy(GLYadmin.YYID);
            ViewBag.QyTotal = rs.GetPqqyList(GLYadmin.YYID);
            ViewBag.QyHj = rs.GetPqqyRsList(GLYadmin.YYID);
            return View();
        }
    }
}