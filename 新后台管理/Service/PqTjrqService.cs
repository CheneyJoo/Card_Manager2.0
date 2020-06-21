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
    public class PqTjrqService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Update(PqTjrqModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update pq_tjrq set ");
            strSql.Append("yybh=@yybh,");
            strSql.Append("qybh=@qybh,");
            strSql.Append("rq=@rq,");
            strSql.Append("tjrs=@tjrs,");
            strSql.Append("flag=@flag");
            strSql.Append(" where id=@id");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(strSql.ToString(), model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="rq"></param>
        /// <returns></returns>
        public PqTjrqModel GetModel(string yybh, string qybh, DateTime rq)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT * FROM dbo.pq_tjrq WHERE yybh=@yybh AND qybh=@qybh AND rq =@rq";
                var model = conn.Query<PqTjrqModel>(sql, new { qybh = qybh, yybh = yybh, rq = rq }).FirstOrDefault();
                return model;
            }
        }

        public List<PqTjrqModel> GetList(string yybh, DateTime day)
        {

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT * FROM dbo.pq_tjrq WHERE yybh=@yybh AND rq = @rq";
                var list = conn.Query<PqTjrqModel>(sql, new { yybh = yybh, rq = day }).ToList();
                return list;
            }
        }

        /// <summary>
        /// 得到排期日期
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="qybh"></param>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public List<PqTjrqModel> GetList(string yybh, string qybh, DateTime ksrq, DateTime jsrq)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT * FROM dbo.pq_tjrq WHERE yybh=@yybh and qybh=@qybh AND rq BETWEEN @ksrq AND @jsrq";
                var list = conn.Query<PqTjrqModel>(sql, new { yybh = yybh, qybh = qybh, ksrq = ksrq, jsrq = jsrq }).ToList();
                return list;
            }
        }
    }
}
