using Dapper;
using Model;
using Model.Paging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PqQyszService
    {
        /// <summary>
        /// 排期，企业基本设置
        /// </summary>
        /// <param name="qybh"></param>
        /// <returns></returns>
        public List<PqQyszModel> GetModelByQybh(string qybh,string yybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
               
                string sql = "select * from pq_qysz a where qybh=@qybh and yybh=@yybh AND jsrq>GETDATE()";
                return conn.Query<PqQyszModel>(sql, new { qybh = qybh, yybh = yybh }).ToList();
            }
        }

   

        public PqQyszModel GetModelByRq(string yybh, string qybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT top 1 * FROM dbo.pq_qysz WHERE yybh=@yybh and qybh=@qybh order by jsrq desc";
                return conn.Query<PqQyszModel>(sql, new { yybh = yybh, qybh = qybh }).FirstOrDefault();
            }
        }


        public PqQyszModel GetModelByDate(string yybh, string qybh,DateTime date)
        {
          
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT top 1 * FROM dbo.pq_qysz WHERE yybh=@yybh and qybh=@qybh AND ksrq<=@date AND @date<=jsrq";
                return conn.Query<PqQyszModel>(sql, new { yybh = yybh, qybh = qybh,date=date }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 排期，企业基本设置
        /// </summary>
        /// <param name="pqbh"></param>
        /// <returns></returns>
        public PqQyszModel GetModel(string pqbh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT * FROM dbo.pq_qysz WHERE pqbh=@pqbh";
                PqQyszModel model= conn.Query<PqQyszModel>(sql, new { pqbh = pqbh }).FirstOrDefault();

                string sql2 = "select count(1) from qy_jbxx where yybh=@yybh and bh=@qybh and sfqd=1";
                int count= conn.Query<int>(sql2, new { yybh = model.yybh,qybh=model.qybh }).FirstOrDefault();
                if(count>0)//如果在渠道基本信息中找到这个企业，说明是渠道
                {
                    model.sfqd = 1;
                }
                else
                {
                    model.sfqd = 0;
                }

                return model;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>      
        /// <param name="count"></param>
        /// <param name="yybh"></param>
        /// <param name="qybh"></param>
        /// <returns></returns>
        public List<dynamic> GetPageList(int pageIndex, int pageSize, ref int count, string yybh, string qybh,int type)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(yybh))
            {
                strWhere.AppendFormat("  a.yybh='{0}'", yybh);
            }
            if (!string.IsNullOrEmpty(qybh))
            {
                strWhere.AppendFormat(" and  a.qybh= '{0}'", qybh);
            }
            if(type==0)
            {
                strWhere.AppendFormat(" and  b.sfqd=0");
            }
            else
            {
                strWhere.AppendFormat(" and  b.sfqd=1");
            }
            PageEntity pageEntity = new PageEntity();
       
          
                pageEntity = new PageEntity()
                {

                    TableName = " dbo.pq_qysz a INNER JOIN dbo.qy_jbxx b ON a.yybh=b.yybh AND a.qybh=b.bh and b.sfqy=1 ",
                    Fields = "a.pqbh,a.yybh,a.qybh,a.ksrq,a.jsrq,a.tqts,a.jzsj,a.xxr,a.tjrs,b.mc",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    WhereStr = strWhere.ToString(),
                    OrderBy = "a.cjsj desc"
                };
          
            var list = new PagingService().GetPage<dynamic>(pageEntity, out count);
            list.ForEach(x =>
            {
                x.xxr = x.xxr.Replace("0", "星期日").Replace("1", "星期一").Replace("2", "星期二").Replace("3", "星期三").Replace("4", "星期四").Replace("5", "星期五").Replace("6", "星期六").Replace("|", "，");
            });
            return list;
        }
        /// <summary>
        /// 单个瓶颈套餐
        /// </summary>
        /// <param name="pqbh"></param>
        /// <returns></returns>
        public List<PqPjtcModel> GetPjtc(string pqbh)
        {
            string sql = "SELECT a.*,b.tjrs,b.xxr,b.sfxz FROM dbo.pq_pjtc a INNER JOIN dbo.pq_pjtcgz b ON a.fzbh=b.fzbh WHERE a.pqbh=@pqbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var list = conn.Query<PqPjtcModel>(sql, new { pqbh = pqbh }).ToList();
                return list;
            }
        }
        /// <summary>
        /// 获取所有瓶颈套餐
        /// </summary>
        /// <param name="pqbh"></param>
        /// <returns></returns>
        public List<PqPjtcModel> GetPjtc(string[] pqbh)
        {
            string sql = "SELECT a.*,b.tjrs,b.xxr,b.sfxz FROM dbo.pq_pjtc a INNER JOIN dbo.pq_pjtcgz b ON a.fzbh=b.fzbh WHERE b.pqbh IN @pqbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var list = conn.Query<PqPjtcModel>(sql, new { pqbh = pqbh }).ToList();
                return list;
            }
        }
      
        /// <summary>
        /// 检查排期日期是否存在
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public bool CheckRepeat(string yybh, string qybh, DateTime ksrq, DateTime jsrq, string pqbh = "")
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT TOP 1 * FROM dbo.pq_tjrq WHERE yybh=@yybh and qybh=@qybh AND rq BETWEEN @ksrq AND @jsrq";
                if (!string.IsNullOrEmpty(pqbh))
                {
                    sql = sql += " AND pqbh!=@pqbh";
                }
                var model = conn.QueryFirstOrDefault<PqQyRqModel>(sql, new { yybh = yybh, qybh = qybh, ksrq = ksrq, jsrq = jsrq, pqbh = pqbh });
                return !(model == null);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pjtc"></param>
        /// <param name="pjtcrs"></param>
        public void Save(PqQyszModel model, string[]pjtc,string[] pjtcrs, string[] pjtcXxr, string[] pjtcSfxz)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    if (!string.IsNullOrEmpty(model.pqbh))
                    {
                        strSql.Append("DELETE FROM dbo.pq_tjrq WHERE pqbh=@pqbh;");
                        strSql.Append("DELETE FROM dbo.pq_tjsj WHERE pqbh=@pqbh;");
                        strSql.Append("DELETE FROM dbo.pq_pjtc WHERE pqbh=@pqbh;");
                        strSql.Append("DELETE FROM dbo.pq_pjtcgz WHERE pqbh=@pqbh;");
                        strSql.Append("UPDATE dbo.pq_qysz SET ksrq=@ksrq,jsrq=@jsrq,jzsj=@jzsj,xxr=@xxr,tqts=@tqts,sjd=@sjd,tjrs=@tjrs,tjrs_dy=@tjrs_dy WHERE pqbh=@pqbh");
                    }
                    else
                    {
                        model.pqbh = GetPqbh();
                        strSql.Append("insert into pq_qysz(pqbh,yybh,qybh,ksrq,jsrq,cjsj,jzsj,xxr,tqts,sjd,tjrs,tjrs_dy) values (@pqbh,@yybh,@qybh,@ksrq,@jsrq,@cjsj,@jzsj,@xxr,@tqts,@sjd,@tjrs,@tjrs_dy);");
                    }
                    conn.Execute(strSql.ToString(), model, transaction);
                    strSql.Clear();


                    //foreach (var item in pjtcList)
                    //{
                    //    strSql.Append("insert into pq_pjtc(");
                    //    strSql.Append("pqbh,tcbh,tjrs)");
                    //    strSql.Append(" values (");
                    //    strSql.Append("@pqbh,@tcbh,@tjrs)");
                    //    item.pqbh = model.pqbh;
                    //    conn.Execute(strSql.ToString(), item, transaction);
                    //    strSql.Clear();
                    //}

                    if (pjtc != null && pjtcrs != null)
                    {

                        for (int i = 0; i < pjtc.Length; i++)
                        {
                            var fzbh = GetFzbh();
                            var tjrs = Convert.ToInt32(pjtcrs[i]);
                            var xxr = pjtcXxr[i];
                            var sfxz = Convert.ToInt32(pjtcSfxz[i]);

                            strSql.Append("insert into pq_pjtcgz(");
                            strSql.Append("pqbh,fzbh,tjrs,xxr,sfxz)");
                            strSql.Append(" values (");
                            strSql.Append("@pqbh,@fzbh,@tjrs,@xxr,@sfxz)");
                            conn.Execute(strSql.ToString(), new { pqbh = model.pqbh, fzbh = fzbh, tjrs = tjrs,xxr=xxr,sfxz=sfxz }, transaction);
                            strSql.Clear();
                            foreach (var item in pjtc[i].Split(','))
                            {
                                strSql.Append("insert into pq_pjtc(");
                                strSql.Append("pqbh,tcbh,fzbh)");
                                strSql.Append(" values (");
                                strSql.Append("@pqbh,@tcbh,@fzbh)");                               
                                conn.Execute(strSql.ToString(), new {pqbh=model.pqbh,fzbh=fzbh,tcbh=item }, transaction);
                                strSql.Clear();
                            }
                        }
                    }


                    for (int i = 0; i < model.jsrq.Subtract(model.ksrq).TotalDays + 1; i++)
                    {
                        DateTime rq = model.ksrq.AddDays(i);

                        PqTjrqModel rqModel = new PqTjrqModel();
                        rqModel.pqbh = model.pqbh;
                        rqModel.qybh = model.qybh;
                        rqModel.rq = model.ksrq.AddDays(i);
                        rqModel.tjrs = model.SjdList.Sum(x => x.tjrs);
                        rqModel.yybh = model.yybh;
                        rqModel.flag = GetFlag(model.ksrq.AddDays(i), model.xxr);

                        strSql.Append("insert into pq_tjrq(");
                        strSql.Append("pqbh,yybh,qybh,rq,tjrs,flag)");
                        strSql.Append(" values (");
                        strSql.Append("@pqbh,@yybh,@qybh,@rq,@tjrs,@flag)");
                        conn.Execute(strSql.ToString(), rqModel, transaction);
                        strSql.Clear();

                        foreach (var item in model.SjdList)
                        {
                            PqTjsjModel sjModel = new PqTjsjModel();
                            sjModel.yybh = model.yybh;
                            sjModel.pqbh = model.pqbh;
                            sjModel.qybh = model.qybh;
                            sjModel.rq = rqModel.rq;
                            sjModel.tjrs = item.tjrs;
                            sjModel.kssj = Convert.ToDateTime(rqModel.rq.ToString("yyyy-MM-dd") + " " + item.kssj);
                            sjModel.jssj = Convert.ToDateTime(rqModel.rq.ToString("yyyy-MM-dd") + " " + item.jssj);
                            strSql.Append("insert into pq_tjsj(");
                            strSql.Append("pqbh,yybh,qybh,rq,kssj,jssj,tjrs)");
                            strSql.Append(" values (");
                            strSql.Append("@pqbh,@yybh,@qybh,@rq,@kssj,@jssj,@tjrs)");
                            conn.Execute(strSql.ToString(), sjModel, transaction);
                            strSql.Clear();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 追加保存
        /// </summary>
        /// <param name="model"></param>
        public void ZjSave(PqQyszModel model)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    StringBuilder strSql = new StringBuilder();
                  
                   strSql.Append("UPDATE dbo.pq_qysz SET jsrq=@jsrq WHERE pqbh=@pqbh");
                   
                    conn.Execute(strSql.ToString(), model, transaction);
                    strSql.Clear();


                    model.SjdList= JsonConvert.DeserializeObject<List<Sjd>>(model.sjd);


                    for (int i = 0; i < model.jsrq.Subtract(model.ksrq).TotalDays + 1; i++)
                    {
                        DateTime rq = model.ksrq.AddDays(i);

                        PqTjrqModel rqModel = new PqTjrqModel();
                        rqModel.pqbh = model.pqbh;
                        rqModel.qybh = model.qybh;
                        rqModel.rq = model.ksrq.AddDays(i);
                        rqModel.tjrs = model.SjdList.Sum(x => x.tjrs);
                        rqModel.yybh = model.yybh;
                        rqModel.flag = GetFlag(model.ksrq.AddDays(i), model.xxr);

                        strSql.Append("insert into pq_tjrq(");
                        strSql.Append("pqbh,yybh,qybh,rq,tjrs,flag)");
                        strSql.Append(" values (");
                        strSql.Append("@pqbh,@yybh,@qybh,@rq,@tjrs,@flag)");
                        conn.Execute(strSql.ToString(), rqModel, transaction);
                        strSql.Clear();

                        foreach (var item in model.SjdList)
                        {
                            PqTjsjModel sjModel = new PqTjsjModel();
                            sjModel.yybh = model.yybh;
                            sjModel.pqbh = model.pqbh;
                            sjModel.qybh = model.qybh;
                            sjModel.rq = rqModel.rq;
                            sjModel.tjrs = item.tjrs;
                            sjModel.kssj = Convert.ToDateTime(rqModel.rq.ToString("yyyy-MM-dd") + " " + item.kssj);
                            sjModel.jssj = Convert.ToDateTime(rqModel.rq.ToString("yyyy-MM-dd") + " " + item.jssj);
                            strSql.Append("insert into pq_tjsj(");
                            strSql.Append("pqbh,yybh,qybh,rq,kssj,jssj,tjrs)");
                            strSql.Append(" values (");
                            strSql.Append("@pqbh,@yybh,@qybh,@rq,@kssj,@jssj,@tjrs)");
                            conn.Execute(strSql.ToString(), sjModel, transaction);
                            strSql.Clear();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
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

        public string GetPqbh()
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BM", "pq_qysz");
            dp.Add("@LSH", "", DbType.String, ParameterDirection.Output);
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute("PROC_XT_ZZB_LSBH", dp, null, null, CommandType.StoredProcedure);
                return dp.Get<string>("@LSH");
            }
        }
        /// <summary>
        /// 获取瓶颈套餐分组编号
        /// </summary>
        /// <returns></returns>
        public string GetFzbh()
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BM", "pq_pjtcgz");
            dp.Add("@LSH", "", DbType.String, ParameterDirection.Output);
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute("PROC_XT_ZZB_LSBH", dp, null, null, CommandType.StoredProcedure);
                return dp.Get<string>("@LSH");
            }
        }
    }
}
