using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Tectransit.Datas
{
    public class WebsetHelper
    {
        public dynamic GetBanData(long sID)
        {
            string sql = $@"
                            SELECT ID, TITLE, DESCR,  IMGURL, URL, BANSEQ, UPSDATE, UPEDATE,
                                   ISTOP, ISENABLE, FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') AS CREDATE,
                                   FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') AS UPDDATE, CREATEBY AS CREBY, UPDBY
                            FROM T_D_BANNER
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                BannerInfo m = new BannerInfo();
                m.BANID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.BANSEQ = DT.Rows[0]["BANSEQ"]?.ToString();
                m.TITLE = DT.Rows[0]["TITLE"]?.ToString();
                m.DESCR = DT.Rows[0]["DESCR"]?.ToString();
                m.IMGURL = DT.Rows[0]["IMGURL"]?.ToString();
                m.URL = DT.Rows[0]["URL"]?.ToString();
                m.UPSDATE = DT.Rows[0]["UPSDATE"]?.ToString();
                m.UPEDATE = DT.Rows[0]["UPEDATE"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();
                m.ISTOP = Convert.ToBoolean(DT.Rows[0]["ISTOP"]) ? "1" : "0";
                m.ISENABLE = Convert.ToBoolean(DT.Rows[0]["ISENABLE"]) ? "1" : "0";

                return new { rows = m };
            }

            return new { rows = "" };
        }

    }
}
