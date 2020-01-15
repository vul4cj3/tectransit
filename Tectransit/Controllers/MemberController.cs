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

        [HttpPost]
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
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

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
            rowTSA.Taxid = htData["IDCODE"]?.ToString();
            rowTSA.Phone = htData["TEL"]?.ToString();
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
            rowTSA.Taxid = htData["IDCODE"]?.ToString();
            rowTSA.Phone = htData["TEL"]?.ToString();
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

                if (sData["USERPASSWORD"] != null)
                    rowTSA.Userpassword = objComm.GetMd5Hash(sData["USERPASSWORD"]?.ToString());
                if (sData["USERNAME"] != null)
                    rowTSA.Username = sData["USERNAME"]?.ToString();
                if (sData["USERDESC"] != null)
                    rowTSA.Userdesc = sData["USERDESC"]?.ToString();
                if (sData["EMAIL"] != null)
                    rowTSA.Email = sData["EMAIL"]?.ToString();
                if (sData["IDCODE"] != null)
                    rowTSA.Taxid = sData["IDCODE"]?.ToString();
                if (sData["TEL"] != null)
                    rowTSA.Phone = sData["TEL"]?.ToString();
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

    }
}