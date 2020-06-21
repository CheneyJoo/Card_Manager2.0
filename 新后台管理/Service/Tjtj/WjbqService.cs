using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;

namespace Service
{
    public class WjbqService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnModel Insert(Wjbq model)
        {
            try
            {
                //验证参数
                if (string.IsNullOrWhiteSpace(model.BQMC))
                {
                    return new ReturnModel { Code = 201, Msg = "标签名称不能为空" };
                }
               
                //保存
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    //判断重名
                    sbSql.Append("SELECT * FROM WJ_BQ WHERE bqmc=@bqmc and sfqy=1");
                    Wjbq sameName = conn.QueryFirstOrDefault<Wjbq>(sbSql.ToString(), model);
                    if (sameName != null)
                    {
                        return new ReturnModel { Code = 201, Msg = "存在相同名称的标签" };
                    }

                    sbSql.Clear();
                    model.SFQY = 1;
                    sbSql.Append("INSERT INTO WJ_BQ(BQMC, BQXQ, SFQY) VALUES (@BQMC, @BQXQ, @SFQY)");
                    int result = conn.Execute(sbSql.ToString(), model);
                    if (result.Equals(1))
                    {
                        return new ReturnModel { Code = 200, Msg = "保存成功" };
                    }
                    return new ReturnModel { Code = 201, Msg = "保存失败" };
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return new ReturnModel { Code = 201, Msg = "保存失败" };
            }

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public ReturnModel Update(Wjbq model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.BQMC))
                {
                    return new ReturnModel { Code = 201, Msg = "标签名称不能为空" };
                }
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    //判断标签存在
                    sbSql.Append("SELECT * FROM WJ_BQ WHERE Id=@Id AND SFQY=1");
                    Wjbq existModel = conn.QueryFirstOrDefault<Wjbq>(sbSql.ToString(), new { Id = model.ID });
                    if (existModel == null)
                    {
                        return new ReturnModel { Code = 201, Msg = "标签不存在" };
                    }

                    //判断重名
                    sbSql.Clear();
                    sbSql.Append("SELECT * FROM WJ_BQ WHERE ID<>@ID AND BQMC=@BQMC and SFQY=1");
                    Wjbq sameName = conn.QueryFirstOrDefault<Wjbq>(sbSql.ToString(), model);
                    if (sameName != null)
                    {
                        return new ReturnModel { Code = 201, Msg = "存在相同名称的标签" };
                    }

                    //保存更新
                    sbSql.Clear();
                    sbSql.Append("UPDATE WJ_BQ SET BQMC = @BQMC, BQXQ = @BQXQ WHERE ID = @ID");
                    int result = conn.Execute(sbSql.ToString(), model);
                    if (result.Equals(1))
                    {
                        return new ReturnModel { Code = 200, Msg = "保存成功" };
                    }

                    return new ReturnModel { Code = 201, Msg = "保存失败" };
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return new ReturnModel { Code = 201, Msg = "保存失败" };
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public ReturnModel Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM WJ_BQ  WHERE ID=@ID";
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    int rows = conn.Execute(sql, new { ID = id });
                    if (rows > 0)
                    {
                        return new ReturnModel { Code = 200, Msg = "删除成功" };
                    }
                    else
                    {
                        return new ReturnModel { Code = 201, Msg = "删除失败" };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return new ReturnModel { Code = 201, Msg = "删除失败" };
            }

        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Wjbq GetEntity(int id)
        {
            string sql = "SELECT * FROM dbo.WJ_BQ WHERE ID=@ID";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.QueryFirstOrDefault<Wjbq>(sql, new {ID = id});
            }
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        public List<Wjbq> GetList(string bqmc, int pageIndex, int pageSize, ref int count)
        {

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT * FROM WJ_BQ WHERE SFQY=1";
                if (!string.IsNullOrWhiteSpace(bqmc))
                {
                    sqlAll += " AND BQMC LIKE @Bqmc";
                    paramList.Add("Bqmc", "%" + bqmc + "%");
                }

                string sqlPage = "";
                int fromRow = (pageIndex - 1) * pageSize;
                int toRow = (pageIndex) * pageSize;
                paramList.Add("FromRow", fromRow);
                paramList.Add("ToRow", toRow);
                sqlPage = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS rowNum, * FROM (" + sqlAll + ") AS T ) AS N WHERE rowNum >@FromRow AND rowNum <=@ToRow ";

                List<Wjbq> li = conn.Query<Wjbq>(sqlPage, paramList).ToList();
                count = conn.Query<int>("SELECT COUNT(1) FROM (" + sqlAll + ") AS t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        /// 获取所有标签列表
        /// </summary>
        /// <returns></returns>
        public List<Wjbq> GetAllList()
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT * FROM WJ_BQ WHERE SFQY=1";
                List<Wjbq> li = conn.Query<Wjbq>(sqlAll, paramList).ToList();
                return li;
            }
        }

    }
}
