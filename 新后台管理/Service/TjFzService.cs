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
    public class TjFzService
    {

        public TjFzModel GetModel(int id)
        {
            string sql = "SELECT * FROM dbo.tj_fz WHERE id=@id";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<TjFzModel>(sql, new { id = id }).FirstOrDefault();
            }
        }

        public void Add(TjFzModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tj_fz(");
            strSql.Append("mc,yybh,sfqy,jsmds,tjkssj,tjjssj,cjsj,jstc,dwbh)");
            strSql.Append(" values (");
            strSql.Append("@mc,@yybh,@sfqy,@jsmds,@tjkssj,@tjjssj,@cjsj,@jstc,@dwbh)");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(),model);
            }
        }

        public void Update(TjFzModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tj_fz set ");
            strSql.Append("mc=@mc,");
            strSql.Append("yybh=@yybh,");
            strSql.Append("sfqy=@sfqy,");
            strSql.Append("jsmds=@jsmds,");
            strSql.Append("jstc=@jstc,");
            strSql.Append("dwbh=@dwbh,");
            strSql.Append("tjkssj=@tjkssj,");
            strSql.Append("tjjssj=@tjjssj");
            strSql.Append(" where id=@id");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), model);
            }
        }

        public void Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM dbo.tj_fz WHERE id=@id;");
            strSql.Append("DELETE FROM dbo.tj_fzry WHERE fzid=@id;");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), new { id=id});
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ygid">企业员工id</param>
        /// <returns></returns>
        public TjFzModel GetModelByYgid(int ygid)
        {
            string sql = "SELECT a.* FROM dbo.tj_fz a INNER JOIN dbo.tj_fzry b ON a.id=b.fzid WHERE b.qyygid=@qyygid";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<TjFzModel>(sql, new { qyygid = ygid }).FirstOrDefault();
            }
        }

    }
}
