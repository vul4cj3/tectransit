using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tectransit.Datas;
using System.Collections;

namespace Tectransit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public dynamic PostLogin([FromBody] object form)
        {
            var jsonData = JObject.FromObject(form);
            string clientIP = HttpContext.Connection.RemoteIpAddress?.ToString();

            Hashtable htData = new Hashtable();
            htData["USERCODE"] = jsonData.Value<string>("USERCODE");
            htData["PASSWORD"] = jsonData.Value<string>("PASSWORD");
            htData["HOSTNAME"] = HttpContext.Request.Host.Host;
            htData["ClientIP"] = clientIP;

            user objuser = new user();
            return objuser.Login(htData, true);
        }
    }
}