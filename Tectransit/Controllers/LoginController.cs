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
                LoginHandler(res.ID?.ToString());

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

        private void LoginHandler(string ID)
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

            Set("_usercode", ID, 480);
            string userName = DBUtil.GetSingleValue1($@"SELECT USERNAME AS COL1 FROM T_S_USER WHERE USERCODE = '{ID}' AND ISENABLE = 'true'");
            Set("_username", userName, 480);

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

    }
}