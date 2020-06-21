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
    /// <summary>
    /// 套餐
    /// </summary>
    public class XttcbService
    {
        /// <summary>
        /// 获取企业套餐
        /// </summary>
        /// <returns></returns>
        public XttcbModel GetTc(string yybh, string tcbh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select id,tclxbh,jcyy,tcpx,tctp,tcbh,tcmc,dwbh,dwmc,jg,sfqy,createtime,dsfbzid,xb,hz,isnull(jsj,0) as jsj,updatetime,yybh,tclx,sxrs  from xt_tcb where yybh=@yybh  and tcbh=@tcbh";
                XttcbModel model = conn.Query<XttcbModel>(sql, new { yybh = yybh, tcbh = tcbh }).FirstOrDefault();
                return model;
            }
        }
        public XttcbModel GetSingleById(int id)
        {
            string sqlString = "SELECT * FROM xt_tcb A WHERE A.id=@id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32);
            using (IDbConnection dbConnection = new DapperConnection().DbConnection)
            {
                return dbConnection.Query<XttcbModel>(sqlString, parameters).FirstOrDefault();
            }
        }


        /// <summary>
        /// 医院端新渠道套餐列表
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<XttcbModel> GetYyQdTcNew(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.* from xt_tcb a  where 1=1";
                StringBuilder sd = new StringBuilder();

                DynamicParameters paramList = new DynamicParameters();
                if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
                {
                    sd.Append(" and a.yybh = @yybh");
                    paramList.Add("yybh", ht["yybh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["qdbh"].ToString()))
                {
                    sd.Append(" and a.dwbh = @qdbh");
                    paramList.Add("qdbh", ht["qdbh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["tcmc"].ToString()))
                {
                    sd.Append(" and a.tcmc like  @tcmc");
                    paramList.Add("tcmc", "%" + ht["tcmc"].ToString() + "%");
                }
                if (!string.IsNullOrEmpty(ht["sfqy"].ToString()))
                {
                    sd.Append(" and a.sfqy=  @sfqy");
                    paramList.Add("sfqy", ht["sfqy"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["tclx"].ToString()))
                {
                    sd.Append(" and a.tclx = @tclx");
                    paramList.Add("tclx", ht["tclx"].ToString());
                }
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<XttcbModel> li = conn.Query<XttcbModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }


        /// <summary>
        /// 同步企业,渠道，个人套餐表
        /// </summary>
        /// <param name="li"></param>
        public void InsertOrUpdate(List<XttcbModel> li, List<XttczhxmbModel> liZhxm)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from xt_tcb where  yybh='" + li[0].yybh + "' and dwbh='" + li[0].dwbh + "'";
                List<XttcbModel> liOld = conn.Query<XttcbModel>(sql, null).ToList();

                string sqlcz = "";
                IDbTransaction transaction = conn.BeginTransaction();

                sqlcz = "delete from xt_tc_zhxmb where yybh='" + li[0].yybh + "' and dwbh='" + li[0].dwbh + "'; delete from xt_tcb where yybh='" + li[0].yybh + "' and dwbh='" + li[0].dwbh + "'";
                conn.Execute(sqlcz, null, transaction);
                sqlcz = "insert into  xt_tcb(dsfbzid,tclx,tcbh,tcmc,dwbh,dwmc,jg,sfqy,xb,hz,yybh,createtime,updatetime) values(@dsfbzid,@tclx,@tcbh,@tcmc,@dwbh,@dwmc,@jg,@sfqy,@xb,@hz,@yybh,getdate(),getdate());";
                conn.Execute(sqlcz, li, transaction);
                string sqlinsertZhxm = "insert into xt_tc_zhxmb(zhxmbh,tcbh,yybh,dwbh)values(@zhxmbh,@tcbh,@yybh,@dwbh)";
                conn.Execute(sqlinsertZhxm, liZhxm, transaction);

                string sqlUpdatejg = "update xt_tcb set jsj=@jsj where yybh=@yybh and dwbh=@dwbh and tcbh=@tcbh";
                conn.Execute(sqlUpdatejg, liOld, transaction);
                transaction.Commit();
            }
        }
        /// <summary>
        /// 同步企业，个人套餐表 组合项目表
        /// </summary>
        /// <param name="li"></param>
        public void InsertOrUpdate(List<XttcbModel> li, List<XttczhxmbModel> liZhxm, List<XttczhxmbmxModel> liZhxmmx)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from xt_tcb where  yybh='" + li[0].yybh + "' and dwbh='" + li[0].dwbh + "'";
                List<XttcbModel> liOld = conn.Query<XttcbModel>(sql, null).ToList();

                string sqlcz = "";
                IDbTransaction transaction = conn.BeginTransaction();

                sqlcz = "delete from xt_zhxmb where yybh='" + li[0].yybh + "'; delete from xt_tc_zhxmb where yybh='" + li[0].yybh + "' and dwbh='" + li[0].dwbh + "'; delete from xt_tcb where yybh='" + li[0].yybh + "' and dwbh='" + li[0].dwbh + "'";
                conn.Execute(sqlcz, null, transaction);
                sqlcz = "insert into  xt_tcb(tclx,tcbh,tcmc,dwbh,dwmc,jg,sfqy,xb,hz,yybh,createtime,updatetime,dsfbzid) values(@tclx,@tcbh,@tcmc,@dwbh,@dwmc,@jg,@sfqy,@xb,@hz,@yybh,getdate(),getdate(),@dsfbzid);";
                conn.Execute(sqlcz, li, transaction);
                string sqlinsertZhxm = "insert into xt_tc_zhxmb(zhxmbh,tcbh,yybh,dwbh)values(@zhxmbh,@tcbh,@yybh,@dwbh)";
                conn.Execute(sqlinsertZhxm, liZhxm, transaction);
                string sqlinsertZhxmmx = "insert into xt_zhxmb(createtime,sffk,sfqy,updatetime,xb,yybh,zhxmbh,zhxmjg,zhxmmc,zhxmms)values(@createtime,@sffk,@sfqy,@updatetime,@xb,@yybh,@zhxmbh,@zhxmjg,@zhxmmc,@zhxmms)";
                conn.Execute(sqlinsertZhxmmx, liZhxmmx, transaction);

                string sqlUpdatejg = "update xt_tcb set jsj=@jsj where yybh=@yybh and dwbh=@dwbh and tcbh=@tcbh";
                conn.Execute(sqlUpdatejg, liOld, transaction);
                transaction.Commit();
            }
        }
        /// <summary>
        /// 套餐组合项目
        /// </summary>
        /// <param name="jgid"></param>
        /// <param name="tcbh"></param>
        /// <returns></returns>
        public List<XtZhxmbModel> GetTcZhxm(string yybh, string tcbh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.zhxmbh,b.zhxmmc,b.zhxmksbh,b.zhxmksmc,b.zhxmjg,b.xb,b.sffk from xt_tc_zhxmb a join xt_zhxmb b on a.yybh=b.yybh and a.zhxmbh=b.zhxmbh where  a.tcbh = @tcbh and a.yybh = @yybh ";
                List<XtZhxmbModel> li = conn.Query<XtZhxmbModel>(sql, new { yybh = yybh, tcbh = tcbh }).ToList();
                return li;
            }
        }

        /// <summary>
        /// 更新结算价
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsj"></param>
        /// <returns></returns>
        public int UpdateTcJgJsj(int id, decimal jsj, decimal jg, int sfxsjg)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "update xt_tcb set jsj=@jsj,jg=@jg,sfxsjg=@sfxsjg where id=@id";
                return conn.Execute(sql, new { jsj = jsj, id = id, jg = jg, sfxsjg = sfxsjg });
            }
        }


        /// <summary>
        /// 获取企业套餐
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="qybh"></param>
        /// <returns></returns>

        public List<XttcbModel> GetListByQybh(string yybh, string qybh)
        {
            string sql = "SELECT id,tcbh,tcmc,dwbh,dwmc,jg,sfqy,createtime,dsfbzid,xb,hz,isnull(jsj,0) as jsj,updatetime,yybh,tclx,sxrs  FROM dbo.xt_tcb WHERE yybh=@yybh AND dwbh=@dwbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<XttcbModel>(sql, new { yybh = yybh, dwbh = qybh }).ToList();
            }
        }
        /// <summary>
        /// 医院端新渠道套餐列表
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<XttcbModel> GetYyQyTcNew(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.*  from xt_tcb a   where 1=1";
                StringBuilder sd = new StringBuilder();

                DynamicParameters paramList = new DynamicParameters();
                if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
                {
                    sd.Append(" and a.yybh = @yybh");
                    paramList.Add("yybh", ht["yybh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["qdbh"].ToString()))
                {
                    sd.Append(" and a.dwbh = @qdbh");
                    paramList.Add("qdbh", ht["qdbh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["tcmc"].ToString()))
                {
                    sd.Append(" and a.tcmc like  @tcmc");
                    paramList.Add("tcmc", "%" + ht["tcmc"].ToString() + "%");
                }
                if (!string.IsNullOrEmpty(ht["sfqy"].ToString()))
                {
                    sd.Append(" and a.sfqy=  @sfqy");
                    paramList.Add("sfqy", ht["sfqy"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["tclx"].ToString()))
                {
                    sd.Append(" and a.tclx = @tclx");
                    paramList.Add("tclx", ht["tclx"].ToString());
                }
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<XttcbModel> li = conn.Query<XttcbModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }
        public DataTable GetMinMeal(string id)
        {
            string sqlString = "SELECT TCXLXID,TCXLXMC,DJS FROM TC_TCXLX WITH(NOLOCK) WHERE TCDLXID=@TCDLXID AND SFXS =1";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TCDLXID", id, DbType.String, size: 32);
            using (IDbConnection dbConnection = new DapperConnection().DbConnection)
            {
                DataTable resultTabe = new DataTable();
                resultTabe.Load(dbConnection.ExecuteReader(sqlString, parameters));
                return resultTabe;
            }
        }
        public int Update(XttcbModel model)
        {
            string sqlString = "UPDATE xt_tcb SET jsj=@jsj,tcmc=@tcmc,jcyy=@jcyy,tcpx=@tcpx,jg=@jg,xb=@xb,hz=@hz,sfqy=@sfqy,tctp=@tctp,tclxbh=@tclxbh WHERE id=@id";
            using (IDbConnection dbConnection = new DapperConnection().DbConnection)
            {
                return dbConnection.Execute(sqlString, model);
            }
        }
    }
}
