using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Dapper;

namespace Service
{
    public class DapperConnection
    {
        private static string _connectionString;
        private static string _connectionKKString;

        /// <summary>
        /// 链接数据库
        /// </summary>
        public DapperConnection()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        }


        public IDbConnection DbConnection
        {
            get
            {
                return GetConnection();
            }
        }
        public IDbConnection DbConnectionKk
        {
            get
            {
                return GetConnectionKk();
            }
        }

        private IDbConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        private IDbConnection GetConnectionKk()
        {
            SqlConnection connection = new SqlConnection(_connectionKKString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="list">SQL列表 Key:Sql Value:参数</param>
        /// <returns></returns>
        public static int ExecuteTransaction(List<KeyValuePair<string, object>> list)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var transaction = con.BeginTransaction();

                try
                {
                    int result = 0;
                    foreach (var sql in list)
                    {
                        result += con.Execute(sql.Key, sql.Value, transaction);
                    }

                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.WriteLog(ex.Message);
                    return -1;
                }
            }
        }
    }
}
