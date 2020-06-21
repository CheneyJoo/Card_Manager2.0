using Model;
using Model.Paging;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Common;
using Newtonsoft.Json;

namespace InternetOrder.Controllers
{
    public class PqController : BaseController
    {
        PagingService paging = new PagingService();
        PqJbszService jbszService = new PqJbszService();
        QyjbxxService qyjbxxService = new QyjbxxService();
        PqQyszService qypqszService = new PqQyszService();
        PqPlszjlService plszjlService = new PqPlszjlService();
        PqTjrqService tjrqService = new PqTjrqService();
        PqTjsjService tjsjService = new PqTjsjService();
        // GET: Pq
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
            request.skyl = request.zdjd - request.qtyl - request.tjyl;
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

        public ActionResult QyList(string mc = "", int page = 1)
        {
            int totalRecord = 0;
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" a.yybh='{0}' and a.isdept=0 and a.sfqy=1 and a.sfqd=0", GLYadmin.YYID);
            if (!string.IsNullOrEmpty(mc))
            {
                strWhere.AppendFormat(" AND a.mc LIKE '%{0}%'", mc);
            }
            PageEntity pageEntity = new PageEntity()
            {
                TableName = " dbo.qy_jbxx a LEFT JOIN dbo.pq_qysz b ON a.bh=b.qybh",
                Fields = "a.bh AS qybh,a.mc,a.yybh,b.xxr,b.jzsj,b.tqts",
                PageIndex = page,
                PageSize = PageSize,
                WhereStr = strWhere.ToString(),
                OrderBy = "a.id desc"
            };
            var list = paging.GetPage<Model.Dto.QyPqListDto>(pageEntity, out totalRecord);
            list.ForEach(x =>
            {
                x.xxr = x.xxr.Replace("0", "星期日").Replace("1", "星期一").Replace("2", "星期二").Replace("3", "星期三").Replace("4", "星期四").Replace("5", "星期五").Replace("6", "星期六").Replace("|", "，");
            });

            ViewData["qymc"] = mc;
            ViewBag.Pager = PagingHelper.ShowFPageForBootstrapAdmin(page, PageSize, totalRecord);//生成分页条    
            return View(list);
        }

        public ActionResult QyDetail(string bh = "")
        {
            var qyModel = qyjbxxService.GetModel(bh,GLYadmin.YYID);
            if (qyModel == null)
            {
                return Content("参数错误");
            }

            var qypqModel = qypqszService.GetModelByQybh(bh, GLYadmin.YYID);//渠道排期基本设置

            if (qypqModel == null)
            {
                PqJbszModel jbszModel = jbszService.GetModel(GLYadmin.YYID);
                jbszModel = jbszModel == null ? new PqJbszModel() : jbszModel;
                qypqModel = new PqQyszModel();
                qypqModel.qybh = bh;
                qypqModel.yybh = qyModel.yybh;
            }
            var plszjlModel = plszjlService.GetModelByQybh(bh);
            plszjlModel = plszjlModel == null ? new PqPlszjlModel() : plszjlModel;
            plszjlModel.mxList = JsonConvert.DeserializeObject<List<PlszMxModel>>(plszjlModel.mx);
            if (plszjlModel.mxList.Count == 0)
            {
                plszjlModel.mxList.Add(new PlszMxModel { kssj = "07:30", jssj = "08:30", tjrs = 10 });
                plszjlModel.mxList.Add(new PlszMxModel { kssj = "08:30", jssj = "09:30", tjrs = 10 });
            }
            ViewBag.qy = qyModel;
            ViewBag.jbsz = qypqModel;//基本设置
            ViewBag.plszjl = plszjlModel;//批量设置

            return View();
        }
        [HttpPost]
        public ActionResult BatchSave()
        {
            string yybh = GLYadmin.YYID;
            string qybh = Request["qybh"];//企业id
            int tqts = Convert.ToInt32(Request["tqts"]);//提前天数
            string jzsj = Request["txtTimeBefor"];//截止时间

            //日期范围
            DateTime ksrq = Convert.ToDateTime(Request["txtStartDate"]);
            DateTime jsrq = Convert.ToDateTime(Request["txtEndDate"]);
            //时间段
            var kssjList = Request.Params.GetValues("kssj");
            var jssjList = Request.Params.GetValues("jssj");
            var tjrsList = Request.Params.GetValues("tjrs");
            //休息日
            var xxr = (Request["xxr"] ?? "").Replace(",", "|");

            List<PlszMxModel> mxList = new List<PlszMxModel>();
            List<PqTjrqModel> tjrqList = new List<PqTjrqModel>();
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

                mxList.Add(new PlszMxModel { kssj = kssjList[i], jssj = jssjList[i], tjrs = Convert.ToInt32(tjrsList[i]) });
            }
            //获取企业排期基本设置
            var qyPqModel = qypqszService.GetModelByQybh(qybh,GLYadmin.YYID);
            if (qyPqModel == null)
            {
                qyPqModel = new PqQyszModel() { qybh = qybh, yybh = yybh, jzsj = jzsj, tqts = tqts, xxr = xxr };
                qypqszService.Add(qyPqModel);
            }
            else
            {
                qyPqModel.jzsj = jzsj;
                qyPqModel.tqts = tqts;
                qyPqModel.xxr = xxr;
                qypqszService.Update(qyPqModel);
            }
            PqPlszjlModel plszjlModel = new PqPlszjlModel();
            plszjlModel.kssj = ksrq;
            plszjlModel.jssj = jsrq;
            plszjlModel.mx = JsonConvert.SerializeObject(mxList);
            plszjlModel.yybh = yybh;
            plszjlModel.qybh = qybh;
            plszjlService.Add(plszjlModel);

            tjrqService.BatchSave(ksrq, jsrq, yybh, qybh, mxList, xxr);

            return Json(new { code = 200, msg = "" });
        }

        public ActionResult List()
        {
            return View();
        }


        public ActionResult GetSchedule(string qybh, int year, int month)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var list = tjrqService.GetListByRq(qybh, startDate, endDate);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var item in list)
            {
                dic.Add(string.Format("DT{0}", item.rq.Day), item);
            }
            return Json(dic);
        }

        public ActionResult ChangeFlag(string qybh, DateTime rq, int flag)
        {
            var yybh = GLYadmin.YYID;
            var model = tjrqService.GetModel(qybh, rq);
            if (model == null)
            {
                model = new PqTjrqModel
                {
                    rq = rq,
                    flag = flag,
                    yybh = yybh,
                    qybh = qybh,
                    tjrs = 0
                };
                tjrqService.Add(model);
            }
            else
            {
                model.flag = flag;
                tjrqService.Update(model);
            }
            return Json(new { code = 200, msg = "" });
        }


        public ActionResult GetDetail(string qybh, DateTime rq)
        {
            var yybh = GLYadmin.YYID;
            var list = tjsjService.GetListByRq(qybh,GLYadmin.YYID, rq);
            List<object> retList = new List<object>();
            if (list.Count == 0)
            {
                retList.Add(new { kssj = "07:30", jssj = "08:30", tjrs = 10 });
                retList.Add(new { kssj = "08:30", jssj = "09:30", tjrs = 10 });
            }
            else
            {
                foreach (var item in list)
                {
                    retList.Add(new { kssj = item.kssj.ToString("HH:mm"), jssj = item.jssj.ToString("HH:mm"), tjrs = item.tjrs });
                }
            }
            return Json(retList);
        }


        public ActionResult Save()
        {
            var yybh = GLYadmin.YYID;
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
                tjsjList.Add(new PqTjsjModel { yybh = yybh, qybh = qybh, rq = rq, kssj = kssj, jssj = jssj, tjrs = Convert.ToInt32(tjrsList[i]) });
            }
            PqTjrqModel tjrqModel = new PqTjrqModel { yybh = yybh, qybh = qybh, rq = rq, flag = 0, tjrs = tjrsList.Sum(x => Convert.ToInt32(x)) };
            tjsjService.Save(tjrqModel, tjsjList);
            return Json(new { code = 200, msg = "" });
        }
    }
}