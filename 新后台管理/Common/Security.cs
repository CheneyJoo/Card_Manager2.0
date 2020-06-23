using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class Security
    {
        /// <summary> 
        /// MD5加密 
        /// </summary> 
        /// <param name="strPwd">被加密的字符串</param> 
        /// <returns>加密后的字符串</returns> 
        public static string MD5(string strPwd)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string a = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strPwd)));
            a = a.Replace("-", "").ToLower();
            return a;


            //MD5 md5 = new MD5CryptoServiceProvider();
            ////将字符编码为一个字节序列 
            //byte[] data = System.Text.Encoding.Default.GetBytes(strPwd);
            ////计算data字节数组的哈希值 
            //byte[] md5data = md5.ComputeHash(data);
            //md5.Clear();
            //string str = "";
            //for (int i = 0; i < md5data.Length - 1; i++)
            //{
            //    //将字符转换成16进制并右对齐字符，字符数为2，不足位填0补足 
            //    str += md5data[i].ToString("x").PadLeft(2, '0');
            //}
            //return str;
        }

        /// <summary>
        /// 实现随机验证码
        /// </summary>
        /// <param name="n">显示验证码的个数</param>
        /// <returns>返回生成的随机数</returns>
        public static string RandomNum(int n)
        {
            //包含数字，大写英文字母和小谢英文字母的字符串
            string strchar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            //将strchar转化为数组
            string[] VcArray = strchar.Split(',');
            string VNum = "";
            //记录上次随即数值，尽量避免产生几个一样的随机数
            int temp = -1;
            //采用简单的算法保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i <= n; i++)
            {
                if (temp != -1)
                {
                    //unchecked:用于取消整型算术运算和转换的溢出检查
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                //返回一个小于所制定最大值的非负随机数,61为VcArray数组长度
                int t = rand.Next(61);
                if ((temp != -1) && (temp == t))
                {
                    return RandomNum(n);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }

        /// <summary>
        /// 对文字加密为HTML字符串
        /// </summary>
        /// <param name="str">要加密的文字</param>
        /// <returns></returns>
        public static string EncodeString(string str) {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 对文字解密为HTML字符串
        /// </summary>
        /// <param name="str">要解密的文字</param>
        /// <returns></returns>
        public static string DecodeString(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        //在C#后台实现JavaScript的函数escape()的字符串转换
        //些方法支持汉字
        public static string escape(string s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byteArr = System.Text.Encoding.Unicode.GetBytes(s);

            for (int i = 0; i < byteArr.Length; i += 2)
            {
                sb.Append("%u");
                sb.Append(byteArr[i + 1].ToString("X2"));//把字节转换为十六进制的字符串表现形式

                sb.Append(byteArr[i].ToString("X2"));
            }
            return sb.ToString();

        }

        //把JavaScript的escape()转换过去的字符串解释回来
        //些方法支持汉字
        public static string unescape(string s)
        {

            string str = s.Remove(0, 2);//删除最前面两个＂%u＂
            string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔
            byte[] byteArr = new byte[strArr.Length * 2];
            for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
            {
                byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16);  //把十六进制形式的字串符串转换为二进制字节
                byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
            }
            str = System.Text.Encoding.Unicode.GetString(byteArr);　//把字节转为unicode编码
            return str;

        }
        public static string Escape(string str)
        {
            return Microsoft.JScript.GlobalObject.escape(str);
        } 

        public static string UnEscape(string str)
        {
            return Microsoft.JScript.GlobalObject.unescape(str); 
            //StringBuilder sb = new StringBuilder();
            //int len = str.Length;
            //int i = 0;
            //while (i != len)
            //{
            //    if (Uri.IsHexEncoding(str, i))
            //        sb.Append(Uri.HexUnescape(str, ref i));
            //    else
            //        sb.Append(str[i++]);
            //}
            //return sb.ToString();
        }

        public static string encodeURIComponent(string str)
        {
            return Microsoft.JScript.GlobalObject.encodeURIComponent(str);
        }

        public static string decodeURIComponent(string str)
        {
            return Microsoft.JScript.GlobalObject.decodeURIComponent(str);
        }


        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "!%$#^@&@");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }


        #endregion

        #region ========解密========


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "!%$#^@&@");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        /// <summary>
        /// 带Emoji表情的的字符串
        /// [e]1f1e6[/e]
        /// [e]1f1fa[/e]
        /// [e]1f1e6[/e]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        public static string ToUnicode(string str)
        {

            foreach (var a in str)
            {
                byte[] bts = Encoding.Unicode.GetBytes(a.ToString());

                if (bts.Length != 4 && bts.Length != 5)
                {
                    throw new ArgumentException("错误的 EmojiCode 16进制数据长度.一般为4位或5位");
                }

                Int32 Bint = System.BitConverter.ToInt32(bts, 0);
                var mm = UTF16ToEmoji(Bint);
                string strA = mm.ToString("x");


                str = str.Replace(a.ToString(), strA);

            }
            return str;


        }
        /// <summary>
        /// 返回Emoji表情utf16 对应的int值
        /// </summary>
        /// <param name="Bint"></param>
        /// <param name="LowHeight"></param>
        /// <returns></returns>
        private static Int32 UTF16ToEmoji(Int32 Bint, bool LowHeight = true)
        {

            Int32 Bintf = ~Bint;
            Int32 Bintffh = Bintf >> 16;
            Int32 y = Bintffh ^ 0xffff;
            Int32 Bintfl = Bintf & 0x7fff;
            Int32 x = Bintfl ^ 0xffff;
            //  (((( (x ^ 0xD800) << 2) | ((y ^ 0xDC00) >> 8)) << 8) | ((y ^ 0xDC00) & 0xFF)) + 0x10000
            Int32 Vx = x ^ 0xD800;
            Int32 Vx1 = Vx << 2;
            Int32 Vy = y ^ 0xDC00;
            Int32 Vy1 = Vy >> 8;
            Int32 xy = (Vx1 | Vy1) << 8;

            Int32 N = y ^ 0xDC00;
            Int32 M = N & 0xFF;

            Int32 d = xy | M;
            return d + 0x10000;
        }
    }
}
