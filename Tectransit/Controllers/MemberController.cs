﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Tectransit.Datas;
using Tectransit.Modles;

namespace Tectransit.Controllers
{
    [Route("api/Member/[action]")]
    public class MemberController : Controller
    {
        CommonHelper objComm = new CommonHelper();
        MemberHelper objMember = new MemberHelper();
        private readonly TECTRANSITDBContext _context;

        public MemberController(TECTRANSITDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public dynamic GetMemData()
        {
            Hashtable htData = new Hashtable();
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            return objMember.GetMemData(htData);
        }

        [HttpPost]
        public dynamic EditMemData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                long id = Convert.ToInt64(jsonData.Value<string>("id"));
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();


                //檢查舊密碼是否符合(不符合則不可修改)
                if (!string.IsNullOrEmpty(htData["USERPASSWORD"]?.ToString()) && !string.IsNullOrEmpty(htData["NEWPW"]?.ToString()))
                {
                    string oldPW = DBUtil.GetSingleValue1($@"SELECT USERPASSWORD AS COL1 FROM T_S_ACCOUNT WHERE ID = {id}");
                    if (objComm.GetMd5Hash(htData["USERPASSWORD"]?.ToString()) != oldPW)
                        return new { status = "99", msg = "保存失敗，用戶密碼(舊密碼)輸入錯誤！" };
                }


                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                UpdateMemData(id, htData);

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "0", msg = "保存失敗！" };
            }
        }

        [HttpGet]
        public dynamic GetCusMemData()
        {
            Hashtable htData = new Hashtable();
            htData["_cuscode"] = Request.Cookies["_cuscode"];
            htData["_cusname"] = Request.Cookies["_cusname"];

            return objMember.GetCusMemData(htData);
        }

        [HttpPost]
        public dynamic EditCusMemData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                long id = Convert.ToInt64(jsonData.Value<string>("id"));
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();


                //檢查舊密碼是否符合(不符合則不可修改)
                if (!string.IsNullOrEmpty(htData["USERPASSWORD"]?.ToString()) && !string.IsNullOrEmpty(htData["NEWPW"]?.ToString()))
                {
                    string oldPW = DBUtil.GetSingleValue1($@"SELECT USERPASSWORD AS COL1 FROM T_S_ACCOUNT WHERE ID = {id}");
                    if (objComm.GetMd5Hash(htData["USERPASSWORD"]?.ToString()) != oldPW)
                        return new { status = "99", msg = "保存失敗，用戶密碼(舊密碼)輸入錯誤！" };
                }


                //get cookies
                htData["_cuscode"] = Request.Cookies["_cuscode"];
                htData["_cusname"] = Request.Cookies["_cusname"];

                UpdateCusMemData(id, htData);

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "0", msg = "保存失敗！" };
            }
        }

        [HttpPost]
        public dynamic SaveRegData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                htData["RANDONCODE"] = objComm.GetRandomString();

                //檢查用戶名是否重複
                bool IsRepeat = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["USERCODE"]}' AND ISENABLE = '1'")) ? false : true;
                if (IsRepeat)
                {
                    return new { status = "99", id = "", msg = "此用戶帳號已有人使用！" }; ;
                }


                InsertMemData(htData);
                SendEmailAccept(htData);

                string userid = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["USERCODE"]}' ORDER BY CREDATE DESC");

                return new { status = "0", id = userid, msg = "註冊成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", id = "0", msg = "註冊失敗！" };
            }
        }

        [HttpPost]
        public dynamic SaveRegCompanyData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                htData["RANDONCODE"] = objComm.GetRandomString();

                //檢查用戶名是否重複
                bool IsRepeat = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT USERCODE AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["USERCODE"]}' AND ISENABLE = '1'")) ? false : true;
                if (IsRepeat)
                {
                    return new { status = "99", id = "", msg = "此用戶帳號已有人使用！" }; ;
                }


                InsertMemCompanyData(htData);
                SendEmailAccept(htData);

                string userid = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["USERCODE"]}' ORDER BY CREDATE DESC");

                return new { status = "0", id = userid, msg = "註冊成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", id = "0", msg = "註冊失敗！" };
            }
        }

        [HttpPost]
        public dynamic ConfirmEmailCode([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                long id = Convert.ToInt64(htData["USERID"]?.ToString());

                string EMAILCODE = DBUtil.GetSingleValue1($@"SELECT USERDESC AS COL1 FROM T_S_ACCOUNT WHERE ID = {id}");

                //get cookies
                htData["_acccode"] = "SYSTEM";
                htData["_accname"] = "SYSTEM";

                if (EMAILCODE.Trim() == (htData["EMAILCODE"]?.ToString()).Trim())
                {
                    htData["USERDESC"] = "";
                    htData["ISENABLE"] = "1";
                    UpdateMemData(id, htData);
                }
                else
                {
                    return new { status = "99", msg = "認證失敗！" };
                }

                return new { status = "0", msg = "認證成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "認證失敗！" };
            }
        }

        [HttpGet]
        public dynamic GetStationData()
        {
            Hashtable htData = new Hashtable();
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            return objMember.GetACStationData(htData);
        }

        /*--------------------- 個人會員 ------------------------*/

        #region 快遞單號(未入庫,已入庫)
        //存入快遞單號
        [HttpPost]
        public dynamic SaveTansferData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                //Master data
                Hashtable mData = new Hashtable();
                mData["STATIONCODE"] = arrData.Value<string>("stationcode");
                mData["TRANSFERNO"] = arrData.Value<string>("transferno");
                mData["TOTAL"] = arrData.Value<string>("total");
                mData["ISMULTRECEIVER"] = arrData.Value<string>("ismultreceiver").ToLower() == "true" ? "Y" : "N";

                if (mData["ISMULTRECEIVER"]?.ToString() == "N")
                {
                    if (arrData.Value<string>("receiver") != null)
                        mData["RECEIVER"] = arrData.Value<string>("receiver");
                    if (arrData.Value<string>("receiveraddr") != null)
                        mData["RECEIVERADDR"] = arrData.Value<string>("receiveraddr");
                    if (arrData.Value<string>("receiverphone") != null)
                        mData["RECEIVERPHONE"] = arrData.Value<string>("receiverphone");
                }

                mData["_acccode"] = Request.Cookies["_acccode"];

                string sql = $@"SELECT A.ID AS COL1 FROM T_S_ACCOUNT A
                                LEFT JOIN T_S_ACRANKMAP B ON B.USERCODE = A.USERCODE
                                LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                WHERE A.USERCODE = '{mData["_acccode"]}' AND C.RANKTYPE = '1'";

                mData["ACCOUNTID"] = DBUtil.GetSingleValue1(sql);

                //Header(box) data                
                JArray boxData = arrData.Value<JArray>("boxform");
                ArrayList AL = new ArrayList();
                for (int i = 0; i < boxData.Count; i++)
                {
                    JObject temp = (JObject)boxData[i];
                    Hashtable hData = new Hashtable();
                    foreach (var t in temp)
                    {
                        //Detail(product) data
                        if (t.Key == "productform")
                        {
                            JArray prdData = temp.Value<JArray>("productform");
                            ArrayList subAL = new ArrayList();
                            for (int j = 0; j < prdData.Count; j++)
                            {
                                JObject temp2 = (JObject)prdData[j];
                                Hashtable dData = new Hashtable();
                                foreach (var t2 in temp2)
                                {
                                    dData[(t2.Key).ToUpper()] = t2.Value?.ToString();
                                }
                                subAL.Add(dData);
                            }
                            hData["PRDFORM"] = subAL;

                        }
                        else
                            hData[(t.Key).ToUpper()] = t.Value?.ToString();

                    }
                    AL.Add(hData);
                }

                //Declarant data
                JArray decData = arrData.Value<JArray>("decform");
                ArrayList decAL = new ArrayList();
                for (int k = 0; k < decData.Count; k++)
                {
                    JObject temp = (JObject)decData[k];
                    Hashtable deData = new Hashtable();
                    foreach (var t in temp)
                        deData[(t.Key).ToUpper()] = t.Value?.ToString();

                    decAL.Add(deData);
                }

                //新增主單
                long MID = InsertTransferM(mData);

                //新增各箱細項                
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        sData["TRANSFERIDM"] = MID;
                        long HID = InsertTransferH(sData);

                        ArrayList subAL = (ArrayList)sData["PRDFORM"];
                        if (subAL.Count > 0)
                        {
                            for (int j = 0; j < subAL.Count; j++)
                            {
                                Hashtable tempData = (Hashtable)subAL[j];
                                tempData["TRANSFERIDM"] = MID;
                                tempData["TRANSFERIDH"] = HID;
                                InsertTransferD(tempData);
                            }
                        }

                    }
                }

                //新增申報人名單
                if (decAL.Count > 0)
                {
                    for (int k = 0; k < decAL.Count; k++)
                    {
                        Hashtable tempData = (Hashtable)decAL[k];
                        tempData["TRANSFERIDM"] = MID;
                        InsertTEDeclarant(tempData);
                    }
                }

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        //取得快遞單號
        [HttpPost]
        public dynamic GetACTransferData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                Hashtable htData = new Hashtable();
                var jsonData = JObject.FromObject(form);
                int pageIndex = jsonData.Value<int>("PAGE_INDEX");
                int pageSize = jsonData.Value<int>("PAGE_SIZE");
                JArray tempArray = jsonData.Value<JArray>("srhForm");

                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                htData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["_acccode"]}'");

                if (tempArray.Count > 0)
                {
                    Dictionary<string, string> srhKey = new Dictionary<string, string>();
                    srhKey.Add("status", "STATUS");
                    srhKey.Add("stationcode", "STATIONCODE");

                    JObject temp = (JObject)tempArray[0];
                    foreach (var t in temp)
                        htData[srhKey[t.Key]] = t.Value?.ToString();

                    if (!string.IsNullOrEmpty(htData["ACCOUNTID"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " ACCOUNTID = " + htData["ACCOUNTID"];

                    if (!string.IsNullOrEmpty(htData["STATUS"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATUS = " + (htData["STATUS"]?.ToString() == "t1" ? 0 : 1);

                    if (!string.IsNullOrEmpty(htData["STATIONCODE"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATIONCODE = '" + htData["STATIONCODE"]?.ToString() + "'";
                }


                return objMember.GetTransferData(sWhere, pageIndex, pageSize);

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        //取得要合併的快遞單號資料
        [HttpPost]
        public dynamic GetTransferData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                var jsonData = JObject.FromObject(form);
                string transid = jsonData.Value<string>("id");

                if (!string.IsNullOrEmpty(transid))
                {
                    string[] arrList = transid.Split(';');
                    if (arrList.Length > 0)
                    {

                        for (int i = 0; i < arrList.Length; i++)
                        {
                            sWhere += (sWhere == "" ? "" : ",") + $@"'{arrList[i]}'";
                        }
                    }
                }

                //return objMember.GetTransferData_Combine(sWhere);
                return "";
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }
        #endregion

        #region 集運單(待出貨,已出貨,已完成)
        //取得集運單
        [HttpPost]
        public dynamic GetACShippingData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                Hashtable htData = new Hashtable();
                var jsonData = JObject.FromObject(form);
                int pageIndex = jsonData.Value<int>("PAGE_INDEX");
                int pageSize = jsonData.Value<int>("PAGE_SIZE");
                JArray tempArray = jsonData.Value<JArray>("srhForm");

                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                htData["ACCOUNTID"] = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{htData["_acccode"]}'");

                if (tempArray.Count > 0)
                {
                    Dictionary<string, string> srhKey = new Dictionary<string, string>();
                    srhKey.Add("status", "STATUS");
                    srhKey.Add("stationcode", "STATIONCODE");

                    JObject temp = (JObject)tempArray[0];
                    foreach (var t in temp)
                        htData[srhKey[t.Key]] = t.Value?.ToString();

                    if (!string.IsNullOrEmpty(htData["ACCOUNTID"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " ACCOUNTID = " + htData["ACCOUNTID"];

                    if (!string.IsNullOrEmpty(htData["STATUS"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATUS = " + (htData["STATUS"]?.ToString() == "t3" ? 2 : (htData["STATUS"]?.ToString() == "t4" ? 3 : 4));

                    if (!string.IsNullOrEmpty(htData["STATIONCODE"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATIONCODE = '" + htData["STATIONCODE"]?.ToString() + "'";
                }


                return objMember.GetShippingData(sWhere, pageIndex, pageSize);

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic GetACTransferData(long id)
        {
            try
            {
                Hashtable htData = new Hashtable();
                htData["TRANSFERIDM"] = id.ToString();
                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                string sql = $@"SELECT A.ID AS COL1 FROM T_S_ACCOUNT A
                                LEFT JOIN T_S_ACRANKMAP B ON B.USERCODE = A.USERCODE
                                LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                WHERE A.USERCODE = '{htData["_acccode"]}' AND C.RANKTYPE = '1'";

                htData["ACCOUNTID"] = DBUtil.GetSingleValue1(sql);

                return objMember.GetSingleACTransferData(htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic GetACShippingData(long id)
        {
            try
            {
                Hashtable htData = new Hashtable();
                htData["SHIPPINGIDM"] = id.ToString();
                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                string sql = $@"SELECT A.ID AS COL1 FROM T_S_ACCOUNT A
                                LEFT JOIN T_S_ACRANKMAP B ON B.USERCODE = A.USERCODE
                                LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                WHERE A.USERCODE = '{htData["_acccode"]}' AND C.RANKTYPE = '1'";

                htData["ACCOUNTID"] = DBUtil.GetSingleValue1(sql);

                return objMember.GetSingleACShippingData(htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        [HttpPost]
        public dynamic DelACTransferData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList DelAL = new ArrayList();
                for (int j = 0; j < arrData.Count; j++)
                {
                    JValue temp = (JValue)arrData[j];

                    DelAL.Add(Convert.ToInt64(temp));
                }

                if (DelAL.Count > 0)
                {
                    for (int i = 0; i < DelAL.Count; i++)
                    {
                        Hashtable tempData = new Hashtable();
                        tempData["TRANSFERIDM"] = Convert.ToInt64(DelAL[i]);

                        //檢查是否有此單可刪除(避免前後台訂單狀態不同步)
                        bool IsExist = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT ACCOUNTID AS COL1 FROM T_E_TRANSFER_M WHERE ID = {tempData["TRANSFERIDM"]} AND STATUS = 0")) ? false : true;

                        if (IsExist)
                        {
                            //delete Declarant data
                            DeleteTEData_All("T_E_DECLARANT", tempData);

                            //delete transfer_H & transfer_D data
                            DeleteTEData_All("T_E_TRANSFER_D", tempData);
                            DeleteTEData_All("T_E_TRANSFER_H", tempData);

                            //delete transfer_M data
                            DeleteTEData_All("T_E_TRANSFER_M", tempData);
                        }
                        else
                            return new { status = "99", msg = "刪除失敗！" };
                    }
                }

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }

        //快遞單申報人編輯
        [HttpPost]
        public dynamic SaveTransferDecData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");
                JArray delArrData = jsonData.Value<JArray>("dellist");

                //Master data
                Hashtable mData = new Hashtable();
                mData["TRANSFERIDM"] = arrData.Value<string>("idm");

                //Declarant data
                JArray decData = arrData.Value<JArray>("decform");
                ArrayList decAL = new ArrayList();
                for (int k = 0; k < decData.Count; k++)
                {
                    JObject temp = (JObject)decData[k];
                    Hashtable deData = new Hashtable();
                    foreach (var t in temp)
                        deData[(t.Key).ToUpper()] = t.Value?.ToString();

                    decAL.Add(deData);
                }

                ArrayList decDelAL = new ArrayList();
                for (int j = 0; j < delArrData.Count; j++)
                {
                    JValue temp = (JValue)delArrData[j];

                    decDelAL.Add(Convert.ToInt64(temp));
                }

                if (decAL.Count > 0)
                {
                    for (int k = 0; k < decAL.Count; k++)
                    {
                        Hashtable tempData = (Hashtable)decAL[k];
                        if (Convert.ToInt64(tempData["ID"]) == 0)
                        {
                            tempData["TRANSFERIDM"] = mData["TRANSFERIDM"];
                            InsertTEDeclarant(tempData);//新增申報人
                        }
                        else
                            UpdateTEDeclarant(Convert.ToInt64(tempData["ID"]), tempData);//更新申報人
                    }
                }

                //刪除申報人
                if (decDelAL.Count > 0)
                {
                    for (int i = 0; i < decDelAL.Count; i++)
                        DeleteTEDeclarant(Convert.ToInt64(decDelAL[i]));
                }

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        //集運單申報人編輯
        [HttpPost]
        public dynamic SaveShippingDecData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");
                JArray delArrData = jsonData.Value<JArray>("dellist");

                //Master data
                Hashtable mData = new Hashtable();
                mData["SHIPPINGIDM"] = arrData.Value<string>("idm");

                //Declarant data
                JArray decData = arrData.Value<JArray>("decform");
                ArrayList decAL = new ArrayList();
                for (int k = 0; k < decData.Count; k++)
                {
                    JObject temp = (JObject)decData[k];
                    Hashtable deData = new Hashtable();
                    foreach (var t in temp)
                        deData[(t.Key).ToUpper()] = t.Value?.ToString();

                    decAL.Add(deData);
                }

                ArrayList decDelAL = new ArrayList();
                for (int j = 0; j < delArrData.Count; j++)
                {
                    JValue temp = (JValue)delArrData[j];

                    decDelAL.Add(Convert.ToInt64(temp));
                }

                if (decAL.Count > 0)
                {
                    for (int k = 0; k < decAL.Count; k++)
                    {
                        Hashtable tempData = (Hashtable)decAL[k];
                        if (Convert.ToInt64(tempData["ID"]) == 0)
                        {
                            tempData["SHIPPINGIDM"] = mData["SHIPPINGIDM"];
                            InsertTNDeclarant(tempData);//新增申報人
                        }
                        else
                            UpdateTNDeclarant(Convert.ToInt64(tempData["ID"]), tempData);//更新申報人
                    }
                }

                //刪除申報人
                if (decDelAL.Count > 0)
                {
                    for (int i = 0; i < decDelAL.Count; i++)
                        DeleteTNDeclarant(Convert.ToInt64(decDelAL[i]));
                }

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }
        #endregion

        #region 申報人管理
        [HttpGet]
        public dynamic GetACDeclarantData()
        {
            Hashtable htData = new Hashtable();
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            return objMember.GetDeclarantData(htData);
        }

        [HttpGet("{id}")]
        public dynamic GetDeclarantData(long id)
        {
            Hashtable htData = new Hashtable();
            htData["DECLARANTID"] = id;
            htData["_acccode"] = Request.Cookies["_acccode"];
            htData["_accname"] = Request.Cookies["_accname"];

            return objMember.GetDeclarantData(htData);
        }

        [HttpPost]
        public dynamic SaveDeclarantData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                Hashtable htData = new Hashtable();
                foreach (var t in arrData)
                    htData[t.Key.ToUpper()] = t.Value?.ToString();

                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                if (htData["ID"]?.ToString() == "0")
                    InsertDeclarant(htData, 2);
                else
                    UpdateDeclarant(Convert.ToInt64(htData["ID"]), htData);

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", id = "0", msg = "保存失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic EditDeclarantData(long id)
        {
            try
            {
                Hashtable htData = new Hashtable();
                htData["DECLARANTID"] = id;
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                DelDeclarant(htData);

                return new { status = "0", msg = "刪除成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }
        #endregion

        #region 私有function

        private void InsertMemCompanyData(Hashtable htData)
        {

            TSAccount rowTSA = new TSAccount();

            rowTSA.Usercode = htData["USERCODE"]?.ToString();
            rowTSA.Userpassword = objComm.GetMd5Hash(htData["USERPASSWORD"]?.ToString());
            rowTSA.Userdesc = (htData["RANDONCODE"]?.ToString()).Trim();
            rowTSA.Username = htData["USERNAME"]?.ToString();
            rowTSA.Companyname = htData["COMPANYNAME"]?.ToString();
            rowTSA.Rateid = htData["RATEID"]?.ToString();
            string WCode = objComm.GetSeqCode("WAREHOUSENO_CUS");
            rowTSA.Warehouseno = "TECWCUS-" + WCode;
            rowTSA.Email = htData["EMAIL"]?.ToString();
            rowTSA.Taxid = htData["TAXID"]?.ToString();
            rowTSA.Phone = htData["PHONE"]?.ToString();
            rowTSA.Mobile = htData["MOBILE"]?.ToString();
            rowTSA.Addr = htData["ADDRESS"]?.ToString();

            rowTSA.Isenable = false;
            rowTSA.Logincount = 0;
            rowTSA.Lastlogindate = DateTime.Now;
            rowTSA.Credate = rowTSA.Lastlogindate;
            rowTSA.Createby = "SYSTEM";
            rowTSA.Upddate = rowTSA.Credate;
            rowTSA.Updby = "SYSTEM";

            _context.TSAccount.Add(rowTSA);
            _context.SaveChanges();

            TSAcrankmap rowTSAR = new TSAcrankmap();
            rowTSAR.Usercode = htData["USERCODE"]?.ToString();
            rowTSAR.Rankid = 2;//一般廠商會員

            _context.TSAcrankmap.Add(rowTSAR);
            _context.SaveChanges();

            //Update code
            objComm.UpdateSeqCode("WAREHOUSENO_CUS");
        }

        private void InsertMemData(Hashtable htData)
        {

            TSAccount rowTSA = new TSAccount();

            rowTSA.Usercode = htData["USERCODE"]?.ToString();
            rowTSA.Userpassword = objComm.GetMd5Hash(htData["USERPASSWORD"]?.ToString());
            rowTSA.Userdesc = (htData["RANDONCODE"]?.ToString()).Trim();
            rowTSA.Username = htData["USERNAME"]?.ToString();
            string WCode = objComm.GetSeqCode("WAREHOUSENO");
            rowTSA.Warehouseno = "TECW-" + WCode;
            rowTSA.Email = htData["EMAIL"]?.ToString();
            rowTSA.Taxid = htData["TAXID"]?.ToString();
            rowTSA.Phone = htData["PHONE"]?.ToString();
            rowTSA.Mobile = htData["MOBILE"]?.ToString();
            rowTSA.Addr = htData["ADDRESS"]?.ToString();

            rowTSA.Isenable = false;
            rowTSA.Logincount = 0;
            rowTSA.Lastlogindate = DateTime.Now;
            rowTSA.Credate = rowTSA.Lastlogindate;
            rowTSA.Createby = "SYSTEM";
            rowTSA.Upddate = rowTSA.Credate;
            rowTSA.Updby = "SYSTEM";

            _context.TSAccount.Add(rowTSA);
            _context.SaveChanges();

            TSAcrankmap rowTSAR = new TSAcrankmap();
            rowTSAR.Usercode = htData["USERCODE"]?.ToString();
            rowTSAR.Rankid = 1;//一般會員

            _context.TSAcrankmap.Add(rowTSAR);
            _context.SaveChanges();

            //Update code
            objComm.UpdateSeqCode("WAREHOUSENO");

        }

        private void UpdateMemData(long id, Hashtable sData)
        {
            var query = _context.TSAccount.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSAccount rowTSA = query;

                if (!string.IsNullOrEmpty(sData["NEWPW"]?.ToString()))
                    rowTSA.Userpassword = objComm.GetMd5Hash(sData["NEWPW"]?.ToString());
                if (sData["USERNAME"] != null)
                    rowTSA.Username = sData["USERNAME"]?.ToString();
                if (sData["USERDESC"] != null)
                    rowTSA.Userdesc = sData["USERDESC"]?.ToString();
                if (sData["EMAIL"] != null)
                    rowTSA.Email = sData["EMAIL"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTSA.Taxid = sData["TAXID"]?.ToString();
                if (sData["PHONE"] != null)
                    rowTSA.Phone = sData["PHONE"]?.ToString();
                if (sData["MOBILE"] != null)
                    rowTSA.Mobile = sData["MOBILE"]?.ToString();
                if (sData["ADDRESS"] != null)
                    rowTSA.Addr = sData["ADDRESS"]?.ToString();
                if (sData["ISENABLE"] != null)
                    rowTSA.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTSA.Upddate = DateTime.Now;
                    rowTSA.Updby = sData["_acccode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void UpdateCusMemData(long id, Hashtable sData)
        {
            var query = _context.TSAccount.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSAccount rowTSA = query;

                if (!string.IsNullOrEmpty(sData["NEWPW"]?.ToString()))
                    rowTSA.Userpassword = objComm.GetMd5Hash(sData["NEWPW"]?.ToString());
                if (sData["USERNAME"] != null)
                    rowTSA.Username = sData["USERNAME"]?.ToString();
                if (sData["USERDESC"] != null)
                    rowTSA.Userdesc = sData["USERDESC"]?.ToString();
                if (sData["EMAIL"] != null)
                    rowTSA.Email = sData["EMAIL"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTSA.Taxid = sData["TAXID"]?.ToString();
                if (sData["PHONE"] != null)
                    rowTSA.Phone = sData["PHONE"]?.ToString();
                if (sData["MOBILE"] != null)
                    rowTSA.Mobile = sData["MOBILE"]?.ToString();
                if (sData["ADDRESS"] != null)
                    rowTSA.Addr = sData["ADDRESS"]?.ToString();
                if (sData["ISENABLE"] != null)
                    rowTSA.Isenable = sData["ISENABLE"]?.ToString() == "1" ? true : false;

                if (sData.Count > 2)//排除cookies
                {
                    rowTSA.Upddate = DateTime.Now;
                    rowTSA.Updby = sData["_cuscode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void SendEmailAccept(Hashtable sData)
        {
            string F_User = "TEC Website System<ebs.sys@t3ex-group.com>";
            string T_User = sData["EMAIL"]?.ToString();
            string subject = "TEC轉運平台信箱認證信";
            string body = "";

            body += $"<p>認證信箱代碼：{sData["RANDONCODE"]}</p><br/>";
            body += "<p>請將此代碼輸入到驗證信箱欄位，來完成註冊程序，謝謝</p>";

            string C_User = "siawu@t3ex-group.com";

            objComm.SendMail(F_User, T_User, subject, body, C_User);
        }

        private long InsertTransferM(Hashtable sData)
        {
            long ID = 0;
            try
            {
                TETransferM TEM = new TETransferM();
                TEM.Accountid = Convert.ToInt64(sData["ACCOUNTID"]);
                TEM.Stationcode = sData["STATIONCODE"]?.ToString();
                TEM.Transferno = sData["TRANSFERNO"]?.ToString();
                TEM.Total = sData["TOTAL"]?.ToString();
                TEM.Receiver = sData["RECEIVER"]?.ToString();
                TEM.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                TEM.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                TEM.Ismultreceiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;
                TEM.Status = 0;
                TEM.Remark = "";
                TEM.Credate = DateTime.Now;
                TEM.Createby = sData["_acccode"]?.ToString();
                TEM.Upddate = TEM.Credate;
                TEM.Updby = TEM.Createby;

                _context.TETransferM.Add(TEM);
                _context.SaveChanges();

                ID = TEM.Id;

                return ID;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return ID;
            }

        }

        private long InsertTransferH(Hashtable sData)
        {
            long ID = 0;
            try
            {
                TETransferH TEH = new TETransferH();
                TEH.Boxno = sData["BOXNO"]?.ToString();
                TEH.Receiver = sData["RECEIVER"]?.ToString();
                TEH.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                TEH.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                TEH.TransferidM = Convert.ToInt64(sData["TRANSFERIDM"]);

                _context.TETransferH.Add(TEH);
                _context.SaveChanges();

                ID = TEH.Id;

                return ID;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return ID;
            }
        }

        private void InsertTransferD(Hashtable sData)
        {
            try
            {
                TETransferD TED = new TETransferD();
                TED.Product = sData["PRODUCT"]?.ToString();
                TED.Quantity = sData["QUANTITY"]?.ToString();
                TED.Unitprice = sData["UNITPRICE"]?.ToString();
                TED.TransferidM = Convert.ToInt64(sData["TRANSFERIDM"]);
                TED.TransferidH = Convert.ToInt64(sData["TRANSFERIDH"]);

                _context.TETransferD.Add(TED);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        private void InsertTEDeclarant(Hashtable sData)
        {
            try
            {
                TEDeclarant TED = new TEDeclarant();
                TED.Name = sData["NAME"]?.ToString();
                TED.Taxid = sData["TAXID"]?.ToString();
                TED.Phone = sData["PHONE"]?.ToString();
                TED.Mobile = sData["MOBILE"]?.ToString();
                TED.Addr = sData["ADDR"]?.ToString();
                TED.IdphotoF = sData["IDPHOTOF"]?.ToString();
                TED.IdphotoB = sData["IDPHOTOB"]?.ToString();
                TED.Appointment = sData["APPOINTMENT"]?.ToString();
                TED.TransferidM = Convert.ToInt64(sData["TRANSFERIDM"]);

                _context.TEDeclarant.Add(TED);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        private void UpdateTEDeclarant(long id, Hashtable sData)
        {
            var query = _context.TEDeclarant.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TEDeclarant rowTED = query;

                if (sData["NAME"] != null)
                    rowTED.Name = sData["NAME"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTED.Taxid = sData["TAXID"]?.ToString();
                if (sData["PHONE"] != null)
                    rowTED.Phone = sData["PHONE"]?.ToString();
                if (sData["MOBILE"] != null)
                    rowTED.Mobile = sData["MOBILE"]?.ToString();
                if (sData["ADDR"] != null)
                    rowTED.Addr = sData["ADDR"]?.ToString();
                if (sData["IDPHOTOF"] != null)
                    rowTED.IdphotoF = sData["IDPHOTOF"]?.ToString();
                if (sData["IDPHOTOB"] != null)
                    rowTED.IdphotoB = sData["IDPHOTOB"]?.ToString();
                if (sData["APPOINTMENT"] != null)
                    rowTED.Appointment = sData["APPOINTMENT"]?.ToString();

                _context.SaveChanges();

            }
        }

        //刪除申報人資料(單筆)
        private void DeleteTEDeclarant(long id)
        {
            var query = _context.TEDeclarant.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                _context.TEDeclarant.Remove(query);
                _context.SaveChanges();
            }
        }

        //刪除主單(Master)/箱號(Header)/細項(Detail)/申報人資料(該集運單下所有)
        private void DeleteTEData_All(string table, Hashtable sData)
        {
            //申報人先移除實體檔案
            if (table == "T_E_DECLARANT")
            {
                var query = _context.TEDeclarant.Where(q => q.TransferidM == Convert.ToInt64(sData["TRANSFERIDM"])).ToList();
                foreach (var qitem in query)
                {
                    if (!string.IsNullOrEmpty(qitem.IdphotoF))
                        objComm.DeleteFileF(qitem.IdphotoF.Replace("res", ""));

                    if (!string.IsNullOrEmpty(qitem.IdphotoB))
                        objComm.DeleteFileF(qitem.IdphotoB.Replace("res", ""));

                    if (!string.IsNullOrEmpty(qitem.Appointment))
                        objComm.DeleteFileF(qitem.Appointment.Replace("res", ""));
                }
            }

            string sql = "";
            if (table == "T_E_TRANSFER_M")
                sql = $@"DELETE FROM {table} WHERE ID = @TRANSFERIDM";
            else
                sql = $@"DELETE FROM {table} WHERE TRANSFERID_M = @TRANSFERIDM";

            DBUtil.EXECUTE(sql, sData);
        }

        private void InsertTNDeclarant(Hashtable sData)
        {
            try
            {
                TNDeclarant TND = new TNDeclarant();
                TND.Name = sData["NAME"]?.ToString();
                TND.Taxid = sData["TAXID"]?.ToString();
                TND.Phone = sData["PHONE"]?.ToString();
                TND.Mobile = sData["MOBILE"]?.ToString();
                TND.Addr = sData["ADDR"]?.ToString();
                TND.IdphotoF = sData["IDPHOTOF"]?.ToString();
                TND.IdphotoB = sData["IDPHOTOB"]?.ToString();
                TND.Appointment = sData["APPOINTMENT"]?.ToString();
                TND.ShippingidM = Convert.ToInt64(sData["SHIPPINGIDM"]);

                _context.TNDeclarant.Add(TND);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        private void UpdateTNDeclarant(long id, Hashtable sData)
        {
            var query = _context.TNDeclarant.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TNDeclarant rowTND = query;

                if (sData["NAME"] != null)
                    rowTND.Name = sData["NAME"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTND.Taxid = sData["TAXID"]?.ToString();
                if (sData["PHONE"] != null)
                    rowTND.Phone = sData["PHONE"]?.ToString();
                if (sData["MOBILE"] != null)
                    rowTND.Mobile = sData["MOBILE"]?.ToString();
                if (sData["ADDR"] != null)
                    rowTND.Addr = sData["ADDR"]?.ToString();
                if (sData["IDPHOTOF"] != null)
                    rowTND.IdphotoF = sData["IDPHOTOF"]?.ToString();
                if (sData["IDPHOTOB"] != null)
                    rowTND.IdphotoB = sData["IDPHOTOB"]?.ToString();
                if (sData["APPOINTMENT"] != null)
                    rowTND.Appointment = sData["APPOINTMENT"]?.ToString();

                _context.SaveChanges();

            }
        }

        //刪除申報人資料(單筆)
        private void DeleteTNDeclarant(long id)
        {
            var query = _context.TNDeclarant.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                _context.TNDeclarant.Remove(query);
                _context.SaveChanges();
            }
        }

        private void InsertDeclarant(Hashtable htData, int type)
        {

            TSDeclarant rowTSD = new TSDeclarant();

            rowTSD.Type = type;
            rowTSD.Name = (htData["NAME"]?.ToString()).Trim();
            rowTSD.Taxid = htData["TAXID"]?.ToString();
            rowTSD.Phone = htData["PHONE"]?.ToString();
            rowTSD.Mobile = htData["MOBILE"]?.ToString();
            rowTSD.Addr = htData["ADDR"]?.ToString();
            rowTSD.IdphotoF = htData["IDPHOTO_F"]?.ToString();
            rowTSD.IdphotoB = htData["IDPHOTO_B"]?.ToString();
            rowTSD.Appointment = htData["APPOINTMENT"]?.ToString();

            rowTSD.Credate = DateTime.Now;
            rowTSD.Createby = htData["_acccode"]?.ToString();
            rowTSD.Upddate = rowTSD.Credate;
            rowTSD.Updby = htData["_acccode"]?.ToString();

            _context.TSDeclarant.Add(rowTSD);
            _context.SaveChanges();

            long decID = rowTSD.Id;

            TSAcdeclarantmap rowTSAD = new TSAcdeclarantmap();
            rowTSAD.Usercode = htData["_acccode"]?.ToString();
            rowTSAD.Declarantid = decID;

            _context.TSAcdeclarantmap.Add(rowTSAD);
            _context.SaveChanges();

        }

        private void UpdateDeclarant(long id, Hashtable sData)
        {
            var query = _context.TSDeclarant.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TSDeclarant rowTSD = query;

                if (sData["NAME"] != null)
                    rowTSD.Name = sData["NAME"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTSD.Taxid = sData["TAXID"]?.ToString();
                if (sData["PHONE"] != null)
                    rowTSD.Phone = sData["PHONE"]?.ToString();
                if (sData["MOBILE"] != null)
                    rowTSD.Mobile = sData["MOBILE"]?.ToString();
                if (sData["ADDR"] != null)
                    rowTSD.Addr = sData["ADDR"]?.ToString();
                if (sData["IDPHOTO_F"] != null)
                    rowTSD.IdphotoF = sData["IDPHOTO_F"]?.ToString();
                if (sData["IDPHOTO_B"] != null)
                    rowTSD.IdphotoB = sData["IDPHOTO_B"]?.ToString();
                if (sData["APPOINTMENT"] != null)
                    rowTSD.Appointment = sData["APPOINTMENT"]?.ToString();

                if (sData.Count > 2)//排除cookies
                {
                    rowTSD.Upddate = DateTime.Now;
                    rowTSD.Updby = sData["_acccode"]?.ToString();

                    _context.SaveChanges();
                }
            }
        }

        private void DelDeclarant(Hashtable sData)
        {
            var query = _context.TSDeclarant.Where(q => q.Id == Convert.ToInt64(sData["DECLARANTID"]) && q.Createby == sData["_acccode"].ToString()).FirstOrDefault();
            if (query != null)
            {
                _context.TSDeclarant.Remove(query);
                _context.SaveChanges();
            }

            var query_ = _context.TSAcdeclarantmap.Where(q => q.Id == Convert.ToInt64(sData["DECLARANTID"]) && q.Usercode == sData["_acccode"].ToString()).FirstOrDefault();
            if (query_ != null)
            {
                _context.TSAcdeclarantmap.Remove(query_);
                _context.SaveChanges();
            }
        }
        #endregion


        /*--------------------- 廠商會員 ------------------------*/

        #region 集運單
        //取得集運單(多筆)
        [HttpPost]
        public dynamic GetShippingCusData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                Hashtable htData = new Hashtable();
                var jsonData = JObject.FromObject(form);
                int pageIndex = jsonData.Value<int>("PAGE_INDEX");
                int pageSize = jsonData.Value<int>("PAGE_SIZE");
                JArray tempArray = jsonData.Value<JArray>("srhForm");

                //get cookies
                htData["_acccode"] = Request.Cookies["_acccode"];
                htData["_accname"] = Request.Cookies["_accname"];

                string sql = $@"SELECT A.ID AS COL1 FROM T_S_ACCOUNT A
                                LEFT JOIN T_S_ACRANKMAP B ON B.USERCODE = A.USERCODE
                                LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                WHERE A.USERCODE = '{htData["_acccode"]}' AND C.RANKTYPE = '2'";

                htData["ACCOUNTID"] = DBUtil.GetSingleValue1(sql);

                if (tempArray.Count > 0)
                {
                    JObject temp = (JObject)tempArray[0];
                    foreach (var t in temp)
                        htData[t.Key.ToUpper()] = t.Value?.ToString();

                    if (!string.IsNullOrEmpty(htData["ACCOUNTID"]?.ToString()))
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " ACCOUNTID = " + htData["ACCOUNTID"];

                    if (!string.IsNullOrEmpty(htData["STATUS"]?.ToString()))
                    {
                        int status = 0;
                        if (htData["STATUS"]?.ToString() == "t1")
                            status = 0;
                        else if (htData["STATUS"]?.ToString() == "t2")
                            status = 1;
                        else if (htData["STATUS"]?.ToString() == "t3")
                            status = 2;
                        else if (htData["STATUS"]?.ToString() == "t4")
                            status = 3;
                        else if (htData["STATUS"]?.ToString() == "t5")
                            status = 4;
                        else { }

                        sWhere += (sWhere == "" ? "WHERE" : " AND") + " STATUS = " + status;
                    }
                    

                    if (!string.IsNullOrEmpty(htData["CRESDATE"]?.ToString()) && !string.IsNullOrEmpty(htData["CREEDATE"]?.ToString()))
                    {
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + $" (CREDATE BETWEEN '{htData["CRESDATE"]?.ToString()} 00:00:00' AND '{htData["CREEDATE"]?.ToString()} 23:59:59')";
                    }

                    //已完成狀態下預設抓當天的
                    if (htData["STATUS"]?.ToString() == "t5" && string.IsNullOrEmpty(htData["CRESDATE"]?.ToString()) && string.IsNullOrEmpty(htData["CREEDATE"]?.ToString()))
                    {
                        sWhere += (sWhere == "" ? "WHERE" : " AND") + $" (CREDATE BETWEEN '{DateTime.Now.ToString("yyyy-MM-dd")} 00:00:00' AND '{DateTime.Now.ToString("yyyy-MM-dd")} 23:59:59')";
                    }

                }


                return objMember.GetShippingCusData(sWhere, pageIndex, pageSize);

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        //取得集運單(單筆)
        [HttpGet("{id}")]
        public dynamic GetSingleShippingCusData(long id)
        {
            try
            {
                Hashtable htData = new Hashtable();
                htData["SHIPPINGIDM"] = id.ToString();
                //get cookies
                htData["_cuscode"] = Request.Cookies["_cuscode"];
                htData["_cusname"] = Request.Cookies["_cusname"];

                string sql = $@"SELECT A.ID AS COL1 FROM T_S_ACCOUNT A
                                LEFT JOIN T_S_ACRANKMAP B ON B.USERCODE = A.USERCODE
                                LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                WHERE A.USERCODE = '{htData["_cuscode"]}' AND C.RANKTYPE = '2'";

                htData["ACCOUNTID"] = DBUtil.GetSingleValue1(sql);

                return objMember.GetSingleShippingCusData(htData);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "取得失敗！" };
            }
        }

        //委託集運
        [HttpPost]
        public dynamic SaveCusShippingData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");

                //Master data
                Hashtable mData = new Hashtable();
                mData["STATIONCODE"] = arrData.Value<string>("stationcode");
                mData["TRANSFERNO"] = arrData.Value<string>("transferno");
                mData["TOTAL"] = arrData.Value<string>("total");
                mData["ISMULTRECEIVER"] = arrData.Value<string>("ismultreceiver").ToLower() == "true" ? "Y" : "N";

                if (mData["ISMULTRECEIVER"]?.ToString() == "N")
                {
                    if (arrData.Value<string>("receiver") != null)
                        mData["RECEIVER"] = arrData.Value<string>("receiver");
                    if (arrData.Value<string>("receiveraddr") != null)
                        mData["RECEIVERADDR"] = arrData.Value<string>("receiveraddr");
                    if (arrData.Value<string>("receiverphone") != null)
                        mData["RECEIVERPHONE"] = arrData.Value<string>("receiverphone");
                }

                mData["_acccode"] = Request.Cookies["_acccode"];

                string sql = $@"SELECT A.ID AS COL1 FROM T_S_ACCOUNT A
                                LEFT JOIN T_S_ACRANKMAP B ON B.USERCODE = A.USERCODE
                                LEFT JOIN T_S_RANK C ON C.ID = B.RANKID
                                WHERE A.USERCODE = '{mData["_acccode"]}' AND C.RANKTYPE = '2'";

                mData["ACCOUNTID"] = DBUtil.GetSingleValue1(sql);

                //Header(box) data                
                JArray boxData = arrData.Value<JArray>("boxform");
                ArrayList AL = new ArrayList();
                for (int i = 0; i < boxData.Count; i++)
                {
                    JObject temp = (JObject)boxData[i];
                    Hashtable hData = new Hashtable();
                    foreach (var t in temp)
                    {
                        //Detail(product) data
                        if (t.Key == "productform")
                        {
                            JArray prdData = temp.Value<JArray>("productform");
                            ArrayList subAL = new ArrayList();
                            for (int j = 0; j < prdData.Count; j++)
                            {
                                JObject temp2 = (JObject)prdData[j];
                                Hashtable dData = new Hashtable();
                                foreach (var t2 in temp2)
                                {
                                    dData[(t2.Key).ToUpper()] = t2.Value?.ToString();
                                }
                                subAL.Add(dData);
                            }
                            hData["PRDFORM"] = subAL;

                        }
                        else
                            hData[(t.Key).ToUpper()] = t.Value?.ToString();

                    }
                    AL.Add(hData);
                }

                //Declarant data
                JArray decData = arrData.Value<JArray>("decform");
                ArrayList decAL = new ArrayList();
                for (int k = 0; k < decData.Count; k++)
                {
                    JObject temp = (JObject)decData[k];
                    Hashtable deData = new Hashtable();
                    foreach (var t in temp)
                        deData[(t.Key).ToUpper()] = t.Value?.ToString();

                    decAL.Add(deData);
                }

                //新增主單
                long MID = InsertCusShippingM(mData);

                //新增各箱細項                
                if (AL.Count > 0)
                {
                    for (int i = 0; i < AL.Count; i++)
                    {
                        Hashtable sData = (Hashtable)AL[i];
                        sData["SHIPPINGIDM"] = MID;
                        long HID = InsertCusShippingH(sData);

                        ArrayList subAL = (ArrayList)sData["PRDFORM"];
                        if (subAL.Count > 0)
                        {
                            for (int j = 0; j < subAL.Count; j++)
                            {
                                Hashtable tempData = (Hashtable)subAL[j];
                                tempData["SHIPPINGIDM"] = MID;
                                tempData["SHIPPINGIDH"] = HID;
                                InsertCusShippingD(tempData);
                            }
                        }

                    }
                }

                //新增申報人名單
                if (decAL.Count > 0)
                {
                    for (int k = 0; k < decAL.Count; k++)
                    {
                        Hashtable tempData = (Hashtable)decAL[k];
                        tempData["SHIPPINGIDM"] = MID;
                        InsertTVDeclarant(tempData);
                    }
                }

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        //集運單申報人編輯
        [HttpPost]
        public dynamic SaveCusShippingDecData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JObject arrData = jsonData.Value<JObject>("formdata");
                JArray delArrData = jsonData.Value<JArray>("dellist");

                JArray boxData = arrData.Value<JArray>("boxform");

                //Master data
                Hashtable mData = new Hashtable();
                mData["SHIPPINGIDM"] = boxData[0].Value<string>("shippingidm");
                mData["SHIPPINGIDH"] = boxData[0].Value<string>("shippingidh");

                //Declarant data
                JArray decData = boxData[0].Value<JArray>("decform");
                ArrayList decAL = new ArrayList();
                for (int k = 0; k < decData.Count; k++)
                {
                    JObject temp = (JObject)decData[k];
                    Hashtable deData = new Hashtable();
                    foreach (var t in temp)
                        deData[(t.Key).ToUpper()] = t.Value?.ToString();

                    decAL.Add(deData);
                }

                ArrayList decDelAL = new ArrayList();
                for (int j = 0; j < delArrData.Count; j++)
                {
                    JValue temp = (JValue)delArrData[j];

                    decDelAL.Add(Convert.ToInt64(temp));
                }

                if (decAL.Count > 0)
                {
                    for (int k = 0; k < decAL.Count; k++)
                    {
                        Hashtable tempData = (Hashtable)decAL[k];
                        if (Convert.ToInt64(tempData["ID"]) == 0)
                        {
                            tempData["SHIPPINGIDM"] = mData["SHIPPINGIDM"];
                            tempData["SHIPPINGIDH"] = mData["SHIPPINGIDH"];
                            InsertTVDeclarant(tempData);//新增申報人
                        }
                        else
                            UpdateTVDeclarant(Convert.ToInt64(tempData["ID"]), tempData);//更新申報人
                    }
                }

                //刪除申報人
                if (decDelAL.Count > 0)
                {
                    for (int i = 0; i < decDelAL.Count; i++)
                        DeleteTVDeclarant(Convert.ToInt64(decDelAL[i]));
                }

                return new { status = "0", msg = "保存成功！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "保存失敗！" };
            }
        }

        //移除集運單(未入庫)
        public dynamic DelShippingCusData([FromBody] object form)
        {
            try
            {
                var jsonData = JObject.FromObject(form);
                JArray arrData = jsonData.Value<JArray>("formdata");

                ArrayList DelAL = new ArrayList();
                for (int j = 0; j < arrData.Count; j++)
                {
                    JValue temp = (JValue)arrData[j];

                    DelAL.Add(Convert.ToInt64(temp));
                }

                List<string> SHIPPINGNO = new List<string>();
                string delfail = "";
                if (DelAL.Count > 0)
                {
                    for (int i = 0; i < DelAL.Count; i++)
                    {
                        Hashtable tempData = new Hashtable();
                        tempData["SHIPPINGIDM"] = Convert.ToInt64(DelAL[i]);

                        //檢查是否有此單可刪除(避免前後台訂單狀態不同步)
                        bool IsExist = string.IsNullOrEmpty(DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {tempData["SHIPPINGIDM"]} AND STATUS = 0")) ? false : true;

                        if (IsExist)
                        {
                            string tempShipping = DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {tempData["SHIPPINGIDM"]} AND STATUS = 0");
                            
                            //delete Declarant data
                            DeleteTVData_All("T_V_DECLARANT", tempData);

                            //delete shipping_H & shipping_D data
                            DeleteTVData_All("T_V_SHIPPING_D", tempData);
                            DeleteTVData_All("T_V_SHIPPING_H", tempData);

                            //delete shipping_M data
                            DeleteTVData_All("T_V_SHIPPING_M", tempData);

                            if (!string.IsNullOrEmpty(tempShipping))
                                SHIPPINGNO.Add(tempShipping);
                        }
                        else
                        {
                            string tempTransfer = DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {tempData["SHIPPINGIDM"]}");
                            delfail += (delfail == "" ? "" : ",") + tempTransfer;
                        }
                    }

                    //寫入異動拋轉紀錄
                    if (SHIPPINGNO.Count() > 0)
                    {
                        for (int k = 0; k < SHIPPINGNO.Count(); k++)
                            objComm.InsertDepotRecord(2, SHIPPINGNO[k]);
                    }

                }

                if (string.IsNullOrEmpty(delfail))
                    return new { status = "0", msg = "刪除成功！" };
                else
                    return new { status = "99", msg = $"集運單號：{delfail}，刪除失敗！" };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "刪除失敗！" };
            }
        }

        //委託集運單(匯入Excel)
        [HttpPost]
        [DisableRequestSizeLimit]
        public dynamic ImportCusShippingData()
        {
            try
            {
                string usercode = Request.Cookies["_cuscode"];

                Hashtable fname = new Hashtable();
                int chknum = 0;
                var file = Request.Form.Files;
                var folderName = Path.Combine(@"tectransit\dist\tectransit\assets\import", usercode);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                if　(file.Count > 0)
                {
                    //save file
                    for (int i = 0;i < file.Count; i++)
                    {
                        chknum = 0;

                        if ((file[i].FileName).ToLower().StartsWith("stock"))
                        {
                            fname["SHIPPINGFILE1"] = file[i].FileName;
                            chknum = 1;
                        }

                        if((file[i].FileName).ToLower().StartsWith("weight"))
                        {
                            fname["BROKERFILE1"] = file[i].FileName;
                            chknum = 2;
                        }

                        if (chknum > 0)
                        {
                            var fileName = Guid.NewGuid() + Path.GetExtension(file[i].FileName).ToLower();
                            var fullPath = Path.Combine(pathToSave, fileName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file[i].CopyTo(stream);
                            }

                            if (chknum == 1)
                                fname["SHIPPINGFILE1"] = fileName;
                            else if (chknum == 2)
                                fname["BROKERFILE1"] = fileName;
                            else { }

                        }
                    }

                    //新增&更新消艙表
                    if (fname["SHIPPINGFILE1"] != null)
                    {
                        //import excel to database
                        string SHIPPINGNO = ImportCusShippingData(usercode, fname);

                        
                        if (!string.IsNullOrEmpty(SHIPPINGNO))
                        {
                            if (SHIPPINGNO == "error00")
                            {
                                return new { status = "99", msg = "匯入失敗，重複匯入已存在的袋號！" };
                            }
                            else if (SHIPPINGNO == "error99")
                            {
                                return new { status = "99", msg = "匯入失敗！" };
                            }
                            else
                            {
                                //寫入拋轉紀錄
                                objComm.InsertDepotRecord(2, SHIPPINGNO);

                                #region 寄信通知台空人員
                                string F_User = "TEC Website System<ebs.sys@t3ex-group.com>";
                                string subject = $"TEC代運平台 - 企業戶集運單匯入通知";
                                string body = "";

                                string cus = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{usercode}'");
                                body += $"<p>企業戶：{cus}</p>";
                                body += $"<p>集運單號：{SHIPPINGNO}</p>";
                                
                                objComm.SendMasterMail(F_User, subject, body);
                                #endregion

                                return new { status = "0", msg = "匯入成功！" };
                            }
                        }
                        else
                        {
                            return new { status = "99", msg = "匯入失敗！" };
                        }
                    }
                    else
                        return new { status = "99", msg = "匯入失敗，請上傳已消倉表！" };
                }
                else
                    return new { status = "99", msg = "匯入失敗，無上傳任何檔案！" };


            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "匯入失敗！" };
            }
        }

        //重匯集運單(匯入Excel)
        [HttpPost]
        [DisableRequestSizeLimit]
        public dynamic CoverCusShippingData()
        {
            try
            {
                string usercode = Request.Cookies["_cuscode"];

                Hashtable htData = new Hashtable();
                string type = Request.Form["type"];
                htData["ID"] = Convert.ToInt64(Request.Form["id"]);
                var file = Request.Form.Files;
                var folderName = Path.Combine(@"tectransit\dist\tectransit\assets\import", usercode);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                if (file.Count > 0)
                {
                    //save file                    
                    if (type == "SHIPPINGFILE")
                        htData["SHIPPINGFILE1"] = file[0].FileName;
                    else
                        htData["BROKERFILE1"] = file[0].FileName;


                    var fileName = Guid.NewGuid() + Path.GetExtension(file[0].FileName).ToLower();
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file[0].CopyTo(stream);
                    }

                    if (htData["SHIPPINGFILE1"] != null)
                        htData["SHIPPINGFILE1"] = fileName;
                    else if (htData["BROKERFILE1"] != null)
                        htData["BROKERFILE1"] = fileName;
                    else { }


                    //更新消倉表
                    if (htData["SHIPPINGFILE1"] != null)
                    {

                        //鎖單檢查
                        bool IsLock = false;
                        string IMBRID = DBUtil.GetSingleValue1($@"SELECT IMBROKERID AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID", htData);
                        string EXBRID = DBUtil.GetSingleValue1($@"SELECT EXBROKERID AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID", htData);
                        if (IMBRID != "0" || EXBRID != "0")
                            IsLock = true;

                        if (IsLock)
                            return new { status = "99", msg = "匯入失敗，此單已進入報關程序中，有問題請洽相關人員！" };

                        //import excel to database
                        string SHIPPINGNO = UpdateCusShippingData(usercode, htData);


                        if (!string.IsNullOrEmpty(SHIPPINGNO))
                        {
                            if (SHIPPINGNO == "error00")
                            {
                                return new { status = "99", msg = "匯入失敗，重複匯入已存在的袋號！" };
                            }
                            else if (SHIPPINGNO == "error99")
                            {
                                return new { status = "99", msg = "匯入失敗！" };
                            }
                            else
                            {
                                //寫入拋轉紀錄
                                objComm.InsertDepotRecord(2, SHIPPINGNO);

                                #region 寄信通知台空人員
                                string F_User = "TEC Website System<ebs.sys@t3ex-group.com>";
                                string subject = $"TEC代運平台 - 企業戶集運單匯入通知";
                                string body = "";

                                string cus = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{usercode}'");
                                body += $"<p>企業戶：{cus}</p>";
                                body += $"<p>集運單號：{SHIPPINGNO}</p>";
                                
                                objComm.SendMasterMail(F_User, subject, body);
                                #endregion

                                return new { status = "0", msg = "匯入成功！" };
                            }
                        }
                        else
                        {
                            return new { status = "99", msg = "匯入失敗！" };
                        }
                    }


                    //更新材積與實重表
                    if (htData["BROKERFILE1"] != null)
                    {
                        htData["BROKERFILE1"] = $"res/assets/import/{usercode}/" + htData["BROKERFILE1"];
                        UpdateCusShippingM(htData);

                        #region 寄信通知台空人員
                        string F_User = "TEC Website System<ebs.sys@t3ex-group.com>";
                        string subject = $"TEC代運平台 - 企業戶匯入通知";
                        string body = "";

                        string cus = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{usercode}'");
                        string SHIPPINGNO = DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID", htData);
                        body += $"<p>企業戶：{cus}</p>";
                        body += $"<p>集運單號：{SHIPPINGNO}</p>";
                        body += $"<p>材積與實重表已更新！</p>";
                        
                        objComm.SendMasterMail(F_User, subject, body);
                        #endregion

                        return new { status = "0", msg = "匯入成功！" };
                    }

                    return new { status = "99", msg = "匯入失敗！" };

                }
                else
                    return new { status = "99", msg = "匯入失敗，無上傳任何檔案！" };


            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = "99", msg = "匯入失敗！" };
            }
        }

        #endregion

        #region 私有function
        private long InsertCusShippingM(Hashtable sData)
        {
            long ID = 0;
            try
            {
                TVShippingM TVM = new TVShippingM();
                TVM.Accountid = Convert.ToInt64(sData["ACCOUNTID"]);
                string autoSeqcode = objComm.GetSeqCode("SHIPPING_CUS");
                TVM.Shippingno = "TECTPEEC1" + DateTime.Now.ToString("yy") + autoSeqcode;
                TVM.Shippingno = sData["SHIPPINGNO"]?.ToString() ?? string.Empty;
                TVM.Mawbno = sData["MAWBNO"]?.ToString() ?? string.Empty;
                TVM.Flightnum = sData["FLIGHTNUM"]?.ToString() ?? string.Empty;
                TVM.Total = sData["TOTAL"]?.ToString();
                TVM.Totalweight = sData["TOTALWEIGHT"]?.ToString() ?? string.Empty;
                TVM.Trackingtype = 0;//尚未使用-0:無
                TVM.Receiver = sData["RECEIVER"]?.ToString();
                TVM.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                TVM.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                TVM.Taxid = sData["TAXID"]?.ToString();
                TVM.Ismultreceiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;
                TVM.Status = 0;
                TVM.Mawbdate = sData["MAWBDATE"]?.ToString();
                TVM.Credate = DateTime.Now;
                TVM.Createby = sData["_acccode"]?.ToString();
                TVM.Upddate = TVM.Credate;
                TVM.Updby = TVM.Createby;

                _context.TVShippingM.Add(TVM);
                _context.SaveChanges();

                objComm.UpdateSeqCode("SHIPPING_CUS");

                ID = TVM.Id;
                
                return ID;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return ID;
            }
        }

        private long InsertCusShippingH(Hashtable sData)
        {
            long ID = 0;
            try
            {
                TVShippingH TVH = new TVShippingH();
                TVH.Clearanceno = sData["CLEARANCENO"]?.ToString();
                TVH.Transferno = sData["TRANSFERNO"]?.ToString();
                TVH.Receiver = sData["RECEIVER"]?.ToString();
                TVH.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                TVH.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                TVH.Taxid = sData["TAXID"]?.ToString();
                TVH.Weight = sData["WEIGHT"]?.ToString() ?? string.Empty;
                TVH.Totalitem = sData["TOTALITEM"]?.ToString() ?? string.Empty;
                TVH.ShippingidM = Convert.ToInt64(sData["SHIPPINGIDM"]);

                _context.TVShippingH.Add(TVH);
                _context.SaveChanges();

                ID = TVH.Id;

                return ID;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return ID;
            }
        }

        private void InsertCusShippingD(Hashtable sData)
        {
            try
            {
                TVShippingD TVD = new TVShippingD();
                TVD.Product = sData["PRODUCT"]?.ToString();
                TVD.Quantity = sData["QUANTITY"]?.ToString();
                TVD.Unitprice = sData["UNITPRICE"]?.ToString();
                TVD.Unit = sData["UNIT"]?.ToString() ?? string.Empty;
                TVD.Origin = sData["ORIGIN"]?.ToString() ?? string.Empty;
                TVD.ShippingidM = Convert.ToInt64(sData["SHIPPINGIDM"]);
                TVD.ShippingidH = Convert.ToInt64(sData["SHIPPINGIDH"]);

                _context.TVShippingD.Add(TVD);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        private void InsertTVDeclarant(Hashtable sData)
        {
            try
            {
                TVDeclarant TVD = new TVDeclarant();
                TVD.Name = sData["NAME"]?.ToString();
                TVD.Taxid = sData["TAXID"]?.ToString();
                TVD.Phone = sData["PHONE"]?.ToString();
                TVD.Mobile = sData["MOBILE"]?.ToString();
                TVD.Addr = sData["ADDR"]?.ToString();
                TVD.IdphotoF = sData["IDPHOTOF"]?.ToString();
                TVD.IdphotoB = sData["IDPHOTOB"]?.ToString();
                TVD.Appointment = sData["APPOINTMENT"]?.ToString();
                TVD.ShippingidM = Convert.ToInt64(sData["SHIPPINGIDM"]);
                TVD.ShippingidH = Convert.ToInt64(sData["SHIPPINGIDH"]);

                _context.TVDeclarant.Add(TVD);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
            }
        }

        private void UpdateCusShippingM(Hashtable sData)
        {
            var query = _context.TVShippingM.Where(q => q.Id == Convert.ToInt64(sData["ID"])).FirstOrDefault();

            if (query != null)
            {
                TVShippingM rowTVM = query;
                
                if (sData["TOTAL"] != null)
                    rowTVM.Total = sData["TOTAL"]?.ToString();
                if (sData["TOTALWEIGHT"] != null)
                    rowTVM.Totalweight = sData["TOTALWEIGHT"]?.ToString();
                if (sData["ISMULTRECEIVER"] != null)
                    rowTVM.Ismultreceiver = sData["ISMULTRECEIVER"]?.ToString() == "Y" ? true : false;
                if (sData["RECEIVER"] != null)
                    rowTVM.Receiver = sData["RECEIVER"]?.ToString();
                if (sData["RECEIVERADDR"] != null)
                    rowTVM.Receiveraddr = sData["RECEIVERADDR"]?.ToString();
                if (sData["RECEIVERPHONE"] != null)
                    rowTVM.Receiverphone = sData["RECEIVERPHONE"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTVM.Taxid = sData["TAXID"]?.ToString();
                if (sData["BROKERFILE1"] != null)
                    rowTVM.Brokerfile1 = sData["BROKERFILE1"]?.ToString();

                _context.SaveChanges();

            }
        }

        private void UpdateCusShippingH(Hashtable sData)
        {
            var query = _context.TVShippingH.Where(q => q.Id == Convert.ToInt64(sData["SHIPPINGIDH"])).FirstOrDefault();

            if (query != null)
            {
                TVShippingH rowTVH = query;
                
                if (sData["WEIGHT"] != null)
                    rowTVH.Weight = sData["WEIGHT"]?.ToString();
                if (sData["TOTALITEM"] != null)
                    rowTVH.Totalitem = sData["TOTALITEM"]?.ToString();

                _context.SaveChanges();

            }
        }

        private void UpdateTVDeclarant(long id, Hashtable sData)
        {
            var query = _context.TVDeclarant.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                TVDeclarant rowTVD = query;

                if (sData["NAME"] != null)
                    rowTVD.Name = sData["NAME"]?.ToString();
                if (sData["TAXID"] != null)
                    rowTVD.Taxid = sData["TAXID"]?.ToString();
                if (sData["PHONE"] != null)
                    rowTVD.Phone = sData["PHONE"]?.ToString();
                if (sData["MOBILE"] != null)
                    rowTVD.Mobile = sData["MOBILE"]?.ToString();
                if (sData["ADDR"] != null)
                    rowTVD.Addr = sData["ADDR"]?.ToString();
                if (sData["IDPHOTOF"] != null)
                    rowTVD.IdphotoF = sData["IDPHOTOF"]?.ToString();
                if (sData["IDPHOTOB"] != null)
                    rowTVD.IdphotoB = sData["IDPHOTOB"]?.ToString();
                if (sData["APPOINTMENT"] != null)
                    rowTVD.Appointment = sData["APPOINTMENT"]?.ToString();

                _context.SaveChanges();

            }
        }

        //刪除申報人資料(單筆)
        private void DeleteTVDeclarant(long id)
        {
            var query = _context.TVDeclarant.Where(q => q.Id == id).FirstOrDefault();

            if (query != null)
            {
                _context.TVDeclarant.Remove(query);
                _context.SaveChanges();
            }
        }

        //刪除主單(Master)/箱號(Header)/細項(Detail)/申報人資料(該集運單下所有)
        private void DeleteTVData_All(string table, Hashtable sData)
        {
            //申報人先移除實體檔案
            //if (table == "T_V_DECLARANT")
            //{
            //    var query = _context.TVDeclarant.Where(q => q.ShippingidM == Convert.ToInt64(sData["SHIPPINGIDM"])).ToList();
            //    foreach (var qitem in query)
            //    {
            //        if (!string.IsNullOrEmpty(qitem.IdphotoF))
            //            objComm.DeleteFileF(qitem.IdphotoF.Replace("res", ""));

            //        if (!string.IsNullOrEmpty(qitem.IdphotoB))
            //            objComm.DeleteFileF(qitem.IdphotoB.Replace("res", ""));

            //        if (!string.IsNullOrEmpty(qitem.Appointment))
            //            objComm.DeleteFileF(qitem.Appointment.Replace("res", ""));
            //    }
            //}

            string sql = "";
            if (table == "T_V_SHIPPING_M")
                sql = $@"DELETE FROM {table} WHERE ID = @SHIPPINGIDM";
            else
                sql = $@"DELETE FROM {table} WHERE SHIPPINGID_M = @SHIPPINGIDM";

            DBUtil.EXECUTE(sql, sData);
        }
        

        //匯入集運(Excel to DB)
        public string ImportCusShippingData(string usercode, Hashtable file)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var folderName = Path.Combine(@"tectransit\dist\tectransit\assets\import", usercode);
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(pathToSave, file["SHIPPINGFILE1"]?.ToString());
                    FileInfo newFile = new FileInfo(fullPath);

                    using (ExcelPackage ep = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet ws = ep.Workbook.Worksheets[1];
                        var rowCt = ws.Dimension.End.Row;
                        Hashtable htData = new Hashtable();
                        htData.Add("SHIPPINGFILE1", $"res/assets/import/{usercode}/{file["SHIPPINGFILE1"]}");//消艙表url
                        htData.Add("BROKERFILE1", $"res/assets/import/{usercode}/{file["BROKERFILE1"]}");//材積與實重表url

                        htData.Add("NEWNUM", "");//編號(比對用)
                        htData.Add("NEWNO", "");//袋號(比對用)
                        htData.Add("NEWTOTALWG", "");//袋重(比對用)
                        htData.Add("NEWBOXNO", "");//提單號碼(比對用)
                        htData.Add("_cuscode", usercode);//用戶帳號
                        string ACID = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{usercode}' AND ISENABLE = 'true'");
                        htData.Add("ACCOUNTID", ACID);//用戶ID
                        htData.Add("MAWBDATE", ws.Cells[1, 6].Value?.ToString().Trim());//消倉單日期
                        htData.Add("MAWBNO", "");//MAWB (ws.Cells[1, 10].Value?.ToString().Trim()).Replace(" ", "").Replace("　", "").Replace("\r", "")
                        htData.Add("FLIGHTNUM", "");//航班號 ws.Cells[1, 15].Value?.ToString().Trim()

                        long MID = 0;
                        long HID = 0;
                        decimal TotalWGM = 0; //總重(主單)
                        decimal Total = 0; //總件數(主單) 
                        decimal TotalWG = 0; //各提單總重(商品)
                        decimal Totalitem = 0; //各提單總件數(商品) 
                                               //List<string> Box = new List<string>();
                        List<string> Rec = new List<string>();
                        
                        MID = objMember.InsertCusShippingM(htData);
                        
                        for (int i = 3; i <= rowCt; i++)
                        {
                            #region Excel資料欄位
                            htData["NEWNUM"] = ws.Cells[i, 1].Value?.ToString().Trim();
                            htData["NEWNO"] = ws.Cells[i, 2].Value?.ToString().Trim();
                            htData["NEWTOTALWG"] = ws.Cells[i, 3].Value?.ToString().Trim();
                            htData["NEWBOXNO"] = ws.Cells[i, 4].Value?.ToString().Trim();

                            if (!string.IsNullOrEmpty(ws.Cells[i, 2].Value?.ToString().Trim()))
                                htData["CLEARANCENO"] = ws.Cells[i, 2].Value?.ToString().Trim();//袋號=清關條碼

                            if (!string.IsNullOrEmpty(ws.Cells[i, 3].Value?.ToString().Trim()))
                                htData["TOTALWEIGHT"] = ws.Cells[i, 3].Value?.ToString().Trim();//總重量

                            htData["TRANSFERNO"] = ws.Cells[i, 4].Value?.ToString().Trim();//提單號碼                        
                            htData["TOTAL"] = ws.Cells[i, 5].Value?.ToString().Trim();//件數                    
                            htData["WEIGHT"] = ws.Cells[i, 6].Value?.ToString().Trim();//提單重量
                            htData["PRODUCT"] = ws.Cells[i, 7].Value?.ToString().Trim();//品名
                            htData["QUANTITY"] = ws.Cells[i, 8].Value?.ToString().Trim();//數量


                            //檢查袋號是否已匯入過(排除自己)
                            bool IsExist = false;
                            string chksql = $@"SELECT DISTINCT CLEARANCENO FROM T_V_SHIPPING_M A
                                           LEFT JOIN T_V_SHIPPING_H B ON A.ID = B.SHIPPINGID_M
                                           WHERE A.ACCOUNTID = {ACID} AND A.STATUS = 0 AND A.ID != {MID}";

                            DataTable DT = DBUtil.SelectDataTable(chksql);
                            if (DT.Rows.Count > 0)
                            {
                                for (int z = 0; z < DT.Rows.Count; z++)
                                {
                                    if (!string.IsNullOrEmpty(DT.Rows[z]["CLEARANCENO"]?.ToString()))
                                        if (DT.Rows[z]["CLEARANCENO"]?.ToString() == htData["CLEARANCENO"]?.ToString())
                                            IsExist = true;
                                    
                                }
                            }

                            if (IsExist)
                                return "error00";


                            //提單重&總件數
                            if (!string.IsNullOrEmpty(htData["PRODUCT"]?.ToString()))
                            {
                                if (string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()) && !string.IsNullOrEmpty(htData["WEIGHT"]?.ToString()))
                                    TotalWG += Convert.ToDecimal(htData["WEIGHT"]);

                                else
                                    TotalWG = Convert.ToDecimal(htData["WEIGHT"]);


                                if (string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()) && !string.IsNullOrEmpty(htData["QUANTITY"]?.ToString()))
                                {
                                    Totalitem += Convert.ToDecimal(htData["QUANTITY"]);
                                    htData["TOTALITEM"] = Totalitem;
                                }
                                else
                                {
                                    Totalitem = Convert.ToDecimal(htData["QUANTITY"]);
                                    htData["TOTALITEM"] = Totalitem;
                                }

                            }

                            htData["UNIT"] = ws.Cells[i, 9].Value?.ToString().Trim();//單位
                            htData["ORIGIN"] = ws.Cells[i, 10].Value?.ToString().Trim();//產地
                            htData["UNITPRICE"] = ws.Cells[i, 11].Value?.ToString().Trim();//單價

                            htData["SHIPPERCOMPANY"] = ws.Cells[i, 12].Value?.ToString().Trim();//寄件公司
                            htData["SHIPPER"] = ws.Cells[i, 13].Value?.ToString().Trim();//寄件人
                            htData["RECEIVERCOMPANY"] = ws.Cells[i, 14].Value?.ToString().Trim();//收件公司
                            htData["RECEIVER"] = ws.Cells[i, 15].Value?.ToString().Trim();//收件人                            
                            htData["RECEIVERPHONE"] = ws.Cells[i, 16].Value?.ToString().Trim();//收件人電話
                            htData["RECEIVERZIPCODE"] = ws.Cells[i, 17].Value?.ToString().Trim();//收件人郵遞區號
                            htData["RECEIVERADDR"] = ws.Cells[i, 18].Value?.ToString().Trim();//收件人地址
                            htData["TAXID"] = ws.Cells[i, 19].Value?.ToString().Trim();//統編or身分證字號
                            htData["SHIPPERREMARK"] = ws.Cells[i, 20].Value?.ToString().Trim();//備註(樂一番用)
                            htData["LOGISTICS"] = ws.Cells[i, 21].Value?.ToString().Trim();//出貨商                            
                            #endregion


                            //if product detail blank then break for loop
                            if (string.IsNullOrEmpty(htData["PRODUCT"]?.ToString()) && string.IsNullOrEmpty(htData["QUANTITY"]?.ToString()) && string.IsNullOrEmpty(htData["UNITPRICE"]?.ToString()))
                            {
                                break;
                            }

                            //new master data & new header data
                            if (!string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()))
                            {

                                htData["SHIPPINGIDM"] = MID;
                                HID = objMember.InsertCusShippingH(htData);
                            }
                            else
                            {
                                htData["SHIPPINGIDH"] = HID;
                                htData["WEIGHT"] = TotalWG;
                                objMember.UpdateCusShippingH(htData);
                            }

                            //New Detail
                            if (MID != 0 && HID != 0)
                            {
                                if (!string.IsNullOrEmpty(htData["PRODUCT"]?.ToString()) && !string.IsNullOrEmpty(htData["QUANTITY"]?.ToString()))
                                {
                                    htData["SHIPPINGIDM"] = MID;
                                    htData["SHIPPINGIDH"] = HID;
                                    objMember.InsertCusShippingD(htData);
                                }
                            }

                            if (MID != 0)
                            {
                                //New Declarant
                                Hashtable sData = new Hashtable();
                                sData["SHIPPINGIDM"] = MID;
                                sData["SHIPPINGIDH"] = HID;
                                sData["NAME"] = htData["RECEIVER"]?.ToString();
                                sData["TAXID"] = htData["TAXID"]?.ToString();
                                sData["PHONE"] = htData["RECEIVERPHONE"]?.ToString();
                                sData["ZIPCODE"] = htData["RECEIVERZIPCODE"]?.ToString();
                                sData["ADDR"] = htData["RECEIVERADDR"]?.ToString();

                                if (!string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()))
                                    objMember.InsertTVDeclarant(sData);

                                //check receiver
                                int chk = 0;
                                foreach (var r in Rec)
                                    if (r == htData["RECEIVER"]?.ToString())
                                        chk++;
                                if (chk == 0 && !string.IsNullOrEmpty(htData["RECEIVER"]?.ToString()))
                                    Rec.Add(htData["RECEIVER"]?.ToString());

                                decimal ReceiverCt = Rec.Count();

                                //Update Master Data
                                string WG = ws.Cells[i, 3].Value?.ToString().Trim();
                                string QTY = ws.Cells[i, 8].Value?.ToString().Trim();
                                if (!string.IsNullOrEmpty(WG))
                                    TotalWGM += Convert.ToDecimal(WG);
                                if (!string.IsNullOrEmpty(QTY))
                                    Total += Convert.ToDecimal(QTY);
                                Hashtable tempData = new Hashtable();
                                tempData["SHIPPINGFILE1"] = htData["SHIPPINGFILE1"]?.ToString();
                                tempData["BROKERFILE1"] = htData["BROKERFILE1"]?.ToString();
                                tempData["STORECODE"] = "C2011"; //倉儲代碼
                                tempData["ID"] = MID;
                                tempData["TOTAL"] = Total;
                                tempData["TOTALWEIGHT"] = TotalWGM;
                                tempData["ISMULTRECEIVER"] = ReceiverCt > 1 ? "Y" : "N";
                                // 單一收件人
                                if (ReceiverCt == 1)
                                {
                                    tempData["RECEIVERCOMPANY"] = htData["RECEIVERCOMPANY"]?.ToString();
                                    tempData["RECEIVER"] = htData["RECEIVER"]?.ToString();
                                    tempData["RECEIVERZIPCODE"] = htData["RECEIVERZIPCODE"]?.ToString();
                                    tempData["RECEIVERPHONE"] = htData["RECEIVERPHONE"]?.ToString();
                                    tempData["RECEIVERADDR"] = htData["RECEIVERADDR"]?.ToString();
                                    tempData["TAXID"] = htData["TAXID"]?.ToString();
                                }
                                else
                                {
                                    tempData["RECEIVERCOMPANY"] = "";
                                    tempData["RECEIVER"] = "";
                                    tempData["RECEIVERZIPCODE"] = "";
                                    tempData["RECEIVERPHONE"] = "";
                                    tempData["RECEIVERADDR"] = "";
                                    tempData["TAXID"] = "";
                                }
                                objMember.UpdateCusShippingM(tempData);
                            }
                        }

                        ws.Dispose();

                        //匯入成功回傳集運單號
                        string SHIPPINGNO = DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {MID}");

                        ts.Complete();

                        
                        return SHIPPINGNO;
                    }

                    #endregion
                    
                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                    return "error99";
                }
            }
        }

        //覆蓋集運單(Excel to DB)
        public string UpdateCusShippingData(string usercode, Hashtable data)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var folderName = Path.Combine(@"tectransit\dist\tectransit\assets\import", usercode);
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(pathToSave, data["SHIPPINGFILE1"]?.ToString());
                    FileInfo newFile = new FileInfo(fullPath);

                    using (ExcelPackage ep = new ExcelPackage(newFile))
                    {
                        long MID = 0;
                        long HID = 0;
                        decimal TotalWGM = 0; //總重(主單)
                        decimal Total = 0; //總件數(主單) 
                        decimal TotalWG = 0; //各提單總重(商品)
                        decimal Totalitem = 0; //各提單總件數(商品) 
                                               //List<string> Box = new List<string>();
                        List<string> Rec = new List<string>();

                        MID = Convert.ToInt64(data["ID"]);

                        ExcelWorksheet ws = ep.Workbook.Worksheets[1];
                        var rowCt = ws.Dimension.End.Row;
                        Hashtable htData = new Hashtable();
                        htData.Add("SHIPPINGFILE1", $"res/assets/import/{usercode}/{data["SHIPPINGFILE1"]}");//消艙表url

                        string GWUrl = DBUtil.GetSingleValue1($@"SELECT BROKERFILE1 AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID", data);
                        htData.Add("BROKERFILE1", GWUrl);//材積與實重表url

                        htData.Add("NEWNUM", "");//編號(比對用)
                        htData.Add("NEWNO", "");//袋號(比對用)
                        htData.Add("NEWTOTALWG", "");//袋重(比對用)
                        htData.Add("NEWBOXNO", "");//提單號碼(比對用)
                        htData.Add("_cuscode", usercode);//用戶帳號
                        string ACID = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = '{usercode}' AND ISENABLE = 'true'");
                        htData.Add("ACCOUNTID", ACID);//用戶ID
                        htData.Add("MAWBDATE", ws.Cells[1, 6].Value?.ToString().Trim());//消倉單日期
                        htData.Add("MAWBNO", "");//MAWB (ws.Cells[1, 10].Value?.ToString().Trim()).Replace(" ", "").Replace("　", "").Replace("\r", "")
                        htData.Add("FLIGHTNUM", "");//航班號 ws.Cells[1, 15].Value?.ToString().Trim()
                        
                        //刪除集運單底下所有資料(保留主單)
                        if(MID != 0)
                        {
                            Hashtable sData = new Hashtable();
                            sData["SHIPPINGIDM"] = MID;
                            DeleteTVData_All("T_V_DECLARANT", sData);
                            DeleteTVData_All("T_V_SHIPPING_D",sData);
                            DeleteTVData_All("T_V_SHIPPING_H",sData);
                        }


                        for (int i = 3; i <= rowCt; i++)
                        {
                            #region Excel資料欄位
                            htData["NEWNUM"] = ws.Cells[i, 1].Value?.ToString().Trim();
                            htData["NEWNO"] = ws.Cells[i, 2].Value?.ToString().Trim();
                            htData["NEWTOTALWG"] = ws.Cells[i, 3].Value?.ToString().Trim();
                            htData["NEWBOXNO"] = ws.Cells[i, 4].Value?.ToString().Trim();

                            if (!string.IsNullOrEmpty(ws.Cells[i, 2].Value?.ToString().Trim()))
                                htData["CLEARANCENO"] = ws.Cells[i, 2].Value?.ToString().Trim();//袋號=清關條碼

                            if (!string.IsNullOrEmpty(ws.Cells[i, 3].Value?.ToString().Trim()))
                                htData["TOTALWEIGHT"] = ws.Cells[i, 3].Value?.ToString().Trim();//總重量

                            htData["TRANSFERNO"] = ws.Cells[i, 4].Value?.ToString().Trim();//提單號碼                        
                            htData["TOTAL"] = ws.Cells[i, 5].Value?.ToString().Trim();//件數                    
                            htData["WEIGHT"] = ws.Cells[i, 6].Value?.ToString().Trim();//提單重量
                            htData["PRODUCT"] = ws.Cells[i, 7].Value?.ToString().Trim();//品名
                            htData["QUANTITY"] = ws.Cells[i, 8].Value?.ToString().Trim();//數量


                            //檢查袋號是否已匯入過(排除自己)
                            bool IsExist = false;
                            string chksql = $@"SELECT DISTINCT CLEARANCENO FROM T_V_SHIPPING_M A
                                           LEFT JOIN T_V_SHIPPING_H B ON A.ID = B.SHIPPINGID_M
                                           WHERE A.ACCOUNTID = {ACID} AND A.STATUS = 0 AND A.ID != {MID}";

                            DataTable DT = DBUtil.SelectDataTable(chksql);
                            if (DT.Rows.Count > 0)
                            {
                                for (int z = 0; z < DT.Rows.Count; z++)
                                {
                                    if (!string.IsNullOrEmpty(DT.Rows[z]["CLEARANCENO"]?.ToString()))
                                        if (DT.Rows[z]["CLEARANCENO"]?.ToString() == htData["CLEARANCENO"]?.ToString())
                                            IsExist = true;

                                }
                            }

                            if (IsExist)
                                return "error00";


                            //提單重&總件數
                            if (!string.IsNullOrEmpty(htData["PRODUCT"]?.ToString()))
                            {
                                if (string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()) && !string.IsNullOrEmpty(htData["WEIGHT"]?.ToString()))
                                    TotalWG += Convert.ToDecimal(htData["WEIGHT"]);

                                else
                                    TotalWG = Convert.ToDecimal(htData["WEIGHT"]);


                                if (string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()) && !string.IsNullOrEmpty(htData["QUANTITY"]?.ToString()))
                                {
                                    Totalitem += Convert.ToDecimal(htData["QUANTITY"]);
                                    htData["TOTALITEM"] = Totalitem;
                                }
                                else
                                {
                                    Totalitem = Convert.ToDecimal(htData["QUANTITY"]);
                                    htData["TOTALITEM"] = Totalitem;
                                }

                            }

                            htData["UNIT"] = ws.Cells[i, 9].Value?.ToString().Trim();//單位
                            htData["ORIGIN"] = ws.Cells[i, 10].Value?.ToString().Trim();//產地
                            htData["UNITPRICE"] = ws.Cells[i, 11].Value?.ToString().Trim();//單價

                            htData["SHIPPERCOMPANY"] = ws.Cells[i, 12].Value?.ToString().Trim();//寄件公司
                            htData["SHIPPER"] = ws.Cells[i, 13].Value?.ToString().Trim();//寄件人
                            htData["RECEIVERCOMPANY"] = ws.Cells[i, 14].Value?.ToString().Trim();//收件公司
                            htData["RECEIVER"] = ws.Cells[i, 15].Value?.ToString().Trim();//收件人                            
                            htData["RECEIVERPHONE"] = ws.Cells[i, 16].Value?.ToString().Trim();//收件人電話
                            htData["RECEIVERZIPCODE"] = ws.Cells[i, 17].Value?.ToString().Trim();//收件人郵遞區號
                            htData["RECEIVERADDR"] = ws.Cells[i, 18].Value?.ToString().Trim();//收件人地址
                            htData["TAXID"] = ws.Cells[i, 19].Value?.ToString().Trim();//統編or身分證字號
                            htData["SHIPPERREMARK"] = ws.Cells[i, 20].Value?.ToString().Trim();//備註(樂一番用)
                            htData["LOGISTICS"] = ws.Cells[i, 21].Value?.ToString().Trim();//出貨商                            
                            #endregion


                            //if product detail blank then break for loop
                            if (string.IsNullOrEmpty(htData["PRODUCT"]?.ToString()) && string.IsNullOrEmpty(htData["QUANTITY"]?.ToString()) && string.IsNullOrEmpty(htData["UNITPRICE"]?.ToString()))
                            {
                                break;
                            }

                            //new master data & new header data
                            if (!string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()))
                            {

                                htData["SHIPPINGIDM"] = MID;
                                HID = objMember.InsertCusShippingH(htData);
                            }
                            else
                            {
                                htData["SHIPPINGIDH"] = HID;
                                htData["WEIGHT"] = TotalWG;
                                objMember.UpdateCusShippingH(htData);
                            }

                            //New Detail
                            if (MID != 0 && HID != 0)
                            {
                                if (!string.IsNullOrEmpty(htData["PRODUCT"]?.ToString()) && !string.IsNullOrEmpty(htData["QUANTITY"]?.ToString()))
                                {
                                    htData["SHIPPINGIDM"] = MID;
                                    htData["SHIPPINGIDH"] = HID;
                                    objMember.InsertCusShippingD(htData);
                                }
                            }

                            if (MID != 0)
                            {
                                //New Declarant
                                Hashtable sData = new Hashtable();
                                sData["SHIPPINGIDM"] = MID;
                                sData["SHIPPINGIDH"] = HID;
                                sData["NAME"] = htData["RECEIVER"]?.ToString();
                                sData["TAXID"] = htData["TAXID"]?.ToString();
                                sData["PHONE"] = htData["RECEIVERPHONE"]?.ToString();
                                sData["ZIPCODE"] = htData["RECEIVERZIPCODE"]?.ToString();
                                sData["ADDR"] = htData["RECEIVERADDR"]?.ToString();

                                if (!string.IsNullOrEmpty(htData["NEWBOXNO"]?.ToString()))
                                    objMember.InsertTVDeclarant(sData);

                                //check receiver
                                int chk = 0;
                                foreach (var r in Rec)
                                    if (r == htData["RECEIVER"]?.ToString())
                                        chk++;
                                if (chk == 0 && !string.IsNullOrEmpty(htData["RECEIVER"]?.ToString()))
                                    Rec.Add(htData["RECEIVER"]?.ToString());

                                decimal ReceiverCt = Rec.Count();

                                //Update Master Data
                                string WG = ws.Cells[i, 3].Value?.ToString().Trim();
                                string QTY = ws.Cells[i, 8].Value?.ToString().Trim();
                                if (!string.IsNullOrEmpty(WG))
                                    TotalWGM += Convert.ToDecimal(WG);
                                if (!string.IsNullOrEmpty(QTY))
                                    Total += Convert.ToDecimal(QTY);
                                Hashtable tempData = new Hashtable();
                                tempData["SHIPPINGFILE1"] = htData["SHIPPINGFILE1"]?.ToString();
                                tempData["BROKERFILE1"] = htData["BROKERFILE1"]?.ToString();
                                tempData["STORECODE"] = "C2011"; //倉儲代碼
                                tempData["ID"] = MID;
                                tempData["TOTAL"] = Total;
                                tempData["TOTALWEIGHT"] = TotalWGM;
                                tempData["ISMULTRECEIVER"] = ReceiverCt > 1 ? "Y" : "N";
                                // 單一收件人
                                if (ReceiverCt == 1)
                                {
                                    tempData["RECEIVERCOMPANY"] = htData["RECEIVERCOMPANY"]?.ToString();
                                    tempData["RECEIVER"] = htData["RECEIVER"]?.ToString();
                                    tempData["RECEIVERZIPCODE"] = htData["RECEIVERZIPCODE"]?.ToString();
                                    tempData["RECEIVERPHONE"] = htData["RECEIVERPHONE"]?.ToString();
                                    tempData["RECEIVERADDR"] = htData["RECEIVERADDR"]?.ToString();
                                    tempData["TAXID"] = htData["TAXID"]?.ToString();
                                }
                                else
                                {
                                    tempData["RECEIVERCOMPANY"] = "";
                                    tempData["RECEIVER"] = "";
                                    tempData["RECEIVERZIPCODE"] = "";
                                    tempData["RECEIVERPHONE"] = "";
                                    tempData["RECEIVERADDR"] = "";
                                    tempData["TAXID"] = "";
                                }
                                objMember.UpdateCusShippingM(tempData);
                            }
                        }

                        ws.Dispose();

                        //匯入成功回傳集運單號
                        string SHIPPINGNO = DBUtil.GetSingleValue1($@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = {MID}");

                        ts.Complete();


                        return SHIPPINGNO;
                    }

                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                    return "error99";
                }
            }
        }
    }
}