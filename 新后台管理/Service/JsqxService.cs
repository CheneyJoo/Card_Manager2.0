using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Dapper;
using Model;

namespace Service
{
     public class JsqxService
    {
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="jsId"></param>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<JsqxModel> Get(int jsId, string yybh)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT * FROM xt_jsqxb WHERE jsid=@JsId AND yybh=@Yybh");
                DynamicParameters dy = new DynamicParameters();
                dy.Add("JsId",jsId);
                dy.Add("Yybh",yybh);
                using (IDbConnection con = new DapperConnection().DbConnection)
                {
                    List<JsqxModel> list = con.Query<JsqxModel>(sbSql.ToString(), dy).ToList();
                    return list;
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <returns></returns>
        public ReturnModel Save(int jsId, string yybh, List<string> lstMenuId)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                DynamicParameters paramList;
                List<KeyValuePair<string, object>> lstKeyValuePair = new List<KeyValuePair<string, object>>();
                List<JsqxModel> lstModels = new List<JsqxModel>();
                int result = 0;

                //判断角色存在
                JsModel jsModel = new JsglbService().GetJs(jsId);
                if(jsModel==null) return new ReturnModel {Code = 201, Msg = "角色不存在"};

                //遍历权限列表
                foreach (string menu in lstMenuId)
                {
                    lstModels.Add(new JsqxModel {JsId = jsId, MenuId = Convert.ToInt32(menu), Yybh = yybh});
                }

                //删除原有权限
                sbSql.Append("DELETE FROM xt_jsqxb WHERE jsid=@JsId AND yybh=@Yybh");
                paramList = new DynamicParameters();
                paramList.Add("JsId", jsId);
                paramList.Add("Yybh", yybh);
                lstKeyValuePair.Add(new KeyValuePair<string, object>(sbSql.ToString(), paramList));

                //重新插入权限列表
                foreach (var model in lstModels)
                {
                    sbSql.Clear();
                    paramList = new DynamicParameters();
                    paramList.Add("JsId", model.JsId);
                    paramList.Add("MenuId", model.MenuId);
                    paramList.Add("Yybh", model.Yybh);

                    sbSql.Append("INSERT INTO xt_jsqxb (menuid, jsid, yybh) VALUES (@MenuId, @JsId, @Yybh)");
                    lstKeyValuePair.Add(new KeyValuePair<string, object>(sbSql.ToString(), paramList));
                }

                result = DapperConnection.ExecuteTransaction(lstKeyValuePair);
                if (result < 0) return new ReturnModel {Code = 201, Msg = "保存失败"};
                return new ReturnModel { Code = 200, Msg = "保存成功" };

            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return new ReturnModel { Code = 201, Msg = "保存失败" };
            }
        }

        public List<XtMenuModel> GetAllMenu(int lx)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT * FROM xt_menu where lx=@lx ORDER BY sort");

                using (IDbConnection con = new DapperConnection().DbConnection)
                {
                    List<XtMenuModel> list = con.Query<XtMenuModel>(sbSql.ToString(), new { lx = lx }).ToList();
                    return list;
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return null;
            }
        }


        public List<XtMenuModel> GetMenu(int jsid)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT a.* FROM dbo.xt_menu a INNER JOIN dbo.xt_jsqxb b ON a.id=b.menuid WHERE b.jsid=@jsid  ORDER BY a.sort");

                using (IDbConnection con = new DapperConnection().DbConnection)
                {
                    List<XtMenuModel> list = con.Query<XtMenuModel>(sbSql.ToString(), new { jsid = jsid }).ToList();
                    return list;
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
