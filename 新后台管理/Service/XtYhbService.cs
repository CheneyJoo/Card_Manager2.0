using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Service
{
    public class XtYhbService
    {
        public void Login(XtYhbModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("");
        }

        public string GetZH()
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BM", "xt_yhb");
            dp.Add("@LSH", "", DbType.String, ParameterDirection.Output);
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute("PROC_XT_ZZB_LSBH", dp, null, null, CommandType.StoredProcedure);
                return dp.Get<string>("@LSH");
            }
        }

        public XtYhbModel GetModel(XtYhbModel model)
        {
            string sql = "SELECT top 1 * FROM dbo.xt_yhb WHERE dh=@dh AND yybh=@yybh AND mm=@mm AND xm=@xm";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.QueryFirstOrDefault<XtYhbModel>(sql,model);
            }
        }
        public XtYhbModel GetModel(string zh)
        {
            string sql = "SELECT top 1 * FROM dbo.xt_yhb WHERE zh=@zh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.QueryFirstOrDefault<XtYhbModel>(sql, new { zh=zh});
            }
        }

        public void Add(XtYhbModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into xt_yhb(");
            strSql.Append("zh,xm,dh,mm,cjsj,yybh)");
            strSql.Append(" values (");
            strSql.Append("@zh,@xm,@dh,@mm,@cjsj,@yybh)");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                 conn.Execute(strSql.ToString(), model);
            }
        }

       
    }
}
