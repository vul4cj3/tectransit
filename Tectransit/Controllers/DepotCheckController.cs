using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tectransit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepotCheckController : ControllerBase
    {

        //倉庫點收API
        [HttpPost]
        public dynamic Post([FromBody]object json)
        {
            try
            {
                string a = "0";
                return new { status = 0, msg = "成功", error = "" };                
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = 99, msg = "失敗", error = json.ToString() };
            }
        }
    }
}