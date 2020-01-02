using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Tectransit.Datas
{
    public class UserManagementHelper
    {
        public dynamic GetRankListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                             SELECT ROW_NUMBER() OVER (ORDER BY RANKSEQ) AS ROW_ID, ID, RANKCODE, RANKTYPE, RANKNAME, RANKDESC,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE
                                            From T_S_RANK
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<RankInfo> rowList = new List<RankInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    RankInfo m = new RankInfo();
                    m.ROWID = Convert.ToInt64(DT.Rows[i]["ROW_ID"]);
                    m.RANKID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.RANKCODE = DT.Rows[i]["RANKCODE"]?.ToString();
                    m.RANKNAME = DT.Rows[i]["RANKNAME"]?.ToString();
                    m.RANKDESC = DT.Rows[i]["RANKDESC"]?.ToString();
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

        public dynamic GetRankData(long sID)
        {
            string sql = $@"
                            SELECT ID, RANKCODE, RANKTYPE, RANKNAME, RANKDESC, RANKSEQ,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY, ISENABLE
                            From T_S_RANK
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                RankInfo m = new RankInfo();
                m.RANKID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.RANKSEQ = DT.Rows[0]["RANKSEQ"]?.ToString();
                m.RANKCODE = DT.Rows[0]["RANKCODE"]?.ToString();
                m.RANKNAME = DT.Rows[0]["RANKNAME"]?.ToString();
                m.RANKDESC = DT.Rows[0]["RANKDESC"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();
                m.ISENABLE = Convert.ToBoolean(DT.Rows[0]["ISENABLE"]) ? "1" : "0";

                return new { rows = m };
            }

            return new { rows = "" };
        }

        public dynamic GetAccountListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY A.USERSEQ) AS ROW_ID, A.ID, A.USERCODE, A.USERNAME, A.USERDESC, A.EMAIL,
                                                   FORMAT(A.CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(A.LASTLOGINDATE, 'yyyy-MM-dd HH:mm:ss') As LASTLOGINDATE,
                                                   A.LOGINCOUNT, A.ISENABLE
                                            From T_S_ACCOUNT A
                                            LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE
                                            LEFT JOIN T_S_RANK C ON B.RANKID = C.ID
                                            {sWhere}) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<AccountInfo> rowList = new List<AccountInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    AccountInfo m = new AccountInfo();
                    m.ROWID = Convert.ToInt64(DT.Rows[i]["ROW_ID"]);
                    m.USERID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.USERCODE = DT.Rows[i]["USERCODE"]?.ToString();
                    m.USERNAME = DT.Rows[i]["USERNAME"]?.ToString();
                    m.USERDESC = DT.Rows[i]["USERDESC"]?.ToString();
                    m.EMAIL = DT.Rows[i]["EMAIL"]?.ToString();
                    m.LASTLOGINDATE = DT.Rows[i]["LASTLOGINDATE"]?.ToString();
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.LOGINCOUNT = DT.Rows[i]["LOGINCOUNT"]?.ToString();
                    m.ISENABLE = Convert.ToBoolean(DT.Rows[i]["ISENABLE"]) ? "1" : "0";

                    rowList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = rowList, total = totalCt };
            }

            return new { rows = "", total = 0 };
        }

        public dynamic GetAccountData(long sID)
        {
            string sql = $@"
                            SELECT ID, USERCODE, WAREHOUSENO, USERNAME, USERDESC, USERSEQ, EMAIL, IDPHOTO_F, IDPHOTO_B, PHONE, MOBILE,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY, ISENABLE, LASTLOGINDATE, LOGINCOUNT, ADDR, TAXID
                            From T_S_ACCOUNT
                            WHERE ID = {sID}";
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
                m.ADDR = DT.Rows[0]["ADDR"]?.ToString();
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

        public dynamic GetCompanyListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY A.USERSEQ) AS ROW_ID, A.ID, A.USERCODE, A.USERNAME, A.USERDESC, A.EMAIL,
                                                   FORMAT(A.CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(A.LASTLOGINDATE, 'yyyy-MM-dd HH:mm:ss') As LASTLOGINDATE,
                                                   A.LOGINCOUNT, A.ISENABLE
                                            From T_S_ACCOUNT A
                                            LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE
                                            LEFT JOIN T_S_RANK C ON B.RANKID = C.ID
                                            {sWhere}) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<AccountInfo> rowList = new List<AccountInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    AccountInfo m = new AccountInfo();
                    m.ROWID = Convert.ToInt64(DT.Rows[i]["ROW_ID"]);
                    m.USERID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.USERCODE = DT.Rows[i]["USERCODE"]?.ToString();
                    m.USERNAME = DT.Rows[i]["USERNAME"]?.ToString();
                    m.USERDESC = DT.Rows[i]["USERDESC"]?.ToString();
                    m.EMAIL = DT.Rows[i]["EMAIL"]?.ToString();
                    m.LASTLOGINDATE = DT.Rows[i]["LASTLOGINDATE"]?.ToString();
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.LOGINCOUNT = DT.Rows[i]["LOGINCOUNT"]?.ToString();
                    m.ISENABLE = Convert.ToBoolean(DT.Rows[i]["ISENABLE"]) ? "1" : "0";

                    rowList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = rowList, total = totalCt };
            }

            return new { rows = "", total = 0 };
        }

        public dynamic GetDeclarantnReceiverData(string sID, long sType)
        {
            string sql = $@"
                            SELECT A.ID, A.NAME, A.IDPHOTO_F, A.IDPHOTO_B, A.PHONE, A.MOBILE, A.ADDR, A.TAXID, A.APPOINTMENT,
                                   FORMAT(A.CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(A.UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   A.CREATEBY AS CREBY, A.UPDBY
                            From T_S_DECLARANT A
							LEFT JOIN T_S_ACDECLARANTMAP B ON A.ID = B.DECLARANTID
							WHERE B.USERCODE = '{sID}' AND A.TYPE = {sType}";
            List<DeclarantInfo> rowList = new List<DeclarantInfo>();
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DeclarantInfo m = new DeclarantInfo();
                    m.ROWID = i + 1;
                    m.ID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.NAME = DT.Rows[i]["NAME"]?.ToString();
                    m.IDPHOTO_F = DT.Rows[i]["IDPHOTO_F"]?.ToString();
                    m.IDPHOTO_B = DT.Rows[i]["IDPHOTO_B"]?.ToString();
                    m.PHONE = DT.Rows[i]["PHONE"]?.ToString();
                    m.MOBILE = DT.Rows[i]["MOBILE"]?.ToString();
                    m.ADDR = DT.Rows[i]["ADDR"]?.ToString();
                    m.TAXID = DT.Rows[i]["TAXID"]?.ToString();
                    m.APPOINTMENT = DT.Rows[i]["APPOINTMENT"]?.ToString();
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.CREBY = DT.Rows[i]["CREBY"]?.ToString();
                    m.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();
                    m.UPDBY = DT.Rows[i]["UPDBY"]?.ToString();

                    rowList.Add(m);
                }

                return new { rows = rowList };
            }

            return new { rows = "" };
        }

    }
}
