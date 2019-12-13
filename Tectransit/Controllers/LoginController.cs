using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tectransit.Datas;

namespace Tectransit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //private IHttpContextAccessor _accessor;
        //[HttpPost("[action]")]
        //public IEnumerable<string> Login(IHttpContextAccessor accessor)
        //{
        //    _accessor = accessor;
        //    string clientIP = _accessor.HttpContext.Connection.RemoteIpAddress?.ToString();

        //    user objuser = new user();
        //    return objuser.Login();
        //}
    }
}