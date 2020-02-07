using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
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

        [HttpGet("{id}")]
        public dynamic GetAllCusRank(string id)
        {
            return objCommon.GetAllCusRank(id);
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

        [HttpGet]
        public dynamic GetStationData()
        {
            return objCommon.GetStationData();
        }

        [HttpGet]
        public dynamic GetMemtype()
        {
            Hashtable htData = new Hashtable();
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            string sql = $@"SELECT A.USERCODE AS COL1 FROM T_S_ACCOUNT A
                            LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE 
                            LEFT JOIN T_S_RANK C ON B.RANKID = C.ID
                            WHERE C.RANKTYPE = 2 AND A.USERCODE = '{htData["_acccode"]}' AND A.ISENABLE = 'true'";
            string IsCusMem = string.IsNullOrEmpty(DBUtil.GetSingleValue1(sql)) ? "N" : "Y";

            return new { status = "0", data = IsCusMem };
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

        [HttpPost]
        [DisableRequestSizeLimit]
        public dynamic UploadImgData_F()
        {
            try
            {
                string usercode = Request.Cookies["_acccode"];

                string type = Request.Form["TYPE"];
                var file = Request.Form.Files;
                var folderName = Path.Combine(@"tectransit\dist\tectransit\assets\" + type, usercode);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                ArrayList AL = new ArrayList();

                if (file.Count > 0)
                {
                    for (int i = 0; i < file.Count; i++)
                    {
                        Hashtable htData = new Hashtable();
                        htData["ID"] = Request.Form["idcode"][i];
                        var fileName = ContentDispositionHeaderValue.Parse(file[i].ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file[i].CopyTo(stream);
                        }

                        dbPath = dbPath.Replace(@"tectransit\dist\tectransit", "res").Replace(@"\", @"/");

                        htData["IMGPATH"] = dbPath;
                        AL.Add(htData);
                        
                    }

                    List<IDImgList> rowlist = new List<IDImgList>();
                    List<string> chkID = new List<string>();
                    bool IsAdd = false;
                    if (AL.Count > 0)
                    {
                        for (int j = 0; j < AL.Count; j++)
                        {
                            Hashtable sData = (Hashtable)AL[j];
                            IsAdd = false;

                            //先檢查id是否已新增
                            foreach (string ck in chkID)
                            {
                                if (ck == sData["ID"]?.ToString())
                                    IsAdd = true;
                            }
                            
                            if (!IsAdd)
                            {
                                //是否有相同的id-->Y:合併成一筆
                                for (int k = j + 1; k < AL.Count; k++)
                                {
                                    Hashtable tData = (Hashtable)AL[k];
                                    if (sData["ID"]?.ToString() == tData["ID"]?.ToString())
                                    {
                                        IDImgList row = new IDImgList();
                                        row.ID = sData["ID"]?.ToString();
                                        row.IDPHOTOF = sData["IMGPATH"]?.ToString();
                                        row.IDPHOTOB = tData["IMGPATH"]?.ToString();

                                        rowlist.Add(row);
                                        chkID.Add(row.ID);

                                        IsAdd = true;
                                    }
                                }
                                
                                if (!IsAdd)
                                {
                                    IDImgList row = new IDImgList();
                                    row.ID = sData["ID"]?.ToString();
                                    row.IDPHOTOF = sData["IMGPATH"]?.ToString();
                                    row.IDPHOTOB = "";

                                    rowlist.Add(row);
                                    chkID.Add(row.ID);
                                }
                            }
                            
                        }
                    }
                    
                    return new { status = "0", imgurl = rowlist };
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

        [HttpPost]
        [DisableRequestSizeLimit]
        public dynamic UploadFileData_F()
        {
            try
            {
                string usercode = Request.Cookies["_acccode"];

                string type = Request.Form["TYPE"];
                var file = Request.Form.Files;
                var folderName = Path.Combine(@"tectransit\dist\tectransit\assets\" + type, usercode);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                List<IDFileList> rowlist = new List<IDFileList>();
                if (file.Count > 0)
                {
                    for (int i = 0; i < file.Count; i++)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file[i].ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file[i].CopyTo(stream);
                        }

                        dbPath = dbPath.Replace(@"tectransit\dist\tectransit", "res").Replace(@"\", @"/");
                        
                        IDFileList row = new IDFileList();
                        row.ID = Request.Form["idcode"][i];
                        row.APPOINTMENT = dbPath;

                        rowlist.Add(row);
                    }

                    return new { status = "0", fileurl = rowlist };
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