using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace Common
{
    public static  class commonModel
    {
        /// <summary>
        /// 站点目录
        /// </summary>
       public static string website = ConfigurationManager.AppSettings["website"];
    }
}
