 using System;
 using System.Data;
 using System.Text;
 using System.Data.SqlClient;
 using Dapper;
 using Service;

 namespace BLL
{
	/// <summary>
	/// ��;��ҵ���߼���XT_ZZB ��ժҪ˵����
	///		ϵͳ_���ӱ�
	/// ���ߣ���ˮƽ
	/// ���ڣ�2012-08-31 06:31:40
	/// </summary>
	public class XT_ZZB
	{
        /// <summary>
        /// ��ȡ���е����Ӻţ�������ˮ��
        /// </summary>
        /// <param name="BM">����</param>
        /// <returns>���ش˱����ˮ��</returns>
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
        /// ���ɽ�����ˮ��
        /// </summary>
        /// <param name="BM">����</param>
        /// <returns>���ش˱����ˮ��</returns>
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

