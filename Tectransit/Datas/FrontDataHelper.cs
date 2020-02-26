using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Tectransit.Datas
{
    public class FrontDataHelper
    {
        // 抓取全部NewsData
        public dynamic GetNewsData()
        {
            DataTable datalist = DBUtil.SelectDataTable($@"SELECT ID, TITLE,DESCR, NEWSSEQ, UPSDATE,
                                                                  UPEDATE, ISTOP, FORMAT(CREDATE, 'yyyy-MM-dd') AS CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd') AS UPDDATE
                                                           FROM T_D_NEWS
                                                           WHERE ISENABLE = 'true' AND ((CAST(REPLACE(UPSDATE,'T', ' ') AS datetime) >= GETDATE() AND CAST(REPLACE(UPEDATE,'T', ' ') AS datetime) >= GETDATE()) 
                                                                 OR (UPSDATE = '' AND UPEDATE = ''))
                                                           ORDER BY ISTOP, UPDDATE");
            if (datalist.Rows.Count > 0)
            {
                List<NewsInfo> rowlist = new List<NewsInfo>();
                for (int i = 0; i < datalist.Rows.Count; i++)
                {
                    NewsInfo m = new NewsInfo();
                    m.NEWSID = Convert.ToInt64(datalist.Rows[i]["ID"]);
                    m.TITLE = datalist.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = datalist.Rows[i]["DESCR"]?.ToString();
                    m.NEWSSEQ = datalist.Rows[i]["NEWSSEQ"]?.ToString();
                    m.UPSDATE = datalist.Rows[i]["UPSDATE"]?.ToString();
                    m.UPEDATE = datalist.Rows[i]["UPEDATE"]?.ToString();
                    m.ISTOP = Convert.ToBoolean(datalist.Rows[i]["ISTOP"]) == true ? "1" : "0";
                    m.CREDATE = datalist.Rows[i]["CREDATE"]?.ToString();
                    m.UPDDATE = datalist.Rows[i]["UPDDATE"]?.ToString();

                    rowlist.Add(m);
                }

                return new { status = "0", row = rowlist };
            }

            return new { status = "99", row = "" };

        }

        // 抓取全部NewsData(分頁)
        public dynamic GetNewsData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                             SELECT ROW_NUMBER() OVER (ORDER BY ISTOP, UPDDATE) AS ROW_ID, ID, TITLE,DESCR, NEWSSEQ, UPSDATE,
                                                    UPEDATE, ISTOP, FORMAT(CREDATE, 'yyyy-MM-dd') AS CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd') AS UPDDATE
                                             FROM T_D_NEWS
                                             WHERE ISENABLE = 'true' AND ((CAST(REPLACE(UPSDATE,'T', ' ') AS datetime) >= GETDATE() AND CAST(REPLACE(UPEDATE,'T', ' ') AS datetime) >= GETDATE()) 
                                                   OR (UPSDATE = '' AND UPEDATE = ''))
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<NewsInfo> rowlist = new List<NewsInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    NewsInfo m = new NewsInfo();
                    m.NEWSID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.TITLE = DT.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = DT.Rows[i]["DESCR"]?.ToString();
                    m.NEWSSEQ = DT.Rows[i]["NEWSSEQ"]?.ToString();
                    m.UPSDATE = DT.Rows[i]["UPSDATE"]?.ToString();
                    m.UPEDATE = DT.Rows[i]["UPEDATE"]?.ToString();
                    m.ISTOP = Convert.ToBoolean(DT.Rows[i]["ISTOP"]) == true ? "1" : "0";
                    m.CREDATE = DT.Rows[i]["CREDATE"]?.ToString();
                    m.UPDDATE = DT.Rows[i]["UPDDATE"]?.ToString();

                    rowlist.Add(m);
                }

                sql = "SELECT COUNT(*) as COL1 FROM (" + sql + ") AS B ";
                string totalCt = DBUtil.GetSingleValue1(sql);

                return new { rows = rowlist, total = totalCt };
            }

            return new { rows = "", total = 0 };
        }

        // 抓取單筆NewsData
        public dynamic GetNews(long id)
        {
            string sql = $@"SELECT ID, TITLE, DESCR, NEWSSEQ, UPSDATE,
                                   UPEDATE, ISTOP, FORMAT(CREDATE, 'yyyy-MM-dd') AS CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd') AS UPDDATE
                            FROM T_D_NEWS
                            WHERE ID = {id} AND ISENABLE = 'true'";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                NewsInfo m = new NewsInfo();
                m.NEWSID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.TITLE = DT.Rows[0]["TITLE"]?.ToString();
                m.DESCR = HttpUtility.HtmlDecode(DT.Rows[0]["DESCR"]?.ToString());
                m.NEWSSEQ = DT.Rows[0]["NEWSSEQ"]?.ToString();
                m.UPSDATE = DT.Rows[0]["UPSDATE"]?.ToString();
                m.UPEDATE = DT.Rows[0]["UPEDATE"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();

                return new { rows = m };
            }

            return new { rows = "" };
        }
    }
}
