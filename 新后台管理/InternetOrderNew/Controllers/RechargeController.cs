using Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Service;
using SYSEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InternetOrder.Controllers
{
    public class RechargeController : BaseController
    {
        // GET: Recharge
        public ActionResult Index(int rows = 10, int page = 1, int maxpages = 10, string card_no = "", string agen_id = "", string brand_id = "", string begin_date = "", string end_date = "")
        {

            return View(GetRechargeList(rows, page, maxpages, card_no, agen_id, brand_id, begin_date, end_date));
        }

        /// <summary>
        /// 查询代理商列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="maxpages"></param>
        /// <param name="agentname"></param>
        /// <returns></returns>
        public DataTable GetRechargeList(int rows, int page, int maxpages, string card_no, string agen_id, string brand_id, string begin_date, string end_date)
        {
            Recharge rechargeBll = new Recharge();
            Dictionary<string, string> queryStr = new Dictionary<string, string>();
            IDictionary<string, object> result;
            if (!string.IsNullOrEmpty(card_no))
            {
                queryStr.Add("CARD_NO", card_no);
            }
            if (!string.IsNullOrEmpty(agen_id))
            {
                queryStr.Add("AGEN_ID", agen_id);
            }
            if (!string.IsNullOrEmpty(brand_id))
            {
                queryStr.Add("BRAND_ID", brand_id);
            }
            if (!string.IsNullOrEmpty(begin_date))
            {
                queryStr.Add("BEGIN_DATE", begin_date);
            }
            if (!string.IsNullOrEmpty(end_date))
            {
                queryStr.Add("END_DATE", end_date);
            }
            result = rechargeBll.QueryRecharge(rows, page, queryStr);
            ViewBag.Pager = PagingNewHelper.ShowFPageForBootstrapAdmin(page, rows, int.Parse(result["totalRow"].ToString()));//生成分页条      
            DataTable dataTable = result["table"] as DataTable;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataTable.Rows[i]["NO"] = ((i + 1) + (rows * (page - 1)));
            }
            return dataTable;
        }

        public ActionResult List(int rows = 10, int page = 1, int maxpages = 10, string card_no = "", string agen_id = "", string brand_id = "", string begin_date = "", string end_date = "")
        {

            return View(GetRechargeList(rows, page, maxpages, card_no, agen_id, brand_id, begin_date, end_date));
        }

        /// <summary>
        /// 查询代理商
        /// </summary>
        /// <param name="Agent_ID"></param>
        /// <returns></returns>
        public JsonResult QueryAgent(string Agent_ID)
        {
            Recharge rechargeBll = new Recharge();
            DataTable agentData = rechargeBll.QueryAgent();
            return Json(agentData.ToJsJson());
        }

        /// <summary>
        /// 查询品牌
        /// </summary>
        /// <param name="Agent_ID"></param>
        /// <returns></returns>
        public JsonResult QueryBrand(string Agent_ID)
        {
            Recharge rechargeBll = new Recharge();
            DataTable brandData = rechargeBll.QueryBrand();
            return Json(brandData.ToJsJson());
        }

        /// <summary>
        /// 删除消费记录
        /// </summary>
        /// <param name="Recharge_IDs"></param>
        /// <returns></returns>
        public JsonResult DeleteRecharge(String Recharge_IDs)
        {
            Recharge rechargeBll = new Recharge();
            int flag = rechargeBll.DeleteRecharge(Recharge_IDs);
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
        /// 上传Excel
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadExcel()
        {
            //校验文件是否存在 保存文件到服务器 校验文件类型 上传文件格式
            string excelPath = string.Empty;
            string msg = string.Empty;
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                string ext = Path.GetExtension(hfc[0].FileName);//文件后缀
                excelPath = Server.MapPath("~/Content/customer/" + DateTime.Now.ToString("yyyy-MM") + "/");
                if (!Directory.Exists(excelPath))
                {
                    Directory.CreateDirectory(excelPath);
                }

                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                excelPath = excelPath + filename;
                hfc[0].SaveAs(excelPath);
            }
            else
            {
                return Json(new { error = -1, msg = "请选择excel文件再提交上传" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                DataTable importData = ExcelToDataTable(excelPath);
                if (!importData.Columns.Contains("品牌"))
                {
                    return Json(new { error = -1, msg = "Excel模板有误，品牌列不存在" }, JsonRequestBehavior.AllowGet);
                }
                if (!importData.Columns.Contains("运营商"))
                {
                    return Json(new { error = -1, msg = "Excel模板有误，运营商列不存在" }, JsonRequestBehavior.AllowGet);
                    if (!importData.Columns.Contains("运营商"))
                    {
                        return Json(new { error = -1, msg = "Excel模板有误，运营商列不存在" }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!importData.Columns.Contains("代理商"))
                {
                    return Json(new { error = -1, msg = "Excel模板有误，代理商列不存在" }, JsonRequestBehavior.AllowGet);
                }
                if (!importData.Columns.Contains("卡号"))
                {
                    return Json(new { error = -1, msg = "Excel模板有误，卡号列不存在" }, JsonRequestBehavior.AllowGet);
                }
                if (!importData.Columns.Contains("充值金额"))
                {
                    return Json(new { error = -1, msg = "Excel模板有误，充值金额列不存在" }, JsonRequestBehavior.AllowGet);
                }
                if (!importData.Columns.Contains("充值时间"))
                {
                    return Json(new { error = -1, msg = "Excel模板有误，充值时间列不存在" }, JsonRequestBehavior.AllowGet);
                }
                if (!importData.Columns.Contains("套餐"))
                {
                    return Json(new { error = -1, msg = "Excel模板有误，套餐列不存在" }, JsonRequestBehavior.AllowGet);
                }
                List<TB_DATA_RECHARGE> recharges = new List<TB_DATA_RECHARGE>();
                StringBuilder stringBuilde = new StringBuilder();
                int success_count = 0, fail_count = 0;
                for (int i = 0; i < importData.Rows.Count; i++)
                {
                    int flag = 0;
                    String brand = String.Empty, service_provider = String.Empty, agent = String.Empty, card_no = String.Empty, recharge_money = String.Empty, recharge_date = String.Empty, package_name = String.Empty;
                    brand = Convert.ToString(importData.Rows[i]["品牌"]);
                    service_provider = Convert.ToString(importData.Rows[i]["运营商"]);
                    agent = Convert.ToString(importData.Rows[i]["代理商"]);
                    card_no = Convert.ToString(importData.Rows[i]["卡号"]);
                    recharge_money = Convert.ToString(importData.Rows[i]["充值金额"]);
                    recharge_date = Convert.ToString(importData.Rows[i]["充值时间"]);
                    package_name = Convert.ToString(importData.Rows[i]["套餐"]);
                    Decimal _recharge_money = 0;
                    DateTime _recharge_date = DateTime.MinValue;

                    if (String.IsNullOrEmpty(brand.Trim()))
                    {
                        stringBuilde.Append("第" + (i + 1) + "行，品牌为空;");
                        flag = 1;
                    }
                    if (String.IsNullOrEmpty(service_provider.Trim()))
                    {
                        stringBuilde.Append("第" + (i + 1) + "行，运营商为空;");
                        flag = 1;
                    }
                    if (String.IsNullOrEmpty(agent.Trim()))
                    {
                        stringBuilde.Append("第" + (i + 1) + "行，代理商为空;");
                        flag = 1;
                    }
                    if (String.IsNullOrEmpty(card_no.Trim()))
                    {
                        stringBuilde.Append("第" + (i + 1) + "行，卡号为空;");
                        flag = 1;
                    }
                    if (String.IsNullOrEmpty(recharge_money.Trim()))
                    {
                        stringBuilde.Append("第" + (i + 1) + "行，充值金额为空;");
                        flag = 1;
                    }
                    else
                    {
                        if (!Decimal.TryParse(recharge_money, out _recharge_money))
                        {
                            stringBuilde.Append("第" + (i + 1) + "行，充值金额格式有误;");
                            flag = 1;
                        }
                    }
                    if (String.IsNullOrEmpty(recharge_date.Trim()))
                    {
                        stringBuilde.Append("第" + (i + 1) + "行，充值时间为空;");
                        flag = 1;
                    }
                    else
                    {
                        if (!DateTime.TryParse(recharge_date, out _recharge_date))
                        {
                            stringBuilde.Append("第" + (i + 1) + "行，充值时间格式有误;");
                            flag = 1;
                        }
                    }
                    if (String.IsNullOrEmpty(package_name.Trim()))
                    {
                        stringBuilde.Append("第" + (i + 1) + "行，套餐为空;");
                        flag = 1;
                    }
                    if (flag == 0)
                    {
                        String CurrUser = HttpContext.Session["UserId"].ToString();
                        TB_DATA_RECHARGE recharge = new TB_DATA_RECHARGE();
                        recharge.RECHARGE_ID = Guid.NewGuid().ToString();
                        recharge.BRAND_ID = brand.Trim();
                        recharge.SERVICE_PROVIDER = service_provider.Trim();
                        recharge.AGEN_ID = agent.Trim();
                        recharge.CARD_NO = card_no.Trim();
                        recharge.RECHARGE_MONEY = _recharge_money;
                        recharge.RECHARGE_DATE = _recharge_date;
                        recharge.PACKAGE_NAME = package_name;
                        recharge.CREATED_BY = CurrUser;
                        recharge.CREATED_TIME = DateTime.Now;
                        recharge.LAST_UPDATED_BY = CurrUser;
                        recharge.LAST_UPDATED_TIME = DateTime.Now;
                        recharges.Add(recharge);
                        success_count += 1;
                    }
                    else
                        fail_count += 1;
                }
                Recharge rechargeBll = new Recharge();
                int result = rechargeBll.SaveRechargeList(recharges);
                if (result == 1)
                {
                    if (fail_count == 0)
                    {
                        return Json(new { error = 1, msg = "保存成功,成功数据数量" + success_count + "条" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = 1, msg = "未全部保存成功,成功数据数量" + success_count + "条，失败数据数量" + fail_count + "条，失败缘由：" + stringBuilde.ToString() }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { error = 0, msg = "保存失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new { error = -1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="card_no"></param>
        /// <param name="agen_id"></param>
        /// <param name="brand_id"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public FileResult ExportExcel(string card_no = "", string agen_id = "", string brand_id = "", string begin_date = "", string end_date = "")
        {
            Recharge rechargeBll = new Recharge();
            IDictionary<string, string> queryStr=new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(card_no))
            {
                queryStr.Add("CARD_NO", card_no);
            }
            if (!string.IsNullOrEmpty(agen_id))
            {
                queryStr.Add("AGEN_ID", agen_id);
            }
            if (!string.IsNullOrEmpty(brand_id))
            {
                queryStr.Add("BRAND_ID", brand_id);
            }
            if (!string.IsNullOrEmpty(begin_date))
            {
                queryStr.Add("BEGIN_DATE", begin_date);
            }
            if (!string.IsNullOrEmpty(end_date))
            {
                queryStr.Add("END_DATE", end_date);
            }
            DataTable dtExport = rechargeBll.ExportRecharge(queryStr);
            for (int i = 0; i < dtExport.Rows.Count; i++)
            {
                dtExport.Rows[i]["编号"] = i + 1;
            }
            if (dtExport.Rows.Count > 0)
            {
                dtExport.TableName = "消费数据";
                MemoryStream ms = Common.ExcelTool.RenderToExcel(dtExport);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyy-MM-dd") + "消费数据.xls");
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 读取excel 默认第一行为表头
        /// </summary>
        /// <param name="strFileName">excel文档绝对路径</param>
        /// <returns></returns>
        private DataTable ExcelToDataTable(string strFileName)
        {
            DataTable dt = new DataTable();

            IWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {

                if (strFileName.IndexOf(".xlsx") > 0)
                {
                    hssfworkbook = new XSSFWorkbook(file);
                }
                else if (strFileName.IndexOf(".xls") > 0)
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                else
                {
                    throw new Exception("Excel格式错误");
                }


            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);

            IRow headRow = sheet.GetRow(0);
            int colCount = headRow.LastCellNum;
            if (headRow != null)
            {

                for (int i = 0; i < colCount; i++)
                {
                    ICell cell = headRow.GetCell(i);
                    if (cell != null)
                    {
                        if (dt.Columns.Contains(cell.ToString()))
                        {

                            throw new Exception("Excel存在重复列，列名为：" + cell.ToString());
                        }
                        dt.Columns.Add(cell.ToString());
                    }
                    else
                    {
                        dt.Columns.Add("column" + i);
                    }
                }
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                bool emptyRow = true;
                object[] itemArray = null;

                if (row != null)
                {
                    itemArray = new object[colCount];
                    if (row.FirstCellNum != -1)
                    {
                        for (int j = row.FirstCellNum; j < colCount; j++)
                        {
                            if (row.GetCell(j) != null)
                            {

                                switch (row.GetCell(j).CellType)
                                {
                                    case CellType.Numeric:
                                        if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))//日期类型
                                        {
                                            itemArray[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                                        }
                                        else//其他数字类型
                                        {
                                            itemArray[j] = row.GetCell(j).NumericCellValue;
                                        }
                                        break;
                                    case CellType.Blank:
                                        itemArray[j] = string.Empty;
                                        break;
                                    case CellType.Formula:
                                        if (Path.GetExtension(strFileName).ToLower().Trim() == ".xlsx")
                                        {
                                            XSSFFormulaEvaluator eva = new XSSFFormulaEvaluator(hssfworkbook);
                                            if (eva.Evaluate(row.GetCell(j)).CellType == CellType.Numeric)
                                            {
                                                if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))//日期类型
                                                {
                                                    itemArray[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                                                }
                                                else//其他数字类型
                                                {
                                                    itemArray[j] = row.GetCell(j).NumericCellValue;
                                                }
                                            }
                                            else
                                            {
                                                itemArray[j] = eva.Evaluate(row.GetCell(j)).StringValue;
                                            }
                                        }
                                        else
                                        {
                                            HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(hssfworkbook);
                                            if (eva.Evaluate(row.GetCell(j)).CellType == CellType.Numeric)
                                            {
                                                if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))//日期类型
                                                {
                                                    itemArray[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                                                }
                                                else//其他数字类型
                                                {
                                                    itemArray[j] = row.GetCell(j).NumericCellValue;
                                                }
                                            }
                                            else
                                            {
                                                itemArray[j] = eva.Evaluate(row.GetCell(j)).StringValue;
                                            }
                                        }
                                        break;
                                    default:
                                        itemArray[j] = row.GetCell(j).StringCellValue;
                                        break;

                                }

                                if (itemArray[j] != null && !string.IsNullOrEmpty(itemArray[j].ToString().Trim()))
                                {
                                    emptyRow = false;
                                }
                            }
                        }
                    }
                }
                //非空数据行数据添加到DataTable
                if (!emptyRow)
                {
                    dt.Rows.Add(itemArray);
                }
            }
            return dt;
        }
    }
}