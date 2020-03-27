using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using Tectransit.Datas;


namespace Tectransit.Datas
{
    public class user
    {
        CommonHelper objComm = new CommonHelper();
        public dynamic Login(Hashtable request, bool IsEncode)
        {
            string sUSERCODE = ConvertString(request["USERCODE"]).ToUpper();
            string sPASSWORD = ConvertString(request["PASSWORD"]);

            //用戶名帳號密碼檢查
            if (String.IsNullOrEmpty(sUSERCODE) || String.IsNullOrEmpty(sPASSWORD))
                return new { status = "error", message = "帳號或密碼不能為空！" };

            //將PWD改為MD5
            if (IsEncode)
                sPASSWORD = objComm.GetMd5Hash(sPASSWORD);
            
            //用戶名密碼驗證
            bool IsPWCorr = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERPASSWORD AS COL1 FROM T_S_USER WHERE USERCODE = '{sUSERCODE}' AND USERPASSWORD = '{sPASSWORD}'"));

            if (IsPWCorr)
                return new { status = "error", message = "登入帳號或密碼錯誤！" };

            //用戶狀態及權限組停用驗證
            string IsUSEREnable = DBUtil.GetSingleValue1($@"SELECT DISTINCT A.USERCODE AS COL1 FROM T_S_USER A
                                                          LEFT JOIN T_S_USERROLEMAP B ON A.USERCODE = B.USERCODE
                                                          LEFT JOIN T_S_ROLE C ON C.ROLECODE = B.ROLECODE
                                                          WHERE A.USERCODE = '{sUSERCODE}' AND A.ISENABLE = 'true' AND C.ISENABLE = 'true'");

            if (string.IsNullOrEmpty(IsUSEREnable))
                return new { status = "error", message = "帳號或權限已被停用，請洽資訊人員！" };

            //登入後處理
            UpdateUserLoginCountAndDate(sUSERCODE); //更新用戶登入次數及時間
            AppendLoginHistory(request);           //增加登入log

            //返回登入成功
            return new { status = "success", ID = sUSERCODE, message = "登入成功！" };
        }

        //function
        public void UpdateUserLoginCountAndDate(string UserCode)
        {
            DBUtil.EXECUTE($@"
                UPDATE T_S_USER
                SET LOGINCOUNT = ISNULL(LOGINCOUNT,0) + 1
                   ,LASTLOGINDATE = GETDATE(), UPDDATE = GETDATE()
                WHERE USERCODE = UPPER('{UserCode}') ");
        }

        public void AppendLoginHistory(Hashtable request)
        {
            Hashtable htData = new Hashtable();
            htData["USERCODE"] = ConvertString(request["USERCODE"]).ToUpper();
            htData["USERNAME"] = DBUtil.GetSingleValue1($@"SELECT USERNAME AS COL1 FROM T_S_USER WHERE USERCODE = '{ConvertString(request["USERCODE"]).ToUpper()}'");
            htData["HOSTNAME"] = ConvertString(request["HOSTNAME"]);
            htData["HOSTIP"] = ConvertString(request["ClientIP"]);
            htData["LOGIN_DATE"] = DateTime.Now;
            
            DBUtil.EXECUTE(@"INSERT INTO T_S_USERLOGINLOG (USERCODE, USERNAME, HOSTNAME, HOSTIP, LOGIN_DATE)
                             VALUES (@USERCODE, @USERNAME, @HOSTNAME, @HOSTIP, @LOGIN_DATE)", htData);

        }

        public dynamic ACLogin(Hashtable request, bool IsEncode)
        {
            string sUSERCODE = ConvertString(request["USERCODE"]);
            string sPASSWORD = ConvertString(request["PASSWORD"]);
            string sRANKTYPE = ConvertString(request["RANKTYPE"]);

            //ACC帳號密碼檢查
            if (String.IsNullOrEmpty(sUSERCODE) || String.IsNullOrEmpty(sPASSWORD))
                return new { status = "error", message = "帳號或密碼不能為空！" };

            //將PWD改為MD5
            if (IsEncode)
                sPASSWORD = objComm.GetMd5Hash(sPASSWORD);

            //用戶名密碼驗證
            bool IsPWCorr = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERPASSWORD AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{sUSERCODE}' AND USERPASSWORD = '{sPASSWORD}'"));

            if (IsPWCorr)
                return new { status = "error", message = "登入帳號或密碼錯誤！" };

            //用戶狀態及權限組停用驗證
            string IsUSEREnable = DBUtil.GetSingleValue1($@"SELECT DISTINCT A.USERCODE AS COL1 FROM T_S_ACCOUNT A
                                                          LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE
                                                          LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                                          WHERE A.USERCODE = '{sUSERCODE}' AND A.ISENABLE = 'true' AND C.RANKTYPE IN ({sRANKTYPE}) AND C.ISENABLE = 'true'");

            if (string.IsNullOrEmpty(IsUSEREnable))
                return new { status = "error", message = "帳號已被停用或不存在，請洽網站人員！" };

            //登入後處理
            UpdateAcLoginCountAndDate(sUSERCODE); //更新用戶登入次數及時間
            AppendACLoginHistory(request);           //增加登入log

            //返回登入成功
            return new { status = "success", ID = sUSERCODE, message = "登入成功！" };
        }

        public void UpdateAcLoginCountAndDate(string UserCode)
        {
            DBUtil.EXECUTE($@"
                UPDATE T_S_ACCOUNT
                SET LOGINCOUNT = ISNULL(LOGINCOUNT,0) + 1
                   ,LASTLOGINDATE = GETDATE(), UPDDATE = GETDATE()
                WHERE USERCODE = UPPER('{UserCode}') ");
        }

        public void AppendACLoginHistory(Hashtable request)
        {
            Hashtable htData = new Hashtable();
            htData["USERCODE"] = ConvertString(request["USERCODE"]).ToUpper();
            htData["USERNAME"] = DBUtil.GetSingleValue1($@"SELECT USERNAME AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{ConvertString(request["USERCODE"]).ToUpper()}'");
            htData["HOSTNAME"] = ConvertString(request["HOSTNAME"]);
            htData["HOSTIP"] = ConvertString(request["ClientIP"]);
            htData["LOGIN_DATE"] = DateTime.Now;

            DBUtil.EXECUTE(@"INSERT INTO T_S_ACLOGINLOG (USERCODE, USERNAME, HOSTNAME, HOSTIP, LOGIN_DATE)
                             VALUES (@USERCODE, @USERNAME, @HOSTNAME, @HOSTIP, @LOGIN_DATE)", htData);

        }

        private string ConvertString(object str)
        {
            if (str == null)
            {
                return "";
            }
            else
            {
                return str.ToString().Trim();
            }
        }

    }
}
