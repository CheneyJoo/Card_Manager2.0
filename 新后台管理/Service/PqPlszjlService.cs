using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PqPlszjlService
    {
        public PqPlszjlModel GetModelByQybh(string qybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT TOP 1 * FROM dbo.pq_plszjl WHERE qybh=@qybh ORDER BY id DESC";
                PqPlszjlModel model = conn.Query<PqPlszjlModel>(sql, new { qybh = qybh }).FirstOrDefault();
                return model;
            }
        }

        public void Add(PqPlszjlModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into pq_plszjl(");
            strSql.Append("yybh,qybh,kssj,jssj,cjsj,mx)");
            strSql.Append(" values (");
            strSql.Append("@yybh,@qybh,@kssj,@jssj,@cjsj,@mx)");
            strSql.Append(";select @@IDENTITY");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.ExecuteScalar(strSql.ToString(), model);
            }
        }
    }
}
