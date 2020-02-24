using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Tectransit.Datas
{
    public class FrontDataHelper
    {
        public dynamic GetNewsData()
        {
            DataTable datalist = DBUtil.SelectDataTable($@"SELECT ID, TITLE,DESCR, NEWSSEQ, UPSDATE,
                                                                  UPEDATE, ISTOP, FORMAT(CREDATE, 'yyyy-MM-dd') AS CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd') AS UPDDATE
                                                           FROM T_D_NEWS
                                                           WHERE ISENABLE = 'true' AND ((CAST(REPLACE(UPSDATE,'T', ' ') AS datetime) >= GETDATE() AND CAST(REPLACE(UPEDATE,'T', ' ') AS datetime) >= GETDATE()) 
                                                                 OR (UPSDATE = '' AND UPEDATE = ''))
                                                           ORDER BY ISTOP, NEWSSEQ");
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
    }
}
