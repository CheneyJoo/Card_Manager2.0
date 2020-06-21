using Dapper;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 企业员工信息
    /// </summary>
    public class QyygjbxxService
    {
        /// <summary>
        /// 企业员工列表
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QyygxxModel> GetQyygjbxxList(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from qy_ygxx a where 1=1";
                StringBuilder sd = new StringBuilder();

                DynamicParameters paramList = new DynamicParameters();
                if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
                {
                    sd.Append(" and a.yybh = @yybh");
                    paramList.Add("yybh", ht["yybh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["dwbh"].ToString()))
                {
                    sd.Append(" and a.dwbh like @dwbh");
                    paramList.Add("dwbh", ht["dwbh"].ToString() + "%");
                }
                if (!string.IsNullOrEmpty(ht["xm"].ToString()))
                {
                    sd.Append(" and a.xm like  @xm");
                    paramList.Add("xm", "%" + ht["xm"].ToString() + "%");
                }
                if (!string.IsNullOrEmpty(ht["dh"].ToString()))
                {
                    sd.Append(" and a.dh=  @dh");
                    paramList.Add("dh", ht["dh"].ToString());
                }

                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<QyygxxModel> li = conn.Query<QyygxxModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        /// 同步企业员工
        /// </summary>
        /// <param name="li"></param>
        public void InsertOrUpdate(List<QyygxxModel> li, string dwbh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select yybh,dwbh,ydjh from qy_ygxx a where yybh='" + li[0].yybh + "' and left(dwbh,4)='" + dwbh + "'";
                List<QyygxxModel> liOld = conn.Query<QyygxxModel>(sql, null).ToList();
                List<QyygxxModel> liUpdate = new List<QyygxxModel>();
                List<QyygxxModel> liInsert = new List<QyygxxModel>();
                string sqlcz = "";
                IDbTransaction transaction = conn.BeginTransaction();
                foreach (QyygxxModel model in li)
                {
                    int i = liOld.Where(p => p.yybh == model.yybh && p.dwbh == model.dwbh && p.ydjh == model.ydjh.Trim()).Count();
                    if (i > 0)
                    {
                        liUpdate.Add(model);
                    }
                    else
                    {
                        liInsert.Add(model);
                    }
                }
                if (liUpdate.Count > 0)
                {
                    sqlcz = "update qy_ygxx set rybh=@rybh, mm=@mm,tcbh=@tcbh,tcmc=@tcmc,dwbh=@dwbh,dwmc=@dwmc,xb=@xb,hz=@hz,jg=@jg,xm=@xm,dh=@dh,csrq=@csrq,dwgdje=@dwgdje,sfzh=@sfzh,createtime=getdate() where yybh=@yybh and dwbh=@dwbh and ydjh=@ydjh;";

                    conn.Execute(sqlcz, liUpdate, transaction);
                }
                if (liInsert.Count > 0)
                {
                    sqlcz = "insert into  qy_ygxx(rybh,yybh,dwbh,dwmc,tcbh,tcmc,jg,sfzh,xm,dh,xb,hz,dwgdje,csrq,sfyy,mm,ydjh,createtime) values(@rybh,@yybh,@dwbh,@dwmc,@tcbh,@tcmc,@jg,@sfzh,@xm,@dh,@xb,@hz,@dwgdje,@csrq,0,@mm,@ydjh,getdate());";
                    conn.Execute(sqlcz, liInsert, transaction);
                }
                transaction.Commit();
            }
        }

        public void InsertOrUpdate(List<QyygxxModel> li)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<QyygxxModel> liUpdate = new List<QyygxxModel>();
                List<QyygxxModel> liInsert = new List<QyygxxModel>();
                foreach (var item in li)
                {
                    if (item.sfzh.Length >= 6)
                    {
                        item.mm = item.sfzh.Substring(item.sfzh.Length - 6, 6);
                    }
                    else
                    {
                        item.mm = "000666";
                    }
                    string sql = "select yybh,dwbh,ydjh from qy_ygxx a where yybh='" +item.yybh + "' and dwbh='" + item.dwbh + "' and ydjh='"+item.ydjh+"' ";
                    List<QyygxxModel> liOld = conn.Query<QyygxxModel>(sql, null).ToList();
                    if(liOld.Count>0)
                    {
                        liUpdate.Add(item);
                    }
                    else
                    {
                        liInsert.Add(item);
                    }
                }
                IDbTransaction transaction = conn.BeginTransaction();
                string sqlcz = "";
                if (liUpdate.Count > 0)
                {
                    sqlcz = "update qy_ygxx set rybh=@rybh, mm=@mm,tcbh=@tcbh,tcmc=@tcmc,dwbh=@dwbh,dwmc=@dwmc,xb=@xb,hz=@hz,jg=@jg,xm=@xm,dh=@dh,csrq=@csrq,dwgdje=@dwgdje,sfzh=@sfzh,createtime=getdate() where yybh=@yybh and dwbh=@dwbh and ydjh=@ydjh;";

                    conn.Execute(sqlcz, liUpdate, transaction);
                }
                if (liInsert.Count > 0)
                {
                    sqlcz = "insert into  qy_ygxx(rybh,yybh,dwbh,dwmc,tcbh,tcmc,jg,sfzh,xm,dh,xb,hz,dwgdje,csrq,sfyy,mm,ydjh,createtime) values(@rybh,@yybh,@dwbh,@dwmc,@tcbh,@tcmc,@jg,@sfzh,@xm,@dh,@xb,@hz,@dwgdje,@csrq,0,@mm,@ydjh,getdate());";
                    conn.Execute(sqlcz, liInsert, transaction);
                }
                transaction.Commit();
            }
        }

        /// <summary>
        /// 得到一行员工信息
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="ydjh"></param>
        /// <returns></returns>
        public QyygxxModel GetModel(string yybh, string ydjh)
        {
            string sql = "SELECT * FROM dbo.qy_ygxx WHERE yybh = @yybh AND ydjh = @ydjh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                QyygxxModel obj = conn.QueryFirstOrDefault<QyygxxModel>(sql, new { yybh = yybh, ydjh = ydjh });
                return obj;
            }
        }
        /// <summary>
        /// 得到自己一批员工信息
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="ydjhs"></param>
        /// <returns></returns>
        public List<QyygxxModel> GetModelList(string yybh, string[] ydjhs)
        {
            string sql = "SELECT * FROM dbo.qy_ygxx WHERE yybh = @yybh AND ydjh in @ydjh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<QyygxxModel> obj = conn.Query<QyygxxModel>(sql, new { yybh = yybh, ydjh = ydjhs }).ToList();
                return obj;
            }
        }
        /// <summary>
        /// 企业员工登录
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QyygxxModel> GetQyygjbxxDlList(Hashtable ht)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from qy_ygxx a where 1=1";
                StringBuilder sd = new StringBuilder();

                DynamicParameters paramList = new DynamicParameters();

                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", ht["yybh"].ToString());


                sd.Append(" and a.xm =  @xm");
                paramList.Add("xm", ht["xm"].ToString());

                sd.Append(" and a.dh=  @dh");
                paramList.Add("dh", ht["dh"].ToString());


                sql += sd.ToString();
                List<QyygxxModel> li = conn.Query<QyygxxModel>(sql, paramList).ToList();
                return li;
            }
        }
        /// <summary>
        /// 北大特诊
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<QyygxxModel> GetQyygjbxxDlListTz(Hashtable ht)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from qy_ygxx a where 1=1";
                StringBuilder sd = new StringBuilder();

                DynamicParameters paramList = new DynamicParameters();

                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", ht["yybh"].ToString());


                sd.Append(" and a.xm =  @xm");
                paramList.Add("xm", ht["xm"].ToString());

                sd.Append(" and a.rybh=  @rybh");
                paramList.Add("rybh", ht["rybh"].ToString());


                sql += sd.ToString();
                List<QyygxxModel> li = conn.Query<QyygxxModel>(sql, paramList).ToList();
                return li;
            }
        }

        public List<QyygxxModel> GetList(string ygzh)
        {
            string sql = "SELECT * FROM dbo.qy_ygxx WHERE  ygzh = @ygzh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var list = conn.Query<QyygxxModel>(sql, new { ygzh = ygzh }).ToList();
                return list;
            }
        }

        public void UpdateYgzh(List<QyygxxModel> list)
        {
            string sql = "UPDATE dbo.qy_ygxx SET ygzh=@ygzh WHERE id=@id AND ygzh=''";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(sql, list);
            }
        }

        public void UpdateSfyy(string yybh, string ydjh)
        {
            string sql = "UPDATE dbo.qy_ygxx SET sfyy=1 WHERE yybh=@yybh AND ydjh=@ydjh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(sql, new { yybh = yybh, ydjh = ydjh });
            }
        }

        /// <summary>
        /// 更新员工密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mm"></param>
        /// <returns></returns>
        public int UpdateQyygMm(int id, string mm)
        {
            string sql = "UPDATE dbo.qy_ygxx SET mm=@mm  WHERE id=@id";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Execute(sql, new { id = id, mm = mm });
            }
        }
        /// <summary>
        /// 更新vip
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sfvip"></param>
        /// <returns></returns>
        public int UpdateQyygVip(int id,int sfvip)
        {
            string sql = "UPDATE dbo.qy_ygxx SET sfvip=@sfvip  WHERE id=@id";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Execute(sql, new { id = id, sfvip = sfvip });
            }
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteQyyg(int id)
        {
            QyygxxModel model = new QyygxxModel();
            string sqlGet = "SELECT * FROM dbo.qy_ygxx WHERE  id=@id";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                model = conn.QueryFirstOrDefault<QyygxxModel>(sqlGet, new { id = id });

            }

            string sql = "delete from xt_yhb where zh='" + model.ygzh + "'; delete from  dd_zhxm where ddbh in (select ddbh from dd_jbxx_new where  yybh ='" + model.yybh + "' and dsfdd='" + model.ydjh + "' and ddly=1 ); delete from dd_jbxx_new where  yybh ='" + model.yybh + "' and dsfdd='" + model.ydjh + "' and ddly=1;delete from  qy_ygxx where id=" + id;
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                return conn.Execute(sql, null);
            }
        }

        /// <summary>
        /// 企业员工，预约详情
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QyygxxModel> GetQyygYyxqList(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.*,b.yykssj,b.yyjssj from qy_ygxx a left join dd_jbxx_new b on a.yybh=b.yybh and a.ydjh=b.dsfdd where 1=1";
                StringBuilder sd = new StringBuilder();
                DynamicParameters paramList = new DynamicParameters();
                if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
                {
                    sd.Append(" and a.yybh = @yybh");
                    paramList.Add("yybh", ht["yybh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["sfyy"].ToString()))
                {
                    sd.Append(" and a.sfyy=@sfyy");
                    paramList.Add("sfyy", ht["sfyy"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["dwbh"].ToString()))
                {
                    sd.Append(" and left(a.dwbh,4)=@dwbh");
                    paramList.Add("dwbh", ht["dwbh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["dept"].ToString()))
                {
                    sd.Append(" and a.dwbh=@dept");
                    paramList.Add("dept", ht["dept"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["xm"].ToString()))
                {
                    sd.Append(" and a.xm like  @xm");
                    paramList.Add("xm", "%" + ht["xm"].ToString() + "%");
                }
                if (!string.IsNullOrEmpty(ht["dh"].ToString()))
                {
                    sd.Append(" and a.dh=  @dh");
                    paramList.Add("dh", ht["dh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["sfzh"].ToString()))
                {
                    sd.Append(" and a.sfzh=@sfzh");
                    paramList.Add("sfzh", ht["sfzh"].ToString());
                }

                if (!string.IsNullOrEmpty(ht["yykssj"].ToString()))
                {
                    sd.Append(" and b.yykssj>=  @yykssj");
                    paramList.Add("yykssj", ht["yykssj"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["yyjssj"].ToString()))
                {
                    sd.Append(" and b.yykssj<=  @yyjssj");
                    paramList.Add("yyjssj", ht["yyjssj"].ToString());
                }

                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<QyygxxModel> li = conn.Query<QyygxxModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        ///  企业员工，预约详情,导出
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetQyygYyxqListExcel(Hashtable ht)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.ydjh as 医院登记号,a.xm as 性名,a.dwmc as 部门,a.tcmc as 套餐名称,case a.xb when 1 then '男' else '女' end as 性别,a.sfzh as 身份证号, a.dh as 联系电话,case a.sfyy when 1 then '已预约' else '未预约' end as 预约状态, b.yykssj as 预约日期 from qy_ygxx a left join dd_jbxx_new b on a.yybh=b.yybh and a.ydjh=b.dsfdd where 1=1";
                StringBuilder sd = new StringBuilder();
                DynamicParameters paramList = new DynamicParameters();
                if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
                {
                    sd.Append(" and a.yybh = @yybh");
                    paramList.Add("yybh", ht["yybh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["sfyy"].ToString()))
                {
                    sd.Append(" and a.sfyy=@sfyy");
                    paramList.Add("sfyy", ht["sfyy"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["dwbh"].ToString()))
                {
                    sd.Append(" and left(a.dwbh,4)=@dwbh");
                    paramList.Add("dwbh", ht["dwbh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["dept"].ToString()))
                {
                    sd.Append(" and a.dwbh=@dept");
                    paramList.Add("dept", ht["dept"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["xm"].ToString()))
                {
                    sd.Append(" and a.xm like  @xm");
                    paramList.Add("xm", "%" + ht["xm"].ToString() + "%");
                }
                if (!string.IsNullOrEmpty(ht["dh"].ToString()))
                {
                    sd.Append(" and a.dh=  @dh");
                    paramList.Add("dh", ht["dh"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["sfzh"].ToString()))
                {
                    sd.Append(" and a.sfzh=@sfzh");
                    paramList.Add("sfzh", ht["sfzh"].ToString());
                }

                if (!string.IsNullOrEmpty(ht["yykssj"].ToString()))
                {
                    sd.Append(" and b.yykssj>=  @yykssj");
                    paramList.Add("yykssj", ht["yykssj"].ToString());
                }
                if (!string.IsNullOrEmpty(ht["yyjssj"].ToString()))
                {
                    sd.Append(" and b.yykssj<=  @yyjssj");
                    paramList.Add("yyjssj", ht["yyjssj"].ToString());
                }

                sql += sd.ToString();

                DataTable dt = new DataTable();
                var reader = conn.ExecuteReader(sql, paramList);
                dt.Load(reader);
                return dt;


            }
        }
    }
}
