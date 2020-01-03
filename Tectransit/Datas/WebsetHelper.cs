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

        public dynamic GetNewsListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY NEWSSEQ) AS ROW_ID, ID, TITLE, DESCR, NEWSSEQ, UPSDATE, UPEDATE,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE, ISTOP
                                            From T_D_NEWS
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<NewsInfo> rowList = new List<NewsInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    NewsInfo m = new NewsInfo();
                    m.NEWSID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.TITLE = DT.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = DT.Rows[i]["DESCR"]?.ToString();
                    m.NEWSSEQ = DT.Rows[i]["NEWSSEQ"]?.ToString();
                    m.UPSDATE = DT.Rows[i]["UPSDATE"]?.ToString();
                    m.UPEDATE = DT.Rows[i]["UPEDATE"]?.ToString();
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.CREBY = DT.Rows[i]["CREBY"]?.ToString();
                    m.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();
                    m.UPDBY = DT.Rows[i]["UPDBY"]?.ToString();
                    m.ISTOP = Convert.ToBoolean(DT.Rows[i]["ISTOP"]) ? "1" : "0";
                    m.ISENABLE = Convert.ToBoolean(DT.Rows[i]["ISENABLE"]) ? "1" : "0";

                    rowList.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = rowList, total = totalCt };
            }

            return new { rows = "", total = 0 };
        }

        public dynamic GetNewsData(long sID)
        {
            string sql = $@"
                            SELECT ID, TITLE, DESCR,  NEWSSEQ, UPSDATE, UPEDATE,
                                   ISTOP, ISENABLE, FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') AS CREDATE,
                                   FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') AS UPDDATE, CREATEBY AS CREBY, UPDBY
                            FROM T_D_NEWS
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                NewsInfo m = new NewsInfo();
                m.NEWSID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.NEWSSEQ = DT.Rows[0]["BANSEQ"]?.ToString();
                m.TITLE = DT.Rows[0]["TITLE"]?.ToString();
                m.DESCR = DT.Rows[0]["DESCR"]?.ToString();
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
