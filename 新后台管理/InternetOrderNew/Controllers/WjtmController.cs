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
    /// 问卷题目
    /// </summary>
    public class WjtmController : BaseController
    {
        WjtmService wjtmService = new WjtmService();
        public ActionResult Index(int wjid = 1)
        {
            var list = wjtmService.GetList(wjid);
            //问卷
            var listWj = wjtmService.GetWjList();
            //标签选项
            var bqxx = new WjbqService().GetAllList();
            ViewBag.ListBqxx = bqxx;
            ViewBag.ListWj = listWj;
            ViewBag.Wjid = wjid;
            return View(list);

        }

        public JsonResult AddOrEdit(int id = 0)
        {
            try
            {
                Wjtm tm = new Wjtm();
                if (id > 0)
                {
                    
                    tm = wjtmService.GetEntity(id);
                }
                return Json(new ReturnModel { Code = 200, Msg = "Success", Result = tm });
            }
            catch (Exception e)
            {
                return Json(new ReturnModel { Code = 201, Msg = "获取标签失败" });
            }
        }

        [HttpPost]
        public JsonResult Save(Wjtm model)
        {
            try
            {
                ReturnModel returnModel;
                model.XGR = GLYadmin.GLYUserAccount;
                model.XGSJ = DateTime.Now;
                
                //新增
                if (model.ID.Equals(0))
                {
                    returnModel = wjtmService.Insert(model);
                    return Json(returnModel, JsonRequestBehavior.AllowGet);
                }

                //修改(先删除，再插入)
                returnModel = wjtmService.Delete(model.ID);
                if (returnModel.Code != 200) return Json(new ReturnModel {Code = 201, Msg = "保存失败"});

                returnModel = wjtmService.Insert(model);
                return Json(returnModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return Json(new ReturnModel {Code = 201, Msg = "保存失败"}, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ReturnModel returnModel = wjtmService.Delete(id);
            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }
    }
}