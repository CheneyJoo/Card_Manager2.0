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
    public class WjgzService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnModel Insert(Wjgz model)
        {
            try
            {
                //验证参数
                if (string.IsNullOrWhiteSpace(model.GZMC))
                {
                    return new ReturnModel { Code = 201, Msg = "规则名称不能为空" };
                }
               
                //保存
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        //判断重名
                        sbSql.Append("SELECT * FROM WJ_GZ WHERE GZMC=@GZMC and SFQY=1");
                        Wjgz sameName = conn.QueryFirstOrDefault<Wjgz>(sbSql.ToString(), model,transaction);
                        if (sameName != null)
                        {
                            return new ReturnModel { Code = 201, Msg = "存在相同名称的规则" };
                        }

                        sbSql.Clear();
                        model.SFQY = 1;
                        sbSql.Append("INSERT INTO WJ_GZ(GZMC, ZXNL, ZDNL, SFQY, XB) VALUES (@GZMC, @ZXNL, @ZDNL, @SFQY, @XB);select @@IDENTITY");
                        int gzid = Convert.ToInt32(conn.ExecuteScalar(sbSql.ToString(), model, transaction));
                        if (!string.IsNullOrWhiteSpace(model.XZBQ))
                        {
                            
                            List<string> lstBq = model.XZBQ.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var item in lstBq)
                            {
                                sbSql.Clear();
                                sbSql.Append("INSERT INTO WJ_GZMX(GZID, BQID) VALUES(@GZID, @BQID)");
                                conn.Execute(sbSql.ToString(), new { GZID = gzid , BQID = item}, transaction);
                            }
                            
                        }
                        transaction.Commit();
                        return new ReturnModel { Code = 200, Msg = "保存成功" };
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Log.WriteLog(e.Message);
                        return new ReturnModel { Code = 201, Msg = "保存失败" };
                    }
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
        public ReturnModel Update(Wjgz model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.GZMC))
                {
                    return new ReturnModel { Code = 201, Msg = "规则名称不能为空" };
                }
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        //判断标签存在
                        sbSql.Append("SELECT * FROM WJ_GZ WHERE ID=@ID AND SFQY=1");
                        Wjgz existModel = conn.QueryFirstOrDefault<Wjgz>(sbSql.ToString(), new { ID = model.ID },transaction);
                        if (existModel == null)
                        {
                            return new ReturnModel { Code = 201, Msg = "规则不存在" };
                        }

                        //判断重名
                        sbSql.Clear();
                        sbSql.Append("SELECT * FROM WJ_GZ WHERE ID<>@ID AND GZMC=@GZMC AND SFQY=1");
                        Wjgz sameName = conn.QueryFirstOrDefault<Wjgz>(sbSql.ToString(), model,transaction);
                        if (sameName != null)
                        {
                            return new ReturnModel { Code = 201, Msg = "存在相同名称的规则" };
                        }

                        //保存更新
                        sbSql.Clear();
                        sbSql.Append("UPDATE WJ_GZ SET GZMC = @GZMC, ZXNL = @ZXNL, ZDNL = @ZDNL, XB=@XB WHERE ID = @ID;");
                        sbSql.Append("DELETE FROM WJ_GZMX WHERE GZID = @ID;");
                        conn.Execute(sbSql.ToString(), model,transaction);
                        if (!string.IsNullOrWhiteSpace(model.XZBQ))
                        {
                            List<string> lstBq = model.XZBQ.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var item in lstBq)
                            {
                                sbSql.Clear();
                                sbSql.Append("INSERT INTO WJ_GZMX(GZID, BQID) VALUES(@GZID, @BQID)");
                                conn.Execute(sbSql.ToString(), new { GZID = model.ID, BQID = item }, transaction);
                            }
                        }
                        transaction.Commit();
                        return new ReturnModel { Code = 200, Msg = "保存成功" };
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Log.WriteLog(e.Message);
                        return new ReturnModel { Code = 201, Msg = "保存失败" };
                    }
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
                string sql = "DELETE FROM WJ_GZ  WHERE ID=@ID";
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
        public Wjgz GetEntity(int id)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM WJ_GZ WHERE ID=@ID");
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var model = conn.QueryFirstOrDefault<Wjgz>(sbSql.ToString(), new {ID = id});
                sbSql.Clear();
                sbSql.Append("SELECT a.*,b.BQMC FROM WJ_GZMX a INNER JOIN WJ_BQ b ON a.BQID=b.ID WHERE GZID=@ID");
                var lstBq = conn.Query<Wjgzmx>(sbSql.ToString(), new { ID = id }).ToList();
                if (model!=null)
                {
                    model.ListMx = lstBq;
                    model.XZBQ = string.Join(",", lstBq.Select(x => x.BQID).ToList());
                }

                return model;
            }
        }

        /// <summary>
        /// 获取规则列表
        /// </summary>
        /// <returns></returns>
        public List<Wjgz> GetList(string gzmc, int pageIndex, int pageSize, ref int count)
        {

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT a.*,STUFF((SELECT ','+bq.BQMC FROM  WJ_GZMX mx INNER JOIN WJ_BQ bq ON mx.BQID=bq.ID WHERE gzid=a.id FOR xml path('')),1,1,'') AS BQXQ  FROM WJ_GZ a  WHERE SFQY=1";
                if (!string.IsNullOrWhiteSpace(gzmc))
                {
                    sqlAll += " AND GZMC LIKE @Gzmc";
                    paramList.Add("Gzmc", "%" + gzmc + "%");
                }

                string sqlPage = "";
                int fromRow = (pageIndex - 1) * pageSize;
                int toRow = (pageIndex) * pageSize;
                paramList.Add("FromRow", fromRow);
                paramList.Add("ToRow", toRow);
                sqlPage = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS rowNum, * FROM (" + sqlAll + ") AS T ) AS N WHERE rowNum >@FromRow AND rowNum <=@ToRow ";

                List<Wjgz> li = conn.Query<Wjgz>(sqlPage, paramList).ToList();
                count = conn.Query<int>("SELECT COUNT(1) FROM (" + sqlAll + ") AS t ", paramList).FirstOrDefault();
                return li;
            }
        }



    }
}
