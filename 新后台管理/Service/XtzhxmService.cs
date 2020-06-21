using Dapper;
using Model;
using Model.Dto;
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
    /// 组合项目
    /// </summary>
    public class XtzhxmService
    {  
        /// <summary>
        /// 医院端组合项目列表
        /// </summary>
        /// <returns></returns>
        public List<XtZhxmbModel> GetZhxmList(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (ht["yybh"] != null)
            {
                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", ht["yybh"].ToString());
            }

            if (ht["sfqy"] != null)
            {
                sd.Append(" and a.sfqy = @sfqy");
                paramList.Add("sfqy", ht["sfqy"].ToString());
            }
            if (ht["mc"] != null)
            {
                sd.Append(" and a.zhxmmc like  @mc");
                paramList.Add("mc", "%"+ht["mc"].ToString()+"%");
            }
            if(ht["sxrs"]!=null)
            {
                if(ht["sxrs"].ToString()=="1")
                {
                    sd.Append(" and a.sxrs>0");
                }
                else
                {
                    sd.Append(" and a.sxrs=0");
                }
            }

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from xt_zhxmb a where 1=1";
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<XtZhxmbModel> li = conn.Query<XtZhxmbModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        /// 渠道端组合项目列表
        /// </summary>
        /// <returns></returns>
        public List<XtZhxmbModel> GetQdZhxmList(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (ht["qdid"] != null)
            {
                sd.Append(" and c.qdid = @qdid");
                paramList.Add("qdid", ht["qdid"].ToString());
            }

            if (ht["sfqy"] != null)
            {
                sd.Append(" and a.sfqy = @sfqy");
                paramList.Add("sfqy", ht["sfqy"].ToString());
            }
            if (ht["mc"] != null)
            {
                sd.Append(" and a.zhxmmc like  @mc");
                paramList.Add("mc", "%" + ht["mc"].ToString() + "%");
            }
            if (ht["jgmc"] != null)
            {
                sd.Append(" and b.jgmc like  @jgmc");
                paramList.Add("jgmc", "%" + ht["jgmc"].ToString() + "%");
            }
            if (ht["sxrs"] != null)
            {
                if (ht["sxrs"].ToString() == "1")
                {
                    sd.Append(" and a.sxrs>0");
                }
                else
                {
                    sd.Append(" and a.sxrs=0");
                }
            }

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.*,b.jgmc from xt_zhxmb a join xt_jgb b on a.yybh=b.yybh join xt_qdjbxx c on b.yybh=c.yybh where 1=1 ";
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By id desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<XtZhxmbModel> li = conn.Query<XtZhxmbModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        /// 得到一个组合项目
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="zhxmbh"></param>
        /// <returns></returns>
        public XtZhxmbModel GetZhxm(string yybh, string zhxmbh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select * from xt_zhxmb a where yybh=@yybh and zhxmbh=@zhxmbh";

                XtZhxmbModel model = conn.Query<XtZhxmbModel>(sql, new { yybh = yybh, zhxmbh = zhxmbh }).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 同步组合项目
        /// </summary>
        /// <param name="li"></param>
        public void InsertOrUpdate(List<XtZhxmbModel> li)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select zhxmbh from xt_zhxmb a where yybh='" + li[0].yybh + "'";
                List<XtZhxmbModel> liOld = conn.Query<XtZhxmbModel>(sql,null).ToList();

                string sqlcz = "";
                IDbTransaction transaction = conn.BeginTransaction();
                foreach (XtZhxmbModel model in li)
                {


                    string zhxmms = string.IsNullOrEmpty(model.zhxmms) ? "" : model.zhxmms;

                    int i = liOld.Where(p => p.zhxmbh == model.zhxmbh.Trim()).Count();
                    if (i > 0)
                    {
                        sqlcz = "update xt_zhxmb set zhxmmc=@zhxmmc,zhxmms=@zhxmms,zhxmjg=@zhxmjg,xb=@xb,sffk=@sffk,sfqy=@sfqy,zhxmksbh=@zhxmksbh,zhxmksmc=@zhxmksmc, sxrs=@sxrs,updatetime=getdate() where yybh=@yybh and zhxmbh=@zhxmbh and zhxmbh=@zhxmbh;";

                        conn.Execute(sqlcz, model, transaction);
                    }
                    else
                    {
                        sqlcz = "insert into  xt_zhxmb(yybh,zhxmbh,zhxmmc,zhxmms,zhxmjg,xb,sffk,sfqy,zhxmksbh,zhxmksmc,createtime,updatetime,sxrs) values(@yybh, @zhxmbh, @zhxmmc,@zhxmms,@zhxmjg,@xb,@sffk,@sfqy,@zhxmksbh,@zhxmksmc,getdate(),getdate(),@sxrs);";
                        conn.Execute(sqlcz, model, transaction);
                    }


                }
                transaction.Commit();               
            }
        }

        /// <summary>
        /// 上限上数组合项目
        /// </summary>
        /// <param name="yybh"></param>
        /// <returns></returns>
        public List<XtZhxmbModel> GetListSxrs(string yybh)
        {
            string sql = "SELECT * FROM dbo.xt_zhxmb WHERE yybh=@yybh and sxrs>0";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var list = conn.Query<XtZhxmbModel>(sql,new { yybh=yybh}).ToList();
                return list;
            }
        }
        /// <summary>
        /// 更新上限人数
        /// </summary>
        /// <param name="zhxmbh"></param>
        /// <param name="sxrs"></param>
        public void UpdateSxrs(string zhxmbh, int sxrs)
        {
            string sql = "UPDATE dbo.xt_zhxmb SET sxrs=@sxrs WHERE zhxmbh=@zhxmbh";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(sql, new { zhxmbh = zhxmbh, sxrs = sxrs });
            }
        }
        /// <summary>
        /// 套餐组合项目
        /// </summary>
        /// <param name="yybh"></param>
        /// <param name="tcbh"></param>
        /// <returns></returns>
        public List<XtZhxmbModel> GetListByTcbh(string yybh,string tcbh)
        {
            string sql = "select b.* from xt_tc_zhxmb a join xt_zhxmb b on a.yybh=b.yybh and a.zhxmbh=b.zhxmbh where  a.yybh=@yybh and a.tcbh=@tcbh ";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var list = conn.Query<XtZhxmbModel>(sql, new { tcbh = tcbh,yybh=yybh }).ToList();
                return list;
            }
        }

        public List<JxbZhxmDto> GetJxbListByTcbh(string yybh,string tcbh)
        {
            string sql = "SELECT a.jxbid,d.zhxmmc,d.zhxmms,c.jg,b.jxbmc,b.syrq,b.jg AS jxbJg FROM dbo.qy_jxbtc a INNER JOIN dbo.qy_jxbjbxx b ON a.jxbid=b.id INNER JOIN dbo.qy_jxbzhxm c ON b.id=c.jxbid INNER JOIN dbo.xt_zhxmb d ON c.zhxmbh=d.zhxmbh and c.yybh=d.yybh WHERE c.yybh=@yybh and a.tcbh=@tcbh AND b.sfqy=1 AND d.sfqy=1";

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var list = conn.Query<JxbZhxmDto>(sql, new { tcbh = tcbh,yybh=yybh }).ToList();
                return list;
            }
        }

    }
}
