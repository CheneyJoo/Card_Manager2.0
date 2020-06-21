using Dapper;
using Model;
using Model.Dto.ReportQy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ReportQyService
    {
       
        /// <summary>
        /// 首页今日预约人数
        /// </summary>
        /// <param name="yybh"></param>
        ///  <param name="ddly">0个检，1团检</param>
        /// <returns></returns>
        public string GetHomejryrs(string yybh, int ddly)
        {
            string sql = "select count(1) from dd_jbxx_new  where yybh='" + yybh + "' and ddly=" + ddly + " and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
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
        public string GetHomezryrs(string yybh, int ddly)
        {
            string sql = "select count(1) from dd_jbxx_new  where yybh='" + yybh + "' and ddly=" + ddly + " and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Query<decimal>(sql, null).FirstOrDefault().ToString("N0");
            }
        }
        /// <summary>
        /// 预约今日团检总进度
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns>百分比，80/100</returns>
        public List<string> GetHometjzryyjd(string yybh)
        {
            List<string> li = new List<string>();
            string sql = "select count(1) from dd_jbxx_new  where yybh='" + yybh + "' and ddly=1 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
            int daystotal = 0;
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                daystotal = conn.Query<int>(sql, null).FirstOrDefault();
            }
            int tjyl = new PqJbszService().GetModel(yybh).tjyl;
            li.Add((daystotal * 1.0 / tjyl * 100).ToString("0.##") + "%");
            li.Add(daystotal.ToString() + "/" + tjyl.ToString());
            return li;
        }
        /// <summary>
        /// 团检今日到检总进度
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns>百分比，80/100</returns>
        public List<string> GetHometjdjzjd(string yybh)
        {
            List<string> li = new List<string>();
            string sqlDj = "select count(1) from dd_jbxx_new  where yybh='" + yybh + "' and ddly=1 and ddzt in(6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
            int daysDjtotal = 0;
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                daysDjtotal = conn.Query<int>(sqlDj, null).FirstOrDefault();
            }

            string sql = "select count(1) from dd_jbxx_new  where yybh='" + yybh + "' and ddly=1 and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
            int daystotal = 0;
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                daystotal = conn.Query<int>(sql, null).FirstOrDefault();
            }
            if (daystotal == 0)
            {
                li.Add("0.00%");
            }
            else
            {
                li.Add((daysDjtotal * 1.0 / daystotal * 100).ToString("0.##") + "%");
            }
            li.Add(daysDjtotal.ToString() + "/" + daystotal.ToString());
            return li;
        }
        /// <summary>
        /// 预约日环比
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomeryyrhb(string yybh, int ddly)
        {
            string sqlold = "select count(1)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=" + ddly + " and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
            string sqlnew = "select count(1)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=" + ddly + " and ddzt in (2,3,6,7,9) and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "' ";

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
        /// 团检到检日环比
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public string GetHomeTjDjrhb(string yybh)
        {
            string sqlold = "select count(1)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=1 and ddzt=6 and yykssj>='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
            string sqlnew = "select count(1)  from dd_jbxx_new  where yybh='" + yybh + "' and ddly=1 and ddzt=6 and yykssj>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "' ";

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
        /// 团检在检企业数
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public int GetZjqys(string yybh)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string sqlqy = "select  count(1)  from pq_tjrq a  where a.rq= '" + date + "' and a.yybh = '" + yybh + "' and a.flag=0";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.ExecuteScalar<int>(sqlqy, null);

            }
        }

        /// <summary>
        /// 团检排期报表甘特图
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>

        public List<GttQy> GetPqqy(string yybh)
        {
            string start = DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01";
            string end = DateTime.Now.AddMonths(2).ToString("yyyy-MM") + "-01";
            string sqlqy = "select  a.qybh,b.mc as qymc, min(rq) as rq from pq_tjrq a join qy_jbxx b on a.yybh = b.yybh and a.qybh = b.bh where b.sfqd=0 and b.sfqd=0 and rq>= '" + start + "' and rq< '" + end + "'  and a.yybh = '" + yybh + "' and flag=0 group by qybh,b.mc order by rq";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<GttQy> li = conn.Query<GttQy>(sqlqy, null).ToList();
                int i = 1;
                foreach (GttQy model in li)
                {
                    model.id = i++;
                }
                return li;
            }
        }
        /// <summary>
        /// 团检排期报表甘特图明细
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<PqTjrqModel> GetPqqyList(string yybh)
        {
            string start = DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01";
            string end = DateTime.Now.AddMonths(2).ToString("yyyy-MM") + "-01";
            string sqlqy = "select * from pq_tjrq where rq>= '" + start + "' and rq< '" + end + "'  and yybh = '" + yybh + "' and flag=0";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<PqTjrqModel> li = conn.Query<PqTjrqModel>(sqlqy, null).ToList();
                return li;
            }
        }

        /// <summary>
        /// 团检排期报表甘特图明细,合计
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>      
        public List<GttHj> GetPqqyRsList(string yybh)
        {
            string start = DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01";
            string end = DateTime.Now.AddMonths(2).ToString("yyyy-MM") + "-01";
            string sqlqy = "select convert(varchar(10),yykssj,120) as yysj,count(1) as counts from dd_jbxx_new where yybh='" + yybh + "' and ddly=1  and yykssj>= '" + start + "' and yykssj< '" + end + "' and dwbh in (select qybh from pq_tjrq where rq>= '" + start + "' and rq< '" + end + "'  and yybh = '" + yybh + "' and flag=0 group by qybh) group by convert(varchar(10), yykssj, 120)";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<GttHj> li = conn.Query<GttHj>(sqlqy, null).ToList();
                return li;
            }
        }
        /// <summary>
        /// 团检结算中心订单导出
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="dwbh"></param>
        /// <returns></returns>
        public DataTable GetTjJsExport(string yybh, string dwbh)
        {
            DataTable dt = new DataTable();
            DataTable dtWj = new DataTable();

            StringBuilder sbSql = new StringBuilder();

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {

                string sql = string.Format(@"SELECT c.ddbh, a.mc as 单位,c.dwmc  as 部门, b.rybh as 工号,c.xm as 姓名, c.tcmc AS 套餐名称, c.ddze as 订单金额,c.ysjsj as 结算价,  c.dh AS 电话,c.zjhm as 证件号码,
                case  c.sfdj when 1 then '已到检' else '未到检 ' end as 到检状态,   case  c.sfdj when 1 then c.djtime else ' ' end as 到检时间,'' as 未检项目
                FROM  dd_jbxx_new c join qy_jbxx a on left(c.dwbh,4)=a.bh  and c.yybh=a.yybh  join qy_ygxx b on c.yybh=b.yybh and c.dsfdd=b.ydjh  WHERE c.yybh = '{0}' and left(c.dwbh,4)='{1}' and ddly = 1 ", yybh, dwbh);

                string sqlWj = "select zhxmmc,jg,a.ddbh from dd_zhxm a join dd_jbxx b on a.ddbh=b.ddbh where a.sfdj=0 and b.yybh='" + yybh + "' and  left(b.dwbh,4)='" + dwbh + "' and ddly=1 and a.sfdj=1";

                var reader = conn.ExecuteReader(sql, null);
                dt.Load(reader);

                var reader2 = conn.ExecuteReader(sqlWj, null);
                dtWj.Load(reader2);

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["到检状态"].ToString() == "已到检")
                    {
                        DataRow[] founds = dtWj.Select(" ddbh='" + dr["ddbh"].ToString() + "'");
                        if (founds.Length > 1)
                        {
                            string xm = "";
                            foreach (DataRow drDetail in founds)
                            {
                                xm += drDetail["zhxmmc"] + ":" + drDetail["jg"] + "，";
                            }
                            xm = xm.TrimEnd('，');
                            dr["未检项目"] = xm;
                        }
                    }
                }
                dt.Columns.Remove("ddbh");
                return dt;
            }

        }
    }
}
