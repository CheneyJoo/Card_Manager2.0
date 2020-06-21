using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using Service;
using System.Data;
using Model;
using System.Configuration;
using Common;

namespace InternetOrder.Controllers
{
    public class OrganController : Controller
    {
        CSService _cityService;
        public OrganController()
        {
            _cityService = new CSService();
        }
        // GET: Organ
        public ActionResult Index()
        {
            ViewBag.CityTable = _cityService.GetAllProvince();
            ViewBag.ID = GLYadmin.YYID;
            XtJgbModel model = new XtjgbService().GetJg(GLYadmin.YYID);
            if (model == null)
                return Redirect("/");
            if (model.yydj == null)
            {
                model.yydj = string.Empty;
            }
            if (model.yyimage == null)
            {
                model.yyimage = string.Empty;
            }
            return View(model);
        }
        public JsonResult GetCity()
        {
            DataTable cityTable = _cityService.GetCityByProvinceId(Request.Params["id"]);
            return Json(new { Code = 200, Data = cityTable.AsEnumerable().Select(rowItem => new { CSBH = rowItem["CSBH"], CSMC = rowItem["CSMC"] }).ToList() });
        }
        public JsonResult UploadImg()
        {
          
            HttpPostedFileBase fileInstance = Request.Files[0];
            string fileName = System.Guid.NewGuid().ToString().Replace("-", string.Empty) + Path.GetExtension(fileInstance.FileName);
            string filePath = Server.MapPath("~/Images/") + fileName;
            Request.Files[0].SaveAs(filePath);
            string website = commonModel.website;
            return Json(new
            {
                code = 0,
                msg = "上传成功.",
                data = new
                {
                    src = website+"/Images/" + fileName,
                    title = fileInstance.FileName
                }
            });
        }

        [ValidateInput(false)]
        public JsonResult SaveData(XtJgbModel model)
        {
            new XtjgbService().UpdateJg(model);
            return Json(new { Code = 200, Msg = "更新成功." });
        }
    }
}