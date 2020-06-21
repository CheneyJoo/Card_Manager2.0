using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Dapper;
using Model;

namespace Service
{
     public class JsglbService
    {
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <returns></returns>
        public ReturnModel AddJs(JsModel model)
        {
            try
            {
                //验证参数
                if (string.IsNullOrWhiteSpace(model.Jsmc))
                {
                    return new ReturnModel { Code = 201, Msg = "角色名称不能为空" };
                }
                if (model.Lx == 1||model.Lx==3)
                {
                    if (string.IsNullOrWhiteSpace(model.Yybh))
                    {
                        return new ReturnModel { Code = 201, Msg = "医院编号不能为空" };
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.QdId.ToString()))
                    {
                        return new ReturnModel { Code = 201, Msg = "渠道不能为空" };
                    }
                }
                //保存
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    //判断重名
                    sbSql.Append("SELECT * FROM xt_jsglb WHERE Jsmc=@Jsmc and Lx="+model.Lx.ToString());
                    JsModel sameName = conn.QueryFirstOrDefault<JsModel>(sbSql.ToString(), model);
                    if (sameName != null)
                    {
                        return new ReturnModel { Code = 201, Msg = "存在相同名称的角色" };
                    }

                    sbSql.Clear();
                    sbSql.Append("INSERT INTO xt_jsglb (Jsmc, Bz, Yybh, Cjrq, Cjr, Gxrq, Gxr,Lx,QdId) VALUES (@Jsmc, @Bz, @Yybh, @Cjrq, @Cjr, @Gxrq, @Gxr,@Lx,@QdId)");
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
        /// 修改角色
        /// </summary>
        /// <returns></returns>
        public ReturnModel UpdateJs(JsModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Jsmc))
                {
                    return new ReturnModel { Code = 201, Msg = "角色名称不能为空" };
                }
                if (model.Lx == 1||model.Lx==3)
                {
                    if (string.IsNullOrWhiteSpace(model.Yybh))
                    {
                        return new ReturnModel { Code = 201, Msg = "医院编号不能为空" };
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.QdId.ToString()))
                    {
                        return new ReturnModel { Code = 201, Msg = "渠道不能为空" };
                    }
                }
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
              

                    //判断角色存在
                    sbSql.Append("SELECT * FROM xt_jsglb WHERE Id=@Id");
                    JsModel existModel = conn.QueryFirstOrDefault<JsModel>(sbSql.ToString(), new { Id = model.Id });
                    if (existModel == null)
                    {
                        return new ReturnModel { Code = 201, Msg = "角色不存在" };
                    }

                    //判断重名
                    sbSql.Clear();
                    sbSql.Append("SELECT * FROM xt_jsglb WHERE Id<>@Id AND Jsmc=@Jsmc and lx="+model.Lx.ToString());
                    JsModel sameName = conn.QueryFirstOrDefault<JsModel>(sbSql.ToString(), model);
                    if (sameName != null)
                    {
                        return new ReturnModel { Code = 201, Msg = "存在相同名称的角色" };
                    }

                    //保存更新
                    sbSql.Clear();
                    sbSql.Append("UPDATE xt_jsglb SET Jsmc=@Jsmc, Bz=@Bz, Yybh=@Yybh, Gxrq=@Gxrq, Gxr=@Gxr WHERE Id=@Id");
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
        /// 单独删除
        /// </summary>
        /// <param name="id"></param>
        public ReturnModel Del(int id)
        {
            try
            {
                string sql = "DELETE FROM xt_jsglb WHERE Id=@Id";
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    int rows = conn.Execute(sql, new { Id = id });
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
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        public ReturnModel DelList(string ids)
        {
            try
            {
                var list = ids.Split(',');
                string sql = "DELETE FROM xt_jsglb WHERE Id IN @ids";
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    int rows = conn.Execute(sql, new { ids = list });
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
        /// 获取单个角色
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        public JsModel GetJs(int id)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "SELECT * FROM xt_jsglb WHERE Id=@Id";
                JsModel model = conn.Query<JsModel>(sql, new { Id = id }).FirstOrDefault();
                return model;
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public List<JsModel> JsList(string jsmc, string yybh,int qdId,int lx, int pageIndex, int pageSize, ref int count)
        {
            
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();
                
                string sqlAll = "SELECT * FROM xt_jsglb WHERE yybh=@Yybh and qdid=@qdId and Lx="+lx.ToString();
                if (!string.IsNullOrWhiteSpace(jsmc))
                {
                    sqlAll += " AND jsmc LIKE @Jsmc";
                    paramList.Add("Jsmc", "%" + jsmc + "%");
                }

                string sqlPage = "";
                int fromRow = (pageIndex - 1) * pageSize;
                int toRow = (pageIndex) * pageSize;
                paramList.Add("Yybh", yybh);
                paramList.Add("qdId", qdId);
                paramList.Add("FromRow", fromRow);
                paramList.Add("ToRow", toRow);
                sqlPage = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS rowNum, * FROM ("+ sqlAll +") AS T ) AS N WHERE rowNum >@FromRow AND rowNum <=@ToRow ";

                List<JsModel> li = conn.Query<JsModel>(sqlPage, paramList).ToList();
                count = conn.Query<int>("SELECT COUNT(1) FROM (" + sqlAll + ") AS t ", paramList).FirstOrDefault();
                return li;
            }
        }

        public List<JsModel> JsQdList(string jsmc, string qdId, int pageIndex, int pageSize, ref int count)
        {

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT * FROM xt_jsglb WHERE Qdid=@qdid";
                if (!string.IsNullOrWhiteSpace(jsmc))
                {
                    sqlAll += " AND jsmc LIKE @Jsmc";
                    paramList.Add("Jsmc", "%" + jsmc + "%");
                }

                string sqlPage = "";
                int fromRow = (pageIndex - 1) * pageSize;
                int toRow = (pageIndex) * pageSize;
                paramList.Add("qdid", qdId);
                paramList.Add("FromRow", fromRow);
                paramList.Add("ToRow", toRow);
                sqlPage = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS rowNum, * FROM (" + sqlAll + ") AS T ) AS N WHERE rowNum >@FromRow AND rowNum <=@ToRow ";

                List<JsModel> li = conn.Query<JsModel>(sqlPage, paramList).ToList();
                count = conn.Query<int>("SELECT COUNT(1) FROM (" + sqlAll + ") AS t ", paramList).FirstOrDefault();
                return li;
            }
        }

        public List<JsModel> GetJsList(string yybh ,int lx)
        {

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT * FROM xt_jsglb WHERE yybh=@Yybh and lx="+lx.ToString();
                paramList.Add("Yybh", yybh);
                List<JsModel> li = conn.Query<JsModel>(sqlAll, paramList).ToList();
                return li;
            }
        }
        public List<JsModel> GetQdJsList(string qdId)
        {

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DynamicParameters paramList = new DynamicParameters();

                string sqlAll = "SELECT * FROM xt_jsglb WHERE qdid=@qdId";
                paramList.Add("qdId", qdId);
                List<JsModel> li = conn.Query<JsModel>(sqlAll, paramList).ToList();
                return li;
            }
        }
        /// <summary>
        /// 角色权限
        /// </summary>
        /// <param name="jsid"></param>
        /// <returns></returns>
        public List<JsqxModel> GetJsQx(int jsid)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from xt_jsqxb where jsid=@jsid";
                List<JsqxModel> li= conn.Query<JsqxModel>(sql, new { jsid = jsid }).ToList();
                return li;
            }
        }



    }
}
