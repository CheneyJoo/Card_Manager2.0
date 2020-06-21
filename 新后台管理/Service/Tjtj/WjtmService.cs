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
    public class WjtmService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnModel Insert(Wjtm model)
        {
            try
            {
                //验证参数
                if (string.IsNullOrWhiteSpace(model.TMNR))
                {
                    return new ReturnModel { Code = 201, Msg = "题目内容不能为空" };
                }
                if (model.ListTMXX == null || model.ListTMXX.Count<2)
                {
                    return new ReturnModel { Code = 201, Msg = "题目选项少于2个" };
                }
                //保存
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        sbSql.Append("SELECT MAX(PX) FROM WJ_WJTM WHERE WJID=@WJID");
                        int maxPx = Convert.ToInt32(conn.ExecuteScalar(sbSql.ToString(), model, transaction));
                        maxPx += 1;

                        sbSql.Clear();
                        model.PX = maxPx;
                        sbSql.Append("INSERT INTO WJ_WJTM(WJID, TMNR, PX, XGR, XGSJ, TMLX) VALUES (@WJID, @TMNR, @PX, @XGR, @XGSJ, @TMLX);select @@IDENTITY");
                        int tmid = Convert.ToInt32(conn.ExecuteScalar(sbSql.ToString(), model, transaction));
                        foreach (var xx in model.ListTMXX)
                        {
                            sbSql.Clear();
                            sbSql.Append("INSERT INTO WJ_TMXX(TMID, XXNR) VALUES(@TMID, @XXNR);select @@IDENTITY");
                            int xxid = Convert.ToInt32(conn.ExecuteScalar(sbSql.ToString(), new { TMID = tmid, XXNR = xx.XXNR }, transaction));
                            if (xx.ListTMXXBQ == null || xx.ListTMXXBQ.Count <= 0) continue;
                            foreach (var bq in xx.ListTMXXBQ)
                            {
                                sbSql.Clear();
                                sbSql.Append("INSERT INTO WJ_TMXXBQ(XXID, BQID) VALUES(@XXID, @BQID)");
                                conn.Execute(sbSql.ToString(), new { XXID = xxid, BQID = bq.BQID }, transaction);
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
        public ReturnModel Update(Wjtm model)
        {
            try
            {
                //验证参数
                if (string.IsNullOrWhiteSpace(model.TMNR))
                {
                    return new ReturnModel { Code = 201, Msg = "题目内容不能为空" };
                }
                if (model.ListTMXX == null || model.ListTMXX.Count < 2)
                {
                    return new ReturnModel { Code = 201, Msg = "题目选项少于2个" };
                }
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        //判断题目存在
                        sbSql.Append("SELECT * FROM WJ_WJTM WHERE Id=@Id");
                        Wjtm existModel = conn.QueryFirstOrDefault<Wjtm>(sbSql.ToString(), new { Id = model.ID }, transaction);
                        if (existModel == null)
                        {
                            return new ReturnModel { Code = 201, Msg = "题目不存在" };
                        }

                        //保存题目
                        sbSql.Clear();
                        sbSql.Append("UPDATE WJ_WJTM SET TMNR = @TMNR, XGR = @XGR, XGSJ = @XGSJ, TMLX = @TMLX WHERE ID = @ID");
                        conn.Execute(sbSql.ToString(), model, transaction);

                        //保存选项
                        foreach (var xx in model.ListTMXX)
                        {
                            //删除选项标签
                            sbSql.Clear();
                            sbSql.Append("DELETE FROM WJ_TMXXBQ WHERE XXID IN (SELECT ID FROM WJ_TMXX WHERE TMID=@TMID);");
                            conn.Execute(sbSql.ToString(), new { TMID = xx.ID }, transaction);

                            //删除选项
                            sbSql.Clear();
                            sbSql.Append("DELETE FROM WJ_TMXX WHERE TMID=@TMID;");
                            conn.Execute(sbSql.ToString(), new { TMID = model.ID }, transaction);

                            //重新插入选项
                            sbSql.Clear();
                            sbSql.Append("INSERT INTO WJ_TMXX(TMID, XXNR) VALUES(@TMID, @XXNR);select @@IDENTITY");
                            int xxid = Convert.ToInt32(conn.ExecuteScalar(sbSql.ToString(), new { TMID = model.ID, XXNR = xx.XXNR }, transaction));

                            //重新插入标签
                            foreach (var bq in xx.ListTMXXBQ)
                            {
                                sbSql.Clear();
                                sbSql.Append("INSERT INTO WJ_TMXXBQ(XXID, BQID) VALUES(@XXID, @BQID)");
                                conn.Execute(sbSql.ToString(), new { XXID = xxid, BQID = bq.BQID }, transaction);
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
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append("DELETE FROM WJ_WJTM  WHERE ID=@ID;");
                    sbSql.Append("DELETE FROM WJ_TMXXBQ WHERE XXID IN (SELECT ID FROM WJ_TMXX WHERE TMID=@ID);");
                    sbSql.Append("DELETE FROM WJ_TMXX  WHERE TMID=@ID;");
                    conn.Execute(sbSql.ToString(), new { ID = id }, transaction);
                    transaction.Commit();
                    return new ReturnModel { Code = 200, Msg = "删除成功" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.WriteLog(ex.Message);
                    return new ReturnModel { Code = 201, Msg = "删除失败" };
                }
            }
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Wjtm GetEntity(int id)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT * FROM dbo.WJ_WJTM WHERE ID=@ID");
                var model = conn.QueryFirstOrDefault<Wjtm>(sbSql.ToString(), new {ID = id});
                if (model != null)
                {
                    sbSql.Clear();
                    sbSql.Append("SELECT * FROM dbo.WJ_TMXX WHERE TMID=@ID");
                    var listXX = conn.Query<Wjtmxx>(sbSql.ToString(), new { ID = id }).ToList();
                    foreach (var xx in listXX)
                    {
                        sbSql.Clear();
                        sbSql.Append("SELECT a.*, b.BQMC FROM dbo.WJ_TMXXBQ a INNER JOIN dbo.WJ_BQ b ON a.BQID=b.ID WHERE XXID=@XXID");
                        var listBq = conn.Query<Wjtmxxbq>(sbSql.ToString(), new { XXID = xx.ID }).ToList();
                        xx.ListTMXXBQ = listBq;
                    }

                    model.ListTMXX = listXX;
                }

                return model;
            }
        }

        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <returns></returns>
        public List<Wjtm> GetList(int wjid)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Clear();
                sbSql.Append("SELECT * FROM dbo.WJ_WJTM WHERE WJID=@ID");
                var listTm = conn.Query<Wjtm>(sbSql.ToString(), new { ID = wjid }).ToList();
                foreach (var wjtm in listTm)
                {
                    
                    sbSql.Clear();
                    sbSql.Append("SELECT * FROM dbo.WJ_TMXX WHERE TMID=@ID");
                    var listXX = conn.Query<Wjtmxx>(sbSql.ToString(), new { ID = wjtm.ID }).ToList();
                    foreach (var xx in listXX)
                    {
                        sbSql.Clear();
                        sbSql.Append("SELECT a.*,b.BQMC FROM dbo.WJ_TMXXBQ a INNER JOIN WJ_BQ b ON a.BQID=b.ID WHERE a.XXID=@XXID");
                        var listBq = conn.Query<Wjtmxxbq>(sbSql.ToString(), new { XXID = xx.ID }).ToList();
                        xx.ListTMXXBQ = listBq;
                    }

                    wjtm.ListTMXX = listXX;
                }
                return listTm;
            }
        }

        public List<Wjb> GetWjList()
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT * FROM dbo.WJ_WJB");
                return conn.Query<Wjb>(sbSql.ToString()).ToList();
               
            }
        }

    }
}
