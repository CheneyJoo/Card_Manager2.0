using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
namespace Service
{
    public class CSService
    {
        public DataTable GetAllProvince()
        {
            string sqlString = "SELECT * FROM XT_SFB";
            using (IDbConnection dbConnection = new DapperConnection().DbConnection)
            {
                DataTable table = new DataTable();
                IDataReader dataReader = dbConnection.ExecuteReader(sqlString);
                table.Load(dataReader);
                return table;
            }
        }
        public DataTable GetCityByProvinceId(string id)
        {
            string sqlString = "SELECT A.CSBH,A.BM,A.CSMC FROM [dbo].[XT_CSLB] A WHERE A.SFID=@SFID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SFID", id);
            using (IDbConnection dbConnection = new DapperConnection().DbConnection)
            {
                DataTable table = new DataTable();
                IDataReader dataReader = dbConnection.ExecuteReader(sqlString, parameters);
                table.Load(dataReader);
                return table;
            }
        }
    }
}
