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
    [Route("api/UserHelp/[action]")]
    public class UserManagementController : Controller
    {
        UserManagementHelper objUsm = new UserManagementHelper();
        CommonHelper objComm = new CommonHelper();
        private readonly TECTRANSITDBContext _context;

        public UserManagementController(TECTRANSITDBContext context)
        {
            _context = context;
        }

        /* --- Rank --- */
        [HttpPost]
        public dynamic GetTSRankListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("srankcode", "RANKCODE");
                srhKey.Add("srankname", "RANKNAME");
                srhKey.Add("sranktype", "RANKTYPE");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["RANKTYPE"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " RANKTYPE = '" + htData["RANKTYPE"]?.ToString() + "'";
                
                if (!string.IsNullOrEmpty(htData["RANKCODE"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " RANKCODE LIKE '%" + htData["RANKCODE"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["RANKNAME"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " RANKNAME LIKE '%" + htData["RANKNAME"]?.ToString() + "%'";

            }

            return objUsm.GetRankListData(sWhere, pageIndex, pageSize);
        }

        [HttpPost]
        public dynamic EditTSRankEnableData([FromBody] object form)
        {
            try
            {
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
                        dataKey.Add("id", "RANKID");
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
                        UpdateRank(Convert.ToInt64(sData["RANKID"]), sData);
                    }
                }

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic GetTSRankData(long id)
        {
            return objUsm.GetRankData(id);
        }

        [HttpPost]
        public dynamic EditTSRankData([FromBody] object form)
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

                return EditRankData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        /* --- Account --- */
        [HttpPost]
        public dynamic GetTSAccountListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");

            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("susercode", "USERCODE");
                srhKey.Add("susername", "USERNAME");
                srhKey.Add("semail", "EMAIL");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["USERCODE"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " USERCODE LIKE '%" + htData["USERCODE"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["USERNAME"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " USERNAME LIKE '%" + htData["USERNAME"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["EMAIL"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " EMAIL LIKE '%" + htData["EMAIL"]?.ToString() + "%'";

            }

            return objUsm.GetAccountListData(sWhere, pageIndex, pageSize);
        }

        [HttpPost]
        public dynamic EditTSAccountData([FromBody] object form)
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

                return EditAccountData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic EditTSAccountEnableData([FromBody] object form)
        {
            try
            {
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
                        dataKey.Add("id", "USERID");
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
                        UpdateAccount(Convert.ToInt64(sData["USERID"]), sData);
                    }
                }

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic GetTSAccountData(long id)
        {
            return objUsm.GetAccountData(id);
        }

        /* Declarant and Receiver*/
        [HttpGet("{type}/{id}")]
        public dynamic GetDeclarantnReceiverData(long type, string id)
        {
            return objUsm.GetDeclarantnReceiverData(id, type);
        }

        /* --- Twotable Map --- */

        [HttpPost]
        public dynamic EditAccountRankData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                string usercode = jsonData.Value<string>("id");
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "RANKID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();

                        htData["USERCODE"] = usercode;
                        htData["_usercode"] = Request.Cookies["_usercode"];
                        htData["_username"] = Request.Cookies["_username"];
                    }

                    AL.Add(htData);
                }

                //Insert Or Delete
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        var query = _context.TSAcrankmap.Where(q => q.Rankid == Convert.ToInt64(sData["RANKID"]) && q.Usercode == sData["USERCODE"].ToString()).FirstOrDefault();
                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            if (query == null)
                            {
                                insertACRank(sData);

                                //add user log
                                objComm.AddUserControlLog(sData, $"accounts/{usercode}", "前台用戶管理 - 權限設定", 1, sData["RANKID"]?.ToString());
                            }
                        }
                        else
                        {
                            if (query != null)
                            {
                                delACRank(query);

                                //add user log
                                objComm.AddUserControlLog(sData, $"accounts/{usercode}", "前台用戶管理 - 權限設定", 3, sData["RANKID"]?.ToString());
                            }

                        }
                    }

                }

                return new { status = "99", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        /* --- private CRUD function --- */
        private dynamic EditRankData(long id, Hashtable htData)
        {
            try
            {
                if (id == 0)
                {
                    //檢查CODE是否重複
                    bool IsRepeat = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT RANKCODE AS COL1 FROM T_S_RANK WHERE RANKCODE = '{htData["RANKCODE"]}'")) ? false : true;
                    if (IsRepeat)
                        return new { status = "99", msg = "已存在相同的CODE！" };

                    InsertRank(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "rank/edit/0", "用戶權限管理", 1, htData["RANKCODE"]?.ToString());
                }
                else
                {
                    UpdateRank(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"rank/edit/{id}", "用戶權限管理", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertRank(Hashtable sData)
        {
            Hashtable htData = sData;
            int newSeq = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT RANKSEQ AS COL1 FROM T_S_RANK ORDER BY RANKSEQ DESC")) ? 1 : Convert.ToInt32(DBUtil.GetSingleValue1($@"SELECT RANKSEQ AS COL1 FROM T_S_RANK ORDER BY RANKSEQ DESC")) + 1;
            htData["RANKSEQ"] = newSeq;
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_S_RANK(RANKCODE, RANKSEQ, RANKTYPE, RANKNAME, RANKDESC, ISENABLE, CREDATE, CREATEBY, UPDDATE, UPDBY) 
                                        VALUES (@RANKCODE, @RANKSEQ, @RANKTYPE, @RANKNAME, @RANKDESC, @ISENABLE, @CREDATE, @CREATEBY, @UPDDATE, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateRank(long id, Hashtable sData)
        {
            var query = _context.TSRank.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSRank rowTSR = query;

                if (sData["RANKCODE"] != null)
                    rowTSR.Rankcode = sData["RANKCODE"]?.ToString();
                if (sData["RANKSEQ"] != null)
                    rowTSR.Rankseq = sData["RANKSEQ"]?.ToString();
                if (sData["RANKNAME"] != null)
                    rowTSR.Rankname = sData["RANKNAME"]?.ToString();
                if (sData["RANKDESC"] != null)
                    rowTSR.Rankdesc = sData["RANKDESC"]?.ToString();
                if (sData["ISENABLE"] != null)
                    rowTSR.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTSR.Upddate = DateTime.Now;
                    rowTSR.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private dynamic EditAccountData(long id, Hashtable htData)
        {
            try
            {
                if (id == 0)
                {
                    //前台用戶自行註冊新增
                }
                else
                {
                    UpdateAccount(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"accounts/edit/{id}", "前台用戶管理", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void UpdateAccount(long id, Hashtable sData)
        {
            var query = _context.TSAccount.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSAccount rowTSA = query;

                if (sData["USERCODE"] != null)
                    rowTSA.Usercode = sData["USERCODE"]?.ToString();
                if (sData["USERPASSWORD"] != null)
                    rowTSA.Userpassword = sData["USERPASSWORD"]?.ToString();
                if (sData["USERSEQ"] != null)
                    rowTSA.Userseq = sData["USERSEQ"]?.ToString();
                if (sData["USERNAME"] != null)
                    rowTSA.Username = sData["USERNAME"]?.ToString();
                if (sData["USERDESC"] != null)
                    rowTSA.Userdesc = sData["USERDESC"]?.ToString();
                if (sData["EMAIL"] != null)
                    rowTSA.Email = sData["EMAIL"]?.ToString();
                if (sData["WAREHOUSENO"] != null)
                    rowTSA.Warehouseno = sData["WAREHOUSENO"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTSA.Taxid = sData["TAXID"]?.ToString();
                if (sData["IDPHOTO_F"] != null)
                    rowTSA.IdphotoF = sData["IDPHOTO_F"]?.ToString();
                if (sData["IDPHOTO_B"] != null)
                    rowTSA.IdphotoB = sData["IDPHOTO_B"]?.ToString();
                if (sData["PHONE"] != null)
                    rowTSA.Phone = sData["PHONE"]?.ToString();
                if (sData["MOBILE"] != null)
                    rowTSA.Mobile = sData["MOBILE"]?.ToString();
                if (sData["ADDR"] != null)
                    rowTSA.Addr = sData["ADDR"]?.ToString();
                if (sData["ISENABLE"] != null)
                    rowTSA.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTSA.Upddate = DateTime.Now;
                    rowTSA.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void insertACRank(Hashtable sData)
        {
            TSAcrankmap rowAR = new TSAcrankmap();
            rowAR.Rankid = Convert.ToInt64(sData["RANKID"]);
            rowAR.Usercode = sData["USERCODE"]?.ToString();

            _context.TSAcrankmap.Add(rowAR);
            _context.SaveChanges();
        }

        private void delACRank(TSAcrankmap rm)
        {
            _context.TSAcrankmap.Remove(rm);
            _context.SaveChanges();
        }

    }
}