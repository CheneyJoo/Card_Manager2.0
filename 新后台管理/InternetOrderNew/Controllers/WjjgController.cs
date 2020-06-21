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
    public class WjjgController : BaseController
    {
        WjjgService wjjgService = new WjjgService();
        public ActionResult Index(string jgmc = "", int wjzt =-1, int page = 1)
        {
            int count = 0;
            var list = wjjgService.GetList(jgmc, wjzt, page, PageSize, ref count);
            
            if (list != null && list.Count > 0)
            {
                ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, 15, count);//生成分页条     
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("JgPart", list);
            }

            //机构选项
            var jgxx = wjjgService.GetJgSelect();
            ViewBag.Jgxx = jgxx;
            return View(list);

        }

        public JsonResult AddOrEdit(int id = 0)
        {
            try
            {
                Wjjg bq = new Wjjg();
                if (id > 0)
                {
                    bq = wjjgService.GetEntity(id);
                }
                return Json(new ReturnModel { Code = 200, Msg = "Success", Result = bq });
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return Json(new ReturnModel { Code = 201, Msg = "获取标签失败" });
            }
        }

        [HttpPost]
        public JsonResult Save(Wjjg model)
        {
            ReturnModel returnModel;
            if (model.ID.Equals(0))
            {
                model.CJR = GLYadmin.GLYUserAccount;
                model.CJSJ = DateTime.Now;
                returnModel = wjjgService.Insert(model);
                return Json(returnModel, JsonRequestBehavior.AllowGet);
            }

            returnModel = wjjgService.Update(model);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ReturnModel returnModel = wjjgService.Delete(id);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }
    }
}