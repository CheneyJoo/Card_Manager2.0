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
    /// 角色权限
    /// </summary>
    public class JsqxController : BaseController
    {

        public ActionResult Index(string jsmc = "", int page = 1)
        {
            int count = 0;
            var list = new JsglbService().JsList(jsmc, GLYadmin.YYID, 0, 1, page, PageSize, ref count);
            if (list != null && list.Count > 0)
            {
                ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, PageSize, count);//生成分页条     
            }
            List<XtMenuModel> li = new JsqxService().GetAllMenu(1);

            XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID); ;

            ViewBag.LstMenu = li;

            if (Request.IsAjaxRequest())
            {
                return PartialView("JsqxPart", list);
            }
            return View(list);

        }

        public JsonResult GetAllMenu()
        {
            try
            {
                List<XtMenuModel> lstMenu = new JsqxService().GetAllMenu(1);

                XtJgbModel jgModel = new Service.XtjgbService().GetJg(GLYadmin.YYID); ;
             

                List<XtMenuModel> lstFstMenu = lstMenu.Where(x => x.scid.Equals(0)).ToList();
                List<object> lstResult = new List<object>();
                foreach (var menu in lstFstMenu)
                {
                    lstResult.Add(new { parent = menu, child = lstMenu.Where(x => x.scid.Equals(menu.Id)).ToList()});
                }
                
                return Json(new ReturnModel { Code = 200, Msg = "Success", Result = lstResult });
            }
            catch (Exception e)
            {
                return Json(new ReturnModel { Code = 201, Msg = "获取菜单选项失败" });
            }
        }

    

        public JsonResult AddOrEdit(int id = 0)
        {
            try
            {
                List<JsqxModel> jsqx = new List<JsqxModel>();
                if (id > 0)
                {
                    jsqx = new JsqxService().Get(id, GLYadmin.GLYUserAccount);
                }
                return Json(new ReturnModel { Code = 200, Msg = "Success", Result = jsqx});
            }
            catch (Exception e)
            {
                return Json(new ReturnModel { Code = 201, Msg = "获取角色权限失败" });
            }
        }

        [HttpPost]
        public JsonResult Save(int jsid, string menuids)
        {
            List<string> lstMenu = menuids.Split(new string[]{","},StringSplitOptions.RemoveEmptyEntries).ToList();
            ReturnModel returnModel = new JsqxService().Save(jsid, GLYadmin.GLYUserAccount, lstMenu);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJsqx(int jsid)
        {
            try
            {
                List<JsqxModel> list = new JsqxService().Get(jsid, GLYadmin.GLYUserAccount);
                return Json(new ReturnModel { Code = 200, Msg = "Success", Result = list });
            }
            catch (Exception e)
            {
                return Json(new ReturnModel { Code = 201, Msg = "获取角色权限失败" });
            }
        }
    }
}