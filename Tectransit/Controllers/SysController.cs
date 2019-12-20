using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Tectransit.Datas;

namespace Tectransit.Controllers
{
    [Route("api/SysHelp/[action]")]
    public class SysController : Controller
    {
        [HttpPost]
        public dynamic GetTSRoleData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");

            SysHelper objSys = new SysHelper();

            return objSys.GetRoleListData(sWhere, pageIndex, pageSize);
        }
    }
}