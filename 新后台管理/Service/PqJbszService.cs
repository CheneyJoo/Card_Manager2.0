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
    public class PqJbszService
    {
        public PqJbszModel GetModel(string tjjgid)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT * FROM dbo.pq_jbsz WHERE tjjgid=@tjjgid";
                PqJbszModel model = conn.Query<PqJbszModel>(sql, new { tjjgid = tjjgid }).FirstOrDefault();
                return model;
            }
        }

        public bool Update(PqJbszModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update pq_jbsz set ");
            strSql.Append("zdjd=@zdjd,");
            strSql.Append("skyl=@skyl,");
            strSql.Append("qtyl=@qtyl,");
            strSql.Append("tjyl=@tjyl,");
            strSql.Append("xxr=@xxr,");
            strSql.Append("tsky=@tsky,");
            strSql.Append("ztyy=@ztyy,");
            strSql.Append("qdyl=@qdyl");
            strSql.Append(" where tjjgid=@tjjgid");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                int num = conn.Execute(strSql.ToString(), model);
                return num > 0;
            }
        }

        public int Add(PqJbszModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into pq_jbsz(");
            strSql.Append("tjjgid,zdjd,skyl,qtyl,tjyl,xxr,tsky,ztyy,qdyl)");
            strSql.Append(" values (");
            strSql.Append("@tjjgid,@zdjd,@skyl,@qtyl,@tjyl,@xxr,@tsky,@ztyy,@qdyl)");
            strSql.Append(";select @@IDENTITY");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                int id = Convert.ToInt32(conn.ExecuteScalar(strSql.ToString(), model));
                return id;
            }
        }
    }
}
