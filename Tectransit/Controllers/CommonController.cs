using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Tectransit.Datas;

namespace Tectransit.Controllers
{
    [Route("api/CommonHelp/[action]")]
    public class CommonController : Controller
    {
        CommonHelper objCommon = new CommonHelper();

        [HttpPost]
        public dynamic GetNavMenu([FromBody] object form)
        {
            var jsonData = JObject.FromObject(form);            
            string USERCODE = jsonData.Value<string>("USERCODE");
            
            return objCommon.GetMenu(USERCODE);
        }

        [HttpGet("{id}")]
        public dynamic GetAllMenu(string id)
        {
            return objCommon.GetAllMenu(id);
        }        

        [HttpGet("{id}")]
        public dynamic GetAllRole(string id)
        {
            return objCommon.GetAllRole(id);
        }

        [HttpGet("{id}")]
        public dynamic GetAllRank(string id)
        {
            return objCommon.GetAllRank(id);
        }

        [HttpGet]
        public dynamic GetAllBacknFrontMenu()
        {
            return objCommon.GetAllBacknFrontMenu();
        }

        [HttpGet]
        public dynamic GetAllBanner()
        {
            return objCommon.GetAllBanner();
        }

        [HttpGet("{id}")]
        public dynamic GetParentMenu(string id)
        {
            return objCommon.GetParentMenu(id);
        }

        [HttpPost, DisableRequestSizeLimit]
        public dynamic UploadImgData()
        {
            try
            {
                string type = Request.Form["TYPE"];
                var file = Request.Form.Files[0];
                var folderName = Path.Combine(@"admin\src\assets", type);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    dbPath = dbPath.Replace(@"admin\src", "").Replace(@"\", @"/");

                    return new { status = "0", imgurl = dbPath };
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return new { status = "99", imgurl = "" };
            }
        }
    }

}