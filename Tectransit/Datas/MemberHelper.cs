﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Tectransit.Datas
{
    public class MemberHelper
    {
        public dynamic GetMemData(Hashtable sData)
        {
            string sql = $@"
                            SELECT ID, USERCODE, WAREHOUSENO, USERNAME, USERDESC, USERSEQ, EMAIL, IDPHOTO_F, IDPHOTO_B, PHONE, MOBILE,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY, ISENABLE, LASTLOGINDATE, LOGINCOUNT, ADDR, TAXID
                            From T_S_ACCOUNT
                            WHERE USERCODE = '{sData["_acccode"]}'";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                AccountInfo m = new AccountInfo();
                m.USERID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.WAREHOUSENO = DT.Rows[0]["WAREHOUSENO"]?.ToString();
                m.USERSEQ = DT.Rows[0]["USERSEQ"]?.ToString();
                m.USERCODE = DT.Rows[0]["USERCODE"]?.ToString();
                m.USERNAME = DT.Rows[0]["USERNAME"]?.ToString();
                m.USERDESC = DT.Rows[0]["USERDESC"]?.ToString();
                m.EMAIL = DT.Rows[0]["EMAIL"]?.ToString();
                m.TAXID = DT.Rows[0]["TAXID"]?.ToString();
                m.IDPHOTO_F = DT.Rows[0]["IDPHOTO_F"]?.ToString();
                m.IDPHOTO_B = DT.Rows[0]["IDPHOTO_B"]?.ToString();
                m.PHONE = DT.Rows[0]["PHONE"]?.ToString();
                m.MOBILE = DT.Rows[0]["MOBILE"]?.ToString();
                m.ADDRESS = DT.Rows[0]["ADDR"]?.ToString();
                m.LASTLOGINDATE = DT.Rows[0]["LASTLOGINDATE"]?.ToString();
                m.LOGINCOUNT = DT.Rows[0]["LOGINCOUNT"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();
                m.ISENABLE = Convert.ToBoolean(DT.Rows[0]["ISENABLE"]) ? "1" : "0";

                return new { rows = m };
            }

            return new { rows = "" };
        }

        public dynamic GetACStationData(Hashtable sData)
        {
            string sql = $@"
                            SELECT ID, STATIONCODE, STATIONNAME, COUNTRYCODE, RECEIVER, PHONE, MOBILE, ADDRESS
                            From T_S_STATION";
            DataTable DT = DBUtil.SelectDataTable(sql);
            string ACName = DBUtil.GetSingleValue1($@"SELECT USERNAME AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{sData["_acccode"]}'");
            string WNO = DBUtil.GetSingleValue1($@"SELECT WAREHOUSENO AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{sData["_acccode"]}'");
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
                    m.USERNAME = ACName;
                    m.WAREHOUSENO = WNO;

                    rowlist.Add(m);
                }

                return new { rows = rowlist };
            }

            return new { rows = "" };
        }


    }
}