using Dapper;
using Model;
using Model.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Service
{
    public class DdyymxbService
    {

        /// <summary>
        /// 医院端企业订单
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DdjbxxModel> OrderYyQyList(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
            {
                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", ht["yybh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["dwbh"].ToString()))
            {
                sd.Append(" and left(a.dwbh,4)=@dwbh");
                paramList.Add("dwbh", ht["dwbh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["ddly"].ToString()))
            {
                sd.Append(" and a.ddly = @ddly");
                paramList.Add("ddly", ht["ddly"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["ddstart"].ToString()))
            {
                sd.Append(" and a.intime>= @ddstart ");
                paramList.Add("ddstart", ht["ddstart"].ToString());

            }
            if (!string.IsNullOrEmpty(ht["ddend"].ToString()))
            {
                sd.Append(" and a.intime<= @ddend ");
                paramList.Add("ddend", ht["ddend"].ToString() + " 23:59:59");
            }

            if (!string.IsNullOrEmpty(ht["yystart"].ToString()))
            {
                sd.Append(" and a.yykssj>= @yystart ");
                paramList.Add("yystart", ht["yystart"].ToString());

            }
            if (!string.IsNullOrEmpty(ht["yyend"].ToString()))
            {
                sd.Append(" and a.yykssj<= @yyend ");
                paramList.Add("yyend", ht["yyend"].ToString() + " 23:59:59");
            }




            if (!string.IsNullOrEmpty(ht["dh"].ToString()))
            {
                sd.Append(" and a.dh = @dh");
                paramList.Add("dh", ht["dh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["xm"].ToString()))
            {
                sd.Append("  and a.xm = @xm");
                paramList.Add("xm", ht["xm"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["tczt"].ToString()))
            {
                sd.Append(" and a.ddzt=@tczt ");
                paramList.Add("tczt", ht["tczt"].ToString());
            }


            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.*  from dd_jbxx_new a  where 1=1";
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By ddbh desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }
        /// <summary>
        /// 医院端企业订单导出
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable ExportOrderYyQyList(Hashtable ht)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
            {
                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", ht["yybh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["dwbh"].ToString()))
            {
                sd.Append(" and left(a.dwbh,4)=@dwbh");
                paramList.Add("dwbh", ht["dwbh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["ddly"].ToString()))
            {
                sd.Append(" and a.ddly = @ddly");
                paramList.Add("ddly", ht["ddly"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["ddstart"].ToString()))
            {
                sd.Append(" and a.intime>= @ddstart ");
                paramList.Add("ddstart", ht["ddstart"].ToString());

            }
            if (!string.IsNullOrEmpty(ht["ddend"].ToString()))
            {
                sd.Append(" and a.intime<= @ddend ");
                paramList.Add("ddend", ht["ddend"].ToString() + " 23:59:59");
            }

            if (!string.IsNullOrEmpty(ht["yystart"].ToString()))
            {
                sd.Append(" and a.yykssj>= @yystart ");
                paramList.Add("yystart", ht["yystart"].ToString());

            }
            if (!string.IsNullOrEmpty(ht["yyend"].ToString()))
            {
                sd.Append(" and a.yykssj<= @yyend ");
                paramList.Add("yyend", ht["yyend"].ToString() + " 23:59:59");
            }




            if (!string.IsNullOrEmpty(ht["dh"].ToString()))
            {
                sd.Append(" and a.dh = @dh");
                paramList.Add("dh", ht["dh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["xm"].ToString()))
            {
                sd.Append("  and a.xm = @xm");
                paramList.Add("xm", ht["xm"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["tczt"].ToString()))
            {
                sd.Append(" and a.ddzt=@tczt ");
                paramList.Add("tczt", ht["tczt"].ToString());
            }

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = @"select a.djlsh as '医院登记编号',a.dwmc as '企业名称',a.dwbh as '企业编号', a.tcjg as '套餐价格', a.xm as '姓名', case when a.xb=1 then '男' else '女' end as '性别'  
                              , a.dh as '电话', case when a.ddzt=2 then '预约中' when a.ddzt=3 then '预约成功' when a.ddzt=4 then '退款中' when a.ddzt=5 then '退款完成'
                            when a.ddzt=6 then '已到检' when a.ddzt=7 then '已完成' when a.ddzt=8 then '已取消' else '已出报告'  end as '订单状态', CONVERT(varchar(10),a.yykssj,120) as '预约时间'
                            from dd_jbxx_new a  where 1=1";
                sql += sd.ToString();

                sql += sd.ToString();
                var reader = conn.ExecuteReader(sql.ToString(), paramList);
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }
      
        /// <summary>
        /// 医院端渠道订单
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DdjbxxModel> OrderYyQdList(Hashtable ht, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
            {
                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", ht["yybh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["qdbh"].ToString())) 
            {
                sd.Append(" and a.dwbh = @qdbh");
                paramList.Add("qdbh", ht["qdbh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["ddly"].ToString())) 
            {
                sd.Append(" and a.ddly = @ddly");
                paramList.Add("ddly", ht["ddly"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["ddstart"].ToString()))
            {     
                sd.Append(" and a.intime>= @ddstart ");
                paramList.Add("ddstart", ht["ddstart"].ToString());
              
            }
            if (!string.IsNullOrEmpty(ht["ddend"].ToString()))
            {            
                sd.Append(" and a.intime<= @ddend ");
                paramList.Add("ddend", ht["ddend"].ToString() + " 23:59:59");
            }

            if (!string.IsNullOrEmpty(ht["yystart"].ToString()))
            {
                sd.Append(" and a.yykssj>= @yystart ");
                paramList.Add("yystart", ht["yystart"].ToString());

            }
            if (!string.IsNullOrEmpty(ht["yyend"].ToString()))
            {
                sd.Append(" and a.yykssj<= @yyend ");
                paramList.Add("yyend", ht["yyend"].ToString()+" 23:59:59");
            }




            if (!string.IsNullOrEmpty(ht["dh"].ToString())) 
            {
                sd.Append(" and a.dh = @dh");
                paramList.Add("dh", ht["dh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["xm"].ToString()))
            {
                sd.Append("  and a.xm = @xm");
                paramList.Add("xm", ht["xm"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["tczt"].ToString()))
            {
                sd.Append(" and a.ddzt=@tczt ");
                paramList.Add("tczt", ht["tczt"].ToString());
            }
            if (!string.IsNullOrEmpty(ht["tcmc"].ToString()))
            {
                sd.Append(" and a.tcmc like @tcmc ");
                paramList.Add("tcmc","%"+ ht["tcmc"].ToString()+"%");
            }

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "select a.* from dd_jbxx_new a  where 1=1";
                sql += sd.ToString();

                string sqlNew = "";
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sqlNew = "Select * From (Select ROW_NUMBER() Over (Order By ddbh desc) As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "";

                List<DdjbxxModel> li = conn.Query<DdjbxxModel>(sqlNew, paramList).ToList();
                count = conn.Query<int>("Select Count(1) From (" + sql + ") As t ", paramList).FirstOrDefault();
                return li;
            }
        }

        /// <summary>
        /// 导出医院端渠道订单
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable ExportOrderYyQdList(Hashtable ht)
        {
            StringBuilder sd = new StringBuilder();

            DynamicParameters paramList = new DynamicParameters();
            if (!string.IsNullOrEmpty(ht["yybh"].ToString()))
            {
                sd.Append(" and a.yybh = @yybh");
                paramList.Add("yybh", ht["yybh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["dsfbz"].ToString()))
            {
                sd.Append(" and a.dsfbzid = @dsfbz");
                paramList.Add("dsfbz", ht["dsfbz"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["ddly"].ToString()))
            {
                sd.Append(" and a.ddly = @ddly");
                paramList.Add("ddly", ht["ddly"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["ddstart"].ToString()))
            {
                sd.Append(" and a.intime>= @ddstart ");
                paramList.Add("ddstart", ht["ddstart"].ToString());

            }
            if (!string.IsNullOrEmpty(ht["ddend"].ToString()))
            {
                sd.Append(" and a.intime<= @ddend ");
                paramList.Add("ddend", ht["ddend"].ToString() + " 23:59:59");
            }

            if (!string.IsNullOrEmpty(ht["yystart"].ToString()))
            {
                sd.Append(" and a.yykssj>= @yystart ");
                paramList.Add("yystart", ht["yystart"].ToString());

            }
            if (!string.IsNullOrEmpty(ht["yyend"].ToString()))
            {
                sd.Append(" and a.yykssj<= @yyend ");
                paramList.Add("yyend", ht["yyend"].ToString() + " 23:59:59");
            }




            if (!string.IsNullOrEmpty(ht["dh"].ToString()))
            {
                sd.Append(" and a.dh = @dh");
                paramList.Add("dh", ht["dh"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["xm"].ToString()))
            {
                sd.Append("  and a.xm = @xm");
                paramList.Add("xm", ht["xm"].ToString());
            }

            if (!string.IsNullOrEmpty(ht["tczt"].ToString()))
            {
                sd.Append(" and a.ddzt=@tczt ");
                paramList.Add("tczt", ht["tczt"].ToString());
            }

            //sd.Append(" and a.ddbh in (" + ddbhs + ")");

            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                //string sql = @"select a.dsfdd as '订单编号', b.dsfbz as '渠道名称', a.tcmc as '套餐名称', a.tcjg as '套餐价格', a.xm as '姓名', case when a.xb=1 then '男' else '女' end as '性别'  
                //              , a.dh as '电话', case when a.ddzt=2 then '预约中' when a.ddzt=3 then '预约成功' when a.ddzt=4 then '退款中' when a.ddzt=5 then '退款完成'
                //            when a.ddzt=6 then '已到检' when a.ddzt=7 then '已完成' when a.ddzt=8 then '已取消' else '已出报告'  end as '订单状态', CONVERT(varchar(10),a.yykssj,120) as '预约时间'
                //            from dd_jbxx a join xt_dsfbz b on a.dsfbzid=b.id where 1=1";
                //sql += sd.ToString();

                string sql = @"select '' as 工卡号,a.dwbh as 单位,a.xm as 姓名,a.csrq  as 出生日期,a.tcmc as 分组名称,
                a.dh as 联系电话,case a.hz when 1 then '已婚' else '未婚' end as 婚姻, '' as 标签个数, a.zjhm as 身份证号,'' as 职称,'' as 职务,'' as 人员类别,'' as 体检类别,a.dh as 手机号码,'' as 电子邮件,0 as 是否VIP ,a.yykssj as 预定体检日期
                from dd_jbxx_new a where 1 = 1";
                sql += sd.ToString();
                var reader = conn.ExecuteReader(sql.ToString(), paramList);
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

      
       
        /// <summary>
        /// 得到订单基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DdjbxxModel GetBydsfddxx(string ddbh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                DdjbxxModel model= conn.Query<DdjbxxModel>("select * From  dd_jbxx_new where ddbh=@ddbh ", new { ddbh = ddbh }).FirstOrDefault();
                model.zhxmlist = GetOrderZhxm(ddbh);
                return model;
            }
        }
      

        /// <summary>
        /// 创建中间平台订单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tcmc"></param>
        /// <param name="nl"></param>
        /// <returns></returns>
        public int CreateOrder(PreOrder model,string tcmc,string ddbh,string dwmc,decimal ysjsj=0)
        {
            int sfjx = 0;
            string jxlist = "";
            string jxlist2 = "";
            if(model.items!=null&&model.items.Count>0)
            {
                sfjx = 1;
                foreach(var item in model.items)
                {
                    if(item.isAdd==1)
                    {
                        jxlist += item.itemCode + ",";
                        jxlist2 += "'" + item.itemCode + "',";
                    }
                }
            }
            jxlist = jxlist.TrimEnd(',');
            jxlist2 = jxlist2.TrimEnd(',');
            string sql = @"insert into dd_jbxx(ddbh,dsfdd,dsfbzid,ddzt,tcid,tcmc,dwbh,tcjg,jxbjg,ddze,intime,sfout,outtime,dh,xm,xb,hz,zjlx,zjhm,yykssj,yyjssj,sfdj,djtime,sfbg,bgtime,djlsh,sfjx,jxlist,sfjs,csrq,nl,remark,yybh,ddly,dwmc,jsbz,ygzh,trade_no,ysjsj)
            values(@ddbh,@dsfdd,@dsfbzid,2,@tcid,@tcmc,@dwbh,@tcjg,
                    0,@ddze,getdate(),0,'1970-01-01',@dh,@xm,@xb,@hz,
                    @zjlx,@zjhm,@yysj,@yysj,0,'1970-01-01',0,'1970-01-01',
                    '',@sfjx,@jxlist,0,@csrq,@nl,'',@yybh,0,@dwmc,0,'',@trade_no,@ysjsj) ";
            
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                IDbTransaction transaction = conn.BeginTransaction();

                int id = conn.Execute(sql, new
                {
                    ddbh = ddbh,dsfdd = model.outOrderCode,dsfbzid = model.interfaceNum,tcid = model.appointmentPackageCode,tcmc = tcmc,dwbh = model.deptID,tcjg = model.ddJe,
                    ddze = model.ddJe,dh = model.customerMobilePhone,xm = model.customerName,xb = model.customerGender,hz = model.customerMedicalStatus,
                    zjlx = model.customerIDType,zjhm = model.customerIDCard,yysj = model.appointmentDate,
                    sfjx = sfjx,jxlist = jxlist,csrq = model.customerBirthday,nl = model.customerAge,yybh = model.yybh,dwmc=dwmc,
                    trade_no=string.IsNullOrEmpty(model.tradeNo)?"":model.tradeNo,ysjsj = ysjsj

                }, transaction);

                string sqlDetail = "insert into dd_zhxm(ddbh,zhxmbh,zhxmmc,jg,sfjx,sfdj) select  '" + ddbh + "',b.zhxmbh,b.zhxmmc,b.zhxmjg,0,0 from  xt_tc_zhxmb a join xt_zhxmb b on a.yybh=b.yybh and a.zhxmbh=b.zhxmbh where a.yybh='" + model.yybh + "' and a.tcbh='" + model.appointmentPackageCode + "' and a.dwbh='" + model.deptID + "'";
                if (jxlist2.Length > 1)
                {
                    string sqlDetailJx = "insert into dd_zhxm(ddbh,zhxmbh,zhxmmc,jg,sfjx,sfdj) select '" + ddbh + "',b.zhxmbh,b.zhxmmc,b.zhxmjg,1,0 from  xt_zhxmb b  where b.yybh='" + model.yybh + "' and b.zhxmbh in (" + jxlist2 + ")" ;
                    conn.Execute(sqlDetailJx, null, transaction);
                }
                conn.Execute(sqlDetail, null, transaction);
                transaction.Commit();
                return id;
            }             
        }

        public void CreateOrder(List<OrderModel> li)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                IDbTransaction transaction = conn.BeginTransaction();
                foreach(OrderModel model in li)
                {
                    string ddbh = new DdJbxxService().GetDdbh();
                    int sfjx = 0;
                    string sql = @"insert into dd_jbxx(ddbh,dsfdd,dsfbzid,ddzt,tcid,tcmc,dwbh,tcjg,
                    jxbjg,ddze,intime,sfout,outtime,dh,xm,xb,hz,
                zjlx,zjhm,yykssj,yyjssj,sfdj,djtime,sfbg,bgtime,
                djlsh,sfjx,jxlist,sfjs,csrq,nl,remark,yybh,ddly,dwmc,jsbz,ygzh,trade_no,ysjsj)
                values(@ddbh,@dsfdd,@dsfbzid,2,@tcid,@tcmc,@dwbh,@tcjg,
                    0,@ddze,getdate(),0,'1970-01-01',@dh,@xm,@xb,@hz,
                    @zjlx,@zjhm,@yysj,@yysj,0,'1970-01-01',0,'1970-01-01',
                    '',@sfjx,@jxlist,0,@csrq,@nl,'',@yybh,0,@dwmc,0,'',@trade_no,@ysjsj) ";
                    int id = conn.Execute(sql, new
                    {
                        ddbh = ddbh,
                        dsfdd = model.outOrderCode,
                        dsfbzid = model.dsfbzId,
                        tcid = model.appointmentPackageCode,
                        tcmc = model.appointmentPackageName,
                        dwbh = model.deptID,
                        tcjg = model.ddJe,
                        ddze = model.ddJe,
                        dh = model.customerMobilePhone,
                        xm = model.customerName,
                        xb = model.customerGender,
                        hz = model.customerMedicalStatus,
                        zjlx = model.customerIDType,
                        zjhm = model.customerIDCard,
                        yysj = model.appointmentDate,
                        sfjx = sfjx,
                        jxlist = "",
                        csrq = model.customerBirthday,
                        nl = model.customerAge,
                        yybh = model.yybh,
                        dwmc = model.deptNm,
                        trade_no ="",
                        ysjsj = model.ysjsj

                    }, transaction);

                    string sqlDetail = "insert into dd_zhxm(ddbh,zhxmbh,zhxmmc,jg,sfjx,sfdj) select  '" + ddbh + "',b.zhxmbh,b.zhxmmc,b.zhxmjg,0,0 from  xt_tc_zhxmb a join xt_zhxmb b on a.yybh=b.yybh and a.zhxmbh=b.zhxmbh where a.yybh='" + model.yybh + "' and a.tcbh='" + model.appointmentPackageCode + "' and a.dwbh='" + model.deptID + "'";
                  
                    conn.Execute(sqlDetail, null, transaction);
                }               
                transaction.Commit();
              
            }
        }

        /// <summary>
        /// 订单组合项目
        /// </summary>
        /// <param name="ddid"></param>
        /// <returns></returns>
        public List<DdZhxmModel> GetOrderZhxm(string ddid)
        {
            string sql = "select c.zhxmbh,c.zhxmmc,b.jg,c.zhxmksbh,c.zhxmksmc,c.xb,c.sffk, b.sfjx,b.sfdj from dd_jbxx_new a join  dd_zhxm b on a.ddbh=b.ddbh join xt_zhxmb c on a.yybh=c.yybh and b.zhxmbh=c.zhxmbh where a.ddbh=@id";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<DdZhxmModel> li = conn.Query<DdZhxmModel>(sql, new { id= ddid }).ToList();
                return li;
            }
        }
        /// <summary>
        /// 订单组合项目加项
        /// </summary>
        /// <param name="ddid"></param>
        /// <returns></returns>
        public List<DdZhxmModel> GetOrderZhxmJx(string ddid)
        {
            string sql = "select zhxmbh from  dd_zhxm where ddbh=@id and  sfjx=1";
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                List<DdZhxmModel> li = conn.Query<DdZhxmModel>(sql, new { id = ddid }).ToList();
                return li;
            }
        }

        /// <summary>
        /// 修改单据到达医院状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="djlsh"></param>
        public void UpdatOrderdjlsh(string ddbh, string djlsh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "update dd_jbxx_new set ddzt=3,sfout=1,outtime=getdate(),djlsh=@djlsh where ddbh=@ddbh";
                conn.Execute(sql, new { djlsh = djlsh, ddbh = ddbh });
            }
        }
       
        public void UpdatOrderdaojiantest(string ddbh, string djtime)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "update dd_jbxx set ddzt=6,sfdj=1,djtime=@djtime where ddbh=@ddbh;update dd_zhxm set sfdj=1 where ddbh=@ddbh";
                conn.Execute(sql, new { djtime = djtime, ddbh = ddbh });
            }
        }
   
        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remark"></param>
        public void UpdatOrderremark(string ddbh, string remark)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "update dd_jbxx_new set remark=@remark where ddbh=@ddbh";
                conn.Execute(sql, new { remark = remark, ddbh = ddbh });
            }
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string ddbh)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                string sql = "update  dd_jbxx_new set  ddzt=8,remark='客户主动取消订单' where  ddbh=@ddbh";
                conn.Execute(sql, new { ddbh = ddbh });
            }
        }
      
        
        /// <summary>
        /// 企业团检订单详情
        /// </summary>
        /// <param name="ddbh"></param>
        /// <returns></returns>
        public DdjbxxModel GetBydsfddxxQy(string ddbh)
        {
            DdjbxxModel model = new DdjbxxModel();
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                model = conn.Query<DdjbxxModel>("select a.*,c.jgmc  from dd_jbxx_new a  join xt_jgb c on a.yybh=c.yybh where  ddbh=@ddbh ", new { ddbh = ddbh }).FirstOrDefault();

            }
            model.zhxmlist = GetOrderZhxm(ddbh);
            return model;
        }
       
        public void Test(string sql)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                conn.Execute(sql, null);
            }
        }

        public ReturnModel BatchComplete(string ddbhs, int ddzt)
        {
            using (IDbConnection conn = new DapperConnection().DbConnection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    List<string> listDdbh = ddbhs.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var sbSql = new StringBuilder();
                    sbSql.Append("update dd_jbxx_new set ddzt=@Ddzt where ddbh in @Ddbh;");
                    if (ddzt.Equals(7))
                    {
                        sbSql.Append("update dd_zhxm set sfdj=1 where ddbh in @Ddbh;");
                    }
                    conn.Execute(sbSql.ToString(), new { Ddzt = ddzt, Ddbh = listDdbh }, transaction);
                    transaction.Commit();
                    return new ReturnModel {Code = 200, Msg = "保存成功"};
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.WriteLog("批量完成失败" + ex.Message);
                    return new ReturnModel { Code = 201, Msg = "保存失败" };
                }
                
            }
        }
    }
}
