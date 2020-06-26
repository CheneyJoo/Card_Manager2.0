using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using SYSEntity;
using Common;
using System.Data;
using System.IO;

namespace InternetOrder.Controllers
{
    public class AgentController : BaseController
    {
        // GET: Agent
        public ActionResult Index(int rows = 10, int page = 1, int maxpages = 10, string agentname = "")
        {
            return View(GetAgentList(rows, page, maxpages, agentname));
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
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
                    src = website + "/Images/" + fileName,
                    title = fileInstance.FileName
                }
            });
        }
        /// <summary>
        /// 保存代理商
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult SaveAgent(string data)
        {
            Agent agentBll = new Agent();

            TB_DATA_AGENT agentobj = (TB_DATA_AGENT)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(TB_DATA_AGENT));

            if (!String.IsNullOrEmpty(agentobj.AGENT_ID))
            {

                Dictionary<string, object> dic = ConvertJson.JsonToDictionary(data);
                agentobj.Attach();
                agentobj.SetModifiedProperties(dic);
                agentobj.LAST_UPDATED_BY = HttpContext.Session["UserId"].ToString();
                agentobj.LAST_UPDATED_TIME = DateTime.Now;
            }
            else
            {
                agentobj.AGENT_ID = Guid.NewGuid().ToString();
                agentobj.STATUS = 1;
                agentobj.CREATED_BY = HttpContext.Session["UserId"].ToString();
                agentobj.CREATED_TIME = DateTime.Now;
                agentobj.LAST_UPDATED_BY = HttpContext.Session["UserId"].ToString();
                agentobj.LAST_UPDATED_TIME = DateTime.Now;
            }
            int flag = agentBll.SaveAgent(agentobj);
            if (flag == 1)
            {
                return Json(new { error = 1, msg = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            if (flag == -1)
            {
                return Json(new { error = -1, msg = "输入的账号已经存在，请重新输入!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = 0, msg = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 查询一个代理商信息
        /// </summary>
        /// <param name="Agent_ID"></param>
        /// <returns></returns>
        public JsonResult QueryOneAgent(String Agent_ID)
        {
            Agent agentBll = new Agent();
            DataTable dataTable = agentBll.QueryOneAgentInfo(Agent_ID);
            return Json(dataTable.ToJsJson());

        }
        /// <summary>
        /// 查询代理商列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="maxpages"></param>
        /// <param name="agentname"></param>
        /// <returns></returns>
        public DataTable GetAgentList(int rows, int page, int maxpages, string agentname)
        {
            Agent agentBll = new Agent();
            Dictionary<string, string> queryStr = new Dictionary<string, string>();
            IDictionary<string, object> result;
            if (agentname != null && !string.IsNullOrEmpty(agentname))
            {
                queryStr.Add("AGENT_NAME", agentname);
            }
            result = agentBll.QueryAgent(rows, page, queryStr);
            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, rows, int.Parse(result["totalRow"].ToString()));//生成分页条      
            DataTable dataTable = result["table"] as DataTable;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataTable.Rows[i]["AGENT_NO"] = ((i + 1) + (rows * (page - 1)));
            }
            return dataTable;
        }

        /// <summary>
        /// 禁用代理商状态
        /// </summary>
        /// <param name="Agent_ID"></param>
        /// <returns></returns>
        public JsonResult DisableAgent(string Agent_ID)
        {
            String CurrUser = HttpContext.Session["UserId"].ToString();
            Agent agentBll = new Agent();

            int delres = agentBll.UpdateAgentStatus(Agent_ID, CurrUser, 0);
            if (delres == 1)
            {
                return Json(new { error = 1, msg = "禁用成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = 0, msg = "禁用失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 启用代理商状态
        /// </summary>
        /// <param name="Agent_ID"></param>
        /// <returns></returns>
        public JsonResult AbleAgent(string Agent_ID)
        {
            String CurrUser = HttpContext.Session["UserId"].ToString();
            Agent agentBll = new Agent();

            int delres = agentBll.UpdateAgentStatus(Agent_ID, CurrUser, 1);
            if (delres == 1)
            {
                return Json(new { error = 1, msg = "启用成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = 0, msg = "启用失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="maxpages"></param>
        /// <param name="agentname"></param>
        /// <returns></returns>
        public ActionResult List(int rows, int page, int maxpages, string agentname)
        {
            return View(GetAgentList(rows, page, maxpages, agentname));
        }

        /// <summary>
        /// 删除代理商
        /// </summary>
        /// <param name="agentList"></param>
        /// <returns></returns>
        public JsonResult DeleteAgent(String agentList)
        {
            String CurrUser = HttpContext.Session["UserId"].ToString();
            Agent agentBll = new Agent();
            int flag = agentBll.DeleteAgent(agentList, CurrUser);
            if (flag == 1)
            {
                return Json(new { error = 1, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = 0, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="agentname"></param>
        /// <returns></returns>
        public FileResult ExportExcel(string agentname = "")
        {

            Agent agentBll = new Agent();
            Dictionary<string, string> queryStr = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(agentname))
            {
                queryStr.Add("AGENT_NAME", agentname);
            }
            DataTable dtExport = agentBll.ExportAgent(queryStr);
            for (int i = 0; i < dtExport.Rows.Count; i++)
            {
                dtExport.Rows[i]["编号"] = i + 1;
            }
            if (dtExport.Rows.Count > 0)
            {
                dtExport.TableName = "代理商数据";
                MemoryStream ms = Common.ExcelTool.RenderToExcel(dtExport);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyy-MM-dd") + "代理商数据.xls");
            }
            else
            {
                return null;
            }
        }
    }
}