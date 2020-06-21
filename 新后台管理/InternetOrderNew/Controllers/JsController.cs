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
    /// 角色
    /// </summary>
    public class JsController : BaseController
    {
        JsglbService jsglbService = new JsglbService();
        public ActionResult Index(string jsmc = "",int page = 1)
        {
            int count = 0;
            var list = jsglbService.JsList(jsmc, GLYadmin.YYID,0,1, page, PageSize, ref count);
            if (list != null && list.Count > 0)
            {
                ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条     
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("JsPart", list);
            }
            return View(list);

        }

        public JsonResult AddOrEdit(int id =0)
        {
            try
            {
                JsModel js = new JsModel();
                if (id>0)
                {
                    js = jsglbService.GetJs(id);
                }
                return Json(new ReturnModel {Code = 200, Msg = "Success", Result = js});
            }
            catch (Exception e)
            {
                return Json(new ReturnModel { Code = 201, Msg = "获取角色失败"});
            }
        }

        [HttpPost]
        public JsonResult Save(JsModel model)
        {
            ReturnModel returnModel;
            model.Gxrq = DateTime.Now;
            model.Gxr = GLYadmin.GLYUserAccount;
            model.Yybh = GLYadmin.YYID;
            if (model.Id.Equals(0))
            {
                model.Cjrq = DateTime.Now;
                model.Cjr = GLYadmin.GLYUserAccount;
                returnModel =  jsglbService.AddJs(model);
                return Json(returnModel, JsonRequestBehavior.AllowGet);
            }
           
            returnModel = jsglbService.UpdateJs(model);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ReturnModel returnModel = jsglbService.Del(id);
            return Json(returnModel , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BatchDelete(string ids)
        {
            ReturnModel returnModel = jsglbService.DelList(ids);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }


    }
}