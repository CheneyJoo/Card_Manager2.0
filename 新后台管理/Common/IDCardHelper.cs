using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 身份证号处理工具类
    /// </summary>
    public class IDCardHelper
    {

        /// <summary>
        /// 根据身份证获取生日
        /// </summary>
        /// <param name="cardid">身份证</param>      
        /// <returns>1990-01-01</returns>
        public static string GetBirthdayByIdentityCardId(string cardid)
        {
            bool res = true;
            string birthday = string.Empty;
            System.Text.RegularExpressions.Regex regex = null;

            if (cardid.Length == 18)
            {
                regex = new Regex(@"^\d{17}(\d|x)$", RegexOptions.IgnoreCase);
                if (regex.IsMatch(cardid))
                {
                    if (res)
                        birthday = cardid.Substring(6, 8).Insert(4, "-").Insert(7, "-");
                    else
                        birthday = cardid.Substring(6, 8);
                }
                else
                {
                    birthday = "invalid cardid";
                }
            }
            else if (cardid.Length == 15)
            {
                regex = new Regex(@"^\d{15}");
                if (regex.IsMatch(cardid))
                {
                    if (res)
                        birthday = cardid.Substring(6, 6).Insert(2, "-").Insert(5, "-");
                    else
                        birthday = cardid.Substring(6, 6);
                }
                else
                {
                    birthday = "invalid cardid";
                }
            }
            else
            {
                birthday = "invalid cardid";
            }

            return birthday;
        }
        /// <summary>
        /// 根据身份证获取身份证信息
        /// 18位身份证
        /// 0地区代码(1~6位,其中1、2位数为各省级政府的代码，3、4位数为地、市级政府的代码，5、6位数为县、区级政府代码)
        /// 1出生年月日(7~14位)
        /// 2顺序号(15~17位单数为男性分配码，双数为女性分配码)
        /// 3性别
        /// 15位身份证
        /// 0地区代码 
        /// 1出生年份(7~8位年,9~10位为出生月份，11~12位为出生日期 
        /// 2顺序号(13~15位)，并能够判断性别，奇数为男，偶数为女
        /// 3性别
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static string[] GetCardIdInfo(string cardId)
        {
            string[] info = new string[4];
            System.Text.RegularExpressions.Regex regex = null;
            if (cardId.Length == 18)
            {
                regex = new Regex(@"^\d{17}(\d|x)$", RegexOptions.IgnoreCase);
                if (regex.IsMatch(cardId))
                {
                    info.SetValue(cardId.Substring(0, 6), 0);
                    info.SetValue(cardId.Substring(6, 8), 1);
                    info.SetValue(cardId.Substring(14, 3), 2);
                    info.SetValue(Convert.ToInt32(info[2]) % 2 != 0 ? "男" : "女", 3);
                }
            }
            else if (cardId.Length == 15)
            {
                regex = new Regex(@"^\d{15}");
                if (regex.IsMatch(cardId))
                {
                    info.SetValue(cardId.Substring(0, 6), 0);
                    info.SetValue(cardId.Substring(6, 6), 1);
                    info.SetValue(cardId.Substring(12, 3), 2);
                    info.SetValue(Convert.ToInt32(info[2]) % 2 != 0 ? "男" : "女", 3);
                }
            }

            return info;

        }
        /// <summary>
        /// 根据身分证号获取年龄
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns>0表示格式不正确，其它返回年龄</returns>
        public static int getAgeByIDCard(string IdCard)
        {
            int age = 0;
            string birthday = GetBirthdayByIdentityCardId(IdCard);
            if (!string.IsNullOrWhiteSpace(birthday) && !birthday.Contains("invalid"))
            {
                DateTime dtNow = DateTime.Now;
                DateTime dtEnd = Convert.ToDateTime(birthday);
                age = dtNow.Year - dtEnd.Year;
                if (dtEnd.Month > dtNow.Month)
                {
                    age = age - 1;
                }
            }
            return age;
        }
        public static int getAge(string birthday)
        {
            int age = 0;
            DateTime dtNow = DateTime.Now;
            DateTime dtEnd = Convert.ToDateTime(birthday);
            age = dtNow.Year - dtEnd.Year;
            if (dtEnd.Month > dtNow.Month)
            {
                age = age - 1;
            }
            return age;
        }

        public static DateTime GetBirthday(string identityCard)
        {
            try
            {
                string birthday = DateTime.Now.ToString("yyyy-MM-dd");
                if (identityCard.Length == 18)//处理18位的身份证号码从号码中得到生日和性别代码
                {
                    birthday = identityCard.Substring(6, 4) + "-" + identityCard.Substring(10, 2) + "-" + identityCard.Substring(12, 2);
                    //sex = identityCard.Substring(14, 3);
                }
                if (identityCard.Length == 15)
                {
                    birthday = "19" + identityCard.Substring(6, 2) + "-" + identityCard.Substring(8, 2) + "-" + identityCard.Substring(10, 2);
                    //sex = identityCard.Substring(12, 3);
                }
                return Convert.ToDateTime(birthday); ;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
    }
}
