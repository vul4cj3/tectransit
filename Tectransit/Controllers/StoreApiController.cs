using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tectransit.Controllers
{
    [Route("api/Store/[action]")]
    public class StoreApiController : ControllerBase
    {
        //post request data to 大智通(7-11)
        [HttpPost]
        public void PostToStore()
        {
        }

        //receive response data from 大智通(7-11)
        [HttpPost]
        public dynamic GetStoreRes([FromBody] object form)
        {
            return form;
        }


    }
}