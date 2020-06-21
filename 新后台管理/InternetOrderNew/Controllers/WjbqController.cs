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
    public class WjbqController : BaseController
    {
        WjbqService wjbqService = new WjbqService();
        public ActionResult Index(string bqmc = "", int page = 1)
        {
            int count = 0;
            var list = wjbqService.GetList(bqmc, page, PageSize, ref count);
            if (list != null && list.Count > 0)
            {
                ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, 15, count);//生成分页条     
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("BqPart", list);
            }
            return View(list);

        }

        public JsonResult AddOrEdit(int id = 0)
        {
            try
            {
                Wjbq bq = new Wjbq();
                if (id > 0)
                {
                    bq = wjbqService.GetEntity(id);
                }
                return Json(new ReturnModel { Code = 200, Msg = "Success", Result = bq });
            }
            catch (Exception e)
            {
                return Json(new ReturnModel { Code = 201, Msg = "获取标签失败" });
            }
        }

        [HttpPost]
        public JsonResult Save(Wjbq model)
        {
            ReturnModel returnModel;
            if (model.ID.Equals(0))
            {
                returnModel = wjbqService.Insert(model);
                return Json(returnModel, JsonRequestBehavior.AllowGet);
            }

            returnModel = wjbqService.Update(model);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ReturnModel returnModel = wjbqService.Delete(id);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }
    }
}