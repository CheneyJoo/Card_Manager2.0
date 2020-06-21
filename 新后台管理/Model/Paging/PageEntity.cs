using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Paging
{
    public class PageEntity
    {
        public string TableName { get; set; }
        public string _TableName
        {
            get
            {
                return TableName;
            }
        }

        public string Fields { get; set; }
        public string _Fields
        {
            get
            {
                return Fields ?? "*";
            }
        }

        public string WhereStr { get; set; }
        public string _WhereStr
        {
            get
            {
                return WhereStr ?? "";
            }
        }
        public string yyzt { get; set; }
        public string _yyzt
        {
            get
            {
                return yyzt ?? "";
            }
        }
        public string OrderBy { get; set; }
        public string _OrderBy
        {
            get
            {
                return OrderBy ?? "Id desc";
            }
        }

        public int PageIndex { get; set; }
        public int _PageIndex
        {
            get
            {
                return PageIndex == 0 ? 1 : PageIndex;
            }
        }

        public int PageSize { get; set; }
        public int _PageSize
        {
            get
            {
                return PageSize == 0 ? 10 : PageSize;
            }
        }
    }
}
