using System;
using System.Collections;
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
                m.RANKTYPE = DT.Rows[0]["RANKTYPE"]?.ToString();
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
                                   CREATEBY AS CREBY, UPDBY, ISENABLE, LASTLOGINDATE, LOGINCOUNT, ADDR, TAXID, COMPANYNAME, RATEID
                            From T_S_ACCOUNT
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                AccountInfo m = new AccountInfo();

                sql = $@"SELECT C.RANKTYPE AS COL1 FROM T_S_ACCOUNT A
                         LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE
                         LEFT JOIN T_S_RANK C ON B.RANKID = C.ID
                         WHERE A.ID = {sID}";
                m.RANKTYPE = DBUtil.GetSingleValue1(sql);
                m.USERID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.WAREHOUSENO = DT.Rows[0]["WAREHOUSENO"]?.ToString();
                m.USERSEQ = DT.Rows[0]["USERSEQ"]?.ToString();
                m.USERCODE = DT.Rows[0]["USERCODE"]?.ToString();
                m.USERNAME = DT.Rows[0]["USERNAME"]?.ToString();
                m.USERDESC = DT.Rows[0]["USERDESC"]?.ToString();
                m.COMPANYNAME = DT.Rows[0]["COMPANYNAME"]?.ToString();
                m.RATEID = DT.Rows[0]["RATEID"]?.ToString();
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

        public dynamic GetCompanyListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                SELECT ROW_NUMBER() OVER (ORDER BY A.ID) AS ROW_ID, * FROM (
                                            SELECT DISTINCT A.ID, A.USERCODE, A.USERNAME, A.USERDESC, A.EMAIL,
                                                   FORMAT(A.CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(A.LASTLOGINDATE, 'yyyy-MM-dd HH:mm:ss') As LASTLOGINDATE,
                                                   A.LOGINCOUNT, A.ISENABLE, A.COMPANYNAME, A.RATEID
                                            From T_S_ACCOUNT A
                                            LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE
                                            LEFT JOIN T_S_RANK C ON B.RANKID = C.ID
                                            {sWhere}) AS A) AS B";
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
                    m.COMPANYNAME = DT.Rows[i]["COMPANYNAME"]?.ToString();
                    m.RATEID = DT.Rows[i]["RATEID"]?.ToString();
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
                    m.IDPHOTOF = DT.Rows[i]["IDPHOTO_F"]?.ToString();
                    m.IDPHOTOB = DT.Rows[i]["IDPHOTO_B"]?.ToString();
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

        public dynamic GetTransferListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                             SELECT ROW_NUMBER() OVER (ORDER BY A.UPDDATE) AS ROW_ID, A.ID, A.ACCOUNTID, B.USERCODE AS ACCOUNTCODE, B.USERNAME AS ACCOUNTNAME,
	                                                A.STATIONCODE, A.TRANSFERNO, A.TRANSFERCOMPANY, A.STATUS,
                                                    FORMAT(A.CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(A.UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                    A.CREATEBY AS CREBY, A.UPDBY
                                             FROM T_E_TRANSFER_M A
                                             LEFT JOIN T_S_ACCOUNT B ON A.ACCOUNTID = B.ID
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<TransferMInfo> rowList = new List<TransferMInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    TransferMInfo m = new TransferMInfo();
                    m.ID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.ACCOUNTID = Convert.ToInt64(DT.Rows[i]["ACCOUNTID"]);
                    m.ACCOUNTCODE = DT.Rows[i]["ACCOUNTCODE"]?.ToString();
                    m.ACCOUNTNAME = DT.Rows[i]["ACCOUNTNAME"]?.ToString();
                    m.STATIONCODE = DT.Rows[i]["STATIONCODE"]?.ToString();
                    m.STATIONNAME = DBUtil.GetSingleValue1($@"SELECT STATIONNAME AS COL1 FROM T_S_STATION WHERE STATIONCODE = '{DT.Rows[i]["STATIONCODE"]?.ToString()}'");
                    m.TRANSFERNO = DT.Rows[i]["TRANSFERNO"]?.ToString();
                    m.TRANSFERCOMPANY = DT.Rows[i]["TRANSFERCOMPANY"]?.ToString();
                    m.STATUS = DT.Rows[i]["STATUS"]?.ToString();
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.CREBY = DT.Rows[i]["CREBY"]?.ToString();
                    m.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();
                    m.UPDBY = DT.Rows[i]["UPDBY"]?.ToString();

                    rowList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = rowList, total = totalCt };
            }

            return new { rows = "", total = 0 };
        }

        public dynamic GetTransferData(Hashtable sData)
        {
            string sql = $@"SELECT ID, ACCOUNTID, STATIONCODE, TRANSFERNO, TRANSFERCOMPANY,
                                   P_LENGTH, P_WIDTH, P_HEIGHT, P_WEIGHT, P_VALUEPRICE,
                                   TOTAL, RECEIVER, RECEIVERADDR, RECEIVERPHONE, STATUS,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY, ISMULTRECEIVER
                            FROM T_E_TRANSFER_M
                            WHERE ID = @TRANSFERIDM AND ACCOUNTID = @ACCOUNTID";
            DataTable DT = DBUtil.SelectDataTable(sql, sData);
            if (DT.Rows.Count > 0)
            {
                TransferMInfo m = new TransferMInfo();
                m.ID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.ACCOUNTID = Convert.ToInt64(DT.Rows[0]["ACCOUNTID"]);
                m.ACCOUNTCODE = DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_ACCOUNT WHERE ID = {DT.Rows[0]["ACCOUNTID"]}");
                m.ACCOUNTNAME = DBUtil.GetSingleValue1($@"SELECT USERNAME AS COL1 FROM T_S_ACCOUNT WHERE ID = {DT.Rows[0]["ACCOUNTID"]}");
                m.STATIONCODE = DT.Rows[0]["STATIONCODE"]?.ToString();
                m.STATIONNAME = DBUtil.GetSingleValue1($@"SELECT STATIONNAME AS COL1 FROM T_S_STATION WHERE STATIONCODE = '{DT.Rows[0]["STATIONCODE"]?.ToString()}'");                
                m.TRANSFERNO = DT.Rows[0]["TRANSFERNO"]?.ToString();
                m.PLENGTH = DT.Rows[0]["P_LENGTH"]?.ToString();
                m.PWIDTH = DT.Rows[0]["P_WIDTH"]?.ToString();
                m.PHEIGHT = DT.Rows[0]["P_HEIGHT"]?.ToString();
                m.PWEIGHT = DT.Rows[0]["P_WEIGHT"]?.ToString();
                m.PVALUEPRICE = DT.Rows[0]["P_VALUEPRICE"]?.ToString();
                m.TOTAL = DT.Rows[0]["TOTAL"]?.ToString();
                m.ISMULTRECEIVER = Convert.ToBoolean(DT.Rows[0]["ISMULTRECEIVER"]) == true ? "Y" : "N";
                m.RECEIVER = DT.Rows[0]["RECEIVER"]?.ToString();
                m.RECEIVERADDR = DT.Rows[0]["RECEIVERADDR"]?.ToString();
                m.RECEIVERPHONE = DT.Rows[0]["RECEIVERPHONE"]?.ToString();
                m.STATUS = DT.Rows[0]["STATUS"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();


                sql = $@"SELECT A.TRANSFERID_M AS MID, A.ID AS HID, B.ID AS DID, A.BOXNO, A.RECEIVER,
                                A.RECEIVERADDR, A.RECEIVERPHONE, B.PRODUCT, B.UNITPRICE, B.QUANTITY
                         FROM T_E_TRANSFER_H A
                         LEFT JOIN T_E_TRANSFER_D B ON A.ID = B.TRANSFERID_H
                         WHERE A.TRANSFERID_M = @TRANSFERIDM";

                DataTable DT_Sub = DBUtil.SelectDataTable(sql, sData);
                List<TransferHInfo> h = new List<TransferHInfo>();
                List<TransferDInfo> d = new List<TransferDInfo>();
                long oldHID = 0;
                if (DT_Sub.Rows.Count > 0)
                {
                    for (int j = 0; j < DT_Sub.Rows.Count; j++)
                    {
                        //新增transfer_H
                        if (Convert.ToInt64(DT_Sub.Rows[j]["HID"]) != oldHID)
                        {
                            TransferHInfo rows = new TransferHInfo();
                            rows.ID = Convert.ToInt64(DT_Sub.Rows[j]["HID"]);
                            rows.BOXNO = DT_Sub.Rows[j]["BOXNO"]?.ToString();
                            rows.RECEIVER = DT_Sub.Rows[j]["RECEIVER"]?.ToString();
                            rows.RECEIVERADDR = DT_Sub.Rows[j]["RECEIVERADDR"]?.ToString();
                            rows.RECEIVERPHONE = DT_Sub.Rows[j]["RECEIVERPHONE"]?.ToString();
                            rows.TRANSFERID_M = Convert.ToInt64(DT_Sub.Rows[j]["MID"]);

                            h.Add(rows);
                        }

                        //新增transfer_D
                        TransferDInfo row = new TransferDInfo();
                        row.ID = Convert.ToInt64(DT_Sub.Rows[j]["DID"]);
                        row.PRODUCT = DT_Sub.Rows[j]["PRODUCT"]?.ToString();
                        row.UNITPRICE = DT_Sub.Rows[j]["UNITPRICE"]?.ToString();
                        row.QUANTITY = DT_Sub.Rows[j]["QUANTITY"]?.ToString();
                        row.TRANSFERID_M = Convert.ToInt64(DT_Sub.Rows[j]["MID"]);
                        row.TRANSFERID_H = Convert.ToInt64(DT_Sub.Rows[j]["HID"]);
                        d.Add(row);

                        oldHID = Convert.ToInt64(DT_Sub.Rows[j]["HID"]);
                    }
                }

                sql = $@"SELECT ID, NAME, TAXID, PHONE, MOBILE, ADDR, IDPHOTO_F AS IDPHOTOF,
                                IDPHOTO_B AS IDPHOTOB, APPOINTMENT, TRANSFERID_M AS MID
                         FROM T_E_DECLARANT
                         WHERE TRANSFERID_M = @TRANSFERIDM";

                DataTable DT_Dec = DBUtil.SelectDataTable(sql, sData);
                List<DeclarantMemInfo> Dec = new List<DeclarantMemInfo>();
                if (DT_Dec.Rows.Count > 0)
                {
                    for (int k = 0; k < DT_Dec.Rows.Count; k++)
                    {
                        DeclarantMemInfo row_d = new DeclarantMemInfo();
                        row_d.ID = Convert.ToInt64(DT_Dec.Rows[k]["ID"]);
                        row_d.NAME = DT_Dec.Rows[k]["NAME"]?.ToString();
                        row_d.TAXID = DT_Dec.Rows[k]["TAXID"]?.ToString();
                        row_d.PHONE = DT_Dec.Rows[k]["PHONE"]?.ToString();
                        row_d.MOBILE = DT_Dec.Rows[k]["MOBILE"]?.ToString();
                        row_d.ADDR = DT_Dec.Rows[k]["ADDR"]?.ToString();
                        row_d.IDPHOTOF = DT_Dec.Rows[k]["IDPHOTOF"]?.ToString();
                        row_d.IDPHOTOB = DT_Dec.Rows[k]["IDPHOTOB"]?.ToString();
                        row_d.APPOINTMENT = DT_Dec.Rows[k]["APPOINTMENT"]?.ToString();
                        row_d.TRANSFERID_M = Convert.ToInt64(DT_Dec.Rows[k]["MID"]);

                        Dec.Add(row_d);
                    }
                }

                return new { status = "0", rowM = m, rowH = h, rowD = d, rowDec = Dec };
            }

            return new { status = "0", rowM = "", rowH = "", rowD = "", rowDEC = "" };
        }

        public dynamic GetTVShippingMListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY UPDDATE DESC) AS ROW_ID, ID, ACCOUNTID, SHIPPINGNO, MAWBNO, STATUS,
                                                   SHIPPINGFILE1, SHIPPINGFILE2, BROKERFILE1, BROKERFILE2, MAWBFILE, IMBROKERID, EXBROKERID,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE
                                            From T_V_SHIPPING_M
                                            {sWhere}
                                           ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<ShippingMCusInfo> rowList = new List<ShippingMCusInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    ShippingMCusInfo m = new ShippingMCusInfo();
                    m.ID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.ACCOUNTID = Convert.ToInt64(DT.Rows[i]["ACCOUNTID"]);
                    m.ACCOUNTCODE = DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_ACCOUNT WHERE ID = {DT.Rows[i]["ACCOUNTID"]}");
                    m.COMPANYNAME = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE ID = {DT.Rows[i]["ACCOUNTID"]}");
                    m.SHIPPINGNO = DT.Rows[i]["SHIPPINGNO"]?.ToString();
                    m.MAWBNO = DT.Rows[i]["MAWBNO"]?.ToString();
                    m.STATUS = Convert.ToInt32(DT.Rows[i]["STATUS"]);
                    m.MAWBFILE = DT.Rows[i]["MAWBFILE"]?.ToString();
                    m.SHIPPINGFILE1 = DT.Rows[i]["SHIPPINGFILE1"]?.ToString();
                    m.SHIPPINGFILE2 = DT.Rows[i]["SHIPPINGFILE2"]?.ToString();
                    m.BROKERFILE1 = DT.Rows[i]["BROKERFILE1"]?.ToString();
                    m.BROKERFILE2 = DT.Rows[i]["BROKERFILE2"]?.ToString();
                    m.IMBROKER = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE ID = {DT.Rows[i]["IMBROKERID"]}");
                    m.EXBROKER = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE ID = {DT.Rows[i]["EXBROKERID"]}");
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();

                    rowList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = rowList, total = totalCt };
            }

            return new { rows = "", total = 0 };
        }

        public dynamic GetSingleShippingCusData(Hashtable sData)
        {
            string sql = $@"SELECT ID, ACCOUNTID, SHIPPINGNO, MAWBNO, FLIGHTNUM, TOTAL, TOTALWEIGHT,
                                   SHIPPERCOMPANY, SHIPPER, RECEIVERCOMPANY, RECEIVER, RECEIVERZIPCODE, RECEIVERADDR, RECEIVERPHONE, TAXID, STATUS, STORECODE, IMBROKERID,
                                   EXBROKERID, FORMAT(PAYDATE, 'yyyy-MM-dd HH:mm:ss') AS PAYDATE, FORMAT(EXPORTDATE, 'yyyy-MM-dd HH:mm:ss') AS EXPORTDATE,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY, ISMULTRECEIVER
                            FROM T_V_SHIPPING_M
                            WHERE ID = @SHIPPINGIDM AND ACCOUNTID = @ACCOUNTID";
            DataTable DT = DBUtil.SelectDataTable(sql, sData);
            if (DT.Rows.Count > 0)
            {
                ShippingMCusInfo m = new ShippingMCusInfo();
                m.ID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.ACCOUNTID = Convert.ToInt64(DT.Rows[0]["ACCOUNTID"]);
                m.ACCOUNTCODE = DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_ACCOUNT WHERE ID = {DT.Rows[0]["ACCOUNTID"]}");
                m.COMPANYNAME = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE ID = {DT.Rows[0]["ACCOUNTID"]}");
                m.SHIPPINGNO = DT.Rows[0]["SHIPPINGNO"]?.ToString();                
                m.MAWBNO = DT.Rows[0]["MAWBNO"]?.ToString();
                m.FLIGHTNUM = DT.Rows[0]["FLIGHTNUM"]?.ToString();
                m.TOTAL = DT.Rows[0]["TOTAL"]?.ToString();
                m.TOTALWEIGHT = DT.Rows[0]["TOTALWEIGHT"]?.ToString();
                m.ISMULTRECEIVER = Convert.ToBoolean(DT.Rows[0]["ISMULTRECEIVER"]) == true ? "Y" : "N";
                m.SHIPPERCOMPANY = DT.Rows[0]["SHIPPERCOMPANY"]?.ToString();
                m.SHIPPER = DT.Rows[0]["SHIPPER"]?.ToString();
                m.RECEIVERCOMPANY = DT.Rows[0]["RECEIVERCOMPANY"]?.ToString();
                m.RECEIVER = DT.Rows[0]["RECEIVER"]?.ToString();
                m.RECEIVERZIPCODE = DT.Rows[0]["RECEIVERZIPCODE"]?.ToString();
                m.RECEIVERADDR = DT.Rows[0]["RECEIVERADDR"]?.ToString();
                m.RECEIVERPHONE = DT.Rows[0]["RECEIVERPHONE"]?.ToString();
                m.RECEIVERTAXID = DT.Rows[0]["TAXID"]?.ToString();
                m.STATUS = Convert.ToInt32(DT.Rows[0]["STATUS"]);
                m.STORECODE = DT.Rows[0]["STORECODE"]?.ToString();
                m.IMBROKERID = Convert.ToInt64(DT.Rows[0]["IMBROKERID"]?.ToString());
                m.EXBROKERID = Convert.ToInt64(DT.Rows[0]["EXBROKERID"]?.ToString());
                m.PAYDATE = DT.Rows[0]["PAYDATE"]?.ToString();
                m.EXPORTDATE = DT.Rows[0]["EXPORTDATE"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();


                sql = $@"SELECT A.SHIPPINGID_M AS MID, A.ID AS HID, B.ID AS DID, A.CLEARANCENO, A.TRANSFERNO,
                                A.TRACKINGNO, A.DEPOTSTATUS, A.WEIGHT, A.TOTALITEM, A.REMARK1, A.REMARK2,
                                A.SHIPPERCOMPANY, A.SHIPPER, A.RECEIVERCOMPANY, A.RECEIVER, A.RECEIVERZIPCODE,
                                A.RECEIVERADDR, A.RECEIVERPHONE, A.TAXID, A.LOGISTICS, A.SHIPPERREMARK, B.PRODUCT, B.UNITPRICE, B.QUANTITY
                         FROM T_V_SHIPPING_H A
                         LEFT JOIN T_V_SHIPPING_D B ON A.ID = B.SHIPPINGID_H
                         WHERE A.SHIPPINGID_M = @SHIPPINGIDM";

                DataTable DT_Sub = DBUtil.SelectDataTable(sql, sData);
                List<ShippingHCusInfo> h = new List<ShippingHCusInfo>();
                List<ShippingDCusInfo> d = new List<ShippingDCusInfo>();
                long oldHID = 0;
                if (DT_Sub.Rows.Count > 0)
                {
                    for (int j = 0; j < DT_Sub.Rows.Count; j++)
                    {
                        //新增shipping_H
                        if (Convert.ToInt64(DT_Sub.Rows[j]["HID"]) != oldHID)
                        {
                            ShippingHCusInfo rows = new ShippingHCusInfo();
                            rows.ID = Convert.ToInt64(DT_Sub.Rows[j]["HID"]);
                            rows.CLEARANCENO = DT_Sub.Rows[j]["CLEARANCENO"]?.ToString();
                            rows.TRANSFERNO = DT_Sub.Rows[j]["TRANSFERNO"]?.ToString();
                            rows.TRACKINGNO = DT_Sub.Rows[j]["TRACKINGNO"]?.ToString();
                            rows.DEPOTSTATUS = string.IsNullOrEmpty(DT_Sub.Rows[j]["DEPOTSTATUS"]?.ToString()) ? 0 : Convert.ToInt32(DT_Sub.Rows[j]["DEPOTSTATUS"]);
                            rows.SHIPPERCOMPANY = DT_Sub.Rows[j]["SHIPPERCOMPANY"]?.ToString();
                            rows.SHIPPER = DT_Sub.Rows[j]["SHIPPER"]?.ToString();
                            rows.RECEIVERCOMPANY = DT_Sub.Rows[j]["RECEIVERCOMPANY"]?.ToString();
                            rows.RECEIVER = DT_Sub.Rows[j]["RECEIVER"]?.ToString();
                            rows.RECEIVERZIPCODE = DT_Sub.Rows[j]["RECEIVERZIPCODE"]?.ToString();
                            rows.RECEIVERADDR = DT_Sub.Rows[j]["RECEIVERADDR"]?.ToString();
                            rows.RECEIVERPHONE = DT_Sub.Rows[j]["RECEIVERPHONE"]?.ToString();
                            rows.RECEIVERTAXID = DT_Sub.Rows[j]["TAXID"]?.ToString();
                            rows.WEIGHT = DT_Sub.Rows[j]["WEIGHT"]?.ToString();
                            rows.TOTALITEM = DT_Sub.Rows[j]["TOTALITEM"]?.ToString();
                            rows.LOGISTICS = DT_Sub.Rows[j]["LOGISTICS"]?.ToString();
                            rows.SHIPPERREMARK = DT_Sub.Rows[j]["SHIPPERREMARK"]?.ToString();
                            rows.REMARK1 = DT_Sub.Rows[j]["REMARK1"]?.ToString();
                            rows.REMARK2 = DT_Sub.Rows[j]["REMARK2"]?.ToString();
                            rows.SHIPPINGID_M = Convert.ToInt64(DT_Sub.Rows[j]["MID"]);

                            h.Add(rows);
                        }

                        //新增shipping_D
                        ShippingDCusInfo row = new ShippingDCusInfo();
                        row.ID = Convert.ToInt64(DT_Sub.Rows[j]["DID"]);
                        row.PRODUCT = DT_Sub.Rows[j]["PRODUCT"]?.ToString();
                        row.UNITPRICE = DT_Sub.Rows[j]["UNITPRICE"]?.ToString();
                        row.QUANTITY = DT_Sub.Rows[j]["QUANTITY"]?.ToString();
                        row.SHIPPINGID_M = Convert.ToInt64(DT_Sub.Rows[j]["MID"]);
                        row.SHIPPINGID_H = Convert.ToInt64(DT_Sub.Rows[j]["HID"]);
                        d.Add(row);

                        oldHID = Convert.ToInt64(DT_Sub.Rows[j]["HID"]);
                    }
                }

                sql = $@"SELECT SHIPPINGID_M AS MID, SHIPPINGID_H AS HID, ID, NAME, TAXID, PHONE, MOBILE, ZIPCODE, ADDR, IDPHOTO_F AS IDPHOTOF,
                                IDPHOTO_B AS IDPHOTOB, APPOINTMENT
                         FROM T_V_DECLARANT
                         WHERE SHIPPINGID_M = @SHIPPINGIDM";

                DataTable DT_Dec = DBUtil.SelectDataTable(sql, sData);
                List<DeclarantCusInfo> Dec = new List<DeclarantCusInfo>();
                if (DT_Dec.Rows.Count > 0)
                {
                    for (int k = 0; k < DT_Dec.Rows.Count; k++)
                    {
                        DeclarantCusInfo row_d = new DeclarantCusInfo();
                        row_d.ID = Convert.ToInt64(DT_Dec.Rows[k]["ID"]);
                        row_d.NAME = DT_Dec.Rows[k]["NAME"]?.ToString();
                        row_d.TAXID = DT_Dec.Rows[k]["TAXID"]?.ToString();
                        row_d.PHONE = DT_Dec.Rows[k]["PHONE"]?.ToString();
                        row_d.MOBILE = DT_Dec.Rows[k]["MOBILE"]?.ToString();
                        row_d.ZIPCODE = DT_Dec.Rows[k]["ZIPCODE"]?.ToString();
                        row_d.ADDR = DT_Dec.Rows[k]["ADDR"]?.ToString();
                        row_d.IDPHOTOF = DT_Dec.Rows[k]["IDPHOTOF"]?.ToString();
                        row_d.IDPHOTOB = DT_Dec.Rows[k]["IDPHOTOB"]?.ToString();
                        row_d.APPOINTMENT = DT_Dec.Rows[k]["APPOINTMENT"]?.ToString();
                        row_d.SHIPPINGID_M = Convert.ToInt64(DT_Dec.Rows[k]["MID"]);
                        row_d.SHIPPINGID_H = Convert.ToInt64(DT_Dec.Rows[k]["HID"]);

                        Dec.Add(row_d);
                    }
                }

                return new { status = "0", rowM = m, rowH = h, rowD = d, rowDec = Dec };
            }

            return new { status = "0", rowM = "", rowH = "", rowD = "", rowDEC = "" };
        }


        public dynamic GetBrokerData()
        {
            string sql = $@"SELECT A.ID, A.USERCODE, A.USERNAME, A.COMPANYNAME, C.RANKTYPE FROM T_S_ACCOUNT A
                            LEFT JOIN T_S_ACRANKMAP B ON A.USERCODE = B.USERCODE 
                            LEFT JOIN T_S_RANK C ON B.RANKID = C.ID
                            WHERE A.ISENABLE = 'true' AND C.ISENABLE = 'true' AND C.RANKTYPE IN (3, 4)
                            ORDER BY C.RANKTYPE, A.ID";
            DataTable DT = DBUtil.SelectDataTable(sql);
            List<AccountInfo> im = new List<AccountInfo>();
            List<AccountInfo> ex = new List<AccountInfo>();

            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    AccountInfo ac = new AccountInfo();
                    ac.USERID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    ac.USERCODE = DT.Rows[i]["USERCODE"]?.ToString();
                    ac.USERNAME = DT.Rows[i]["USERNAME"]?.ToString();
                    ac.COMPANYNAME = DT.Rows[i]["COMPANYNAME"]?.ToString();

                    if (DT.Rows[i]["RANKTYPE"]?.ToString() == "3")
                        im.Add(ac);
                    else
                        ex.Add(ac);
                }

                return new { status = "0", imList = im, exList = ex };
            }

            return new { status = "0", imList = "", exList = "" };
        }


        public long InsertCusShippingH(Hashtable sData)
        {
            long ID = 0;
            try
            {

                string sql = $@"INSERT INTO T_V_SHIPPING_H (CLEARANCENO, TRANSFERNO, SHIPPERCOMPANY, SHIPPER, RECEIVERCOMPANY,
                                                            RECEIVER, RECEIVERZIPCODE, RECEIVERADDR, RECEIVERPHONE, TAXID,
                                                            WEIGHT, TOTALITEM, SHIPPERREMARK, LOGISTICS, SHIPPINGID_M)
                                VALUES (@CLEARANCENO, @TRANSFERNO, @SHIPPERCOMPANY, @SHIPPER, @RECEIVERCOMPANY,
                                                            @RECEIVER, @RECEIVERZIPCODE, @RECEIVERADDR, @RECEIVERPHONE, @TAXID,
                                                            @WEIGHT, @TOTALITEM, @SHIPPERREMARK, @LOGISTICS, @SHIPPINGIDM)";

                DBUtil.EXECUTE(sql, sData);

                string HID = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_V_SHIPPING_H WHERE TRANSFERNO = @TRANSFERNO AND SHIPPINGID_M = @SHIPPINGIDM", sData);
                ID = string.IsNullOrEmpty(HID) ? 0 : Convert.ToInt64(HID);

                return ID;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return ID;
            }
        }

        public void UpdateCusShippingH(Hashtable sData)
        {

            string sql = $@"UPDATE T_V_SHIPPING_H SET
                                                       WEIGHT = @WEIGHT,
                                                       TOTALITEM = @TOTALITEM
                             WHERE ID = @SHIPPINGIDH";


            DBUtil.EXECUTE(sql, sData);

        }

        public void InsertCusShippingD(Hashtable sData)
        {
            try
            {
                string sql = $@"INSERT INTO T_V_SHIPPING_D (PRODUCT, QUANTITY, UNITPRICE, UNIT, ORIGIN,
                                                            SHIPPINGID_M, SHIPPINGID_H)
                                VALUES (@PRODUCT, @QUANTITY, @UNITPRICE, @UNIT, @ORIGIN,
                                                            @SHIPPINGIDM, @SHIPPINGIDH)";

                DBUtil.EXECUTE(sql, sData);

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        public void InsertTVDeclarant(Hashtable sData)
        {
            string sql = $@"INSERT INTO T_V_DECLARANT (NAME, TAXID, PHONE, ZIPCODE, ADDR, SHIPPINGID_M, SHIPPINGID_H)
                           VALUES (@NAME, @TAXID, @PHONE, @ZIPCODE, @ADDR, @SHIPPINGIDM, @SHIPPINGIDH)";

            DBUtil.EXECUTE(sql, sData);

        }

        public void UpdateCusShippingM(Hashtable sData)
        {
            sData["ISMULTRECEIVER"] = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;

            string sql = $@"UPDATE T_V_SHIPPING_M SET
                                                       MAWBNO = @MAWBNO,
                                                       FLIGHTNUM = @FLIGHTNUM,
                                                       SHIPPINGFILE2 = @SHIPPINGFILE2,
                                                       TOTAL = @TOTAL,
                                                       TOTALWEIGHT = @TOTALWEIGHT,
                                                       ISMULTRECEIVER = @ISMULTRECEIVER,
                                                       SHIPPERCOMPANY = @SHIPPERCOMPANY,
                                                       SHIPPER = @SHIPPER,
                                                       RECEIVERCOMPANY = @RECEIVERCOMPANY,
                                                       RECEIVER = @RECEIVER,
                                                       RECEIVERZIPCODE = @RECEIVERZIPCODE,
                                                       RECEIVERADDR = @RECEIVERADDR,
                                                       RECEIVERPHONE = @RECEIVERPHONE,
                                                       TAXID = @TAXID,
                                                       STORECODE = @STORECODE,
                                                       UPDDATE = @UPDDATE,
                                                       UPDBY = @UPDBY
                             WHERE ID = @ID";


            DBUtil.EXECUTE(sql, sData);
        }

    }
}
