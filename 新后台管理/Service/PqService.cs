using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PqService
    {
        /// <summary>
        /// 所选日期判断是否能预约
        /// </summary>
        /// <param name="ydjh"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool GetScheduleResult(string yybh, string qybh, string ydjh, string rq, string sj)
        {
            DateTime rq_date = Convert.ToDateTime(rq);
            var kssj = sj.Split('-')[0];
            DateTime sj_date = Convert.ToDateTime(rq + " " + kssj);

            var ygxx = new QyygjbxxService().GetModel(yybh, ydjh);

            var jbsz = new PqJbszService().GetModel(yybh);
            var qypq = new PqQyszService().GetModelByDate(yybh, qybh, rq_date);
            var tjrq = new PqTjrqService().GetModel(yybh, qybh, rq_date);
            var tjsj = new PqTjsjService().GetListByRq(qybh, yybh, rq_date).FirstOrDefault(x => x.kssj <= sj_date && sj_date < x.jssj);
            var yyList = new DdJbxxService().GetQyYyListByDate(yybh, qybh, rq_date);//每天预约人数


            if (jbsz == null || jbsz.tjyl <= yyList.Count)
            {
                return false;
            }

            var weekDay = Convert.ToInt32(rq_date.DayOfWeek).ToString();
            if ((jbsz.xxr.Contains(weekDay) || jbsz.ztyy.Contains(rq)) && !jbsz.tsky.Contains(rq))
            {
                return false;
            }
            if (tjrq == null || tjrq.flag > 0 || tjrq.tjrs <= yyList.Count)
            {
                return false;
            }
            if (tjsj == null)
            {
                return false;
            }
            var sj_yyList = yyList.Where(x => x.yykssj <= sj_date && sj_date < x.yyjssj);
            if (tjsj.tjrs <= sj_yyList.Count())
            {
                return false;
            }

            if (qypq != null)
            {
                var pjtcList = new PqQyszService().GetPjtc(qypq.pqbh);//瓶颈套餐
                var pjtcModel = pjtcList.FirstOrDefault(x => x.tcbh == ygxx.tcbh);
                if (pjtcModel != null)
                {
                    var pjtcs = pjtcList.Where(x => x.fzbh == pjtcModel.pqbh).Select(a => a.tcbh).ToArray(); ;
                    var tcyyrs = yyList.Where(a => pjtcs.Contains(a.tcid)).Count();//套餐体检人数
                    if (tcyyrs >= pjtcModel.tjrs)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
