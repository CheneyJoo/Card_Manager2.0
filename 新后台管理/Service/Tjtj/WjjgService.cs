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
    public class WjjgService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnModel Insert(Wjjg model)
        {
            try
            {
                //验证参数
                if (string.IsNullOrWhiteSpace(model.YYBH))
                {
                    return new ReturnModel { Code = 201, Msg = "请选择医院" };
                }
               
                //保存
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    //判断重名
                    sbSql.Append("SELECT * FROM WJ_JG WHERE YYBH=@YYBH AND WJZT=@WJZT");
                    Wjjg sameName = conn.QueryFirstOrDefault<Wjjg>(sbSql.ToString(), model);
                    if (sameName != null)
                    {
                        return new ReturnModel { Code = 201, Msg = "存在相同名称的医院" };
                    }

                    sbSql.Clear();
                    model.WJZT = 1;
                    sbSql.Append("INSERT INTO WJ_JG(YYBH, WJZT, CJR, CJSJ) VALUES (@YYBH, @WJZT, @CJR, @CJSJ)");
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
        public ReturnModel Update(Wjjg model)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    //判断医院存在
                    sbSql.Append("SELECT * FROM WJ_JG WHERE Id=@Id");
                    Wjjg existModel = conn.QueryFirstOrDefault<Wjjg>(sbSql.ToString(), new { Id = model.ID });
                    if (existModel == null)
                    {
                        return new ReturnModel { Code = 201, Msg = "医院不存在" };
                    }

                    //保存更新
                    sbSql.Clear();
                    sbSql.Append("UPDATE WJ_JG SET WJZT = @WJZT WHERE ID = @ID");
                    int result = conn.Execute(sbSql.ToString(), model);
                    
                    if (result.Equals(1))
                    {
                        return new ReturnModel { Code = 200, Msg = model.WJZT.Equals(0)?"停用成功":"启用成功" };
                    }

                    return new ReturnModel { Code = 201, Msg = model.WJZT.Equals(0) ? "停用失败" : "启用失败" };
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
                    sbSql.Append($"SELECT * FROM WJ_JG WHERE ID={id}");
                    Wjjg model = conn.QueryFirstOrDefault<Wjjg>(sbSql.ToString(),null,transaction);
                    if(model == null) return new ReturnModel { Code = 201, Msg = "医院不存在" };

                    sbSql.Clear();
                    sbSql.Append($"DELETE FROM WJ_TCBQ WHERE GLID IN (SELECT id FROM WJ_TC WHERE TCID IN (SELECT TCID FROM TC_TJ WHERE YYBH='{model.YYBH}'))");
                    conn.Execute(sbSql.ToString(),null, transaction);

                    sbSql.Clear();
                    sbSql.Append($"DELETE FROM WJ_TC WHERE TCID IN (SELECT tcid FROM TC_TJ WHERE yybh='{model.YYBH}')");
                    conn.Execute(sbSql.ToString(), null, transaction);

                    sbSql.Append($"DELETE FROM WJ_JG WHERE ID={id}");
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
        public Wjjg GetEntity(int id)
        {
            string sql = "SELECT * FROM dbo.WJ_JG WHERE ID=@ID";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.QueryFirstOrDefault<Wjjg>(sql, new {ID = id});
            }
        }

        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        public List<Wjjg> GetList(string jgmc, int wjzt, int pageIndex, int pageSize, ref int count)
        {

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT a.*,b.JGMC FROM WJ_JG a LEFT JOIN JG_TJJG b ON a.YYBH=b.TJJGID WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(jgmc))
                {
                    sqlAll += " AND b.JGMC LIKE @JGMC";
                    paramList.Add("JGMC", "%" + jgmc + "%");
                }
                if (wjzt >-1)
                {
                    sqlAll += " AND a.WJZT=@WJZT";
                    paramList.Add("WJZT", wjzt);
                }
                string sqlPage = "";
                int fromRow = (pageIndex - 1) * pageSize;
                int toRow = (pageIndex) * pageSize;
                paramList.Add("FromRow", fromRow);
                paramList.Add("ToRow", toRow);
                sqlPage = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS rowNum, * FROM (" + sqlAll + ") AS T ) AS N WHERE rowNum >@FromRow AND rowNum <=@ToRow ";

                List<Wjjg> li = conn.Query<Wjjg>(sqlPage, paramList).ToList();
                count = conn.Query<int>("SELECT COUNT(1) FROM (" + sqlAll + ") AS t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        /// 获取机构选项
        /// </summary>
        /// <returns></returns>
        public List<Wjjg> GetJgSelect()
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sqlAll = "SELECT TJJGID AS YYBH, JGMC FROM JG_TJJG WHERE SFQY=1";
                List<Wjjg> li = conn.Query<Wjjg>(sqlAll).ToList();
                return li;
            }
        }

    }
}
