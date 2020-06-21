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
    public class WjtcService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnModel Insert(Wjtc model)
        {
            try
            {
                //验证参数
                if (string.IsNullOrWhiteSpace(model.TCID))
                {
                    return new ReturnModel { Code = 201, Msg = "套餐不能为空" };
                }
               
                //保存
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        //判断重名
                        sbSql.Append("SELECT * FROM WJ_TC WHERE TCID=@TCID and SFQY=1");
                        Wjbq sameName = conn.QueryFirstOrDefault<Wjbq>(sbSql.ToString(), model, transaction);
                        if (sameName != null)
                        {
                            return new ReturnModel { Code = 201, Msg = "存在相同名称的套餐" };
                        }

                        sbSql.Clear();
                        model.SFQY = 1;
                        sbSql.Append("INSERT INTO WJ_TC(TCID, SYRQ, TCZY, SFQY) VALUES (@TCID, @SYRQ, @TCZY, @SFQY);select @@IDENTITY");
                        int id = Convert.ToInt32(conn.ExecuteScalar(sbSql.ToString(), model, transaction));

                        if (!string.IsNullOrWhiteSpace(model.BQIDS))
                        {
                            //标签列表
                            List<string> lstBq = model.BQIDS.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            //深度列表
                            List<string> lstSd = model.BQSDS.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var item in lstBq)
                            {
                                var sd = lstSd[lstBq.IndexOf(item)];
                                sbSql.Clear();
                                sbSql.Append("INSERT INTO WJ_TCBQ(GLID, BQID, JCSD) VALUES (@GLID, @BQID, @JCSD)");
                                conn.Execute(sbSql.ToString(), new { GLID = id, BQID = item , JCSD = sd }, transaction);
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
        public ReturnModel Update(Wjtc model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.TCID))
                {
                    return new ReturnModel { Code = 201, Msg = "套餐不能为空" };
                }
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        //判断存在
                        sbSql.Append("SELECT * FROM WJ_TC WHERE Id=@Id AND SFQY=1");
                        Wjbq existModel = conn.QueryFirstOrDefault<Wjbq>(sbSql.ToString(), new { Id = model.ID }, transaction);
                        if (existModel == null)
                        {
                            return new ReturnModel { Code = 201, Msg = "标签不存在" };
                        }

                        //判断重名
                        sbSql.Clear();
                        sbSql.Append("SELECT * FROM WJ_TC WHERE ID<>@ID AND TCID=@TCID and SFQY=1");
                        Wjbq sameName = conn.QueryFirstOrDefault<Wjbq>(sbSql.ToString(), model, transaction);
                        if (sameName != null)
                        {
                            return new ReturnModel { Code = 201, Msg = "存在相同名称的套餐" };
                        }

                        //保存更新
                        sbSql.Clear();
                        model.SFQY = 1;
                        sbSql.Append("UPDATE WJ_TC SET TCID = @TCID, SYRQ = @SYRQ, TCZY = @TCZY, SFQY = @SFQY WHERE ID = @ID;");
                        sbSql.Append("DELETE FROM WJ_TCBQ WHERE GLID = @ID;");
                        conn.Execute(sbSql.ToString(), model, transaction);
                        if (!string.IsNullOrWhiteSpace(model.BQIDS))
                        {
                            //标签列表
                            List<string> lstBq = model.BQIDS.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            //深度列表
                            List<string> lstSd = model.BQSDS.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var item in lstBq)
                            {
                                var sd = lstSd[lstBq.IndexOf(item)];
                                sbSql.Clear();
                                sbSql.Append("INSERT INTO WJ_TCBQ(GLID, BQID, JCSD) VALUES (@GLID, @BQID, @JCSD)");
                                conn.Execute(sbSql.ToString(), new { GLID = model.ID, BQID = item, JCSD = sd }, transaction);
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
            StringBuilder sbSql = new StringBuilder();
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    sbSql.Append($"DELETE FROM WJ_TCBQ WHERE GLID={id}");
                    conn.Execute(sbSql.ToString(), null, transaction);

                    sbSql.Clear();
                    sbSql.Append($"DELETE FROM WJ_TC WHERE ID={id}");
                    conn.Execute(sbSql.ToString(), null, transaction);

                    transaction.Commit();
                    return new ReturnModel { Code = 200, Msg = "删除成功" };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Log.WriteLog(e.Message);
                    return new ReturnModel { Code = 201, Msg = "删除失败" };
                }
            }

        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Wjtc GetEntity(int id)
        {
            string sql = "SELECT a.*, b.TCMC, b.JG FROM dbo.WJ_TC a LEFT JOIN xt_tcb b ON a.TCID=b.tcbh WHERE a.SFQY=1 AND b.SFQY=1 AND a.ID=@ID";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var model = conn.QueryFirstOrDefault<Wjtc>(sql, new {ID = id});
                if (model != null)
                {
                    sql = "SELECT a.*,b.BQMC FROM dbo.WJ_TCBQ a INNER JOIN WJ_BQ b ON a.BQID=b.ID WHERE GLID=@ID";
                    var listBq = conn.Query<Wjtcbq>(sql, new { ID = id }).ToList();
                    model.ListBq = listBq;
                }

                return model;
            }
        }

        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <returns></returns>
        public List<Wjtc> GetList(string tcbh, string tcmc, string yybh, int pageIndex, int pageSize, ref int count)
        {

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT a.*, b.YYBH, b.JG, b.TCMC FROM WJ_TC a LEFT JOIN xt_tcb b ON a.TCID=b.tcbh WHERE a.SFQY=1 AND b.SFQY=1 and dwbh='0000' and tclx=2";
                paramList.Add("YYBH", yybh);
                if (!string.IsNullOrWhiteSpace(tcmc))
                {
                    sqlAll += " AND b.TCMC LIKE @TCMC";
                    paramList.Add("TCMC", "%" + tcmc + "%");
                }
                if (!string.IsNullOrWhiteSpace(tcbh))
                {
                    sqlAll += " AND a.TCBH = @TCBH";
                    paramList.Add("TCBH", tcbh);
                }
                string sqlPage = "";
                int fromRow = (pageIndex - 1) * pageSize;
                int toRow = (pageIndex) * pageSize;
                paramList.Add("FromRow", fromRow);
                paramList.Add("ToRow", toRow);
                sqlPage = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS rowNum, * FROM (" + sqlAll + ") AS T ) AS N WHERE rowNum >@FromRow AND rowNum <=@ToRow ";

                List<Wjtc> li = conn.Query<Wjtc>(sqlPage, paramList).ToList();
                count = conn.Query<int>("SELECT COUNT(1) FROM (" + sqlAll + ") AS t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        /// 获取套餐选项
        /// </summary>
        /// <returns></returns>
        public List<Wjtc> GetTcSelect(string yybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT tcbh as  TCID, TCMC FROM xt_tcb WHERE SFQY=1 AND  yybh=@YYBH and tclx=2 and dwbh='0000'";
                paramList.Add("YYBH",yybh);
                List<Wjtc> li = conn.Query<Wjtc>(sqlAll, paramList).ToList();
                return li;
            }
        }

        /// <summary>
        /// 获取套餐信息
        /// </summary>
        /// <returns></returns>
        public XttcbModel GetTcxx(string tcid)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT * FROM xt_tcb WHERE SFQY=1 AND tcbh=@TCID and tclx=2 and dwbh='0000'";
                paramList.Add("TCID", tcid);
                XttcbModel model = conn.QueryFirstOrDefault<XttcbModel>(sqlAll, paramList);
                return model;
            }
        }

        /// <summary>
        /// 套餐组合项目
        /// </summary>
        /// <param name="jgid"></param>
        /// <param name="tcbh"></param>
        /// <returns></returns>
        public List<XtZhxmbModel> GetTcZhxm(string yybh, string tcbh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select b.zhxmbh,b.zhxmmc,b.zhxmjg,b.xb from xt_tc_zhxmb a join xt_zhxmb b on a.zhxmbh=b.zhxmbh and a.yybh=b.yybh  where a.yybh=@yybh and a.tcbh = @tcbh";
                List<XtZhxmbModel> li = conn.Query<XtZhxmbModel>(sql, new { yybh = yybh, tcbh = tcbh }).ToList();
                return li;
            }
        }
    }
}
