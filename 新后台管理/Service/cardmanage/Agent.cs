using Common;
using NBear.Data;
using SYSEntity;
using System;
using System.Collections.Generic;
using System.Data;
namespace Service
{
    public class Agent
    {
        /// <summary>
        /// 保存代理商
        /// </summary>
        /// <param name="tB_DATA_AGENT">代理商对象</param>
        /// <returns></returns>
        public int SaveAgent(TB_DATA_AGENT tB_DATA_AGENT)
        {
            try
            {
                //判断输入的账户名是否存在用户信息
                int userID = 0;
                if (!String.IsNullOrEmpty(tB_DATA_AGENT.AGENT_ACCOUNT))
                {
                    var obj = Gateway.Default.Find<Xt_zhb>(Xt_zhb._.Zh == tB_DATA_AGENT.AGENT_ACCOUNT);
                    if (obj != null)
                    {
                        userID = obj.Id;
                    }
                }
                //如果不是修改代理商，需先创建指定用户
                if (!tB_DATA_AGENT.IsAttached())
                {
                    if (userID > 0)
                    {
                        return -1;
                    }
                    Xt_zhb xt_Zhb = new Xt_zhb();
                    xt_Zhb.Zh = tB_DATA_AGENT.AGENT_ACCOUNT;
                    xt_Zhb.Mm = "E10ADC3949BA59ABBE56E057F20F883E";
                    xt_Zhb.Dh = tB_DATA_AGENT.AGENT_TEL;
                    xt_Zhb.Lxr = tB_DATA_AGENT.AGENT_NAME;
                    xt_Zhb.Jsid = 2;
                    xt_Zhb.Createtime = DateTime.Now;
                    Gateway.Default.Save<Xt_zhb>(xt_Zhb);
                    userID = Gateway.Default.Find<Xt_zhb>(Xt_zhb._.Zh == tB_DATA_AGENT.AGENT_ACCOUNT).Id;
                    tB_DATA_AGENT.USER_ID = userID.ToString();
                }
                else
                {
                    tB_DATA_AGENT.USER_ID = Gateway.Default.Find<TB_DATA_AGENT>(TB_DATA_AGENT._.AGENT_ID == tB_DATA_AGENT.AGENT_ID).USER_ID;
                    if (String.IsNullOrEmpty(tB_DATA_AGENT.USER_ID))
                    {
                        Xt_zhb xt_Zhb = new Xt_zhb();
                        xt_Zhb.Zh = tB_DATA_AGENT.AGENT_ACCOUNT;
                        xt_Zhb.Mm = "E10ADC3949BA59ABBE56E057F20F883E";
                        xt_Zhb.Dh = tB_DATA_AGENT.AGENT_TEL;
                        xt_Zhb.Lxr = tB_DATA_AGENT.AGENT_NAME;
                        xt_Zhb.Jsid = 2;
                        xt_Zhb.Createtime = DateTime.Now;
                        Gateway.Default.Save<Xt_zhb>(xt_Zhb);
                        userID = Gateway.Default.Find<Xt_zhb>(Xt_zhb._.Zh == tB_DATA_AGENT.AGENT_ACCOUNT).Id;
                        tB_DATA_AGENT.USER_ID = userID.ToString();
                    }
                    else if (userID.ToString() != tB_DATA_AGENT.USER_ID)
                    {
                        return -1;
                    }
                    else
                    {
                        Gateway.Default.FromCustomSql("update Xt_zhb set Zh=@USER_NAME where id=@id")
                               .AddInputParameter("USER_NAME", System.Data.DbType.String, tB_DATA_AGENT.AGENT_ACCOUNT)
                               .AddInputParameter("id", System.Data.DbType.String, userID.ToString()).ExecuteNonQuery();
                    }
                }
                Gateway.Default.Save<TB_DATA_AGENT>(tB_DATA_AGENT);
                return 1;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return 0;
            }

        }
        /// <summary>
        /// 修改代理商状态
        /// </summary>
        /// <param name="Agent_ID">代理商ID</param>
        /// <param name="Updated_By">更新人</param>
        /// <param name="STATUS">状态</param>
        /// <returns></returns>
        public int UpdateAgentStatus(String Agent_ID, String Updated_By, int STATUS)
        {
            String sql = "update TB_DATA_AGENT set STATUS=@STATUS,LAST_UPDATED_BY=@LAST_UPDATED_BY,LAST_UPDATED_TIME=GETDATE() where AGENT_ID=@AGENT_ID";
            try
            {
                Gateway.Default.FromCustomSql(sql)
                    .AddInputParameter("STATUS", System.Data.DbType.Int32, STATUS)
                    .AddInputParameter("LAST_UPDATED_BY", System.Data.DbType.String, Updated_By)
                    .AddInputParameter("AGENT_ID", System.Data.DbType.String, Agent_ID)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 批量删除代理商
        /// </summary>
        /// <param name="Agent_IDs"></param>
        /// <param name="Updated_By"></param>
        /// <param name="STATUS"></param>
        /// <returns></returns>
        public int DeleteAgent(String Agent_IDs, String Updated_By)
        {
            String sql = "update TB_DATA_AGENT set STATUS=-1,LAST_UPDATED_BY=@LAST_UPDATED_BY,LAST_UPDATED_TIME=GETDATE() where AGENT_ID in("+ "'" + String.Join("','", Agent_IDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) + "'" + ")";
            try
            {
                Gateway.Default.FromCustomSql(sql)
                    .AddInputParameter("LAST_UPDATED_BY", System.Data.DbType.String, Updated_By)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 查询一个代理商信息
        /// </summary>
        /// <param name="Agent_ID"></param>
        /// <returns></returns>
        public DataTable QueryOneAgentInfo(String Agent_ID)
        {
            String SQL = @"SELECT [AGENT_ID],[USER_ID],[AGENT_NO],[AGENT_NAME],[AGENT_ACCOUNT],[AGENT_CARD_NUMBER],[AGENT_AREA],[AGENT_CONTACTS],[AGENT_TEL],[AGENT_BUSINESS_LICENSE]
                            ,[AGENT_ID_CARD],[AGENT_COMMITMENT_LETTER],[STATUS] FROM [dbo].[TB_DATA_AGENT] WHERE AGENT_ID=@AGENT_ID";

            return Gateway.Default.FromCustomSql(SQL).AddInputParameter("AGENT_ID", DbType.String, Agent_ID).ToDataSet().Tables[0];

        }

        /// <summary>
        /// 查询代理商
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currPage"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IDictionary<String, Object> QueryAgent(Int32 pageSize, Int32 currPage, IDictionary<String, String> where)
        {
            DataTable dtPage = null;
            Int32 totalRow = 0;
            Int32 totalPage = 0;

            string sqlCount = @"select  count(1) totalrow  ";
            string sql = @"SELECT [AGENT_ID],[USER_ID],[AGENT_NO],[AGENT_NAME],[AGENT_CARD_NUMBER],[AGENT_AREA],[AGENT_CONTACTS],[AGENT_TEL],[AGENT_BUSINESS_LICENSE],[AGENT_ID_CARD]
                            ,[AGENT_COMMITMENT_LETTER],[STATUS],[CREATED_TIME]";
            string othersql = " FROM [dbo].[TB_DATA_AGENT] WHERE STATUS>-1";

            if (where.ContainsKey("AGENT_NAME"))
            {
                othersql = othersql + String.Format(" AND AGENT_NAME like @AGENT_NAME");
            }
            sqlCount = sqlCount + othersql;
            sql = sql + othersql;

            //设定排序条件
            sql = sql + String.Format(" ORDER BY {0} {1} ", "CREATED_TIME", "DESC");

            CustomSqlSection customCount = Gateway.Default.FromCustomSql(sqlCount);
            CustomSqlSection custom = Gateway.Default.FromCustomSql(sql);

            if (where.ContainsKey("AGENT_NAME"))
            {
                customCount.AddInputParameter("AGENT_NAME", DbType.String, "%" + where["AGENT_NAME"] + "%");
                custom.AddInputParameter("AGENT_NAME", DbType.String, "%" + where["AGENT_NAME"] + "%");
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
        public DataTable ExportAgent(IDictionary<String, String> where)
        {
            string sql = @"SELECT [AGENT_NO] AS '编号',[AGENT_NAME] AS '代理商',[AGENT_CARD_NUMBER] AS '卡数量',[AGENT_AREA] AS '地区',[AGENT_CONTACTS] AS '联系人',[AGENT_TEL] AS '联系电话'
                            ,CASE WHEN [STATUS]=1 THEN '启用' WHEN [STATUS]=0 THEN '禁用' ELSE '删除' END AS '状态',[CREATED_TIME] AS '创建时间'";
            string othersql = " FROM [dbo].[TB_DATA_AGENT] WHERE STATUS>-1";

            if (where.ContainsKey("AGENT_NAME"))
            {
                othersql = othersql + String.Format(" AND AGENT_NAME like @AGENT_NAME");
            }
            sql = sql + othersql;

            //设定排序条件
            sql = sql + String.Format(" ORDER BY {0} {1} ", "CREATED_TIME", "DESC");

            CustomSqlSection custom = Gateway.Default.FromCustomSql(sql);

            if (where.ContainsKey("AGENT_NAME"))
            {
                custom.AddInputParameter("AGENT_NAME", DbType.String, "%" + where["AGENT_NAME"] + "%");
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
