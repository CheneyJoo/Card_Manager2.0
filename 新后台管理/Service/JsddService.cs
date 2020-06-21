using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Common;
using Dapper;
using Model;

namespace Service
{
    /// <summary>
    /// 结算订单服务
    /// </summary>
    public class JsddService
    {
        /// <summary>
        /// 医院端获取结算订单列表
        /// </summary>
        /// <returns></returns>
        public List<DdjbxxModel> GetList(string yybh,int qdid, string fromdate, string todate, string xm, string dh, int sftk)
        {
            StringBuilder sbSql = new StringBuilder();
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();
                sbSql.Append("SELECT ddbh, a.dsfbzid,b.dsfbz as qdmc, dsfdd, tcmc, xm, dh, ysjsj, tkze, sfjs FROM dd_jbxx a join xt_dsfbz b on a.dsfbzid=b.id WHERE  ddzt in (6,7,9) and yybh=@yybh and  sfjs=0 AND dsfbzid=@Qdid AND ddbh NOT IN" +
                             " (SELECT ddbh FROM qd_jsjbxxmx mx INNER JOIN qd_jsjbxx jb ON mx.jbxxid=jb.id WHERE jb.yybh=@yybh and  jb.qdid=@Qdid)");
                if (!string.IsNullOrEmpty(fromdate))
                {
                    sbSql.Append(" and convert(varchar(10), djtime,120)>=@Fromdate");
                }
                if (!string.IsNullOrEmpty(todate))
                {
                    sbSql.Append(" and convert(varchar(10), djtime,120)<=@Todate");
                }
                if (!string.IsNullOrEmpty(xm))
                {
                    sbSql.Append(" and xm = @Xm");
                }
                if (!string.IsNullOrEmpty(dh))
                {
                    sbSql.Append(" and dh = @Dh");
                }
                if (sftk.Equals(0))
                {
                    sbSql.Append(" and ISNULL(tkze,0) = 0");
                }
                else if(sftk.Equals(1))
                {
                    sbSql.Append(" and tkze > 0");
                }
               
                paramList.Add("Qdid", qdid);
                paramList.Add("Fromdate", fromdate);
                paramList.Add("Todate", todate);
                paramList.Add("Xm", xm);
                paramList.Add("Dh", dh);
                paramList.Add("yybh", yybh);
                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sbSql.ToString(), paramList).ToList();
                return li;
            }
        }

        /// <summary>
        /// 渠道端获取结算订单列表
        /// </summary>
        /// <returns></returns>
        public List<DdjbxxModel> GetListQd(int qdid,string yymc, string fromdate, string todate, int sfjs, string xm, string dh, int sftk,int pageIndex,int pageSize, ref int count, ref decimal tkze, ref decimal ysjsze)
        {
            StringBuilder sbSql = new StringBuilder();
            if (!string.IsNullOrEmpty(yymc))
            {
                sbSql.Append(" and c.jgmc LIKE @Yymc");
            }
            if (!string.IsNullOrEmpty(fromdate))
            {
                sbSql.Append(" and convert(varchar(10), djtime,120)>=@Fromdate");
            }
            if (!string.IsNullOrEmpty(todate))
            {
                sbSql.Append(" and convert(varchar(10), djtime,120)<=@Todate");
            }
            if (sfjs > -1)
            {
                sbSql.Append(" and a.sfjs=@Jszt");
            }
            if (!string.IsNullOrEmpty(xm))
            {
                sbSql.Append(" and a.xm = @Xm");
            }
            if (!string.IsNullOrEmpty(dh))
            {
                sbSql.Append(" and a.dh = @Dh");
            }
            if (sftk.Equals(0))
            {
                sbSql.Append(" and ISNULL(a.tkze,0) = 0");
            }
            else if (sftk.Equals(1))
            {
                sbSql.Append(" and a.tkze > 0");
            }

            string strWhere = sbSql.ToString();
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();
                sbSql.Clear();
                sbSql.Append("SELECT ddbh, c.jgmc, dsfdd, tcmc, xm, dh, ysjsj, tkze, sfjs FROM dd_jbxx a  JOIN xt_dsfbz b ON a.dsfbzid=b.id LEFT JOIN xt_jgb c ON a.yybh=c.yybh WHERE a.ddly=0 and a.ddzt in(2,3,6,7,9) and a.dsfbzid=@Qdid");
                sbSql.Append(strWhere);

                string sqlPage = "";
                int fromRow = (pageIndex - 1) * pageSize;
                int toRow = (pageIndex) * pageSize;
                paramList.Add("Yymc", "%" + yymc + "%");
                paramList.Add("Fromdate", fromdate);
                paramList.Add("Todate", todate);
                paramList.Add("Jszt", sfjs);
                paramList.Add("Xm", xm);
                paramList.Add("Dh", dh);
                paramList.Add("FromRow", fromRow);
                paramList.Add("ToRow", toRow);
                paramList.Add("Qdid", qdid);
                sqlPage = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ddbh DESC) AS rowNum, * FROM (" + sbSql + ") AS T ) AS N WHERE rowNum >@FromRow AND rowNum <=@ToRow ";
                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sqlPage, paramList).ToList();
                //count = conn.Query<int>("SELECT COUNT(1) FROM (" + sbSql + ") AS t ", paramList).FirstOrDefault();

                sbSql.Clear();
                sbSql.Append($"SELECT isnull(sum(ysjsj),0) as ysjsze, isnull(sum(tkze),0) as tkze, count(*) as total FROM dd_jbxx a  JOIN xt_dsfbz b ON a.dsfbzid=b.id LEFT JOIN xt_jgb c ON a.yybh=c.yybh WHERE a.ddly=0 and a.ddzt in(2,3,6,7,9) and a.dsfbzid={qdid}");
                sbSql.Append(strWhere);
                var reader = conn.ExecuteReader(sbSql.ToString(),paramList);
                DataTable dt = new DataTable();
                dt.Load(reader);
                if (dt.Rows.Count>0)
                {
                    ysjsze = Convert.ToDecimal(dt.Rows[0]["ysjsze"]);
                    tkze = Convert.ToDecimal(dt.Rows[0]["tkze"]);
                    count  = Convert.ToInt32(dt.Rows[0]["total"]);
                }
                dt.Load(reader);

                return li;
            }
        }

       
        public ReturnModel PostToCalculate(List<string> listDdbh, string yybh, int qdid, int tjr)
        {
            try
            {
                //验证参数
                if (listDdbh.Count.Equals(0))
                {
                    return new ReturnModel { Code = 201, Msg = "请选择订单" };
                }

                StringBuilder sbSql = new StringBuilder();
                //生成流水号
                string jsbh = new XT_ZZB().GetJsLsh(qdid);
               
                //保存
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        //获取订单基本信息
                        sbSql.Append("SELECT * FROM dd_jbxx WHERE ddbh IN @ids");
                        var listDd = conn.Query<DdjbxxModel>(sbSql.ToString(), new { ids = listDdbh },transaction).ToList();

                        int result = 0;
                        decimal ysjsze = listDd.Sum(x => x.ysjsj);
                        decimal tkze = listDd.Sum(x => x.tkze);
                        //插入结算基本信息表获取自增Id
                        sbSql.Clear();
                        sbSql.Append("INSERT INTO qd_jsjbxx (jsbh, yybh, qdid, zt, tjsj, tjr, ysjsze, sjjsze, tkze) VALUES (@Jsbh, @Yybh, @Qdid, @Zt, @Tjsj, @Tjr, @Ysjsze, @Sjjsze, @Tkze);select @@IDENTITY");
                        int jsjbxxid = Convert.ToInt32(conn.ExecuteScalar(sbSql.ToString(), new { Jsbh = jsbh, Yybh = yybh, Qdid = qdid, Zt = 1, Tjsj = DateTime.Now, Tjr = tjr, Ysjsze = ysjsze, Sjjsze = ysjsze - tkze, Tkze = tkze }, transaction));

                        foreach (var model in listDd)
                        {
                            //插入结算明细
                            sbSql.Clear();
                            sbSql.Append("INSERT INTO qd_jsjbxxmx (jbxxid, ddbh, ysjsj, tkze, jstz, sjjsj) VALUES (@Jbxxid, @Ddbh, @Ysjsj, @Tkze, @Jstz, @Sjjsj)");
                            conn.Execute(sbSql.ToString(), new { Jbxxid = jsjbxxid, Ddbh = model.ddbh, Ysjsj = model.ysjsj, Tkze = model.tkze, Jstz = 0, Sjjsj = model.ysjsj - model.tkze }, transaction);
                        }

                        transaction.Commit();
                        return new ReturnModel { Code = 200, Msg = "提交成功" };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.WriteLog(ex.Message);
                        return new ReturnModel { Code = 201, Msg = "提交失败" };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return new ReturnModel { Code = 201, Msg = "提交失败" };
            }
            
        }
    }
}
