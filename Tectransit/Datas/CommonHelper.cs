﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
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

        //取得選單資料(後台or前台)
        public dynamic GetAllMenu(string code, string isback)
        {
            DataTable dtMenulist = DBUtil.SelectDataTable($@"SELECT A.MENUSEQ, A.MENUCODE, A.PARENTCODE,A.MENUURL, A.MENUNAME, A.ICONURL FROM T_S_MENU A
                                                             WHERE A.ISBACK = '{isback}' AND ISENABLE = 'true'
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
                    if (isback == "true")
                        m.HASPOWER = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT MENUCODE AS COL1 FROM T_S_ROLEMENUMAP WHERE ROLECODE = '{code}' AND MENUCODE = '{m.MENUCODE}'")) ? "0" : "1";
                    else
                        m.HASPOWER = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT MENUCODE AS COL1 FROM T_S_RANKMENUMAP WHERE RANKID = {code} AND MENUCODE = '{m.MENUCODE}'")) ? "0" : "1";

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
        public dynamic GetAllRole(string code, string sWhere = "")
        {
            DataTable dtlist = DBUtil.SelectDataTable($@"SELECT A.ID AS ROLEID, A.ROLECODE, A.ROLENAME, A.ROLESEQ FROM T_S_ROLE A
                                                         {sWhere}
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

            return new { status = "99", item = "" };

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

            return new { status = "99", item = "" };

        }

        //取得前台廠商用戶權限組
        public dynamic GetAllCusRank(string code)
        {
            DataTable dtlist = DBUtil.SelectDataTable($@"SELECT A.ID AS RANKID, A.RANKCODE, A.RANKNAME, A.RANKSEQ FROM T_S_RANK A
                                                         WHERE A.RANKTYPE != '1'
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

            return new { status = "99", item = "" };

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

        //取得所有Banner
        public dynamic GetAllBanner()
        {
            DataTable dtlist = DBUtil.SelectDataTable($@"SELECT ID, TITLE, DESCR, IMGURL, URL, BANSEQ, UPSDATE, UPEDATE,
                                                                ISTOP, ISENABLE, FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') AS CREDATE,
                                                                FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') AS UPDDATE, CREATEBY AS CREBY, UPDBY
                                                         FROM T_D_BANNER
                                                         ORDER BY BANSEQ");
            if (dtlist.Rows.Count > 0)
            {
                List<BannerInfo> bList = new List<BannerInfo>();
                for (int i = 0; i < dtlist.Rows.Count; i++)
                {
                    BannerInfo m = new BannerInfo();
                    m.BANID = Convert.ToInt64(dtlist.Rows[i]["ID"]);
                    m.TITLE = dtlist.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = dtlist.Rows[i]["DESCR"]?.ToString();
                    m.IMGURL = dtlist.Rows[i]["IMGURL"]?.ToString();
                    m.URL = dtlist.Rows[i]["URL"]?.ToString();
                    m.BANSEQ = dtlist.Rows[i]["BANSEQ"]?.ToString();
                    m.UPSDATE = dtlist.Rows[i]["UPSDATE"]?.ToString();
                    m.UPEDATE = dtlist.Rows[i]["UPEDATE"]?.ToString();
                    m.ISTOP = Convert.ToBoolean(dtlist.Rows[i]["ISTOP"]) == true ? "1" : "0";
                    m.ISENABLE = Convert.ToBoolean(dtlist.Rows[i]["ISENABLE"]) == true ? "1" : "0";
                    m.CREDATE = dtlist.Rows[i]["CREDATE"]?.ToString();
                    m.CREBY = dtlist.Rows[i]["CREBY"]?.ToString();
                    m.UPDDATE = dtlist.Rows[i]["UPDDATE"]?.ToString();
                    m.UPDBY = dtlist.Rows[i]["UPDBY"]?.ToString();

                    bList.Add(m);
                    
                }

                return new { status = "0", dataList = bList };
            }

            return new { status = "99", backList = "", frontList = "" };
        }

        //前台banner
        public dynamic GetBanner()
        {
            DataTable dtlist = DBUtil.SelectDataTable($@"SELECT ID, TITLE, DESCR, IMGURL, URL, 
	                                                            BANSEQ, UPSDATE, UPEDATE, ISTOP
                                                         FROM T_D_BANNER
                                                         WHERE ISENABLE = 'true'
                                                         ORDER BY BANSEQ");
            if (dtlist.Rows.Count > 0)
            {
                List<BannerInfo> bList = new List<BannerInfo>();
                for (int i = 0; i < dtlist.Rows.Count; i++)
                {
                    BannerInfo m = new BannerInfo();
                    m.BANID = Convert.ToInt64(dtlist.Rows[i]["ID"]);
                    m.TITLE = dtlist.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = dtlist.Rows[i]["DESCR"]?.ToString();
                    m.IMGURL = dtlist.Rows[i]["IMGURL"]?.ToString();
                    m.URL = dtlist.Rows[i]["URL"]?.ToString();
                    m.BANSEQ = dtlist.Rows[i]["BANSEQ"]?.ToString();
                    m.UPSDATE = dtlist.Rows[i]["UPSDATE"]?.ToString();
                    m.UPEDATE = dtlist.Rows[i]["UPEDATE"]?.ToString();
                    m.ISTOP = Convert.ToBoolean(dtlist.Rows[i]["ISTOP"]) == true ? "1" : "0";

                    bList.Add(m);

                }

                return new { status = "0", dataList = bList };
            }

            return new { status = "99", backList = "", frontList = "" };
        }

        public dynamic GetStationData()
        {
            string sql = $@"
                            SELECT ID, STATIONCODE, STATIONNAME, COUNTRYCODE, RECEIVER, PHONE, MOBILE, ADDRESS
                            From T_S_STATION";
            DataTable DT = DBUtil.SelectDataTable(sql);
            List<MemStationInfo> rowlist = new List<MemStationInfo>();
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    MemStationInfo m = new MemStationInfo();
                    m.STATIONID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.STATIONCODE = DT.Rows[i]["STATIONCODE"]?.ToString();
                    m.STATIONNAME = DT.Rows[i]["STATIONNAME"]?.ToString();
                    m.COUNTRYCODE = DT.Rows[i]["COUNTRYCODE"]?.ToString();
                    m.RECEIVER = DT.Rows[i]["RECEIVER"]?.ToString();
                    m.PHONE = DT.Rows[i]["PHONE"]?.ToString();
                    m.MOBILE = DT.Rows[i]["MOBILE"]?.ToString();
                    m.ADDRESS = DT.Rows[i]["ADDRESS"]?.ToString();

                    rowlist.Add(m);
                }

                return new { rows = rowlist };
            }

            return new { rows = "" };
        }

        //取得前台選單
        public dynamic GetMenu_Fornt(long id)
        {
            DataTable dtMenulist = DBUtil.SelectDataTable($@"SELECT A.ID AS MENUID, A.MENUCODE, A.PARENTCODE, A.MENUURL, A.MENUSEQ, A.MENUNAME
                                                             FROM T_S_MENU A
                                                             LEFT JOIN T_S_RANKMENUMAP B ON A.MENUCODE = B.MENUCODE
                                                             WHERE A.ISBACK = 'false' AND A.ISVISIBLE = 'true' AND A.ISENABLE = 'true' AND B.RANKID = {id}
                                                             ORDER BY MENUSEQ");
            if (dtMenulist.Rows.Count > 0)
            {
                List<MenuInfo_F> pmenuList = new List<MenuInfo_F>();
                List<MenuInfo_F> dmenuList = new List<MenuInfo_F>();                
                for (int i = 0; i < dtMenulist.Rows.Count; i++)
                {
                    MenuInfo_F m = new MenuInfo_F();
                    m.MENUID = Convert.ToInt64(dtMenulist.Rows[i]["MENUID"]);
                    m.MENUCODE = dtMenulist.Rows[i]["MENUCODE"]?.ToString();
                    m.PARENTCODE = dtMenulist.Rows[i]["PARENTCODE"]?.ToString();
                    m.MENUURL = dtMenulist.Rows[i]["MENUURL"]?.ToString();
                    m.MENUNAME = dtMenulist.Rows[i]["MENUNAME"]?.ToString();
                    m.MENUSEQ = Convert.ToInt32(dtMenulist.Rows[i]["MENUSEQ"]);

                    //if (m.PARENTCODE == "0")
                        pmenuList.Add(m);
                    //else
                    //    dmenuList.Add(m);
                }

                DataTable dtAboutlist = DBUtil.SelectDataTable($@"SELECT ID AS CATEID, TITLE
                                                                  FROM T_D_ABOUT_H
                                                                  WHERE ISENABLE = 'true'
                                                                  ORDER BY ISTOP, ABOUTHSEQ");

                List<AboutCate> aboutList = new List<AboutCate>();
                if (dtAboutlist.Rows.Count > 0)
                {
                    for (int j = 0; j < dtAboutlist.Rows.Count; j++)
                    {
                        AboutCate m = new AboutCate();
                        m.CATEID = Convert.ToInt64(dtAboutlist.Rows[j]["CATEID"]);
                        m.TITLE = dtAboutlist.Rows[j]["TITLE"]?.ToString();

                        aboutList.Add(m);
                    }
                }

                return new { status = "0", pList = pmenuList, item = dmenuList, aboutitem = aboutList };
            }

            return new { status = "99", pList = "", item = "", aboutitem = "" };

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

        public void InsertSeqCode(Hashtable sData, string type)
        {
            if (type == "station") //各集運站追蹤代碼-流水號
            {
                Hashtable tempData = new Hashtable();
                tempData["STARTCODE"] = "000000";
                tempData["ENDCODE"] = "999999";
                tempData["FIRSTCODE"] = "*";
                tempData["NEXTCODE"] = "000000";
                tempData["CODENAME"] = sData["STATIONCODE"];
                tempData["CODENAME2"] = sData["STATIONCODE"] +"_CUS";
                tempData["CODEDESC"] = sData["STATIONNAME"] +"(會員)";
                tempData["CODEDESC2"] = sData["STATIONNAME"] +"(廠商)";
                tempData["CREDATE"] = DateTime.Now;
                tempData["CREATEBY"] = "SYSTEM";
                tempData["UPDDATE"] = tempData["CREDATE"];
                tempData["UPDBY"] = tempData["CREATEBY"];

                string sql = $@"INSERT INTO T_S_SEQUENCECODE(STARTCODE, ENDCODE, FIRSTCODE, NEXTCODE, CODENAME, CODEDESC, CREDATE, CREATEBY, UPDDATE, UPDBY)
                            VALUES (@STARTCODE, @ENDCODE, @FIRSTCODE, @NEXTCODE, @CODENAME, @CODEDESC, @CREDATE, @CREATEBY, @UPDDATE, @UPDBY)";

                string sql1 = $@"INSERT INTO T_S_SEQUENCECODE(STARTCODE, ENDCODE, FIRSTCODE, NEXTCODE, CODENAME, CODEDESC, CREDATE, CREATEBY, UPDDATE, UPDBY)
                            VALUES (@STARTCODE, @ENDCODE, @FIRSTCODE, @NEXTCODE, @CODENAME2, @CODEDESC2, @CREDATE, @CREATEBY, @UPDDATE, @UPDBY)";

                DBUtil.EXECUTE(sql, tempData);//會員用
                DBUtil.EXECUTE(sql1, tempData);//廠商用
            }
            else { }
        }

        
        public string GetSeqCode(string type)
        {
            if (type.IndexOf("WAREHOUSENO") < 0)
            {
                string fcode = DBUtil.GetSingleValue1($@"SELECT FIRSTCODE AS COL1 FROM T_S_SEQUENCECODE WHERE CODENAME = '{type}'");
                string num = DBUtil.GetSingleValue1($@"SELECT NEXTCODE AS COL1 FROM T_S_SEQUENCECODE WHERE CODENAME = '{type}'");

                //字母編組用
                if (fcode != "*")
                    num = fcode + num;

                return num;
            }
            else
            {
                string num = DBUtil.GetSingleValue1($@"SELECT NEXTCODE AS COL1 FROM T_S_SEQUENCECODE WHERE CODENAME = '{type}'");

                return num;
            }
        }

        public void UpdateSeqCode(string type)
        {
            string num = DBUtil.GetSingleValue1($@"SELECT NEXTCODE AS COL1 FROM T_S_SEQUENCECODE WHERE CODENAME = '{type}'");
            string endcode = DBUtil.GetSingleValue1($@"SELECT ENDCODE AS COL1 FROM T_S_SEQUENCECODE WHERE CODENAME = '{type}'");

            if (!string.IsNullOrEmpty(num))
            {
                long tempCode = Convert.ToInt64(num);
                tempCode++;

                if (type.IndexOf("WAREHOUSENO") < 0)
                    num = tempCode.ToString().PadLeft(endcode.Length, '0');
                else
                    num = tempCode.ToString();

                DBUtil.EXECUTE($@"UPDATE T_S_SEQUENCECODE SET NEXTCODE = '{num}' WHERE CODENAME = '{type}'");

                //檢查code是否已用完
                CheckSeqCode(type, tempCode);
            }

        }

        public void CheckSeqCode(string type, long code)
        {
            string num = DBUtil.GetSingleValue1($@"SELECT ENDCODE AS COL1 FROM T_S_SEQUENCECODE WHERE CODENAME = '{type}'");
            string firstcode = DBUtil.GetSingleValue1($@"SELECT FIRSTCODE AS COL1 FROM T_S_SEQUENCECODE WHERE CODENAME = '{type}'");
            string newcode = "";
            string newstartcode = "";
            string newendcode = "";

            //超過範圍則重新編號
            if (code > Convert.ToInt64(num))
            {
                string[] Alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                                   "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                                   "U", "V", "W", "X", "Y", "Z"};

                if (firstcode == "*")
                {
                    newcode = Alpha[0];
                    newstartcode = "00000";
                    newendcode = "99999";
                }
                else
                {
                    if (firstcode.Length == 3)
                    {
                        for (int i = 0; i < Alpha.Length; i++)
                        {
                            if (firstcode.Substring(0, 1) == Alpha[i])
                                newcode = Alpha[i + 1];
                        }
                    }
                    else
                        newcode = firstcode + firstcode.Substring(0, 1);

                    
                    if (newcode.Length == 1)
                    {
                        newstartcode = "00000";
                        newendcode = "99999";
                    }
                    else if (newcode.Length == 2)
                    {
                        newstartcode = "0000";
                        newendcode = "9999";
                    }
                    else if (newcode.Length == 3)
                    {
                        newstartcode = "000";
                        newendcode = "999";
                    }
                    else { }
                }

                DBUtil.EXECUTE($@"UPDATE T_S_SEQUENCECODE SET FIRSTCODE = '{newcode}', STARTCODE = '{newstartcode}', ENDCODE = '{newendcode}', NEXTCODE = '{newstartcode}' WHERE CODENAME = '{type}'");

            }
        }

        public void DeleteSingleTableData(string table, string column, string value)
        {
            string sql = $@"DELETE FROM {table} WHERE {column} = {value}";
            DBUtil.EXECUTE(sql);
        }

        //刪除後台實體檔案
        public void DeleteFileB(string file)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "admin/dist/admin" + file);
            if (File.Exists(filepath))
                File.Delete(filepath);
            
        }

        //刪除前台實體檔案
        public void DeleteFileF(string file)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "TecTransit/dist/TecTransit" + file);
            if (File.Exists(filepath))
                File.Delete(filepath);

        }

        public string GetRandomString()
        {
            string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            int passwordLength = 5;//密碼長度
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                //allowedChars -> 這個String ，隨機取得一個字，丟給chars[i]
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            string pwd = new string(chars);
            return pwd;
        }

        public string GetRandomStringNumber(int length)
        {
            string ranlist = "abcdefghijklmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = ranlist[rd.Next(0, ranlist.Length)];
            }

            string num = new string(chars);

            return num;
        }

        public string GetRandomNumber(int length)
        {
            string ranlist = "0123456789";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = ranlist[rd.Next(0, ranlist.Length)];
            }

            string num = new string(chars);

            return num;
        }

        public void SendMail(string FromName, string ToUser, string Mailsubject, string Body, string ccUser)
        {
            string myMailEncoding = "utf-8";

            string myFromEmail = "tomato@t3ex-group.com";
            string myFromName = FromName;
            MailAddress from = new MailAddress(myFromEmail, myFromName, Encoding.GetEncoding(myMailEncoding));

            //string myToEmail = "";
            //string myToName = "";
            //MailAddress to = new MailAddress(myToEmail, myToName, Encoding.GetEncoding(myMailEncoding));

            Body += @"<br/><br/><br/><br/><br/>=================================================<br/>此為系統自動發送通知<br/>請勿直接回信";

            try
            {
                SmtpClient sc = new SmtpClient("mail.t3ex-group.com");
                sc.UseDefaultCredentials = false;
                sc.Port = 587;
                sc.Credentials = new NetworkCredential("tomato", "1qaz2WSX");
                MailMessage myMessage = new MailMessage();
                myMessage.From = from;

                string[] UserList = ToUser.Split(';');
                for (int i = 0; i < UserList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(UserList[i]))
                    {
                        MailAddress to = new MailAddress(UserList[i], "", Encoding.GetEncoding(myMailEncoding));
                        myMessage.To.Add(to);
                    }
                }

                string[] CCUserList = ccUser.Split(';');
                for (int i = 0; i < CCUserList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(CCUserList[i])) {
                        MailAddress CC = new MailAddress(CCUserList[i], "", Encoding.GetEncoding(myMailEncoding));
                        myMessage.CC.Add(CC);
                    }
                }
                myMessage.Subject = Mailsubject;
                myMessage.SubjectEncoding = Encoding.GetEncoding(myMailEncoding);
                myMessage.Body = Body;
                myMessage.BodyEncoding = Encoding.GetEncoding(myMailEncoding);
                myMessage.IsBodyHtml = true;
                myMessage.Priority = MailPriority.High;
                
                sc.Send(myMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendMasterMail(string FromName, string Mailsubject, string Body)
        {
            string myMailEncoding = "utf-8";

            string myFromEmail = "tomato@t3ex-group.com";
            string myFromName = FromName;
            MailAddress from = new MailAddress(myFromEmail, myFromName, Encoding.GetEncoding(myMailEncoding));

            //string myToEmail = "";
            //string myToName = "";
            //MailAddress to = new MailAddress(myToEmail, myToName, Encoding.GetEncoding(myMailEncoding));

            Body += @"<br/><br/><br/><br/><br/>=================================================<br/>此為系統自動發送通知<br/>請勿直接回信";

            try
            {
                SmtpClient sc = new SmtpClient("mail.t3ex-group.com");
                sc.UseDefaultCredentials = false;
                sc.Port = 587;
                sc.Credentials = new NetworkCredential("tomato", "1qaz2WSX");
                MailMessage myMessage = new MailMessage();
                myMessage.From = from;

                //寄給TECTPE_MG權限組
                string sql = $@"SELECT A.USERCODE, A.USERNAME, A.EMAIL FROM T_S_USER A
                            LEFT JOIN T_S_USERROLEMAP B ON A.USERCODE = B.USERCODE
                            LEFT JOIN T_S_ROLE C ON C.ROLECODE = B.ROLECODE
                            WHERE C.ROLECODE = 'TECTPE_MG' AND A.ISENABLE = 'true'";

                DataTable DT = DBUtil.SelectDataTable(sql);
                //加入信件的收件人address
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(DT.Rows[i]["EMAIL"]?.ToString()))
                        {
                            MailAddress to = new MailAddress(DT.Rows[i]["EMAIL"]?.ToString(), DT.Rows[i]["USERNAME"]?.ToString(), Encoding.GetEncoding(myMailEncoding));
                            myMessage.To.Add(to);
                        }
                    }
                }
                
                myMessage.Subject = Mailsubject;
                myMessage.SubjectEncoding = Encoding.GetEncoding(myMailEncoding);
                myMessage.Body = Body;
                myMessage.BodyEncoding = Encoding.GetEncoding(myMailEncoding);
                myMessage.IsBodyHtml = true;
                myMessage.Priority = MailPriority.High;

                sc.Send(myMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 寄信程式(Old)
        //public void OldSendMail(string fromUser, string ToUser, string Mailsubject, string Mailbody, string ccUser)
        //{
        //    #region  send mail
        //    string smtpServer = "mail.t3ex-group.com";
        //    int smtpPort = 25;
        //    string mailAccount = "tomato";
        //    string mailPwd = "1qaz2WSX";

        //    //建立MailMessage物件
        //    System.Net.Mail.MailMessage mms = new System.Net.Mail.MailMessage();
        //    //指定一位寄信人MailAddress
        //    mms.From = new MailAddress(fromUser);
        //    //信件主旨
        //    mms.Subject = Mailsubject;
        //    //信件內容
        //    mms.Body = Mailbody;
        //    //信件內容 是否採用Html格式
        //    mms.IsBodyHtml = true;

        //    //加入信件的收件人address
        //    mms.To.Add(ToUser);
        //    //加入信件的副本address
        //    mms.CC.Add(ccUser);
        //    //備存
        //    //mms.CC.Add(new MailAddress("ebs.sys@t3ex-group.com"));

        //    using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))//或公司、客戶的smtp_server
        //    {
        //        if (!string.IsNullOrEmpty(mailAccount) && !string.IsNullOrEmpty(mailPwd))//.config有帳密的話
        //        {
        //            client.Credentials = new NetworkCredential(mailAccount, mailPwd);//寄信帳密
        //        }
        //        client.Send(mms);//寄出一封信
        //    }//end using 
        //    #endregion
        //}

        ////寄給客服信箱
        //public void OldSendMasterMail(string fromUser, string Mailsubject, string Mailbody, string ccUser)
        //{
        //    #region  send mail
        //    string smtpServer = "mail.t3ex-group.com";
        //    int smtpPort = 25;
        //    string mailAccount = "tomato";
        //    string mailPwd = "1qaz2WSX";

        //    //建立MailMessage物件
        //    System.Net.Mail.MailMessage mms = new System.Net.Mail.MailMessage();
        //    //指定一位寄信人MailAddress
        //    mms.From = new MailAddress(fromUser);
        //    //信件主旨
        //    mms.Subject = Mailsubject;
        //    //信件內容
        //    mms.Body = Mailbody;
        //    //信件內容 是否採用Html格式
        //    mms.IsBodyHtml = true;

        //    //寄給TECTPE_MG權限組
        //    string sql = $@"SELECT A.USERCODE, A.EMAIL FROM T_S_USER A
        //                    LEFT JOIN T_S_USERROLEMAP B ON A.USERCODE = B.USERCODE
        //                    LEFT JOIN T_S_ROLE C ON C.ROLECODE = B.ROLECODE
        //                    WHERE C.ROLECODE = 'TECTPE_MG' AND A.ISENABLE = 'true'";

        //    DataTable DT = DBUtil.SelectDataTable(sql);
        //    //加入信件的收件人address
        //    if (DT.Rows.Count > 0)
        //    {
        //        for(int i = 0; i < DT.Rows.Count; i++)
        //        {
        //            if (!string.IsNullOrEmpty(DT.Rows[i]["EMAIL"]?.ToString()))
        //                mms.To.Add(DT.Rows[i]["EMAIL"]?.ToString());
        //        }
        //    }            
        //    //加入信件的副本address
        //    //if (!string.IsNullOrEmpty(ccUser))
        //    //{
        //    //    string[] CcList = ccUser.Split(';');
        //    //    for (int i = 0; i < CcList.Length; i++)
        //    //    {
        //    //        mms.CC.Add(CcList[i]);
        //    //    }
        //    //}
        //    //備存
        //    //mms.CC.Add(new MailAddress("ebs.sys@t3ex-group.com"));

        //    using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))//或公司、客戶的smtp_server
        //    {
        //        if (!string.IsNullOrEmpty(mailAccount) && !string.IsNullOrEmpty(mailPwd))//.config有帳密的話
        //        {
        //            client.Credentials = new NetworkCredential(mailAccount, mailPwd);//寄信帳密
        //        }
        //        client.Send(mms);//寄出一封信
        //    }//end using 
        //    #endregion
        //}
        #endregion

        //寫入拋轉厚生倉API紀錄
        /// <summary>
        /// [type] = 1:個人用戶/2:廠商
        /// [shippingno] = 個人用戶: 快遞單號?/廠商: 集運單號
        /// </summary>
        /// <returns></returns>
        public void InsertDepotRecord(int type, string shippingno)
        {
            Hashtable sData = new Hashtable();
            sData["TYPE"] = type;
            sData["ACTIVE"] = 0; //0:未拋轉/1:拋轉成功/2:拋轉失敗
            sData["SHIPPINGNO"] = shippingno;
            sData["CREDATE"] = DateTime.Now;
            sData["UPDDATE"] = sData["CREDATE"];

            string sql = $@"INSERT INTO T_S_DEPOTRECORD(TYPE, ACTIVE, SHIPPINGNO, CREDATE, UPDDATE)
                            VALUES (@TYPE, @ACTIVE, @SHIPPINGNO, @CREDATE, @UPDDATE)";

            DBUtil.EXECUTE(sql, sData);            
        }

        //寫入拋轉到台空貨況API的紀錄
        /// <summary>
        /// [type] = 1:個人用戶/2:廠商
        /// [shippingno] = 個人用戶: 快遞單號?/廠商: 集運單號
        /// </summary>
        /// <returns></returns>
        public void InsertTectrackRecord(int type, string shippingno)
        {
            Hashtable sData = new Hashtable();
            sData["TYPE"] = type;
            sData["ACTIVE"] = 0; //0:未拋轉/1:拋轉成功/2:拋轉失敗/3:其他
            sData["SHIPPINGNO"] = shippingno;
            sData["CREDATE"] = DateTime.Now;
            sData["UPDDATE"] = sData["CREDATE"];

            string sql = $@"INSERT INTO T_S_TECTRACKRECORD(TYPE, ACTIVE, SHIPPINGNO, CREDATE, UPDDATE)
                            VALUES (@TYPE, @ACTIVE, @SHIPPINGNO, @CREDATE, @UPDDATE)";

            DBUtil.EXECUTE(sql, sData);
        }

    }

}
