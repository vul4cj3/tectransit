using System;
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

        public dynamic GetTransferData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY UPDDATE) AS ROW_ID, ID, STATIONCODE, TRASFERNO, TRASFERCOMPANY,
                                                   P_LENGTH, P_WIDTH, P_HEIGHT, P_WEIGHT, P_VALUEPRICE, REMARK,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, STATUS
                                            From T_E_TRANSFER_H
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<TransferHInfo> rowList = new List<TransferHInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    TransferHInfo m = new TransferHInfo();
                    m.TRANSID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.STATIONCODE = DT.Rows[i]["STATIONCODE"]?.ToString();
                    m.TRASFERNO = DT.Rows[i]["TRASFERNO"]?.ToString();
                    m.TRASFERCOMPANY = DT.Rows[i]["TRASFERCOMPANY"]?.ToString();
                    m.PLENGTH = DT.Rows[i]["P_LENGTH"]?.ToString();
                    m.PWIDTH = DT.Rows[i]["P_WIDTH"]?.ToString();
                    m.PHEIGHT = DT.Rows[i]["P_HEIGHT"]?.ToString();
                    m.PWEIGHT = DT.Rows[i]["P_WEIGHT"]?.ToString();
                    m.PVALUEPRICE = DT.Rows[i]["P_VALUEPRICE"]?.ToString();
                    m.STATUS = DT.Rows[i]["STATUS"]?.ToString();
                    m.REMARK = DT.Rows[i]["REMARK"]?.ToString();
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
                                                   P_LENGTH, P_WIDTH, P_HEIGHT, P_WEIGHT, P_TRACKINGNO, TOTAL, RECEIVER, RECEIVER_ADDR, TRACKINGTYPE, STATUS,
                                                   PAYTYPE, PAYSTATUS, REMARK1, REMARK2, REMARK3, FORMAT(PAYDATE, 'yyyy-MM-dd HH:mm:ss') AS PAYDATE, FORMAT(EXPORTDATE, 'yyyy-MM-dd HH:mm:ss') AS EXPORTDATE,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, STATUS
                                            From T_N_SHIPPING_H
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<ShippingHInfo> rowList = new List<ShippingHInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    ShippingHInfo m = new ShippingHInfo();
                    m.SHIPPINGID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.SHIPPINGNO = DT.Rows[i]["SHIPPINGNO"]?.ToString();
                    m.STATIONCODE = DT.Rows[i]["STATIONCODE"]?.ToString();
                    m.TRACKINGNO = DT.Rows[i]["TRACKINGNO"]?.ToString();
                    m.TRACKINGDESC = DT.Rows[i]["TRACKINGDESC"]?.ToString();
                    m.TRACKINGREMARK = DT.Rows[i]["TRACKINGREMARK"]?.ToString();
                    m.PLENGTH = DT.Rows[i]["P_LENGTH"]?.ToString();
                    m.PWIDTH = DT.Rows[i]["P_WIDTH"]?.ToString();
                    m.PHEIGHT = DT.Rows[i]["P_HEIGHT"]?.ToString();
                    m.PWEIGHT = DT.Rows[i]["P_WEIGHT"]?.ToString();
                    m.PTRACKINGNO = DT.Rows[i]["P_TRACKINGNO"]?.ToString();
                    m.TOTAL = DT.Rows[i]["TOTAL"]?.ToString();
                    m.RECEIVER = DT.Rows[i]["RECEIVER"]?.ToString();
                    m.RECEIVER_ADDR = DT.Rows[i]["RECEIVER_ADDR"]?.ToString();
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

        public dynamic GetACTransferData(long sID)
        {
            string sql = $@"
                           SELECT A.*, B.*, C.USERCODE AS ACCOUNTCODE, D.STATIONNAME FROM T_E_TRANSFER_H A
                           LEFT JOIN T_E_TRANSFER_D B ON A.ID = B.TRANSFERHID
                           LEFT JOIN T_S_ACCOUNT C ON A.ACCOUNTID = C.ID
                           LEFT JOIN T_S_STATION D ON A.STATIONCODE = D.STATIONCODE
                           WHERE A.ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            List<TransferDInfo> sublist = new List<TransferDInfo>();
            if (DT.Rows.Count > 0)
            {
                TransferHListInfo m = new TransferHListInfo();
                m.TRANSID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.STATIONNAME = DT.Rows[0]["STATIONNAME"]?.ToString();
                m.ACCOUNTCODE = DT.Rows[0]["ACCOUNTCODE"]?.ToString();
                m.TRASFERNO = DT.Rows[0]["TRASFERNO"]?.ToString();
                m.TRASFERCOMPANY = DT.Rows[0]["TRASFERCOMPANY"]?.ToString();
                m.PLENGTH = DT.Rows[0]["P_LENGTH"]?.ToString();
                m.PWIDTH = DT.Rows[0]["P_WIDTH"]?.ToString();
                m.PHEIGHT = DT.Rows[0]["P_HEIGHT"]?.ToString();
                m.PWEIGHT = DT.Rows[0]["P_WEIGHT"]?.ToString();
                m.STATUS = DT.Rows[0]["STATUS"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREATEBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();


                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    TransferDInfo d = new TransferDInfo();
                    d.PRODUCT = DT.Rows[i]["PRODUCT"]?.ToString();
                    d.QUANTITY = DT.Rows[i]["QUANTITY"]?.ToString();
                    d.UNITPRICE = DT.Rows[i]["UNIT_PRICE"]?.ToString();
                    sublist.Add(d);
                }
                return new { rows = m, subitem = sublist };
            }

            return new { rows = "", subitem = "" };
        }

        public void DelACTransferData(Hashtable sData)
        {
            string sql = $@"DELETE FROM T_E_TRANSFER_D WHERE TRANSFERHID = @TRANSFERID";
            string sql2 = $@"DELETE FROM T_E_TRANSFER_H WHERE ID = @TRANSFERID";
            if(sData["ISENABLE"]?.ToString() == "1")
            {
                DBUtil.EXECUTE(sql, sData);
                DBUtil.EXECUTE(sql2, sData);                
            }            
        }

    }
}
