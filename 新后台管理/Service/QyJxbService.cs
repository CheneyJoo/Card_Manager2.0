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
    /// 企业加项包
    /// </summary>
    public class QyJxbService
    {
        /// <summary>
        /// 企业加项包列表
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QyJxbJbxxModel> GetQyJxblist(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from qy_jxbjbxx a where 1=1";
                StringBuilder sd = new StringBuilder();

                DynamicParameters paramList = new DynamicParameters();
                if (ht["yybh"] != null)
                {
                    sd.Append(" and a.yybh = @yybh");
                    paramList.Add("yybh", ht["yybh"].ToString());
                }
                if (ht["dwbh"] != null)
                {
                    sd.Append(" and a.qybh = @dwbh");
                    paramList.Add("dwbh", ht["dwbh"].ToString());
                }
                if (ht["jxbmc"] != null)
                {
                    sd.Append(" and a.jxbmc like  @jxbmc");
                    paramList.Add("jxbmc", "%" + ht["jxbmc"].ToString() + "%");
                }
                if (ht["sfqy"] != null)
                {
                    sd.Append(" and a.sfqy=  @sfqy");
                    paramList.Add("sfqy", ht["sfqy"].ToString());
                }

                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<QyJxbJbxxModel> li = conn.Query<QyJxbJbxxModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }
        /// <summary>
        /// 获取一个加项包
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QyJxbJbxxModel GetJxbById(int id)
        {
            QyJxbJbxxModel model = new QyJxbJbxxModel();
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from qy_jxbjbxx a where id=@id";
                model = conn.Query<QyJxbJbxxModel>(sql, new { id = id }).FirstOrDefault();
                string sqltc = "select * from qy_jxbtc where jxbid=@id";
                model.ltc = conn.Query<QyJxbTcModel>(sqltc, new { id = id }).ToList();
                string sqlzhxm = "select * from qy_jxbzhxm where jxbid=@id";
                model.lZhxm = conn.Query<QyJxbZhxmModel>(sqlzhxm, new { id = id }).ToList();

                string zhxmbhs = "";
                string zhxms = "";
                string tcbhs = "";
                string tcmcs = "";
                foreach (QyJxbTcModel item in model.ltc)
                {
                    tcbhs += item.tcbh + ",";
                    tcmcs += item.tcmc + ",";
                }
                model.tcmcs = tcmcs.TrimEnd(',');
                model.tcbhs = tcbhs.TrimEnd(',');
                foreach (QyJxbZhxmModel item in model.lZhxm)
                {
                    zhxmbhs += item.zhxmbh + ",";
                    zhxms += item.zhxmmc + ",";
                }
                model.zhxmbhs = zhxmbhs.TrimEnd(',');
                model.zhxms = zhxms.TrimEnd(',');
                return model;
            }
        }
        /// <summary>
        /// 保存加项包
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveJxb(QyJxbJbxxModel model)
        {
            int i = 0;
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<QyJxbTcModel> litc = new List<QyJxbTcModel>();
                foreach (string a in model.tcbhs.Split(','))
                {
                    XttcbModel item = new XttcbService().GetTc(model.yybh, a);
                    litc.Add(new QyJxbTcModel() { yybh = model.yybh, tcbh = item.tcbh, tcmc = item.tcmc });
                }
                List<QyJxbZhxmModel> lizhxm = new List<QyJxbZhxmModel>();
                foreach (string a in model.zhxmbhs.Split(','))
                {
                    XtZhxmbModel item = new XtzhxmService().GetZhxm(model.yybh, a);
                    lizhxm.Add(new QyJxbZhxmModel() { yybh = model.yybh, jg = item.zhxmjg, zhxmbh = item.zhxmbh, zhxmmc = item.zhxmmc });
                }

                IDbTransaction transaction = conn.BeginTransaction();
                if (model.id == 0)
                {
                    string sql = "insert into qy_jxbjbxx(yybh,qybh,qymc,jxbmc,lrjs,syrq,sfqy,jg,xb,jsj,createtime,updatetime) values(@yybh,@qybh,@qymc,@jxbmc,@lrjs,@syrq,@sfqy,@jg,@xb,@jsj,getdate(),getdate());SELECT SCOPE_IDENTITY()";
                    string id = conn.ExecuteScalar(sql, model, transaction).ToString();
                    model.id = int.Parse(id);


                }
                else
                {
                    string sql = "delete from qy_jxbzhxm where jxbid=@id;delete from qy_jxbtc where jxbid=@id; update qy_jxbjbxx set jxbmc=@jxbmc,lrjs=@lrjs,syrq=@syrq,sfqy=@sfqy,jg=@jg,xb=@xb,jsj=@jsj,updatetime=getdate() where id=@id";
                    conn.Execute(sql, model, transaction).ToString();
                }

                string sqlZhxm = "insert into qy_jxbzhxm(jxbid,yybh,zhxmbh,zhxmmc,jg)values(" + model.id + ",@yybh,@zhxmbh,@zhxmmc,@jg)";
                conn.Execute(sqlZhxm, lizhxm, transaction);
                string sqlTc = "insert into qy_jxbtc(jxbid,yybh,tcbh,tcmc)values(" + model.id + ",@yybh,@tcbh,@tcmc)";
                conn.Execute(sqlTc, litc, transaction);

                transaction.Commit();
                i = 1;
            }
            return model.id;
        }

        public List<QyJxbJbxxModel> GetList(string tcbh)
        {
            string sql = "SELECT b.* FROM dbo.qy_jxbtc a INNER JOIN dbo.qy_jxbjbxx b ON a.jxbid=b.id WHERE a.tcbh=@tcbh AND b.sfqy=1";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var list = conn.Query<QyJxbJbxxModel>(sql, new { tcbh = tcbh }).ToList();
                return list;
            }
        }
        public QyJxbJbxxModel GetModel(string yybh, string tcbh, int id)
        {
            string sql = "SELECT b.* FROM dbo.qy_jxbtc a INNER JOIN dbo.qy_jxbjbxx b ON a.jxbid=b.id WHERE a.yybh=@yybh AND a.tcbh=@tcbh AND b.id=@id AND b.sfqy=1";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var model = conn.QueryFirstOrDefault<QyJxbJbxxModel>(sql, new { yybh = yybh, tcbh = tcbh, id = id });
                return model;
            }
        }

        public List<QyJxbZhxmModel> GetZhxmList(int jxbid)
        {
            string sql = "SELECT * FROM dbo.qy_jxbzhxm  WHERE jxbid=@jxbid";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var list = conn.Query<QyJxbZhxmModel>(sql, new { jxbid = jxbid }).ToList();
                return list;
            }
        }

        //public List<QyJxbZhxmModel> GetZhxmList(string yybh, string tcbh, int jxbid)
        //{
        //    string sql = "SELECT c.* FROM dbo.qy_jxbtc  a INNER JOIN dbo.qy_jxbjbxx b ON a.jxbid=b.id INNER JOIN dbo.qy_jxbzhxm c ON b.id=c.jxbid WHERE c.jxbid=@jxbid AND a.yybh=@yybh AND a.tcbh=@tcbh AND b.sfqy=1";
        //    using (IDbConnection conn = new DapperConnection().DbConnection)
        //    {
        //        var list = conn.Query<QyJxbZhxmModel>(sql, new { yybh = yybh, tcbh = tcbh, jxbid = jxbid }).ToList();
        //        return list;
        //    }
        //}
    }
}
