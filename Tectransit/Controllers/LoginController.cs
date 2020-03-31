using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tectransit.Datas;
using System.Collections;
using System;

namespace Tectransit.Controllers
{
    [Route("api/Login/[action]")]
    public class LoginController : Controller
    {
        private const string _captchaHashKey = "CaptchaHash";
        public Captchabll captchabll = new Captchabll();
        
        private string CaptchaHash
        {
            get {return HttpContext.Session.GetString(_captchaHashKey) as string; }
            set { HttpContext.Session.SetString(_captchaHashKey, value); }
        }


        [HttpPost]
        public dynamic doLogin([FromBody] object form)
        {
            var jsonData = JObject.FromObject(form);
            string clientIP = HttpContext.Connection.RemoteIpAddress?.ToString();

            Hashtable htData = new Hashtable();
            htData["USERCODE"] = jsonData.Value<string>("USERCODE");
            htData["PASSWORD"] = jsonData.Value<string>("PASSWORD");
            htData["HOSTNAME"] = HttpContext.Request.Host.Host;
            htData["ClientIP"] = clientIP;

            user objuser = new user();

            var res = objuser.Login(htData, true);
            if (res.status?.ToString() == "success")
                LoginHandler(res.ID?.ToString(), "B");

            return res;
        }

        [HttpGet]
        public dynamic doLogout()
        {
            //Delete the cookie  
            Remove("_usercode");
            Remove("_username");

            return "0";
        }

        [HttpPost]
        public dynamic doAccLogin([FromBody] object form)
        {
            var jsonData = JObject.FromObject(form);
            string clientIP = HttpContext.Connection.RemoteIpAddress?.ToString();
            
            Hashtable htData = new Hashtable();
            htData["USERCODE"] = jsonData.Value<string>("USERCODE");
            htData["PASSWORD"] = jsonData.Value<string>("PASSWORD");
            htData["CAPTCHA"] = jsonData.Value<string>("CODE");
            htData["RANKTYPE"] = 1;//個人用戶
            htData["HOSTNAME"] = HttpContext.Request.Host.Host;
            htData["ClientIP"] = clientIP;

            if (!CheckCode(htData["CAPTCHA"]?.ToString()))
                return new { status = "error", message = "驗證碼輸入錯誤！" };

            user objuser = new user();

            var res = objuser.ACLogin(htData, true);
            if (res.status?.ToString() == "success")
                LoginHandler(res.ID?.ToString(), "A");

            return res;
        }

        [HttpGet]
        public dynamic doAccLogout()
        {
            //Delete the cookie  
            Remove("_acccode");
            Remove("_accname");

            return "0";
        }

        [HttpPost]
        public dynamic doCusLogin([FromBody] object form)
        {
            var jsonData = JObject.FromObject(form);
            string clientIP = HttpContext.Connection.RemoteIpAddress?.ToString();

            Hashtable htData = new Hashtable();
            htData["USERCODE"] = jsonData.Value<string>("USERCODE");
            htData["PASSWORD"] = jsonData.Value<string>("PASSWORD");
            htData["CAPTCHA"] = jsonData.Value<string>("CODE");
            htData["RANKTYPE"] = "2,3,4";//廠商&進出口報關行
            htData["HOSTNAME"] = HttpContext.Request.Host.Host;
            htData["ClientIP"] = clientIP;

            if (!CheckCode(htData["CAPTCHA"]?.ToString()))
                return new { status = "error", message = "驗證碼輸入錯誤！" };

            user objuser = new user();

            var res = objuser.ACLogin(htData, true);
            if (res.status?.ToString() == "success")
                LoginHandler(res.ID?.ToString(), "C");

            return res;
        }

        [HttpGet]
        public dynamic doCusLogout()
        {
            //Delete the cookie  
            Remove("_cuscode");
            Remove("_cusname");

            return "0";
        }

        [HttpGet]
        public ActionResult GetCaptcha()
        {
            // 隨機產生四個字元
            var randomText = captchabll.GenerateRandomText(4);
            // 加密後存在 Session，也可以不用加密，比對時一致就好。
            CaptchaHash = captchabll.ComputeMd5Hash(randomText);
            // 回傳 gif 圖檔
            return File(captchabll.GenerateCaptchaImage(randomText), "image/gif");
        }

        private void LoginHandler(string ID, string Type)
        {
            /*string ucode_cookie = Request.Cookies["_usercode"];
            string uname_cookie = Request.Cookies["_username"];
            if (string.IsNullOrEmpty(ucode_cookie) && string.IsNullOrEmpty(uname_cookie))
            {
                Set("_usercode", ID, 120);
                string userName = DBUtil.GetSingleValue1($@"SELECT USERNAME AS COL1 FROM T_S_USER WHERE USERCODE = '{ID}' AND ISENABLE = 'true'");
                Set("_username", userName, 120);
            }
            */

            if (Type == "B")
            {
                Set("_usercode", ID, 480);
                string userName = DBUtil.GetSingleValue1($@"SELECT USERNAME AS COL1 FROM T_S_USER WHERE USERCODE = '{ID}' AND ISENABLE = 'true'");
                Set("_username", userName, 480);
            }
            else if (Type == "A")
            {
                Set("_acccode", ID, 120);
                string accName = DBUtil.GetSingleValue1($@"SELECT USERNAME AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{ID}' AND ISENABLE = 'true'");
                Set("_accname", accName, 120);
            }
            else if (Type == "C")
            {
                Set("_cuscode", ID, 120);
                string accName = DBUtil.GetSingleValue1($@"SELECT USERNAME AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{ID}' AND ISENABLE = 'true'");
                Set("_cusname", accName, 120);
            }

        }
        
        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void Set(string key, string value, int? expireTime)
        {
            
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }

        private bool CheckCode(string code)
        {
            if (CaptchaHash == captchabll.ComputeMd5Hash(code))
                return true;
            else
                return false;
        }

    }
}