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
    [Route("api/Member/[action]")]
    public class MemberController : Controller
    {
        CommonHelper objComm = new CommonHelper();
        MemberHelper objMember = new MemberHelper();
        private readonly TECTRANSITDBContext _context;

        public MemberController(TECTRANSITDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public dynamic GetMemData()
        {
            Hashtable htData = new Hashtable();
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            return objMember.GetMemData(htData);
        }

        [HttpPost]
        public dynamic EditMemData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                long id = Convert.ToInt64(jsonData.Value<string>("id"));
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();


                //檢查舊密碼是否符合(不符合則不可修改)
                if (!string.IsNullOrEmpty(htData["USERPASSWORD"]?.ToString()) && !string.IsNullOrEmpty(htData["NEWPW"]?.ToString()))
                {
                    string oldPW = DBUtil.GetSingleValue1($@"SELECT USERPASSWORD AS COL1 FROM T_S_ACCOUNT WHERE ID = {id}");
                    if (objComm.GetMd5Hash(htData["USERPASSWORD"]?.ToString()) != oldPW)
                        return new { status = "99", msg = "保存失敗，用戶密碼(舊密碼)輸入錯誤！" };
                }
                

                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                UpdateMemData(id, htData);

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "0", msg = "保存失敗！" };
            }
        }

        [HttpPost]
        public dynamic SaveRegData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                htData["RANDONCODE"] = objComm.GetRandomString();

                //檢查用戶名是否重複
                bool IsRepeat = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["USERCODE"]}' AND ISENABLE = '1'")) ? false : true;
                if (IsRepeat)
                {
                    return new { status = "99", id = "", msg = "此用戶帳號已有人使用！" }; ;
                }


                InsertMemData(htData);
                SendEmailAccept(htData);

                string userid = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["USERCODE"]}' ORDER BY CREDATE DESC");

                return new { status = "0", id = userid, msg = "註冊成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", id = "0", msg = "註冊失敗！" };
            }
        }

        [HttpPost]
        public dynamic SaveRegCompanyData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                htData["RANDONCODE"] = objComm.GetRandomString();

                //檢查用戶名是否重複
                bool IsRepeat = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["USERCODE"]}' AND ISENABLE = '1'")) ? false : true;
                if (IsRepeat)
                {
                    return new { status = "99", id = "", msg = "此用戶帳號已有人使用！" }; ;
                }


                InsertMemCompanyData(htData);
                SendEmailAccept(htData);

                string userid = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["USERCODE"]}' ORDER BY CREDATE DESC");

                return new { status = "0", id = userid, msg = "註冊成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", id = "0", msg = "註冊失敗！" };
            }
        }

        [HttpPost]
        public dynamic ConfirmEmailCode([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                long id = Convert.ToInt64(htData["USERID"]?.ToString());

                string EMAILCODE = DBUtil.GetSingleValue1($@"SELECT USERDESC AS COL1 FROM T_S_ACCOUNT WHERE ID = {id}");

                //get cookies
                htData["_acccode"] = "SYSTEM";
                htData["_accname"] = "SYSTEM";

                if (EMAILCODE.Trim() == (htData["EMAILCODE"]?.ToString()).Trim())
                {
                    htData["USERDESC"] = "";
                    htData["ISENABLE"] = "1";
                    UpdateMemData(id, htData);
                }
                else
                {
                    return new { status = "99", msg = "認證失敗！" };
                }

                return new { status = "0", msg = "認證成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "認證失敗！" };
            }
        }

        [HttpGet]
        public dynamic GetStationData()
        {
            Hashtable htData = new Hashtable();
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            return objMember.GetACStationData(htData);
        }

        /*--------------------- 個人會員 ------------------------*/

        #region 快遞單號(未入庫,已入庫)
        //存入快遞單號
        [HttpPost]
        public dynamic SaveTansferData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");
                JArray rowsData = arrData.Value<JArray>("rows");

                Hashtable htData = new Hashtable();

                htData["STATIONCODE"] = arrData.Value<string>("stationcode");
                htData["TRASFERNO"] = arrData.Value<string>("trasferno");

                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["_acccode"]}'");

                //新增主單
                string HID = InsertTransferH(htData);

                //新增細項
                ArrayList AL = new ArrayList();
                for (int i = 0; i < rowsData.Count; i++)
                {
                    JObject temp = (JObject)rowsData[i];

                    Hashtable tempData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("product", "PRODUCT");
                        dataKey.Add("price", "UNIT_PRICE");
                        dataKey.Add("quantity", "QUANTITY");
                        tempData[dataKey[t.Key]] = t.Value?.ToString();
                    }

                    tempData["TRANSFERHID"] = HID;

                    AL.Add(tempData);
                }

                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        InsertTransferD(sData);
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

        //取得快遞單號
        [HttpPost]
        public dynamic GetACTransferData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                Hashtable htData = new Hashtable();
                var jsonData = JObject.FromObject(form);
                int pageIndex = jsonData.Value<int>("PAGE_INDEX");
                int pageSize = jsonData.Value<int>("PAGE_SIZE");
                JArray tempArray = jsonData.Value<JArray>("srhForm");

                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                htData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["_acccode"]}'");

                if (tempArray.Count > 0)
                {
                    Dictionary<string, string> srhKey = new Dictionary<string, string>();
                    srhKey.Add("status", "STATUS");
                    srhKey.Add("stationcode", "STATIONCODE");

                    JObject temp = (JObject)tempArray[0];
                    foreach (var t in temp)
                        htData[srhKey[t.Key]] = t.Value?.ToString();

                    if (!string.IsNullOrEmpty(htData["ACCOUNTID"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " ACCOUNTID = " + htData["ACCOUNTID"];

                    if (!string.IsNullOrEmpty(htData["STATUS"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATUS = " + (htData["STATUS"]?.ToString() == "t1" ? 0 : 1);

                    if (!string.IsNullOrEmpty(htData["STATIONCODE"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATIONCODE = '" + htData["STATIONCODE"]?.ToString() + "'";
                }


                return objMember.GetTransferData(sWhere, pageIndex, pageSize);

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        //取得要合併的快遞單號資料
        [HttpPost]
        public dynamic GetTransferData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                var jsonData = JObject.FromObject(form);
                string transid = jsonData.Value<string>("id");
                
                if (!string.IsNullOrEmpty(transid))
                {
                    string[] arrList = transid.Split(';');
                    if (arrList.Length > 0)
                    {
                        
                        for (int i = 0; i < arrList.Length; i++)
                        {
                            sWhere += (sWhere == "" ? "" : ",") + $@"'{arrList[i]}'";
                        }
                    }
                }

                return objMember.GetTransferData_Combine(sWhere);

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }
        #endregion

        #region 集運單(待出貨,已出貨,已完成)
        //取得集運單
        [HttpPost]
        public dynamic GetACShippingData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                Hashtable htData = new Hashtable();
                var jsonData = JObject.FromObject(form);
                int pageIndex = jsonData.Value<int>("PAGE_INDEX");
                int pageSize = jsonData.Value<int>("PAGE_SIZE");
                JArray tempArray = jsonData.Value<JArray>("srhForm");

                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                htData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["_acccode"]}'");

                if (tempArray.Count > 0)
                {
                    Dictionary<string, string> srhKey = new Dictionary<string, string>();
                    srhKey.Add("status", "STATUS");
                    srhKey.Add("stationcode", "STATIONCODE");

                    JObject temp = (JObject)tempArray[0];
                    foreach (var t in temp)
                        htData[srhKey[t.Key]] = t.Value?.ToString();

                    if (!string.IsNullOrEmpty(htData["ACCOUNTID"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " ACCOUNTID = " + htData["ACCOUNTID"];

                    if (!string.IsNullOrEmpty(htData["STATUS"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATUS = " + (htData["STATUS"]?.ToString() == "t1" ? 0 : 1);

                    if (!string.IsNullOrEmpty(htData["STATIONCODE"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATIONCODE = '" + htData["STATIONCODE"]?.ToString() + "'";
                }


                return objMember.GetShippingData(sWhere, pageIndex, pageSize);

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic GetACTransferData(long id)
        {
            Hashtable htData = new Hashtable();
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            return objMember.GetACTransferData(id);
        }

        [HttpPost]
        public dynamic DelACTransferData([FromBody] object form)
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
                        dataKey.Add("id", "TRANSFERID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();

                        htData["_acccode"] = Request.Cookies["_acccode"];
                        htData["_accname"] = Request.Cookies["_accname"];
                    }

                    AL.Add(htData);
                }

                if (AL.Count > 0)
                {
                    for (int j = 0; j < AL.Count; j++)
                    {
                        Hashtable tempData = (Hashtable)AL[j];
                        objMember.DelACTransferData(tempData);
                    }
                }

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        #region 申報人管理
        [HttpGet]
        public dynamic GetACDeclarantData()
        {
            Hashtable htData = new Hashtable();
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            return objMember.GetDeclarantData(htData);
        }

        [HttpGet("{id}")]
        public dynamic GetDeclarantData(long id)
        {
            Hashtable htData = new Hashtable();
            htData["DECLARANTID"] = id;
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            return objMember.GetDeclarantData(htData);
        }

        [HttpPost]
        public dynamic SaveDeclarantData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                if (htData["ID"]?.ToString() == "0")
                    InsertDeclarant(htData, 2);
                else
                    UpdateDeclarant(Convert.ToInt64(htData["ID"]), htData);
                
                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", id = "0", msg = "保存失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic EditDeclarantData(long id)
        {
            try
            {
                Hashtable htData = new Hashtable();
                htData["DECLARANTID"] = id;
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                DelDeclarant(htData);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        #region 私有function

        private void InsertMemCompanyData(Hashtable htData)
        {

            TSAccount rowTSA = new TSAccount();

            rowTSA.Usercode = htData["USERCODE"]?.ToString();
            rowTSA.Userpassword = objComm.GetMd5Hash(htData["USERPASSWORD"]?.ToString());
            rowTSA.Userdesc = (htData["RANDONCODE"]?.ToString()).Trim();
            rowTSA.Username = htData["USERNAME"]?.ToString();
            rowTSA.Companyname = htData["COMPANYNAME"]?.ToString();
            rowTSA.Rateid = htData["RATEID"]?.ToString();
            rowTSA.Warehouseno = "";
            rowTSA.Email = htData["EMAIL"]?.ToString();
            rowTSA.Taxid = htData["TAXID"]?.ToString();
            rowTSA.Phone = htData["PHONE"]?.ToString();
            rowTSA.Mobile = htData["MOBILE"]?.ToString();
            rowTSA.Addr = htData["ADDRESS"]?.ToString();

            rowTSA.Isenable = false;
            rowTSA.Logincount = 0;
            rowTSA.Lastlogindate = DateTime.Now;
            rowTSA.Credate = rowTSA.Lastlogindate;
            rowTSA.Createby = "SYSTEM";
            rowTSA.Upddate = rowTSA.Credate;
            rowTSA.Updby = "SYSTEM";

            _context.TSAccount.Add(rowTSA);
            _context.SaveChanges();
            
            TSAcrankmap rowTSAR = new TSAcrankmap();
            rowTSAR.Usercode = htData["USERCODE"]?.ToString();
            rowTSAR.Rankid = 2;//一般廠商會員

            _context.TSAcrankmap.Add(rowTSAR);
            _context.SaveChanges();
            

        }

        private void InsertMemData(Hashtable htData)
        {

            TSAccount rowTSA = new TSAccount();

            rowTSA.Usercode = htData["USERCODE"]?.ToString();
            rowTSA.Userpassword = objComm.GetMd5Hash(htData["USERPASSWORD"]?.ToString());
            rowTSA.Userdesc = (htData["RANDONCODE"]?.ToString()).Trim();
            rowTSA.Username = htData["USERNAME"]?.ToString();
            string WCode = objComm.GetSeqCode("WAREHOUSENO");
            rowTSA.Warehouseno = "TECW-" + WCode;
            rowTSA.Email = htData["EMAIL"]?.ToString();
            rowTSA.Taxid = htData["TAXID"]?.ToString();
            rowTSA.Phone = htData["PHONE"]?.ToString();
            rowTSA.Mobile = htData["MOBILE"]?.ToString();
            rowTSA.Addr = htData["ADDRESS"]?.ToString();

            rowTSA.Isenable = false;
            rowTSA.Logincount = 0;
            rowTSA.Lastlogindate = DateTime.Now;
            rowTSA.Credate = rowTSA.Lastlogindate;
            rowTSA.Createby = "SYSTEM";
            rowTSA.Upddate = rowTSA.Credate;
            rowTSA.Updby = "SYSTEM";

            _context.TSAccount.Add(rowTSA);
            _context.SaveChanges();

            TSAcrankmap rowTSAR = new TSAcrankmap();
            rowTSAR.Usercode = htData["USERCODE"]?.ToString();
            rowTSAR.Rankid = 1;//一般會員

            _context.TSAcrankmap.Add(rowTSAR);
            _context.SaveChanges();

            //Update code
            objComm.UpdateSeqCode("WAREHOUSENO");

        }

        private void UpdateMemData(long id, Hashtable sData)
        {
            var query = _context.TSAccount.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSAccount rowTSA = query;

                if (!string.IsNullOrEmpty(sData["NEWPW"]?.ToString()))
                    rowTSA.Userpassword = objComm.GetMd5Hash(sData["NEWPW"]?.ToString());
                if (sData["USERNAME"] != null)
                    rowTSA.Username = sData["USERNAME"]?.ToString();
                if (sData["USERDESC"] != null)
                    rowTSA.Userdesc = sData["USERDESC"]?.ToString();
                if (sData["EMAIL"] != null)
                    rowTSA.Email = sData["EMAIL"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTSA.Taxid = sData["TAXID"]?.ToString();
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
                    rowTSA.Updby = sData["_acccode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void SendEmailAccept(Hashtable sData)
        {
            string F_User = "TEC Website System<ebs.sys@t3ex-group.com>";
            string T_User = sData["EMAIL"]?.ToString();
            string subject = "TEC轉運平台信箱認證信";
            string body = "";

            body += $"<p>認證信箱代碼：{sData["RANDONCODE"]}</p><br/>";
            body += "<p>請將此代碼輸入到驗證信箱欄位，來完成註冊程序，謝謝</p>";

            string C_User = "siawu@t3ex-group.com";

            objComm.SendMail(F_User, T_User, subject, body, C_User);
        }

        private string InsertTransferH(Hashtable sData)
        {
            TETransferH TEH = new TETransferH();
            TEH.Accountid = Convert.ToInt64(sData["ACCOUNTID"]);
            TEH.Stationcode = sData["STATIONCODE"].ToString();
            TEH.Trasferno = sData["TRASFERNO"].ToString();
            TEH.Status = 0;

            TEH.Credate = DateTime.Now;
            TEH.Createby = sData["_acccode"].ToString();
            TEH.Upddate = DateTime.Now;
            TEH.Updby = sData["_acccode"].ToString();

            _context.TETransferH.Add(TEH);
            _context.SaveChanges();

            string hid = TEH.Id.ToString();

            return hid;
            
        }

        private void InsertTransferD(Hashtable sData)
        {
            TETransferD TED = new TETransferD();
            TED.Product = sData["PRODUCT"].ToString();
            TED.Quantity = sData["QUANTITY"].ToString();
            TED.UnitPrice = sData["UNIT_PRICE"].ToString();
            TED.Transferhid = sData["TRANSFERHID"].ToString();

            _context.TETransferD.Add(TED);
            _context.SaveChanges();
            
        }

        private void InsertDeclarant(Hashtable htData, int type)
        {

            TSDeclarant rowTSD = new TSDeclarant();

            rowTSD.Type = type;
            rowTSD.Name = (htData["NAME"]?.ToString()).Trim();
            rowTSD.Taxid = htData["TAXID"]?.ToString();
            rowTSD.Phone = htData["PHONE"]?.ToString();
            rowTSD.Mobile = htData["MOBILE"]?.ToString();
            rowTSD.Addr = htData["ADDR"]?.ToString();
            rowTSD.IdphotoF = htData["IDPHOTO_F"]?.ToString();
            rowTSD.IdphotoB = htData["IDPHOTO_B"]?.ToString();
            rowTSD.Appointment = htData["APPOINTMENT"]?.ToString();

            rowTSD.Credate = DateTime.Now;
            rowTSD.Createby = htData["_acccode"]?.ToString();
            rowTSD.Upddate = rowTSD.Credate;
            rowTSD.Updby = htData["_acccode"]?.ToString();
            
            _context.TSDeclarant.Add(rowTSD);
            _context.SaveChanges();

            long decID = rowTSD.Id;

            TSAcdeclarantmap rowTSAD = new TSAcdeclarantmap();
            rowTSAD.Usercode = htData["_acccode"]?.ToString();
            rowTSAD.Declarantid = decID;

            _context.TSAcdeclarantmap.Add(rowTSAD);
            _context.SaveChanges();            

        }

        private void UpdateDeclarant(long id, Hashtable sData)
        {
            var query = _context.TSDeclarant.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSDeclarant rowTSD = query;
                
                if (sData["NAME"] != null)
                    rowTSD.Name = sData["NAME"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTSD.Taxid = sData["TAXID"]?.ToString();
                if (sData["PHONE"] != null)
                    rowTSD.Phone = sData["PHONE"]?.ToString();
                if (sData["MOBILE"] != null)
                    rowTSD.Mobile = sData["MOBILE"]?.ToString();
                if (sData["ADDR"] != null)
                    rowTSD.Addr = sData["ADDR"]?.ToString();
                if (sData["IDPHOTO_F"] != null)
                    rowTSD.IdphotoF = sData["IDPHOTO_F"]?.ToString();
                if (sData["IDPHOTO_B"] != null)
                    rowTSD.IdphotoB = sData["IDPHOTO_B"]?.ToString();
                if (sData["APPOINTMENT"] != null)
                    rowTSD.Appointment = sData["APPOINTMENT"]?.ToString();

                if (sData.Count > 2)//排除cookies
                {
                    rowTSD.Upddate = DateTime.Now;
                    rowTSD.Updby = sData["_acccode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void DelDeclarant(Hashtable sData)
        {
            var query = _context.TSDeclarant.Where(q => q.Id == Convert.ToInt64(sData["DECLARANTID"]) && q.Createby == sData["_acccode"].ToString()).FirstOrDefault();
            if (query != null)
            {
                _context.TSDeclarant.Remove(query);
                _context.SaveChanges();
            }

            var query_ = _context.TSAcdeclarantmap.Where(q => q.Id == Convert.ToInt64(sData["DECLARANTID"]) && q.Usercode == sData["_acccode"].ToString()).FirstOrDefault();
            if (query_ != null)
            {
                _context.TSAcdeclarantmap.Remove(query_);
                _context.SaveChanges();
            }
        }
        #endregion


        /*--------------------- 廠商會員 ------------------------*/

        #region 集運單
        //取得集運單
        [HttpPost]
        public dynamic GetShippingCusData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                Hashtable htData = new Hashtable();
                var jsonData = JObject.FromObject(form);
                int pageIndex = jsonData.Value<int>("PAGE_INDEX");
                int pageSize = jsonData.Value<int>("PAGE_SIZE");
                JArray tempArray = jsonData.Value<JArray>("srhForm");

                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                string sql = $@"SELECT A.ID AS COL1 FROM T_S_ACCOUNT A
                                LEFT JOIN T_S_ACRANKMAP B ON B.USERCODE = A.USERCODE
                                LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                WHERE A.USERCODE = '{htData["_acccode"]}' AND C.RANKTYPE = '2'";

                htData["ACCOUNTID"] = DBUtil.GetSingleValue1(sql);

                if (tempArray.Count > 0)
                {
                    Dictionary<string, string> srhKey = new Dictionary<string, string>();
                    srhKey.Add("status", "STATUS");
                    srhKey.Add("stationcode", "STATIONCODE");

                    JObject temp = (JObject)tempArray[0];
                    foreach (var t in temp)
                        htData[srhKey[t.Key]] = t.Value?.ToString();

                    if (!string.IsNullOrEmpty(htData["ACCOUNTID"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " ACCOUNTID = " + htData["ACCOUNTID"];

                    if (!string.IsNullOrEmpty(htData["STATUS"]?.ToString()))
                    {
                        int status = 0;
                        if (htData["STATUS"]?.ToString() == "t1")
                            status = 0;
                        else if (htData["STATUS"]?.ToString() == "t2")
                            status = 1;
                        else if (htData["STATUS"]?.ToString() == "t3")
                            status = 2;
                        else if (htData["STATUS"]?.ToString() == "t4")
                            status = 3;
                        else if (htData["STATUS"]?.ToString() == "t5")
                            status = 4;
                        else { }

                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATUS = " + status;
                    }
                    
                    if (!string.IsNullOrEmpty(htData["STATIONCODE"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATIONCODE = '" + htData["STATIONCODE"]?.ToString() + "'";
                }


                return objMember.GetShippingCusData(sWhere, pageIndex, pageSize);

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        [HttpPost]
        public dynamic SaveCusShippingData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                //Master data
                Hashtable mData = new Hashtable();
                mData["STATIONCODE"] = arrData.Value<string>("stationcode");
                mData["TRASFERNO"] = arrData.Value<string>("trasferno");
                mData["TOTAL"] = arrData.Value<string>("total");
                mData["ISMULTRECEIVER"] = arrData.Value<string>("ismultreceiver");

                if (string.IsNullOrEmpty(mData["ISMULTRECEIVER"]?.ToString())) {
                    if (arrData.Value<string>("receiver") != null)
                        mData["RECEIVER"] = arrData.Value<string>("receiver");
                    if (arrData.Value<string>("receiveraddr") != null)
                        mData["RECEIVERADDR"] = arrData.Value<string>("receiveraddr");                    
                }

                mData["_acccode"] = Request.Cookies["_acccode"];

                string sql = $@"SELECT A.ID AS COL1 FROM T_S_ACCOUNT A
                                LEFT JOIN T_S_ACRANKMAP B ON B.USERCODE = A.USERCODE
                                LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                WHERE A.USERCODE = '{mData["_acccode"]}' AND C.RANKTYPE = '2'";

                mData["ACCOUNTID"] = DBUtil.GetSingleValue1(sql);

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
                
                //新增主單
                long MID = InsertCusShippingM(mData);

                //新增各箱細項                
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        sData["SHIPPINGIDM"] = MID;
                        long HID = InsertCusShippingH(sData);

                        ArrayList subAL = (ArrayList)sData["PRDFORM"];
                        if (subAL.Count > 0)
                        {
                            for (int j = 0; j < subAL.Count; j++)
                            {
                                Hashtable tempData = (Hashtable)subAL[j];
                                tempData["SHIPPINGIDM"] = MID;
                                tempData["SHIPPINGIDH"] = HID;
                                InsertCusShippingD(tempData);
                            }
                        }

                    }
                }

                //新增申報人名單
                if (decAL.Count > 0)
                {
                    for (int k = 0; k < decAL.Count; k++)
                    {
                        Hashtable tempData = (Hashtable)decAL[k];
                        tempData["SHIPPINGIDM"] = MID;
                        InsertTVDeclarant(tempData);
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

        #endregion

        #region 私有function
        private long InsertCusShippingM(Hashtable sData)
        {
            long ID = 0;
            try
            {
                TVShippingM TVM = new TVShippingM();
                TVM.Accountid = Convert.ToInt64(sData["ACCOUNTID"]);
                string autoSeqcode = objComm.GetSeqCode(sData["STATIONCODE"] + "_CUS");
                TVM.Stationcode = sData["STATIONCODE"]?.ToString();
                TVM.Shippingno = "TECV" + DateTime.Now.ToString("yyyyMMdd") + autoSeqcode;
                TVM.Trackingno = sData["STATIONCODE"] + "-" + autoSeqcode;
                TVM.Mawbno = "";
                TVM.Trasferno = sData["TRASFERNO"]?.ToString();
                TVM.Total = sData["TOTAL"]?.ToString();
                TVM.Trackingtype = 0;//尚未使用-0:無
                TVM.Receiver = sData["RECEIVER"]?.ToString();
                TVM.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                TVM.Ismultreceiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;
                TVM.Status = 0;
                TVM.Credate = DateTime.Now;
                TVM.Createby = sData["_acccode"]?.ToString();
                TVM.Upddate = TVM.Credate;
                TVM.Updby = TVM.Createby;
                
                _context.TVShippingM.Add(TVM);
                _context.SaveChanges();

                ID = TVM.Id;

                //更新流水號
                objComm.UpdateSeqCode(sData["STATIONCODE"] + "_CUS");

                return ID;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return ID;
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
                TVH.Receiceraddr = sData["RECEIVERADDR"]?.ToString();
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
        #endregion

    }
}