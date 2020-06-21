using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Service
{
    public class DdTksqjlbService
    {

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(DdTksqjlbModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dd_tksqjlb(");
            strSql.Append("ddbh,jylsbh,tkje,sqtksj,tksj,zffs,tklsh,trade_no,refund_no,tkyy,tkzt,yybh,dwbh,ddje)");
            strSql.Append(" values (");
            strSql.Append("@ddbh,@jylsbh,@tkje,@sqtksj,@tksj,@zffs,@tklsh,@trade_no,@refund_no,@tkyy,@tkzt,@yybh,@dwbh,@ddje)");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), model);
            }
        }

        /// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(DdTksqjlbModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dd_tksqjlb set ");
            strSql.Append("ddbh=@ddbh,");
            strSql.Append("jylsbh=@jylsbh,");
            strSql.Append("tkje=@tkje,");
            strSql.Append("sqtksj=@sqtksj,");
            strSql.Append("tksj=@tksj,");
            strSql.Append("zffs=@zffs,");
            strSql.Append("tklsh=@tklsh,");
            strSql.Append("trade_no=@trade_no,");
            strSql.Append("refund_no=@refund_no,");
            strSql.Append("tkyy=@tkyy,");
            strSql.Append("tkzt=@tkzt,");
            strSql.Append("yybh=@yybh,");
            strSql.Append("dwbh=@dwbh,");
            strSql.Append("ddje=@ddje");
            strSql.Append(" where id=@id");

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), model);
            }
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DdTksqjlbModel GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from dd_tksqjlb ");
            strSql.Append(" where id=@id");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
               return conn.QueryFirstOrDefault<DdTksqjlbModel>(strSql.ToString(), new { id=id});
            }
        }

    }
}
