using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model;
using Service;

namespace InternetOrder.Controllers
{
    public class WjtcController : BaseController
    {
        WjtcService wjtcService = new WjtcService();
        public ActionResult Index(string tcmc = "", string tcbh = "", int page = 1)
        {
            int count = 0;
            var list = wjtcService.GetList(tcbh, tcmc, GLYadmin.YYID, page, PageSize, ref count);
            if (list != null && list.Count > 0)
            {
                ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, 15, count);//生成分页条     
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("TcPart", list);
            }
            
            ViewBag.Yybh = GLYadmin.YYID;
            return View(list);

        }

        public ActionResult AddOrEdit(int id = 0, string yybh ="")
        {
            Wjtc tc = new Wjtc();
            if (id > 0)
            {
                tc = wjtcService.GetEntity(id);
            }
            //套餐选项
            var tcxx = wjtcService.GetTcSelect(yybh);
            ViewBag.Tcxx = tcxx;
            //标签选项
            var bqxx = new WjbqService().GetAllList();
            ViewBag.Bqxx = bqxx;
            ViewBag.Yybh = yybh;
            return View(tc);
        }

        public JsonResult GetTcjg(string tcid)
        {
            try
            {
                var model = wjtcService.GetTcxx(tcid);
                return Json(new ReturnModel {Code = 200, Msg = "Success", Result = model.jg},JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new ReturnModel { Code = 201, Msg = "获取价格失败"});
            }
            

        }

        [HttpPost]
        public JsonResult Save(Wjtc model)
        {
            ReturnModel returnModel;
            if (model.ID.Equals(0))
            {
                returnModel = wjtcService.Insert(model);
                return Json(returnModel, JsonRequestBehavior.AllowGet);
            }

            returnModel = wjtcService.Update(model);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ReturnModel returnModel = wjtcService.Delete(id);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 套餐组合项目查看
        /// </summary>
        /// <returns></returns>
        public ActionResult TcZhxmDetail(string yybh, string tcbh = "")
        {
            List<XtZhxmbModel> list = wjtcService.GetTcZhxm(yybh, tcbh);
            return PartialView("TcDetailPart", list);
        }
    }
}