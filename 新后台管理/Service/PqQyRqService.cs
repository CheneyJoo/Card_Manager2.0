using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Service
{
    public class PqQyRqService
    {
        /// <summary>
        /// 获取渠道的每天排期对象
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<PqQyRqModel> GetList(string yybh, DateTime day)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT * FROM dbo.pq_tjrq WHERE yybh=@yybh AND rq=@rq";
                var list = conn.Query<PqQyRqModel>(sql, new { yybh = yybh, rq = day }).ToList();
                return list;
            }
        }

        /// <summary>
        /// 获取渠道的每天排期列表
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="yybh"></param>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public List<PqQyRqModel> GetList(string qybh, string yybh, DateTime ksrq, DateTime jsrq)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT * FROM dbo.pq_TJRQ WHERE qybh=@qybh AND yybh=@yybh AND rq BETWEEN @ksrq AND @jsrq";
                var list = conn.Query<PqQyRqModel>(sql, new { qybh = qybh, yybh = yybh, ksrq = ksrq, jsrq = jsrq }).ToList();
                return list;
            }
        }

    }
}
