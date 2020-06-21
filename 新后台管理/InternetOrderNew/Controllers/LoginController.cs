using Common;
using MK.Common;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InternetOrder.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.referUrl = Request["referUrl"];
            return View();
        }
        public string GetClientIPv4Address()
        {
            string ipv4 = String.Empty;

            foreach (IPAddress ip in Dns.GetHostAddresses(GetClientIP()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            if (ipv4 != String.Empty)
            {
                return ipv4;
            }
            // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，
            // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
            foreach (IPAddress ip in Dns.GetHostEntry(GetClientIP()).AddressList)          
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            return ipv4;
        }
        public string GetClientIP()
        {
            if (null == HttpContext.Request.ServerVariables["HTTP_VIA"])
            {
                return HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                return HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }
        public JsonResult LoginPost(string YHM, string MM, string YZM)
        {
            if (String.IsNullOrEmpty(YHM))
            {
                return Json(new { flag = false, msg = "请输入用户名" });
            }
            if (String.IsNullOrEmpty(MM))
            {
                return Json(new { flag = false, msg = "请输入密码" });
            }
            if (String.IsNullOrEmpty(YZM))
            {
                return Json(new { flag = false, msg = "请输入验证码" });
            }
            if (Session["ImgCode"] == null)
            {
                return Json(new { flag = false, msg = "验证码错误" });
            }
            if (!YZM.ToUpper().Equals(Session["ImgCode"].ToString().ToUpper()))
            {
                return Json(new { flag = false, msg = "验证码错误" });
            }
            try
            {
              string   mm = md5.to32MD5(MM);

          

                object IP_Visit = Session["IP_Visit"];//来访IP
                string IP_Submit = GetClientIPv4Address();//提交IP
                object IP_Num = Session["IP_Num"];//来访ip访问次数
                Log.WriteLog("来访IP:" + IP_Visit);
                if (IP_Visit == null && IP_Num == null)
                {
                    Session["IP_Num"] = "1";
                    Session["IP_Visit"] = IP_Submit;
                }
                else if (IP_Visit.ToString() == IP_Submit && IP_Num.ToString() == "1")
                {
                    Session["IP_Num"] = "2";
                    Session["IP_Visit"] = IP_Submit;
                }
                else if (IP_Visit.ToString() == IP_Submit && IP_Num.ToString() == "2")
                {
                    Session["IP_Num"] = "3";
                    Session["IP_Visit"] = IP_Submit;
                }
                else if (IP_Visit.ToString() == IP_Submit && int.Parse((IP_Num == null ? 0 : IP_Num).ToString()) >= 300)
                {
                    Log.WriteLog("登录失败，来访IP:" + IP_Submit);
                    return Json(new { flag = false, msg = "你的IP访问次数过多，请换台电脑登录" }, JsonRequestBehavior.AllowGet);
                }
                FormsAuthentication.SetAuthCookie(YHM, false);
                XtZhbModel yy = new XtzhbService().Login(YHM, mm,1);
                if (yy != null)
                {
                    GLYadmin.GLYLoginIn(yy.zh,yy.yybh, yy.lxr, yy.jsid,yy.id);

                    FormsAuthentication.SetAuthCookie(yy.id.ToString(), false);

                    Log.WriteLog(YHM + ":登录成功！时间：" + System.DateTime.Now);               
                    return Json(new { flag = true, msg = "登陆成功" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { flag = false, msg = "用户名或者密码错误！" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(YHM + ":登录失败！：" + ex.Message);
                return Json(new { flag = false, msg = "登陆失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LogOut()
        {
            GLYadmin.GLYLoginOut();
            return RedirectToAction("Index", "Login");
         
        }

        public ActionResult ZzrmyyTest()
        {
            JsddService jsddService = new JsddService();
            DdyymxbService ddService = new DdyymxbService();
            string ddbh = new DdJbxxService().GetDdbh();
            Model.Api.PreOrder model = new Model.Api.PreOrder();
           
            model.appointmentDate = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
            model.appointmentPackageCode = "18984";
            model.customerAge = 30;
            model.customerBirthday = "1993-01-05";
            model.customerGender = 1;
            model.customerIDCard = "500236198711174413";
            model.customerIDType = 1;
            model.customerMedicalStatus = 0;
            model.customerMobilePhone = GetTel();
            model.customerName = "宋西德";
            model.ddJe = (decimal)687.5;
            model.deptID = "0025";
            model.yybh = "YY201512241012120001";
            model.interfaceNum = 1;
            model.outOrderCode = ddbh;

            ddService.CreateOrder(model, "肿瘤筛查抽血组合精英", ddbh, "康康网");
            return Content(ddbh);

        }

        /// <summary>
        /// 制造测试订单
        /// </summary>
        /// <returns></returns>
        public ActionResult ZcOrder()
        {
            JsddService jsddService = new JsddService();
            DdyymxbService ddService = new DdyymxbService();
            string sql = @"delete from qd_jsjbxxmx where jbxxid in (select id from qd_jsjbxx where yybh='YY201603111349440001');
            delete from  qd_jsjbxx where yybh = 'YY201603111349440001';
            delete from  qd_tksqjlb where ddbh in (select ddbh from dd_jbxx where yybh = 'YY201603111349440001');
            delete from dd_zhxm where ddbh in (select ddbh from dd_jbxx where yybh = 'YY201603111349440001');
            delete from dd_jbxx where yybh = 'YY201603111349440001' and ddly=0";
            ddService.Test(sql);


            //过去订单
            List<string> lstDdbh = new List<string>();

            ///自营
            for (int i = 0; i < 3000; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 10;
                model.appointmentDate = DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd") + " 08:00";
                model.appointmentPackageCode = "000054";
                model.customerAge = 30;
                model.customerBirthday = "1990-01-05";
                model.customerGender = 1;
                model.customerIDCard = "500236199001052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 741;
                model.deptID = "0026";
                model.yybh = "YY201603111349440001";
                model.interfaceNum = 6;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "白领精英A体检", ddbh, "医院公众号");

                if (i % 3 == 1)
                {
                    ddService.UpdatOrderdjlsh(ddbh, ddbh);
                }
                else
                {
                    ddService.UpdatOrderdaojiantest(ddbh, DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd"));
                    if (i % 7 == 0)
                    {
                        lstDdbh.Add(ddbh);
                    }
                }
                if (i % 100 == 0)//退款
                {
                    QuDaoTkModel td = new QuDaoTkModel();
                    td.ddbh = ddbh;
                    td.tkyy = "测试订单，申请退款";
                    td.tkje = 25;
                    td.sqrid = 14;
                    string msg = "";
                    new QuDaoTkService().QdTkSq(td, ref msg);
                }
                if (i % 200 == 0)//退款审核
                {
                    new QuDaoTkService().UpdateTkTy(ddbh, 25, 11);
                }
            }
            ddService.Test("update dd_jbxx set ysjsj=90 where yybh='YY201603111349440001' and tcid='000054'");

            //结算申请
            int count = lstDdbh.Count / 5;
            for (int i = 0; i < count; i++)
            {

                List<string> li = lstDdbh.Skip((count - 1) * 5).Take(5).ToList<string>();
                jsddService.PostToCalculate(li, "YY201603111349440001", 6, 11);
            }
            //康康
            for (int i = 0; i < 3000; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 10;
                model.appointmentDate = DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd") + " 08:00";
                model.appointmentPackageCode = "000044";
                model.customerAge = 30;
                model.customerBirthday = "1993-01-05";
                model.customerGender = 1;
                model.customerIDCard = "410185199301052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 425;
                model.deptID = "0025";
                model.yybh = "YY201603111349440001";
                model.interfaceNum = 1;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "	中年体检", ddbh, "康康体检网");
                if (i % 3 == 1)
                {
                    ddService.UpdatOrderdjlsh(ddbh, ddbh);
                }
                else
                {
                    ddService.UpdatOrderdaojiantest(ddbh, DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd"));
                }

                if (i % 100 == 0)//退款
                {
                    QuDaoTkModel td = new QuDaoTkModel();
                    td.ddbh = ddbh;
                    td.tkyy = "测试订单，申请退款";
                    td.tkje = 25;
                    td.sqrid = 14;
                    string msg = "";
                    new QuDaoTkService().QdTkSq(td, ref msg);
                }
                if (i % 200 == 0)//退款审核
                {
                    new QuDaoTkService().UpdateTkTy(ddbh, 25, 11);
                }
            }
            ddService.Test("update dd_jbxx set ysjsj=400 where yybh='YY201603111349440001' and tcid='000044'");

            //未来订单
            ///自营
            for (int i = 0; i < 300; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 10;
                model.appointmentDate = DateTime.Now.AddDays(j).ToString("yyyy-MM-dd") + " 08:00";
                model.appointmentPackageCode = "000055";
                model.customerAge = 30;
                model.customerBirthday = "1990-01-05";
                model.customerGender = 1;
                model.customerIDCard = "500236199001052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 76;
                model.deptID = "0026";
                model.yybh = "YY201603111349440001";

                model.interfaceNum = 6;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "白领精英B体检", ddbh, "医院公众号");

                ddService.UpdatOrderdjlsh(ddbh, ddbh);


            }
            ddService.Test("update dd_jbxx set ysjsj=70 where yybh='YY201603111349440001' and tcid='000055'");

            //康康
            for (int i = 0; i < 300; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 10;
                model.appointmentDate = DateTime.Now.AddDays(j).ToString("yyyy-MM-dd") + " 08:00";
                model.appointmentPackageCode = "000046";
                model.customerAge = 30;
                model.customerBirthday = "1993-01-05";
                model.customerGender = 1;
                model.customerIDCard = "410185199301052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 63;
                model.deptID = "0025";
                model.yybh = "YY201603111349440001";
                model.interfaceNum = 1;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "糖尿病专项体检", ddbh, "康康体检网");
                ddService.UpdatOrderdjlsh(ddbh, ddbh);

            }

            ddService.Test("update dd_jbxx set ysjsj=58 where yybh='YY201603111349440001' and tcid='000046'");
         
            return Content("成功");
        }

        /// <summary>
        /// 制造测试订单，阿里，腾讯，京东
        /// </summary>
        /// <returns></returns>
        public void ZcOrderOther()
        {
            JsddService jsddService = new JsddService();
            DdyymxbService ddService = new DdyymxbService();
       
            //过去订单
            List<string> lstDdbh = new List<string>();

            ///阿里
            for (int i = 0; i < 10012; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 100;
                model.appointmentDate = DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd");
                model.appointmentPackageCode = "000019";
                model.customerAge = 30;
                model.customerBirthday = "1990-01-05";
                model.customerGender = 1;
                model.customerIDCard = "500236199001052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 1032;
                model.deptID = "0007";
                model.yybh = "YY201603111349440001";
                model.interfaceNum = 8;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "体检C套餐", ddbh, "阿里健康");

                if (i % 3 == 1)
                {
                    ddService.UpdatOrderdjlsh(ddbh, ddbh);
                }
                else
                {
                    ddService.UpdatOrderdaojiantest(ddbh, DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd"));
                    if (i % 7 == 0)
                    {
                        lstDdbh.Add(ddbh);
                    }
                }
                if (i % 100 == 0)//退款
                {
                    QuDaoTkModel td = new QuDaoTkModel();
                    td.ddbh = ddbh;
                    td.tkyy = "测试订单，申请退款";
                    td.tkje = 25;
                    td.sqrid = 14;
                    string msg = "";
                    new QuDaoTkService().QdTkSq(td, ref msg);
                }
                if (i % 200 == 0)//退款审核
                {
                    new QuDaoTkService().UpdateTkTy(ddbh, 25, 11);
                }
            }
            ddService.Test("update dd_jbxx set ysjsj=995 where yybh='YY201603111349440001' and tcid='000019'");

            //结算申请
            int count = lstDdbh.Count / 5;
            for (int i = 0; i < count; i++)
            {

                List<string> li = lstDdbh.Skip((count - 1) * 5).Take(5).ToList<string>();
                jsddService.PostToCalculate(li, "YY201603111349440001", 8, 11);
            }
            //腾讯
            for (int i = 0; i < 1000; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 10;
                model.appointmentDate = DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd");
                model.appointmentPackageCode = "000022";
                model.customerAge = 30;
                model.customerBirthday = "1993-01-05";
                model.customerGender = 1;
                model.customerIDCard = "410185199301052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 854;
                model.deptID = "0008";
                model.yybh = "YY201603111349440001";
                model.interfaceNum = 9;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "腾讯C套餐", ddbh, "腾讯健康");
                if (i % 3 == 1)
                {
                    ddService.UpdatOrderdjlsh(ddbh, ddbh);
                }
                else
                {
                    ddService.UpdatOrderdaojiantest(ddbh, DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd"));
                }

                if (i % 100 == 0)//退款
                {
                    QuDaoTkModel td = new QuDaoTkModel();
                    td.ddbh = ddbh;
                    td.tkyy = "测试订单，申请退款";
                    td.tkje = 25;
                    td.sqrid = 14;
                    string msg = "";
                    new QuDaoTkService().QdTkSq(td, ref msg);
                }
                if (i % 200 == 0)//退款审核
                {
                    new QuDaoTkService().UpdateTkTy(ddbh, 25, 11);
                }
            }
            ddService.Test("update dd_jbxx set ysjsj=800 where yybh='YY201603111349440001' and tcid='000022'");



            //京东
            for (int i = 0; i < 1000; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 10;
                model.appointmentDate = DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd");
                model.appointmentPackageCode = "000025";
                model.customerAge = 30;
                model.customerBirthday = "1993-01-05";
                model.customerGender = 1;
                model.customerIDCard = "410185199301052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 944;
                model.deptID = "0009";
                model.yybh = "YY201603111349440001";
                model.interfaceNum = 10;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "京东H套餐", ddbh, "京东健康");
                if (i % 3 == 1)
                {
                    ddService.UpdatOrderdjlsh(ddbh, ddbh);
                }
                else
                {
                    ddService.UpdatOrderdaojiantest(ddbh, DateTime.Now.AddDays(-j).ToString("yyyy-MM-dd"));
                }

                if (i % 100 == 0)//退款
                {
                    QuDaoTkModel td = new QuDaoTkModel();
                    td.ddbh = ddbh;
                    td.tkyy = "测试订单，申请退款";
                    td.tkje = 25;
                    td.sqrid = 14;
                    string msg = "";
                    new QuDaoTkService().QdTkSq(td, ref msg);
                }
                if (i % 200 == 0)//退款审核
                {
                    new QuDaoTkService().UpdateTkTy(ddbh, 25, 11);
                }
            }
            ddService.Test("update dd_jbxx set ysjsj=900 where yybh='YY201603111349440001' and tcid='000025'");


            //未来订单
            ///阿里
            for (int i = 0; i < 30000; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 100;
                model.appointmentDate = DateTime.Now.AddDays(j).ToString("yyyy-MM-dd");
                model.appointmentPackageCode = "000016";
                model.customerAge = 30;
                model.customerBirthday = "1990-01-05";
                model.customerGender = 1;
                model.customerIDCard = "500236199001052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 544;
                model.deptID = "0007";
                model.yybh = "YY201603111349440001";

                model.interfaceNum = 8;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "体检A套餐", ddbh, "阿里健康");

                ddService.UpdatOrderdjlsh(ddbh, ddbh);


            }
            ddService.Test("update dd_jbxx set ysjsj=500 where yybh='YY201603111349440001' and tcid='000016'");

            //腾讯
            for (int i = 0; i < 300; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 10;
                model.appointmentDate = DateTime.Now.AddDays(j).ToString("yyyy-MM-dd");
                model.appointmentPackageCode = "000021";
                model.customerAge = 30;
                model.customerBirthday = "1993-01-05";
                model.customerGender = 1;
                model.customerIDCard = "410185199301052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 504;
                model.deptID = "0008";
                model.yybh = "YY201603111349440001";
                model.interfaceNum =9;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "腾讯B套餐", ddbh, "腾讯健康");
                ddService.UpdatOrderdjlsh(ddbh, ddbh);

            }

            ddService.Test("update dd_jbxx set ysjsj=480 where yybh='YY201603111349440001' and tcid='000021'");

            //京东
            for (int i = 0; i < 300; i++)
            {
                string ddbh = new DdJbxxService().GetDdbh();
                Model.Api.PreOrder model = new Model.Api.PreOrder();
                int j = i / 10;
                model.appointmentDate = DateTime.Now.AddDays(j).ToString("yyyy-MM-dd");
                model.appointmentPackageCode = "000024";
                model.customerAge = 30;
                model.customerBirthday = "1993-01-05";
                model.customerGender = 1;
                model.customerIDCard = "410185199301052040";
                model.customerIDType = 1;
                model.customerMedicalStatus = 0;
                model.customerMobilePhone = GetTel();
                model.customerName = GetName();
                model.ddJe = 504;
                model.deptID = "0009";
                model.yybh = "YY201603111349440001";
                model.interfaceNum = 10;
                model.outOrderCode = ddbh;

                ddService.CreateOrder(model, "京东B套餐", ddbh, "京东健康");
                ddService.UpdatOrderdjlsh(ddbh, ddbh);

            }

            ddService.Test("update dd_jbxx set ysjsj=700 where yybh='YY201603111349440001' and tcid='000024'");

        }

        #region 名字生成，电话生成

        private string[] telStarts = "134,135,136,137,138,139,150,151,152,157,158,159,130,131,132,155,156,133,153,180,181,182,183,185,186,176,187,188,189,177,178".Split(',');


        /// <summary>
        /// 随机生成电话号码
        /// </summary>
        /// <returns></returns>
        public string GetTel()
        {
            Random ran = new Random();
            int n = ran.Next(10, 1000);
            int index = ran.Next(0, telStarts.Length - 1);
            string first = telStarts[index];
            string second = (ran.Next(100, 888) + 10000).ToString().Substring(1);
            string thrid = (ran.Next(1, 9100) + 10000).ToString().Substring(1);
            return first + second + thrid;
        }

        public string GetName()
        {
            //姓
            string surname = GetSurname();
            //获取GB2312编码页（表）
            Encoding gb = Encoding.GetEncoding("gb2312");
            //调用函数产生4个随机中文汉字编码
            object[] bytes = CreateRegionCode(2, true);
            //根据汉字编码的字节数组解码出中文汉字
            string name = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                name += gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[])));
            }
            return surname + name;

        }
        public object[] CreateRegionCode(int strlength, bool isRandomCount = false)
        {
            if (isRandomCount)
            {
                Random random = new Random();
                strlength = random.Next(1, strlength + 1);
            }

            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            Random rnd = new Random();
            //定义一个object数组用来
            object[] bytes = new object[strlength];
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中
             每个汉字有四个区位码组成
             区位码第1位和区位码第2位作为字节数组第一个元素
             区位码第3位和区位码第4位作为字节数组第二个元素
            */

            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();
                //区位码第2位
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的种子避免产生重复值
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();
                //区位码第3位
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();
                //区位码第4位
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();
                //定义两个字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中
                byte[] str_r = new byte[] { byte1, byte2 };
                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);
            }
            return bytes;
        }

        public string GetSurname()
        {
            Random random = new Random();
            int index = random.Next(0, surname.Count());
            return surname[index];
        }
        public List<string> surname = new List<string>() {"赵", "钱", "孙", "李", "周", "吴", "郑", "王", "冯", "陈", "楮", "卫", "蒋", "沈", "韩", "杨",
  "朱", "秦", "尤", "许", "何", "吕", "施", "张", "孔", "曹", "严", "华", "金", "魏", "陶", "姜",
  "戚", "谢", "邹", "喻", "柏", "水", "窦", "章", "云", "苏", "潘", "葛", "奚", "范", "彭", "郎",
  "鲁", "韦", "昌", "马", "苗", "凤", "花", "方", "俞", "任", "袁", "柳", "酆", "鲍", "史", "唐",
  "费", "廉", "岑", "薛", "雷", "贺", "倪", "汤", "滕", "殷", "罗", "毕", "郝", "邬", "安", "常",
  "乐", "于", "时", "傅", "皮", "卞", "齐", "康", "伍", "余", "元", "卜", "顾", "孟", "平", "黄",
  "和", "穆", "萧", "尹", "姚", "邵", "湛", "汪", "祁", "毛", "禹", "狄", "米", "贝", "明", "臧",
  "计", "伏", "成", "戴", "谈", "宋", "茅", "庞", "熊", "纪", "舒", "屈", "项", "祝", "董", "梁",
  "杜", "阮", "蓝", "闽", "席", "季", "麻", "强", "贾", "路", "娄", "危", "江", "童", "颜", "郭",
  "梅", "盛", "林", "刁", "锺", "徐", "丘", "骆", "高", "夏", "蔡", "田", "樊", "胡", "凌", "霍",
  "虞", "万", "支", "柯", "昝", "管", "卢", "莫", "经", "房", "裘", "缪", "干", "解", "应", "宗",
  "丁", "宣", "贲", "邓", "郁", "单", "杭", "洪", "包", "诸", "左", "石", "崔", "吉", "钮", "龚",
  "程", "嵇", "邢", "滑", "裴", "陆", "荣", "翁", "荀", "羊", "於", "惠", "甄", "麹", "家", "封",
  "芮", "羿", "储", "靳", "汲", "邴", "糜", "松", "井", "段", "富", "巫", "乌", "焦", "巴", "弓",
  "牧", "隗", "山", "谷", "车", "侯", "宓", "蓬", "全", "郗", "班", "仰", "秋", "仲", "伊", "宫",
  "宁", "仇", "栾", "暴", "甘", "斜", "厉", "戎", "祖", "武", "符", "刘", "景", "詹", "束", "龙",
  "叶", "幸", "司", "韶", "郜", "黎", "蓟", "薄", "印", "宿", "白", "怀", "蒲", "邰", "从", "鄂",
  "索", "咸", "籍", "赖", "卓", "蔺", "屠", "蒙", "池", "乔", "阴", "郁", "胥", "能", "苍", "双",
  "闻", "莘", "党", "翟", "谭", "贡", "劳", "逄", "姬", "申", "扶", "堵", "冉", "宰", "郦", "雍",
  "郤", "璩", "桑", "桂", "濮", "牛", "寿", "通", "边", "扈", "燕", "冀", "郏", "浦", "尚", "农",
  "温", "别", "庄", "晏", "柴", "瞿", "阎", "充", "慕", "连", "茹", "习", "宦", "艾", "鱼", "容",
  "向", "古", "易", "慎", "戈", "廖", "庾", "终", "暨", "居", "衡", "步", "都", "耿", "满", "弘",
  "匡", "国", "文", "寇", "广", "禄", "阙", "东", "欧", "殳", "沃", "利", "蔚", "越", "夔", "隆",
  "师", "巩", "厍", "聂", "晁", "勾", "敖", "融", "冷", "訾", "辛", "阚", "那", "简", "饶", "空",
  "曾", "毋", "沙", "乜", "养", "鞠", "须", "丰", "巢", "关", "蒯", "相", "查", "后", "荆", "红",
  "游", "竺", "权", "逑", "盖", "益", "桓", "公", "仉", "督", "晋", "楚", "阎", "法", "汝", "鄢",
  "涂", "钦", "岳", "帅", "缑", "亢", "况", "后", "有", "琴", "归", "海", "墨", "哈", "谯", "笪",
  "年", "爱", "阳", "佟", "商", "牟", "佘", "佴", "伯", "赏",
  "万俟", "司马", "上官", "欧阳", "夏侯", "诸葛", "闻人", "东方", "赫连", "皇甫", "尉迟", "公羊",
  "澹台", "公冶", "宗政", "濮阳", "淳于", "单于", "太叔", "申屠", "公孙", "仲孙", "轩辕", "令狐",
  "锺离", "宇文", "长孙", "慕容", "鲜于", "闾丘", "司徒", "司空", "丌官", "司寇", "子车", "微生",
  "颛孙", "端木", "巫马", "公西", "漆雕", "乐正", "壤驷", "公良", "拓拔", "夹谷", "宰父", "谷梁",
  "段干", "百里", "东郭", "南门", "呼延", "羊舌", "梁丘", "左丘", "东门", "西门", "南宫"};
    }

    #endregion


  

}