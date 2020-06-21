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
    public class PqTjsjService
    {
        public List<PqTjsjModel> GetListByRq(string qybh,string yybh, DateTime rq)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
               
                string sql = "SELECT a.* FROM dbo.pq_tjsj a INNER JOIN dbo.pq_tjrq b ON a.pqbh=b.pqbh AND a.rq=b.rq WHERE a.yybh=@yybh and a.qybh=@qybh AND a.rq=@rq AND b.flag=0";
                List<PqTjsjModel> list = conn.Query<PqTjsjModel>(sql, new { yybh=yybh, qybh = qybh, rq = rq }).ToList();
                return list;
            }
        }

        public void Save(PqTjrqModel model, List<PqTjsjModel> list)
        {
            StringBuilder strSql = new StringBuilder();
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    strSql.AppendLine("IF EXISTS(SELECT 1 FROM dbo.pq_tjrq WHERE qybh=@qybh AND rq=@rq)");
                    strSql.AppendLine("BEGIN");
                    strSql.AppendLine("update pq_tjrq SET flag=0,tjrs=@tjrs WHERE qybh=@qybh AND rq=@rq");
                    strSql.AppendLine("END");
                    strSql.AppendLine("ELSE ");
                    strSql.AppendLine("BEGIN");
                    strSql.AppendLine("insert into pq_tjrq(yybh,qybh,rq,tjrs,flag)values (@yybh,@qybh,@rq,@tjrs,0)");
                    strSql.AppendLine("END");
                    conn.Execute(strSql.ToString(),model,transaction);


                    strSql.Clear();
                    strSql.Append("DELETE FROM dbo.pq_tjsj WHERE qybh=@qybh AND rq=@rq ");
                    conn.Execute(strSql.ToString(), new { qybh = model.qybh, rq = model.rq}, transaction);
                    strSql.Clear();

                    foreach (var item in list)
                    {
                        strSql.Append("insert into pq_tjsj(");
                        strSql.Append("yybh,qybh,rq,kssj,jssj,tjrs,pqbh)");
                        strSql.Append(" values (");
                        strSql.Append("@yybh,@qybh,@rq,@kssj,@jssj,@tjrs,@pqbh)");
                        conn.Execute(strSql.ToString(), item, transaction);
                        strSql.Clear();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }


 
    }
}
