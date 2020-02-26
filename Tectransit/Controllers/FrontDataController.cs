﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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

        [HttpPost]
        public dynamic GetNewsData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            //JObject temp = jsonData.Value<JObject>("srhForm");            

            return objFront.GetNewsData(sWhere, pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public dynamic GetNews(long id)
        {
            return objFront.GetNews(id);
        }
    }
}