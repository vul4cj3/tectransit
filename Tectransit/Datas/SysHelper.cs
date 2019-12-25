using System;
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
                                            SELECT ROW_NUMBER() OVER (ORDER BY ROLESEQ) AS ROW_SEQ, ID, ROLECODE, ROLENAME, ROLEDESC,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE
                                            From T_S_ROLE) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                List<RoleInfo> roleList = new List<RoleInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    RoleInfo m = new RoleInfo();
                    m.ROLEID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.ROLECODE = DT.Rows[i]["ROLECODE"]?.ToString();
                    m.ROLENAME = DT.Rows[i]["ROLENAME"]?.ToString();
                    m.ROLEDESC = DT.Rows[i]["ROLEDESC"]?.ToString();
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.CREBY = DT.Rows[i]["CREBY"]?.ToString();
                    m.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();
                    m.UPDBY = DT.Rows[i]["UPDBY"]?.ToString();
                    m.ISENABLE = Convert.ToBoolean(DT.Rows[i]["ISENABLE"]) ? "1" : "0";

                    roleList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = roleList, total = totalCt };
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

    }

    public class RoleInfo
    {
        public long ROLEID { set; get; }
        public string ROLESEQ { set; get; }
        public string ROLECODE { set; get; }
        public string ROLENAME { set; get; }
        public string ROLEDESC { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
        public string ISENABLE { set; get; }
    }

}
