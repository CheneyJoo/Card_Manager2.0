using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class JobHelper
    {
        /// <summary>
        /// 执行一个job, 使用DateTime时间执行
        /// </summary>
        /// <param name="strJobKey">job key</param>
        /// <param name="strTriggerKey">trigger key</param>
        /// <param name="parameterList">传入参数</param>
        /// <param name="dtExecuteTime">执行时间</param>
        public static void Execute<T>(string strJobKey, string strTriggerKey, IDictionary<string, object> parameterList, DateTime dtExecuteTime) where T : IJob
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();


            JobKey jobKey = new JobKey(strJobKey);
            sched.DeleteJob(jobKey);
            TriggerKey triggerKey = new TriggerKey(strTriggerKey);

            IJobDetail job = JobBuilder.Create<T>()
                                    .WithIdentity(jobKey)
                                    .Build();

            foreach (var item in parameterList)
            {
                job.JobDataMap.Add(item.Key, item.Value);
            }

            DateTimeOffset runTime = DateBuilder.DateOf(dtExecuteTime.Hour,
                                                        dtExecuteTime.Minute,
                                                        dtExecuteTime.Second,
                                                        dtExecuteTime.Day,
                                                        dtExecuteTime.Month,
                                                        dtExecuteTime.Year);

            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                              .WithIdentity(triggerKey)
                                              .StartAt(runTime)
                                              .Build();

            sched.ScheduleJob(job, trigger);
            sched.Start();

        }

        /// <summary>
        /// 根据cron表达式定时执行一个job
        /// </summary>
        /// <param name="strJobKey">job key</param>
        /// <param name="strTriggerKey">trigger key</param>
        /// <param name="parameterList">传入参数</param>
        /// <param name="cron">cron 表达式</param>
        public static void Execute<T>(string strJobKey, string strTriggerKey, IDictionary<string, object> parameterList, string cron) where T : IJob
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();


            JobKey jobKey = new JobKey(strJobKey);
            sched.DeleteJob(jobKey);
            TriggerKey triggerKey = new TriggerKey(strTriggerKey);

            IJobDetail job = JobBuilder.Create<T>()
                                    .WithIdentity(jobKey)
                                    .Build();

            if (null != parameterList)
            {
                foreach (var item in parameterList)
                {
                    job.JobDataMap.Add(item.Key, item.Value);
                }
            }


            // 触发器  
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                                    .WithIdentity(triggerKey)
                                                                    .WithCronSchedule(cron)
                                                                    .Build();
            sched.ScheduleJob(job, trigger);
            sched.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTriggerKey">触发器Key</param>
        /// <returns></returns>
        public static bool CheckExistsTriggerKey(string strTriggerKey)
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();

            TriggerKey triggerKey = new TriggerKey(strTriggerKey);
            return sched.CheckExists(triggerKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTriggerKey">触发器Key</param>
        /// <returns></returns>
        public static bool CheckExistsJobKey(string strJobKey)
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();

            JobKey jobKey = new JobKey(strJobKey);
            return sched.CheckExists(jobKey);
        }

        #region 修改job时间
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTriggerKey">触发器Key</param>
        /// <param name="cron">新的执行时间 cron表达式</param>
        public static void ModifyJobTime(string strTriggerKey, string cron)
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();

            TriggerKey triggerKey = new TriggerKey(strTriggerKey);
            // 触发器  
            ICronTrigger newTrigger = (ICronTrigger)TriggerBuilder.Create()
                                                                    .WithIdentity(triggerKey)
                                                                    .WithCronSchedule(cron)
                                                                    .Build();

            sched.RescheduleJob(triggerKey, newTrigger);
            //sched.Start();
            //bool isStart = sched.IsStarted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTriggerKey">触发器Key</param>
        /// <param name="dtExecuteTime">执行Job的具体时间,只执行一次</param>
        public static void ModifyJobTime(string strTriggerKey, DateTime dtExecuteTime)
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();

            TriggerKey triggerKey = new TriggerKey(strTriggerKey);

            DateTimeOffset runTime = DateBuilder.DateOf(dtExecuteTime.Hour,
                                                        dtExecuteTime.Minute,
                                                        dtExecuteTime.Second,
                                                        dtExecuteTime.Day,
                                                        dtExecuteTime.Month,
                                                        dtExecuteTime.Year);

            ISimpleTrigger newTrigger = (ISimpleTrigger)TriggerBuilder.Create()
                                                .WithIdentity(triggerKey)
                                                .StartAt(runTime)
                                                .Build();
            sched.RescheduleJob(triggerKey, newTrigger);
        }

        #endregion

        /// <summary>
        /// 删除job
        /// </summary>
        /// <param name="strJobKey">要删除的Job Key</param>
        /// <returns></returns>
        public static bool DeleteJob(string strJobKey)
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();


            JobKey jobKey = new JobKey(strJobKey);
            return sched.DeleteJob(jobKey);
        }
    }
}
