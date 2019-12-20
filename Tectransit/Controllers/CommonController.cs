using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Tectransit.Datas;

namespace Tectransit.Controllers
{
    [Route("api/CommonHelp/[action]")]
    public class CommonController : Controller
    {
        [HttpPost]
        public dynamic GetNavMenu([FromBody] object form)
        {
            var jsonData = JObject.FromObject(form);            
            string USERCODE = jsonData.Value<string>("USERCODE");

            CommonHelper objCommon = new CommonHelper();

            return objCommon.GetMenu(USERCODE);
        }



    }
}