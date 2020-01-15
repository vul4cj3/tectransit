using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Tectransit.Datas;
using Tectransit.Modles;

namespace Tectransit.Controllers
{
    [Route("api/WebSetHelp/[action]")]
    public class WebsetController : Controller
    {
        CommonHelper objComm = new CommonHelper();
        WebsetHelper objWebs = new WebsetHelper();

        private readonly TECTRANSITDBContext _context;

        public WebsetController(TECTRANSITDBContext context)
        {
            _context = context;
        }

        /* --- Banner --- */
        #region 首頁廣告
        [HttpGet("{id}")]
        public dynamic GetBanData(long id)
        {
            return objWebs.GetBanData(id);
        }

        [HttpPost]
        public dynamic EditTDBanData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                return EditBanData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic EditTDBanTopData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "BANNERID");
                        dataKey.Add("isenable", "ISTOP");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateBan(Convert.ToInt64(sData["BANNERID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[BANNERID({sData["BANNERID"]}):{((sData["ISTOP"]?.ToString() == "1") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/banner", "首頁廣告管理-置頂變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTDBanEnableData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "BANNERID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "0" : "1";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateBan(Convert.ToInt64(sData["BANNERID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[BANNERID({sData["BANNERID"]}):{((sData["ISENABLE"]?.ToString() == "0") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/banner", "首頁廣告管理-停用變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic DelTDBanData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "BANNERID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];

                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            var query = _context.TDBanner.Where(q => q.Id == Convert.ToInt64(sData["BANNERID"])).FirstOrDefault();
                            if (query != null)
                                delBan(query);

                            logMsg += (logMsg == "" ? "" : ",") + $@"[BANNERID({sData["BANNERID"]})]";
                        }
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/banner", "首頁廣告管理", 3, logMsg);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        /* --- About Category --- */
        #region 集貨介紹-分類
        [HttpPost]
        public dynamic GetTDAboutCateListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("skeyword", "KEYWORD");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["KEYWORD"]?.ToString()))
                {
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " TITLE LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " DESCR LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                }


            }

            return objWebs.GetAboutCateListData(sWhere, pageIndex, pageSize);
        }

        [HttpPost]
        public dynamic GetTDAboutCateData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);

            return objWebs.GetTDAboutCateData(sWhere);
        }

        [HttpGet("{id}")]
        public dynamic GetAboutCateData(long id)
        {
            return objWebs.GetAboutCateData(id);
        }

        [HttpPost]
        public dynamic EditTDAboutCateData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                return EditAboutCateData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic EditTDAboutCateTopData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "CATEID");
                        dataKey.Add("isenable", "ISTOP");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateAboutCate(Convert.ToInt64(sData["CATEID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[CATEID({sData["CATEID"]}):{((sData["ISTOP"]?.ToString() == "1") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/about", "集貨介紹管理-分類-置頂變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTDAboutCateEnableData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "CATEID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "0" : "1";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateAboutCate(Convert.ToInt64(sData["CATEID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[CATEID({sData["CATEID"]}):{((sData["ISENABLE"]?.ToString() == "0") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/about", "集貨介紹管理-分類-停用變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic DelAboutCateData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                string logMsg2 = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "CATEID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];

                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            var query = _context.TDAboutH.Where(q => q.Id == Convert.ToInt64(sData["CATEID"])).FirstOrDefault();
                            if (query != null)
                            {
                                //檢查是否有細項並刪掉
                                var AL_D = _context.TDAboutD.Where(q => q.Abouthid == sData["CATEID"].ToString()).ToList();
                                if (AL_D.Count > 0)
                                {
                                    foreach (var D in AL_D)
                                    {
                                        logMsg2 += (logMsg2 == "" ? "" : ",") + $@"[ABOUTID({D.Id})]";
                                        delAbout(D);
                                    }
                                }

                                //刪掉分類
                                delAboutCate(query);
                            }

                            logMsg += (logMsg == "" ? "" : ",") + $@"[CATEID({sData["CATEID"]})]";
                        }
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/about", "集貨介紹管理-分類", 3, logMsg);
                if (!string.IsNullOrEmpty(logMsg2))
                    objComm.AddUserControlLog(logData, "/about", "集貨介紹管理-細項", 3, logMsg2);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        /* --- About --- */
        #region 集貨介紹-細項
        [HttpPost]
        public dynamic GetTDAboutListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            string cateID = jsonData.Value<string>("CID");
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("skeyword", "KEYWORD");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();


                sWhere += (sWhere == "" ? "WHERE" : " AND") + " ABOUTHID = '" + cateID + "'";

                if (!string.IsNullOrEmpty(htData["KEYWORD"]?.ToString()))
                {
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " (TITLE LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " DESCR LIKE '%" + htData["KEYWORD"]?.ToString() + "%')";
                }


            }

            return objWebs.GetAboutListData(sWhere, pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public dynamic GetAboutData(long id)
        {
            return objWebs.GetAboutData(id);
        }

        [HttpPost]
        public dynamic EditTDAboutData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                return EditAboutData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic EditTDAboutTopData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "ABOUTID");
                        dataKey.Add("isenable", "ISTOP");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateAbout(Convert.ToInt64(sData["ABOUTID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[ABOUTID({sData["ABOUTID"]}):{((sData["ISTOP"]?.ToString() == "1") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/about/info", "集貨介紹管理-細項-置頂變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTDAboutEnableData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "ABOUTID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "0" : "1";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateAbout(Convert.ToInt64(sData["ABOUTID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[ABOUTID({sData["ABOUTID"]}):{((sData["ISENABLE"]?.ToString() == "0") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/about/info", "集貨介紹管理-細項-停用變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic DelAboutData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "ABOUTID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];

                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            var query = _context.TDAboutD.Where(q => q.Id == Convert.ToInt64(sData["ABOUTID"])).FirstOrDefault();
                            if (query != null)
                            {
                                delAbout(query);
                            }

                            logMsg += (logMsg == "" ? "" : ",") + $@"[ABOUTID({sData["ABOUTID"]})]";
                        }
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/about/info", "集貨介紹管理-細項", 3, logMsg);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        /* --- News --- */
        #region 公告消息
        [HttpPost]
        public dynamic GetTDNewsListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("skeyword", "KEYWORD");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["KEYWORD"]?.ToString()))
                {
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " TITLE LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " DESCR LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                }


            }

            return objWebs.GetNewsListData(sWhere, pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public dynamic GetNewsData(long id)
        {
            return objWebs.GetNewsData(id);
        }

        [HttpPost]
        public dynamic EditTDNewsData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                #region 檢查上架日期
                if (!string.IsNullOrEmpty(htData["UPSDATE"]?.ToString()) && !string.IsNullOrEmpty(htData["UPEDATE"]?.ToString()))
                {
                    DateTime sDate = Convert.ToDateTime(htData["UPSDATE"]);
                    DateTime eDate = Convert.ToDateTime(htData["UPEDATE"]);

                    if (sDate > eDate)
                        return new { status = "99", msg = "上架日期(起)不可晚於上架日期(迄)！" };

                }
                else if (!string.IsNullOrEmpty(htData["UPSDATE"]?.ToString()))
                {
                    string tempDate = DBUtil.GetSingleValue1($@"SELECT UPEDATE AS COL1 FROM T_D_NEWS WHERE ID = {id}");
                    if (!string.IsNullOrEmpty(tempDate))
                    {
                        DateTime sDate = Convert.ToDateTime(htData["UPSDATE"]);
                        DateTime eDate = Convert.ToDateTime(tempDate);

                        if (sDate > eDate)
                            return new { status = "99", msg = "上架日期(起)不可晚於上架日期(迄)！" };
                    }

                }
                else if (!string.IsNullOrEmpty(htData["UPEDATE"]?.ToString()))
                {
                    string tempDate = DBUtil.GetSingleValue1($@"SELECT UPSDATE AS COL1 FROM T_D_NEWS WHERE ID = {id}");
                    if (!string.IsNullOrEmpty(tempDate))
                    {
                        DateTime sDate = Convert.ToDateTime(tempDate);
                        DateTime eDate = Convert.ToDateTime(htData["UPEDATE"]);

                        if (sDate > eDate)
                            return new { status = "99", msg = "上架日期(起)不可晚於上架日期(迄)！" };
                    }
                }
                else { }
                #endregion

                return EditNewsData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic EditTDNewsTopData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "NEWSID");
                        dataKey.Add("isenable", "ISTOP");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateNews(Convert.ToInt64(sData["NEWSID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[NEWSID({sData["NEWSID"]}):{((sData["ISTOP"]?.ToString() == "1") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/news", "公告消息管理-置頂變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTDNewsEnableData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "NEWSID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "0" : "1";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateNews(Convert.ToInt64(sData["NEWSID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[NEWSID({sData["NEWSID"]}):{((sData["ISENABLE"]?.ToString() == "0") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/news", "公告消息管理-停用變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic DelNewsData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "NEWSID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];

                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            var query = _context.TDNews.Where(q => q.Id == Convert.ToInt64(sData["NEWSID"])).FirstOrDefault();
                            if (query != null)
                                delNews(query);

                            logMsg += (logMsg == "" ? "" : ",") + $@"[NEWSID({sData["NEWSID"]})]";
                        }
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/news", "公告消息管理", 3, logMsg);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        /* --- Faq Category --- */
        #region 常見問題-分類
        [HttpPost]
        public dynamic GetTDFaqCateListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("skeyword", "KEYWORD");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["KEYWORD"]?.ToString()))
                {
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " TITLE LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " DESCR LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                }


            }

            return objWebs.GetFaqCateListData(sWhere, pageIndex, pageSize);
        }

        [HttpPost]
        public dynamic GetTDFaqCateData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);

            return objWebs.GetTDFaqCateData(sWhere);
        }

        [HttpGet("{id}")]
        public dynamic GetFaqCateData(long id)
        {
            return objWebs.GetFaqCateData(id);
        }

        [HttpPost]
        public dynamic EditTDFaqCateData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                return EditFaqCateData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic EditTDFaqCateTopData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "CATEID");
                        dataKey.Add("isenable", "ISTOP");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateFaqCate(Convert.ToInt64(sData["CATEID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[CATEID({sData["CATEID"]}):{((sData["ISTOP"]?.ToString() == "1") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/faq", "常見問題管理-分類-置頂變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTDFaqCateEnableData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "CATEID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "0" : "1";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateFaqCate(Convert.ToInt64(sData["CATEID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[CATEID({sData["CATEID"]}):{((sData["ISENABLE"]?.ToString() == "0") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/faq", "常見問題管理-分類-停用變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic DelFaqCateData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                string logMsg2 = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "CATEID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];

                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            var query = _context.TDFaqH.Where(q => q.Id == Convert.ToInt64(sData["CATEID"])).FirstOrDefault();
                            if (query != null)
                            {
                                //檢查是否有細項並刪掉
                                var AL_D = _context.TDFaqD.Where(q => q.Faqhid == sData["CATEID"].ToString()).ToList();
                                if (AL_D.Count > 0)
                                {
                                    foreach (var D in AL_D)
                                    {
                                        logMsg2 += (logMsg2 == "" ? "" : ",") + $@"[FAQID({D.Id})]";
                                        delFaq(D);
                                    }
                                }

                                //刪掉分類
                                delFaqCate(query);
                            }

                            logMsg += (logMsg == "" ? "" : ",") + $@"[CATEID({sData["CATEID"]})]";
                        }
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/faq", "常見問題管理-分類", 3, logMsg);
                if (!string.IsNullOrEmpty(logMsg2))
                    objComm.AddUserControlLog(logData, "/faq", "常見問題管理-細項", 3, logMsg2);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        /* --- Faq --- */
        #region 常見問題-細項
        [HttpPost]
        public dynamic GetTDFaqListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            string cateID = jsonData.Value<string>("CID");
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("skeyword", "KEYWORD");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();


                sWhere += (sWhere == "" ? "WHERE" : " AND") + " FAQHID = '" + cateID + "'";

                if (!string.IsNullOrEmpty(htData["KEYWORD"]?.ToString()))
                {
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " (TITLE LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " DESCR LIKE '%" + htData["KEYWORD"]?.ToString() + "%')";
                }


            }

            return objWebs.GetFaqListData(sWhere, pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public dynamic GetFaqData(long id)
        {
            return objWebs.GetFaqData(id);
        }

        [HttpPost]
        public dynamic EditTDFaqData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                return EditFaqData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic EditTDFaqTopData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "FAQID");
                        dataKey.Add("isenable", "ISTOP");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateFaq(Convert.ToInt64(sData["FAQID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[FAQID({sData["ABOUTID"]}):{((sData["ISTOP"]?.ToString() == "1") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/faq/info", "常見問題管理-細項-置頂變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTDFaqEnableData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "FAQID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "0" : "1";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateFaq(Convert.ToInt64(sData["FAQID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[FAQID({sData["FAQID"]}):{((sData["ISENABLE"]?.ToString() == "0") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/faq/info", "常見問題管理-細項-停用變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic DelFaqData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "FAQID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];

                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            var query = _context.TDFaqD.Where(q => q.Id == Convert.ToInt64(sData["FAQID"])).FirstOrDefault();
                            if (query != null)
                            {
                                delFaq(query);
                            }

                            logMsg += (logMsg == "" ? "" : ",") + $@"[FAQID({sData["FAQID"]})]";
                        }
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/faq/info", "常見問題管理-細項", 3, logMsg);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        /* --- Station --- */
        #region 集貨站

        [HttpPost]
        public dynamic GetTSStationListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("skeyword", "KEYWORD");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["KEYWORD"]?.ToString()))
                {
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " STATIONCODE LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                    sWhere += (sWhere == "" ? "WHERE" : " OR") + " STATIONNAME LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                }
                
            }

            return objWebs.GetStationListData(sWhere, pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public dynamic GetStationData(long id)
        {
            return objWebs.GetStationData(id);
        }

        [HttpPost]
        public dynamic EditTSStationData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];
                
                return EditStationData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic DelStationData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "STATIONID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];

                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            var query = _context.TSStation.Where(q => q.Id == Convert.ToInt64(sData["STATIONID"])).FirstOrDefault();
                            if (query != null)
                                delStation(query);

                            logMsg += (logMsg == "" ? "" : ",") + $@"[STATIONID({sData["STATIONID"]})]";
                        }
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/station", "集貨站管理", 3, logMsg);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        /* --- private CRUD function --- */

        private dynamic EditBanData(long id, Hashtable htData)
        {
            try
            {
                if (id == 0)
                {

                    InsertBan(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "banner/edit/0", "首頁廣告管理", 1, htData["BANID"]?.ToString());
                }
                else
                {
                    UpdateBan(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"banner/edit/{id}", "首頁廣告管理", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertBan(Hashtable sData)
        {
            Hashtable htData = sData;
            int newSeq = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT BANSEQ AS COL1 FROM T_D_BANNER ORDER BY BANSEQ DESC")) ? 1 : Convert.ToInt32(DBUtil.GetSingleValue1($@"SELECT BANSEQ AS COL1 FROM T_D_BANNER ORDER BY BANSEQ DESC")) + 1;
            htData["BANSEQ"] = newSeq;
            htData["ISTOP"] = htData["ISTOP"]?.ToString() == "1" ? true : false;
            htData["ISENABLE"] = htData["ISENABLE"]?.ToString() == "1" ? true : false;
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_D_BANNER(TITLE, DESCR, IMGURL, URL, BANSEQ, UPSDATE, UPEDATE, ISTOP, ISENABLE, CREDATE, UPDDATE, CREATEBY, UPDBY) 
                                        VALUES (@TITLE, @DESCR, @IMGURL, @URL, @BANSEQ, @UPSDATE, @UPEDATE, @ISTOP, @ISENABLE, @CREDATE, @UPDDATE, @CREATEBY, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateBan(long id, Hashtable sData)
        {
            var query = _context.TDBanner.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TDBanner rowTDB = query;

                if (sData["TITLE"] != null)
                    rowTDB.Title = sData["TITLE"]?.ToString();
                if (sData["DESCR"] != null)
                    rowTDB.Descr = sData["DESCR"]?.ToString();
                if (sData["IMGURL"] != null)
                    rowTDB.Imgurl = sData["IMGURL"]?.ToString();
                if (sData["URL"] != null)
                    rowTDB.Url = sData["URL"]?.ToString();
                if (sData["UPSDATE"] != null)
                    rowTDB.Upsdate = sData["UPSDATE"]?.ToString();
                if (sData["UPEDATE"] != null)
                    rowTDB.Upedate = sData["UPEDATE"]?.ToString();
                if (sData["ISTOP"] != null)
                    rowTDB.Istop = sData["ISTOP"]?.ToString() == "1" ? true : false;
                if (sData["ISENABLE"] != null)
                    rowTDB.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTDB.Upddate = DateTime.Now;
                    rowTDB.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void delBan(TDBanner rm)
        {
            _context.TDBanner.Remove(rm);
            _context.SaveChanges();
        }

        private dynamic EditNewsData(long id, Hashtable htData)
        {
            try
            {

                if (id == 0)
                {

                    InsertNews(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "news/edit/0", "公告消息管理", 1, htData["NEWSID"]?.ToString());
                }
                else
                {
                    UpdateNews(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"news/edit/{id}", "公告消息管理", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertNews(Hashtable sData)
        {
            Hashtable htData = sData;
            //Encode context
            htData["DESCR"] = HttpUtility.HtmlEncode(htData["DESCR"]);
            htData["ISTOP"] = htData["ISTOP"]?.ToString() == "1" ? true : false;
            htData["ISENABLE"] = htData["ISENABLE"]?.ToString() == "1" ? true : false;
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_D_NEWS(TITLE, DESCR, NEWSSEQ, UPSDATE, UPEDATE, ISTOP, ISENABLE, CREDATE, UPDDATE, CREATEBY, UPDBY) 
                                        VALUES (@TITLE, @DESCR, @NEWSSEQ, @UPSDATE, @UPEDATE, @ISTOP, @ISENABLE, @CREDATE, @UPDDATE, @CREATEBY, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateNews(long id, Hashtable sData)
        {
            var query = _context.TDNews.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TDNews rowTDN = query;

                if (sData["TITLE"] != null)
                    rowTDN.Title = sData["TITLE"]?.ToString();
                if (sData["DESCR"] != null)
                    rowTDN.Descr = HttpUtility.HtmlEncode(sData["DESCR"]?.ToString());//Encode context
                if (sData["UPSDATE"] != null)
                    rowTDN.Upsdate = sData["UPSDATE"]?.ToString();
                if (sData["UPEDATE"] != null)
                    rowTDN.Upedate = sData["UPEDATE"]?.ToString();
                if (sData["ISTOP"] != null)
                    rowTDN.Istop = sData["ISTOP"]?.ToString() == "1" ? true : false;
                if (sData["ISENABLE"] != null)
                    rowTDN.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTDN.Upddate = DateTime.Now;
                    rowTDN.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void delNews(TDNews rm)
        {
            _context.TDNews.Remove(rm);
            _context.SaveChanges();
        }

        private dynamic EditAboutCateData(long id, Hashtable htData)
        {
            try
            {

                if (id == 0)
                {

                    InsertAboutCate(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "about/edit/0", "集貨介紹管理-分類", 1, htData["CATEID"]?.ToString());
                }
                else
                {
                    UpdateAboutCate(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"about/edit/{id}", "集貨介紹管理-分類", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertAboutCate(Hashtable sData)
        {
            Hashtable htData = sData;
            htData["ISTOP"] = false;
            htData["ISENABLE"] = htData["ISENABLE"]?.ToString() == "1" ? true : false;
            htData["ABOUTHSEQ"] = htData["ABOUTSEQ"];
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_D_ABOUT_H(TITLE, DESCR, ABOUTHSEQ, ISTOP, ISENABLE, CREDATE, UPDDATE, CREATEBY, UPDBY) 
                                        VALUES (@TITLE, @DESCR, @ABOUTHSEQ, @ISTOP, @ISENABLE, @CREDATE, @UPDDATE, @CREATEBY, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateAboutCate(long id, Hashtable sData)
        {
            var query = _context.TDAboutH.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TDAboutH rowTDA = query;

                if (sData["TITLE"] != null)
                    rowTDA.Title = sData["TITLE"]?.ToString();
                if (sData["DESCR"] != null)
                    rowTDA.Descr = sData["DESCR"]?.ToString();
                if (sData["ABOUTSEQ"] != null)
                    rowTDA.Abouthseq = sData["ABOUTSEQ"]?.ToString();
                if (sData["ISTOP"] != null)
                    rowTDA.Istop = sData["ISTOP"]?.ToString() == "1" ? true : false;
                if (sData["ISENABLE"] != null)
                    rowTDA.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTDA.Upddate = DateTime.Now;
                    rowTDA.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void delAboutCate(TDAboutH rm)
        {
            _context.TDAboutH.Remove(rm);
            _context.SaveChanges();
        }

        private dynamic EditAboutData(long id, Hashtable htData)
        {
            try
            {

                if (id == 0)
                {

                    InsertAbout(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "about/infoedit/0", "集貨介紹管理-細項", 1, htData["ABOUTID"]?.ToString());
                }
                else
                {
                    UpdateAbout(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"about/infoedit/{id}", "集貨介紹管理-細項", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertAbout(Hashtable sData)
        {
            Hashtable htData = sData;
            //encode context
            htData["DESCR"] = HttpUtility.HtmlEncode(htData["DESCR"]);
            htData["ISTOP"] = htData["ISTOP"]?.ToString() == "1" ? true : false;
            htData["ISENABLE"] = htData["ISENABLE"]?.ToString() == "1" ? true : false;
            htData["ABOUTDSEQ"] = htData["ABOUTSEQ"];
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];
            htData["ABOUTHID"] = htData["CATEID"];

            string sql = @"INSERT INTO T_D_ABOUT_D(TITLE, DESCR, ABOUTDSEQ, ISTOP, ISENABLE, CREDATE, UPDDATE, CREATEBY, UPDBY, ABOUTHID) 
                                        VALUES (@TITLE, @DESCR, @ABOUTDSEQ, @ISTOP, @ISENABLE, @CREDATE, @UPDDATE, @CREATEBY, @UPDBY, @ABOUTHID)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateAbout(long id, Hashtable sData)
        {
            var query = _context.TDAboutD.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TDAboutD rowTDA = query;

                if (sData["TITLE"] != null)
                    rowTDA.Title = sData["TITLE"]?.ToString();
                if (sData["DESCR"] != null)
                    rowTDA.Descr = HttpUtility.HtmlEncode(sData["DESCR"]?.ToString()); //encode context
                if (sData["ABOUTSEQ"] != null)
                    rowTDA.Aboutdseq = sData["ABOUTSEQ"]?.ToString();
                if (sData["ISTOP"] != null)
                    rowTDA.Istop = sData["ISTOP"]?.ToString() == "1" ? true : false;
                if (sData["ISENABLE"] != null)
                    rowTDA.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTDA.Upddate = DateTime.Now;
                    rowTDA.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void delAbout(TDAboutD rm)
        {
            _context.TDAboutD.Remove(rm);
            _context.SaveChanges();
        }

        private dynamic EditFaqCateData(long id, Hashtable htData)
        {
            try
            {

                if (id == 0)
                {

                    InsertFaqCate(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "faq/edit/0", "常見問題管理-分類", 1, htData["CATEID"]?.ToString());
                }
                else
                {
                    UpdateFaqCate(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"faq/edit/{id}", "常見問題管理-分類", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertFaqCate(Hashtable sData)
        {
            Hashtable htData = sData;
            htData["ISTOP"] = false;
            htData["ISENABLE"] = htData["ISENABLE"]?.ToString() == "1" ? true : false;
            htData["FAQHSEQ"] = htData["FAQSEQ"];
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_D_FAQ_H(TITLE, DESCR, FAQHSEQ, ISTOP, ISENABLE, CREDATE, UPDDATE, CREATEBY, UPDBY) 
                                        VALUES (@TITLE, @DESCR, @FAQHSEQ, @ISTOP, @ISENABLE, @CREDATE, @UPDDATE, @CREATEBY, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateFaqCate(long id, Hashtable sData)
        {
            var query = _context.TDFaqH.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TDFaqH rowTDF = query;

                if (sData["TITLE"] != null)
                    rowTDF.Title = sData["TITLE"]?.ToString();
                if (sData["DESCR"] != null)
                    rowTDF.Descr = sData["DESCR"]?.ToString();
                if (sData["FAQSEQ"] != null)
                    rowTDF.Faqhseq = sData["FAQSEQ"]?.ToString();
                if (sData["ISTOP"] != null)
                    rowTDF.Istop = sData["ISTOP"]?.ToString() == "1" ? true : false;
                if (sData["ISENABLE"] != null)
                    rowTDF.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTDF.Upddate = DateTime.Now;
                    rowTDF.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void delFaqCate(TDFaqH rm)
        {
            _context.TDFaqH.Remove(rm);
            _context.SaveChanges();
        }

        private dynamic EditFaqData(long id, Hashtable htData)
        {
            try
            {

                if (id == 0)
                {

                    InsertFaq(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "faq/infoedit/0", "常見問題管理-細項", 1, htData["FAQID"]?.ToString());
                }
                else
                {
                    UpdateFaq(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"faq/infoedit/{id}", "常見問題管理-細項", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertFaq(Hashtable sData)
        {
            Hashtable htData = sData;
            //encode context
            htData["DESCR"] = HttpUtility.HtmlEncode(htData["DESCR"]);
            htData["ISTOP"] = htData["ISTOP"]?.ToString() == "1" ? true : false;
            htData["ISENABLE"] = htData["ISENABLE"]?.ToString() == "1" ? true : false;
            htData["FAQDSEQ"] = htData["FAQSEQ"];
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];
            htData["FAQHID"] = htData["CATEID"];

            string sql = @"INSERT INTO T_D_FAQ_D(TITLE, DESCR, FAQDSEQ, ISTOP, ISENABLE, CREDATE, UPDDATE, CREATEBY, UPDBY, FAQHID) 
                                        VALUES (@TITLE, @DESCR, @FAQDSEQ, @ISTOP, @ISENABLE, @CREDATE, @UPDDATE, @CREATEBY, @UPDBY, @FAQHID)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateFaq(long id, Hashtable sData)
        {
            var query = _context.TDFaqD.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TDFaqD rowTDF = query;

                if (sData["TITLE"] != null)
                    rowTDF.Title = sData["TITLE"]?.ToString();
                if (sData["DESCR"] != null)
                    rowTDF.Descr = HttpUtility.HtmlEncode(sData["DESCR"]?.ToString()); //encode context
                if (sData["FAQSEQ"] != null)
                    rowTDF.Faqdseq = sData["FAQSEQ"]?.ToString();
                if (sData["ISTOP"] != null)
                    rowTDF.Istop = sData["ISTOP"]?.ToString() == "1" ? true : false;
                if (sData["ISENABLE"] != null)
                    rowTDF.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTDF.Upddate = DateTime.Now;
                    rowTDF.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void delFaq(TDFaqD rm)
        {
            _context.TDFaqD.Remove(rm);
            _context.SaveChanges();
        }

        private dynamic EditStationData(long id, Hashtable htData)
        {
            try
            {

                if (id == 0)
                {

                    InsertStation(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "station/edit/0", "集貨站管理", 1, htData["STATIONID"]?.ToString());
                }
                else
                {
                    UpdateStation(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"station/edit/{id}", "集貨站管理", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }
        private void InsertStation(Hashtable sData)
        {
            Hashtable htData = sData;
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_S_STATION(STATIONCODE, STATIONNAME, COUNTRYCODE, RECEIVER, PHONE, MOBILE, ADDRESS, STATIONSEQ, REMARK, CREDATE, UPDDATE, CREATEBY, UPDBY) 
                                        VALUES (@STATIONCODE, @STATIONNAME, @COUNTRYCODE, @RECEIVER, @PHONE, @MOBILE, @ADDRESS, @STATIONSEQ, @REMARK, @CREDATE, @UPDDATE, @CREATEBY, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateStation(long id, Hashtable sData)
        {
            var query = _context.TSStation.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSStation rowTSS = query;

                if (sData["STATIONNAME"] != null)
                    rowTSS.Stationname = sData["STATIONNAME"]?.ToString();
                if (sData["COUNTRYCODE"] != null)
                    rowTSS.Countrycode = sData["COUNTRYCODE"]?.ToString();
                if (sData["RECEIVER"] != null)
                    rowTSS.Receiver = sData["RECEIVER"]?.ToString();
                if (sData["PHONE"] != null)
                    rowTSS.Phone = sData["PHONE"]?.ToString();
                if (sData["MOBILE"] != null)
                    rowTSS.Mobile = sData["MOBILE"]?.ToString();
                if (sData["ADDRESS"] != null)
                    rowTSS.Address = sData["ADDRESS"]?.ToString();
                if (sData["STATIONSEQ"] != null)
                    rowTSS.Stationseq = sData["STATIONSEQ"]?.ToString();
                if (sData["REMARK"] != null)
                    rowTSS.Remark = sData["REMARK"]?.ToString();

                if (sData.Count > 2)//排除cookies
                {
                    rowTSS.Upddate = DateTime.Now;
                    rowTSS.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void delStation(TSStation rm)
        {
            _context.TSStation.Remove(rm);
            _context.SaveChanges();
        }
    }
}