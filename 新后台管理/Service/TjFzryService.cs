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
    public class TjFzryService
    {
        public void Add(TjFzryModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tj_fzry(");
            strSql.Append("fzid,qyygid)");
            strSql.Append(" values (");
            strSql.Append("@fzid,@qyygid)");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), model);
            }
        }

        public void AddList(List<TjFzryModel> list)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tj_fzry(");
            strSql.Append("fzid,qyygid)");
            strSql.Append(" values (");
            strSql.Append("@fzid,@qyygid)");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), list);
            }
        }

        public void Del(int id)
        {
            string sql = "DELETE FROM dbo.tj_fzry WHERE id=@id";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(sql, new {id=id });
            }
        }

        public void DelList(string ids)
        {
            var list = ids.Split(',');
            string sql = "DELETE FROM dbo.tj_fzry WHERE id IN @ids";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(sql, new { ids = list });
            }
        }
        /// <summary>
        /// 通过企业账号,得到规则
        /// </summary>
        /// <param name="qyyqid">多个用逗号隔开</param>
        /// <returns></returns>
        public TjFzModel GetFzbyrq(string ygzh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select top 1 a.jstc, a.jsmds,c.ydjh from tj_fz a join tj_fzry b on a.id = b.fzid INNER JOIN dbo.qy_ygxx c ON b.qyygid=c.id WHERE c.ygzh=@ygzh AND  a.sfqy = 1 and tjjssj> getdate() order by a.id desc";
                return conn.Query<TjFzModel>(sql, new { ygzh=ygzh}).FirstOrDefault();
            }
        }
    }
}
