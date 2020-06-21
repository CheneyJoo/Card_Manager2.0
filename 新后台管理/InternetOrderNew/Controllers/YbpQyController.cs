using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetOrder.Controllers
{
    public class YbpQyController : Controller
    {
        // GET: YbpQy
        public ActionResult Index()
        {
            ReportQyService rs = new ReportQyService();

            ViewBag.zjqys = rs.GetZjqys(GLYadmin.YYID);//在检企业数

            ViewBag.jsyrs = rs.GetHomejryrs(GLYadmin.YYID, 1);//首页今日预约人数
            ViewBag.zryrs = rs.GetHomezryrs(GLYadmin.YYID, 1);//首页昨日预约人数

            List<string> jsList = rs.GetHometjzryyjd(GLYadmin.YYID);//今日预约总进度
            ViewBag.jsyyzjd = jsList[0];
            ViewBag.jsyyzjdsz = jsList[1];
            ViewBag.yyrhb = rs.GetHomeryyrhb(GLYadmin.YYID, 1);//预约日环比

            //今日到检总进度
            List<string> jsDjList = rs.GetHometjdjzjd(GLYadmin.YYID);
            ViewBag.jsDjzjd = jsDjList[0];
            ViewBag.jsDjzjdsz = jsDjList[1];
            ViewBag.djrhb = rs.GetHomeTjDjrhb(GLYadmin.YYID);//到检日环比

            //top 5到检进度
            QyyyService qyy = new QyyyService();
            ViewBag.top5 = qyy.GetQyygjbxxHome(GLYadmin.YYID);

            ViewBag.day = qyy.GetVipDdDay(GLYadmin.YYID);//vip 当天
            ViewBag.week = qyy.GetVipDdDay(GLYadmin.YYID);//vip 当周

            return View();
        }
    }
}