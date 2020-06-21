using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Model;
using Model.Dto.ReportQd;
using Common;

namespace Service
{
    public class DdJbxxService
    {

        public string GetDdbh()
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BM", "dd_jbxx");
            dp.Add("@LSH", "", DbType.String, ParameterDirection.Output);
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute("PROC_XT_ZZB_LSBH", dp, null, null, CommandType.StoredProcedure);
                string ddbh = dp.Get<string>("@LSH");
                return ddbh;
            }
        }
   
        public void ConfirmOrder(DdjbxxModel ddModel, List<DdZhxmModel> zhxmList, QyygxxModel ygModel)
        {
            StringBuilder strSql = new StringBuilder();
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    if (ddModel.jsbz == 0)//自已才更新
                    {
                        strSql.AppendFormat("UPDATE dbo.qy_ygxx SET sfyy=@sfyy WHERE id=@id");
                        conn.Execute(strSql.ToString(), new { sfyy = ygModel.sfyy, id = ygModel.id }, transaction);
                    }

                    strSql.Clear();
                    if(string.IsNullOrEmpty(ddModel.csrq))
                    {
                        ddModel.csrq = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    strSql.Append("insert into dd_jbxx_new(");
                    strSql.Append("trade_no,zffs,ddbh,dsfdd,dsfbzid,ddzt,tcid,tcmc,dwbh,tcjg,jxbjg,ddze,intime,sfout,outtime,dh,xm,xb,hz,zjlx,zjhm,yykssj,yyjssj,sfdj,djtime,sfbg,bgtime,djlsh,sfjx,jxlist,sfjs,csrq,nl,remark,yybh,ddly,dwmc,jsbz,ygzh)");
                    strSql.Append(" values (");
                    strSql.Append("'',@zffs,@ddbh,@dsfdd,@dsfbzid,@ddzt,@tcid,@tcmc,@dwbh,@tcjg,@jxbjg,@ddze,@intime,@sfout,@outtime,@dh,@xm,@xb,@hz,@zjlx,@zjhm,@yykssj,@yyjssj,@sfdj,@djtime,@sfbg,@bgtime,@djlsh,@sfjx,@jxlist,@sfjs,@csrq,@nl,@remark,@yybh,@ddly,@dwmc,@jsbz,@ygzh)");

                    conn.Execute(strSql.ToString(), ddModel, transaction);

                    strSql.Clear();

                    strSql.Append("insert into dd_zhxm(");
                    strSql.Append("ddbh,zhxmbh,zhxmmc,jg,sfjx,sfdj)");
                    strSql.Append(" values (");
                    strSql.Append("@ddbh,@zhxmbh,@zhxmmc,@jg,@sfjx,@sfdj)");

                    conn.Execute(strSql.ToString(), zhxmList, transaction);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    LogApiHelper.AddLog(ex.Message);
                    transaction.Rollback();
                    throw;
                }
            }
        }
             

        /// <summary>
        /// 获取指定时间当天每个渠道的预约人数
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<dynamic> GetYyrsByDay(string yybh, DateTime day)
        {
            DateTime startDate = day;
            DateTime endDate = day.AddDays(1);

            string sql = "SELECT COUNT(1) AS yyrs,a.dwbh as qybh FROM dbo.dd_jbxx_new a  WHERE a.yybh=@yybh AND a.ddly=0 AND a.ddzt IN (2,3,6,7,9) AND a.yykssj>=@startDate AND a.yykssj<@endDate  GROUP BY a.dwbh ";

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<dynamic>(sql, new {yybh=yybh, startDate = startDate, endDate = endDate }).ToList();
            }

        }
        /// <summary>
        /// 获取医院所有渠道指定月份每天预约人数
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<dynamic> GetQdDayYyrsByMonth(string yybh, int year,int month)
        {
            DateTime startDate = new DateTime(year,month,1);
            DateTime endDate = startDate.AddMonths(1);

            string sql = "SELECT COUNT(1) AS yyrs, DAY(yykssj) AS [day] FROM dbo.dd_jbxx_new WHERE yybh=@yybh AND ddly=0 AND yykssj>@startDate AND yykssj<@endDate AND ddzt IN (2,3,6,7,9) GROUP BY DAY(yykssj)";

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<dynamic>(sql, new { yybh = yybh, startDate = startDate, endDate = endDate }).ToList();
            }

        }

   
        /// <summary>
        /// 获取过去一年每个月各个渠道的销售量
        /// </summary>
        /// <returns></returns>
        public List<SalesTrends> GetSalesTrends(string yybh)
        {
            var date = DateTime.Now.AddMonths(-11).ToString("yyyy-MM-dd");
        
            string sql = "SELECT SUM((tcjg+jxbjg)/10000) AS [money],a.dwbh as qdid,a.dwmc AS qdmc,MONTH(a.yykssj) AS [month] FROM dbo.dd_jbxx_new a  WHERE a.yybh=@yybh AND a.yykssj>=@date AND a.ddly=0 GROUP BY a.dwbh,a.dwmc,MONTH(a.yykssj)";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<SalesTrends>(sql, new { yybh = yybh, date = date }).ToList();
            }
        }

        /// <summary>
        /// 获取当天各个渠道的销售量，饼图
        /// </summary>
        /// <returns></returns>
        public List<SalesTrends> GetSalesScale(string yybh)
        {
            string sql = "SELECT SUM((tcjg+jxbjg)/10000) AS [money], dwbh as qdid FROM dbo.dd_jbxx_new a   WHERE a.yybh=@yybh and a.yykssj>='"+DateTime.Now.ToString("yyyy-MM-dd")+ "' and a.yykssj<'"+DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")+ "' AND a.ddly=0 GROUP BY a.dwbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<SalesTrends>(sql, new { yybh = yybh }).ToList();
            }
        }

        /// <summary>
        /// 团检取消预约
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="ydjh"></param>
        /// <returns></returns>
        public int DeleteTjOrder(string yybh, string ydjh)
        {
            string sql = "update qy_ygxx set sfyy=0 where yybh=@yybh and ydjh=@ydjh; delete from  dd_zhxm where ddbh in (select ddbh from dd_jbxx_new where  yybh =@yybh and dsfdd=@ydjh and ddly=1); delete from dd_jbxx_new where yybh =@yybh and dsfdd=@ydjh and ddly=1;";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Execute(sql, new { yybh = yybh, ydjh = ydjh });
            }
        }

        public List<dynamic> GetTjYyrsByDay(string yybh, DateTime day)
        {
            DateTime startDate = day;
            DateTime endDate = day.AddDays(1);

            string sql = "SELECT COUNT(1) AS yyrs,left(dwbh,4) as dwbh FROM dbo.dd_jbxx_new a   WHERE a.yybh=@yybh AND a.ddly=1 AND a.ddzt IN (2,3,6,7,9) AND a.yykssj>=@startDate AND a.yykssj<@endDate  GROUP BY left(a.dwbh,4) ";

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<dynamic>(sql, new { yybh = yybh, startDate = startDate, endDate = endDate }).ToList();
            }

        }

        /// <summary>
        /// 获取医院所有渠道指定月份每天预约人数
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<dynamic> GetQyDayYyrsByMonth(string yybh, int year, int month)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1);

            string sql = "SELECT COUNT(1) AS yyrs, DAY(yykssj) AS [day] FROM dbo.dd_jbxx_new WHERE yybh=@yybh AND ddly=1 AND yykssj>@startDate AND yykssj<@endDate AND ddzt IN (2,3,6,7,9) GROUP BY DAY(yykssj)";

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<dynamic>(sql, new { yybh = yybh, startDate = startDate, endDate = endDate }).ToList();
            }

        }

        /// <summary>
        /// 获取医院指定企业之指定日期预约数据 
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<DdjbxxModel> GetQyYyListByDate(string yybh, string dwbh,DateTime date)
        {

            DateTime kssj = Convert.ToDateTime(date);
            DateTime jssj = kssj.AddDays(1);
            string sql = "SELECT yykssj,yyjssj,tcid FROM dbo.dd_jbxx_new WHERE yybh=@yybh AND dwbh=@dwbh AND yykssj>@kssj AND yykssj<@jssj AND ddzt IN (2,3,6,7,9) AND ddly=1";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<DdjbxxModel>(sql, new { yybh = yybh, dwbh = dwbh, kssj = kssj,jssj=jssj }).ToList();
            }
        }

        
    }
}
