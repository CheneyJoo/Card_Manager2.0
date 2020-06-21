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
    public class QuDaoJbxxService
    {
        /// <summary>
        /// 渠道开启的渠道渠道基本信息
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<QuDaoJbxx> GetYyQudao(string yybh, string mc)
        {
            string sql = "select * from qy_jbxx where sfqd=1 and yybh=@yybh";

            if (!string.IsNullOrEmpty(mc))
            {
                sql += " and mc like  @mc";
            }
          
            sql += " order by id desc";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<QuDaoJbxx> li = conn.Query<QuDaoJbxx>(sql, new { yybh = yybh, mc = "%" + mc + "%" }).ToList();
                return li;
            }
        }
       
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sfqy"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int UpdateYyQudaoStatus(int sfqy, int id)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sqlcz = "update  qy_jbxx set sfqy=@sfqy where id=@id";
                return conn.Execute(sqlcz, new { sfqy = sfqy, id = id });
            }
        }
        /// <summary>
        /// 更新联系人
        /// </summary>
        /// <param name="lxr"></param>
        /// <param name="lxdh"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int UpdateYyQudao(string lxr, string lxdh, int id)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sqlcz = "update  qy_jbxx set lxdh=@lxdh,dwfzr=@lxr where id=@id";
                return conn.Execute(sqlcz, new { lxdh = lxdh, lxr = lxr, id = id });
            }
        }
        

    }
}
