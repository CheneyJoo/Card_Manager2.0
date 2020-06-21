 using System;
 using System.Data;
 using System.Text;
 using System.Data.SqlClient;
 using Dapper;
 using Service;

 namespace BLL
{
	/// <summary>
	/// 用途：业务逻辑类XT_ZZB 的摘要说明。
	///		系统_种子表
	/// 作者：吴水平
	/// 日期：2012-08-31 06:31:40
	/// </summary>
	public class XT_ZZB
	{
        /// <summary>
        /// 获取表中的种子号，生成流水号
        /// </summary>
        /// <param name="BM">表名</param>
        /// <returns>返回此表的流水号</returns>
        public string GetXTLSBH(string BM)
        {
            string result = string.Empty;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("BM",BM);
            parameters.Add("LSH", result, DbType.String, ParameterDirection.Output);
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var res2 = conn.Execute("PROC_XT_ZZB_LSBH", parameters, null, null, CommandType.StoredProcedure);
                return parameters.Get<string>("@LSH");
            }
        }

        /// <summary>
        /// 生成结算流水号
        /// </summary>
        /// <param name="BM">表名</param>
        /// <returns>返回此表的流水号</returns>
        public string GetJsLsh(int qdid)
        {
            string result = string.Empty;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("QDID", qdid);
            parameters.Add("LSH", result, DbType.String, ParameterDirection.Output);
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var res2 = conn.Execute("PROC_XT_ZZB_LSBH_V2", parameters, null, null, CommandType.StoredProcedure);
                return parameters.Get<string>("@LSH");
            }
        }

    }
}

