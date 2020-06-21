using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetOrder.Controllers
{
    public class ImgCodeController : Controller
    {
        public void Index()
        {

            //首先实例化验证码的类
            ValidateCode validateCode = new ValidateCode();
            //生成验证码指定的长度
            string code = validateCode.GetRandomString(4);
            Session["ImgCode"] = code;
            //创建验证码的图片
            byte[] bytes = validateCode.CreateImage(code);
            //最后将验证码返回
            //return File(bytes, @"image/jpeg");

            Response.ClearContent();
            Response.ContentType = "image/Png";
            Response.BinaryWrite(bytes);


        }
    }
}