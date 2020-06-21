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
    /// 企业基本信息
    /// </summary>
    public class QyjbxxService
    {

        /// <summary>
        /// 企业列表
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QyJbxxModel> GetqyjbxxList(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
            {
                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", ht["yybh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["qymc"].ToString()))
            {
                sd.Append(" and a.mc like @qymc");
                paramList.Add("qymc", ht["qymc"].ToString() + "%");
            }
            if (!string.IsNullOrEmpty(ht["qybh"].ToString()))
            {
                sd.Append(" and a.bh = @bh");
                paramList.Add("bh", ht["qybh"].ToString());
            }
            if (!string.IsNullOrEmpty(ht["sfqy"].ToString()))
            {
                sd.Append(" and a.sfqy=@sfqy");
                paramList.Add("sfqy", ht["sfqy"].ToString());
            }
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.* from qy_jbxx a where 1=1 and isdept=0 and sfqd=0";
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<QyJbxxModel> li = conn.Query<QyJbxxModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }
        /// <summary>
        /// 企业下的部门
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="dwbh"></param>
        /// <returns></returns>
        public List<QyJbxxModel> GetqyjbxxdeptList(string yybh, string dwbh)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();


            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.* from qy_jbxx a where 1=1 and isdept=1 and yybh='" + yybh + "' and bh like '" + dwbh + "%'";
                List<QyJbxxModel> li = conn.Query<QyJbxxModel>(sql, paramList).ToList();

                return li;
            }
        }
        /// <summary>
        /// 得到一行企业信息
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public QyJbxxModel GetqyjbxxOne(string yybh, string dwbh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from qy_jbxx a where yybh='" + yybh + "' and  isdept=0 and bh='" + dwbh + "'";
                QyJbxxModel model = conn.Query<QyJbxxModel>(sql, null).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 企业登陆
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="dwbh"></param>
        /// <param name="zh"></param>
        /// <returns></returns>
        public QyJbxxModel GetqyjbxxDl(string yybh, string dwbh,string zh,string mm)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from qy_jbxx a where yybh=@yybh and  isdept=0 and bh=@dwbh and zh=@zh and mm=@mm";
                QyJbxxModel model = conn.Query<QyJbxxModel>(sql, new { yybh=yybh, dwbh =dwbh,zh=zh,mm=mm}).FirstOrDefault();
                return model;
            }
        }
     

        /// <summary>
        /// 企业列表
        /// </summary>
        /// <param name="jgId"></param>
        /// <returns></returns>
        public List<KeyValueModel> GetqyList(string yybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select bh as [key],mc as [values] from qy_jbxx where yybh='" + yybh + "' and isdept=0 and sfqy=1 and sfqd=0";
                List<KeyValueModel> li = conn.Query<KeyValueModel>(sql, null).ToList();
                return li;

            }
        }

        /// <summary>
        /// 企业部门列表
        /// </summary>
        /// <param name="jgId"></param>
        /// <returns></returns>
        public List<KeyValueModel> GetqybmList(string yybh,string qybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select bh as [key],mc as [values] from qy_jbxx where yybh='" + yybh + "' and bh like '"+qybh+"%' ";
                List<KeyValueModel> li = conn.Query<KeyValueModel>(sql, null).ToList();
                return li;

            }
        }

        /// <summary>
        /// 渠道企业列表
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<KeyValueModel> GetqyqdList(string yybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = " select bh as [key],mc as [values] from qy_jbxx where sfqd=1 and sfqy=1  and yybh='" + yybh + "' ";
                List<KeyValueModel> li = conn.Query<KeyValueModel>(sql, null).ToList();
                return li;

            }
        }
     

        /// <summary>
        /// 企业开关
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="bh"></param>
        /// <param name="sfqy"></param>
        /// <returns></returns>
        public int UpdateSfqy(string yybh, string bh, int sfqy)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sqlcz = "update  qy_jbxx set sfqy=@sfqy,updatetime=getdate() where yybh=@yybh and bh=@bh";
                return conn.Execute(sqlcz, new { yybh = yybh, bh = bh, sfqy = sfqy });
            }
        }
        /// <summary>
        /// 渠道开关
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="bh"></param>
        /// <param name="sfqy"></param>
        /// <returns></returns>
        public int UpdateSfqd(string yybh, string bh, int sfqd)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sqlcz = "update  qy_jbxx set sfqd=@sfqd,updatetime=getdate() where yybh=@yybh and bh=@bh";
                return conn.Execute(sqlcz, new { yybh = yybh, bh = bh, sfqd = sfqd });
            }
        }
        /// <summary>
        /// 同步保存
        /// </summary>
        /// <param name="li"></param>
        public void InsertOrUpdate(List<QyJbxxModel> li)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sqlold = "select id,bh,mc from qy_jbxx where yybh='" + li[0].yybh + "'";
                List<QyJbxxModel> liOld = conn.Query<QyJbxxModel>(sqlold, null).ToList();

                string sqlcz = "";
                IDbTransaction transaction = conn.BeginTransaction();
                List<QyJbxxModel> liUpdate = new List<QyJbxxModel>();
                List<QyJbxxModel> liInsert = new List<QyJbxxModel>();

                foreach (QyJbxxModel model in li)
                {

                    int i = liOld.Where(p => p.bh == model.bh).Count();

                    if (i > 0)
                    {
                        model.id = liOld.Where(p => p.bh == model.bh).FirstOrDefault().id;
                        liUpdate.Add(model);
                    }
                    else
                    {
                        liInsert.Add(model);
                    }

                }
                if (liUpdate.Count > 0)
                {
                    sqlcz = "update  qy_jbxx set isdept=@isdept,mc=@mc,lxdh=@lxdh,@lxdz=@lxdz,updatetime=getdate() where id=@id";
                    conn.Execute(sqlcz, liUpdate, transaction);
                }
                if (liInsert.Count > 0)
                {
                    sqlcz = "insert into  qy_jbxx(sfqd,sfqy,isdept,yybh,bh,mc,dwfzr,lxdh,lxdz,createtime,updatetime) values(0,0,@isdept,@yybh,@bh,@mc,@dwfzr,@lxdh,@lxdz,getdate(),getdate());";
                    conn.Execute(sqlcz, liInsert, transaction);
                }
                transaction.Commit();
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bh"></param>
        /// <returns></returns>
        public QyJbxxModel GetModel(string bh,string yybh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from qy_jbxx a where bh=@bh and yybh=@yybh";

                return conn.Query<QyJbxxModel>(sql, new { bh = bh,yybh=yybh }).FirstOrDefault();

            }
        }
        public int UpdateQy(string lxr, string lxdh, int id,int sfxstcje,string zh,string mm)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sqlcz = "update  qy_jbxx set lxdh=@lxdh,dwfzr=@lxr,sfxstcje=@sfxstcje,zh=@zh,mm=@mm where id=@id";
                return conn.Execute(sqlcz, new { lxdh = lxdh, lxr = lxr, id = id, sfxstcje= sfxstcje,zh=zh,mm=mm });
            }
        }
        /// <summary>
        /// 企业开关
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="bh"></param>
        /// <param name="sfqy"></param>
        /// <returns></returns>
        public int UpdateSfqy(int id, int sfqy)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sqlcz = "update  qy_jbxx set sfqy=@sfqy,updatetime=getdate() where id=@id";
                return conn.Execute(sqlcz, new { id = id, sfqy = sfqy });
            }
        }
        /// <summary>
        /// 企业排期，企业列表
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<KeyValueModel> GetqypaList(string yybh, string days)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select bh as [key],mc as [values] from qy_jbxx where yybh='" + yybh + "' and sfqd=0 and isdept=0 and sfqy=1 and sfqd=0 and bh in(select qybh from pq_qysz where yybh='" + yybh + "' and ksrq<='" + days + "' and jsrq>='" + days + "') ";
                List<KeyValueModel> li = conn.Query<KeyValueModel>(sql, null).ToList();
                return li;

            }
        }


    }
}
