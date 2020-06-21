using Common;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetOrder.Controllers
{
    /// <summary>
    /// 企业排期
    /// </summary>
    public class ScheduleQyController :  BaseController
    {
        PagingService paging = new PagingService();
        QyjbxxService qyjbxxService = new QyjbxxService();
        XttcbService tcService = new XttcbService();
        PqQyszService pqqyszService = new PqQyszService();
        PqTjrqService pqtjrqSevice = new PqTjrqService();
        PqTjsjService pqtjsjService = new PqTjsjService();
        PqJbszService jbszService = new PqJbszService();
        // GET: Schedule
        public ActionResult Index()
        {
            PqJbszModel model = jbszService.GetModel(GLYadmin.YYID);
            model = model == null ? new PqJbszModel() : model;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PqJbszModel request)
        {
            PqJbszModel model = jbszService.GetModel(GLYadmin.YYID);
            request.tjjgid = GLYadmin.YYID;
            request.skyl = request.zdjd - request.qtyl - request.tjyl - request.qdyl;
            request.xxr = string.IsNullOrEmpty(Request["xxr"]) ? "" : Request["xxr"].Replace(",", "|");
            if (model == null)
            {
                jbszService.Add(request);
            }
            else
            {
                jbszService.Update(request);
            }
            return Json(new { code = 200, msg = "成功" });
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="type">1，渠道，0企业（其实渠道也是企业的）</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult List(string qybh = "",int type=0, int page = 1)
        {
            int count = 0;
            List<dynamic> list = pqqyszService.GetPageList(page, PageSize,ref count, GLYadmin.YYID, qybh,type);
            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条       
            if (Request.IsAjaxRequest())
            {
                return PartialView("ListPart", list);
            }
            else
            {
                if(type==0)
                {
                    ViewBag.QyList = qyjbxxService.GetqyList(GLYadmin.YYID);
                }
                else
                {
                    ViewBag.QyList = qyjbxxService.GetqyqdList(GLYadmin.YYID);
                }
               
            }
            ViewBag.type = type;
            return View(list);
        }


        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">0企业新增，1渠道</param>
        /// <returns></returns>
        public ActionResult Edit(string id = "",int type=0)
        {
            PqQyszModel model = new PqQyszModel();
            List<XttcbModel> tcList = new List<XttcbModel>();
            List<PqPjtcModel> pjtcList = new List<PqPjtcModel>();
            if (!string.IsNullOrEmpty(id))
            {
                model = pqqyszService.GetModel(id);

                tcList = tcService.GetListByQybh(GLYadmin.YYID, model.qybh);
                pjtcList = pqqyszService.GetPjtc(model.pqbh);

                if (model.sfqd == 0)
                {
                    ViewBag.type = 0;
                    ViewBag.QyList = qyjbxxService.GetqyList(GLYadmin.YYID);
                }
                else
                {
                    ViewBag.type = 1;
                    ViewBag.QyList = qyjbxxService.GetqyqdList(GLYadmin.YYID);
                }

            }
            else
            {
                if (type == 0)
                {
                    ViewBag.type = 0;
                    ViewBag.QyList = qyjbxxService.GetqyList(GLYadmin.YYID);
                }
                else
                {
                    ViewBag.type = 1;
                    ViewBag.QyList = qyjbxxService.GetqyqdList(GLYadmin.YYID);
                }
            }

            if (!string.IsNullOrEmpty(model.sjd))
            {
                model.SjdList = JsonConvert.DeserializeObject<List<Sjd>>(model.sjd);
            }
            else
            {
                model.SjdList.Add(new Sjd());
            }


           
            ViewBag.TcList = tcList;
            ViewBag.PjtcList = pjtcList;
            return View(model);
        }


        public string GetTcList(string qybh)
        {
            var list = tcService.GetListByQybh(GLYadmin.YYID, qybh);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult BatchSave(PqQyszModel model)
        {
            var rq = Request["rq"];
            model.ksrq = Convert.ToDateTime(rq.Split('~')[0]);
            model.jsrq = Convert.ToDateTime(rq.Split('~')[1]);
            if (pqqyszService.CheckRepeat(GLYadmin.YYID, model.qybh, model.ksrq, model.jsrq, model.pqbh))
            {
                return Json(new { code = 500, msg = "排期日期有重复", pqbh = model.pqbh });
            }


            model.yybh = GLYadmin.YYID;
            model.xxr = string.IsNullOrEmpty(Request["xxr"]) ? "" : Request["xxr"].Replace(",", "|");
            //时间段
            var kssjList = Request.Params.GetValues("kssj") ?? new string[] { };
            var jssjList = Request.Params.GetValues("jssj") ?? new string[] { };
            var tjrsList = Request.Params.GetValues("tjrs") ?? new string[] { };
            var tjrs_dyList = Request.Params.GetValues("tjrs_dy") ?? new string[] { };

            model.SjdList = new List<Sjd>();
            for (int i = 0; i < kssjList.Count(); i++)
            {
                if (string.IsNullOrEmpty(kssjList[i]) || string.IsNullOrEmpty(jssjList[i]))
                {
                    return Json(new { code = 500, msg = "时间段不能为空" });
                }
                else if (string.IsNullOrEmpty(tjrsList[i]))
                {
                    return Json(new { code = 500, msg = "预留人数不能为空" });
                }
                for (int j = i + 1; j < kssjList.Count(); j++)
                {
                    if (Convert.ToDateTime(kssjList[j]) > Convert.ToDateTime(kssjList[i]) && Convert.ToDateTime(kssjList[j]) < Convert.ToDateTime(jssjList[i]))
                    {
                        return Json(new { code = 500, msg = "第" + (i + 1) + "和第" + (j + 1) + "行时间有重叠" });
                    }
                    else if (Convert.ToDateTime(jssjList[j]) > Convert.ToDateTime(kssjList[i]) && Convert.ToDateTime(jssjList[j]) < Convert.ToDateTime(jssjList[i]))
                    {
                        return Json(new { code = 500, msg = "第" + (i + 1) + "和第" + (j + 1) + "行时间有重叠" });
                    }
                    else if (Convert.ToDateTime(kssjList[j]) < Convert.ToDateTime(kssjList[i]) && Convert.ToDateTime(jssjList[j]) > Convert.ToDateTime(jssjList[i]))
                    {
                        return Json(new { code = 500, msg = "第" + (i + 1) + "和第" + (j + 1) + "行时间有重叠" });
                    }
                }

                int tjrs_dy = Convert.ToInt32(string.IsNullOrEmpty(tjrs_dyList[i]) ? tjrsList[i] : tjrs_dyList[i]);
                model.SjdList.Add(new Sjd { kssj = kssjList[i], jssj = jssjList[i], tjrs = Convert.ToInt32(tjrsList[i]),tjrs_dy= tjrs_dy });
            }

            model.tjrs = model.SjdList.Sum(x => x.tjrs);
            model.tjrs_dy = model.SjdList.Sum(x=>x.tjrs_dy);
            model.sjd = JsonConvert.SerializeObject(model.SjdList);
            string[] pjtc = Request.Params.GetValues("pjtc");
            string[] pjtcrs = Request.Params.GetValues("pjtcrs");
            string[] pjtcXxr = Request.Params.GetValues("pjtcXxr");
            string[] pjtcSfxz = Request.Params.GetValues("pjtcSfxz");
            List<PqPjtcModel> pjtcList = new List<PqPjtcModel>();
            if (pjtc != null && pjtcrs != null)
            {
                for (int i = 0; i < pjtc.Length; i++)
                {
                    foreach (var item in pjtc[i].Split(','))
                    {
                        pjtcList.Add(new PqPjtcModel { tcbh = item, tjrs = Convert.ToInt32(pjtcrs[i])});
                    }
                }
                if (pjtcList.Count() > 0 && pjtcList.Select(x => x.tcbh).Count() != pjtcList.Select(x => x.tcbh).Distinct().Count())
                {
                    return Json(new { code = 500, msg = "瓶颈套餐有重复" });
                }
            }
            pqqyszService.Save(model, pjtc, pjtcrs, pjtcXxr, pjtcSfxz);


            return Json(new { code = 200, msg = "", pqbh = model.pqbh });

        }

        /// <summary>
        /// 单个保存
        /// </summary>
        /// <returns></returns>
        public ActionResult Save()
        {
            var yybh = GLYadmin.YYID;
            string pqbh = Request["pqbh"];
            string qybh = Request["qybh"];

            //日期
            DateTime rq = Convert.ToDateTime(Request["tjrq"]);

            //时间段
            var kssjList = Request.Params.GetValues("kssj");
            var jssjList = Request.Params.GetValues("jssj");
            var tjrsList = Request.Params.GetValues("tjrs");


            List<PqTjsjModel> tjsjList = new List<PqTjsjModel>();
            for (int i = 0; i < kssjList.Count(); i++)
            {
                if (string.IsNullOrEmpty(kssjList[i]) || string.IsNullOrEmpty(jssjList[i]))
                {
                    return Json(new { code = 500, msg = "时间段不能为空" });
                }
                else if (string.IsNullOrEmpty(tjrsList[i]))
                {
                    return Json(new { code = 500, msg = "预留人数不能为空" });
                }
                for (int j = i + 1; j < kssjList.Count(); j++)
                {
                    if (Convert.ToDateTime(kssjList[j]) > Convert.ToDateTime(kssjList[i]) && Convert.ToDateTime(kssjList[j]) < Convert.ToDateTime(jssjList[i]))
                    {
                        return Json(new { code = 500, msg = "第" + (i + 1) + "和第" + (j + 1) + "行时间有重叠" });
                    }
                    else if (Convert.ToDateTime(jssjList[j]) > Convert.ToDateTime(kssjList[i]) && Convert.ToDateTime(jssjList[j]) < Convert.ToDateTime(jssjList[i]))
                    {
                        return Json(new { code = 500, msg = "第" + (i + 1) + "和第" + (j + 1) + "行时间有重叠" });
                    }
                    else if (Convert.ToDateTime(kssjList[j]) < Convert.ToDateTime(kssjList[i]) && Convert.ToDateTime(jssjList[j]) > Convert.ToDateTime(jssjList[i]))
                    {
                        return Json(new { code = 500, msg = "第" + (i + 1) + "和第" + (j + 1) + "行时间有重叠" });
                    }
                }
                var kssj = Convert.ToDateTime(rq.ToString("yyyy-MM-dd") + " " + kssjList[i]);
                var jssj = Convert.ToDateTime(rq.ToString("yyyy-MM-dd") + " " + jssjList[i]);
                tjsjList.Add(new PqTjsjModel { pqbh = pqbh, yybh = yybh, qybh = qybh, rq = rq, kssj = kssj, jssj = jssj, tjrs = Convert.ToInt32(tjrsList[i]) });
            }
            PqTjrqModel tjrqModel = new PqTjrqModel { pqbh = pqbh, yybh = yybh, qybh = qybh, rq = rq, flag = 0, tjrs = tjrsList.Sum(x => Convert.ToInt32(x)) };
            pqtjsjService.Save(tjrqModel, tjsjList);
            return Json(new { code = 200, msg = "" });
        }
        /// <summary>
        /// 追加保存
        /// </summary>
        /// <returns></returns>
        public ActionResult ZjSave()
        {

            string pqbh = Request["pqbh"];

            //结束日期
            DateTime jsrq = Convert.ToDateTime(Request["jssj"]);

            PqQyszModel model = pqqyszService.GetModel(pqbh);
            if (jsrq <= model.jsrq)
            {
                return Json(new { code = 500, msg = "追加日期必须大于结束日期" });
            }
            model.ksrq = model.jsrq.AddDays(1);
            model.jsrq = jsrq;
            pqqyszService.ZjSave(model);
            return Json(new { code = 200, msg = "",pqbh=pqbh });
        }
        public ActionResult ChangeFlag(string qybh, DateTime rq, int flag)
        {
            var yybh = GLYadmin.YYID;
            var model = pqtjrqSevice.GetModel(GLYadmin.YYID, qybh, rq);

            model.flag = flag;
            pqtjrqSevice.Update(model);

            return Json(new { code = 200, msg = "" });
        }


        public ActionResult GetSchedule(string qybh, int year, int month)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var list = pqtjrqSevice.GetList(GLYadmin.YYID, qybh, startDate, endDate);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var item in list)
            {
                dic.Add(string.Format("DT{0}", item.rq.Day), item);
            }
            return Json(dic);
        }


        public ActionResult EditNumByDay(int qdid, DateTime rq, int tjrs)
        {
            //pqqyrqSevice.UpdateTjrsByRq(GLYadmin.YYID, qdid, rq, tjrs);
            return Json(new { code = 200, msg = "" });
        }

        public string GetTjsj(string qybh, DateTime rq)
        {
            List<PqTjsjModel> list = pqtjsjService.GetListByRq(qybh, GLYadmin.YYID, rq);
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "HH:mm:ss" };
            return JsonConvert.SerializeObject(list, timeConverter);
        }
    }
}