using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Tectransit.Datas
{
    public class CommonHelper
    {
        //取得後台選單
        public dynamic GetMenu(string USERCODE)
        {
            //Get Role
            string sRolelist = "";
            DataTable dtRoles = DBUtil.SelectDataTable($@"SELECT A.ROLECODE FROM T_S_ROLE A 
                                                         LEFT JOIN T_S_USERROLEMAP B ON A.ROLECODE = B.ROLECODE
                                                         WHERE B.USERCODE = '{USERCODE}' AND A.ISENABLE = 'true'");
            if (dtRoles.Rows.Count > 0)
            {
                for (int i = 0; i < dtRoles.Rows.Count; i++)
                    sRolelist += (sRolelist == "" ? "" : ",") + "'" + dtRoles.Rows[i]["ROLECODE"] + "'";

                DataTable dtMenulist = DBUtil.SelectDataTable($@"SELECT DISTINCT A.MENUSEQ, A.MENUCODE, A.PARENTCODE,A.MENUURL, A.MENUNAME, A.ICONURL FROM T_S_MENU A
                                                                 LEFT JOIN T_S_ROLEMENUMAP B ON A.MENUCODE = B.MENUCODE
                                                                 WHERE B.ROLECODE IN ({sRolelist}) AND A.ISBACK = 'true' AND ISVISIBLE = 'true' AND ISENABLE = 'true'
                                                                 ORDER BY A.MENUSEQ");
                if (dtMenulist.Rows.Count > 0)
                {
                    List<MenuInfo> pmenuList = new List<MenuInfo>();
                    List<MenuInfo> dmenuList = new List<MenuInfo>();
                    for (int i = 0; i < dtMenulist.Rows.Count; i++)
                    {
                        MenuInfo m = new MenuInfo();
                        m.MENUCODE = dtMenulist.Rows[i]["MENUCODE"]?.ToString();
                        m.PARENTCODE = dtMenulist.Rows[i]["PARENTCODE"]?.ToString();
                        m.MENUURL = dtMenulist.Rows[i]["MENUURL"]?.ToString();
                        m.MENUNAME = dtMenulist.Rows[i]["MENUNAME"]?.ToString();
                        m.ICONURL = dtMenulist.Rows[i]["ICONURL"]?.ToString();

                        if (m.PARENTCODE == "0")
                            pmenuList.Add(m);
                        else
                            dmenuList.Add(m);
                    }

                    return new { status = "0", pList = pmenuList, item = dmenuList };
                }

                return new { status = "99", pList = "", item = "" };
            }
            else
                return new { status = "99", pList = "", item = "" };

        }

        //取得所有後台選單資料
        public dynamic GetAllMenu(string code)
        {
            DataTable dtMenulist = DBUtil.SelectDataTable($@"SELECT A.MENUSEQ, A.MENUCODE, A.PARENTCODE,A.MENUURL, A.MENUNAME, A.ICONURL FROM T_S_MENU A
                                                             WHERE A.ISBACK = 'true' AND ISENABLE = 'true'
                                                             ORDER BY A.MENUCODE");
            if (dtMenulist.Rows.Count > 0)
            {
                List<MenuInfo> pmenuList = new List<MenuInfo>();
                List<MenuInfo> dmenuList = new List<MenuInfo>();
                for (int i = 0; i < dtMenulist.Rows.Count; i++)
                {
                    MenuInfo m = new MenuInfo();
                    m.MENUCODE = dtMenulist.Rows[i]["MENUCODE"]?.ToString();
                    m.PARENTCODE = dtMenulist.Rows[i]["PARENTCODE"]?.ToString();
                    m.MENUURL = dtMenulist.Rows[i]["MENUURL"]?.ToString();
                    m.MENUNAME = dtMenulist.Rows[i]["MENUNAME"]?.ToString();
                    m.ICONURL = dtMenulist.Rows[i]["ICONURL"]?.ToString();
                    // 0:無權限 1:有權限
                    m.HASPOWER = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT MENUCODE AS COL1 FROM T_S_ROLEMENUMAP WHERE ROLECODE = '{code}' AND MENUCODE = '{m.MENUCODE}'")) ? "0" : "1";

                    if (m.PARENTCODE == "0")
                        pmenuList.Add(m);
                    else
                        dmenuList.Add(m);
                }

                return new { status = "0", pList = pmenuList, item = dmenuList };
            }

            return new { status = "99", pList = "", item = "" };

        }

        //取得所有權限組資料
        public dynamic GetAllRole(string code)
        {
            DataTable dtlist = DBUtil.SelectDataTable($@"SELECT A.ID AS ROLEID, A.ROLECODE, A.ROLENAME, A.ROLESEQ FROM T_S_ROLE A
                                                         ORDER BY A.ROLESEQ");
            if (dtlist.Rows.Count > 0)
            {
                List<UserRoleInfo> roleList = new List<UserRoleInfo>();
                for (int i = 0; i < dtlist.Rows.Count; i++)
                {
                    UserRoleInfo m = new UserRoleInfo();
                    m.ROLEID = dtlist.Rows[i]["ROLEID"]?.ToString();
                    m.ROLECODE = dtlist.Rows[i]["ROLECODE"]?.ToString();
                    m.ROLENAME = dtlist.Rows[i]["ROLENAME"]?.ToString();
                    m.ROLESEQ = dtlist.Rows[i]["ROLESEQ"]?.ToString();
                    // 0:無權限 1:有權限
                    m.HASPOWER = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_USERROLEMAP WHERE USERCODE = '{code}' AND ROLECODE = '{m.ROLECODE}'")) ? "0" : "1";

                    roleList.Add(m);
                }

                return new { status = "0", item = roleList };
            }

            return new { status = "99", pList = "", item = "" };

        }

        //取得前台用戶權限組
        public dynamic GetAllRank(string code)
        {
            DataTable dtlist = DBUtil.SelectDataTable($@"SELECT A.ID AS RANKID, A.RANKCODE, A.RANKNAME, A.RANKSEQ FROM T_S_RANK A
                                                         WHERE A.RANKTYPE = '1'
                                                         ORDER BY A.RANKSEQ");
            if (dtlist.Rows.Count > 0)
            {
                List<RankAccountInfo> roleList = new List<RankAccountInfo>();
                for (int i = 0; i < dtlist.Rows.Count; i++)
                {
                    RankAccountInfo m = new RankAccountInfo();
                    m.RANKID = dtlist.Rows[i]["RANKID"]?.ToString();
                    m.RANKCODE = dtlist.Rows[i]["RANKCODE"]?.ToString();
                    m.RANKNAME = dtlist.Rows[i]["RANKNAME"]?.ToString();
                    m.RANKSEQ = dtlist.Rows[i]["RANKSEQ"]?.ToString();
                    // 0:無權限 1:有權限
                    m.HASPOWER = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_ACRANKMAP WHERE USERCODE = '{code}' AND RANKID = {m.RANKID}")) ? "0" : "1";

                    roleList.Add(m);
                }

                return new { status = "0", item = roleList };
            }

            return new { status = "99", pList = "", item = "" };

        }

        //取得前後台選單資料
        public dynamic GetAllBacknFrontMenu()
        {
            DataTable dtlist = DBUtil.SelectDataTable($@"SELECT ID, MENUCODE, PARENTCODE, MENUURL, MENUDESC, MENUSEQ, MENUNAME,
                                                         ISBACK, ISVISIBLE, ISENABLE, CREATEBY AS CREBY, CREDATE, UPDDATE,UPDBY
                                                         FROM T_S_MENU ORDER BY MENUCODE, MENUSEQ");
            if (dtlist.Rows.Count > 0)
            {
                List<MenuInfo> bList = new List<MenuInfo>();
                List<MenuInfo> fList = new List<MenuInfo>();
                for (int i = 0; i < dtlist.Rows.Count; i++)
                {
                    MenuInfo m = new MenuInfo();
                    m.MENUID = dtlist.Rows[i]["ID"]?.ToString();
                    m.MENUCODE = dtlist.Rows[i]["MENUCODE"]?.ToString();
                    m.PARENTCODE = dtlist.Rows[i]["PARENTCODE"]?.ToString();
                    m.MENUNAME = dtlist.Rows[i]["MENUNAME"]?.ToString();
                    m.MENUDESC = dtlist.Rows[i]["MENUDESC"]?.ToString();
                    m.MENUURL = dtlist.Rows[i]["MENUURL"]?.ToString();
                    m.ISBACK = Convert.ToBoolean(dtlist.Rows[i]["ISBACK"]) == true ? "1" : "0";
                    m.ISVISIBLE = Convert.ToBoolean(dtlist.Rows[i]["ISVISIBLE"]) == true ? "1" : "0";
                    m.ISENABLE = Convert.ToBoolean(dtlist.Rows[i]["ISENABLE"]) == true ? "1" : "0";
                    m.CREDATE = dtlist.Rows[i]["CREDATE"]?.ToString();
                    m.CREBY = dtlist.Rows[i]["CREBY"]?.ToString();
                    m.UPDDATE = dtlist.Rows[i]["UPDDATE"]?.ToString();
                    m.UPDBY = dtlist.Rows[i]["UPDBY"]?.ToString();

                    if (m.ISBACK == "1")
                        bList.Add(m);
                    else
                        fList.Add(m);
                }

                return new { status = "0", backList = bList, frontList = fList };
            }

            return new { status = "99", backList = "", frontList = "" };
        }

        //取得前後台父選單項目
        public dynamic GetParentMenu(string isback)
        {
            DataTable dtlist = DBUtil.SelectDataTable($@"SELECT ID, MENUCODE, PARENTCODE, MENUURL, MENUDESC, MENUSEQ, MENUNAME,
                                                         ISBACK, ISVISIBLE, ISENABLE, CREATEBY AS CREBY, CREDATE, UPDDATE,UPDBY
                                                         FROM T_S_MENU
                                                         WHERE PARENTCODE = '0' AND ISBACK = " + (isback == "1" ? "'true'" : "'false'")
                                                         + "ORDER BY MENUCODE, MENUSEQ");
            if (dtlist.Rows.Count > 0)
            {
                List<MenuInfo> parentList = new List<MenuInfo>();
                for (int i = 0; i < dtlist.Rows.Count; i++)
                {
                    MenuInfo m = new MenuInfo();
                    m.MENUID = dtlist.Rows[i]["ID"]?.ToString();
                    m.MENUCODE = dtlist.Rows[i]["MENUCODE"]?.ToString();
                    m.PARENTCODE = dtlist.Rows[i]["PARENTCODE"]?.ToString();
                    m.MENUNAME = dtlist.Rows[i]["MENUNAME"]?.ToString();
                    m.MENUDESC = dtlist.Rows[i]["MENUDESC"]?.ToString();
                    m.MENUURL = dtlist.Rows[i]["MENUURL"]?.ToString();
                    m.ISBACK = Convert.ToBoolean(dtlist.Rows[i]["ISBACK"]) == true ? "1" : "0";
                    m.ISVISIBLE = Convert.ToBoolean(dtlist.Rows[i]["ISVISIBLE"]) == true ? "1" : "0";
                    m.ISENABLE = Convert.ToBoolean(dtlist.Rows[i]["ISENABLE"]) == true ? "1" : "0";
                    m.CREDATE = dtlist.Rows[i]["CREDATE"]?.ToString();
                    m.CREBY = dtlist.Rows[i]["CREBY"]?.ToString();
                    m.UPDDATE = dtlist.Rows[i]["UPDDATE"]?.ToString();
                    m.UPDBY = dtlist.Rows[i]["UPDBY"]?.ToString();

                    parentList.Add(m);
                }

                return new { status = "0", pList = parentList };
            }

            return new { status = "99", pList = "" };
        }

        /// <summary>
        /// POSITION: 頁面URL
        /// TARGET: 頁面名稱
        /// MESSAGE: 操作Log --> 1:新增, 2:修改, 3:刪除
        /// </summary>
        public void AddUserControlLog(Hashtable sData, string position, string target, int type, string msg = "")
        {
            Hashtable htData = new Hashtable();
            htData["USERCODE"] = sData["_usercode"];
            htData["USERNAME"] = sData["_username"];
            htData["POSITION"] = position;
            htData["TARGET"] = target;
            htData["MESSAGE"] = (type == 1 ? "新增:" : (type == 2 ? "修改:" : "刪除:")) + msg;
            htData["LOG_DATE"] = DateTime.Now;

            string sql = $@"INSERT INTO T_S_USERLOG(USERCODE, USERNAME, POSITION, TARGET, MESSAGE, LOG_DATE)
                            VALUES (@USERCODE, @USERNAME, @POSITION, @TARGET, @MESSAGE, @LOG_DATE)";

            DBUtil.EXECUTE(sql, htData);

        }

        public string GetMd5Hash(string argInput)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(argInput))).Replace("-", "");
            }
        }

    }    
    
}
