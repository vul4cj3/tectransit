using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Tectransit.Datas;
using System.Collections;
using System;
using Tectransit.Modles;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace Tectransit.Controllers
{
    [Route("api/SysHelp/[action]")]
    public class SysController : Controller
    {
        CommonHelper objComm = new CommonHelper();
        SysHelper objSys = new SysHelper();
        private readonly TECTRANSITDBContext _context;

        public SysController(TECTRANSITDBContext context)
        {
            _context = context;
        }

        /* --- ROLE --- */
        [HttpPost]
        public dynamic GetTSRoleListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");
            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("srolecode", "ROLECODE");
                srhKey.Add("srolename", "ROLENAME");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["ROLECODE"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " ROLECODE LIKE '%" + htData["ROLECODE"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["ROLENAME"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " ROLENAME LIKE '%" + htData["ROLENAME"]?.ToString() + "%'";

            }

            return objSys.GetRoleListData(sWhere, pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public dynamic GetTSRoleData(long id)
        {
            return objSys.GetRoleData(id);
        }

        [HttpPost]
        public dynamic EditTSRoleData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                return EditRoleData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic EditTSRoleEnableData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "ROLEID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "0" : "1";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateRole(Convert.ToInt64(sData["ROLEID"]), sData);
                    }
                }

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        /* --- USER --- */

        [HttpPost]
        public dynamic GetTSUserListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");

            JObject temp = jsonData.Value<JObject>("srhForm");

            if (temp.Count > 0)
            {
                Dictionary<string, string> srhKey = new Dictionary<string, string>();
                srhKey.Add("susercode", "USERCODE");
                srhKey.Add("susername", "USERNAME");
                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[srhKey[t.Key]] = t.Value?.ToString();

                if (!string.IsNullOrEmpty(htData["USERCODE"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " USERCODE LIKE '%" + htData["USERCODE"]?.ToString() + "%'";

                if (!string.IsNullOrEmpty(htData["USERNAME"]?.ToString()))
                    sWhere += (sWhere == "" ? "WHERE" : " AND") + " USERNAME LIKE '%" + htData["USERNAME"]?.ToString() + "%'";

            }

            return objSys.GetUserListData(sWhere, pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public dynamic GetTSUserData(long id)
        {
            return objSys.GetUserData(id);
        }

        [HttpPost]
        public dynamic EditTSUserData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                return EditUserData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }

        [HttpPost]
        public dynamic EditTSUserEnableData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "USERID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "0" : "1";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateUser(Convert.ToInt64(sData["USERID"]), sData);
                    }
                }

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic ResetPassword(long id)
        {
            try
            {
                Hashtable htData = new Hashtable();
                htData["USERPASSWORD"] = objComm.GetMd5Hash("thi_2636");
                htData["ISRESETPW"] = "1";
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                UpdateUser(id, htData);

                return new { status = "0", msg = "重置成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "重置失敗！" };
            }
        }


        /* --- Menu --- */
        [HttpPost]
        public dynamic EditTSMenuVisibleData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "MENUID");
                        dataKey.Add("isenable", "ISVISIBLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateMenu(Convert.ToInt64(sData["MENUID"]), sData);
                    }
                }

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditTSMenuEnableData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "MENUID");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "0" : "1";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();
                    }
                    //get cookies
                    htData["_usercode"] = Request.Cookies["_usercode"];
                    htData["_username"] = Request.Cookies["_username"];

                    AL.Add(htData);
                }


                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        UpdateMenu(Convert.ToInt64(sData["MENUID"]), sData);
                    }
                }

                return new { status = "0", msg = "修改成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "修改失敗！" };
            }
        }
        
        [HttpGet("{id}")]
        public dynamic GetMenuData(long id)
        {
            return objSys.GetMenuData(id);
        }

        [HttpPost]
        public dynamic EditMenuData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                long id = Convert.ToInt64(arrData.Value<string>("id"));
                JObject temp = arrData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in temp)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                //get cookies
                htData["_usercode"] = Request.Cookies["_usercode"];
                htData["_username"] = Request.Cookies["_username"];

                return EditMenuData(id, htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return err;
            }
        }


        /* --- TWOTABLE MAP --- */

        [HttpPost]
        public dynamic EditRoleMenuData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                string rolecode = jsonData.Value<string>("id");
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "MENUCODE");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();

                        htData["ROLECODE"] = rolecode;
                        htData["_usercode"] = Request.Cookies["_usercode"];
                        htData["_username"] = Request.Cookies["_username"];
                    }

                    AL.Add(htData);
                }

                //Insert Or Delete
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        var query = _context.TSRolemenumap.Where(q => q.Rolecode == sData["ROLECODE"].ToString() && q.Menucode == sData["MENUCODE"].ToString()).FirstOrDefault();
                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            if (query == null)
                            {
                                insertRoleMenu(sData);

                                //add user log
                                objComm.AddUserControlLog(sData, $"roles/{rolecode}", "權限管理 - 選單權限", 1, sData["MENUCODE"]?.ToString());
                            }
                        }
                        else
                        {
                            if (query != null)
                            {
                                delRoleMenu(query);

                                //add user log
                                objComm.AddUserControlLog(sData, $"roles/{rolecode}", "權限管理 - 選單權限", 3, sData["MENUCODE"]?.ToString());
                            }

                        }
                    }

                    #region 檢查PARENTCODE的權限是否存在
                    Hashtable sData_ = new Hashtable();
                    sData_["_usercode"] = Request.Cookies["_usercode"];
                    sData_["_username"] = Request.Cookies["_username"];

                    DataTable DT_P = DBUtil.SelectDataTable($@"SELECT MENUCODE FROM T_S_MENU WHERE PARENTCODE = '0'");
                    if (DT_P.Rows.Count > 0)
                    {
                        for (int i = 0; i < DT_P.Rows.Count; i++)
                        {
                            DataTable DT = DBUtil.SelectDataTable($@"SELECT B.PARENTCODE, COUNT(ROLECODE) AS CT FROM T_S_ROLEMENUMAP A
                                                                     LEFT JOIN T_S_MENU B ON A.MENUCODE = B.MENUCODE
                                                                     WHERE A.ROLECODE = '{rolecode}' AND B.PARENTCODE = '{DT_P.Rows[i]["MENUCODE"]}'
                                                                     GROUP BY B.PARENTCODE");

                            if (DT.Rows.Count > 0)
                            {
                                if (Convert.ToInt32(DT.Rows[0]["CT"]) > 0)
                                {
                                    var query = _context.TSRolemenumap.Where(q => q.Rolecode == rolecode && q.Menucode == DT.Rows[0]["PARENTCODE"].ToString()).FirstOrDefault();
                                    if (query == null)
                                    {
                                        Hashtable htData = new Hashtable();
                                        htData["ROLECODE"] = rolecode;
                                        htData["MENUCODE"] = DT.Rows[0]["PARENTCODE"];

                                        insertRoleMenu(htData);

                                        //add user log
                                        objComm.AddUserControlLog(sData_, $"roles/{rolecode}", "權限管理 - 選單權限", 1, htData["MENUCODE"]?.ToString());
                                    }
                                }

                            }
                            else
                            {
                                var query = _context.TSRolemenumap.Where(q => q.Rolecode == rolecode && q.Menucode == DT_P.Rows[i]["MENUCODE"].ToString()).FirstOrDefault();
                                if (query != null)
                                    delRoleMenu(query);

                                //add user log
                                objComm.AddUserControlLog(sData_, $"roles/{rolecode}", "權限管理 - 選單權限", 3, DT_P.Rows[i]["MENUCODE"]?.ToString());
                            }
                        }
                    }




                    #endregion

                }

                return new { status = "99", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        [HttpPost]
        public dynamic EditUserRoleData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                string usercode = jsonData.Value<string>("id");
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];

                    Hashtable htData = new Hashtable();
                    foreach (var t in temp)
                    {
                        Dictionary<string, string> dataKey = new Dictionary<string, string>();
                        dataKey.Add("id", "ROLECODE");
                        dataKey.Add("isenable", "ISENABLE");
                        if (t.Key == "isenable")
                            htData[dataKey[t.Key]] = t.Value?.ToString().ToLower() == "true" ? "1" : "0";
                        else
                            htData[dataKey[t.Key]] = t.Value?.ToString();

                        htData["USERCODE"] = usercode;
                        htData["_usercode"] = Request.Cookies["_usercode"];
                        htData["_username"] = Request.Cookies["_username"];
                    }

                    AL.Add(htData);
                }

                //Insert Or Delete
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        var query = _context.TSUserrolemap.Where(q => q.Rolecode == sData["ROLECODE"].ToString() && q.Usercode == sData["USERCODE"].ToString()).FirstOrDefault();
                        if (sData["ISENABLE"]?.ToString() == "1")
                        {
                            if (query == null)
                            {
                                insertUserRole(sData);

                                //add user log
                                objComm.AddUserControlLog(sData, $"users/{usercode}", "用戶管理 - 權限設定", 1, sData["ROLECODE"]?.ToString());
                            }
                        }
                        else
                        {
                            if (query != null)
                            {
                                delUserRole(query);

                                //add user log
                                objComm.AddUserControlLog(sData, $"users/{usercode}", "權限管理 - 權限設定", 3, sData["ROLECODE"]?.ToString());
                            }

                        }
                    }

                }

                return new { status = "99", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }


        /*private CRUD function*/

        private dynamic EditRoleData(long id, Hashtable htData)
        {
            try
            {
                if (id == 0)
                {
                    //檢查CODE是否重複
                    bool IsRepeat = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT ROLECODE AS COL1 FROM T_S_ROLE WHERE ROLECODE = '{htData["ROLECODE"]}'")) ? false : true;
                    if (IsRepeat)
                        return new { status = "99", msg = "已存在相同的CODE！" };

                    InsertRole(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "roles/edit/0", "權限管理", 1, htData["ROLECODE"]?.ToString());
                }
                else
                {
                    UpdateRole(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"roles/edit/{id}", "權限管理", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertRole(Hashtable sData)
        {
            Hashtable htData = sData;
            int newSeq = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT ROLESEQ AS COL1 FROM T_S_ROLE ORDER BY ROLESEQ DESC")) ? 1 : Convert.ToInt32(DBUtil.GetSingleValue1($@"SELECT ROLESEQ AS COL1 FROM T_S_ROLE ORDER BY ROLESEQ DESC")) + 1;
            htData["ROLESEQ"] = newSeq;
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_S_ROLE(ROLECODE, ROLESEQ, ROLENAME, ROLEDESC, ISENABLE, CREDATE, CREATEBY, UPDDATE, UPDBY) 
                                        VALUES (@ROLECODE, @ROLESEQ, @ROLENAME, @ROLEDESC, @ISENABLE, @CREDATE, @CREATEBY, @UPDDATE, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateRole(long id, Hashtable sData)
        {
            var query = _context.TSRole.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSRole rowTSR = query;

                if (sData["ROLECODE"] != null)
                    rowTSR.Rolecode = sData["ROLECODE"]?.ToString();
                if (sData["ROLESEQ"] != null)
                    rowTSR.Roleseq = sData["ROLESEQ"]?.ToString();
                if (sData["ROLENAME"] != null)
                    rowTSR.Rolename = sData["ROLENAME"]?.ToString();
                if (sData["ROLEDESC"] != null)
                    rowTSR.Roledesc = sData["ROLEDESC"]?.ToString();
                if (sData["ISENABLE"] != null)
                    rowTSR.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTSR.Upddate = DateTime.Now;
                    rowTSR.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void insertRoleMenu(Hashtable sData)
        {
            TSRolemenumap rowRM = new TSRolemenumap();
            rowRM.Rolecode = sData["ROLECODE"]?.ToString();
            rowRM.Menucode = sData["MENUCODE"]?.ToString();

            _context.TSRolemenumap.Add(rowRM);
            _context.SaveChanges();
        }

        private void delRoleMenu(TSRolemenumap rm)
        {
            _context.TSRolemenumap.Remove(rm);
            _context.SaveChanges();
        }

        private dynamic EditUserData(long id, Hashtable htData)
        {
            try
            {
                if (id == 0)
                {
                    //檢查CODE是否重複
                    bool IsRepeat = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_USER WHERE USERCODE = '{htData["USERCODE"]}'")) ? false : true;
                    if (IsRepeat)
                        return new { status = "99", msg = "已存在相同的CODE！" };

                    InsertUser(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "users/edit/0", "用戶管理", 1, htData["USERCODE"]?.ToString());
                }
                else
                {
                    UpdateUser(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"users/edit/{id}", "用戶管理", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertUser(Hashtable sData)
        {
            Hashtable htData = sData;
            int newSeq = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERSEQ AS COL1 FROM T_S_USER ORDER BY USERSEQ DESC")) ? 1 : Convert.ToInt32(DBUtil.GetSingleValue1($@"SELECT USERSEQ AS COL1 FROM T_S_USER ORDER BY USERSEQ DESC")) + 1;
            htData["USERSEQ"] = newSeq;
            htData["USERPASSWORD"] = objComm.GetMd5Hash("thi_2636");
            htData["ISRESETPW"] = true;
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_S_USER(USERCODE, USERPASSWORD, USERSEQ, USERNAME, USERDESC, ISENABLE, ISRESETPW, EMAIL, CREDATE, CREATEBY, UPDDATE, UPDBY) 
                                        VALUES (@USERCODE, @USERPASSWORD, @USERSEQ, @USERNAME, @USERDESC, @ISENABLE, @ISRESETPW, @EMAIL, @CREDATE, @CREATEBY, @UPDDATE, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateUser(long id, Hashtable sData)
        {
            var query = _context.TSUser.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSUser rowTSU = query;

                if (sData["USERCODE"] != null)
                    rowTSU.Usercode = sData["USERCODE"]?.ToString();
                if (sData["USERPASSWORD"] != null)
                    rowTSU.Userpassword = sData["USERPASSWORD"]?.ToString();
                if (sData["USERSEQ"] != null)
                    rowTSU.Userseq = sData["USERSEQ"]?.ToString();
                if (sData["USERNAME"] != null)
                    rowTSU.Username = sData["USERNAME"]?.ToString();
                if (sData["USERDESC"] != null)
                    rowTSU.Userdesc = sData["USERDESC"]?.ToString();
                if (sData["EMAIL"] != null)
                    rowTSU.Email = sData["EMAIL"]?.ToString();
                if (sData["ISENABLE"] != null)
                    rowTSU.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;
                if (sData["ISRESETPW"] != null)
                    rowTSU.Isresetpw = sData["ISRESETPW"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTSU.Upddate = DateTime.Now;
                    rowTSU.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void insertUserRole(Hashtable sData)
        {
            TSUserrolemap rowUR = new TSUserrolemap();
            rowUR.Rolecode = sData["ROLECODE"]?.ToString();
            rowUR.Usercode = sData["USERCODE"]?.ToString();

            _context.TSUserrolemap.Add(rowUR);
            _context.SaveChanges();
        }

        private void delUserRole(TSUserrolemap rm)
        {
            _context.TSUserrolemap.Remove(rm);
            _context.SaveChanges();
        }

        private dynamic EditMenuData(long id, Hashtable htData)
        {
            try
            {
                if (id == 0)
                {
                    //檢查CODE是否重複
                    bool IsRepeat = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT MENUCODE AS COL1 FROM T_S_MENU WHERE MENUCODE = '{htData["MENUCODE"]}'")) ? false : true;
                    if (IsRepeat)
                        return new { status = "99", msg = "已存在相同的CODE！" };

                    InsertMenu(htData);

                    //add user log
                    objComm.AddUserControlLog(htData, "menu/edit/0", "選單管理", 1, htData["MENUCODE"]?.ToString());
                }
                else
                {
                    UpdateMenu(id, htData);

                    string updMsg = "";
                    foreach (DictionaryEntry ht in htData)
                    {
                        if (ht.Key.ToString() == "_usercode" || ht.Key.ToString() == "_username") { }
                        else
                            updMsg += (updMsg == "" ? "" : ",") + ht.Key + ":" + ht.Value;
                    }

                    //add user log
                    objComm.AddUserControlLog(htData, $"menu/edit/{id}", "選單管理", 2, updMsg);
                }

                return new { status = "0", msg = "保存成功！" };

            }
            catch (Exception ex)
            {
                string err = ex.Message?.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        private void InsertMenu(Hashtable sData)
        {
            Hashtable htData = sData;
            htData["ISBACK"] = htData["ISBACK"]?.ToString() == "1" ? true : false;
            htData["ISVISIBLE"] = htData["ISVISIBLE"]?.ToString() == "1" ? true : false;
            htData["ISENABLE"] = htData["ISENABLE"]?.ToString() == "1" ? true : false;
            htData["CREDATE"] = DateTime.Now;
            htData["CREATEBY"] = sData["_usercode"];
            htData["UPDDATE"] = htData["CREDATE"];
            htData["UPDBY"] = htData["CREATEBY"];

            string sql = @"INSERT INTO T_S_MENU(MENUCODE, PARENTCODE, MENUURL, MENUSEQ, MENUNAME, MENUDESC, ISBACK, ISVISIBLE, ISENABLE, ICONURL, CREDATE, CREATEBY, UPDDATE, UPDBY) 
                                        VALUES (@MENUCODE, @PARENTCODE, @MENUURL, @MENUSEQ, @MENUNAME, @MENUDESC, @ISBACK, @ISVISIBLE, @ISENABLE, @ICONURL, @CREDATE, @CREATEBY, @UPDDATE, @UPDBY)";

            DBUtil.EXECUTE(sql, htData);
        }

        private void UpdateMenu(long id, Hashtable sData)
        {
            var query = _context.TSMenu.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSMenu rowTSM = query;

                if (sData["MENUCODE"] != null)
                    rowTSM.Menucode = sData["MENUCODE"]?.ToString();
                if (sData["PARENTCODE"] != null)
                    rowTSM.Parentcode = sData["PARENTCODE"]?.ToString();
                if (sData["MENUNAME"] != null)
                    rowTSM.Menuname = sData["MENUNAME"]?.ToString();
                if (sData["MENUSEQ"] != null)
                    rowTSM.Menuseq = sData["MENUSEQ"]?.ToString();
                if (sData["MENUDESC"] != null)
                    rowTSM.Menudesc = sData["MENUDESC"]?.ToString();
                if (sData["MENUURL"] != null)
                    rowTSM.Menuurl = sData["MENUURL"]?.ToString();
                if (sData["ICONURL"] != null)
                    rowTSM.Iconurl = sData["ICONURL"]?.ToString();
                if (sData["ISBACK"] != null)
                    rowTSM.Isback = sData["ISBACK"]?.ToString() == "1" ? true : false;
                if (sData["ISENABLE"] != null)
                    rowTSM.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;
                if (sData["ISVISIBLE"] != null)
                    rowTSM.Isvisible = sData["ISVISIBLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTSM.Upddate = DateTime.Now;
                    rowTSM.Updby = sData["_usercode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

    }
}