using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PagingService
    {
        public List<T> GetPage<T>(Model.Paging.PageEntity pageEntity, out int totalRecord)
        {        
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@TableName", pageEntity._TableName);
            dp.Add("@Fields", pageEntity._Fields);
            dp.Add("@WhereStr", pageEntity._WhereStr);
            dp.Add("@OrderBy", pageEntity._OrderBy);
            dp.Add("@PageIndex", pageEntity._PageIndex);
            dp.Add("@PageSize", pageEntity._PageSize);
            dp.Add("@totalRecord", 0, DbType.Int32, ParameterDirection.Output);

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<T> list = conn.Query<T>("PROC_SqlPage", dp, null, true, null, CommandType.StoredProcedure).ToList();
                totalRecord = dp.Get<int>("@totalRecord");//获取数据库输出的值
                return list;
            }
        }
    }
}
