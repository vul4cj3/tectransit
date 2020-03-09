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

                sWhere += (sWhere == "" ? "WHERE" : " AND") + " C.RANKTYPE = 2";

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
                srhKey.Add("strackno", "TRASFERNO");
                srhKey.Add("sacccode", "ACCOUNTCODE");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["STATIONCODE"]?.ToString()))
                    if (htData["STATIONCODE"]?.ToString() != "ALL")
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.STATIONCODE = '" + htData["STATIONCODE"]?.ToString() + "'";

                if (!string.IsNullOrEmpty(htData["TRASFERNO"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " A.TRASFERNO LIKE '%" + htData["TRASFERNO"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["ACCOUNTCODE"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " B.USERCODE LIKE '%" + htData["ACCOUNTCODE"]?.ToString() + "%'";

            }

            //return objUsm.GetTransferListData(sWhere, pageIndex, pageSize);
            return "";
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
                        dataKey.Add("id", "TRANSFERID");
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
                        //UpdateTransfer(Convert.ToInt64(sData["TRANSFERID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[TRANSFERID({sData["TRANSFERID"]}):{((sData["STATUS"]?.ToString() == "1") ? "已入庫" : "未入庫")}]";
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
                        dataKey.Add("id", "TRANSFERID");
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
                        //UpdateTransfer(Convert.ToInt64(sData["TRANSFERID"]), sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[TRANSFERID({sData["TRANSFERID"]}):{((sData["STATUS"]?.ToString() == "1") ? "已入庫" : "未入庫")}]";
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
                srhKey.Add("sstationcode", "STATIONCODE");
                srhKey.Add("sshippingno", "SHIPPINGNO");
                srhKey.Add("strackingno", "TRACKINGNO");
                srhKey.Add("stransferno", "TRANSFERNO");
                srhKey.Add("sacccode", "ACCOUNTCODE");
                srhKey.Add("sstatus", "STATUS");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                {

                    htData[srhKey[t.Key]] = t.Value?.ToString();
                }

                if (!string.IsNullOrEmpty(htData["STATIONCODE"]?.ToString()))
                    if (htData["STATIONCODE"]?.ToString() != "ALL")
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATIONCODE LIKE '%" + htData["STATIONCODE"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["SHIPPINGNO"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " SHIPPINGNO LIKE '%" + htData["SHIPPINGNO"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["TRACKINGNO"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " TRACKINGNO LIKE '%" + htData["TRACKINGNO"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["TRANSFERNO"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " TRANSFERNO LIKE '%" + htData["TRANSFERNO"]?.ToString() + "%'";

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


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = new Hashtable();
                        sData["SHIPPINGIDM"] = AL[i].ToString();
                        sData["TRASFERNO"] = DBUtil.GetSingleValue1($@"SELECT TRASFERNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {sData["SHIPPINGIDM"]}");
                        sData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ACCOUNTID AS COL1 FROM T_V_SHIPPING_M WHERE ID = {sData["SHIPPINGIDM"]}");
                        sData["COMPANYNAME"] = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE ID = {sData["ACCOUNTID"]}");

                        //delete Declarant data
                        DeleteTVData_All("T_V_DECLARANT", sData);

                        //delete shipping_H & shipping_D data
                        DeleteTVData_All("T_V_SHIPPING_D", sData);
                        DeleteTVData_All("T_V_SHIPPING_H", sData);

                        //delete shipping_M data
                        DeleteTVData_All("T_V_SHIPPING_M", sData);

                        logMsg += (logMsg == "" ? "" : ",") + $@"[SHIPPINGIDM({sData["SHIPPINGIDM"]})=COMPANYNAME:{sData["COMPANYNAME"]?.ToString()}/TRASFERNO: {sData["TRASFERNO"]?.ToString()}]";
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

                //delete header&detail
                for (int i = 0; i < hidList.Count; i++)
                {
                    JValue temp = (JValue)hidList[i];
                    objComm.DeleteSingleTableData("T_V_SHIPPING_D", "SHIPPINGID_H", temp.ToString());
                    objComm.DeleteSingleTableData("T_V_SHIPPING_H", "ID", temp.ToString());
                }

                //delete declarant
                for (int i = 0; i < decidList.Count; i++)
                {
                    JValue temp = (JValue)decidList[i];
                    objComm.DeleteSingleTableData("T_V_DECLARANT", "ID", temp.ToString());
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
                mData["MAWBNO"] = arrData.Value<string>("mawbno");
                mData["CLEARANCENO"] = arrData.Value<string>("clearanceno");
                mData["HAWBNO"] = arrData.Value<string>("hawbno");
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
                        sData["SHIPPINGIDM"] = mData["SHIPPINGIDM"];
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
                                tempData["SHIPPINGIDM"] = mData["SHIPPINGIDM"];
                                tempData["SHIPPINGIDH"] = HID;
                                if (tempData["ID"]?.ToString() == "0")
                                    InsertCusShippingD(tempData);
                                else
                                    updateShippingD(tempData);
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
                        tempData["SHIPPINGIDM"] = mData["SHIPPINGIDM"];
                        if (tempData["ID"]?.ToString() == "0")
                            InsertTVDeclarant(tempData);
                        else
                            updateTVDeclarant(tempData);
                    }
                }

                //update master
                updateCusShippingM(mData);
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
            //return objUsm.GetTransferData(id);
            return "";
        }

        [HttpPost]
        public dynamic EditTransferData([FromBody] object form)
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

                return EditTransferData(id, htData);
            }
            catch(Exception ex)
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
                    //前台用戶自行註冊新增
                    //InsertAccount(htData);
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
            htData["USERSEQ"] = newSeq;
            htData["LOGINCOUNT"] = 0;
            htData["LASTLOGINDATE"] = DateTime.Now;
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_S_ACCOUNT(USERCODE, USERPASSWORD, USERSEQ, USERNAME, USERDESC, WAREHOUSENO, EMAIL, TAXID, IDPHOTO_F, IDPHOTO_B, PHONE, MOBILE, ADDR, ISENABLE, LOGINCOUNT, LASTLOGINDATE, CREDATE, CREATEBY, UPDDATE, UPDBY) 
                                        VALUES (@USERCODE, @USERPASSWORD, @USERSEQ, @USERNAME, @USERDESC, @WAREHOUSENO, @EMAIL, @TAXID, @IDPHOTO_F, @IDPHOTO_B, @PHONE, @MOBILE, @ADDR, @ISENABLE, @LOGINCOUNT, @LASTLOGINDATE, @CREDATE, @CREATEBY, @UPDDATE, @UPDBY)";

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
                    rowTSA.Userpassword = sData["USERPASSWORD"]?.ToString();
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

        private dynamic EditTransferData(long id, Hashtable htData)
        {
            try
            {
                if (id == 0)
                {
                    //由前台用戶自行新增
                    //InsertTranser(htData);
                }
                else
                {
                    //UpdateTransfer(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }
                    
                    objComm.AddUserControlLog(htData, $"transfer/edit/{id}", "快遞單管理-編輯", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        //private void UpdateTransfer(long id, Hashtable sData)
        //{
        //    var query = _context.TETransferH.Where(q => q.Id == id).FirstOrDefault();

        //    if (query != null)
        //    {
        //        TETransferH rowTET = query;

        //        if (sData["TRASFERCOMPANY"] != null)
        //            rowTET.Trasfercompany = sData["TRASFERCOMPANY"]?.ToString();
        //        if (sData["PLENGTH"] != null)
        //            rowTET.PLength = sData["PLENGTH"]?.ToString();
        //        if (sData["PWIDTH"] != null)
        //            rowTET.PWidth = sData["PWIDTH"]?.ToString();
        //        if (sData["PHEIGHT"] != null)
        //            rowTET.PHeight = sData["PHEIGHT"]?.ToString();
        //        if (sData["PWEIGHT"] != null)
        //            rowTET.PWeight = sData["PWEIGHT"]?.ToString();
        //        if (sData["PVALUEPRICE"] != null)
        //            rowTET.PValueprice = sData["PVALUEPRICE"]?.ToString();
        //        if (sData["STATUS"] != null)
        //            rowTET.Status = Convert.ToInt32(sData["STATUS"]);
        //        if (sData["REMARK"] != null)
        //            rowTET.Remark = sData["REMARK"]?.ToString();                

        //        if (sData.Count > 2)//排除cookies
        //        {
        //            rowTET.Upddate = DateTime.Now;
        //            rowTET.Updby = sData["_usercode"]?.ToString();

        //            _context.SaveChanges();
        //        }
        //    }
        //}

        private void updateCusShippingM(Hashtable sData)
        {
            var query = _context.TVShippingM.Where(q => q.Id == Convert.ToInt64(sData["ID"])).FirstOrDefault();
            if (query != null)
            {
                query.Total = sData["TOTAL"]?.ToString();
                query.Receiver = sData["RECEIVER"]?.ToString();
                query.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                query.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                query.Mawbno = sData["MAWBNO"]?.ToString();
                query.Clearanceno = sData["CLEARANCENO"]?.ToString();
                query.Hawbno = sData["HAWBNO"]?.ToString();
                query.Ismultreceiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;
                query.Status = Convert.ToInt32(sData["STATUS"]);

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
                TVH.Boxno = sData["BOXNO"]?.ToString();
                TVH.Receiver = sData["RECEIVER"]?.ToString();
                TVH.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                TVH.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
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
                query.Boxno = sData["BOXNO"]?.ToString();
                query.Receiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVER"]?.ToString() : "";
                query.Receiveraddr = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVERADDR"]?.ToString() : "";
                query.Receiverphone = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? sData["RECEIVERPHONE"]?.ToString() : "";

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

        private void updateShippingD(Hashtable sData)
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
                TVD.Addr = sData["ADDR"]?.ToString();
                TVD.IdphotoF = sData["IDPHOTOF"]?.ToString();
                TVD.IdphotoB = sData["IDPHOTOB"]?.ToString();
                TVD.Appointment = sData["APPOINTMENT"]?.ToString();
                TVD.ShippingidM = Convert.ToInt64(sData["SHIPPINGIDM"]);

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
        #endregion
    }
}