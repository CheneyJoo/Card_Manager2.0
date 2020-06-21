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
    public class DdJyjlbService
    {

        /// <summary>
        ///获得一个交易记录编号
        /// </summary>
        /// <returns></returns>
        public string GetJyjlbh(string ddbh)
        {
            Random rad = new Random();
            int codeNow = rad.Next(100000, 999999);
            string guid = ddbh + codeNow.ToString();
            return guid;
        }
        public void Add(DdJyjlbModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dd_jyjlb(");
            strSql.Append("ddbh,jyjlbh,zffs,jyje,sfzfcg)");
            strSql.Append(" values (");
            strSql.Append("@ddbh,@jyjlbh,@zffs,@jyje,@sfzfcg)");

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), model);
            }
        }
        public void Update(DdJyjlbModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dd_jyjlb set ");
            strSql.Append("ddbh=@ddbh,");
            strSql.Append("jyjlbh=@jyjlbh,");
            strSql.Append("zffs=@zffs,");
            strSql.Append("sfzfcg=@sfzfcg");
            strSql.Append(" where id=@id");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), model);
            }
        }

        public DdJyjlbModel GetModel(string jyjlbh)
        {
            string sql = "SELECT TOP 1 * FROM dbo.dd_jyjlb WHERE jyjlbh=@jyjlbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
              return  conn.QueryFirstOrDefault<DdJyjlbModel>(sql, new { jyjlbh = jyjlbh });
            }
        }

        public DdJyjlbModel GetModelByDdbh(string ddbh)
        {
            string sql = "SELECT TOP 1 * FROM dbo.dd_jyjlb WHERE ddbh=@ddbh AND sfzfcg=1 ORDER BY ID DESC";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.QueryFirstOrDefault<DdJyjlbModel>(sql, new { ddbh = ddbh });
            }
        }
    }
}
