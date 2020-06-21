using Common;
using NBear.Data;
using SYSEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace SYS.Business.cardmanage
{
    public class Card
    {
        /// <summary>
        /// 新增单条卡记录
        /// </summary>
        /// <param name="tB_DATA_CARD"></param>
        /// <returns></returns>
        public int SaveCard(TB_DATA_CARD tB_DATA_CARD) 
        {
            Gateway gateway = Gateway.Default;
            DbTransaction tran = gateway.BeginTransaction();
            try
            {
                //取所有非删除的代理商
                String agentSql = "SELECT [AGENT_ID],[AGENT_NAME] FROM [dbo].[TB_DATA_AGENT] WHERE STATUS>-1";
                DataTable agentData = gateway.FromCustomSql(agentSql).SetTransaction(tran).ToDataSet().Tables[0];
                //取所有非删除的终端用户
                String endCustomSql = "select END_CUSTOMER_ID,END_CUSTOMER_NAME FROM  [dbo].[TB_DATA_END_CUSTOMER] WHERE STATUS>-1";
                DataTable endCustomData = gateway.FromCustomSql(endCustomSql).SetTransaction(tran).ToDataSet().Tables[0];
                //取所有非删除的品牌信息
                String brandSql = "select BRAND_ID,BRAND_NAME FROM [dbo].[TB_DATA_BRAND]";
                DataTable brandData = gateway.FromCustomSql(brandSql).SetTransaction(tran).ToDataSet().Tables[0];
                String agent_id = String.Empty, end_customer_id = String.Empty, brand_id = String.Empty;
                //获取导入的代理商 终端客户 品牌信息字段对应的ID，不存在就创建对应的信息
                if (!String.IsNullOrEmpty(tB_DATA_CARD.AGENT_ID.Trim()))
                {
                    agent_id = agentData.Select().Where(s => s["AGENT_NAME"].ToString() == tB_DATA_CARD.AGENT_ID.Trim()).Select(s => s["AGENT_ID"].ToString()).FirstOrDefault();
                    if (String.IsNullOrEmpty(agent_id))
                    {
                        TB_DATA_AGENT tB_DATA_AGENT = new TB_DATA_AGENT();
                        agent_id = Guid.NewGuid().ToString();
                        tB_DATA_AGENT.AGENT_ID = agent_id;
                        tB_DATA_AGENT.AGENT_NAME = tB_DATA_CARD.AGENT_ID.Trim();
                        tB_DATA_AGENT.STATUS = 1;
                        tB_DATA_AGENT.CREATED_BY = tB_DATA_CARD.CREATED_BY;
                        tB_DATA_AGENT.CREATED_TIME = DateTime.Now;
                        tB_DATA_AGENT.LAST_UPDATED_BY = tB_DATA_CARD.CREATED_BY;
                        tB_DATA_AGENT.LAST_UPDATED_TIME = DateTime.Now;
                        gateway.Save<TB_DATA_AGENT>(tB_DATA_AGENT, tran);
                    }
                    tB_DATA_CARD.AGENT_ID = agent_id;
                }
                if (!String.IsNullOrEmpty(tB_DATA_CARD.END_CUSTOMER_ID.Trim()))
                {
                    end_customer_id = endCustomData.Select().Where(s => s["END_CUSTOMER_NAME"].ToString() == tB_DATA_CARD.END_CUSTOMER_ID.Trim()).Select(s => s["END_CUSTOMER_ID"].ToString()).FirstOrDefault();
                    if (String.IsNullOrEmpty(end_customer_id))
                    {
                        TB_DATA_END_CUSTOMER tB_DATA_END_CUSTOMER = new TB_DATA_END_CUSTOMER();
                        end_customer_id = Guid.NewGuid().ToString();
                        tB_DATA_END_CUSTOMER.END_CUSTOMER_ID = end_customer_id;
                        tB_DATA_END_CUSTOMER.END_CUSTOMER_NAME = tB_DATA_CARD.END_CUSTOMER_ID.Trim();
                        tB_DATA_END_CUSTOMER.STATUS = 1;
                        tB_DATA_END_CUSTOMER.CREATED_BY = tB_DATA_CARD.CREATED_BY;
                        tB_DATA_END_CUSTOMER.CREATED_TIME = DateTime.Now;
                        tB_DATA_END_CUSTOMER.LAST_UPDATED_BY = tB_DATA_CARD.CREATED_BY;
                        tB_DATA_END_CUSTOMER.LAST_UPDATED_TIME = DateTime.Now;
                        gateway.Save<TB_DATA_END_CUSTOMER>(tB_DATA_END_CUSTOMER, tran);
                    }
                    tB_DATA_CARD.END_CUSTOMER_ID = end_customer_id;
                }
                if (!String.IsNullOrEmpty(tB_DATA_CARD.BRAND_ID.Trim()))
                {
                    brand_id = brandData.Select().Where(s => s["BRAND_NAME"].ToString() == tB_DATA_CARD.BRAND_ID.Trim()).Select(s => s["BRAND_ID"].ToString()).FirstOrDefault();
                    if (String.IsNullOrEmpty(brand_id))
                    {
                        TB_DATA_BRAND tB_DATA_BRAND = new TB_DATA_BRAND();
                        brand_id = Guid.NewGuid().ToString();
                        tB_DATA_BRAND.BRAND_ID = brand_id;
                        tB_DATA_BRAND.BRAND_NAME = tB_DATA_CARD.BRAND_ID.Trim();
                        tB_DATA_BRAND.CREATED_BY = tB_DATA_CARD.CREATED_BY;
                        tB_DATA_BRAND.CREATED_TIME = DateTime.Now;
                        tB_DATA_BRAND.LAST_UPDATED_BY = tB_DATA_CARD.CREATED_BY;
                        tB_DATA_BRAND.LAST_UPDATED_TIME = DateTime.Now;
                        gateway.Save<TB_DATA_BRAND>(tB_DATA_BRAND, tran);
                    }
                    tB_DATA_CARD.BRAND_ID = brand_id;
                }
                String cardID = gateway.Find<TB_DATA_CARD>(TB_DATA_CARD._.CARD_ICCID == tB_DATA_CARD.CARD_ICCID || TB_DATA_CARD._.CARD_NO == tB_DATA_CARD.CARD_NO).CARD_ID;
                if (!String.IsNullOrEmpty(cardID))
                {
                    tB_DATA_CARD.Attach();
                    tB_DATA_CARD.SetAllPropertiesAsModified();
                }
                gateway.Save<TB_DATA_CARD>(tB_DATA_CARD, tran);
                String updateSql = @"update TB_DATA_AGENT set AGENT_CARD_NUMBER=T.card_count
                                    from (
                                        select count(1) card_count,AGENT_ID from TB_DATA_CARD
                                        GROUP BY AGENT_ID
                                    ) T where TB_DATA_AGENT.AGENT_ID=T.AGENT_ID";
                gateway.FromCustomSql(updateSql).SetTransaction(tran).ExecuteNonQuery();
                tran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 保存卡数据集合管理员
        /// </summary>
        /// <param name="tB_DATA_CARD_LIST"></param>
        /// <returns></returns>
        public int SaveImportCardForAdmin(List<TB_DATA_CARD> tB_DATA_CARD_LIST)
        {
            Gateway gateway = Gateway.Default;
            DbTransaction tran = gateway.BeginTransaction();
            try
            {
                //取所有非删除的代理商
                String agentSql = "SELECT [AGENT_ID],[AGENT_NAME] FROM [dbo].[TB_DATA_AGENT] WHERE STATUS>-1";
                DataTable agentData = gateway.FromCustomSql(agentSql).SetTransaction(tran).ToDataSet().Tables[0];
                //取所有非删除的终端用户
                String endCustomSql = "select END_CUSTOMER_ID,END_CUSTOMER_NAME FROM  [dbo].[TB_DATA_END_CUSTOMER] WHERE STATUS>-1";
                DataTable endCustomData = gateway.FromCustomSql(endCustomSql).SetTransaction(tran).ToDataSet().Tables[0];
                //取所有非删除的品牌信息
                String brandSql = "select BRAND_ID,BRAND_NAME FROM [dbo].[TB_DATA_BRAND]";
                DataTable brandData = gateway.FromCustomSql(brandSql).SetTransaction(tran).ToDataSet().Tables[0];
                //循环添加卡信息
                for (int i = 0; i < tB_DATA_CARD_LIST.Count; i++)
                {
                    TB_DATA_CARD tB_DATA_CARD = tB_DATA_CARD_LIST[i];
                    String agent_id = String.Empty, end_customer_id = String.Empty, brand_id = String.Empty;
                    //获取导入的代理商 终端客户 品牌信息字段对应的ID，不存在就创建对应的信息
                    if (!String.IsNullOrEmpty(tB_DATA_CARD.AGENT_ID.Trim()))
                    {
                        agent_id = agentData.Select().Where(s => s["AGENT_NAME"].ToString() == tB_DATA_CARD.AGENT_ID.Trim()).Select(s => s["AGENT_ID"].ToString()).FirstOrDefault();
                        if (String.IsNullOrEmpty(agent_id))
                        {
                            TB_DATA_AGENT tB_DATA_AGENT = new TB_DATA_AGENT();
                            agent_id = Guid.NewGuid().ToString();
                            tB_DATA_AGENT.AGENT_ID = agent_id;
                            tB_DATA_AGENT.AGENT_NAME = tB_DATA_CARD.AGENT_ID.Trim();
                            tB_DATA_AGENT.STATUS = 1;
                            tB_DATA_AGENT.CREATED_BY = tB_DATA_CARD.CREATED_BY;
                            tB_DATA_AGENT.CREATED_TIME = DateTime.Now;
                            tB_DATA_AGENT.LAST_UPDATED_BY = tB_DATA_CARD.CREATED_BY;
                            tB_DATA_AGENT.LAST_UPDATED_TIME = DateTime.Now;
                            gateway.Save<TB_DATA_AGENT>(tB_DATA_AGENT, tran);
                        }
                        tB_DATA_CARD.AGENT_ID = agent_id;
                    }
                    if (!String.IsNullOrEmpty(tB_DATA_CARD.END_CUSTOMER_ID.Trim()))
                    {
                        end_customer_id = endCustomData.Select().Where(s => s["END_CUSTOMER_NAME"].ToString() == tB_DATA_CARD.END_CUSTOMER_ID.Trim()).Select(s => s["END_CUSTOMER_ID"].ToString()).FirstOrDefault();
                        if (String.IsNullOrEmpty(end_customer_id))
                        {
                            TB_DATA_END_CUSTOMER tB_DATA_END_CUSTOMER = new TB_DATA_END_CUSTOMER();
                            end_customer_id = Guid.NewGuid().ToString();
                            tB_DATA_END_CUSTOMER.END_CUSTOMER_ID = end_customer_id;
                            tB_DATA_END_CUSTOMER.END_CUSTOMER_NAME = tB_DATA_CARD.END_CUSTOMER_ID.Trim();
                            tB_DATA_END_CUSTOMER.STATUS = 1;
                            tB_DATA_END_CUSTOMER.CREATED_BY = tB_DATA_CARD.CREATED_BY;
                            tB_DATA_END_CUSTOMER.CREATED_TIME = DateTime.Now;
                            tB_DATA_END_CUSTOMER.LAST_UPDATED_BY = tB_DATA_CARD.CREATED_BY;
                            tB_DATA_END_CUSTOMER.LAST_UPDATED_TIME = DateTime.Now;
                            gateway.Save<TB_DATA_END_CUSTOMER>(tB_DATA_END_CUSTOMER, tran);
                        }
                        tB_DATA_CARD.END_CUSTOMER_ID = end_customer_id;
                    }
                    if (!String.IsNullOrEmpty(tB_DATA_CARD.BRAND_ID.Trim()))
                    {
                        brand_id = brandData.Select().Where(s => s["BRAND_NAME"].ToString() == tB_DATA_CARD.BRAND_ID.Trim()).Select(s => s["BRAND_ID"].ToString()).FirstOrDefault();
                        if (String.IsNullOrEmpty(brand_id))
                        {
                            TB_DATA_BRAND tB_DATA_BRAND = new TB_DATA_BRAND();
                            brand_id = Guid.NewGuid().ToString();
                            tB_DATA_BRAND.BRAND_ID = brand_id;
                            tB_DATA_BRAND.BRAND_NAME = tB_DATA_CARD.BRAND_ID.Trim();
                            tB_DATA_BRAND.CREATED_BY = tB_DATA_CARD.CREATED_BY;
                            tB_DATA_BRAND.CREATED_TIME = DateTime.Now;
                            tB_DATA_BRAND.LAST_UPDATED_BY = tB_DATA_CARD.CREATED_BY;
                            tB_DATA_BRAND.LAST_UPDATED_TIME = DateTime.Now;
                            gateway.Save<TB_DATA_BRAND>(tB_DATA_BRAND, tran);
                        }
                        tB_DATA_CARD.BRAND_ID = brand_id;
                    }
                    String cardID = gateway.Find<TB_DATA_CARD>(TB_DATA_CARD._.CARD_ICCID == tB_DATA_CARD.CARD_ICCID || TB_DATA_CARD._.CARD_NO == tB_DATA_CARD.CARD_NO).CARD_ID;
                    if (!String.IsNullOrEmpty(cardID))
                    {
                        tB_DATA_CARD.Attach();
                        tB_DATA_CARD.SetAllPropertiesAsModified();
                    }
                    gateway.Save<TB_DATA_CARD>(tB_DATA_CARD, tran);
                }
                String updateSql = @"update TB_DATA_AGENT set AGENT_CARD_NUMBER=T.card_count
                                    from (
                                        select count(1) card_count,AGENT_ID from TB_DATA_CARD
                                        GROUP BY AGENT_ID
                                    ) T where TB_DATA_AGENT.AGENT_ID=T.AGENT_ID";
                gateway.FromCustomSql(updateSql).SetTransaction(tran).ExecuteNonQuery();
                tran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Log.WriteLog(ex.Message);
                return 0;
            }
            finally
            {

                gateway.CloseTransaction(tran);
                tran.Dispose();
            }
        }
        /// <summary>
        /// 保存卡数据集合代理商
        /// </summary>
        /// <param name="tB_DATA_CARD_LIST"></param>
        /// <returns></returns>
        public int SaveImportCardForAgent(List<TB_DATA_CARD> tB_DATA_CARD_LIST)
        {
            Gateway gateway = Gateway.Default;
            DbTransaction tran = gateway.BeginTransaction();
            try
            {
                //取所有非删除的终端用户
                String endCustomSql = "select END_CUSTOMER_ID,END_CUSTOMER_NAME FROM [dbo].[TB_DATA_END_CUSTOMER] WHERE STATUS>-1";
                DataTable endCustomData = gateway.FromCustomSql(endCustomSql).ToDataSet().Tables[0];
                for (int i = 0; i < tB_DATA_CARD_LIST.Count; i++)
                {
                    TB_DATA_CARD tB_DATA_CARD = tB_DATA_CARD_LIST[i];
                    String cardID = gateway.Find<TB_DATA_CARD>(TB_DATA_CARD._.CARD_ICCID == tB_DATA_CARD.CARD_ICCID || TB_DATA_CARD._.CARD_NO == tB_DATA_CARD.CARD_NO).CARD_ID;
                    if (!String.IsNullOrEmpty(cardID))
                    {
                        String end_customer_id = String.Empty;
                        if (!String.IsNullOrEmpty(tB_DATA_CARD.END_CUSTOMER_ID.Trim()))
                        {
                            end_customer_id = endCustomData.Select().Where(s => s["END_CUSTOMER_NAME"].ToString() == tB_DATA_CARD.END_CUSTOMER_ID.Trim()).Select(s => s["END_CUSTOMER_ID"].ToString()).FirstOrDefault();
                            if (String.IsNullOrEmpty(end_customer_id))
                            {
                                TB_DATA_END_CUSTOMER tB_DATA_END_CUSTOMER = new TB_DATA_END_CUSTOMER();
                                end_customer_id = Guid.NewGuid().ToString();
                                tB_DATA_END_CUSTOMER.END_CUSTOMER_ID = end_customer_id;
                                tB_DATA_END_CUSTOMER.END_CUSTOMER_NAME = tB_DATA_CARD.END_CUSTOMER_ID.Trim();
                                tB_DATA_END_CUSTOMER.STATUS = 1;
                                tB_DATA_END_CUSTOMER.CREATED_BY = tB_DATA_CARD.CREATED_BY;
                                tB_DATA_END_CUSTOMER.CREATED_TIME = DateTime.Now;
                                tB_DATA_END_CUSTOMER.LAST_UPDATED_BY = tB_DATA_CARD.CREATED_BY;
                                tB_DATA_END_CUSTOMER.LAST_UPDATED_TIME = DateTime.Now;
                                gateway.Save<TB_DATA_END_CUSTOMER>(tB_DATA_END_CUSTOMER, tran);
                            }
                            tB_DATA_CARD.END_CUSTOMER_ID = end_customer_id;
                        }
                        tB_DATA_CARD.Attach();
                        tB_DATA_CARD.SetModifiedProperties(new Dictionary<string, object> {
                            {"END_CUSTOMER_ID",tB_DATA_CARD.END_CUSTOMER_ID},
                            {"STATUS",tB_DATA_CARD.STATUS},
                            {"SERVICE_PROVIDER",tB_DATA_CARD.SERVICE_PROVIDER},
                            {"REAL_NAME_PERSON",tB_DATA_CARD.REAL_NAME_PERSON},
                            {"REAL_NAME_ID_CARD",tB_DATA_CARD.REAL_NAME_ID_CARD},
                            {"LAST_UPDATED_BY",tB_DATA_CARD.LAST_UPDATED_BY},
                            {"LAST_UPDATED_TIME",tB_DATA_CARD.LAST_UPDATED_TIME}
                        });
                    }
                    gateway.Save<TB_DATA_CARD>(tB_DATA_CARD, tran);
                }
                tran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Log.WriteLog(ex.Message);
                return 0;
            }
            finally
            {

                gateway.CloseTransaction(tran);
                tran.Dispose();
            }
        }
        /// <summary>
        /// 删除卡
        /// </summary>
        /// <param name="Card_ID"></param>
        /// <returns></returns>
        public int DeleteCardInfo(String Card_ID) 
        {
            String sql = "DELETE FROM TB_DATA_CARD WHERE CARD_ID=@CARD_ID";
            try
            {
                Gateway.Default.FromCustomSql(sql).AddInputParameter("CARD_ID", DbType.String, Card_ID).ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 查询卡信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currPage"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IDictionary<String, Object> QueryCard(Int32 pageSize, Int32 currPage, IDictionary<String, String> where)
        {
            DataTable dtPage = null;
            Int32 totalRow = 0;
            Int32 totalPage = 0;

            string sqlCount = @"select  count(1) totalrow  ";
            string sql = @"SELECT [CARD_ID],a.[AGENT_ID],b.AGENT_NAME,a.[END_CUSTOMER_ID],c.END_CUSTOMER_NAME,a.[BRAND_ID],d.BRAND_NAME,
		                          [CARD_ICCID],[CARD_NO],[SERVICE_PROVIDER],a.[STATUS],[IS_COMPLAINT],[COMPLAINT_DATE],[COMPLAINT_URL],
                                  [REAL_NAME_PERSON],[REAL_NAME_ID_CARD],[DISTRIBUTION_DATE],a.[LAST_UPDATED_TIME]";
            string othersql = @" FROM [dbo].[TB_DATA_CARD] a
                           left join TB_DATA_AGENT b on a.AGENT_ID = b.AGENT_ID
                           left join TB_DATA_END_CUSTOMER c on a.[END_CUSTOMER_ID]=c.[END_CUSTOMER_ID]
                           left join TB_DATA_BRAND d on a.[BRAND_ID]=d.[BRAND_ID]
                           where 1=1";

            if (where.ContainsKey("CARD_NO"))
            {
                othersql = othersql + String.Format(" AND CARD_NO like @CARD_NO");
            }
            if (where.ContainsKey("BEGIN_NUM"))
            {
                othersql = othersql + String.Format(" AND Convert(int, RIGHT(CARD_ICCID,6))>=@BEGIN_NUM");
            }
            if (where.ContainsKey("END_NUM"))
            {
                othersql = othersql + String.Format(" AND Convert(int, RIGHT(CARD_ICCID,6))<=@END_NUM");
            }
            if (where.ContainsKey("AGENT_ID"))
            {
                othersql = othersql + String.Format(" AND a.AGENT_ID=@AGENT_ID");
            }
            if (where.ContainsKey("END_CUSTOMER_ID"))
            {
                othersql = othersql + String.Format(" AND a.END_CUSTOMER_ID]=@END_CUSTOMER_ID");
            }
            if (where.ContainsKey("BRAND_ID"))
            {
                othersql = othersql + String.Format(" AND a.BRAND_ID=@BRAND_ID");
            }
            if (where.ContainsKey("COMPLAINT_BEGIN_DATE"))
            {
                othersql = othersql + String.Format(" AND a.COMPLAINT_DATE>=@COMPLAINT_BEGIN_DATE");
            }
            if (where.ContainsKey("COMPLAINT_END_DATE"))
            {
                othersql = othersql + String.Format(" AND a.COMPLAINT_DATE<=@COMPLAINT_END_DATE");
            }
            if (where.ContainsKey("UPDATED_BEGIN_DATE"))
            {
                othersql = othersql + String.Format(" AND a.LAST_UPDATED_TIME>=@UPDATED_BEGIN_DATE");
            }
            if (where.ContainsKey("UPDATED_END_DATE"))
            {
                othersql = othersql + String.Format(" AND a.LAST_UPDATED_TIME<=@UPDATED_END_DATE");
            }
            if (where.ContainsKey("STATUS"))
            {
                othersql = othersql + String.Format(" AND a.STATUS=@STATUS");
            }
            sqlCount = sqlCount + othersql;
            sql = sql + othersql;

            //设定排序条件
            sql = sql + String.Format(" ORDER BY {0} {1} ", "a.CREATED_TIME", "DESC");

            CustomSqlSection customCount = Gateway.Default.FromCustomSql(sqlCount);
            CustomSqlSection custom = Gateway.Default.FromCustomSql(sql);

            if (where.ContainsKey("CARD_NO"))
            {
                customCount.AddInputParameter("AGENT_NAME", DbType.String, "%" + where["CARD_NO"] + "%");
                custom.AddInputParameter("AGENT_NAME", DbType.String, "%" + where["CARD_NO"] + "%");
            }
            if (where.ContainsKey("BEGIN_NUM"))
            {
                customCount.AddInputParameter("BEGIN_NUM", DbType.Int32, where["BEGIN_NUM"]);
                custom.AddInputParameter("BEGIN_NUM", DbType.Int32, where["BEGIN_NUM"]);
            }
            if (where.ContainsKey("END_NUM"))
            {
                customCount.AddInputParameter("END_NUM", DbType.Int32, where["END_NUM"]);
                custom.AddInputParameter("END_NUM", DbType.Int32, where["END_NUM"]);
            }
            if (where.ContainsKey("AGENT_ID"))
            {
                customCount.AddInputParameter("AGENT_ID", DbType.String, where["AGENT_ID"]);
                custom.AddInputParameter("AGENT_ID", DbType.String, where["AGENT_ID"]);
            }
            if (where.ContainsKey("END_CUSTOMER_ID"))
            {
                customCount.AddInputParameter("END_CUSTOMER_ID", DbType.String, where["END_CUSTOMER_ID"]);
                custom.AddInputParameter("END_CUSTOMER_ID", DbType.String, where["END_CUSTOMER_ID"]);
            }
            if (where.ContainsKey("BRAND_ID"))
            {
                customCount.AddInputParameter("BRAND_ID", DbType.String, where["BRAND_ID"]);
                custom.AddInputParameter("BRAND_ID", DbType.String, where["BRAND_ID"]);
            }
            if (where.ContainsKey("COMPLAINT_BEGIN_DATE"))
            {
                customCount.AddInputParameter("COMPLAINT_BEGIN_DATE", DbType.String, where["COMPLAINT_BEGIN_DATE"]);
                custom.AddInputParameter("COMPLAINT_BEGIN_DATE", DbType.String, where["COMPLAINT_BEGIN_DATE"]);
            }
            if (where.ContainsKey("COMPLAINT_END_DATE"))
            {
                customCount.AddInputParameter("COMPLAINT_END_DATE", DbType.String, where["COMPLAINT_END_DATE"]);
                custom.AddInputParameter("COMPLAINT_END_DATE", DbType.String, where["COMPLAINT_END_DATE"]);
            }
            if (where.ContainsKey("UPDATED_BEGIN_DATE"))
            {
                customCount.AddInputParameter("UPDATED_BEGIN_DATE", DbType.String, where["UPDATED_BEGIN_DATE"]);
                custom.AddInputParameter("UPDATED_BEGIN_DATE", DbType.String, where["UPDATED_BEGIN_DATE"]);
            }
            if (where.ContainsKey("UPDATED_END_DATE"))
            {
                customCount.AddInputParameter("UPDATED_END_DATE", DbType.String, where["UPDATED_END_DATE"]);
                custom.AddInputParameter("UPDATED_END_DATE", DbType.String, where["UPDATED_END_DATE"]);
            }
            if (where.ContainsKey("STATUS"))
            {
                customCount.AddInputParameter("STATUS", DbType.String, where["STATUS"]);
                custom.AddInputParameter("STATUS", DbType.String, where["STATUS"]);
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
        /// 导出卡信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currPage"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable ExportCard(Int32 pageSize, Int32 currPage, IDictionary<String, String> where)
        {
            string sql = @"SELECT d.BRAND_NAME AS '品牌',[SERVICE_PROVIDER] AS '运营商',[CARD_ICCID] AS 'ICCID',[CARD_NO] AS '卡号',b.AGENT_NAME AS '代理商',c.END_CUSTOMER_NAME AS '终端客户',
		                          CASE when a.[STATUS]=-1 THEN '违规封停'  when a.[STATUS]=1 THEN '已激活' ELSE '未激活' END AS '状态',CASE WHEN [IS_COMPLAINT]=1 THEN '已被投诉' ELSE '未投诉' END AS '是否被投诉',
                                 [COMPLAINT_DATE] AS '投诉时间',[REAL_NAME_PERSON] AS '实名人',[REAL_NAME_ID_CARD] AS '身份证',[DISTRIBUTION_DATE] AS '分配时间',a.[LAST_UPDATED_TIME] AS '修改时间'";
            string othersql = @" FROM [dbo].[TB_DATA_CARD] a
                           left join TB_DATA_AGENT b on a.AGENT_ID = b.AGENT_ID
                           left join TB_DATA_END_CUSTOMER c on a.[END_CUSTOMER_ID]=c.[END_CUSTOMER_ID]
                           left join TB_DATA_BRAND d on a.[BRAND_ID]=d.[BRAND_ID]
                           where 1=1";

            if (where.ContainsKey("CARD_NO"))
            {
                othersql = othersql + String.Format(" AND CARD_NO like @CARD_NO");
            }
            if (where.ContainsKey("BEGIN_NUM"))
            {
                othersql = othersql + String.Format(" AND Convert(int, RIGHT(CARD_ICCID,6))>=@BEGIN_NUM");
            }
            if (where.ContainsKey("END_NUM"))
            {
                othersql = othersql + String.Format(" AND Convert(int, RIGHT(CARD_ICCID,6))<=@END_NUM");
            }
            if (where.ContainsKey("AGENT_ID"))
            {
                othersql = othersql + String.Format(" AND a.AGENT_ID=@AGENT_ID");
            }
            if (where.ContainsKey("END_CUSTOMER_ID"))
            {
                othersql = othersql + String.Format(" AND a.END_CUSTOMER_ID]=@END_CUSTOMER_ID");
            }
            if (where.ContainsKey("BRAND_ID"))
            {
                othersql = othersql + String.Format(" AND a.BRAND_ID=@BRAND_ID");
            }
            if (where.ContainsKey("COMPLAINT_BEGIN_DATE"))
            {
                othersql = othersql + String.Format(" AND a.COMPLAINT_DATE>=@COMPLAINT_BEGIN_DATE");
            }
            if (where.ContainsKey("COMPLAINT_END_DATE"))
            {
                othersql = othersql + String.Format(" AND a.COMPLAINT_DATE<=@COMPLAINT_END_DATE");
            }
            if (where.ContainsKey("UPDATED_BEGIN_DATE"))
            {
                othersql = othersql + String.Format(" AND a.LAST_UPDATED_TIME>=@UPDATED_BEGIN_DATE");
            }
            if (where.ContainsKey("UPDATED_END_DATE"))
            {
                othersql = othersql + String.Format(" AND a.LAST_UPDATED_TIME<=@UPDATED_END_DATE");
            }
            if (where.ContainsKey("STATUS"))
            {
                othersql = othersql + String.Format(" AND a.STATUS=@STATUS");
            }
            sql = sql + othersql;

            //设定排序条件
            sql = sql + String.Format(" ORDER BY {0} {1} ", "a.CREATED_TIME", "DESC");

            CustomSqlSection custom = Gateway.Default.FromCustomSql(sql);

            if (where.ContainsKey("CARD_NO"))
            {
                custom.AddInputParameter("AGENT_NAME", DbType.String, "%" + where["CARD_NO"] + "%");
            }
            if (where.ContainsKey("BEGIN_NUM"))
            {
                custom.AddInputParameter("BEGIN_NUM", DbType.Int32, where["BEGIN_NUM"]);
            }
            if (where.ContainsKey("END_NUM"))
            {
                custom.AddInputParameter("END_NUM", DbType.Int32, where["END_NUM"]);
            }
            if (where.ContainsKey("AGENT_ID"))
            {
                custom.AddInputParameter("AGENT_ID", DbType.String, where["AGENT_ID"]);
            }
            if (where.ContainsKey("END_CUSTOMER_ID"))
            {
                custom.AddInputParameter("END_CUSTOMER_ID", DbType.String, where["END_CUSTOMER_ID"]);
            }
            if (where.ContainsKey("BRAND_ID"))
            {
                custom.AddInputParameter("BRAND_ID", DbType.String, where["BRAND_ID"]);
            }
            if (where.ContainsKey("COMPLAINT_BEGIN_DATE"))
            {
                custom.AddInputParameter("COMPLAINT_BEGIN_DATE", DbType.String, where["COMPLAINT_BEGIN_DATE"]);
            }
            if (where.ContainsKey("COMPLAINT_END_DATE"))
            {
                custom.AddInputParameter("COMPLAINT_END_DATE", DbType.String, where["COMPLAINT_END_DATE"]);
            }
            if (where.ContainsKey("UPDATED_BEGIN_DATE"))
            {
                custom.AddInputParameter("UPDATED_BEGIN_DATE", DbType.String, where["UPDATED_BEGIN_DATE"]);
            }
            if (where.ContainsKey("UPDATED_END_DATE"))
            {
                custom.AddInputParameter("UPDATED_END_DATE", DbType.String, where["UPDATED_END_DATE"]);
            }
            if (where.ContainsKey("STATUS"))
            {
                custom.AddInputParameter("STATUS", DbType.String, where["STATUS"]);
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
