using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model;
using Service;
using MK.Common;

namespace InternetOrder.Controllers
{
    /// <summary>
    /// 系统账号
    /// </summary>
    public class XtzhController : BaseController
    {
        XtzhbService xtzhbService = new XtzhbService();
        public ActionResult Index(string mc = "", string cjrq = "", int page = 1)
        {
            int count = 0;
            var list = xtzhbService.GetList(mc, cjrq, GLYadmin.YYID, page, PageSize, ref count);
            if (list != null && list.Count > 0)
            {
                ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条     
            }
            ViewBag.Jsid = new JsglbService().GetJsList(GLYadmin.YYID,1);
            if (Request.IsAjaxRequest())
            {
                return PartialView("XtzhPart", list);
            }
            return View(list);

        }

        public JsonResult AddOrEdit(int id = 0)
        {
            try
            {
                XtZhbModel model = new XtZhbModel();
                if (id > 0)
                {
                    model = xtzhbService.GetModel(id);
                }
                return Json(new ReturnModel { Code = 200, Msg = "Success", Result = model });
            }
            catch (Exception e)
            {
                return Json(new ReturnModel { Code = 201, Msg = "获取账号失败" });
            }
        }

        [HttpPost]
        public JsonResult Save(XtZhbModel model)
        {
            ReturnModel returnModel;
            if (model.id>0)
            {
                XtZhbModel temp = new XtzhbService().GetModel(model.id);
                if (temp == null) return Json(new ReturnModel {Code = 201,Msg = "账号不存在"},JsonRequestBehavior.AllowGet);
                temp.zh = model.zh;
                temp.dh = model.dh;
                temp.jsid = model.jsid;
                temp.lxr = model.lxr;
                temp.bz = model.bz;
                returnModel = new XtzhbService().Update(temp);
                return Json(returnModel, JsonRequestBehavior.AllowGet);
            }
            model.createtime = DateTime.Now;
            model.yybh = GLYadmin.YYID;
            model.mm = md5.to32MD5("123456");
            model.zt = 1;
            model.txlj = "/Content/assets/images/photo/" + new Random().Next(1, 24) + ".png";
            returnModel = new XtzhbService().Add(model);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ReturnModel returnModel = new XtzhbService().Delete(id);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BatchDelete(string ids)
        {
            ReturnModel returnModel = new XtzhbService().BatchDelete(ids);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStaus(int id, int zt =0)
        {
            ReturnModel returnModel = new XtzhbService().UpdateStatus(id,zt);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ResetPassword(int id)
        {
            ReturnModel returnModel = new XtzhbService().ResetPassword(id, md5.to32MD5("123456"));
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }
    }
}