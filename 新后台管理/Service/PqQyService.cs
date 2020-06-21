using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Model.Dto;
using Model.Paging;

namespace Service
{
    public class PqQyService
    {
        public string GetPqbh()
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BM", "pq_qy");
            dp.Add("@LSH", "", DbType.String, ParameterDirection.Output);
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute("PROC_XT_ZZB_LSBH", dp, null, null, CommandType.StoredProcedure);
                return dp.Get<string>("@LSH");
            }
        }

        public PqQytDto GetModel(string  pqbh)
        {
            string sql = "SELECT * FROM dbo.pq_qy WHERE pqbh=@pqbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                PqQytDto model = conn.QueryFirstOrDefault<PqQytDto>(sql, new { pqbh = pqbh });
                sql = "SELECT * FROM dbo.pq_qy_pjtc WHERE pqbh=@pqbh";
                model.pjtcList = conn.Query<PqQyPjtcModel>(sql, new { pqbh = pqbh }).ToList();
                return model;
            }
        }

        public void Save(PqQytDto model)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    if (!string.IsNullOrEmpty(model.pqbh))
                    {
                        strSql.Append("DELETE FROM dbo.pq_qy_rq WHERE pqbh=@pqbh;");
                        strSql.Append("DELETE FROM dbo.pq_qy_pjtc WHERE pqbh=@pqbh;");
                        strSql.Append("UPDATE dbo.pq_qy SET ksrq=@ksrq,jsrq=@jsrq,jzsj=@jzsj,xxr=@xxr,tqts=@tqts,tjrs=@tjrs WHERE pqbh=@pqbh");
                    }
                    else {
                        model.pqbh = GetPqbh();
                        strSql.Append("insert into pq_qy(pqbh,yybh,qdid,ksrq,jsrq,cjsj,jzsj,xxr,tqts,tjrs) values (@pqbh,@yybh,@qdid,@ksrq,@jsrq,@cjsj,@jzsj,@xxr,@tqts,@tjrs);");
                    }
                    conn.Execute(strSql.ToString(), model, transaction);
                    strSql.Clear();


                    foreach (var item in model.pjtcList)
                    {                      
                        strSql.Append("insert into pq_qy_pjtc(");
                        strSql.Append("pqbh,tcbh,tjrs)");
                        strSql.Append(" values (");
                        strSql.Append("@pqbh,@tcbh,@tjrs)");
                        item.pqbh = model.pqbh;
                        conn.Execute(strSql.ToString(), item, transaction);
                        strSql.Clear();
                    }


                    for (int i = 0; i < model.jsrq.Subtract(model.ksrq).TotalDays + 1; i++)
                    {
                        DateTime rq = model.ksrq.AddDays(i);

                        PqQyRqModel rqModel = new PqQyRqModel();
                        rqModel.pqbh = model.pqbh;
                        rqModel.qdid = model.qdid;
                        rqModel.rq = model.ksrq.AddDays(i);
                        rqModel.tjrs = model.tjrs;
                        rqModel.yybh = model.yybh;
                        rqModel.flag = GetFlag(model.ksrq.AddDays(i), model.xxr);

                        strSql.Append("insert into pq_qy_rq(");
                        strSql.Append("pqbh,yybh,qdid,rq,tjrs,flag)");
                        strSql.Append(" values (");
                        strSql.Append("@pqbh,@yybh,@qdid,@rq,@tjrs,@flag)");
                        conn.Execute(strSql.ToString(), rqModel, transaction);
                        strSql.Clear();

                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// 获取flag
        /// </summary>
        /// <param name="date"></param>
        /// <param name="xxr"></param>
        /// <returns>0:默认1休息日</returns>
        public int GetFlag(DateTime date, string xxr)
        {
            bool val = xxr.Contains(Convert.ToInt32(date.DayOfWeek).ToString());
            return val ? 1 : 0;
        }
        /// <summary>
        /// 排
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="qdid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<PqQyModel> GetQuDaoPqList(string yybh,int qdid, int pageIndex, int pageSize, ref int count)
        {
            
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(yybh))
            {
                strWhere.AppendFormat(" a.yybh='{0}'", yybh);
            }
            if (qdid > 0)
            {
                strWhere.AppendFormat(" AND a.qdid = {0}", qdid);
            }
            PageEntity pageEntity = new PageEntity()
            {
                TableName = " dbo.pq_qy a INNER JOIN dbo.xt_qdjbxx b ON a.qdid=b.qdid INNER JOIN dbo.xt_dsfbz c ON b.qdid=c.id and a.yybh=b.yybh ",
                Fields = "a.pqbh,a.qdid,a.ksrq,a.jsrq,a.jzsj,a.tqts,a.tjrs,a.xxr,c.dsfbz as qdmc",
                PageIndex = pageIndex,
                PageSize = pageSize,
                WhereStr = strWhere.ToString(),
                OrderBy = "a.cjsj desc"
            };
            var list = new PagingService().GetPage<PqQyModel>(pageEntity, out count);
            list.ForEach(x =>
            {
                x.xxr = x.xxr.Replace("0", "星期日").Replace("1", "星期一").Replace("2", "星期二").Replace("3", "星期三").Replace("4", "星期四").Replace("5", "星期五").Replace("6", "星期六").Replace("|", "，");
            });
            return list;
        }

        public List<PqQytDto> GetList(List<string>pqbh)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM dbo.pq_qy WHERE pqbh IN @pqbh");

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var list = conn.Query<PqQytDto>(strSql.ToString(),new { pqbh=pqbh}).ToList();
                foreach (var item in list)
                {
                    strSql.Clear();
                    strSql.Append("SELECT * FROM dbo.pq_qy_pjtc WHERE pqbh=@pqbh");
                    item.pjtcList = conn.Query<PqQyPjtcModel>(strSql.ToString(), new { pqbh = pqbh }).ToList();
                }
                return list;
            }
        }
    }
}
