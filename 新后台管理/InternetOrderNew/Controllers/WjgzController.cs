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
    /// <summary>
    /// 问卷规则
    /// </summary>
    public class WjgzController : BaseController
    {
        WjgzService wjgzService = new WjgzService();
        WjbqService wjbqService = new WjbqService();
        public ActionResult Index(string gzmc = "", int page = 1)
        {
            int count = 0;
            var list = wjgzService.GetList(gzmc, page, PageSize, ref count);
            
            if (list != null && list.Count > 0)
            {
                ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, 15, count);//生成分页条     
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("GzPart", list);
            }
            var listBq = wjbqService.GetAllList();
            ViewBag.ListBq = listBq;
            return View(list);

        }

        public JsonResult AddOrEdit(int id = 0)
        {
            try
            {
                Wjgz gz = new Wjgz();
                if (id > 0)
                {
                    gz = wjgzService.GetEntity(id);
                }
                return Json(new ReturnModel { Code = 200, Msg = "Success", Result = gz });
            }
            catch (Exception e)
            {
                return Json(new ReturnModel { Code = 201, Msg = "获取标签失败" });
            }
        }

        [HttpPost]
        public JsonResult Save(Wjgz model)
        {
            ReturnModel returnModel;
            
            if (model.ID.Equals(0))
            {
                returnModel = wjgzService.Insert(model);
                return Json(returnModel, JsonRequestBehavior.AllowGet);
            }

            returnModel = wjgzService.Update(model);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ReturnModel returnModel = wjgzService.Delete(id);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }
    }
}