using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
                    m.DESCR = HttpUtility.HtmlDecode(DT.Rows[i]["DESCR"]?.ToString());
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
                m.NEWSSEQ = DT.Rows[0]["NEWSSEQ"]?.ToString();
                m.TITLE = DT.Rows[0]["TITLE"]?.ToString();
                m.DESCR = HttpUtility.HtmlDecode(DT.Rows[0]["DESCR"]?.ToString());
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

        public dynamic GetAboutCateListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY ABOUTHSEQ) AS ROW_ID, ID, TITLE, DESCR, ABOUTHSEQ,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE, ISTOP
                                            From T_D_ABOUT_H
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<AboutCate> rowList = new List<AboutCate>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    AboutCate m = new AboutCate();
                    m.CATEID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.TITLE = DT.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = DT.Rows[i]["DESCR"]?.ToString();
                    m.ABOUTSEQ = DT.Rows[i]["ABOUTHSEQ"]?.ToString();
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

        public dynamic GetTDAboutCateData(string sWhere)
        {
            string sql = $@"SELECT ROW_NUMBER() OVER (ORDER BY ABOUTHSEQ) AS ROW_ID, ID, TITLE, DESCR, ABOUTHSEQ,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE, ISTOP
                                            From T_D_ABOUT_H
                                            {sWhere}";            
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                List<AboutCate> rowList = new List<AboutCate>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    AboutCate m = new AboutCate();
                    m.CATEID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.TITLE = DT.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = DT.Rows[i]["DESCR"]?.ToString();
                    m.ABOUTSEQ = DT.Rows[i]["ABOUTHSEQ"]?.ToString();
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

        public dynamic GetAboutCateData(long sID)
        {
            string sql = $@"
                            SELECT ID, TITLE, DESCR,  ABOUTHSEQ,
                                   ISTOP, ISENABLE, FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') AS CREDATE,
                                   FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') AS UPDDATE, CREATEBY AS CREBY, UPDBY
                            FROM T_D_ABOUT_H
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                AboutCate m = new AboutCate();
                m.CATEID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.ABOUTSEQ = DT.Rows[0]["ABOUTHSEQ"]?.ToString();
                m.TITLE = DT.Rows[0]["TITLE"]?.ToString();
                m.DESCR = DT.Rows[0]["DESCR"]?.ToString();
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

        public dynamic GetAboutListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY ABOUTDSEQ) AS ROW_ID, ID, TITLE, DESCR, ABOUTDSEQ, ABOUTHID AS CATEID,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE, ISTOP
                                            From T_D_ABOUT_D
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<AboutInfo> rowList = new List<AboutInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    AboutInfo m = new AboutInfo();
                    m.ABOUTID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.CATEID = DT.Rows[i]["CATEID"]?.ToString();
                    m.TITLE = DT.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = DT.Rows[i]["DESCR"]?.ToString();
                    m.ABOUTSEQ = DT.Rows[i]["ABOUTDSEQ"]?.ToString();
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

        public dynamic GetAboutData(long sID)
        {
            string sql = $@"
                            SELECT ID, TITLE, DESCR,  ABOUTDSEQ, ABOUTHID AS CATEID,
                                   ISTOP, ISENABLE, FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') AS CREDATE,
                                   FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') AS UPDDATE, CREATEBY AS CREBY, UPDBY
                            FROM T_D_ABOUT_D
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                AboutInfo m = new AboutInfo();
                m.ABOUTID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.CATEID = DT.Rows[0]["CATEID"]?.ToString();
                m.ABOUTSEQ = DT.Rows[0]["ABOUTDSEQ"]?.ToString();
                m.TITLE = DT.Rows[0]["TITLE"]?.ToString();
                m.DESCR = HttpUtility.HtmlDecode(DT.Rows[0]["DESCR"]?.ToString());
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

        public dynamic GetFaqCateListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY FAQHSEQ) AS ROW_ID, ID, TITLE, DESCR, FAQHSEQ,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE, ISTOP
                                            From T_D_FAQ_H
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<FaqCate> rowList = new List<FaqCate>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    FaqCate m = new FaqCate();
                    m.CATEID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.TITLE = DT.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = DT.Rows[i]["DESCR"]?.ToString();
                    m.FAQSEQ = DT.Rows[i]["FAQHSEQ"]?.ToString();
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

        public dynamic GetTDFaqCateData(string sWhere)
        {
            string sql = $@"SELECT ROW_NUMBER() OVER (ORDER BY FAQHSEQ) AS ROW_ID, ID, TITLE, DESCR, FAQHSEQ,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE, ISTOP
                                            From T_D_FAQ_H
                                            {sWhere}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                List<FaqCate> rowList = new List<FaqCate>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    FaqCate m = new FaqCate();
                    m.CATEID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.TITLE = DT.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = DT.Rows[i]["DESCR"]?.ToString();
                    m.FAQSEQ = DT.Rows[i]["FAQHSEQ"]?.ToString();
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

        public dynamic GetFaqCateData(long sID)
        {
            string sql = $@"
                            SELECT ID, TITLE, DESCR,  FAQHSEQ,
                                   ISTOP, ISENABLE, FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') AS CREDATE,
                                   FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') AS UPDDATE, CREATEBY AS CREBY, UPDBY
                            FROM T_D_FAQ_H
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                FaqCate m = new FaqCate();
                m.CATEID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.FAQSEQ = DT.Rows[0]["FAQHSEQ"]?.ToString();
                m.TITLE = DT.Rows[0]["TITLE"]?.ToString();
                m.DESCR = DT.Rows[0]["DESCR"]?.ToString();
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

        public dynamic GetFaqListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY FAQDSEQ) AS ROW_ID, ID, TITLE, DESCR, FAQDSEQ, FAQHID AS CATEID,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY, ISENABLE, ISTOP
                                            From T_D_FAQ_D
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<FaqInfo> rowList = new List<FaqInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    FaqInfo m = new FaqInfo();
                    m.FAQID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.CATEID = DT.Rows[i]["CATEID"]?.ToString();
                    m.TITLE = DT.Rows[i]["TITLE"]?.ToString();
                    m.DESCR = DT.Rows[i]["DESCR"]?.ToString();
                    m.FAQSEQ = DT.Rows[i]["FAQDSEQ"]?.ToString();
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

        public dynamic GetFaqData(long sID)
        {
            string sql = $@"
                            SELECT ID, TITLE, DESCR,  FAQDSEQ, FAQHID AS CATEID,
                                   ISTOP, ISENABLE, FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') AS CREDATE,
                                   FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') AS UPDDATE, CREATEBY AS CREBY, UPDBY
                            FROM T_D_FAQ_D
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                FaqInfo m = new FaqInfo();
                m.FAQID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.CATEID = DT.Rows[0]["CATEID"]?.ToString();
                m.FAQSEQ = DT.Rows[0]["FAQDSEQ"]?.ToString();
                m.TITLE = DT.Rows[0]["TITLE"]?.ToString();
                m.DESCR = HttpUtility.HtmlDecode(DT.Rows[0]["DESCR"]?.ToString());
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

        public dynamic GetStationListData(string sWhere, int pageIndex, int pageSize)
        {
            string sql = $@"SELECT * FROM (
                                            SELECT ROW_NUMBER() OVER (ORDER BY STATIONSEQ) AS ROW_ID, ID, STATIONCODE, STATIONNAME, COUNTRYCODE, RECEIVER,
                                                   PHONE, MOBILE, ADDRESS, STATIONSEQ, REMARK,
                                                   FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') As CREDATE, FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') As UPDDATE,
                                                   CREATEBY AS CREBY, UPDBY
                                            From T_S_STATION
                                            {sWhere}
                            ) AS A";
            string sql1 = sql + $@" WHERE ROW_ID BETWEEN {((pageIndex - 1) * pageSize + 1).ToString()} AND {(pageIndex * pageSize).ToString()}";
            DataTable DT = DBUtil.SelectDataTable(sql1);
            if (DT.Rows.Count > 0)
            {
                List<StationInfo> rowList = new List<StationInfo>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    StationInfo m = new StationInfo();
                    m.STATIONID = Convert.ToInt64(DT.Rows[i]["ID"]);
                    m.STATIONCODE = DT.Rows[i]["STATIONCODE"]?.ToString();
                    m.STATIONNAME = DT.Rows[i]["STATIONNAME"]?.ToString();
                    m.COUNTRYCODE = DT.Rows[i]["COUNTRYCODE"]?.ToString();
                    m.RECEIVER = DT.Rows[i]["RECEIVER"]?.ToString();
                    m.PHONE = DT.Rows[i]["PHONE"]?.ToString();
                    m.MOBILE = DT.Rows[i]["MOBILE"]?.ToString();
                    m.ADDRESS = DT.Rows[i]["ADDRESS"]?.ToString();
                    m.STATIONSEQ = DT.Rows[i]["STATIONSEQ"]?.ToString();
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

        public dynamic GetStationData(long sID)
        {
            string sql = $@"
                            SELECT ID, STATIONCODE, STATIONNAME,  COUNTRYCODE, RECEIVER, PHONE,
                                   MOBILE, ADDRESS, STATIONSEQ, REMARK, FORMAT(CREDATE, 'yyyy-MM-dd HH:mm:ss') AS CREDATE,
                                   FORMAT(UPDDATE, 'yyyy-MM-dd HH:mm:ss') AS UPDDATE, CREATEBY AS CREBY, UPDBY
                            FROM T_S_STATION
                            WHERE ID = {sID}";
            DataTable DT = DBUtil.SelectDataTable(sql);
            if (DT.Rows.Count > 0)
            {
                StationInfo m = new StationInfo();
                m.STATIONID = Convert.ToInt64(DT.Rows[0]["ID"]);
                m.STATIONSEQ = DT.Rows[0]["STATIONSEQ"]?.ToString();
                m.STATIONCODE = DT.Rows[0]["STATIONCODE"]?.ToString();
                m.STATIONNAME = DT.Rows[0]["STATIONNAME"]?.ToString();
                m.COUNTRYCODE = DT.Rows[0]["COUNTRYCODE"]?.ToString();
                m.RECEIVER = DT.Rows[0]["RECEIVER"]?.ToString();
                m.PHONE = DT.Rows[0]["PHONE"]?.ToString();
                m.MOBILE = DT.Rows[0]["MOBILE"]?.ToString();
                m.ADDRESS = DT.Rows[0]["ADDRESS"]?.ToString();
                m.RECEIVER = DT.Rows[0]["RECEIVER"]?.ToString();
                m.REMARK = DT.Rows[0]["REMARK"]?.ToString();
                m.CREDATE = DT.Rows[0]["CREDATE"]?.ToString();
                m.CREBY = DT.Rows[0]["CREBY"]?.ToString();
                m.UPDDATE = DT.Rows[0]["UPDDATE"]?.ToString();
                m.UPDBY = DT.Rows[0]["UPDBY"]?.ToString();

                return new { rows = m };
            }

            return new { rows = "" };
        }
    }
}
