using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tectransit.Datas;
using Tectransit.Modles;

namespace Tectransit.Controllers
{
    [Route("api/FrontHelp/[action]")]
    public class FrontDataController : ControllerBase
    {
        CommonHelper objComm = new CommonHelper();
        FrontDataHelper objFront = new FrontDataHelper();

        private readonly TECTRANSITDBContext _context;

        public FrontDataController(TECTRANSITDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public dynamic GetNewsData()
        {
            return objFront.GetNewsData();
        }
    }
}