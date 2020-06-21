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
    /// <summary>
    /// 渠道退款服务
    /// </summary>
    public class QuDaoTkService
    {
        /// <summary>
        /// 渠道端退款查询
        /// </summary>
        /// <param name="dsfdd"></param>
        /// <param name="qdId"></param>
        /// <param name="xm"></param>
        /// <param name="dh"></param>
        /// <returns></returns>
        public List<DdjbxxModel> OrderQdList(string dsfdd, string qdId, string xm, string dh)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (!string.IsNullOrEmpty(dsfdd))
            {
                sd.Append(" and a.dsfdd = @dsfdd");
                paramList.Add("dsfdd", dsfdd);
            }


            sd.Append(" and a.dsfbzid = @dsfbz");
            paramList.Add("dsfbz", qdId);


            if (!string.IsNullOrEmpty(xm))
            {
                sd.Append(" and a.xm = @xm");
                paramList.Add("xm", xm);
            }

            if (!string.IsNullOrEmpty(dh))
            {
                sd.Append(" and a.dh= @dh ");
                paramList.Add("dh", dh);

            }

            sd.Append(" and a.ddly=0 and a.ddzt in (6,7,9) ");

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.*,b.dsfbz as qdmc,c.jgmc from dd_jbxx a join xt_dsfbz b on a.dsfbzid=b.id join xt_jgb c on a.yybh=c.yybh where 1=1";
                sql += sd.ToString();
             
                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sql, paramList).ToList();

                return li;
            }
        }
        /// <summary>
        /// 退款申请
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int QdTkSq(QuDaoTkModel model, ref string msg)
        {
            bool res = ExistsTk(model.ddbh);
            if (res)
            {
                msg = "已经提交过退款，不能重复提交";
                return 0;

            }
            string sql = "insert into qd_tksqjlb(ddbh,tkje,sqtksj,tkyy,tkzt,qdid,sqrid) values(@ddbh,@tkje,getdate(),@tkyy,1,@qdid,@sqrid)";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
               int i= conn.Execute(sql, model);
                return i;
            }

        }
        /// <summary>
        /// 重新申请退款
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateTkSq(QuDaoTkModel model)
        {
            string sql = "update qd_tksqjlb set tkje=@tkje,tkyy=@tkyy,sqtksj=getdate(),tkzt=1,sqrid=@sqrid where ddbh=@ddbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                int i = conn.Execute(sql, model);
                return i;
            }
        }
        /// <summary>
        /// 医院审核退款
        /// </summary>
        /// <param name="ddbh"></param>
        /// <param name="shrId">审核人id</param>
        /// <returns></returns>
        public int UpdateTkTy(string ddbh,decimal tkje, int shrId)
        {
            string sql = "update qd_tksqjlb set tkzt=2,shrid=@shrid,shsj=getdate(),shyy='' where ddbh=@ddbh;update dd_jbxx set tkze=@tkje where ddbh=@ddbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                int i = conn.Execute(sql, new { ddbh=ddbh, shrid =shrId,tkje=tkje});
                return i;
            }
        }
        /// <summary>
        /// 医院审核拒绝
        /// </summary>
        /// <param name="ddbh"></param>
        /// <param name="tkje"></param>
        /// <param name="shrId"></param>
        /// <returns></returns>
        public int UpdateTkQj(string ddbh, string yy, int shrId)
        {
            string sql = "update qd_tksqjlb set tkzt=3,shrid=@shrid,shsj=getdate(),shyy=@shyy where ddbh=@ddbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                int i = conn.Execute(sql, new { ddbh = ddbh, shrid = shrId,shyy=yy });
                return i;
            }
        }
        /// <summary>
        /// 是否申请过退款
        /// </summary>
        /// <param name="ddbh"></param>
        /// <returns></returns>
        private bool ExistsTk(string ddbh)
        {
            string sql = "select count(1) from qd_tksqjlb where ddbh=@ddbh and tkzt>0";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
               int i=  conn.Query<int>(sql, new { ddbh = ddbh }).FirstOrDefault();
                if(i>0)
                { return true; }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 渠道端退款中,退款完成订单
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DdjbxxModel> OrderQdTkList(string dh, string xm, string dsfdd, string qdId,int tkzt, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (!string.IsNullOrEmpty(dsfdd))
            {
                sd.Append(" and a.dsfdd =@dsfdd");
                paramList.Add("dsfdd", dsfdd);
            }

            sd.Append(" and a.dsfbzid = " + qdId.ToString());

            sd.Append(" and a.ddly = 0");

            if (!string.IsNullOrEmpty(dh))
            {
                sd.Append(" and a.dh = @dh");
                paramList.Add("dh", dh);
            }

            if (!string.IsNullOrEmpty(xm))
            {
                sd.Append("  and a.xm = @xm");
                paramList.Add("xm", xm);
            }
            if(tkzt==2)
            {
                sd.Append(" and c.tkzt=2");
            }
            else
            {
                sd.Append(" and c.tkzt in (1,3)");
            }
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.*,b.jgmc,c.tkje,c.tkzt,c.id as tkid from dd_jbxx a join xt_jgb b on a.yybh=b.yybh join qd_tksqjlb c on a.ddbh=c.ddbh where 1=1";
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By tkid asc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }
        /// <summary>
        /// 退款单详情
        /// </summary>
        /// <param name="ddbh"></param>
        /// <returns></returns>
        public DdjbxxModel OrderQdTzDetail(string ddbh)
        {
            StringBuilder sd = new StringBuilder();          

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.*,b.jgmc,d.dsfbz as qdmc,c.tkje,c.tkzt,c.tkyy,c.shyy as tkjjyy  from dd_jbxx a join xt_jgb b on a.yybh=b.yybh join qd_tksqjlb c on a.ddbh=c.ddbh join xt_dsfbz d on a.dsfbzid=d.id   where a.ddbh=@ddbh";

                DdjbxxModel li = conn.Query<DdjbxxModel>(sql, new { ddbh = ddbh }).FirstOrDefault();
                if (li !=null)
                {
                    li.zhxmlist = new DdyymxbService().GetOrderZhxm(ddbh);
                }
                return li;
            }
        }

        /// <summary>
        /// 医院端退款列表
        /// </summary>
        /// <param name="dh"></param>
        /// <param name="xm"></param>
        /// <param name="qdId"></param>
        /// <param name="yybh"></param>
        /// <param name="tkzt"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>

        public List<DdjbxxModel> OrderQdYyTkList(string dh, string xm,  string qdId,string yybh, int tkzt, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();

            sd.Append(" and a.yybh ='" + yybh + "'");


            if (!string.IsNullOrEmpty(qdId))
            {
                sd.Append(" and a.dsfbzid = " + qdId.ToString());
            }

            sd.Append(" and a.ddly = 0");

            if (!string.IsNullOrEmpty(dh))
            {
                sd.Append(" and a.dh = @dh");
                paramList.Add("dh", dh);
            }

            if (!string.IsNullOrEmpty(xm))
            {
                sd.Append("  and a.xm = @xm");
                paramList.Add("xm", xm);
            }
            if (tkzt == 2)
            {
                sd.Append(" and c.tkzt=2");
            }
            else
            {
                sd.Append(" and c.tkzt=1");
            }
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.*,b.dsfbz as qdmc,c.tkje,c.tkzt,c.id as tkid from dd_jbxx a join xt_dsfbz b on a.dsfbzid=b.id join qd_tksqjlb c on a.ddbh=c.ddbh where 1=1";
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By tkid asc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        /// 结算订单详情
        /// </summary>
        /// <param name="ddbh"></param>
        /// <returns></returns>
        public DdjbxxModel JsddDetail(string ddbh)
        {
            StringBuilder sd = new StringBuilder();

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.*,b.jgmc,d.dsfbz as qdmc,c.tkje,c.tkzt,c.tkyy,e.jstz,e.tzyy,e.sjjsj,f.jjyy from dd_jbxx a left join xt_jgb b on a.yybh=b.yybh left join qd_tksqjlb c on a.ddbh=c.ddbh " +
                             "left join xt_dsfbz d on a.dsfbzid=d.id left join qd_jsjbxxmx e on a.ddbh=e.ddbh left join qd_jsjbxx f on e.jbxxid=f.id where a.ddbh=@ddbh";

                DdjbxxModel li = conn.Query<DdjbxxModel>(sql, new { ddbh = ddbh }).FirstOrDefault();
                if (li != null)
                {
                    li.zhxmlist = new DdyymxbService().GetOrderZhxm(ddbh);
                }
                return li;
            }
        }
    }
}
