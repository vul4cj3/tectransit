﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Tectransit.Modles;
using System.Linq;

namespace Tectransit.Datas
{
    public class SysHelper
    {

        //取得權限資料(List)
        public dynamic GetRoleListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY ROLESEQ) AS ROW_ID, ID, ROLECODE, ROLENAME, ROLEDESC,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE
                                            From T_S_ROLE
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<RoleInfo> rowList = new List<RoleInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    RoleInfo m = new RoleInfo();
                    m.ROWID = Convert.ToInt64(DT.Rows[i]["ROW_ID"]);
                    m.ROLEID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.ROLECODE = DT.Rows[i]["ROLECODE"]?.ToString();
                    m.ROLENAME = DT.Rows[i]["ROLENAME"]?.ToString();
                    m.ROLEDESC = DT.Rows[i]["ROLEDESC"]?.ToString();
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.CREBY = DT.Rows[i]["CREBY"]?.ToString();
                    m.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();
                    m.UPDBY = DT.Rows[i]["UPDBY"]?.ToString();
                    m.ISENABLE = Convert.ToBoolean(DT.Rows[i]["ISENABLE"]) ? "1" : "0";

                    rowList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = rowList, total = totalCt };
            }

            return new { rows = "", total = 0 };
        }

        public dynamic GetRoleData(long sID)
        {
            string sql = $@"
                            SELECT ID, ROLECODE, ROLENAME, ROLEDESC, ROLESEQ,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY, ISENABLE
                            From T_S_ROLE
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                RoleInfo m = new RoleInfo();
                m.ROLEID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.ROLESEQ = DT.Rows[0]["ROLESEQ"]?.ToString();
                m.ROLECODE = DT.Rows[0]["ROLECODE"]?.ToString();
                m.ROLENAME = DT.Rows[0]["ROLENAME"]?.ToString();
                m.ROLEDESC = DT.Rows[0]["ROLEDESC"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();
                m.ISENABLE = Convert.ToBoolean(DT.Rows[0]["ISENABLE"]) ? "1" : "0";

                return new { rows = m };
            }

            return new { rows = "" };
        }

        //取得用戶資料(List)
        public dynamic GetUserListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY USERSEQ) AS ROW_ID, ID, USERCODE, USERNAME, USERDESC, EMAIL,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE
                                            From T_S_USER
                                            {sWhere}) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<UserInfo> rowList = new List<UserInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    UserInfo m = new UserInfo();
                    m.ROWID = Convert.ToInt64(DT.Rows[i]["ROW_ID"]);
                    m.USERID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.USERCODE = DT.Rows[i]["USERCODE"]?.ToString();
                    m.USERNAME = DT.Rows[i]["USERNAME"]?.ToString();
                    m.USERDESC = DT.Rows[i]["USERDESC"]?.ToString();
                    m.EMAIL = DT.Rows[i]["EMAIL"]?.ToString();
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.CREBY = DT.Rows[i]["CREBY"]?.ToString();
                    m.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();
                    m.UPDBY = DT.Rows[i]["UPDBY"]?.ToString();
                    m.ISENABLE = Convert.ToBoolean(DT.Rows[i]["ISENABLE"]) ? "1" : "0";

                    rowList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = rowList, total = totalCt };
            }

            return new { rows = "", total = 0 };
        }

        public dynamic GetUserData(long sID)
        {
            string sql = $@"
                            SELECT ID, USERCODE, USERNAME, USERDESC, USERSEQ, EMAIL,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY, ISENABLE
                            From T_S_USER
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                UserInfo m = new UserInfo();
                m.USERID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.USERSEQ = DT.Rows[0]["USERSEQ"]?.ToString();
                m.USERCODE = DT.Rows[0]["USERCODE"]?.ToString();
                m.USERNAME = DT.Rows[0]["USERNAME"]?.ToString();
                m.USERDESC = DT.Rows[0]["USERDESC"]?.ToString();
                m.EMAIL = DT.Rows[0]["EMAIL"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();
                m.ISENABLE = Convert.ToBoolean(DT.Rows[0]["ISENABLE"]) ? "1" : "0";

                return new { rows = m };
            }

            return new { rows = "" };
        }

        public dynamic GetUserLogListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY ID DESC) AS ROW_ID, ID, USERCODE, USERNAME, POSITION, TARGET, MESSAGE,
                                                   FORMAT(LOG_DATE, 'yyyy-MM-dd HH:mm:ss') As LOG_DATE
                                            From T_S_USERLOG
                                            {sWhere}) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<UserLogInfo> rowList = new List<UserLogInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    UserLogInfo m = new UserLogInfo();
                    m.ROWID = Convert.ToInt64(DT.Rows[i]["ROW_ID"]);
                    m.USERCODE = DT.Rows[i]["USERCODE"]?.ToString();
                    m.USERNAME = DT.Rows[i]["USERNAME"]?.ToString();
                    m.POSITION = DT.Rows[i]["POSITION"]?.ToString();
                    m.TARGET = DT.Rows[i]["TARGET"]?.ToString();
                    m.MESSAGE = DT.Rows[i]["MESSAGE"]?.ToString();
                    m.LOGDATE = DT.Rows[i]["LOG_DATE"]?.ToString();

                    rowList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = rowList, total = totalCt };
            }

            return new { rows = "", total = 0 };
        }

        public dynamic GetMenuData(long sID)
        {
            string sql = $@"
                            SELECT ID, MENUCODE, PARENTCODE, MENUURL, MENUDESC, MENUSEQ, MENUNAME, ICONURL,
                                   ISBACK, ISVISIBLE, ISENABLE, CREATEBY AS CREBY, CREDATE, UPDDATE,UPDBY
                            FROM T_S_MENU
                            WHERE ID = {sID}";

            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                MenuInfo m = new MenuInfo();
                m.MENUID = DT.Rows[0]["ID"]?.ToString();
                m.PARENTCODE = DT.Rows[0]["PARENTCODE"]?.ToString();
                m.MENUSEQ = DT.Rows[0]["MENUSEQ"]?.ToString();
                m.MENUCODE = DT.Rows[0]["MENUCODE"]?.ToString();
                m.MENUNAME = DT.Rows[0]["MENUNAME"]?.ToString();
                m.MENUDESC = DT.Rows[0]["MENUDESC"]?.ToString();
                m.MENUURL = DT.Rows[0]["MENUURL"]?.ToString();
                m.ICONURL = DT.Rows[0]["ICONURL"]?.ToString();
                m.ISBACK = Convert.ToBoolean(DT.Rows[0]["ISBACK"]) == true ? "1" : "0";
                m.ISVISIBLE = Convert.ToBoolean(DT.Rows[0]["ISVISIBLE"]) == true ? "1" : "0";
                m.ISENABLE = Convert.ToBoolean(DT.Rows[0]["ISENABLE"]) == true ? "1" : "0";
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();
                m.ISENABLE = Convert.ToBoolean(DT.Rows[0]["ISENABLE"]) ? "1" : "0";

                return new { rows = m };
            }

            return new { rows = "" };
        }

    }    

}
