using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using NPOI.SS.UserModel;

namespace InternetOrder.Controllers
{
    public class BaseController : Controller
    {
        #region 分页相关
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (Request["page"] != null)
                {
                    try
                    {
                        return Int32.Parse(Request["page"]);
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                PageIndex = value;
            }
        }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize = 25;
        /// <summary>
        /// 开始索引
        /// </summary>
        public int StartIndex
        {
            get
            {
                int start = (PageIndex - 1) * PageSize;
                if (PageIndex > 1)
                {
                    start++;
                }
                return start;
            }
        }
        /// <summary>
        /// 结束索引
        /// </summary>
        public int EndIndex
        {
            get
            {
                int end = StartIndex + PageSize;
                if (PageIndex > 1)
                {
                    end--;
                }

                return end;
            }
        }
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            //string userId = filterContext.HttpContext.User.Identity.Name;


            if (GLYadmin.GLYUserAccount == "")
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
            if(GLYadmin.UserId=="")
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
            if(GLYadmin.JsId=="")
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
            if(GLYadmin.YYID=="")
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
            if (GLYadmin.YYID != null)
            {
                ViewBag.YYID = GLYadmin.YYID;
            }
            else
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }

            ViewBag.GLYUserAccount = GLYadmin.GLYUserAccount;


            base.OnActionExecuting(filterContext);
        }

        //获取cell的数据，并设置为对应的数据类型
        public object GetCellValue(IRow row, int index)
        {
            ICell cell = row.GetCell(index);
            if (cell == null)
            {
                return string.Empty;
            }
            object value = null;
            try
            {
                if (cell.CellType != CellType.Blank)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            // Date comes here
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                value = cell.DateCellValue;
                            }
                            else
                            {
                                // Numeric type
                                value = cell.NumericCellValue;
                            }
                            break;
                        case CellType.Boolean:
                            // Boolean type
                            value = cell.BooleanCellValue;
                            break;
                        case CellType.Formula:
                            value = cell.CellFormula;
                            break;
                        default:
                            // String type
                            value = cell.StringCellValue;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                value = "";
            }

            return value;
        }
    }
}