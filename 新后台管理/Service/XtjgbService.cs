using Dapper;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class XtjgbService
    {
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <returns></returns>
        public XtJgbModel GetJg(string yybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = @"
                        select a.*,b.jgjkurl,b.account,b.pwd,b.iskkservice, d.SFBH
                        from xt_jgb a 
                        join xt_jgjk b on a.yybh=b.yybh 
                        inner join XT_CSLB c on c.CSBH=a.csbh
                        inner join XT_SFB d on d.SFBH=c.SFID where a.yybh=@yybh";
                XtJgbModel model = conn.Query<XtJgbModel>(sql, new { yybh = yybh }).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 机构列表
        /// </summary>
        /// <returns></returns>
        public List<XtJgbModel> JgList(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (ht["yybh"] != null)
            {
                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", ht["yybh"].ToString());
            }

            if (ht["sfqy"] != null)
            {
                sd.Append(" and a.sfqy = @sfqy");
                paramList.Add("sfqy", ht["sfqy"].ToString());
            }

            if (ht["jgmc"] != null)
            {
                sd.Append(" and a.jgmc like @jgmc");
                paramList.Add("jgmc", ht["jgmc"].ToString()+"%");
            }



            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.* from xt_jgb a where 1=1";
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<XtJgbModel> li = conn.Query<XtJgbModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }
        /// <summary>
        /// 更新机构
        /// </summary>
        /// <param name="model"></param>
        public int UpdateJg(XtJgbModel model)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "update xt_jgb set jgmc=@jgmc,jgdz=@jgdz,csbh=@csbh,dj=@dj,bw=@bw,lxr=@lxr,lxdh=@lxdh,zyfs=@zyfs,jgxz=@jgxz,yydj=@yydj,sfqy=@sfqy,yyimage=@yyimage,yylogoimage=@yylogoimage,yyjs=@yyjs where id=@id";
                return conn.Execute(sql, model);
            }
        }
    }
}
