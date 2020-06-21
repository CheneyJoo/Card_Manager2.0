using Common;
using Model;
using Model.Dto;
using Model.Paging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InternetOrder.Controllers
{

    public class ScheduleController : BaseController

    {

        PagingService paging = new PagingService();
        QyjbxxService qyjbxxService = new QyjbxxService();
        XttcbService tcService = new XttcbService();    
        PqQyRqService pqqyrqSevice = new PqQyRqService();
        PqJbszService jbszService = new PqJbszService();
        // GET: Schedule
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
            request.skyl = request.zdjd - request.qtyl - request.tjyl - request.qdyl;
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
    }
}