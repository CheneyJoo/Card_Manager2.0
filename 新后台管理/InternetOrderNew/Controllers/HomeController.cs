using MK.Common;
using Model;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetOrder.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
           
            string account = GLYadmin.GLYUserAccount;
            string jsid = GLYadmin.JsId;
            //权限Id
            List <XtMenuModel> li =new Service.JsqxService().GetMenu(int.Parse(jsid));
            ViewBag.LiQx = li;
           
          
            var zhbModel = new XtzhbService().GetModel(Convert.ToInt32(GLYadmin.UserId));
            if (zhbModel != null)
            {
                ViewBag.Txlj = zhbModel.txlj;
            }
            return View();
        }
      
       
        /// <summary>
        /// 普通人员首页
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeJbxx()
        {
            return View();
        }
        /// <summary>
        /// 管理员仪表盘
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeManager()
        {
            return View();
        }
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Grzx()
        {
            XtZhbModel model = new Service.XtzhbService().GetModelWithJs(int.Parse(GLYadmin.UserId));
            return View(model);
        }
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="oldpwd"></param>
        /// <param name="newpwd"></param>
        /// <returns></returns>
        public ActionResult Mmxg(string oldpwd,string newpwd)
        {
            XtZhbModel yy = new XtzhbService().Login(GLYadmin.GLYUserAccount, md5.to32MD5(oldpwd), 1);
            if (yy != null)
            {
                int i = new Service.XtzhbService().UpdatePassword(int.Parse(GLYadmin.UserId), md5.to32MD5(newpwd));
                if (i > 0)
                {
                    return Json(new { flag = true, msg = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { flag = false, msg = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { flag = false, msg = "原始密码不对" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Ybp()
        {
            ReportQdService rs = new ReportQdService();
            ViewBag.zryye = rs.GetzrYye(GLYadmin.YYID);//昨日营业额
            ViewBag.dryye= rs.GetdrYye(GLYadmin.YYID);//当日营业额
            ViewBag.rhb= rs.GetHomerhb(GLYadmin.YYID);//日环比
        
            ViewBag.jsyrs= rs.GetHomejryrs(GLYadmin.YYID);//首页今日预约人数
            ViewBag.zryrs = rs.GetHomezryrs(GLYadmin.YYID);//首页昨日预约人数
    
         
            ViewBag.yyjd= rs.GetHomezryyjd(GLYadmin.YYID);//今日预约总进度
            ViewBag.yyrhb = rs.GetHomeryyrhb(GLYadmin.YYID);//预约日环比
            return View();
        }

        public string GetSalesTrendsBarData()
        {
           var qdlist = new QyjbxxService().GetqyqdList(GLYadmin.YYID); 

            var list = new DdJbxxService().GetSalesTrends(GLYadmin.YYID);
            var monthList = new List<int>();
            var now = DateTime.Now;
            
            for (int i = -11; i <=0; i++)
            {
                monthList.Add(now.AddMonths(i).Month);
            }
            var months = monthList.Select(x => x.ToString()+"月");

            var series = new List<object>();
            foreach (var item in qdlist)
            {
                var name = item.values;
                var type = "bar";
                var color = "#58afff";
                var stack = "abc";
                var data = new List<decimal>();


                var qdData = list.Where(x=>x.qdid==item.key);
                foreach (var month in monthList)
                {
                    var monthData = qdData.FirstOrDefault(x => x.month == month);
                    data.Add(monthData == null ? 0 : Convert.ToDecimal(monthData.money.ToString("0.#")));
                }
                series.Add(new { name=name,type=type,color=color,stack=stack,data=data });
            }

            var value = new { xAxisData = months, series = series };
            return JsonConvert.SerializeObject(value); 
        }
        /// <summary>
        /// 饼图
        /// </summary>
        /// <returns></returns>
        public string GetSalesTrendsPieData()
        {
            var qdlist = new QyjbxxService().GetqyqdList(GLYadmin.YYID);
            var list = new DdJbxxService().GetSalesScale(GLYadmin.YYID);
            var seriesData = new List<object>();
            var totalNum = Math.Round(list.Sum(x => x.money), 2);
            foreach (var item in qdlist)
            {
                var qdData = list.Count > 0 ? (list.Where(x => x.qdid == item.key).Count()>0? list.First(x => x.qdid == item.key) : null) : null;
                var name = item.values;
                var value = qdData == null ? 0 : qdData.money;
                var per = totalNum == 0 ? "100%" : (value / totalNum).ToString("0.##%");
                seriesData.Add(new {name=name,value=value, per =per});
            }
            var legendData = qdlist.Select(x=>x.key);
            var color = "red";
            return JsonConvert.SerializeObject(new { legendData= legendData, seriesData= seriesData, color=color });
        }
    }

   
}