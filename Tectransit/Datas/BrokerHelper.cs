using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Tectransit.Datas
{
    public class BrokerHelper
    {
        public dynamic GetBrokerData(string sWhere, Hashtable sData, int pageIndex, int pageSize)
        {
            string BRCOL = "";
            if (sData["IMBROKERID"] != null)
                BRCOL = "IMBROKERID";
            else
                BRCOL = "EXBROKERID";

            string sql = $@"SELECT * FROM (
                                            SELECT ID, SHIPPINGNO, MAWBFILE, SHIPPINGFILE1, SHIPPINGFILE2, BROKERFILE1, BROKERFILE2, FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE
                                            FROM T_V_SHIPPING_M 
                                            WHERE {BRCOL} = @{BRCOL} AND STATUS = 0 {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql, sData);
            List<ShippingMCusInfo> rowList = new List<ShippingMCusInfo>();
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    ShippingMCusInfo m = new ShippingMCusInfo();
                    m.ID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.SHIPPINGNO = DT.Rows[i]["SHIPPINGNO"]?.ToString();
                    m.MAWBFILE = DT.Rows[i]["MAWBFILE"]?.ToString();
                    m.SHIPPINGFILE1 = DT.Rows[i]["SHIPPINGFILE1"]?.ToString();
                    m.SHIPPINGFILE2 = DT.Rows[i]["SHIPPINGFILE2"]?.ToString();
                    m.BROKERFILE1 = DT.Rows[i]["BROKERFILE1"]?.ToString();
                    m.BROKERFILE2 = DT.Rows[i]["BROKERFILE2"]?.ToString();
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    
                    rowList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql, sData);

                return new { rows = rowList, total = totalCt };
            }
            
            return new { rows = rowList, total = 0 };
        }

    }
}
