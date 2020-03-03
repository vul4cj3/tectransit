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
                                            SELECT ROW_NUMBER() OVER (ORDER BY A.USERSEQ) AS ROW_ID, A.ID, A.USERCODE, A.USERNAME, A.USERDESC, A.EMAIL,
                                                   FORMAT(A.CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(A.LASTLOGINDATE, 'yyyy-MM-dd HH:mm:ss') As LASTLOGINDATE,
                                                   A.LOGINCOUNT, A.ISENABLE, A.COMPANYNAME, A.RATEID
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

        //public dynamic GetTransferListData(string sWhere, int pageIndex, int pageSize)
        //{
        //    string sql = $@"SELECT * FROM (
        //                                     SELECT ROW_NUMBER() OVER (ORDER BY A.UPDDATE) AS ROW_ID, A.ID, A.ACCOUNTID, A.STATIONCODE, A.TRASFERNO, A.TRASFERCOMPANY,
        //                                           A.P_LENGTH, A.P_WIDTH, A.P_HEIGHT, A.P_WEIGHT, A.P_VALUEPRICE, A.STATUS, A.REMARK,
        //                                           FORMAT(A.CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(A.UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
        //                                           A.CREATEBY AS CREBY, A.UPDBY, B.USERCODE AS ACCOUNTCODE
        //                                    FROM T_E_TRANSFER_H A
        //                                    LEFT JOIN T_S_ACCOUNT B ON A.ACCOUNTID = B.ID
        //                                    {sWhere}
        //                    ) AS A";
        //    string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
        //    DataTable DT = DBUtil.SelectDataTable(sql1);
        //    if (DT.Rows.Count > 0)
        //    {
        //        List<TransferHListInfo> rowList = new List<TransferHListInfo>();
        //        for (int i = 0; i < DT.Rows.Count; i++)
        //        {
        //            TransferHListInfo m = new TransferHListInfo();
        //            m.TRANSID = Convert.ToInt64(DT.Rows[i]["ID"]);
        //            m.ACCOUNTID = DT.Rows[i]["ACCOUNTID"]?.ToString();
        //            m.ACCOUNTCODE = DT.Rows[i]["ACCOUNTCODE"]?.ToString();
        //            m.STATIONCODE = DT.Rows[i]["STATIONCODE"]?.ToString();
        //            m.STATIONNAME = DBUtil.GetSingleValue1($@"SELECT STATIONNAME AS COL1 FROM T_S_STATION WHERE STATIONCODE = '{DT.Rows[i]["STATIONCODE"]?.ToString()}'");
        //            m.TRASFERNO = DT.Rows[i]["TRASFERNO"]?.ToString();
        //            m.TRASFERCOMPANY = DT.Rows[i]["TRASFERCOMPANY"]?.ToString();
        //            m.PLENGTH = DT.Rows[i]["P_LENGTH"]?.ToString();
        //            m.PWIDTH = DT.Rows[i]["P_WIDTH"]?.ToString();
        //            m.PHEIGHT = DT.Rows[i]["P_HEIGHT"]?.ToString();
        //            m.PWEIGHT = DT.Rows[i]["P_WEIGHT"]?.ToString();
        //            m.PVALUEPRICE = DT.Rows[i]["P_VALUEPRICE"]?.ToString();
        //            m.STATUS = DT.Rows[i]["STATUS"]?.ToString();
        //            m.REMARK = DT.Rows[i]["REMARK"]?.ToString();
        //            m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
        //            m.CREBY = DT.Rows[i]["CREBY"]?.ToString();
        //            m.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();
        //            m.UPDBY = DT.Rows[i]["UPDBY"]?.ToString();

        //            rowList.Add(m);
        //        }

        //        sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
        //        string totalCt = DBUtil.GetSingleValue1(sql);

        //        return new { rows = rowList, total = totalCt };
        //    }

        //    return new { rows = "", total = 0 };
        //}

        //public dynamic GetTransferData(long sID)
        //{
        //    string sql = $@"
        //                   SELECT A.*, B.*, C.USERCODE AS ACCOUNTCODE, D.STATIONNAME FROM T_E_TRANSFER_H A
        //                   LEFT JOIN T_E_TRANSFER_D B ON A.ID = B.TRANSFERHID
        //                   LEFT JOIN T_S_ACCOUNT C ON A.ACCOUNTID = C.ID
        //                   LEFT JOIN T_S_STATION D ON A.STATIONCODE = D.STATIONCODE
        //                   WHERE A.ID = {sID}";
        //    DataTable DT = DBUtil.SelectDataTable(sql);
        //    List<TransferDInfo> sublist = new List<TransferDInfo>();
        //    if (DT.Rows.Count > 0)
        //    {
        //        TransferHListInfo m = new TransferHListInfo();
        //        m.TRANSID = Convert.ToInt64(DT.Rows[0]["ID"]);        
        //        m.STATIONNAME = DT.Rows[0]["STATIONNAME"]?.ToString();
        //        m.ACCOUNTCODE = DT.Rows[0]["ACCOUNTCODE"]?.ToString();
        //        m.TRASFERNO = DT.Rows[0]["TRASFERNO"]?.ToString();
        //        m.TRASFERCOMPANY = DT.Rows[0]["TRASFERCOMPANY"]?.ToString();                
        //        m.PLENGTH = DT.Rows[0]["P_LENGTH"]?.ToString();                
        //        m.PWIDTH = DT.Rows[0]["P_WIDTH"]?.ToString();                
        //        m.PHEIGHT = DT.Rows[0]["P_HEIGHT"]?.ToString();                
        //        m.PWEIGHT = DT.Rows[0]["P_WEIGHT"]?.ToString();                
        //        m.STATUS = DT.Rows[0]["STATUS"]?.ToString();                
        //        m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
        //        m.CREBY = DT.Rows[0]["CREATEBY"]?.ToString();
        //        m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
        //        m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();


        //        for (int i = 0;i < DT.Rows.Count; i++) {
        //            TransferDInfo d = new TransferDInfo();
        //            d.PRODUCT = DT.Rows[i]["PRODUCT"]?.ToString();
        //            d.QUANTITY = DT.Rows[i]["QUANTITY"]?.ToString();
        //            d.UNITPRICE = DT.Rows[i]["UNIT_PRICE"]?.ToString();
        //            sublist.Add(d);
        //        }
        //        return new { rows = m, subitem = sublist };
        //    }

        //    return new { rows = "", subitem = "" };
        //}

        public dynamic GetTVShippingMListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY UPDDATE DESC) AS ROW_ID, ID, ACCOUNTID, STATIONCODE, SHIPPINGNO, TRACKINGNO, TRANSFERNO, STATUS,
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
                    m.STATIONCODE = DT.Rows[i]["STATIONCODE"]?.ToString();
                    m.STATIONNAME = DBUtil.GetSingleValue1($@"SELECT STATIONNAME AS COL1 FROM T_S_STATION WHERE STATIONCODE = '{DT.Rows[i]["STATIONCODE"]}'");
                    m.SHIPPINGNO = DT.Rows[i]["SHIPPINGNO"]?.ToString();
                    m.TRACKINGNO = DT.Rows[i]["TRACKINGNO"]?.ToString();
                    m.TRANSFERNO = DT.Rows[i]["TRANSFERNO"]?.ToString();
                    m.STATUS = DT.Rows[i]["STATUS"]?.ToString();
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
            string sql = $@"SELECT ID, ACCOUNTID, STATIONCODE, SHIPPINGNO, TRACKINGNO, TRANSFERNO, MAWBNO, CLEARANCENO, HAWBNO,
                                   TOTAL, RECEIVER, RECEIVERADDR, STATUS, FORMAT(PAYDATE, 'yyyy-MM-dd HH:mm:ss') AS PAYDATE, FORMAT(EXPORTDATE, 'yyyy-MM-dd HH:mm:ss') AS EXPORTDATE,
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
                m.STATIONCODE = DT.Rows[0]["STATIONCODE"]?.ToString();
                m.STATIONNAME = DBUtil.GetSingleValue1($@"SELECT STATIONNAME AS COL1 FROM T_S_STATION WHERE STATIONCODE = '{DT.Rows[0]["STATIONCODE"]?.ToString()}'");
                m.TRACKINGNO = DT.Rows[0]["TRACKINGNO"]?.ToString();
                m.TRANSFERNO = DT.Rows[0]["TRANSFERNO"]?.ToString();
                m.MAWBNO = DT.Rows[0]["MAWBNO"]?.ToString();
                m.CLEARANCENO = DT.Rows[0]["CLEARANCENO"]?.ToString();
                m.HAWBNO = DT.Rows[0]["HAWBNO"]?.ToString();
                m.TOTAL = DT.Rows[0]["TOTAL"]?.ToString();
                m.ISMULTRECEIVER = Convert.ToBoolean(DT.Rows[0]["ISMULTRECEIVER"]) == true ? "Y" : "N";
                m.RECEIVER = DT.Rows[0]["RECEIVER"]?.ToString();
                m.RECEIVERADDR = DT.Rows[0]["RECEIVERADDR"]?.ToString();
                m.STATUS = DT.Rows[0]["STATUS"]?.ToString();
                m.PAYDATE = DT.Rows[0]["PAYDATE"]?.ToString();
                m.EXPORTDATE = DT.Rows[0]["EXPORTDATE"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();


                sql = $@"SELECT A.SHIPPINGID_M AS MID, A.ID AS HID, B.ID AS DID, A.BOXNO, A.RECEIVER,
                                A.RECEIVERADDR, B.ID AS DID, B.PRODUCT, B.UNITPRICE, B.QUANTITY
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
                            rows.BOXNO = DT_Sub.Rows[j]["BOXNO"]?.ToString();
                            rows.RECEIVER = DT_Sub.Rows[j]["RECEIVER"]?.ToString();
                            rows.RECEIVERADDR = DT_Sub.Rows[j]["RECEIVERADDR"]?.ToString();
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

                sql = $@"SELECT ID, NAME, TAXID, PHONE, MOBILE, ADDR, IDPHOTO_F AS IDPHOTOF,
                                IDPHOTO_B AS IDPHOTOB, APPOINTMENT, SHIPPINGID_M AS MID
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
                        row_d.ADDR = DT_Dec.Rows[k]["ADDR"]?.ToString();
                        row_d.IDPHOTOF = DT_Dec.Rows[k]["IDPHOTOF"]?.ToString();
                        row_d.IDPHOTOB = DT_Dec.Rows[k]["IDPHOTOB"]?.ToString();
                        row_d.APPOINTMENT = DT_Dec.Rows[k]["APPOINTMENT"]?.ToString();
                        row_d.SHIPPINGID_M = Convert.ToInt64(DT_Dec.Rows[k]["MID"]);

                        Dec.Add(row_d);
                    }
                }

                return new { status = "0", rowM = m, rowH = h, rowD = d, rowDec = Dec };
            }

            return new { status = "0", rowM = "", rowH = "", rowD = "", rowDEC = "" };
        }

    }
}
