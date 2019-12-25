using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Tectransit.Datas
{
    public class CommonHelper
    {
        //取得後台選單
        public dynamic GetMenu(string USERCODE)
        {
            //Get Role
            string sRolelist = "";
            DataTable dtRoles = DBUtil.SelectDataTable($@"SELECT A.ROLECODE FROM T_S_ROLE A 
                                                         LEFT JOIN T_S_USERROLEMAP B ON A.ROLECODE = B.ROLECODE
                                                         WHERE B.USERCODE = '{USERCODE}' AND A.ISENABLE = 'true'");
            if (dtRoles.Rows.Count > 0)
            {
                for (int i = 0; i < dtRoles.Rows.Count; i++)
                    sRolelist += (sRolelist == "" ? "" : ",") + "'" + dtRoles.Rows[i]["ROLECODE"] + "'";

                DataTable dtMenulist = DBUtil.SelectDataTable($@"SELECT DISTINCT A.MENUSEQ, A.MENUCODE, A.PARENTCODE,A.MENUURL, A.MENUNAME, A.ICONURL FROM T_S_MENU A
                                                                 LEFT JOIN T_S_ROLEMENUMAP B ON A.MENUCODE = B.MENUCODE
                                                                 WHERE B.ROLECODE IN ({sRolelist}) AND A.ISBACK = 'true' AND ISVISIBLE = 'true' AND ISENABLE = 'true'
                                                                 ORDER BY A.MENUSEQ");
                if (dtMenulist.Rows.Count > 0)
                {
                    List<MenuInfo> pmenuList = new List<MenuInfo>();
                    List<MenuInfo> dmenuList = new List<MenuInfo>();
                    for (int i=0; i<dtMenulist.Rows.Count;i++)
                    {
                        MenuInfo m = new MenuInfo();
                        m.MENUCODE = dtMenulist.Rows[i]["MENUCODE"]?.ToString();
                        m.PARENTCODE = dtMenulist.Rows[i]["PARENTCODE"]?.ToString();
                        m.MENUURL = dtMenulist.Rows[i]["MENUURL"]?.ToString();
                        m.MENUNAME = dtMenulist.Rows[i]["MENUNAME"]?.ToString();
                        m.ICONURL = dtMenulist.Rows[i]["ICONURL"]?.ToString();

                        if (m.PARENTCODE == "0")
                            pmenuList.Add(m);
                        else
                            dmenuList.Add(m);
                    }

                    return new {status = "0", pList = pmenuList, item = dmenuList };
                }

                return new { status = "99", pList = "", item = "" };
            }
            else
                return new { status = "99", pList = "", item = "" };

        }        

    }


    public class MenuInfo
    {
        public string MENUCODE { set; get; }
        public string PARENTCODE { set; get; }
        public string MENUURL { set; get; }
        public string MENUNAME { set; get; }
        public string ICONURL { set; get; }
    }

}
