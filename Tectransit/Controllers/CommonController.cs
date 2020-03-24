using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Tectransit.Datas;

namespace Tectransit.Controllers
{
    [Route("api/CommonHelp/[action]")]
    public class CommonController : Controller
    {
        public IConfiguration _configuration { get; }

        CommonHelper objCommon = new CommonHelper();
        private const string _captchaHashKey = "CaptchaHash";
        public Captchabll captchabll = new Captchabll();

        public CommonController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        private string CaptchaHash
        {
            get { return HttpContext.Session.GetString(_captchaHashKey) as string; }
            set { HttpContext.Session.SetString(_captchaHashKey, value); }
        }

        public bool CheckCode(string code)
        {
            if (CaptchaHash == captchabll.ComputeMd5Hash(code))
                return true;
            else
                return false;
        }

        [HttpGet]
        public ActionResult GetCaptcha()
        {
            // 隨機產生四個字元
            var randomText = captchabll.GenerateRandomText(4);
            // 加密後存在 Session，也可以不用加密，比對時一致就好。
            CaptchaHash = captchabll.ComputeMd5Hash(randomText);
            // 回傳 gif 圖檔
            return File(captchabll.GenerateCaptchaImage(randomText), "image/gif");
        }

        [HttpPost]
        public dynamic GetNavMenu([FromBody] object form)
        {
            var jsonData = JObject.FromObject(form);            
            string USERCODE = jsonData.Value<string>("USERCODE");
            
            return objCommon.GetMenu(USERCODE);
        }

        [HttpGet]
        public dynamic GetNavMenu_Front()
        {
            return objCommon.GetMenu_Fornt();
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
        public dynamic GetAllRole_C(string id)
        {
            string sWhere = "WHERE A.ID > 3";
            return objCommon.GetAllRole(id, sWhere);
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

        //後台用
        [HttpGet]
        public dynamic GetAllBanner()
        {
            return objCommon.GetAllBanner();
        }

        //前台用
        [HttpGet]
        public dynamic GetBanner()
        {
            return objCommon.GetBanner();
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
        public dynamic GetFirstStation()
        {

            string sql = $@"SELECT TOP 1 STATIONCODE AS COL1 FROM T_S_STATION ORDER BY ID";
            string stationcode = DBUtil.GetSingleValue1(sql);

            return new { status = "0", data = stationcode };
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

            /*--- 2020.03.10 廠商會員都使用匯入集運---*/
            //有月結廠商會員權限-->顯示"匯入集運"
            /*sql = $@"SELECT A.USERCODE AS COL1 FROM T_S_ACCOUNT A
                            LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE 
                            LEFT JOIN T_S_RANK C ON B.RANKID = C.ID
                            WHERE C.RANKTYPE = 2 AND A.USERCODE = '{htData["_acccode"]}' AND A.ISENABLE = 'true' AND C.ID = 7";
            string IsCusMem_m = string.IsNullOrEmpty(DBUtil.GetSingleValue1(sql)) ? "N" : "Y";*/

            return new { status = "0", data = IsCusMem };
        }

        [HttpPost]
        public dynamic GetNewPassword([FromBody]object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");
                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString().Replace("'", "").Replace("=", "");

                string sql = $@"SELECT USERCODE AS COL1 
                            FROM T_S_ACCOUNT 
                            WHERE TAXID = '{htData["IDCODE"]}' AND EMAIL = '{htData["EMAIL"]}' AND ISENABLE = 'true'";

                string IsCusMem = DBUtil.GetSingleValue1(sql);

                //更新密碼
                string newPW = objCommon.GetRandomStringNumber(10);
                string MD5PW = objCommon.GetMd5Hash(newPW);
                sql = $@"UPDATE T_S_ACCOUNT SET USERPASSWORD = '{MD5PW}' WHERE USERCODE = '{IsCusMem}'";
                DBUtil.EXECUTE(sql);

                if (!string.IsNullOrEmpty(IsCusMem))
                {
                    string F_User = "TEC Website System<ebs.sys@t3ex-group.com>";
                    string T_User = htData["EMAIL"]?.ToString();
                    string subject = "TEC轉運平台 - 重設會員密碼";
                    string body = "";

                    body += $"<p>帳號：{IsCusMem}</p><br/>";
                    body += $"<p>新密碼：{newPW}</p><br/>";
                    body += "<p>請使用新密碼至會員管理頁面自行修改密碼，謝謝</p>";

                    string C_User = "siawu@t3ex-group.com";

                    objCommon.SendMail(F_User, T_User, subject, body, C_User);

                    return new { status = "0", msg = "已寄出信件！" };
                }

                return new { status = "99", msg = "會員不存在！" };
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return new { status = "99", msg = "送出失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditContact([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");
                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString().Replace("'", "").Replace("=", "");

                if (!CheckCode(htData["CAPTCHA"]?.ToString()))
                    return new { status = "99", msg = "驗證碼輸入錯誤！" };

                string F_User = "TEC Website System<ebs.sys@t3ex-group.com>";
                string T_User = _configuration.GetSection("WebsitSetting")["csMail"];
                string subject = $"TEC轉運平台 - 聯絡我們({htData["NAME"]}/{htData["EMAIL"]})";
                string body = "";

                //移除html
                string regex = @"(<.+?>|&nbsp;)";
                htData["MESSAGE"] = Regex.Replace(htData["MESSAGE"]?.ToString(), regex, "");

                body += $"<p>姓名：{htData["NAME"]}</p>";
                body += $"<p>連絡電話：{htData["PHONE"]}</p>";
                body += $"<p>電子信箱：{htData["EMAIL"]}</p>";
                body += $"<p>內容：</p>";
                body += $"<p>{htData["MESSAGE"]}</p><br/>";
                body += "<p style='color:#ff0000'>[此為系統自動寄送信件，請勿直接回覆，謝謝！]</p>";

                string C_User = "";

                objCommon.SendMasterMail(F_User, T_User, subject, body, C_User);

                return new { status = "0", msg = "送出成功！" };
            }
            catch (Exception ex)
            {
                string errMsg= ex.Message.ToString();
                return new { status = "99", msg = "送出失敗！請洽客服人員，謝謝。" };
            }
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public dynamic UploadImgData()
        {
            try
            {
                string type = Request.Form["TYPE"];
                var file = Request.Form.Files[0];
                var folderName = Path.Combine(@"admin\dist\admin\assets", type);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    dbPath = dbPath.Replace(@"admin\dist\admin", "").Replace(@"\", @"/");

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
        public IActionResult editorImgUpload(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
            var folderpath = Path.Combine(Directory.GetCurrentDirectory(), @"tectransit\dist\tectransit\assets\ArticleImg\");
            var filepath = Path.Combine(folderpath, fileName);

            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }

            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                upload.CopyTo(stream);
            }
            
            var url = $"{"/res/assets/ArticleImg/"}{fileName}";
            
            return Json(new { uploaded = true, url });
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public dynamic UploadImgData_F()
        {
            try
            {
                string usercode = Request.Cookies["_acccode"] ?? Request.Cookies["_cuscode"];

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
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + objCommon.GetRandomNumber(5) + "." + ContentDispositionHeaderValue.Parse(file[i].ContentDisposition).FileName.Trim('"').Split('.')[1];
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
                string usercode = Request.Cookies["_acccode"] ?? Request.Cookies["_cuscode"];

                string type = Request.Form["TYPE"];
                var file = Request.Form.Files;
                var folderName = Path.Combine(@"tectransit\dist\tectransit\assets\" + type, usercode);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                List<IDFileList> rowlist = new List<IDFileList>();
                if (file.Count > 0)
                {
                    for (int i = 0; i < file.Count; i++)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + objCommon.GetRandomNumber(5) + "." + ContentDispositionHeaderValue.Parse(file[i].ContentDisposition).FileName.Trim('"').Split('.')[1];
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