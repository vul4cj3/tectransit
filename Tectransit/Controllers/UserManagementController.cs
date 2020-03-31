using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Tectransit.Datas;
using Tectransit.Modles;

namespace Tectransit.Controllers
{
    [Route("api/UserHelp/[action]")]
    public class UserManagementController : Controller
    {
        public IConfiguration _configuration { get; }
        UserManagementHelper objUsm = new UserManagementHelper();
        CommonHelper objComm = new CommonHelper();
        private readonly TECTRANSITDBContext _context;
        

        public UserManagementController(IConfiguration Configuration, TECTRANSITDBContext context)
        {
            _configuration = Configuration;
            _context = context;
        }

        #region /* --- Rank --- */
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
                {
                    if (htData["RANKTYPE"]?.ToString() != "ALL")
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " RANKTYPE = '" + htData["RANKTYPE"]?.ToString() + "'";
                    else
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " RANKTYPE != '1'";
                }

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
                string logMsg = "";
                bool IsCompany = false;
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

                        // log use
                        logMsg += (logMsg == "" ? "" : ",") + $"[RANKID({sData["RANKID"]}):{((sData["ISENABLE"]?.ToString() == "0") ? "true" : "false")}]";
                        IsCompany = DBUtil.GetSingleValue1($@"SELECT RANKTYPE AS COL1 FROM T_S_RANK WHERE ID = {sData["RANKID"]}") == "2" ? true : false;
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, (IsCompany ? "/companyrank" : "/rank"), (IsCompany ? "廠商權限管理" : "用戶權限管理") + "-停用變更", 2, logMsg);

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

        /* --- TWOTABLE MAP --- */

        [HttpPost]
        public dynamic EditRankMenuData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                string rankid = jsonData.Value<string>("id");
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "MENUCODE");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();

                        htData["RANKID"] = rankid;
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
                        var query = _context.TSRankmenumap.Where(q => q.Rankid == Convert.ToInt64(sData["RANKID"]) && q.Menucode == sData["MENUCODE"].ToString()).FirstOrDefault();
                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            if (query == null)
                            {
                                insertRankMenu(sData);

                                //add user log
                                objComm.AddUserControlLog(sData, $"ranks/{rankid}", "廠商權限管理 - 選單權限", 1, sData["MENUCODE"]?.ToString());
                            }
                        }
                        else
                        {
                            if (query != null)
                            {
                                delRankMenu(query);

                                //add user log
                                objComm.AddUserControlLog(sData, $"ranks/{rankid}", "廠商權限管理 - 選單權限", 3, sData["MENUCODE"]?.ToString());
                            }

                        }
                    }                    

                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void insertRankMenu(Hashtable sData)
        {
            TSRankmenumap rowRM = new TSRankmenumap();
            rowRM.Rankid = Convert.ToInt64(sData["RANKID"]);
            rowRM.Menucode = sData["MENUCODE"]?.ToString();

            _context.TSRankmenumap.Add(rowRM);
            _context.SaveChanges();
        }

        private void delRankMenu(TSRankmenumap rm)
        {
            _context.TSRankmenumap.Remove(rm);
            _context.SaveChanges();
        }
        #endregion

        #region /* --- Account --- */
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
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.USERCODE LIKE '%" + htData["USERCODE"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["USERNAME"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.USERNAME LIKE '%" + htData["USERNAME"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["EMAIL"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.EMAIL LIKE '%" + htData["EMAIL"]?.ToString() + "%'";

                sWhere += (sWhere == "" ? "WHERE" : " AND") + " C.RANKTYPE = 1";

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

                        logMsg += (logMsg == "" ? "" : ",") + $@"[USERID({sData["USERID"]}):{((sData["ISENABLE"]?.ToString() == "0") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/accounts", "前台用戶管理-停用變更", 2, logMsg);

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

        #endregion

        #region /* --- Company ---*/
        [HttpPost]
        public dynamic GetTSCompanyListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");

            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("sranktype", "RANKTYPE");
                srhKey.Add("susercode", "USERCODE");
                srhKey.Add("susername", "USERNAME");
                srhKey.Add("semail", "EMAIL");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();
                
                if (!string.IsNullOrEmpty(htData["USERCODE"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.USERCODE LIKE '%" + htData["USERCODE"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["USERNAME"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.USERNAME LIKE '%" + htData["USERNAME"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["EMAIL"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.EMAIL LIKE '%" + htData["EMAIL"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["RANKTYPE"]?.ToString()))
                {
                    if (htData["RANKTYPE"]?.ToString() != "ALL")
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " C.RANKTYPE = '" + htData["RANKTYPE"]?.ToString() + "'";
                    else
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " C.RANKTYPE != '1'";
                }

            }

            return objUsm.GetCompanyListData(sWhere, pageIndex, pageSize);
        }
        

        [HttpPost]
        public dynamic EditTSCompanyEnableData([FromBody] object form)
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

                        logMsg += (logMsg == "" ? "" : ",") + $@"[USERID({sData["USERID"]}):{((sData["ISENABLE"]?.ToString() == "0") ? "true" : "false")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/company", "廠商管理-停用變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }
        #endregion

        #region /* --- Declarant and Receiver --- */
        [HttpGet("{type}/{id}")]
        public dynamic GetDeclarantnReceiverData(long type, string id)
        {
            return objUsm.GetDeclarantnReceiverData(id, type);
        }

        /* --- Transfer --- */
        [HttpPost]
        public dynamic GetTETransferListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("sstationcode", "STATIONCODE");
                srhKey.Add("stransferno", "TRANSFERNO");
                srhKey.Add("sacccode", "ACCOUNTCODE");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["STATIONCODE"]?.ToString()))
                    if (htData["STATIONCODE"]?.ToString() != "ALL")
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.STATIONCODE = '" + htData["STATIONCODE"]?.ToString() + "'";

                if (!string.IsNullOrEmpty(htData["TRANSFERNO"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.TRANSFERNO LIKE '%" + htData["TRANSFERNO"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["ACCOUNTCODE"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " B.USERCODE LIKE '%" + htData["ACCOUNTCODE"]?.ToString() + "%'";

            }

            return objUsm.GetTransferListData(sWhere, pageIndex, pageSize);
        }

        [HttpPost]
        public dynamic EditTransferStatus_instore([FromBody] object form)
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
                        dataKey.Add("id", "ID");
                        dataKey.Add("isenable", "STATUS");
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
                        UpdateTransferM(sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[TRANSFERID({sData["ID"]}):{((sData["STATUS"]?.ToString() == "1") ? "已入庫" : "未入庫")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/transfer", "快遞單管理-入庫狀態變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTransferStatus_unstore([FromBody] object form)
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
                        dataKey.Add("id", "ID");
                        dataKey.Add("isenable", "STATUS");
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
                        UpdateTransferM(sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[TRANSFERID({sData["ID"]}):{((sData["STATUS"]?.ToString() == "1") ? "已入庫" : "未入庫")}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/transfer", "快遞單管理-入庫狀態變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        public dynamic DelTETransferData([FromBody]object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JValue temp = (JValue)arrData[i];

                    AL.Add(temp);
                }
                
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = new Hashtable();
                        sData["TRANSFERIDM"] = AL[i].ToString();
                        sData["TRANSFERNO"] = DBUtil.GetSingleValue1($@"SELECT TRANSFERNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {sData["SHIPPINGIDM"]}");
                        sData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ACCOUNTID AS COL1 FROM T_V_SHIPPING_M WHERE ID = {sData["SHIPPINGIDM"]}");
                        sData["ACCOUNTNAME"] = DBUtil.GetSingleValue1($@"SELECT ACCOUNTNAME AS COL1 FROM T_S_ACCOUNT WHERE ID = {sData["ACCOUNTID"]}");
                        
                        //delete Declarant data
                        DeleteTEData_All("T_E_DECLARANT", sData);

                        //delete transfer_H & transfer_D data
                        DeleteTEData_All("T_E_TRANSFER_D", sData);
                        DeleteTEData_All("T_E_TRANSFER_H", sData);

                        //delete transfer_M data
                        DeleteTEData_All("T_E_TRANSFER_M", sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[TRANSFERIDM({sData["TRANSFERIDM"]})=ACCOUNTNAME:{sData["ACCOUNTNAME"]?.ToString()}/TRANSFERNO: {sData["TRANSFERNO"]?.ToString()}]";
                        
                    }
                    
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/transfer", "快遞單管理-刪除", 3, logMsg);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }

        #endregion

        #region /* --- ShippingCus Data --- */
        [HttpPost]
        public dynamic GetTVShippingMListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");

            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("scompany", "COMPANY");
                srhKey.Add("sshippingno", "SHIPPINGNO");
                srhKey.Add("smawbno", "MAWBNO");
                srhKey.Add("sacccode", "ACCOUNTCODE");
                srhKey.Add("sstatus", "STATUS");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                {

                    htData[srhKey[t.Key]] = t.Value?.ToString();
                }
                
                if (!string.IsNullOrEmpty(htData["COMPANY"]?.ToString()))
                {
                    string sql = $@"SELECT DISTINCT A.ID FROM T_S_ACCOUNT A
                                    LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE
                                    LEFT JOIN T_S_RANK C ON B.RANKID = C.ID
                                    WHERE C.RANKTYPE = '2' AND A.COMPANYNAME LIKE '%{htData["COMPANY"]?.ToString()}%'";
                    DataTable DT = DBUtil.SelectDataTable(sql);
                    string tempACID = "";
                    if (DT.Rows.Count > 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            tempACID += (tempACID == "" ? "" : ",") + DT.Rows[i]["ID"];
                        }
                        
                    }
                    else { tempACID = "0"; }
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " ACCOUNTID IN (" + tempACID + ")";
                }

                if (!string.IsNullOrEmpty(htData["SHIPPINGNO"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " SHIPPINGNO LIKE '%" + htData["SHIPPINGNO"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["MAWBNO"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " MAWBNO LIKE '%" + htData["MAWBNO"]?.ToString() + "%'";
                

                //if (!string.IsNullOrEmpty(htData["ACCOUNTCODE"]?.ToString()))
                //{
                //    string sql = $@"SELECT A.ID AS COL1 FROM T_S_ACCOUNT A
                //                    LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE
                //                    LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                //                    WHERE C.RANKTYPE = 2 AND A.USERCODE = '{htData["ACCOUNTCODE"]?.ToString()}'";

                //    string acid = DBUtil.GetSingleValue1(sql);
                //    if (!string.IsNullOrEmpty(acid))
                //        sWhere += (sWhere == "" ? "WHERE" : " AND") + " ACCOUNTID = " + acid;
                //}

                sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATUS = " + htData["STATUS"]?.ToString();

            }

            return objUsm.GetTVShippingMListData(sWhere, pageIndex, pageSize);
        }

        //取得集運單(單筆)
        [HttpGet("{id}")]
        public dynamic GetSingleShippingCusData(long id)
        {
            try
            {
                Hashtable htData = new Hashtable();
                htData["SHIPPINGIDM"] = id.ToString();
                htData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ACCOUNTID AS COL1 FROM T_V_SHIPPING_M WHERE ID = {htData["SHIPPINGIDM"]}");

                return objUsm.GetSingleShippingCusData(htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        [HttpGet]
        public dynamic GetBrokerData()
        {
            try
            {
                return objUsm.GetBrokerData();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTVShippingMStatus([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");
                string status = jsonData.Value<string>("status");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JValue temp = (JValue)arrData[i];
                    
                    AL.Add(temp);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = new Hashtable();
                        sData["SHIPPINGIDM"] = AL[i].ToString();
                        sData["STATUS"] = status;
                        UpdateShippingMStatus(sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[SHIPPINGIDM({sData["SHIPPINGIDM"]}):STATUS - {sData["STATUS"]?.ToString()}]";
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/shippingcus", "廠商集運單管理-狀態變更", 2, logMsg);

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        public dynamic DelTVShippingMData([FromBody]object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JValue temp = (JValue)arrData[i];

                    AL.Add(temp);
                }

                List<string> SHIPPINGNO = new List<string>();
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = new Hashtable();
                        sData["SHIPPINGIDM"] = AL[i].ToString();
                        sData["TRANSFERNO"] = DBUtil.GetSingleValue1($@"SELECT TRANSFERNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {sData["SHIPPINGIDM"]}");
                        sData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ACCOUNTID AS COL1 FROM T_V_SHIPPING_M WHERE ID = {sData["SHIPPINGIDM"]}");
                        sData["COMPANYNAME"] = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE ID = {sData["ACCOUNTID"]}");

                        string tempShipping = DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {sData["SHIPPINGIDM"]} AND STATUS = 0");

                        //delete Declarant data
                        DeleteTVData_All("T_V_DECLARANT", sData);

                        //delete shipping_H & shipping_D data
                        DeleteTVData_All("T_V_SHIPPING_D", sData);
                        DeleteTVData_All("T_V_SHIPPING_H", sData);

                        //delete shipping_M data
                        DeleteTVData_All("T_V_SHIPPING_M", sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[SHIPPINGIDM({sData["SHIPPINGIDM"]})=COMPANYNAME:{sData["COMPANYNAME"]?.ToString()}/TRANSFERNO: {sData["TRANSFERNO"]?.ToString()}]";

                        if (!string.IsNullOrEmpty(tempShipping))
                            SHIPPINGNO.Add(tempShipping);
                    }

                    //寫入異動拋轉紀錄
                    if (SHIPPINGNO.Count() > 0)
                    {
                        for (int k = 0; k < SHIPPINGNO.Count(); k++)
                            objComm.InsertDepotRecord(2, SHIPPINGNO[k]);
                    }
                }

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                objComm.AddUserControlLog(logData, "/shippingcus", "廠商集運單管理-刪除", 3, logMsg);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTVShippingMBroker([FromBody] object form)
        {
            try
            {
                string Shippingno = "";
                var jsonData = JObject.FromObject(form);

                Hashtable htData = new Hashtable();
                if (jsonData.Value<string>("imid") != "0")
                    htData["IMBROKERID"] = jsonData.Value<string>("imid");
                if (jsonData.Value<string>("exid") != "0")
                    htData["EXBROKERID"] = jsonData.Value<string>("exid");
                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                JArray arrData = jsonData.Value<JArray>("formdata");                
                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JValue temp = (JValue)arrData[i];

                    AL.Add(temp);
                }

                if (AL.Count > 0)
                {
                    for (int j = 0; j < AL.Count; j++)
                    {
                        //未入庫狀態的才可更新
                        htData["ID"] = AL[j].ToString();
                        string tempSno = DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID AND STATUS = 0", htData);
                        if (!string.IsNullOrEmpty(tempSno))
                        {
                            updateCusShippingM(htData);
                            
                            Shippingno += (Shippingno == "" ? "" : ",") + tempSno;
                        }

                    }

                    #region 寄信通知進出口報關行(未入庫狀態才寄)
                    string F_User = "TEC Website System<ebs.sys@t3ex-group.com>";
                    string subject = $"TEC代運平台 - 報關資料通知";
                    string domainUrl = _configuration.GetSection("WebsitSetting")["domain"];
                    string C_User = _configuration.GetSection("WebsitSetting")["csMail"];
                    string body = "";
                    body += $"<p>集運單號：{Shippingno}</p>";
                    body += $"<p>報關資料已上傳，請至<a href='{domainUrl}cuslogin' target='_blank'>台灣空運後台系統</a>下載！</p>";
                    body += "<p style='color:#ff0000'>[此為系統自動寄送信件，請勿直接回覆，謝謝！]</p>";

                    string IMACID = DBUtil.GetSingleValue1($@"SELECT IMBROKERID AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID AND STATUS = 0", htData);
                    string EXACID = DBUtil.GetSingleValue1($@"SELECT EXBROKERID AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID AND STATUS = 0", htData);

                    if (!string.IsNullOrEmpty(IMACID) && IMACID != "0")
                    {
                        string T_User = DBUtil.GetSingleValue1($@"SELECT EMAIL AS COL1 FROM T_S_ACCOUNT WHERE ID = {IMACID}");
                        if (!string.IsNullOrEmpty(T_User))
                            objComm.SendMail(F_User, T_User, subject, body, C_User);
                    }

                    if (!string.IsNullOrEmpty(EXACID) && EXACID != "0")
                    {
                        string T_User = DBUtil.GetSingleValue1($@"SELECT EMAIL AS COL1 FROM T_S_ACCOUNT WHERE ID = {EXACID}");
                        if (!string.IsNullOrEmpty(T_User))
                            objComm.SendMail(F_User, T_User, subject, body, C_User);
                    }
                    #endregion

                    return new { status = "99", msg = "分配完成，並已寄送通知信給報關行！" };
                }
                
                return new { status = "99", msg = "操作失敗！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "操作失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditShippingCusData([FromBody]object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray hidList = jsonData.Value<JArray>("delH");
                JArray didList = jsonData.Value<JArray>("delD");
                JArray decidList = jsonData.Value<JArray>("delDec");
                JObject arrData = jsonData.Value<JObject>("formdata");

                #region 刪除資料
                //delete detail
                for (int i = 0; i < didList.Count; i++)
                {
                    JValue temp = (JValue)didList[i];
                    objComm.DeleteSingleTableData("T_V_SHIPPING_D", "ID", temp.ToString());
                }

                //delete declarant
                for (int i = 0; i < decidList.Count; i++)
                {
                    JValue temp = (JValue)decidList[i];
                    objComm.DeleteSingleTableData("T_V_DECLARANT", "ID", temp.ToString());
                }

                //delete header&detail&declarant
                for (int i = 0; i < hidList.Count; i++)
                {
                    JValue temp = (JValue)hidList[i];
                    objComm.DeleteSingleTableData("T_V_SHIPPING_D", "SHIPPINGID_H", temp.ToString());
                    objComm.DeleteSingleTableData("T_V_DECLARANT", "SHIPPINGID_H", temp.ToString());
                    objComm.DeleteSingleTableData("T_V_SHIPPING_H", "ID", temp.ToString());                    
                }
                #endregion

                #region 資料處理
                //Master data
                Hashtable mData = new Hashtable();
                mData["ID"] = arrData.Value<string>("id");
                mData["MAWBNO"] = arrData.Value<string>("mawbno");
                mData["FLIGHTNUM"] = arrData.Value<string>("flightnum");
                mData["TOTAL"] = arrData.Value<string>("total");
                mData["TOTALWEIGHT"] = arrData.Value<string>("totalweight");
                mData["SHIPPERCOMPANY"] = arrData.Value<string>("shippercompany");
                mData["SHIPPER"] = arrData.Value<string>("shipper");
                mData["RECEIVERCOMPANY"] = arrData.Value<string>("receivercompany");
                mData["RECEIVER"] = arrData.Value<string>("receiver");
                mData["RECEIVERZIPCODE"] = arrData.Value<string>("receiverzipcode");
                mData["RECEIVERADDR"] = arrData.Value<string>("receiveraddr");
                mData["RECEIVERPHONE"] = arrData.Value<string>("receiverphone");
                mData["RECEIVERTAXID"] = arrData.Value<string>("receivertaxid");                
                mData["ISMULTRECEIVER"] = arrData.Value<string>("ismultreceiver").ToUpper();
                mData["STATUS"] = arrData.Value<string>("status");
                //mData["STORECODE"] = arrData.Value<string>("storecode");
                mData["IMBROKERID"] = arrData.Value<string>("imbrokerid");
                mData["EXBROKERID"] = arrData.Value<string>("exbrokerid");
                mData["_usercode"] = Request.Cookies["_usercode"];
                mData["_username"] = Request.Cookies["_username"];

                if (mData["ISMULTRECEIVER"]?.ToString() == "Y")
                {
                    mData["RECEIVERCOMPANY"] = "";
                    mData["RECEIVER"] = "";
                    mData["RECEIVERZIPCODE"] = "";
                    mData["RECEIVERADDR"] = "";
                    mData["RECEIVERPHONE"] = "";
                    mData["RECEIVERTAXID"] = "";
                }

                //Header(box) data                
                JArray boxData = arrData.Value<JArray>("boxform");
                ArrayList AL = new ArrayList();
                for (int i = 0; i < boxData.Count; i++)
                {
                    JObject temp = (JObject)boxData[i];
                    Hashtable hData = new Hashtable();
                    foreach (var t in temp)
                    {
                        //Detail(product) data
                        if (t.Key == "productform")
                        {
                            JArray prdData = temp.Value<JArray>("productform");
                            ArrayList subAL = new ArrayList();
                            for (int j = 0; j < prdData.Count; j++)
                            {
                                JObject temp2 = (JObject)prdData[j];
                                Hashtable dData = new Hashtable();
                                foreach (var t2 in temp2)
                                {
                                    dData[(t2.Key).ToUpper()] = t2.Value?.ToString();
                                }
                                subAL.Add(dData);
                            }
                            hData["PRDFORM"] = subAL;

                        }
                        else if (t.Key == "decform")
                        {
                            JArray decData = temp.Value<JArray>("decform");
                            ArrayList decAL = new ArrayList();
                            for (int j = 0; j < decData.Count; j++)
                            {
                                JObject temp2 = (JObject)decData[j];
                                Hashtable dData = new Hashtable();
                                foreach (var t2 in temp2)
                                {
                                    dData[(t2.Key).ToUpper()] = t2.Value?.ToString();
                                }
                                decAL.Add(dData);
                            }
                            hData["DECFORM"] = decAL;
                        }
                        else
                            hData[(t.Key).ToUpper()] = t.Value?.ToString();

                    }
                    AL.Add(hData);
                }

                ////Declarant data
                //JArray decData = arrData.Value<JArray>("decform");
                //ArrayList decAL = new ArrayList();
                //for (int k = 0; k < decData.Count; k++)
                //{
                //    JObject temp = (JObject)decData[k];
                //    Hashtable deData = new Hashtable();
                //    foreach (var t in temp)
                //        deData[(t.Key).ToUpper()] = t.Value?.ToString();

                //    decAL.Add(deData);
                //}
                #endregion

                #region 新增or更新資料

                //insert or update header&detail&declarant               
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        sData["SHIPPINGIDM"] = mData["ID"];
                        sData["ISMULTRECEIVER"] = mData["ISMULTRECEIVER"];
                        long HID = 0;
                        if (sData["ID"]?.ToString() == "0")
                            HID = InsertCusShippingH(sData);
                        else
                        {
                            HID = Convert.ToInt64(sData["ID"]);
                            updateCusShippingH(sData);
                        }

                        ArrayList subAL = (ArrayList)sData["PRDFORM"];
                        if (subAL.Count > 0)
                        {
                            for (int j = 0; j < subAL.Count; j++)
                            {
                                Hashtable tempData = (Hashtable)subAL[j];
                                tempData["SHIPPINGIDM"] = mData["ID"];
                                tempData["SHIPPINGIDH"] = HID;
                                if (tempData["ID"]?.ToString() == "0")
                                    InsertCusShippingD(tempData);
                                else
                                    updateCusShippingD(tempData);
                            }
                        }

                        ArrayList decAL = (ArrayList)sData["DECFORM"];
                        if (decAL.Count > 0)
                        {
                            for (int k = 0; k < decAL.Count; k++)
                            {
                                Hashtable tempData = (Hashtable)decAL[k];
                                tempData["SHIPPINGIDM"] = mData["ID"];
                                tempData["SHIPPINGIDH"] = HID;
                                if (tempData["ID"]?.ToString() == "0")
                                    InsertTVDeclarant(tempData);
                                else
                                    updateTVDeclarant(tempData);
                            }
                        }

                    }
                }

                ////insert or update declarant
                //if (decAL.Count > 0)
                //{
                //    for (int k = 0; k < decAL.Count; k++)
                //    {
                //        Hashtable tempData = (Hashtable)decAL[k];
                //        tempData["SHIPPINGIDM"] = mData["ID"];
                //        if (tempData["ID"]?.ToString() == "0")
                //            InsertTVDeclarant(tempData);
                //        else
                //            updateTVDeclarant(tempData);
                //    }
                //}

                //update master
                updateCusShippingM(mData);
                #endregion


                //寫入異動拋轉紀錄(未入庫狀態)
                string tempShipping = DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {mData["ID"]?.ToString()} AND STATUS = 0");
                if (!string.IsNullOrEmpty(tempShipping))
                    objComm.InsertDepotRecord(2, tempShipping);

                
                #region 寄信通知進出口報關行(未入庫狀態才寄)
                string F_User = "TEC Website System<ebs.sys@t3ex-group.com>";
                string subject = $"TEC代運平台 - 報關資料通知";
                string domainUrl = _configuration.GetSection("WebsitSetting")["domain"];
                string C_User = _configuration.GetSection("WebsitSetting")["csMail"];
                string body = "";                
                body += $"<p>集運單號：{tempShipping}</p>";
                body += $"<p>報關資料已上傳，請至<a href='{domainUrl}cuslogin' target='_blank'>台灣空運後台系統</a>下載！</p>";
                body += "<p style='color:#ff0000'>[此為系統自動寄送信件，請勿直接回覆，謝謝！]</p>";

                string IMACID = DBUtil.GetSingleValue1($@"SELECT IMBROKERID AS COL1 FROM T_V_SHIPPING_M WHERE ID = {mData["ID"]?.ToString()} AND STATUS = 0");
                string EXACID = DBUtil.GetSingleValue1($@"SELECT EXBROKERID AS COL1 FROM T_V_SHIPPING_M WHERE ID = {mData["ID"]?.ToString()} AND STATUS = 0");

                if (!string.IsNullOrEmpty(IMACID) && IMACID != "0") {
                    string T_User = DBUtil.GetSingleValue1($@"SELECT EMAIL AS COL1 FROM T_S_ACCOUNT WHERE ID = {IMACID}");                    
                    objComm.SendMail(F_User, T_User, subject, body, C_User);
                }

                if (!string.IsNullOrEmpty(EXACID) && EXACID != "0")
                {
                    string T_User = DBUtil.GetSingleValue1($@"SELECT EMAIL AS COL1 FROM T_S_ACCOUNT WHERE ID = {EXACID}");
                    objComm.SendMail(F_User, T_User, subject, body, C_User);
                }
                #endregion


                //add user operation log
                Hashtable logData = new Hashtable();
                logData["SHIPPINGIDM"] = mData["ID"];
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                logMsg = "集運單細項內容";
                objComm.AddUserControlLog(logData, "/shippingcus/edit/" + logData["SHIPPINGIDM"], "廠商集運單管理-編輯", 2, logMsg);

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        //重匯集運單(匯入Excel)
        [HttpPost]
        [DisableRequestSizeLimit]
        public dynamic CoverCusShippingData()
        {
            try
            {
                Hashtable htData = new Hashtable();
                string type = Request.Form["type"];
                htData["ID"] = Convert.ToInt64(Request.Form["id"]);
                htData["_usercode"] = Request.Cookies["_usercode"];
                var file = Request.Form.Files;
                string ACID = DBUtil.GetSingleValue1($@"SELECT ACCOUNTID AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID", htData);
                string usercode = DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_ACCOUNT WHERE ID = {ACID}");
                var folderName = Path.Combine(@"tectransit\dist\tectransit\assets\import", usercode);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                if (file.Count > 0)
                {
                    //save file                    
                    if (type == "SHIPPINGFILE")
                        htData["SHIPPINGFILE2"] = file[0].FileName;
                    else if (type == "BROKERFILE")
                        htData["BROKERFILE2"] = file[0].FileName;
                    else
                        htData["MAWBFILE"] = file[0].FileName;

                    var fileName = Guid.NewGuid() + Path.GetExtension(file[0].FileName).ToLower();
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file[0].CopyTo(stream);
                    }

                    if (htData["SHIPPINGFILE2"] != null)
                        htData["SHIPPINGFILE2"] = fileName;
                    else if (htData["BROKERFILE2"] != null)
                        htData["BROKERFILE2"] = fileName;
                    else { htData["MAWBFILE"] = fileName; }


                    //更新消倉表
                    if (htData["SHIPPINGFILE2"] != null)
                    {

                        //鎖單檢查
                        bool IsLock = false;
                        string IMBRID = DBUtil.GetSingleValue1($@"SELECT IMBROKERID AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID", htData);
                        string EXBRID = DBUtil.GetSingleValue1($@"SELECT EXBROKERID AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID", htData);
                        if (IMBRID != "0" || EXBRID != "0")
                            IsLock = true;

                        if (IsLock)
                            return new { status = "99", msg = "匯入失敗，此單已選定報關人員，需取消報關人員資料才可重匯！" };

                        //import excel to database
                        string SHIPPINGNO = UpdateCusShippingData(usercode, htData);


                        if (!string.IsNullOrEmpty(SHIPPINGNO))
                        {
                            if (SHIPPINGNO == "error00")
                            {
                                return new { status = "99", msg = "匯入失敗，重複匯入已存在的袋號！" };
                            }
                            else if (SHIPPINGNO == "error99")
                            {
                                return new { status = "99", msg = "匯入失敗！" };
                            }
                            else
                            {
                                //寫入拋轉紀錄
                                objComm.InsertDepotRecord(2, SHIPPINGNO);
                                
                                return new { status = "0", msg = "匯入成功！" };
                            }
                        }
                        else
                        {
                            return new { status = "99", msg = "匯入失敗！" };
                        }
                    }


                    //更新材積與實重表
                    if (htData["BROKERFILE2"] != null)
                    {
                        htData["BROKERFILE2"] = $"res/assets/import/{usercode}/" + htData["BROKERFILE2"];
                        updateCusShippingM(htData);
                        
                        return new { status = "0", msg = "匯入成功！" };
                    }

                    //更新MAWB
                    if (htData["MAWBFILE"] != null)
                    {
                        htData["MAWBFILE"] = $"res/assets/import/{usercode}/" + htData["MAWBFILE"];
                        updateCusShippingM(htData);

                        return new { status = "0", msg = "匯入成功！" };
                    }

                    return new { status = "99", msg = "匯入失敗！" };

                }
                else
                    return new { status = "99", msg = "匯入失敗，無上傳任何檔案！" };


            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "匯入失敗！" };
            }
        }
        
        #endregion

        #region /* --- Twotable Map --- */

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

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditCompanyRankData([FromBody] object form)
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
                                objComm.AddUserControlLog(sData, $"company/{usercode}", "廠商管理 - 權限設定", 1, sData["RANKID"]?.ToString());
                            }
                        }
                        else
                        {
                            if (query != null)
                            {
                                delACRank(query);

                                //add user log
                                objComm.AddUserControlLog(sData, $"company/{usercode}", "廠商管理 - 權限設定", 3, sData["RANKID"]?.ToString());
                            }

                        }
                    }

                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic GetTransferData(long id)
        {
            try
            {
                Hashtable htData = new Hashtable();
                htData["TRANSFERIDM"] = id.ToString();
                htData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ACCOUNTID AS COL1 FROM T_E_TRANSFER_M WHERE ID = {htData["TRANSFERIDM"]}");

                return objUsm.GetTransferData(htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }           
        }

        [HttpPost]
        public dynamic EditTransferData([FromBody] object form)
        {
            try
            {
                string logMsg = "";
                var jsonData = JObject.FromObject(form);
                JArray hidList = jsonData.Value<JArray>("delH");
                JArray didList = jsonData.Value<JArray>("delD");
                JArray decidList = jsonData.Value<JArray>("delDec");
                JObject arrData = jsonData.Value<JObject>("formdata");

                #region 刪除資料
                //delete detail
                for (int i = 0; i < didList.Count; i++)
                {
                    JValue temp = (JValue)didList[i];
                    objComm.DeleteSingleTableData("T_E_TRANSFER_D", "ID", temp.ToString());
                }

                //delete header&detail
                for (int i = 0; i < hidList.Count; i++)
                {
                    JValue temp = (JValue)hidList[i];
                    objComm.DeleteSingleTableData("T_E_TRANSFER_D", "TRANSFERID_H", temp.ToString());
                    objComm.DeleteSingleTableData("T_E_TRANSFER_H", "ID", temp.ToString());
                }

                //delete declarant
                for (int i = 0; i < decidList.Count; i++)
                {
                    JValue temp = (JValue)decidList[i];
                    objComm.DeleteSingleTableData("T_E_DECLARANT", "ID", temp.ToString());
                }
                #endregion

                #region 資料處理
                //Master data
                Hashtable mData = new Hashtable();
                mData["ID"] = arrData.Value<string>("id");
                mData["TOTAL"] = arrData.Value<string>("total");
                mData["RECEIVER"] = arrData.Value<string>("receiver");
                mData["RECEIVERADDR"] = arrData.Value<string>("receiveraddr");
                mData["RECEIVERPHONE"] = arrData.Value<string>("receiverphone");
                mData["PLENGTH"] = arrData.Value<string>("plength");
                mData["PWIDTH"] = arrData.Value<string>("pwidth");
                mData["PHEIGHT"] = arrData.Value<string>("pheight");
                mData["PWEIGHT"] = arrData.Value<string>("pweight");
                mData["PVALUEPRICE"] = arrData.Value<string>("pvalueprice");
                mData["ISMULTRECEIVER"] = arrData.Value<string>("ismultreceiver").ToUpper();
                mData["STATUS"] = arrData.Value<string>("status");
                mData["_usercode"] = Request.Cookies["_usercode"];
                mData["_username"] = Request.Cookies["_username"];

                if (mData["ISMULTRECEIVER"]?.ToString() == "Y")
                {
                    mData["RECEIVER"] = "";
                    mData["RECEIVERADDR"] = "";
                    mData["RECEIVERPHONE"] = "";
                }

                //Header(box) data                
                JArray boxData = arrData.Value<JArray>("boxform");
                ArrayList AL = new ArrayList();
                for (int i = 0; i < boxData.Count; i++)
                {
                    JObject temp = (JObject)boxData[i];
                    Hashtable hData = new Hashtable();
                    foreach (var t in temp)
                    {
                        //Detail(product) data
                        if (t.Key == "productform")
                        {
                            JArray prdData = temp.Value<JArray>("productform");
                            ArrayList subAL = new ArrayList();
                            for (int j = 0; j < prdData.Count; j++)
                            {
                                JObject temp2 = (JObject)prdData[j];
                                Hashtable dData = new Hashtable();
                                foreach (var t2 in temp2)
                                {
                                    dData[(t2.Key).ToUpper()] = t2.Value?.ToString();
                                }
                                subAL.Add(dData);
                            }
                            hData["PRDFORM"] = subAL;

                        }
                        else
                            hData[(t.Key).ToUpper()] = t.Value?.ToString();

                    }
                    AL.Add(hData);
                }

                //Declarant data
                JArray decData = arrData.Value<JArray>("decform");
                ArrayList decAL = new ArrayList();
                for (int k = 0; k < decData.Count; k++)
                {
                    JObject temp = (JObject)decData[k];
                    Hashtable deData = new Hashtable();
                    foreach (var t in temp)
                        deData[(t.Key).ToUpper()] = t.Value?.ToString();

                    decAL.Add(deData);
                }
                #endregion

                #region 新增or更新資料

                //insert or update header&detail               
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        sData["TRANSFERIDM"] = mData["ID"];
                        sData["ISMULTRECEIVER"] = mData["ISMULTRECEIVER"];
                        long HID = 0;
                        if (sData["ID"]?.ToString() == "0")
                            HID = InsertTransferH(sData);
                        else
                        {
                            HID = Convert.ToInt64(sData["ID"]);
                            updateTransferH(sData);
                        }

                        ArrayList subAL = (ArrayList)sData["PRDFORM"];
                        if (subAL.Count > 0)
                        {
                            for (int j = 0; j < subAL.Count; j++)
                            {
                                Hashtable tempData = (Hashtable)subAL[j];
                                tempData["TRANSFERIDM"] = mData["TRANSFERIDM"];
                                tempData["TRANSFERIDH"] = HID;
                                if (tempData["ID"]?.ToString() == "0")
                                    InsertTransferD(tempData);
                                else
                                    updateTransferD(tempData);
                            }
                        }

                    }
                }

                //insert or update declarant
                if (decAL.Count > 0)
                {
                    for (int k = 0; k < decAL.Count; k++)
                    {
                        Hashtable tempData = (Hashtable)decAL[k];
                        tempData["TRANSFERIDM"] = mData["ID"];
                        if (tempData["ID"]?.ToString() == "0")
                            InsertMemDeclarant("T_E_DECLARANT",tempData);
                        else
                            UpdateMemDeclarant("T_E_DECLARANT", tempData);
                    }
                }

                //update master
                UpdateTransferM(mData);
                #endregion
                

                //add user operation log
                Hashtable logData = new Hashtable();
                logData["TRANSFERIDM"] = mData["ID"];
                logData["_usercode"] = Request.Cookies["_usercode"];
                logData["_username"] = Request.Cookies["_username"];
                logMsg = "快遞單細項內容";
                objComm.AddUserControlLog(logData, "/transfer/edit/" + logData["TRANSFERIDM"], "快遞單管理-編輯", 2, logMsg);

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }
        #endregion

        #region /* --- private CRUD function --- */
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
                    bool IsCompany = DBUtil.GetSingleValue1($@"SELECT RANKTYPE AS COL1 FROM T_S_RANK WHERE RANKCODE = '{htData["RANKCODE"]}'") == "2" ? true : false;                    
                    objComm.AddUserControlLog(htData, (IsCompany ? "companyrank/edit/0" : "rank/edit/0"), (IsCompany ? "廠商權限管理" : "用戶權限管理"), 1, htData["RANKCODE"]?.ToString());
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
                    bool IsCompany = DBUtil.GetSingleValue1($@"SELECT RANKTYPE AS COL1 FROM T_S_RANK WHERE RANKCODE = '{htData["RANKCODE"]}'") == "2" ? true : false;
                    objComm.AddUserControlLog(htData, (IsCompany ? $"companyrank/edit/{id}" : $"rank/edit/{id}"), (IsCompany ? "廠商權限管理" : "用戶權限管理"), 2, updMsg);
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
                    //目前後台只開放新增廠商會員
                    InsertAccount(htData);

                    htData["RANKID"] = htData["RANKTYPE"];
                    insertACRank(htData);
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
                    string sql = $@"SELECT C.RANKTYPE AS COL1 FROM T_S_ACCOUNT A
                                   LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE
                                   LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                   WHERE A.ID = {id}";

                    bool IsCompany = DBUtil.GetSingleValue1(sql) == "2" ? true : false;
                    objComm.AddUserControlLog(htData, (IsCompany ? $"company/edit/{id}" : $"accounts/edit/{id}"), (IsCompany ? "廠商管理" : "前台用戶管理"), 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertAccount(Hashtable sData)
        {
            Hashtable htData = sData;
            int newSeq = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERSEQ AS COL1 FROM T_S_ACCOUNT ORDER BY USERSEQ DESC")) ? 1 : Convert.ToInt32(DBUtil.GetSingleValue1($@"SELECT USERSEQ AS COL1 FROM T_S_ACCOUNT ORDER BY USERSEQ DESC")) + 1;
            htData["USERPASSWORD"] = objComm.GetMd5Hash(htData["USERPASSWORD"]?.ToString());
            htData["ADDR"] = htData["ADDRESS"];
            htData["USERSEQ"] = newSeq;
            htData["LOGINCOUNT"] = 0;
            htData["LASTLOGINDATE"] = DateTime.Now;
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_S_ACCOUNT(USERCODE, USERPASSWORD, USERSEQ, USERNAME, USERDESC,
                                                   COMPANYNAME, RATEID, WAREHOUSENO, EMAIL, TAXID,
                                                   IDPHOTO_F, IDPHOTO_B, PHONE, MOBILE, ADDR,
                                                   ISENABLE, LOGINCOUNT, LASTLOGINDATE, CREDATE, CREATEBY,
                                                   UPDDATE, UPDBY) 
                                        VALUES (@USERCODE, @USERPASSWORD, @USERSEQ, @USERNAME, @USERDESC,
                                                   @COMPANYNAME, @RATEID, @WAREHOUSENO, @EMAIL, @TAXID,
                                                   @IDPHOTO_F, @IDPHOTO_B, @PHONE, @MOBILE, @ADDR,
                                                   @ISENABLE, @LOGINCOUNT, @LASTLOGINDATE, @CREDATE, @CREATEBY,
                                                   @UPDDATE, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
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
                    if (!string.IsNullOrEmpty(sData["USERPASSWORD"]?.ToString()))
                        rowTSA.Userpassword = objComm.GetMd5Hash(sData["USERPASSWORD"]?.ToString());
                if (sData["USERSEQ"] != null)
                    rowTSA.Userseq = sData["USERSEQ"]?.ToString();
                if (sData["USERNAME"] != null)
                    rowTSA.Username = sData["USERNAME"]?.ToString();
                if (sData["USERDESC"] != null)
                    rowTSA.Userdesc = sData["USERDESC"]?.ToString();
                if (sData["COMPANYNAME"] != null)
                    rowTSA.Companyname = sData["COMPANYNAME"]?.ToString();
                if (sData["RATEID"] != null)
                    rowTSA.Rateid = sData["RATEID"]?.ToString();
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
                if (sData["ADDRESS"] != null)
                    rowTSA.Addr = sData["ADDRESS"]?.ToString();
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
        

        private void UpdateTransferM(Hashtable sData)
        {
            var query = _context.TETransferM.Where(q => q.Id == Convert.ToInt64(sData["ID"])).FirstOrDefault();

            if (query != null)
            {
                TETransferM rowTEM = query;

                if (sData["TRASFERCOMPANY"] != null)
                    rowTEM.Transfercompany = sData["TRASFERCOMPANY"]?.ToString();
                if (sData["PLENGTH"] != null)
                    rowTEM.PLength = sData["PLENGTH"]?.ToString();
                if (sData["PWIDTH"] != null)
                    rowTEM.PWidth = sData["PWIDTH"]?.ToString();
                if (sData["PHEIGHT"] != null)
                    rowTEM.PHeight = sData["PHEIGHT"]?.ToString();
                if (sData["PWEIGHT"] != null)
                    rowTEM.PWeight = sData["PWEIGHT"]?.ToString();
                if (sData["PVALUEPRICE"] != null)
                    rowTEM.PValueprice = sData["PVALUEPRICE"]?.ToString();
                if (sData["TOTAL"] != null)
                    rowTEM.Total = sData["TOTAL"]?.ToString();
                if (sData["RECEIVER"] != null)
                    rowTEM.Receiver = sData["RECEIVER"]?.ToString();
                if (sData["RECEIVERPHONE"] != null)
                    rowTEM.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                if (sData["RECEIVERADDR"] != null)
                    rowTEM.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                if (sData["ISMULTRECEIVER"] != null)
                    rowTEM.Ismultreceiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;
                if (sData["STATUS"] != null)
                    rowTEM.Status = Convert.ToInt32(sData["STATUS"]);
                if (sData["REMARK"] != null)
                    rowTEM.Remark = sData["REMARK"]?.ToString();

                if (sData.Count > 2)//排除cookies
                {
                    rowTEM.Upddate = DateTime.Now;
                    rowTEM.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private long InsertTransferH(Hashtable sData)
        {
            long ID = 0;
            try
            {
                TETransferH TEH = new TETransferH();
                TEH.Boxno = sData["BOXNO"]?.ToString();
                TEH.Receiver = sData["RECEIVER"]?.ToString();
                TEH.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                TEH.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                TEH.TransferidM = Convert.ToInt64(sData["TRANSFERIDM"]);

                _context.TETransferH.Add(TEH);
                _context.SaveChanges();

                ID = TEH.Id;

                return ID;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return ID;
            }
        }

        private void updateTransferH(Hashtable sData)
        {
            var query = _context.TETransferH.Where(q => q.Id == Convert.ToInt64(sData["ID"])).FirstOrDefault();
            if (query != null)
            {
                query.Boxno = sData["BOXNO"]?.ToString();
                query.Receiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVER"]?.ToString() : "";
                query.Receiveraddr = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVERADDR"]?.ToString() : "";
                query.Receiverphone = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVERPHONE"]?.ToString() : "";

                _context.SaveChanges();
            }
        }

        private void InsertTransferD(Hashtable sData)
        {
            try
            {
                TETransferD TED = new TETransferD();
                TED.Product = sData["PRODUCT"]?.ToString();
                TED.Quantity = sData["QUANTITY"]?.ToString();
                TED.Unitprice = sData["UNITPRICE"]?.ToString();
                TED.TransferidM = Convert.ToInt64(sData["TRANSFERIDM"]);
                TED.TransferidH = Convert.ToInt64(sData["TRANSFERIDH"]);

                _context.TETransferD.Add(TED);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        private void updateTransferD(Hashtable sData)
        {
            var query = _context.TETransferD.Where(q => q.Id == Convert.ToInt64(sData["ID"])).FirstOrDefault();
            if (query != null)
            {
                query.Product = sData["PRODUCT"]?.ToString();
                query.Quantity = sData["QUANTITY"]?.ToString();
                query.Unitprice = sData["UNITPRICE"]?.ToString();

                _context.SaveChanges();
            }
        }

        private void InsertMemDeclarant(string sTable, Hashtable sData)
        {
            try
            {
                string MID_C = (sTable == "T_E_DECLARANT" ? "TRANSFERID_M" : "SHIPPINGID_M");
                string MID_V = (sTable == "T_E_DECLARANT" ? "@TRANSFERIDM" : "@SHIPPINGIDM");
                string sql = $@"INSERT INTO {sTable} (NAME, TAXID, PHONE, MOBILE, ADDR, IDPHOTO_F, IDPHOTO_B, APPOINTMENT, {MID_C}) 
                                VALUES (@NAME, @TAXID, @PHONE, @MOBILE, @ADDR, @IDPHOTOF, @IDPHOTOB, @APPOINTMENT, {MID_V})";

                DBUtil.EXECUTE(sql, sData);

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        private void UpdateMemDeclarant(string sTable, Hashtable sData)
        {
            try
            {
                string sql = $@"UPDATE {sTable} SET 
                                                NAME = @NAME,
                                                TAXID = @TAXID, 
                                                PHONE = @PHONE, 
                                                MOBILE = @MOBILE, 
                                                ADDR = @ADDR, 
                                                IDPHOTO_F = @IDPHOTOF, 
                                                IDPHOTO_B = @IDPHOTOB,
                                                APPOINTMENT = @APPOINTMENT 
                            WHERE ID = {sData["ID"]}";

                DBUtil.EXECUTE(sql, sData);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }

        }

        //快遞單管理
        //刪除主單(Master)/箱號(Header)/細項(Detail)/申報人資料(該快遞單下所有)
        private void DeleteTEData_All(string table, Hashtable sData)
        {
            string sql = "";
            if (table == "T_E_TRANSFER_M")
                sql = $@"DELETE FROM {table} WHERE ID = @TRANSFERIDM";
            else
                sql = $@"DELETE FROM {table} WHERE TRANSFERID_M = @TRANSFERIDM";

            DBUtil.EXECUTE(sql, sData);
        }

        private void updateCusShippingM(Hashtable sData)
        {
            var query = _context.TVShippingM.Where(q => q.Id == Convert.ToInt64(sData["ID"])).FirstOrDefault();
            if (query != null)
            {
                if (sData["MAWBNO"] != null)
                    query.Mawbno = sData["MAWBNO"]?.ToString();
                if (sData["FLIGHTNUM"] != null)
                    query.Flightnum = sData["FLIGHTNUM"]?.ToString();
                if (sData["TOTAL"] != null)
                    query.Total = sData["TOTAL"]?.ToString();
                if (sData["TOTALWEIGHT"] != null)
                    query.Totalweight = sData["TOTALWEIGHT"]?.ToString();
                if (sData["SHIPPERCOMPANY"] != null)
                    query.Shippercompany = sData["SHIPPERCOMPANY"]?.ToString();
                if (sData["SHIPPER"] != null)
                    query.Shipper = sData["SHIPPER"]?.ToString();
                if (sData["RECEIVERCOMPANY"] != null)
                    query.Receivercompany = sData["RECEIVERCOMPANY"]?.ToString();
                if (sData["RECEIVER"] != null)
                    query.Receiver = sData["RECEIVER"]?.ToString();
                if (sData["RECEIVERZIPCODE"] != null)
                    query.Receiverzipcode = sData["RECEIVERZIPCODE"]?.ToString();
                if (sData["RECEIVERADDR"] != null)
                    query.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                if (sData["RECEIVERPHONE"] != null)
                    query.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                if (sData["RECEIVERTAXID"] != null)
                    query.Taxid = sData["RECEIVERTAXID"]?.ToString();
                if (sData["ISMULTRECEIVER"] != null)
                    query.Ismultreceiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;
                if (sData["STATUS"] != null)
                    query.Status = Convert.ToInt32(sData["STATUS"]);
                //if (sData["STORECODE"] != null)
                //    query.Storecode = sData["STORECODE"]?.ToString();
                if (sData["IMBROKERID"] != null)
                    query.Imbrokerid = Convert.ToInt64(sData["IMBROKERID"]);
                if (sData["EXBROKERID"] != null)
                    query.Exbrokerid = Convert.ToInt64(sData["EXBROKERID"]);
                if (sData["MAWBFILE"] != null)
                    query.Mawbfile = sData["MAWBFILE"]?.ToString();
                if (sData["SHIPPINGFILE2"] != null)
                    query.Shippingfile2 = sData["SHIPPINGFILE2"]?.ToString();
                if (sData["BROKERFILE2"] != null)
                    query.Brokerfile2 = sData["BROKERFILE2"]?.ToString();

                query.Upddate = DateTime.Now;
                query.Updby = sData["_usercode"]?.ToString();

                _context.SaveChanges();
            }
        }
        

        private long InsertCusShippingH(Hashtable sData)
        {
            long ID = 0;
            try
            {
                TVShippingH TVH = new TVShippingH();
                TVH.Clearanceno = sData["CLEARANCENO"]?.ToString();
                TVH.Transferno = sData["TRANSFERNO"]?.ToString();
                TVH.Weight = sData["WEIGHT"]?.ToString();
                TVH.Totalitem = sData["TOTALITEM"]?.ToString();
                TVH.Shippercompany = sData["SHIPPERCOMPANY"]?.ToString();
                TVH.Shipper = sData["SHIPPER"]?.ToString();
                TVH.Receivercompany = sData["RECEIVERCOMPANY"]?.ToString();
                TVH.Receivercompany = sData["RECEIVERCOMPANY"]?.ToString();
                TVH.Receiver = sData["RECEIVER"]?.ToString();
                TVH.Receiverzipcode = sData["RECEIVERZIPCODE"]?.ToString();
                TVH.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                TVH.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                TVH.Taxid = sData["RECEIVERTAXID"]?.ToString();
                TVH.Logistics = sData["LOGISTICS"]?.ToString();
                TVH.Shipperremark = sData["SHIPPERREMARK"]?.ToString();
                TVH.ShippingidM = Convert.ToInt64(sData["SHIPPINGIDM"]);

                _context.TVShippingH.Add(TVH);
                _context.SaveChanges();

                ID = TVH.Id;

                return ID;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return ID;
            }
        }

        private void updateCusShippingH(Hashtable sData)
        {
            var query = _context.TVShippingH.Where(q => q.Id == Convert.ToInt64(sData["ID"])).FirstOrDefault();
            if (query != null)
            {

                query.Clearanceno = sData["CLEARANCENO"]?.ToString();
                query.Transferno = sData["TRANSFERNO"]?.ToString();
                query.Weight = sData["WEIGHT"]?.ToString();
                query.Totalitem = sData["TOTALITEM"]?.ToString();
                query.Shippercompany = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["SHIPPERCOMPANY"]?.ToString() : "";
                query.Shipper = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["SHIPPER"]?.ToString() : "";
                query.Receivercompany = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVERCOMPANY"]?.ToString() : "";
                query.Receiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVER"]?.ToString() : "";
                query.Receiverzipcode = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVERZIPCODE"]?.ToString() : "";
                query.Receiveraddr = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVERADDR"]?.ToString() : "";
                query.Receiverphone = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVERPHONE"]?.ToString() : "";
                query.Taxid= sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVERTAXID"]?.ToString() : "";
                query.Logistics= sData["LOGISTICS"]?.ToString();
                query.Shipperremark= sData["SHIPPERREMARK"]?.ToString();

                _context.SaveChanges();
            }
        }

        private void InsertCusShippingD(Hashtable sData)
        {
            try
            {
                TVShippingD TVD = new TVShippingD();
                TVD.Product = sData["PRODUCT"]?.ToString();
                TVD.Quantity = sData["QUANTITY"]?.ToString();
                TVD.Unitprice = sData["UNITPRICE"]?.ToString();
                TVD.ShippingidM = Convert.ToInt64(sData["SHIPPINGIDM"]);
                TVD.ShippingidH = Convert.ToInt64(sData["SHIPPINGIDH"]);

                _context.TVShippingD.Add(TVD);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        private void updateCusShippingD(Hashtable sData)
        {
            var query = _context.TVShippingD.Where(q => q.Id == Convert.ToInt64(sData["ID"])).FirstOrDefault();
            if (query != null)
            {
                query.Product = sData["PRODUCT"]?.ToString();
                query.Quantity = sData["QUANTITY"]?.ToString();
                query.Unitprice = sData["UNITPRICE"]?.ToString();

                _context.SaveChanges();
            }
        }

        private void InsertTVDeclarant(Hashtable sData)
        {
            try
            {
                TVDeclarant TVD = new TVDeclarant();
                TVD.Name = sData["NAME"]?.ToString();
                TVD.Taxid = sData["TAXID"]?.ToString();
                TVD.Phone = sData["PHONE"]?.ToString();
                TVD.Mobile = sData["MOBILE"]?.ToString();
                TVD.Zipcode = sData["ZIPCODE"]?.ToString();
                TVD.Addr = sData["ADDR"]?.ToString();
                TVD.IdphotoF = sData["IDPHOTOF"]?.ToString();
                TVD.IdphotoB = sData["IDPHOTOB"]?.ToString();
                TVD.Appointment = sData["APPOINTMENT"]?.ToString();
                TVD.ShippingidM = Convert.ToInt64(sData["SHIPPINGIDM"]);
                TVD.ShippingidH = Convert.ToInt64(sData["SHIPPINGIDH"]);

                _context.TVDeclarant.Add(TVD);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        private void updateTVDeclarant(Hashtable sData)
        {
            var query = _context.TVDeclarant.Where(q => q.Id == Convert.ToInt64(sData["ID"])).FirstOrDefault();
            if (query != null)
            {
                query.Name = sData["NAME"]?.ToString();
                query.Taxid = sData["TAXID"]?.ToString();
                query.Phone = sData["PHONE"]?.ToString();
                query.Mobile = sData["MOBILE"]?.ToString();
                query.Zipcode = sData["ZIPCODE"]?.ToString();
                query.Addr = sData["ADDR"]?.ToString();

                _context.SaveChanges();
            }
        }

        private void UpdateShippingMStatus(Hashtable sData)
        {
            var query = _context.TVShippingM.Where(q => q.Id == Convert.ToInt64(sData["SHIPPINGIDM"])).FirstOrDefault();
            if (query != null)
            {
                query.Status = Convert.ToInt32(sData["STATUS"]);
                _context.SaveChanges();
            }
        }

        //刪除主單(Master)/箱號(Header)/細項(Detail)/申報人資料(該集運單下所有)
        private void DeleteTVData_All(string table, Hashtable sData)
        {
            string sql = "";
            if (table == "T_V_SHIPPING_M")
                sql = $@"DELETE FROM {table} WHERE ID = @SHIPPINGIDM";
            else
                sql = $@"DELETE FROM {table} WHERE SHIPPINGID_M = @SHIPPINGIDM";

            DBUtil.EXECUTE(sql, sData);
        }

        //覆蓋集運單(Excel to DB)
        public string UpdateCusShippingData(string usercode, Hashtable data)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var folderName = Path.Combine(@"tectransit\dist\tectransit\assets\import", usercode);
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(pathToSave, data["SHIPPINGFILE2"]?.ToString());
                    FileInfo newFile = new FileInfo(fullPath);

                    using (ExcelPackage ep = new ExcelPackage(newFile))
                    {
                        long MID = 0;
                        long HID = 0;
                        decimal TotalWGM = 0; //總重(主單)
                        decimal Total = 0; //總件數(主單) 
                        decimal TotalWG = 0; //各提單總重(商品)
                        decimal Totalitem = 0; //各提單總件數(商品) 
                                               //List<string> Box = new List<string>();
                        List<string> Rec = new List<string>();

                        MID = Convert.ToInt64(data["ID"]);

                        ExcelWorksheet ws = ep.Workbook.Worksheets[1];
                        var rowCt = ws.Dimension.End.Row;
                        Hashtable htData = new Hashtable();
                        htData["SHIPPINGFILE2"] = $"res/assets/import/{usercode}/{data["SHIPPINGFILE2"]}";//消艙表url                        

                        htData["NEWNUM"] = "";//編號(比對用)
                        htData["NEWNO"] = "";//袋號(比對用)
                        htData["NEWTOTALWG"] = "";//袋重(比對用)
                        htData["NEWBOXNO"] = "";//提單號碼(比對用)
                        htData["_cuscode"] = usercode;//用戶帳號
                        string ACID = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{usercode}' AND ISENABLE = 'true'");
                        htData["ACCOUNTID"] = ACID;//用戶ID
                        htData["MAWBDATE"] = ws.Cells[1, 6].Value?.ToString().Trim();//消倉單日期
                        htData["MAWBNO"] = string.IsNullOrEmpty(ws.Cells[1, 10].Value?.ToString()) ? "" : (ws.Cells[1, 10].Value?.ToString().Trim()).Replace(" ", "").Replace("　", "").Replace("\r", ""); ;//MAWB
                        htData["FLIGHTNUM"] = string.IsNullOrEmpty(ws.Cells[1, 15].Value?.ToString()) ? "" : ws.Cells[1, 15].Value?.ToString().Trim();//航班號

                        //刪除集運單底下所有資料(保留主單)
                        if (MID != 0)
                        {
                            Hashtable sData = new Hashtable();
                            sData["SHIPPINGIDM"] = MID;
                            DeleteTVData_All("T_V_DECLARANT", sData);
                            DeleteTVData_All("T_V_SHIPPING_D", sData);
                            DeleteTVData_All("T_V_SHIPPING_H", sData);
                        }


                        for (int i = 3; i <= rowCt; i++)
                        {
                            #region Excel資料欄位
                            htData["NEWNUM"] = ws.Cells[i, 1].Value?.ToString().Trim();
                            htData["NEWNO"] = ws.Cells[i, 2].Value?.ToString().Trim();
                            htData["NEWTOTALWG"] = ws.Cells[i, 3].Value?.ToString().Trim();
                            htData["NEWBOXNO"] = ws.Cells[i, 4].Value?.ToString().Trim();

                            if (!string.IsNullOrEmpty(ws.Cells[i, 2].Value?.ToString().Trim()))
                                htData["CLEARANCENO"] = ws.Cells[i, 2].Value?.ToString().Trim();//袋號=清關條碼

                            if (!string.IsNullOrEmpty(ws.Cells[i, 3].Value?.ToString().Trim()))
                                htData["TOTALWEIGHT"] = ws.Cells[i, 3].Value?.ToString().Trim();//總重量

                            htData["TRANSFERNO"] = ws.Cells[i, 4].Value?.ToString().Trim();//提單號碼                        
                            htData["TOTAL"] = ws.Cells[i, 5].Value?.ToString().Trim();//件數                    
                            htData["WEIGHT"] = ws.Cells[i, 6].Value?.ToString().Trim();//提單重量
                            htData["PRODUCT"] = ws.Cells[i, 7].Value?.ToString().Trim();//品名
                            htData["QUANTITY"] = ws.Cells[i, 8].Value?.ToString().Trim();//數量


                            //檢查袋號是否已匯入過(排除自己)
                            bool IsExist = false;
                            string chksql = $@"SELECT DISTINCT CLEARANCENO FROM T_V_SHIPPING_M A
                                           LEFT JOIN T_V_SHIPPING_H B ON A.ID = B.SHIPPINGID_M
                                           WHERE A.ACCOUNTID = {ACID} AND A.STATUS = 0 AND A.ID != {MID}";

                            DataTable DT = DBUtil.SelectDataTable(chksql);
                            if (DT.Rows.Count > 0)
                            {
                                for (int z = 0; z < DT.Rows.Count; z++)
                                {
                                    if (!string.IsNullOrEmpty(DT.Rows[z]["CLEARANCENO"]?.ToString()))
                                        if (DT.Rows[z]["CLEARANCENO"]?.ToString() == htData["CLEARANCENO"]?.ToString())
                                            IsExist = true;

                                }
                            }

                            if (IsExist)
                                return "error00";


                            //提單重&總件數
                            if (!string.IsNullOrEmpty(htData["PRODUCT"]?.ToString()))
                            {
                                if (string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()) && !string.IsNullOrEmpty(htData["WEIGHT"]?.ToString()))
                                    TotalWG += Convert.ToDecimal(htData["WEIGHT"]);

                                else
                                    TotalWG = Convert.ToDecimal(htData["WEIGHT"]);


                                if (string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()) && !string.IsNullOrEmpty(htData["QUANTITY"]?.ToString()))
                                {
                                    Totalitem += Convert.ToDecimal(htData["QUANTITY"]);
                                    htData["TOTALITEM"] = Totalitem;
                                }
                                else
                                {
                                    Totalitem = Convert.ToDecimal(htData["QUANTITY"]);
                                    htData["TOTALITEM"] = Totalitem;
                                }

                            }

                            htData["UNIT"] = ws.Cells[i, 9].Value?.ToString().Trim();//單位
                            htData["ORIGIN"] = ws.Cells[i, 10].Value?.ToString().Trim();//產地
                            htData["UNITPRICE"] = ws.Cells[i, 11].Value?.ToString().Trim();//單價

                            htData["SHIPPERCOMPANY"] = ws.Cells[i, 12].Value?.ToString().Trim();//寄件公司
                            htData["SHIPPER"] = ws.Cells[i, 13].Value?.ToString().Trim();//寄件人
                            htData["RECEIVERCOMPANY"] = ws.Cells[i, 14].Value?.ToString().Trim();//收件公司
                            htData["RECEIVER"] = ws.Cells[i, 15].Value?.ToString().Trim();//收件人                            
                            htData["RECEIVERPHONE"] = ws.Cells[i, 16].Value?.ToString().Trim();//收件人電話
                            htData["RECEIVERZIPCODE"] = ws.Cells[i, 17].Value?.ToString().Trim();//收件人郵遞區號
                            htData["RECEIVERADDR"] = ws.Cells[i, 18].Value?.ToString().Trim();//收件人地址
                            htData["TAXID"] = ws.Cells[i, 19].Value?.ToString().Trim();//統編or身分證字號
                            htData["SHIPPERREMARK"] = ws.Cells[i, 20].Value?.ToString().Trim();//備註(樂一番用)
                            htData["LOGISTICS"] = ws.Cells[i, 21].Value?.ToString().Trim();//出貨商                            
                            #endregion


                            //if product detail blank then break for loop
                            if (string.IsNullOrEmpty(htData["PRODUCT"]?.ToString()) && string.IsNullOrEmpty(htData["QUANTITY"]?.ToString()) && string.IsNullOrEmpty(htData["UNITPRICE"]?.ToString()))
                            {
                                break;
                            }

                            //new master data & new header data
                            if (!string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()))
                            {

                                htData["SHIPPINGIDM"] = MID;
                                HID = objUsm.InsertCusShippingH(htData);
                            }
                            else
                            {
                                htData["SHIPPINGIDH"] = HID;
                                htData["WEIGHT"] = TotalWG;
                                objUsm.UpdateCusShippingH(htData);
                            }

                            //New Detail
                            if (MID != 0 && HID != 0)
                            {
                                if (!string.IsNullOrEmpty(htData["PRODUCT"]?.ToString()) && !string.IsNullOrEmpty(htData["QUANTITY"]?.ToString()))
                                {
                                    htData["SHIPPINGIDM"] = MID;
                                    htData["SHIPPINGIDH"] = HID;
                                    objUsm.InsertCusShippingD(htData);
                                }
                            }

                            if (MID != 0)
                            {
                                //New Declarant
                                Hashtable sData = new Hashtable();
                                sData["SHIPPINGIDM"] = MID;
                                sData["SHIPPINGIDH"] = HID;
                                sData["NAME"] = htData["RECEIVER"]?.ToString();
                                sData["TAXID"] = htData["TAXID"]?.ToString();
                                sData["PHONE"] = htData["RECEIVERPHONE"]?.ToString();
                                sData["ZIPCODE"] = htData["RECEIVERZIPCODE"]?.ToString();
                                sData["ADDR"] = htData["RECEIVERADDR"]?.ToString();

                                if (!string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()))
                                    objUsm.InsertTVDeclarant(sData);

                                //check receiver
                                int chk = 0;
                                foreach (var r in Rec)
                                    if (r == htData["RECEIVER"]?.ToString())
                                        chk++;
                                if (chk == 0 && !string.IsNullOrEmpty(htData["RECEIVER"]?.ToString()))
                                    Rec.Add(htData["RECEIVER"]?.ToString());

                                decimal ReceiverCt = Rec.Count();

                                //Update Master Data
                                string WG = ws.Cells[i, 3].Value?.ToString().Trim();
                                string QTY = ws.Cells[i, 8].Value?.ToString().Trim();
                                if (!string.IsNullOrEmpty(WG))
                                    TotalWGM += Convert.ToDecimal(WG);
                                if (!string.IsNullOrEmpty(QTY))
                                    Total += Convert.ToDecimal(QTY);
                                Hashtable tempData = new Hashtable();
                                tempData["SHIPPINGFILE2"] = htData["SHIPPINGFILE2"]?.ToString();
                                tempData["STORECODE"] = "C2011"; //倉儲代碼
                                tempData["MAWBNO"] = htData["MAWBNO"];
                                tempData["FLIGHTNUM"] = htData["FLIGHTNUM"];
                                tempData["ID"] = MID;
                                tempData["TOTAL"] = Total;
                                tempData["TOTALWEIGHT"] = TotalWGM;
                                tempData["ISMULTRECEIVER"] = ReceiverCt > 1 ? "Y" : "N";
                                // 單一收件人
                                if (ReceiverCt == 1)
                                {
                                    tempData["RECEIVERCOMPANY"] = htData["RECEIVERCOMPANY"]?.ToString();
                                    tempData["RECEIVER"] = htData["RECEIVER"]?.ToString();
                                    tempData["RECEIVERZIPCODE"] = htData["RECEIVERZIPCODE"]?.ToString();
                                    tempData["RECEIVERPHONE"] = htData["RECEIVERPHONE"]?.ToString();
                                    tempData["RECEIVERADDR"] = htData["RECEIVERADDR"]?.ToString();
                                    tempData["TAXID"] = htData["TAXID"]?.ToString();
                                }
                                else
                                {
                                    tempData["RECEIVERCOMPANY"] = "";
                                    tempData["RECEIVER"] = "";
                                    tempData["RECEIVERZIPCODE"] = "";
                                    tempData["RECEIVERPHONE"] = "";
                                    tempData["RECEIVERADDR"] = "";
                                    tempData["TAXID"] = "";
                                }
                                tempData["UPDDATE"] = DateTime.Now;
                                tempData["UPDBY"] = htData["_usercode"];

                                objUsm.UpdateCusShippingM(tempData);
                            }
                        }

                        ws.Dispose();

                        //匯入成功回傳集運單號
                        string SHIPPINGNO = DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {MID}");

                        ts.Complete();


                        return SHIPPINGNO;
                    }

                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                    return "error99";
                }
            }
        }
        #endregion
    }
}