using System;
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

        [HttpGet]
        public dynamic GetFaqCate()
        {
            return objFront.GetFaqCate();
        }

        [HttpGet("{id}")]
        public dynamic GetFaqData(long id)
        {
            return objFront.GetFaqData(id);
        }

        [HttpGet]
        public dynamic GetAboutCate()
        {
            return objFront.GetAboutCateData("", null);
        }

        [HttpGet("{id}")]
        public dynamic GetAboutCateData(long id)
        {
            string sWhere = "AND ID = @ABOUTHID";
            Hashtable htData = new Hashtable();
            htData["ABOUTHID"] = id;

            return objFront.GetAboutCateData(sWhere, htData);
        }

        [HttpPost]
        public dynamic GetAboutListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JArray tempAL = jsonData.Value<JArray>("srhForm");

            Hashtable htData = new Hashtable();
            for (int i = 0; i < tempAL.Count; i++)
            {
                JObject temp = (JObject)tempAL[i];

                foreach (var t in temp)
                {
                    htData[t.Key.ToUpper()] = t.Value?.ToString();
                }
            }

            sWhere = "WHERE ISENABLE = 'true'";

            if (!string.IsNullOrEmpty(htData["ABOUTHID"]?.ToString()))
            {
                sWhere += " AND ABOUTHID = @ABOUTHID";
            }

            return objFront.GetAboutListData(sWhere, htData, pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public dynamic GetAboutData(long id)
        {
            Hashtable htData = new Hashtable();
            htData["ABOUTID"] = id;

            return objFront.GetAboutData(htData);
        }
    }
}