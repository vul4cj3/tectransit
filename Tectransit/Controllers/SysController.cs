using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Tectransit.Datas;
using System.Collections;
using System;
using Tectransit.Modles;
using System.Transactions;
using System.Linq;
using System.Collections.Generic;

namespace Tectransit.Controllers
{
    [Route("api/SysHelp/[action]")]
    public class SysController : Controller
    {
        SysHelper objSys = new SysHelper();
        private readonly TECTRANSITDBContext _context;

        public SysController(TECTRANSITDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public dynamic GetTSRoleListData([FromBody] object form)
        {
            string sWhere = "";
            var jsonData = JObject.FromObject(form);
            int pageIndex = jsonData.Value<int>("PAGE_INDEX");
            int pageSize = jsonData.Value<int>("PAGE_SIZE");

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

        public dynamic EditRoleData(long id, Hashtable htData)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    if (id == 0) //新增
                        InsertRole(htData);
                    else
                        UpdateRole(id, htData);

                    ts.Complete();
                    return new { status = "0", msg = "保存成功！" };
                }
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
    }
}