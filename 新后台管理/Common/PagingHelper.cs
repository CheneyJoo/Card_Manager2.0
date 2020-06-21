using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common
{
    public static class PagingHelper
    {
        public static IQueryable<T> GetPageList<T>(IOrderedQueryable<T> List, int PageIndex, int PageSize)
        {
            int PageCount = GetPageCount(PageSize, List.Count());
            PageIndex = CheckPageIndex(PageIndex, PageCount);
            return List.Skip((PageIndex - 1) * PageSize).Take(PageSize);
        }

        public static int GetPageCount(int PageSize, int recordCount)
        {
            int PageCount = recordCount % PageSize == 0 ? recordCount / PageSize : recordCount / PageSize + 1;
            if (PageCount < 1) PageCount = 1;
            return PageCount;
        }

        public static int CheckPageIndex(int PageIndex, int PageCount)
        {
            if (PageIndex > PageCount) PageIndex = PageCount;
            if (PageIndex < 1) PageIndex = 1;
            return PageIndex;
        }

        public enum FPageMode { Normal, Numeric, GroupNumeric, AdminNumeric }
        public static MvcHtmlString ShowFPage(string urlFormat, int PageIndex, int PageSize, int recordCount, FPageMode Mode)
        {
            if (recordCount < 1)
            {
                return MvcHtmlString.Create(null);
            }
            urlFormat = urlFormat.Replace("%7B0%7D", "{0}");
            int PageCount = GetPageCount(PageSize, recordCount);

            StringBuilder TempHtml = new StringBuilder();
            //TempHtml.AppendFormat("总共{0}条记录,共{1}页,当前第{2}页&nbsp;&nbsp;", recordCount, PageCount, PageIndex);
            if (PageIndex == 1)
            {
                //TempHtml.Append("首页&nbsp;&nbsp;&nbsp;&nbsp;上一页&nbsp;");
            }
            else
            {
                TempHtml.AppendFormat(" <a href=\"{0}\"><span>←前一页</span></a>&nbsp;", string.Format(urlFormat, PageIndex - 1));
            }
            // 数字分页
            switch (Mode)
            {
                case FPageMode.Numeric:
                    TempHtml.Append(GetNumericPage(urlFormat, PageIndex, PageSize, PageCount));
                    break;
                case FPageMode.GroupNumeric:
                    TempHtml.Append(GetGroupNumericPage(urlFormat, PageIndex, PageSize, PageCount));
                    break;
            }

            if (PageIndex == PageCount)
            {
                //TempHtml.Append("下一页&nbsp;&nbsp;&nbsp;&nbsp;末页");
            }
            else
            {
                TempHtml.AppendFormat("<label>|</label>&nbsp;<a href=\"{0}\"><span>后一页→</span></a>", string.Format(urlFormat, PageIndex + 1));
                //.AppendFormat("<a href=\"{0}\">末页</a>", string.Format(urlFormat, PageCount));
            }



            return MvcHtmlString.Create(TempHtml.ToString());
        }
        public static MvcHtmlString ShowAdminFPage(int PageIndex, int PageSize, int recordCount, FPageMode Mode)
        {
            if (recordCount <= PageSize)
            {
                return MvcHtmlString.Create(null);
            }

            int PageCount = GetPageCount(PageSize, recordCount);

            StringBuilder TempHtml = new StringBuilder();
            TempHtml.AppendFormat("总共{0}条记录,共{1}页,当前第{2}页&nbsp;&nbsp;", recordCount, PageCount, PageIndex);
            if (PageIndex == 1)
            {
                TempHtml.Append("首页&nbsp;&nbsp;&nbsp;&nbsp;上一页&nbsp;");
            }
            else
            {
                TempHtml.AppendFormat("<a name=\"{0}\" href=\"javascript:void(0)\">首页</a>&nbsp;&nbsp;&nbsp;&nbsp;", 1)
                    .AppendFormat("<a name=\"{0}\" href=\"javascript:void(0)\">上一页</a>&nbsp;", PageIndex - 1);
            }
            // 数字分页
            switch (Mode)
            {
                case FPageMode.AdminNumeric:
                    TempHtml.Append(GetAdminNumericPage(PageIndex, PageSize, PageCount));
                    break;
            }

            if (PageIndex == PageCount)
            {
                TempHtml.Append("下一页&nbsp;&nbsp;&nbsp;&nbsp;末页");
            }
            else
            {
                TempHtml.AppendFormat("<a name=\"{0}\" href=\"javascript:void(0)\">下一页</a>&nbsp;&nbsp;&nbsp;&nbsp;", PageIndex + 1)
                    .AppendFormat("<a name=\"{0}\" href=\"javascript:void(0)\">末页</a>", PageCount);
            }



            return MvcHtmlString.Create(TempHtml.ToString());
        }
        public static MvcHtmlString ShowFPage(string urlFormat, int PageIndex, int PageSize, int recordCount)
        {
            urlFormat = urlFormat.Replace("%7B0%7D", "{0}");
            int PageCount = GetPageCount(PageSize, recordCount);

            StringBuilder TempHtml = new StringBuilder();
            //TempHtml.AppendFormat("总共{0}条记录,共{1}页,当前第{2}页&nbsp;&nbsp;", recordCount, PageCount, PageIndex);
            if (PageIndex == 1)
            {
                TempHtml.Append("首页&nbsp;&nbsp;&nbsp;&nbsp;上一页&nbsp;");
            }
            else
            {
                TempHtml.AppendFormat("<a href=\"{0}\">首页</a>&nbsp;&nbsp;&nbsp;&nbsp;", string.Format(urlFormat, 1))
                    .AppendFormat("<a href=\"{0}\">上一页</a>&nbsp;", string.Format(urlFormat, PageIndex - 1));
            }

            int SpanNum = 4;
            //int BeginNum = pageIndex - (SpanNum - 1) / 2;
            //if (BeginNum < 1) BeginNum = 1;
            //int EndNum = pageIndex + (SpanNum - 1) / 2;
            int BeginNum = PageIndex - SpanNum;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = PageIndex + SpanNum;
            if (EndNum < SpanNum * 2 + 1) EndNum = SpanNum * 2 + 1;
            if (EndNum > PageCount) EndNum = PageCount;

            StringBuilder midHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == PageIndex)
                    midHtml.AppendFormat("<span style=\"color:red\">{0}</span>&nbsp;", i);
                else
                    midHtml.AppendFormat("<a href=\"{0}\">{1}</a>&nbsp;", string.Format(urlFormat, i), i);
            }



            TempHtml.Append(midHtml);








            if (PageIndex == PageCount)
            {
                TempHtml.Append("下一页&nbsp;&nbsp;&nbsp;&nbsp;末页");
            }
            else
            {
                TempHtml.AppendFormat("<a href=\"{0}\">下一页</a>&nbsp;&nbsp;&nbsp;&nbsp;", string.Format(urlFormat, PageIndex + 1))
                    .AppendFormat("<a href=\"{0}\">末页</a>", string.Format(urlFormat, PageCount));
            }



            return MvcHtmlString.Create(TempHtml.ToString());
        }

        public static MvcHtmlString ShowUnionsFPage(string urlFormat, int PageIndex, int PageSize, int recordCount)
        {
            urlFormat = urlFormat.Replace("%7B0%7D", "{0}");
            int PageCount = GetPageCount(PageSize, recordCount);
            StringBuilder TempHtml = new StringBuilder();
            //TempHtml.AppendFormat("总共{0}条记录,共{1}页,当前第{2}页&nbsp;&nbsp;", recordCount, PageCount, PageIndex);
            TempHtml.Append("<dl>");
            if (PageIndex == 1)
            {
                TempHtml.AppendFormat("<dt><a href=\"{0}\">首页</a></dt>", string.Format(urlFormat, 1))
                    .AppendFormat("<dt><a href= \"{0}\">上一页</a></dt>", string.Format(urlFormat, 1));
            }
            else
            {
                TempHtml.AppendFormat("<dt><a href=\"{0}\">首页</a></dt>", string.Format(urlFormat, 1))
                    .AppendFormat("<dt><a href= \"{0}\">上一页</a></dt>", string.Format(urlFormat, PageIndex - 1));
            }
            int SpanNum = 4;
            int BeginNum = PageIndex - SpanNum;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = PageIndex + SpanNum;
            if (EndNum < SpanNum * 2 + 1) EndNum = SpanNum * 2 + 1;
            if (EndNum > PageCount) EndNum = PageCount;
            StringBuilder midHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == PageIndex)
                    midHtml.AppendFormat("<dt class=\"thisclass\"><a href=\"{0}\">{1}</a></dt>", string.Format(urlFormat, i), i);
                else
                    midHtml.AppendFormat("<dt><a href=\"{0}\">{1}</a></dt>", string.Format(urlFormat, i), i);
            }
            TempHtml.Append(midHtml);
            if (PageIndex == PageCount)
            {
                TempHtml.AppendFormat("<dt><a href=\"{0}\">下一页</a></dt>", string.Format(urlFormat, PageIndex))
                    .AppendFormat("<dt><a href= \"{0}\">末页</a></dt>", string.Format(urlFormat, PageIndex));
            }
            else
            {
                TempHtml.AppendFormat("<dt><a href=\"{0}\">下一页</a></dt>", string.Format(urlFormat, PageIndex + 1))
                    .AppendFormat("<dt><a href=\"{0}\">末页</a></dt>", string.Format(urlFormat, PageCount));
            }
            TempHtml.Append("</dl>");
            return MvcHtmlString.Create(TempHtml.ToString());
        }

        public static MvcHtmlString ShowFPageForDefaultAdmin(int PageIndex, int PageSize, int recordCount)
        {
            int PageCount = GetPageCount(PageSize, recordCount);
            //string urlFormat = "";
            StringBuilder TempHtml = new StringBuilder();
            //TempHtml.AppendFormat("总共{0}条记录,共{1}页,当前第{2}页&nbsp;&nbsp;", recordCount, PageCount, PageIndex);
            if (PageIndex == 1)
            {
                TempHtml.Append("首页&nbsp;&nbsp;上一页&nbsp;");
            }
            else
            {
                TempHtml.Append("<a  onclick=\"changePostList(1)\" href=\"javascript:void(0)\">首页</a>&nbsp;&nbsp;")
                    .Append("<a onclick=\"changePostList(" + (PageIndex - 1) + ")\" href=\"javascript:void(0)\">上一页</a>&nbsp;");
            }

            int SpanNum = 4;
            //int BeginNum = pageIndex - (SpanNum - 1) / 2;
            //if (BeginNum < 1) BeginNum = 1;
            //int EndNum = pageIndex + (SpanNum - 1) / 2;
            int BeginNum = PageIndex - SpanNum;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = PageIndex + SpanNum;
            if (EndNum < SpanNum * 2 + 1) EndNum = SpanNum * 2 + 1;
            if (EndNum > PageCount) EndNum = PageCount;

            StringBuilder midHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == PageIndex)
                    midHtml.Append("<span style=\"color:red\">" + i + "</span>&nbsp;");
                else
                    midHtml.Append("<a onclick=\"changePostList(" + i + ")\" href=\"javascript:void(0)\">" + i + "</a>&nbsp;");
            }

            TempHtml.Append(midHtml);

            if (PageIndex == PageCount)
            {
                TempHtml.Append("下一页&nbsp;&nbsp;末页");
            }
            else
            {
                TempHtml.Append("<a onclick=\"changePostList(" + (PageIndex + 1) + ")\" href=\"javascript:void(0)\">下一页</a>&nbsp;&nbsp;")
                    .Append("<a onclick=\"changePostList(" + PageCount + ")\" href=\"javascript:void(0)\">末页</a>");
            }

            TempHtml.Append("&nbsp;&nbsp;共：" + recordCount + "条记录，" + PageCount + "页&nbsp;&nbsp;");
            return MvcHtmlString.Create(TempHtml.ToString());
        }

        public static MvcHtmlString ShowFPageForBootstrapAdmin(int PageIndex, int PageSize, int recordCount)
        {
            int PageCount = GetPageCount(PageSize, recordCount);
            //if (PageCount == 1)
            //{
            //    return MvcHtmlString.Empty;
            //}
            StringBuilder TempHtml = new StringBuilder();
            TempHtml.Append("<div class=\"row\">")
                .Append("<div class=\"col-sm-12\">")
                .Append("<div class=\"dataTables_paginate paging_bootstrap\">")
                .Append("<ul class=\"pagination\">");
            if (PageIndex > 1)
            {
                TempHtml.Append("<li class=\"prev\"><a onclick=\"changePostList(1)\" href=\"javascript:void(0)\"><i class=\"ace-icon fa fa-angle-double-left\"></i></a></li>");
            }
            int SpanNum = 4;
            int BeginNum = PageIndex - SpanNum;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = PageIndex + SpanNum;
            if (EndNum < SpanNum * 2 + 1) EndNum = SpanNum * 2 + 1;
            if (EndNum > PageCount) EndNum = PageCount;

            StringBuilder midHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == PageIndex)
                    midHtml.Append("<li class=\"active\"><a href=\"javascript:void(0)\">" + i + "</a></li>");
                else
                    midHtml.Append("<li><a onclick=\"changePostList(" + i + ")\" href=\"javascript:void(0)\">" + i + "</a></li>");
            }

            TempHtml.Append(midHtml);

            if (PageIndex < PageCount)
            {
                TempHtml.Append("<li class=\"next\"><a onclick=\"changePostList(" + PageCount + ")\" href=\"javascript:void(0)\"><i class=\"ace-icon fa fa-angle-double-right\"></i></a></li>");
            }
            TempHtml.Append("<li><a href=\"javascript:;\">共" + PageCount + "页</a></li>").Append("<li><a href=\"javascript:;\">" + recordCount + "条记录</a></li>");
            TempHtml.Append("</ul>").Append("</div>").Append("</div>").Append("</div>");

            return MvcHtmlString.Create(TempHtml.ToString());
        }

        public static MvcHtmlString ShowFPageForNomal(int PageIndex, int PageSize, int recordCount)
        {
            int PageCount = GetPageCount(PageSize, recordCount);
            //string urlFormat = "";
            StringBuilder TempHtml = new StringBuilder("<div class=\"page\"><label>");
            //TempHtml.AppendFormat("总共{0}条记录,共{1}页,当前第{2}页&nbsp;&nbsp;", recordCount, PageCount, PageIndex);
            if (PageIndex == 1)
            {
                TempHtml.Append("首页&nbsp;上一页&nbsp;");
            }
            else
            {
                TempHtml.Append("<a  onclick=\"changePostList(1)\" href=\"javascript:void(0)\">首页</a>&nbsp;")
                    .Append("<a onclick=\"changePostList(" + (PageIndex - 1) + ")\" href=\"javascript:void(0)\">上一页</a>&nbsp;");
            }

            int SpanNum = 4;
            //int BeginNum = pageIndex - (SpanNum - 1) / 2;
            //if (BeginNum < 1) BeginNum = 1;
            //int EndNum = pageIndex + (SpanNum - 1) / 2;
            int BeginNum = PageIndex - SpanNum;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = PageIndex + SpanNum;
            if (EndNum < SpanNum * 2 + 1) EndNum = SpanNum * 2 + 1;
            if (EndNum > PageCount) EndNum = PageCount;

            StringBuilder midHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == PageIndex)
                    midHtml.Append("<span style=\"color:red\">" + i + "</span>&nbsp;");
                else
                    midHtml.Append("<a onclick=\"changePostList(" + i + ")\" href=\"javascript:void(0)\">" + i + "</a>&nbsp;");
            }

            TempHtml.Append(midHtml);

            if (PageIndex == PageCount)
            {
                TempHtml.Append("下一页&nbsp;末页");
            }
            else
            {
                TempHtml.Append("<a onclick=\"changePostList(" + (PageIndex + 1) + ")\" href=\"javascript:void(0)\">下一页</a>&nbsp;")
                    .Append("<a onclick=\"changePostList(" + PageCount + ")\" href=\"javascript:void(0)\">末页</a>");
            }
            TempHtml.Append("<input id=\"goPageNum\" onkeyup=\"checkIsNum(this)\" data-value=\"1\" name=\"goPageNum\" type=\"text\" size=\"3\" /></label><label><input type=\"button\" onclick=\"goPostList()\" name=\"button\" value=\" GO \" />");
            TempHtml.Append("</label></div>");
            return MvcHtmlString.Create(TempHtml.ToString());
        }

        public static MvcHtmlString ShowFPageForHrefNomal(string urlFormat, int PageIndex, int PageSize, int recordCount)
        {
            int PageCount = GetPageCount(PageSize, recordCount);
            //string urlFormat = "";
            StringBuilder TempHtml = new StringBuilder("<label>");
            //TempHtml.AppendFormat("总共{0}条记录,共{1}页,当前第{2}页&nbsp;&nbsp;", recordCount, PageCount, PageIndex);
            if (PageIndex == 1)
            {
                TempHtml.Append("首页&nbsp;上一页&nbsp;");
            }
            else
            {
                TempHtml.Append("<a href=\"" + string.Format(urlFormat, 1) + "\">首页</a>&nbsp;")
                    .Append("<a href=\"" + string.Format(urlFormat, PageIndex - 1) + "\">上一页</a>&nbsp;");
            }

            int SpanNum = 4;
            //int BeginNum = pageIndex - (SpanNum - 1) / 2;
            //if (BeginNum < 1) BeginNum = 1;
            //int EndNum = pageIndex + (SpanNum - 1) / 2;
            int BeginNum = PageIndex - SpanNum;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = PageIndex + SpanNum;
            if (EndNum < SpanNum * 2 + 1) EndNum = SpanNum * 2 + 1;
            if (EndNum > PageCount) EndNum = PageCount;

            StringBuilder midHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == PageIndex)
                    midHtml.Append("<span style=\"color:red\">" + i + "</span>&nbsp;");
                else
                    midHtml.Append("<a href=\"" + string.Format(urlFormat, i) + "\">" + i + "</a>&nbsp;");
            }

            TempHtml.Append(midHtml);

            if (PageIndex == PageCount)
            {
                TempHtml.Append("下一页&nbsp;末页");
            }
            else
            {
                TempHtml.Append("<a href=\"" + string.Format(urlFormat, PageIndex + 1) + "\">下一页</a>&nbsp;")
                    .Append("<a href=\"" + string.Format(urlFormat, PageCount) + "\">末页</a>");
            }
            TempHtml.Append("<input id=\"goPageNum\" onkeyup=\"checkIsNum(this)\" data-value=\"1\" name=\"goPageNum\" type=\"text\" size=\"3\" /></label><label><input type=\"button\" onclick=\"goToPage('" + urlFormat + "')\" name=\"button\" value=\" GO \" />");
            TempHtml.Append("</label>");
            return MvcHtmlString.Create(TempHtml.ToString());
        }


        public static MvcHtmlString ShowFPageForqd(int PageIndex, int PageSize, int recordCount)
        {
            int PageCount = GetPageCount(PageSize, recordCount);
            StringBuilder TempHtml = new StringBuilder();

            if (PageIndex > 1)
            {
                TempHtml.Append("<a onclick=\"changePostList(1)\" href=\"javascript:void(0)\"><</a>");
            }
            int SpanNum = 4;
            int BeginNum = PageIndex - SpanNum;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = PageIndex + SpanNum;
            if (EndNum < SpanNum * 2 + 1) EndNum = SpanNum * 2 + 1;
            if (EndNum > PageCount) EndNum = PageCount;

            StringBuilder midHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == PageIndex)
                    midHtml.Append("<a href=\"javascript:void(0)\" class=\"current\">" + i + "</a>");
                else
                    midHtml.Append("<a onclick=\"changePostList(" + i + ")\" href=\"javascript:void(0)\">" + i + "</a>");
            }

            TempHtml.Append(midHtml);

            if (PageIndex < PageCount)
            {
                TempHtml.Append("<a onclick=\"changePostList(" + PageCount + ")\" href=\"javascript:void(0)\">></a>");
            }
            return MvcHtmlString.Create(TempHtml.ToString());
        }

        /// <summary>
        /// 分组数字分页
        /// </summary>
        /// <param name="urlFormat"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static string GetGroupNumericPage(string urlFormat, int pageIndex, int pageSize, int pageCount)
        {
            int GroupChildCount = 10; // 分组显示个数
            int DGroup = pageIndex / GroupChildCount; //当前组
            int GroupCount = pageCount / GroupChildCount;      //组数

            //如果正好是当前组最后一页 不进入下一组
            if (pageIndex % GroupChildCount == 0) DGroup--;

            //当前组数量
            int GroupSpan = (DGroup == GroupCount) ? pageCount % GroupChildCount : GroupChildCount;

            StringBuilder TempHtml = new StringBuilder();
            for (int i = DGroup * GroupChildCount + 1; i <= DGroup * GroupChildCount + GroupSpan; i++)
            {
                if (i == pageIndex)
                    TempHtml.AppendFormat("<span style=\"color:red\">{0}</span>&nbsp;", i);
                else
                    TempHtml.AppendFormat("<a href=\"{0}\">{1}</a>&nbsp;", string.Format(urlFormat, i), i);
            }
            return TempHtml.ToString();
        }

        /// <summary>
        /// 数字分页
        /// </summary>
        /// <param name="urlFormat"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static string GetNumericPage(string urlFormat, int pageIndex, int pageSize, int pageCount)
        {
            if (pageCount < 2)
            {
                return null;
            }
            int SpanNum = 4;
            //int BeginNum = pageIndex - (SpanNum - 1) / 2;
            //if (BeginNum < 1) BeginNum = 1;
            //int EndNum = pageIndex + (SpanNum - 1) / 2;
            int BeginNum = pageIndex - SpanNum;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = pageIndex + SpanNum;
            if (EndNum < SpanNum * 2 + 1) EndNum = SpanNum * 2 + 1;
            if (EndNum > pageCount) EndNum = pageCount;

            StringBuilder TempHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == pageIndex)
                    TempHtml.AppendFormat("<label>|</label><span class=\"dj\">{0}</span>", i);
                else
                    TempHtml.AppendFormat("<label>|</label><a href=\"{0}\"><span >{1}</span></a>", string.Format(urlFormat, i), i);
            }
            return TempHtml.ToString();
        }
        /// <summary>
        /// 后台数字分页
        /// </summary>
        /// <param name="urlFormat"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static string GetAdminNumericPage(int pageIndex, int pageSize, int pageCount)
        {
            int SpanNum = 4;
            //int BeginNum = pageIndex - (SpanNum - 1) / 2;
            //if (BeginNum < 1) BeginNum = 1;
            //int EndNum = pageIndex + (SpanNum - 1) / 2;
            int BeginNum = pageIndex - SpanNum;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = pageIndex + SpanNum;
            if (EndNum < SpanNum * 2 + 1) EndNum = SpanNum * 2 + 1;
            if (EndNum > pageCount) EndNum = pageCount;

            StringBuilder TempHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == pageIndex)
                    TempHtml.AppendFormat("<span style=\"color:red\">{0}</span>&nbsp;", i);
                else
                    TempHtml.AppendFormat("<a name=\"{0}\" href=\"javascript:void(0)\">{1}</a>&nbsp;", i, i);
            }
            return TempHtml.ToString();
        }

    }
}
