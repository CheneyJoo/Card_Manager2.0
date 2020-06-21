using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common
{
    public static class PagingNewHelper
    {
        public static int GetPageCount(int PageSize, int recordCount)
        {
            int PageCount = recordCount % PageSize == 0 ? recordCount / PageSize : recordCount / PageSize + 1;
            if (PageCount < 1) PageCount = 1;
            return PageCount;
        }
        public static MvcHtmlString ShowFPageForBootstrapAdmin(int PageIndex, int PageSize, int recordCount)
        {
            int PageCount = GetPageCount(PageSize, recordCount);
           
            StringBuilder TempHtml = new StringBuilder();
            TempHtml.Append("<div class=\"table_pages\">");
              
            if (PageIndex > 1)
            {
                TempHtml.Append("<span onclick=\"changePostList(" + (PageIndex - 1) + ")\" class=\"page-prev\"><i class=\"layui-icon\">&#xe603;</i></span>");
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
                {
                    midHtml.Append("<a href=\"javascript:void(0)\" class=\"on\">" + i + "</a>");
                }
                else
                {
                    midHtml.Append("<a onclick=\"changePostList(" + i + ")\" href=\"javascript:void(0)\">" + i + "</a>");
                }
            }

            TempHtml.Append(midHtml);

            if (PageIndex < PageCount)
            {
                TempHtml.Append("<span  onclick=\"changePostList(" + (PageIndex + 1) + ")\" class=\"page-next\"><i class=\"layui-icon\">&#xe602;</i></span>");
            }
            TempHtml.Append("	<span class=\"page-count\">共 " + PageCount + " 页</span>&nbsp;&nbsp;<span >共" + recordCount + " 条</span").Append("</div>");          

            return MvcHtmlString.Create(TempHtml.ToString());
        }
            

    }
}
