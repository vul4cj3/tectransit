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

        /* --- News --- */
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
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " TITLE LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " DESCR LIKE '%" + htData["KEYWORD"]?.ToString() + "%'";
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
                    rowTDN.Descr = sData["DESCR"]?.ToString();
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
    }
}