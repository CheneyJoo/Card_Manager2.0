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
    public class DdTkjlbService
    {

        public string GetLsh()
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BM", "dd_tkjlb ");
            dp.Add("@LSH", "", DbType.String, ParameterDirection.Output);
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute("PROC_XT_ZZB_LSBH", dp, null, null, CommandType.StoredProcedure);
                return dp.Get<string>("@LSH");
            }
        }


        public DdTkjlbModel GetModel(string tklsh)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from dd_tksqjlb ");
            strSql.Append(" where tklsh=@tklsh");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.QueryFirstOrDefault(strSql.ToString(), new { tklsh = tklsh });
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(DdTkjlbModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dd_tkjlb(");
            strSql.Append("tklsh,jylsh,zffs,ddbh,tkzt,sqtkid,yybh,tjsj,refund_no,tksj)");
            strSql.Append(" values (");
            strSql.Append("@tklsh,@jylsh,@zffs,@ddbh,@tkzt,@sqtkid,@yybh,@tjsj,@refund_no,@tksj)");

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), model);
            }
        }

        /// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(DdTkjlbModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dd_tkjlb set ");
            strSql.Append("jylsh=@jylsh,");
            strSql.Append("zffs=@zffs,");
            strSql.Append("ddbh=@ddbh,");
            strSql.Append("tkzt=@tkzt,");
            strSql.Append("sqtkid=@sqtkid,");
            strSql.Append("yybh=@yybh,");
            strSql.Append("tjsj=@tjsj,");
            strSql.Append("refund_no=@refund_no,");
            strSql.Append("tksj=@tksj");
            strSql.Append(" where tklsh=@tklsh");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), model);
            }
        }
    }
}
