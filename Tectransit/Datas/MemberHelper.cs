using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Tectransit.Datas
{
    public class MemberHelper
    {
        CommonHelper objComm = new CommonHelper();
        public dynamic GetMemData(Hashtable sData)
        {
            string sql = $@"
                            SELECT ID, USERCODE, WAREHOUSENO, USERNAME, USERDESC, USERSEQ, EMAIL, IDPHOTO_F, IDPHOTO_B, PHONE, MOBILE,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY, ISENABLE, LASTLOGINDATE, LOGINCOUNT, ADDR, TAXID, COMPANYNAME, RATEID
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

        public dynamic GetCusMemData(Hashtable sData)
        {
            string sql = $@"
                            SELECT ID, USERCODE, WAREHOUSENO, USERNAME, USERDESC, USERSEQ, EMAIL, IDPHOTO_F, IDPHOTO_B, PHONE, MOBILE,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY, ISENABLE, LASTLOGINDATE, LOGINCOUNT, ADDR, TAXID, COMPANYNAME, RATEID
                            From T_S_ACCOUNT
                            WHERE USERCODE = '{sData["_cuscode"]}'";
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

        public dynamic GetTransferData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY UPDDATE) AS ROW_ID, ID, STATIONCODE, TRANSFERNO, TRANSFERCOMPANY,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, STATUS
                                            From T_E_TRANSFER_M
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
                    m.STATIONCODE = DT.Rows[i]["STATIONCODE"]?.ToString();
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

        public dynamic GetShippingData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY UPDDATE) AS ROW_ID, ID, STATIONCODE, SHIPPINGNO, TRACKINGNO, TRACKINGDESC, TRACKINGREMARK,
                                                   P_LENGTH, P_WIDTH, P_HEIGHT, P_WEIGHT, TOTAL, RECEIVER, RECEIVERADDR, ISMULTRECEIVER, TRACKINGTYPE, STATUS,
                                                   PAYTYPE, PAYSTATUS, REMARK1, REMARK2, REMARK3, FORMAT(PAYDATE, 'yyyy-MM-dd HH:mm:ss') AS PAYDATE, FORMAT(EXPORTDATE, 'yyyy-MM-dd HH:mm:ss') AS EXPORTDATE,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY
                                            From T_N_SHIPPING_M
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<ShippingMInfo> rowList = new List<ShippingMInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    ShippingMInfo m = new ShippingMInfo();
                    m.ID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.SHIPPINGNO = DT.Rows[i]["SHIPPINGNO"]?.ToString();
                    m.STATIONCODE = DT.Rows[i]["STATIONCODE"]?.ToString();
                    m.TRACKINGNO = DT.Rows[i]["TRACKINGNO"]?.ToString();
                    m.TRACKINGDESC = DT.Rows[i]["TRACKINGDESC"]?.ToString();
                    m.TRACKINGREMARK = DT.Rows[i]["TRACKINGREMARK"]?.ToString();
                    m.PLENGTH = DT.Rows[i]["P_LENGTH"]?.ToString();
                    m.PWIDTH = DT.Rows[i]["P_WIDTH"]?.ToString();
                    m.PHEIGHT = DT.Rows[i]["P_HEIGHT"]?.ToString();
                    m.PWEIGHT = DT.Rows[i]["P_WEIGHT"]?.ToString();
                    m.TOTAL = DT.Rows[i]["TOTAL"]?.ToString();
                    m.RECEIVER = DT.Rows[i]["RECEIVER"]?.ToString();
                    m.RECEIVERADDR = DT.Rows[i]["RECEIVERADDR"]?.ToString();
                    m.ISMULTRECEIVER = Convert.ToBoolean(DT.Rows[0]["ISMULTRECEIVER"]) == true ? "Y" : "N";
                    m.TRACKINGTYPE = DT.Rows[i]["TRACKINGTYPE"]?.ToString();
                    m.STATUS = DT.Rows[i]["STATUS"]?.ToString();
                    m.PAYTYPE = DT.Rows[i]["PAYTYPE"]?.ToString();
                    m.PAYSTATUS = DT.Rows[i]["PAYSTATUS"]?.ToString();
                    m.REMARK1 = DT.Rows[i]["REMARK1"]?.ToString();
                    m.REMARK2 = DT.Rows[i]["REMARK2"]?.ToString();
                    m.REMARK3 = DT.Rows[i]["REMARK3"]?.ToString();
                    m.PAYDATE = DT.Rows[i]["PAYDATE"]?.ToString();
                    m.EXPORTDATE = DT.Rows[i]["EXPORTDATE"]?.ToString();
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

        public dynamic GetSingleACTransferData(Hashtable sData)
        {
            string sql = $@"SELECT ID, ACCOUNTID, STATIONCODE, TRANSFERNO, TOTAL,
                                   RECEIVER, RECEIVERPHONE, RECEIVERADDR, ISMULTRECEIVER, STATUS,
								   P_LENGTH, P_WIDTH, P_HEIGHT, P_WEIGHT, P_VALUEPRICE,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY
                            FROM T_E_TRANSFER_M
                            WHERE ID = @TRANSFERIDM AND ACCOUNTID = @ACCOUNTID";
            DataTable DT = DBUtil.SelectDataTable(sql, sData);
            if (DT.Rows.Count > 0)
            {
                TransferMInfo m = new TransferMInfo();
                m.ID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.ACCOUNTID = Convert.ToInt64(DT.Rows[0]["ACCOUNTID"]);
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
                m.RECEIVERPHONE = DT.Rows[0]["RECEIVERPHONE"]?.ToString();
                m.RECEIVERADDR = DT.Rows[0]["RECEIVERADDR"]?.ToString();
                m.STATUS = DT.Rows[0]["STATUS"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();


                sql = $@"SELECT A.TRANSFERID_M AS MID, A.ID AS HID, B.ID AS DID, A.BOXNO, A.RECEIVER,
                                A.RECEIVERPHONE, A.RECEIVERADDR, B.PRODUCT, B.UNITPRICE, B.QUANTITY
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
                            rows.RECEIVERPHONE = DT_Sub.Rows[j]["RECEIVERPHONE"]?.ToString();
                            rows.RECEIVERADDR = DT_Sub.Rows[j]["RECEIVERADDR"]?.ToString();
                            rows.TRANSFERID_M = Convert.ToInt64(DT_Sub.Rows[j]["MID"]);

                            h.Add(rows);
                        }

                        //新增transfer_D
                        TransferDInfo row = new TransferDInfo();
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

            return new { status = "99", rowM = "", rowH = "", rowD = "", rowDEC = "" };
        }

        public dynamic GetSingleACShippingData(Hashtable sData)
        {
            string sql = $@"SELECT ID, ACCOUNTID, STATIONCODE, SHIPPINGNO, TOTAL, TOTALPRICE,
								   TRACKINGNO, TRACKINGDESC, TRACKINGREMARK,
                                   RECEIVER, RECEIVERPHONE, RECEIVERADDR, ISMULTRECEIVER, STATUS,
								   P_LENGTH, P_WIDTH, P_HEIGHT, P_WEIGHT, P_VALUEPRICE,
                                   MAWBNO, CLEARANCENO, HAWBNO,
								   PAYTYPE, PAYSTATUS, TRACKINGTYPE, FORMAT(PAYDATE, 'yyyy-MM-dd HH:mm:ss') As PAYDATE,
								   FORMAT(EXPORTDATE, 'yyyy-MM-dd HH:mm:ss') As EXPORTDATE,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY
                            FROM T_N_SHIPPING_M
                            WHERE ID = @SHIPPINGIDM AND ACCOUNTID = @ACCOUNTID";
            DataTable DT = DBUtil.SelectDataTable(sql, sData);
            if (DT.Rows.Count > 0)
            {
                ShippingMInfo m = new ShippingMInfo();
                m.ID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.ACCOUNTID = Convert.ToInt64(DT.Rows[0]["ACCOUNTID"]);
                m.STATIONCODE = DT.Rows[0]["STATIONCODE"]?.ToString();
                m.STATIONNAME = DBUtil.GetSingleValue1($@"SELECT STATIONNAME AS COL1 FROM T_S_STATION WHERE STATIONCODE = '{DT.Rows[0]["STATIONCODE"]?.ToString()}'");
                m.SHIPPINGNO = DT.Rows[0]["SHIPPINGNO"]?.ToString();
                m.TRACKINGNO = DT.Rows[0]["TRACKINGNO"]?.ToString();
                m.TRACKINGDESC = DT.Rows[0]["TRACKINGDESC"]?.ToString();
                m.TRACKINGREMARK = DT.Rows[0]["TRACKINGREMARK"]?.ToString();
                m.PLENGTH = DT.Rows[0]["P_LENGTH"]?.ToString();
                m.PWIDTH = DT.Rows[0]["P_WIDTH"]?.ToString();
                m.PHEIGHT = DT.Rows[0]["P_HEIGHT"]?.ToString();
                m.PWEIGHT = DT.Rows[0]["P_WEIGHT"]?.ToString();
                m.PVALUEPRICE = DT.Rows[0]["P_VALUEPRICE"]?.ToString();
                m.MAWBNO = DT.Rows[0]["MAWBNO"]?.ToString();
                m.CLEARANCENO = DT.Rows[0]["CLEARANCENO"]?.ToString();
                m.HAWBNO = DT.Rows[0]["HAWBNO"]?.ToString();
                m.TOTAL = DT.Rows[0]["TOTAL"]?.ToString();
                m.TOTALPRICE = DT.Rows[0]["TOTALPRICE"]?.ToString();
                m.ISMULTRECEIVER = Convert.ToBoolean(DT.Rows[0]["ISMULTRECEIVER"]) == true ? "Y" : "N";
                m.RECEIVER = DT.Rows[0]["RECEIVER"]?.ToString();
                m.RECEIVERPHONE = DT.Rows[0]["RECEIVERPHONE"]?.ToString();
                m.RECEIVERADDR = DT.Rows[0]["RECEIVERADDR"]?.ToString();
                m.STATUS = DT.Rows[0]["STATUS"]?.ToString();
                m.TRACKINGTYPE = DT.Rows[0]["TRACKINGTYPE"]?.ToString();
                m.PAYTYPE = DT.Rows[0]["PAYTYPE"]?.ToString();
                m.PAYSTATUS = DT.Rows[0]["PAYSTATUS"]?.ToString();
                m.PAYDATE = DT.Rows[0]["PAYDATE"]?.ToString();
                m.EXPORTDATE = DT.Rows[0]["EXPORTDATE"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();


                sql = $@"SELECT A.SHIPPINGID_M AS MID, A.ID AS HID, B.ID AS DID, A.TRANSFERNO, A.BOXNO, A.RECEIVER,
                                A.RECEIVERPHONE, A.RECEIVERADDR, B.PRODUCT, B.UNITPRICE, B.QUANTITY
                         FROM T_N_SHIPPING_H A
                         LEFT JOIN T_N_SHIPPING_D B ON A.ID = B.SHIPPINGID_H
                         WHERE A.SHIPPINGID_M = @SHIPPINGIDM";

                DataTable DT_Sub = DBUtil.SelectDataTable(sql, sData);
                List<ShippingHInfo> h = new List<ShippingHInfo>();
                List<ShippingDInfo> d = new List<ShippingDInfo>();
                long oldHID = 0;
                if (DT_Sub.Rows.Count > 0)
                {
                    for (int j = 0; j < DT_Sub.Rows.Count; j++)
                    {
                        //新增shipping_H
                        if (Convert.ToInt64(DT_Sub.Rows[j]["HID"]) != oldHID)
                        {
                            ShippingHInfo rows = new ShippingHInfo();
                            rows.ID = Convert.ToInt64(DT_Sub.Rows[j]["HID"]);
                            rows.TRANSFERNO = DT_Sub.Rows[j]["TRANSFERNO"]?.ToString();
                            rows.BOXNO = DT_Sub.Rows[j]["BOXNO"]?.ToString();
                            rows.RECEIVER = DT_Sub.Rows[j]["RECEIVER"]?.ToString();
                            rows.RECEIVERPHONE = DT_Sub.Rows[j]["RECEIVERPHONE"]?.ToString();
                            rows.RECEIVERADDR = DT_Sub.Rows[j]["RECEIVERADDR"]?.ToString();
                            rows.SHIPPINGID_M = Convert.ToInt64(DT_Sub.Rows[j]["MID"]);

                            h.Add(rows);
                        }

                        //新增shipping_D
                        ShippingDInfo row = new ShippingDInfo();
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
                         FROM T_N_DECLARANT
                         WHERE SHIPPINGID_M = @SHIPPINGIDM";

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
                        row_d.SHIPPINGID_M = Convert.ToInt64(DT_Dec.Rows[k]["MID"]);

                        Dec.Add(row_d);
                    }
                }

                return new { status = "0", rowM = m, rowH = h, rowD = d, rowDec = Dec };
            }

            return new { status = "99", rowM = "", rowH = "", rowD = "", rowDEC = "" };
        }

        //public dynamic GetTransferData_Combine(string sID)
        //{
        //    string sql = $@"
        //                   SELECT A.TRASFERNO AS TRASFERNO, B.*, C.USERCODE AS ACCOUNTCODE, D.STATIONNAME FROM T_E_TRANSFER_H A
        //                   LEFT JOIN T_E_TRANSFER_D B ON A.ID = B.TRANSFERHID
        //                   LEFT JOIN T_S_ACCOUNT C ON A.ACCOUNTID = C.ID
        //                   LEFT JOIN T_S_STATION D ON A.STATIONCODE = D.STATIONCODE
        //                   WHERE A.ID IN ({sID})";
        //    DataTable DT = DBUtil.SelectDataTable(sql);
        //    List<TransferDInfo_Combine> sublist = new List<TransferDInfo_Combine>();
        //    if (DT.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < DT.Rows.Count; i++)
        //        {
        //            TransferDInfo_Combine d = new TransferDInfo_Combine();
        //            d.TRANSID = Convert.ToInt64(DT.Rows[i]["ID"]);
        //            d.PRODUCT = DT.Rows[i]["PRODUCT"]?.ToString();
        //            d.QUANTITY = DT.Rows[i]["QUANTITY"]?.ToString();
        //            d.UNITPRICE = DT.Rows[i]["UNIT_PRICE"]?.ToString();
        //            d.TRANSHID = DT.Rows[i]["TRANSFERHID"]?.ToString();
        //            d.TRASFERNO = DT.Rows[i]["TRASFERNO"]?.ToString();
        //            sublist.Add(d);
        //        }
        //        return new { rows = sublist };
        //    }

        //    return new { rows = "" };
        //}

        public dynamic GetDeclarantData(Hashtable sData)
        {
            string sWhere = "";
            if (sData["DECLARANTID"] != null)
            {
                if (sData["DECLARANTID"]?.ToString() != "0")
                    sWhere = $@" AND A.ID = {sData["DECLARANTID"]}";
            }

            string sql = $@"
                            SELECT A.* FROM T_S_DECLARANT A
                            LEFT JOIN T_S_ACDECLARANTMAP B ON A.ID = B.DECLARANTID
                            WHERE A.TYPE = 2 AND B.USERCODE = '{sData["_acccode"]}'
                            {sWhere}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                if (sData["DECLARANTID"] != null)
                {
                    DeclarantInfo m = new DeclarantInfo();
                    m.ID = Convert.ToInt64(DT.Rows[0]["ID"]);
                    m.TYPE = Convert.ToInt64(DT.Rows[0]["TYPE"]);
                    m.NAME = DT.Rows[0]["NAME"]?.ToString();
                    m.TAXID = DT.Rows[0]["TAXID"]?.ToString();
                    m.PHONE = DT.Rows[0]["PHONE"]?.ToString();
                    m.MOBILE = DT.Rows[0]["MOBILE"]?.ToString();
                    m.ADDR = DT.Rows[0]["ADDR"]?.ToString();
                    m.IDPHOTOF = DT.Rows[0]["IDPHOTO_F"]?.ToString();
                    m.IDPHOTOB = DT.Rows[0]["IDPHOTO_B"]?.ToString();
                    m.APPOINTMENT = DT.Rows[0]["APPOINTMENT"]?.ToString();
                    m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                    m.CREBY = DT.Rows[0]["CREATEBY"]?.ToString();
                    m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                    m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();

                    return new { rows = m };
                }
                else
                {
                    List<DeclarantInfo> m = new List<DeclarantInfo>();
                    for (int i = 0; i < DT.Rows.Count;i++)
                    {
                        DeclarantInfo d = new DeclarantInfo();
                        d.ID = Convert.ToInt64(DT.Rows[i]["ID"]);
                        d.TYPE = Convert.ToInt64(DT.Rows[i]["TYPE"]);
                        d.NAME = DT.Rows[i]["NAME"]?.ToString();
                        d.TAXID = DT.Rows[i]["TAXID"]?.ToString();
                        d.PHONE = DT.Rows[i]["PHONE"]?.ToString();
                        d.MOBILE = DT.Rows[i]["MOBILE"]?.ToString();
                        d.ADDR = DT.Rows[i]["ADDR"]?.ToString();
                        d.IDPHOTOF = DT.Rows[i]["IDPHOTO_F"]?.ToString();
                        d.IDPHOTOB = DT.Rows[i]["IDPHOTO_B"]?.ToString();
                        d.APPOINTMENT = DT.Rows[i]["APPOINTMENT"]?.ToString();
                        d.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                        d.CREBY = DT.Rows[i]["CREATEBY"]?.ToString();
                        d.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();
                        d.UPDBY = DT.Rows[i]["UPDBY"]?.ToString();

                        m.Add(d);
                    }

                    return new { rows = m };
                }
                
            }

            return new { rows = "" };
        }

        /*--------------------- 廠商會員 --------------------------*/
        public dynamic GetShippingCusData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY CREDATE DESC) AS ROW_ID, ID, ACCOUNTID, SHIPPINGNO, MAWBNO,
                                                   TOTAL, TOTALWEIGHT, STATUS, FORMAT(PAYDATE, 'yyyy-MM-dd HH:mm:ss') AS PAYDATE, FORMAT(EXPORTDATE, 'yyyy-MM-dd HH:mm:ss') AS EXPORTDATE,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY
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
                    m.SHIPPINGNO = DT.Rows[i]["SHIPPINGNO"]?.ToString();
                    m.MAWBNO = DT.Rows[i]["MAWBNO"]?.ToString();
                    m.TOTAL = DT.Rows[i]["TOTAL"]?.ToString();
                    m.TOTALWEIGHT = DT.Rows[i]["TOTALWEIGHT"]?.ToString();
                    m.STATUS = Convert.ToInt32(DT.Rows[i]["STATUS"]);
                    m.PAYDATE = DT.Rows[i]["PAYDATE"]?.ToString();
                    m.EXPORTDATE = DT.Rows[i]["EXPORTDATE"]?.ToString();
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

        public dynamic GetSingleShippingCusData(Hashtable sData)
        {
            string sql = $@"SELECT ID, ACCOUNTID, SHIPPINGNO, MAWBNO, TOTAL,
                                   TOTALWEIGHT, ISMULTRECEIVER, RECEIVER, RECEIVERADDR, RECEIVERPHONE, TAXID, STATUS, FORMAT(PAYDATE, 'yyyy-MM-dd HH:mm:ss') AS PAYDATE, FORMAT(EXPORTDATE, 'yyyy-MM-dd HH:mm:ss') AS EXPORTDATE,
                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                   CREATEBY AS CREBY, UPDBY
                            FROM T_V_SHIPPING_M
                            WHERE ID = @SHIPPINGIDM AND ACCOUNTID = @ACCOUNTID";
            DataTable DT = DBUtil.SelectDataTable(sql, sData);
            if (DT.Rows.Count > 0)
            {
                ShippingMCusInfo m = new ShippingMCusInfo();
                m.ID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.ACCOUNTID = Convert.ToInt64(DT.Rows[0]["ACCOUNTID"]);
                m.SHIPPINGNO = DT.Rows[0]["SHIPPINGNO"]?.ToString();                
                m.MAWBNO = DT.Rows[0]["MAWBNO"]?.ToString();                
                m.TOTAL = DT.Rows[0]["TOTAL"]?.ToString();
                m.TOTALWEIGHT = DT.Rows[0]["TOTALWEIGHT"]?.ToString();
                m.ISMULTRECEIVER = Convert.ToBoolean(DT.Rows[0]["ISMULTRECEIVER"]) == true ? "Y" : "N";
                m.RECEIVER = DT.Rows[0]["RECEIVER"]?.ToString();
                m.RECEIVERADDR = DT.Rows[0]["RECEIVERADDR"]?.ToString();
                m.RECEIVERPHONE = DT.Rows[0]["RECEIVERPHONE"]?.ToString();
                m.RECEIVERTAXID = DT.Rows[0]["TAXID"]?.ToString();
                m.STATUS = Convert.ToInt32(DT.Rows[0]["STATUS"]);                
                m.PAYDATE = DT.Rows[0]["PAYDATE"]?.ToString();
                m.EXPORTDATE = DT.Rows[0]["EXPORTDATE"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();


                sql = $@"SELECT A.SHIPPINGID_M AS MID, A.ID AS HID, B.ID AS DID, A.CLEARANCENO, A.TRANSFERNO,
                                A.TRACKINGNO, A.DEPOTSTATUS, A.RECEIVER, A.RECEIVERADDR, A.RECEIVERPHONE, A.TAXID,
                                A.WEIGHT, A.TOTALITEM, A.REMARK1, A.REMARK2,
                                B.PRODUCT, B.UNITPRICE, B.QUANTITY
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
                            rows.RECEIVER = DT_Sub.Rows[j]["RECEIVER"]?.ToString();
                            rows.RECEIVERADDR = DT_Sub.Rows[j]["RECEIVERADDR"]?.ToString();
                            rows.RECEIVERPHONE = DT_Sub.Rows[j]["RECEIVERPHONE"]?.ToString();
                            rows.RECEIVERTAXID = DT_Sub.Rows[j]["TAXID"]?.ToString();
                            rows.WEIGHT = DT_Sub.Rows[j]["WEIGHT"]?.ToString();
                            rows.TOTALITEM = DT_Sub.Rows[j]["TOTALITEM"]?.ToString();
                            rows.REMARK1 = DT_Sub.Rows[j]["REMARK1"]?.ToString();
                            rows.REMARK2 = DT_Sub.Rows[j]["REMARK2"]?.ToString();
                            rows.SHIPPINGID_M = Convert.ToInt64(DT_Sub.Rows[j]["MID"]);

                            h.Add(rows);
                        }

                        //新增shipping_D
                        ShippingDCusInfo row = new ShippingDCusInfo();
                        row.PRODUCT = DT_Sub.Rows[j]["PRODUCT"]?.ToString();
                        row.UNITPRICE = DT_Sub.Rows[j]["UNITPRICE"]?.ToString();
                        row.QUANTITY = DT_Sub.Rows[j]["QUANTITY"]?.ToString();
                        row.SHIPPINGID_M = Convert.ToInt64(DT_Sub.Rows[j]["MID"]);
                        row.SHIPPINGID_H = Convert.ToInt64(DT_Sub.Rows[j]["HID"]);
                        d.Add(row);

                        oldHID = Convert.ToInt64(DT_Sub.Rows[j]["HID"]);
                    }
                }

                sql = $@"SELECT SHIPPINGID_M AS MID, SHIPPINGID_H AS HID, ID, NAME, TAXID, PHONE, MOBILE, ADDR, IDPHOTO_F AS IDPHOTOF,
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

        public long InsertCusShippingM(Hashtable sData)
        {
            long ID = 0;
            try
            {
                string autoSeqcode = objComm.GetSeqCode("SHIPPING_CUS");
                sData["SHIPPINGNO"] = "TECTPEEC1" + DateTime.Now.ToString("yy") + autoSeqcode;
                sData["TRACKINGTYPE"] = 0;
                sData["ISMULTRECEIVER"] = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;
                sData["STATUS"] = 0;
                sData["CREDATE"] = DateTime.Now;
                sData["CREATEBY"] = sData["_acccode"];
                sData["UPDDATE"] = sData["CREDATE"];
                sData["UPDBY"] = sData["CREATEBY"];


                string sql = $@"INSERT INTO T_V_SHIPPING_M (ACCOUNTID, SHIPPINGNO, MAWBNO, FLIGHTNUM, TOTAL,
                                                            TOTALWEIGHT, TRACKINGTYPE, RECEIVER, RECEIVERADDR, RECEIVERPHONE,
                                                            TAXID, ISMULTRECEIVER, STATUS, MAWBDATE, CREDATE,
                                                            CREATEBY, UPDDATE, UPDBY)
                                VALUES (@ACCOUNTID, @SHIPPINGNO, @MAWBNO, @FLIGHTNUM, @TOTAL,
                                        @TOTALWEIGHT, @TRACKINGTYPE, @RECEIVER, @RECEIVERADDR, @RECEIVERPHONE,
                                        @TAXID, @ISMULTRECEIVER, @STATUS, @MAWBDATE, @CREDATE,
                                        @CREATEBY, @UPDDATE, @UPDBY)";

                DBUtil.EXECUTE(sql, sData);

                objComm.UpdateSeqCode("SHIPPING_CUS");

                string MID = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_V_SHIPPING_M WHERE SHIPPINGNO = @SHIPPINGNO", sData);
                ID = string.IsNullOrEmpty(MID) ? 0 : Convert.ToInt64(MID);

                return ID;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return ID;
            }
        }

        public long InsertCusShippingH(Hashtable sData)
        {
            long ID = 0;
            try
            {

                string sql = $@"INSERT INTO T_V_SHIPPING_H (CLEARANCENO, TRANSFERNO, RECEIVER, RECEIVERADDR, RECEIVERPHONE,
                                                            TAXID, WEIGHT, TOTALITEM, SHIPPINGID_M)
                                VALUES (@CLEARANCENO, @TRANSFERNO, @RECEIVER, @RECEIVERADDR, @RECEIVERPHONE,
                                                            @TAXID, @WEIGHT, @TOTALITEM, @SHIPPINGIDM)";
                
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
            string sql = $@"INSERT INTO T_V_DECLARANT (NAME, TAXID, PHONE, ADDR, SHIPPINGID_M, SHIPPINGID_H)
                           VALUES (N'{sData["NAME"]}', N'{sData["TAXID"]}', {sData["PHONE"]}, N'{sData["ADDR"]}', {sData["SHIPPINGIDM"]}, {sData["SHIPPINGIDH"]})";

            DBUtil.EXECUTE(sql);

        }

        public void UpdateCusShippingM(Hashtable sData)
        {
            sData["ISMULTRECEIVER"] = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;

            string sql = $@"UPDATE T_V_SHIPPING_M SET
                                                       TOTAL = @TOTAL,
                                                       TOTALWEIGHT = @TOTALWEIGHT,
                                                       ISMULTRECEIVER = @ISMULTRECEIVER,
                                                       RECEIVER = @RECEIVER,
                                                       RECEIVERADDR = @RECEIVERADDR,
                                                       RECEIVERPHONE = @RECEIVERPHONE,
                                                       TAXID = @TAXID
                             WHERE ID = @ID";


            DBUtil.EXECUTE(sql, sData);
        }

        public void UpdateCusShippingH(Hashtable sData)
        {
            
            string sql = $@"UPDATE T_V_SHIPPING_H SET
                                                       WEIGHT = @WEIGHT,
                                                       TOTALITEM = @TOTALITEM
                             WHERE ID = @SHIPPINGIDH";


            DBUtil.EXECUTE(sql, sData);
            
        }

    }
}
