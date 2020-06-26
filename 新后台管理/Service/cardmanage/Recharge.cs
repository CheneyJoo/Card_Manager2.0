using Common;
using NBear.Data;
using SYSEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Recharge
    {
        /// <summary>
        /// 保存导入数据
        /// </summary>
        /// <param name="recharges"></param>
        /// <returns></returns>
        public int SaveRechargeList(List<TB_DATA_RECHARGE> recharges)
        {
            Gateway gateway = Gateway.Default;
            DbTransaction tran = gateway.BeginTransaction();
            try
            {
                DataTable brandData = gateway.FromCustomSql("select * from TB_DATA_BRAND").SetTransaction(tran).ToDataSet().Tables[0];
                DataTable agentData = gateway.FromCustomSql("select * from TB_DATA_AGENT  where STATUS>-1").SetTransaction(tran).ToDataSet().Tables[0];
                String agent_id = String.Empty, end_customer_id = String.Empty, brand_id = String.Empty;
                //获取导入的代理商 终端客户 品牌信息字段对应的ID，不存在就创建对应的信息
                for (int i = 0; i < recharges.Count; i++)
                {
                    TB_DATA_RECHARGE tB_DATA_RECHARGE = recharges[i];
                    if (!String.IsNullOrEmpty(tB_DATA_RECHARGE.AGEN_ID.Trim()))
                    {
                        agent_id = agentData.Select().Where(s => s["AGENT_NAME"].ToString() == tB_DATA_RECHARGE.AGEN_ID.Trim()).Select(s => s["AGENT_ID"].ToString()).FirstOrDefault();
                        if (String.IsNullOrEmpty(agent_id))
                        {
                            TB_DATA_AGENT tB_DATA_AGENT = new TB_DATA_AGENT();
                            agent_id = Guid.NewGuid().ToString();
                            tB_DATA_AGENT.AGENT_ID = agent_id;
                            tB_DATA_AGENT.AGENT_NAME = tB_DATA_RECHARGE.AGEN_ID.Trim();
                            tB_DATA_AGENT.STATUS = 1;
                            tB_DATA_AGENT.CREATED_BY = tB_DATA_RECHARGE.CREATED_BY;
                            tB_DATA_AGENT.CREATED_TIME = DateTime.Now;
                            tB_DATA_AGENT.LAST_UPDATED_BY = tB_DATA_RECHARGE.CREATED_BY;
                            tB_DATA_AGENT.LAST_UPDATED_TIME = DateTime.Now;
                            gateway.Save<TB_DATA_AGENT>(tB_DATA_AGENT, tran);
                            agentData = gateway.FromCustomSql("select * from TB_DATA_AGENT  where STATUS>-1").SetTransaction(tran).ToDataSet().Tables[0];
                        }
                        tB_DATA_RECHARGE.AGEN_ID = agent_id;
                    }
                    if (!String.IsNullOrEmpty(tB_DATA_RECHARGE.BRAND_ID.Trim()))
                    {
                        brand_id = brandData.Select().Where(s => s["BRAND_NAME"].ToString() == tB_DATA_RECHARGE.BRAND_ID.Trim()).Select(s => s["BRAND_ID"].ToString()).FirstOrDefault();
                        if (String.IsNullOrEmpty(brand_id))
                        {
                            TB_DATA_BRAND tB_DATA_BRAND = new TB_DATA_BRAND();
                            brand_id = Guid.NewGuid().ToString();
                            tB_DATA_BRAND.BRAND_ID = brand_id;
                            tB_DATA_BRAND.BRAND_NAME = tB_DATA_RECHARGE.BRAND_ID.Trim();
                            tB_DATA_BRAND.CREATED_BY = tB_DATA_RECHARGE.CREATED_BY;
                            tB_DATA_BRAND.CREATED_TIME = DateTime.Now;
                            tB_DATA_BRAND.LAST_UPDATED_BY = tB_DATA_RECHARGE.CREATED_BY;
                            tB_DATA_BRAND.LAST_UPDATED_TIME = DateTime.Now;
                            gateway.Save<TB_DATA_BRAND>(tB_DATA_BRAND, tran);
                            brandData = gateway.FromCustomSql("select * from TB_DATA_BRAND").SetTransaction(tran).ToDataSet().Tables[0];
                        }
                        tB_DATA_RECHARGE.BRAND_ID = brand_id;
                    }
                    gateway.Save<TB_DATA_RECHARGE>(tB_DATA_RECHARGE, tran);
                }
                tran.Commit();
                return 1;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                return 0;
            }
            finally
            {
                gateway.CloseTransaction(tran);
                tran.Dispose();
            }
        }

        /// <summary>
        /// 删除消费数据
        /// </summary>
        /// <param name="RECHARGE_ID"></param>
        /// <returns></returns>
        public int DeleteRecharge(String RECHARGE_IDs)
        {
            try
            {
                Gateway.Default.FromCustomSql("delete from TB_DATA_RECHARGE where RECHARGE_ID in(" + "'" + String.Join("','", RECHARGE_IDs.Split(new char[] { ',' })) + "'" + ")").ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取所有的品牌
        /// </summary>
        /// <returns></returns>
        public DataTable QueryBrand() 
        {
            DataTable brandData = Gateway.Default.FromCustomSql("select * from TB_DATA_BRAND ORDER BY CREATED_TIME DESC").ToDataSet().Tables[0];
            return brandData;
        }
        /// <summary>
        /// 获取所有的代理商
        /// </summary>
        /// <returns></returns>
        public DataTable QueryAgent()
        {
            DataTable agentData = Gateway.Default.FromCustomSql("select * from TB_DATA_AGENT  where STATUS>-1 ORDER BY CREATED_TIME DESC").ToDataSet().Tables[0];
            return agentData;
        }
        /// <summary>
        /// 查询代理商
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currPage"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IDictionary<String, Object> QueryRecharge(Int32 pageSize, Int32 currPage, IDictionary<String, String> where)
        {
            DataTable dtPage = null;
            Int32 totalRow = 0;
            Int32 totalPage = 0;

            string sqlCount = @"select  count(1) totalrow  ";
            string sql = @"select 0 as 'NO',a.RECHARGE_ID,b.BRAND_NAME,a.SERVICE_PROVIDER,c.AGENT_NAME,a.CARD_NO,a.RECHARGE_MONEY,a.RECHARGE_DATE,a.PACKAGE_NAME ";
            string othersql = @" from TB_DATA_RECHARGE a
                            inner join TB_DATA_BRAND b on a.BRAND_ID = b.BRAND_ID
                            inner join TB_DATA_AGENT c on a.AGEN_ID = c.AGENT_ID 
                            WHERE c.STATUS>-1";

            if (where.ContainsKey("CARD_NO"))
            {
                othersql = othersql + String.Format(" AND a.CARD_NO like @CARD_NO");
            }
            if (where.ContainsKey("AGEN_ID"))
            {
                othersql = othersql + String.Format(" AND a.AGEN_ID = @AGEN_ID");
            }
            if (where.ContainsKey("BRAND_ID"))
            {
                othersql = othersql + String.Format(" AND a.BRAND_ID = @BRAND_ID");
            }
            if (where.ContainsKey("BEGIN_DATE"))
            {
                othersql = othersql + String.Format(" AND a.RECHARGE_DATE >= @BEGIN_DATE");
            }
            if (where.ContainsKey("END_DATE"))
            {
                othersql = othersql + String.Format(" AND a.RECHARGE_DATE < @END_DATE");
            }
            sqlCount = sqlCount + othersql;
            sql = sql + othersql;

            //设定排序条件
            sql = sql + String.Format(" ORDER BY {0} {1} ", "a.CREATED_TIME", "DESC");

            CustomSqlSection customCount = Gateway.Default.FromCustomSql(sqlCount);
            CustomSqlSection custom = Gateway.Default.FromCustomSql(sql);

            if (where.ContainsKey("CARD_NO"))
            {
                customCount.AddInputParameter("CARD_NO", DbType.String, "%" + where["CARD_NO"] + "%");
                custom.AddInputParameter("CARD_NO", DbType.String, "%" + where["CARD_NO"] + "%");
            }
            if (where.ContainsKey("AGEN_ID"))
            {
                customCount.AddInputParameter("AGEN_ID", DbType.String, where["AGEN_ID"]);
                custom.AddInputParameter("AGEN_ID", DbType.String, where["AGEN_ID"]);
            }
            if (where.ContainsKey("BRAND_ID"))
            {
                customCount.AddInputParameter("BRAND_ID", DbType.String, where["BRAND_ID"]);
                custom.AddInputParameter("BRAND_ID", DbType.String, where["BRAND_ID"]);
            }
            if (where.ContainsKey("BEGIN_DATE"))
            {
                customCount.AddInputParameter("BEGIN_DATE", DbType.String, where["BEGIN_DATE"]);
                custom.AddInputParameter("BEGIN_DATE", DbType.String, where["BEGIN_DATE"]);
            }
            if (where.ContainsKey("END_DATE"))
            {
                customCount.AddInputParameter("END_DATE", DbType.DateTime,Convert.ToDateTime(where["END_DATE"]).AddDays(1));
                custom.AddInputParameter("END_DATE", DbType.DateTime, Convert.ToDateTime(where["END_DATE"]).AddDays(1));
            }
            try
            {
                totalRow = Convert.ToInt32(customCount.ToDataSet().Tables[0].Rows[0]["totalrow"]);
                totalPage = Convert.ToInt32(Math.Ceiling(totalRow * 1.0 / pageSize));
                totalPage = totalPage == 0 ? 1 : totalPage;

                if (totalPage < currPage)
                {
                    currPage = totalPage;
                }
                dtPage = custom.ToDataTable(pageSize, currPage);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return null;
            }

            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            dic.Add("table", dtPage);
            dic.Add("totalRow", totalRow);
            dic.Add("totalPage", totalPage);
            return dic;
        }

        /// <summary>
        /// 导出代理商
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currPage"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable ExportRecharge(IDictionary<String, String> where)
        {
            string sql = @"select '0' AS '编号',b.BRAND_NAME AS '品牌',a.SERVICE_PROVIDER AS '运营商',c.AGENT_NAME AS '代理商',a.CARD_NO AS '卡号',a.RECHARGE_MONEY AS '充值金额',a.RECHARGE_DATE AS '充值时间',a.PACKAGE_NAME AS '套餐'";
            string othersql = @" from TB_DATA_RECHARGE a
                            inner join TB_DATA_BRAND b on a.BRAND_ID = b.BRAND_ID
                            inner join TB_DATA_AGENT c on a.AGEN_ID = c.AGENT_ID 
                            WHERE c.STATUS>-1";

            if (where.ContainsKey("CARD_NO"))
            {
                othersql = othersql + String.Format(" AND a.CARD_NO like @CARD_NO");
            }
            if (where.ContainsKey("AGEN_ID"))
            {
                othersql = othersql + String.Format(" AND a.AGEN_ID = @AGEN_ID");
            }
            if (where.ContainsKey("BRAND_ID"))
            {
                othersql = othersql + String.Format(" AND a.BRAND_ID = @BRAND_ID");
            }
            if (where.ContainsKey("BEGIN_DATE"))
            {
                othersql = othersql + String.Format(" AND a.RECHARGE_DATE >= @BEGIN_DATE");
            }
            if (where.ContainsKey("END_DATE"))
            {
                othersql = othersql + String.Format(" AND a.RECHARGE_DATE < @END_DATE");
            }
            sql = sql + othersql;

            //设定排序条件
            sql = sql + String.Format(" ORDER BY {0} {1} ", "a.CREATED_TIME", "DESC");

            CustomSqlSection custom = Gateway.Default.FromCustomSql(sql);

            if (where.ContainsKey("CARD_NO"))
            {
                custom.AddInputParameter("CARD_NO", DbType.String, "%" + where["CARD_NO"] + "%");
            }
            if (where.ContainsKey("AGEN_ID"))
            {
                custom.AddInputParameter("AGEN_ID", DbType.String, where["AGEN_ID"]);
            }
            if (where.ContainsKey("BRAND_ID"))
            {
                custom.AddInputParameter("BRAND_ID", DbType.String, where["BRAND_ID"]);
            }
            if (where.ContainsKey("BEGIN_DATE"))
            {
                custom.AddInputParameter("BEGIN_DATE", DbType.String, where["BEGIN_DATE"]);
            }
            if (where.ContainsKey("END_DATE"))
            {
                custom.AddInputParameter("END_DATE", DbType.DateTime, Convert.ToDateTime(where["END_DATE"]).AddDays(1));
            }
            try
            {
               return custom.ToDataSet().Tables[0];
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return null;
            }
        }
    }
}
