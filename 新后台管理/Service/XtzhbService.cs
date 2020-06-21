using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Service
{
    public class XtzhbService
    {
        /// <summary>
        /// 登陆,1医院，2渠道,3医院团险，4，团险企业
        /// </summary>
        /// <returns></returns>
        public XtZhbModel Login(string zh,string mm,int lx)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from xt_zhb where  lx=@lx and mm=@mm and zh=@zh and zt=1";
                XtZhbModel model = conn.Query<XtZhbModel>(sql, new { zh=zh,mm=mm,lx=lx}).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 登陆,移动端
        /// </summary>
        /// <returns></returns>
        public XtZhbModel Login(string sjhm)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from xt_zhb where  lx=@lx and dh=@dh and zt=1";
                XtZhbModel model = conn.Query<XtZhbModel>(sql, new { dh = sjhm, lx = 1 }).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public int UpdatePassword(int id,string pwd)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "update xt_zhb set Mm=@mm where id=@id";
                return conn.Execute(sql, new { id = id, mm = pwd });
            }
        }
        public ReturnModel Add(XtZhbModel model)
        {
            try
            {
                //验证参数
                if (string.IsNullOrWhiteSpace(model.zh)) return new ReturnModel { Code = 201, Msg = "账号不能为空" };
                if (string.IsNullOrWhiteSpace(model.mm)) return new ReturnModel { Code = 201, Msg = "医院编号不能为空" };
                if (string.IsNullOrWhiteSpace(model.dh)) return new ReturnModel { Code = 201, Msg = "手机号不能为空" };
                if (model.jsid.Equals(0)) return new ReturnModel { Code = 201, Msg = "请选择角色" };
                if (string.IsNullOrWhiteSpace(model.lxr)) return new ReturnModel { Code = 201, Msg = "姓名不能为空" };
                StringBuilder sbSql = new StringBuilder();
                DynamicParameters paramList = new DynamicParameters();
                paramList.Add("Zh", model.zh);
                paramList.Add("Mm", model.mm);
                paramList.Add("CreateTime", model.createtime);
                paramList.Add("Dh", model.dh);
                paramList.Add("Lxr", model.lxr);
                paramList.Add("Ys", model.ys);
                paramList.Add("Yybh", model.yybh);
                paramList.Add("Jsid", model.jsid);
                paramList.Add("Zt", model.zt);
                paramList.Add("Bz", model.bz);
                paramList.Add("QdId", model.QdId);
                paramList.Add("Lx", model.Lx);
                paramList.Add("Txlj", model.txlj);
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    //判断重名
                    sbSql.Clear();
                    sbSql.Append("SELECT * FROM xt_zhb WHERE zh=@Zh");
                    JsModel sameName = conn.QueryFirstOrDefault<JsModel>(sbSql.ToString(), paramList);
                    if (sameName != null)
                    {
                        return new ReturnModel { Code = 201, Msg = "存在相同的账号名" };
                    }

                    sbSql.Clear();
                    sbSql.Append("INSERT INTO xt_zhb (zh, mm, createtime, dh, lxr, ys, yybh, jsid, zt, bz,Lx,QdId,txlj) VALUES (@Zh, @Mm, @CreateTime, @Dh, @Lxr, @Ys, @Yybh, @Jsid, @Zt, @Bz,@Lx,@QdId,@Txlj)");
                    int result = conn.Execute(sbSql.ToString(), paramList);
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
        /// 修改账号
        /// </summary>
        /// <returns></returns>
        public ReturnModel Update(XtZhbModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.zh)) return new ReturnModel { Code = 201, Msg = "账号不能为空" };
                if (string.IsNullOrWhiteSpace(model.mm)) return new ReturnModel { Code = 201, Msg = "医院编号不能为空" };
                if (model.jsid.Equals(0)) return new ReturnModel { Code = 201, Msg = "请选择角色" };
                if (string.IsNullOrWhiteSpace(model.lxr)) return new ReturnModel { Code = 201, Msg = "姓名不能为空" };
                StringBuilder sbSql = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    DynamicParameters paramList = new DynamicParameters();
                    paramList.Add("Id", model.id);
                    paramList.Add("Zh", model.zh);
                    paramList.Add("Mm", model.mm);
                    paramList.Add("Dh", model.dh);
                    paramList.Add("Lxr", model.lxr);
                    paramList.Add("Ys", model.ys);
                    paramList.Add("Yybh", model.yybh);
                    paramList.Add("Jsid", model.jsid);
                    paramList.Add("Zt", model.zt);
                    paramList.Add("Bz", model.bz);
                    //判断账号存在
                    sbSql.Append("SELECT * FROM xt_zhb WHERE Id=@Id");
                    XtZhbModel existModel = conn.QueryFirstOrDefault<XtZhbModel>(sbSql.ToString(), paramList);
                    if (existModel == null)
                    {
                        return new ReturnModel { Code = 201, Msg = "账号不存在" };
                    }

                    //判断重名
                    sbSql.Clear();
                    sbSql.Append("SELECT * FROM xt_zhb WHERE id<>@Id AND zh=@Zh");
                    JsModel sameName = conn.QueryFirstOrDefault<JsModel>(sbSql.ToString(), paramList);
                    if (sameName != null)
                    {
                        return new ReturnModel { Code = 201, Msg = "存在相同的账号名" };
                    }

                    //保存更新
                    sbSql.Clear();
                    sbSql.Append("UPDATE xt_zhb SET zh=@Zh, mm=@Mm, dh=@Dh, lxr=@Lxr, ys=@Ys, jsid=@Jsid, zt=@Zt, bz=@Bz WHERE Id=@Id");
                    int result = conn.Execute(sbSql.ToString(), paramList);
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
        public ReturnModel Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM xt_zhb WHERE Id=@Id";
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
        public ReturnModel BatchDelete(string ids)
        {
            try
            {
                var list = ids.Split(',');
                string sql = "DELETE FROM xt_zhb WHERE Id IN @ids";
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
        /// 更改状态
        /// </summary>
        /// <param name="id"></param>
        public ReturnModel UpdateStatus(int id, int zt)
        {
            try
            {

                string sql = $"UPDATE xt_zhb SET zt={zt} WHERE Id=@Id";
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    int rows = conn.Execute(sql, new { Id = id });
                    if (rows > 0)
                    {
                        string result = zt == 1 ? "已启用" : "已停用";
                        return new ReturnModel { Code = 200, Msg = result };
                    }
                    else
                    {
                        return new ReturnModel { Code = 201, Msg = "操作失败" };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return new ReturnModel { Code = 201, Msg = "操作失败" };
            }
        }

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id"></param>
        public ReturnModel ResetPassword(int id,string mm)
        {
            try
            {
                string sql = $"UPDATE xt_zhb SET mm=@mm WHERE Id=@Id";
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    int rows = conn.Execute(sql, new { Id = id,mm=mm });
                    if (rows > 0)
                    {
                        return new ReturnModel { Code = 200, Msg = "操作成功" };
                    }
                    else
                    {
                        return new ReturnModel { Code = 201, Msg = "操作失败" };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return new ReturnModel { Code = 201, Msg = "操作失败" };
            }
        }

        /// <summary>
        /// 获取单个账号
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        public XtZhbModel GetModel(int id)
        {
            try
            {
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    string sql = "SELECT * FROM xt_zhb WHERE Id=@Id";
                    XtZhbModel model = conn.Query<XtZhbModel>(sql, new { Id = id }).FirstOrDefault();
                    return model;
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return null;
            }

        }

        /// <summary>
        /// 获取单个账号
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        public XtZhbModel GetModelWithJs(int id)
        {
            try
            {
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    string sql = "SELECT a.*,b.JSMC FROM xt_zhb a join xt_jsglb b on a.jsid=b.id WHERE a.Id=@Id";
                    XtZhbModel model = conn.Query<XtZhbModel>(sql, new { Id = id }).FirstOrDefault();
                    return model;
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return null;
            }
          
        }

        /// <summary>
        /// 账号列表
        /// </summary>
        /// <param name="zh">账号</param>
        /// <param name="cjsj">创建时间</param>
        /// <param name="yybh">医院编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<XtZhbModel> GetList(string zh, string cjsj, string yybh , int pageIndex, int pageSize, ref int count)
        {
            try
            {
                StringBuilder sd = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    DynamicParameters paramList = new DynamicParameters();

                    string sqlAll = "SELECT a.Id, a.zh, a.mm, a.createtime, a.dh, a.lxr, b.jsmc, a.zt FROM xt_zhb a LEFT JOIN xt_jsglb b ON a.Jsid=b.Id WHERE a.yybh=@Yybh";
                    if (!string.IsNullOrEmpty(zh))
                    {
                        sqlAll += " and a.zh like @Zh";
                    }
                    if (!string.IsNullOrEmpty(cjsj))
                    {
                        sqlAll += " and convert(varchar(10),a.createtime,120)=@cjsj";
                    }
                    sqlAll += sd.ToString();

                    string sqlPage = "";
                    int fromRow = (pageIndex - 1) * pageSize;
                    int toRow = (pageIndex) * pageSize;
                    paramList.Add("Yybh", yybh);
                    paramList.Add("Zh", zh);
                    paramList.Add("Cjsj", cjsj);
                    paramList.Add("FromRow", fromRow);
                    paramList.Add("ToRow", toRow);
                    sqlPage = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS rowNum, * FROM (" + sqlAll + ") AS T ) AS N WHERE rowNum >@FromRow AND rowNum <=@ToRow ";

                    List<XtZhbModel> li = conn.Query<XtZhbModel>(sqlPage, paramList).ToList();
                    count = conn.Query<int>("SELECT COUNT(1) FROM (" + sqlAll + ") AS t ", paramList).FirstOrDefault();
                    return li;
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return null;
            }
            
        }

        public List<XtZhbModel> GetQdList(string zh, string cjsj, string qdId, int pageIndex, int pageSize, ref int count)
        {
            try
            {
                StringBuilder sd = new StringBuilder();
                using (IDbConnection conn = new DapperConnection().DbConnection)
                {
                    DynamicParameters paramList = new DynamicParameters();

                    string sqlAll = "SELECT a.Id, a.zh, a.mm, a.createtime, a.dh, a.lxr, b.jsmc, a.zt FROM xt_zhb a LEFT JOIN xt_jsglb b ON a.Jsid=b.Id WHERE a.qdId=@qdid";
                    if (!string.IsNullOrEmpty(zh))
                    {
                        sqlAll += " and a.zh like @Zh";
                    }
                    if (!string.IsNullOrEmpty(cjsj))
                    {
                        sqlAll += " and convert(varchar(10),a.createtime,120)=@cjsj";
                    }
                    sqlAll += sd.ToString();

                    string sqlPage = "";
                    int fromRow = (pageIndex - 1) * pageSize;
                    int toRow = (pageIndex) * pageSize;
                    paramList.Add("qdid", qdId);
                    paramList.Add("Zh", zh);
                    paramList.Add("Cjsj", cjsj);
                    paramList.Add("FromRow", fromRow);
                    paramList.Add("ToRow", toRow);
                    sqlPage = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS rowNum, * FROM (" + sqlAll + ") AS T ) AS N WHERE rowNum >@FromRow AND rowNum <=@ToRow ";

                    List<XtZhbModel> li = conn.Query<XtZhbModel>(sqlPage, paramList).ToList();
                    count = conn.Query<int>("SELECT COUNT(1) FROM (" + sqlAll + ") AS t ", paramList).FirstOrDefault();
                    return li;
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return null;
            }

        }
    }
}
