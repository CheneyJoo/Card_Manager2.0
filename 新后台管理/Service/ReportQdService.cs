using Dapper;
using Model;
using Model.Dto.ReportQd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ReportQdService
    {

        /// <summary>
        /// 医院时间段营业额
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetYyeByPeriod(string yybh, string fromDate, string toDate, int qdId = 0)
        {
            string sql = "select isnull(sum(ddze),0)  from dd_jbxx  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + fromDate + "' and yykssj<'" + toDate + "' ";
            if (qdId > 0)
            {
                sql += " and dsfbzid=" + qdId.ToString();
            }
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0"); ;
            }
        }
        /// <summary>
        /// 首页今日预约人数
        /// </summary>
        /// <param name="yybh"></param>
        ///  <param name="ddly">0个检，1团检</param>
        /// <returns></returns>
        public string GetHomejryrs(string yybh, int ddly)
        {
            string sql = "select count(1) from dd_jbxx  where yybh='" + yybh + "' and ddly=" + ddly + " and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0");
            }
        }
        /// <summary>
        /// 医院当日营业额
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetdrYye(string yybh,int qdId=0)
        {
            string sql = "select isnull(sum(ddze),0)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "' ";
            if(qdId>0)
            {
                sql += " and dwbh=" + qdId.ToString();
            }
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0"); ;
            }
        }
        /// <summary>
        /// 医院上一日营业额
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetzrYye(string yybh)
        {
            string sql = "select isnull(sum(ddze),0)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0"); ;
            }
        }

        /// <summary>
        /// 首页日环比
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomerhb(string yybh)
        {
            string sqlold = "select isnull(sum(ddze),0)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
            string sqlnew = "select isnull(sum(ddze),0)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "' ";

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                decimal old = conn.Query<decimal>(sqlold, null).FirstOrDefault();
                decimal news = conn.Query<decimal>(sqlnew, null).FirstOrDefault();
                if (old == 0)
                {
                    return "100%";
                }
                if (news == 0)
                {
                    return "-100%";
                }
                return ((news - old) / old * 100).ToString("0.##") + "%";

            }
        }
    
        /// <summary>
        /// 首页每天最大预约人数
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomezyyrs(string yybh)
        {
            string sql = "select qdyl from pq_jbsz where tjjgid='"+yybh+"'";           
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
              return conn.Query<decimal>(sql, null).FirstOrDefault().ToString();      
            }
        }
    

        /// <summary>
        /// 首页今日预约人数
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomejryrs(string yybh)
        {
            string sql = "select count(1) from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='"+DateTime.Now.ToString("yyyy-MM-dd") +"' and yykssj<'"+DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")+"'";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0");
            }
        }
        /// <summary>
        /// 首页昨日预约人数
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomezryrs(string yybh)
        {
            string sql = "select count(1) from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0");
            }
        }


        /// <summary>
        /// 今日预约总进度
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomezryyjd(string yybh)
        {
            string sql = "select count(1) from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
            int daystotal = 0;
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                daystotal = conn.Query<int>(sql, null).FirstOrDefault();
            }
            int qdl = 0;
            var pqModel = new PqJbszService().GetModel(yybh);
            if (pqModel !=null)
            {
                qdl = pqModel.zdjd;
            }
            return qdl.Equals(0)?"100%":(daystotal*1.0/qdl*100).ToString("0.##") + "%";

        }
        /// <summary>
        /// 预约日环比
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomeryyrhb(string yybh)
        {
            string sqlold = "select count(1)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
            string sqlnew = "select count(1)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "' ";

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                decimal old = conn.Query<decimal>(sqlold, null).FirstOrDefault();
                decimal news = conn.Query<decimal>(sqlnew, null).FirstOrDefault();
                if (old == 0)
                {
                    return "100%";
                }
                if (news == 0)
                {
                    return "-100%";
                }
                return ((news - old) / old * 100).ToString("0.##") + "%";

            }
        }

   


        /// <summary>
        /// 医院近30天营业额
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string Get30Yye(string yybh,int qdId=0)
        {
            string sql = "select isnull(sum(ddze),0)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            if (qdId > 0)
            {
                sql += " and dsfbzid=" + qdId.ToString();
            }
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0"); ;
            }
        }
       
        /// <summary>
        /// 医院当月营业额
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetdyYye(string yybh,int qdId=0)
        {
            string sql = "select isnull(sum(ddze),0)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-01") +"' and yykssj<'" + DateTime.Now.AddMonths(1).ToString("yyyy-MM-01") + "'";
            if (qdId > 0)
            {
                sql += " and dwbh=" + qdId.ToString();
            }
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0"); ;
            }
        }
        /// <summary>
        /// 医院营业额报表list
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<YyYsReport> ReporYyYsList(string start,string end ,string yybh)
        {
            string sql = string.Format(@"select a.*,b.ddze,b.ddzs,isnull(c.tkzs,0) as tkzs,isnull(c.tkze,0) as tkze from (select a.bh as qdid, a.mc as qdmc from qy_jbxx a  where a.sfqd=1 and a.yybh='{0}') a join

            (select isnull(sum(ddze), 0) as ddze, count(1) as ddzs, dwbh from dd_jbxx_new  where yybh ='{0}' and ddly = 0 and ddzt in (2, 3, 6, 7, 9) and yykssj>='{1}' and yykssj<'{2}'
            group by dwbh) b on a.qdid = b.dwbh
            left join
            (
            select isnull(sum(tkze), 0) as tkze, count(1) as tkzs, dwbh from dd_jbxx_new  where yybh = '{0}' and ddly = 0 and ddzt in (2, 3, 6, 7, 9) and yykssj>='{1}' and yykssj<'{2}' and tkze> 0
            group by dwbh) c on b.dwbh = c.dwbh where 1=1", yybh, start, Convert.ToDateTime(end).AddDays(1).ToString("yyyy-MM-dd"));

            DynamicParameters paramList = new DynamicParameters();
           
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {             
                List<YyYsReport> li = conn.Query<YyYsReport>(sql, null).ToList();              
                return li;
            }
        }

        /// <summary>
        /// 医院端导出营收报表
        /// </summary>
        /// <param name="jsjlid"></param>
        /// <returns></returns>
        public DataTable GetYsExport(string start, string end, string yybh,int qdid)
        {
            DataTable dt = new DataTable();

            StringBuilder sbSql = new StringBuilder();

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
               
               string sql=  string.Format(@"SELECT d.mc AS 订单来源, c.ddbh AS 单号, c.tcmc AS 套餐名称, c.xm AS 姓名, c.dh AS 电话,
c.ddze as 订单金额, c.yykssj as 预约时间, case c.ddzt when 2 then '预约中' when 3 then '预约成功' when 6 then '已到检' when 7 then '已完成' when 9 then '已出报告' end as 套餐状态
 FROM  dd_jbxx_new c 
 join  qy_jbxx d ON c.dwbh = d.bh and c.yybh=d.yybh    WHERE  ddly = 0   and ddzt in (2, 3, 6, 7, 9)  and yykssj>='{0}' 	 and yykssj<'{1}'  ", start, Convert.ToDateTime(end).AddDays(1).ToString());

                var reader = conn.ExecuteReader(sql, null);
                dt.Load(reader);
                return dt;
            }

        }

        #region 渠道端报表



        /// <summary>
        /// 渠道端首页日环比
        /// </summary>
        /// <param name="dsfbzid"></param>
        /// <returns></returns>
        public string GetHomerhbQd(string dsfbzid)
        {
            string sqlold = "select isnull(sum(ddze),0)  from dd_jbxx  where dsfbzid='" + dsfbzid + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
            string sqlnew = "select isnull(sum(ddze),0)  from dd_jbxx  where dsfbzid='" + dsfbzid + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "' ";

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                decimal old = conn.Query<decimal>(sqlold, null).FirstOrDefault();
                decimal news = conn.Query<decimal>(sqlnew, null).FirstOrDefault();
                if (old == 0)
                {
                    return "100%";
                }
                if (news == 0)
                {
                    return "-100%";
                }
                return ((news - old) / old * 100).ToString("0.##") + "%";

            }
        }

        /// <summary>
        /// 渠道端首页当日营业额
        /// </summary>
        /// <param name="dsfbzid"></param>
        /// <returns></returns>
        public string GetdrYyeQd(string dsfbzid)
        {
            string sql = "select isnull(sum(ddze),0)  from dd_jbxx  where dsfbzid='" + dsfbzid + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'   ";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0"); ;
            }
        }
        /// <summary>
        /// 渠道端首页昨日营业额
        /// </summary>
        /// <param name="dsfbzid"></param>
        /// <returns></returns>
        public string GetzrYyeQd(string dsfbzid)
        {
            string sql = "select isnull(sum(ddze),0)  from dd_jbxx  where dsfbzid='" + dsfbzid + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'   ";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0"); ;
            }
        }

        /// <summary>
        /// 渠道端首页今日预约人数
        /// </summary>
        /// <param name="dsfbzid"></param>
        /// <returns></returns>
        public string GetHomejryrsQd(string dsfbzid)
        {
            string sql = "select count(1) from dd_jbxx  where dsfbzid='" + dsfbzid + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0");
            }
        }
        /// <summary>
        /// 渠道端首页昨日预约人数
        /// </summary>
        /// <param name="dsfbzid"></param>
        /// <returns></returns>
        public string GetHomezryrsQd(string dsfbzid)
        {
            string sql = "select count(1) from dd_jbxx  where dsfbzid='" + dsfbzid + "' and ddly=0 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var result = conn.Query<decimal?>(sql, null).FirstOrDefault();
                if (result !=null)
                {
                    return result.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// 已结算金额
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomeyjs(string qdid)
        {
            string sql = "select isnull(sum(b.sjjsj),0) as sjjsj  from dd_jbxx a join qd_jsjbxxmx b on a.ddbh=b.ddbh where a.sfjs=1 and dsfbzid='" + qdid + "' and a.ddly=0";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0");
            }
        }
        /// <summary>
        /// 待结算金额
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomedjs(string qdid)
        {
            string sql = "select isnull(sum(ysjsj),0) as ysjsj  from dd_jbxx  where sfjs=0 and ddzt in (2,3,6,7,9) and  dsfbzid='" + qdid + "' and ddly=0";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0");
            }
        }

        #endregion
    }
}
