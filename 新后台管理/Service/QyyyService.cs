using Dapper;
using Model;
using Model.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 企业预约管理
    /// </summary>
    public class QyyyService
    {
        /// <summary>
        /// 企业预约管理列表
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="pqstart"></param>
        /// <param name="paend"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QyyyModel> GetQyygjbxxList(string yybh,string dwbh, string pqstart,string pqend, int pageIndex, int pageSize, ref int count)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = @"select a.id, a.yybh, b.bh as dwbh ,b.mc as dwmc,a.ksrq,a.jsrq,c.tjrs,d.ylme,isnull(e.yyrs,0) as yyrs,isnull(h.djrs,0) as djrs from pq_qysz a join qy_jbxx b on a.yybh=b.yybh and a.qybh=b.bh 
                join(select count(1) as tjrs, left(dwbh, 4) as dwbh, yybh from qy_ygxx group by yybh, left(dwbh, 4)) c on a.yybh = c.yybh and a.qybh = c.dwbh--体检人数
                join(select sum(tjrs) as ylme, pqbh from pq_tjrq group by pqbh) d on a.pqbh = d.pqbh--预留名额
                left join(select count(1) as yyrs, left(dwbh, 4) as dwbh, yybh from qy_ygxx where sfyy = 1 group by yybh, left(dwbh, 4)) e on a.yybh = e.yybh and a.qybh = e.dwbh--预约人数
                left join(select count(1) as djrs, left(dwbh, 4) as dwbh, yybh from dd_jbxx_new where ddly = 1 and ddzt in (6, 7, 9) group by yybh,left(dwbh, 4)) h on a.yybh = h.yybh and a.qybh = h.dwbh --到检人数 
                where 1=1";
                StringBuilder sd = new StringBuilder();

                DynamicParameters paramList = new DynamicParameters();
                if (!string.IsNullOrEmpty(yybh))
                {
                    sd.Append(" and a.yybh = @yybh");
                    paramList.Add("yybh", yybh);
                }
                if (!string.IsNullOrEmpty(dwbh))
                {
                    sd.Append(" and a.qybh=@dwbh");
                    paramList.Add("dwbh",dwbh);
                }
                if (!string.IsNullOrEmpty(pqstart))
                {
                    sd.Append(" and a.jsrq>=@ksrq");
                    paramList.Add("ksrq", pqstart);
                }
                if (!string.IsNullOrEmpty(pqend))
                {
                    sd.Append(" and a.jsrq<=@jsrq");
                    paramList.Add("jsrq",pqend);
                }

                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<QyyyModel> li = conn.Query<QyyyModel>(sqlNew, paramList).ToList();
                foreach(QyyyModel model in li)
                {
                    model.yyl = (model.yyrs * 1.0 / model.tjrs * 100).ToString("0.##") + "%";
                    if (model.yyrs == 0)
                    {
                        model.djjd = "0.00%";
                    }
                    else
                    {
                        model.djjd = (model.djrs * 1.0 / model.tjrs * 100).ToString("0.##") + "%";
                    }
                    if(DateTime.Now<model.ksrq)
                    {
                        model.tjzt = "未开始";
                    }
                    else if(DateTime.Now>model.jsrq)
                    {
                        model.tjzt = "已完成";
                    }
                    else
                    {
                        model.tjzt = "进行中";
                    }
                }
            
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        /// 团检首页top 5到检率
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="dwbh"></param>
        /// <returns></returns>
        public List<QyyyModel> GetQyygjbxxHome(string yybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = @"select top 5 a.id, a.yybh, b.bh as dwbh ,b.mc as dwmc,a.ksrq,a.jsrq,e.tjrs,isnull(h.djrs,0) as djrs ,CAST(isnull(h.djrs,0) AS FLOAT)/e.tjrs as jd from pq_qysz a join qy_jbxx b on a.yybh=b.yybh and a.qybh=b.bh 
       
               
                join(select count(1) as tjrs, left(dwbh, 4) as dwbh, yybh from qy_ygxx   group by yybh, left(dwbh, 4)) e on a.yybh = e.yybh and a.qybh = e.dwbh--总人数
                left join(select count(1) as djrs, left(dwbh, 4) as dwbh, yybh from dd_jbxx_new where ddly = 1 and ddzt in (6, 7, 9) group by yybh,left(dwbh, 4)) h on a.yybh = h.yybh and a.qybh = h.dwbh --到检人数 
                where 1=1";
                StringBuilder sd = new StringBuilder();
                DynamicParameters paramList = new DynamicParameters();

                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", yybh);

                sd.Append(" and a.ksrq<=@ksrq");
                paramList.Add("ksrq", DateTime.Now.ToString("yyyy-MM-dd"));

                sd.Append(" and a.jsrq>=@jsrq");
                paramList.Add("jsrq", DateTime.Now.ToString("yyyy-MM-dd"));


                sql += sd.ToString();
                sql += " order by jd desc";
                List<QyyyModel> li = conn.Query<QyyyModel>(sql, paramList).ToList();
                foreach (QyyyModel model in li)
                {
                    model.djjd = (model.djrs * 1.0 / model.tjrs * 100).ToString("0.##") + "%";
                }

                return li;
            }
        }
        /// <summary>
        /// vip 预约,当日
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<DdjbxxModel> GetVipDdDay(string yybh)
        {
            string sql = "select a.xm,c.mc as dwmc,b.yykssj,b.dh,b.ddzt from qy_ygxx a join dd_jbxx_new b on a.yybh=b.yybh and a.ydjh=b.dsfdd and b.ddly=1 join qy_jbxx c on left(a.dwbh,4) =c.bh and a.yybh=c.yybh where a.sfvip=1  and b.yykssj >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and b.yykssj<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "' and b.yybh = '" + yybh + "'";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sql, null).ToList();
                return li;
            }
        }
        /// <summary>
        /// vip 预约本周
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<DdjbxxModel> GetVipDdZhou(string yybh)
        {
            DateTime start = GetFirstDayOfWeek(DateTime.Now);
            DateTime end = GetFirstDayOfWeek(DateTime.Now).AddDays(8);
            string sql = "select a.xm,c.mc as dwmc,b.yykssj,b.dh,b.ddzt from qy_ygxx a join dd_jbxx_new b on a.yybh=b.yybh and a.ydjh=b.dsfdd and b.ddly=1 join qy_jbxx c on left(a.dwbh,4) =c.bh and a.yybh=c.yybh where a.sfvip=1  and b.yykssj >= '" + start.ToString("yyyy-MM-dd") + "' and b.yykssj<'" + end.AddDays(1).ToString("yyyy-MM-dd") + "' and b.yybh = '" + yybh + "'";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sql, null).ToList();
                return li;
            }
        }
        private DateTime GetFirstDayOfWeek(DateTime dt)
        {
            dt = dt == null ? DateTime.Now : dt;
            int daydiff = (int)dt.DayOfWeek - 1 < 0 ? 6 : (int)dt.DayOfWeek - 1;//如果是0结果小于0表示周日 那最后要减6天:其他天数在dayOfWeek上减1，表示回到周一
            DateTime result = dt.AddDays(-daydiff);
            return result;
        }




        /// <summary>
        /// 获取企业预约数据
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="qybh"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DdjbxxModel> GetOrderDataByDateRange(string yybh, string qybh, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT c.mc AS dwmc,a.xm,b.yykssj,b.yyjssj,b.ddzt,b.dh FROM dbo.qy_ygxx a INNER JOIN dbo.dd_jbxx_new b ON a.ydjh=b.dsfdd INNER JOIN dbo.qy_jbxx c ON a.dwbh=c.bh WHERE a.yybh=@yybh AND b.dwbh LIKE @dwbh AND b.yykssj >=@startDate AND b.yykssj<=@endDate";
            var param = new { yybh = yybh, qybh = qybh, startDate = startDate, endDate = endDate };
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sql, param).ToList();
                return li;
            }
        }
    }
}
