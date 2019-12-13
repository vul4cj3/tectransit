using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tectransit.Datas
{
    public class user
    {
        public object Login(JObject request, bool IsEncode)
        {
            string sUSERCODE = request.Value<string>("USERCODE")?.ToUpper();
            string sPASSWORD = request.Value<string>("PASSWORD");

            //用戶名帳號密碼檢查
            if (String.IsNullOrEmpty(sUSERCODE) || String.IsNullOrEmpty(sPASSWORD))
                return new { status = "error", message = "帳號或密碼不能為空！" };

            request["USERCODE"] = sUSERCODE;

            //將PWD改為MD5
            if (IsEncode)
                sPASSWORD = GetMd5Hash(sPASSWORD);
            
            //用戶名密碼驗證
            bool IsPWCorr = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERPASSWORD AS COL1 FROM T_S_USER WHERE USERCODE = '{sUSERCODE}' AND USERPASSWORD = '{sPASSWORD}'"));
            
            if (IsPWCorr)
                return new { status = "error", message = "登入密碼錯誤！" };            
            
            //登入後處理
            UpdateUserLoginCountAndDate(sUSERCODE); //更新用戶登入次數及時間
            AppendLoginHistory(request);           //增加登入履歷
            //LoginHandler(request);
            
            //返回登入成功
            return new { status = "success", message = "處理中，請稍後...", loading = "登入中，請稍後..." };
        }

        //function
        public void UpdateUserLoginCountAndDate(string UserCode)
        {
            DBUtil.EXECUTE($@"
                UPDATE T_S_USER
                SET LOGINCOUNT = ISNULL(LOGINCOUNT,0) + 1
                   ,LASTLOGINDATE = SYSDATE
                WHERE USERCODE = UPPER('{UserCode}') ");
        }

        public void AppendLoginHistory(JObject request)
        {
            var lanIP = request.Value<string>("ClientIP");
            //var hostName = ZHttp.IsLanIP(lanIP) ? ZHttp.ClientHostName : string.Empty; //如果是內網就獲取，否則出錯獲取不到，且影響效率

            var UserCode = request.Value<string>("USERCODE").ToUpper();
            //var UserName = FormsAuth.GetUserData().USERCODE;
            var IP = request.Value<string>("IP");
            var City = request.Value<string>("CITY");
            if (IP != lanIP)
                IP = string.Format("{0}/{1}", IP, lanIP).Trim('/').Replace("::1", "localhost");


            DBUtil.EXECUTE(@"INSERT INTO T_S_USERLOGINLOG ()
                             VALUES ()");
            
        }

        public static string GetMd5Hash(string argInput)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(argInput))).Replace("-", "");
            }
        }
    }
}
