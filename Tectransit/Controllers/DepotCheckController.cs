using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tectransit.Datas;

namespace Tectransit.Controllers
{
    [Route("api/Depot/[action]")]
    public class DepotCheckController : ControllerBase
    {

        //倉庫點收API
        [HttpPost]
        public dynamic GetCheck([FromBody]object json)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var jsonData = JObject.FromObject(json);
                    JArray arrData = jsonData.Value<JArray>("json");

                    #region 資料處理
                    Dictionary<string, string> DC = new Dictionary<string, string>();
                    DC.Add("SHIPPING_NO", "SHIPPINGNO");
                    DC.Add("ACCOUNT_ID", "ACCOUNTID");
                    DC.Add("ACCOUNT_NAME", "ACCOUNTNAME");
                    DC.Add("TRANSFER_NO", "TRANSFERNO");
                    DC.Add("STATE", "STATE");


                    ArrayList AL = new ArrayList();
                    for (int i = 0; i < arrData.Count; i++)
                    {
                        JObject temp = (JObject)arrData[i];
                        Hashtable hData = new Hashtable();
                        foreach (var t in temp)
                        {
                            //Detail(product) data
                            if (t.Key == "DETAIL")
                            {
                                JArray prdData = temp.Value<JArray>("DETAIL");
                                ArrayList subAL = new ArrayList();
                                for (int j = 0; j < prdData.Count; j++)
                                {
                                    JObject temp2 = (JObject)prdData[j];
                                    Hashtable dData = new Hashtable();
                                    foreach (var t2 in temp2)
                                    {
                                        dData[DC[(t2.Key).ToUpper()]] = t2.Value?.ToString();
                                    }
                                    subAL.Add(dData);
                                }
                                hData["HEADER"] = subAL;

                            }
                            else
                                hData[DC[(t.Key).ToUpper()]] = t.Value?.ToString();

                        }
                        AL.Add(hData);
                    }
                    #endregion

                    #region 更新提單狀態
                    int UPDNum = 0;//有更新到的提單數
                    string SHIPPINGNO = "";
                    string sql = "";
                    if (AL.Count > 0)
                    {
                        for (int i = 0; i < AL.Count; i++)
                        {
                            Hashtable sData = (Hashtable)AL[i];
                            ArrayList subAL = (ArrayList)sData["HEADER"];

                            if (subAL.Count > 0)
                            {
                                for (int j = 0; j < subAL.Count; j++)
                                {
                                    Hashtable mData = (Hashtable)subAL[j];
                                    sData["TRANSFERNO"] = mData["TRANSFERNO"];

                                    // 查詢該會員是否有此集運單號和提單
                                    sql = $@"SELECT A.ID AS MID, A.STATUS, B.ID AS HID, B.TRANSFERNO, B.DEPOTSTATUS 
                                         FROM T_V_SHIPPING_M A
                                         LEFT JOIN T_V_SHIPPING_H B ON A.ID = B.SHIPPINGID_M
                                         WHERE SHIPPINGNO = @SHIPPINGNO AND ACCOUNTID = @ACCOUNTID AND TRANSFERNO = @TRANSFERNO";

                                    DataTable DT = DBUtil.SelectDataTable(sql, sData);
                                    if (DT.Rows.Count > 0)
                                    {
                                        mData["SHIPPINGIDM"] = DT.Rows[0]["MID"];
                                        mData["ID"] = DT.Rows[0]["HID"];

                                        string res = UpdateShippingCusHState(mData);

                                        if (res == "OK")
                                            UPDNum++;

                                    }
                                }

                                // 檢查是否有沒傳到的提單(全部改為已入庫/備註:未收到點收紀錄)
                                sql = $@"SELECT B.ID, B.TRANSFERNO, B.DEPOTSTATUS
                                     FROM T_V_SHIPPING_M A
                                     LEFT JOIN T_V_SHIPPING_H B ON A.ID = B.SHIPPINGID_M
                                     WHERE A.SHIPPINGNO = @SHIPPINGNO AND DEPOTSTATUS IS NULL";

                                DataTable DT_N = DBUtil.SelectDataTable(sql, sData);
                                if (DT_N.Rows.Count > 0)
                                {
                                    for (int k = 0; k < DT_N.Rows.Count; k++)
                                    {
                                        Hashtable mData = new Hashtable();
                                        mData["ID"] = DT_N.Rows[k]["ID"];
                                        mData["STATE"] = "0";

                                        string res = UpdateShippingCusHState(mData);
                                    }
                                }
                            }

                            //更新集運單狀態
                            sData["STATUS"] = 1;//訂單狀態(已入庫)
                            UpdateShippingCusMState(sData);

                            SHIPPINGNO = sData["SHIPPINGNO"]?.ToString();
                        }

                        if (UPDNum > 0)
                        {
                            ts.Complete();
                            return new { status = 0, msg = "成功", error = "" };
                        }
                        else
                        {
                            return new { status = 99, msg = "失敗", error = SHIPPINGNO };
                        }

                    }
                    else
                    {
                        return new { status = 99, msg = "失敗", error = "無提單資料可更新！" };
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                    return new { status = 99, msg = "失敗", error = json.ToString() };
                }
            }

        }

        //接收大榮提單資訊API(倉庫)
        [HttpPost]
        public dynamic GetTracking([FromBody]object json)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var jsonData = JObject.FromObject(json);
                    JArray arrData = jsonData.Value<JArray>("json");

                    #region 資料處理
                    Dictionary<string, string> DC = new Dictionary<string, string>();
                    DC.Add("SHIPPING_NO", "SHIPPINGNO");
                    //DC.Add("ACCOUNT_ID", "ACCOUNTID");
                    //DC.Add("ACCOUNT_NAME", "ACCOUNTNAME");
                    DC.Add("TRANSFER_NO", "TRANSFERNO");
                    DC.Add("TRACKING_NO", "TRACKINGNO");


                    ArrayList AL = new ArrayList();
                    for (int i = 0; i < arrData.Count; i++)
                    {
                        JObject temp = (JObject)arrData[i];
                        Hashtable hData = new Hashtable();
                        foreach (var t in temp)
                        {
                            //Detail(product) data
                            if (t.Key == "DETAIL")
                            {
                                JArray prdData = temp.Value<JArray>("DETAIL");
                                ArrayList subAL = new ArrayList();
                                for (int j = 0; j < prdData.Count; j++)
                                {
                                    JObject temp2 = (JObject)prdData[j];
                                    Hashtable dData = new Hashtable();
                                    foreach (var t2 in temp2)
                                    {
                                        dData[DC[(t2.Key).ToUpper()]] = t2.Value?.ToString();
                                    }
                                    subAL.Add(dData);
                                }
                                hData["HEADER"] = subAL;

                            }
                            else
                                hData[DC[(t.Key).ToUpper()]] = t.Value?.ToString();

                        }
                        AL.Add(hData);
                    }
                    #endregion

                    #region 更新提單-->託運單號資訊(大榮)
                    int UPDNum = 0;//有更新到的提單數
                    string SHIPPINGNO = "";
                    string sql = "";
                    if (AL.Count > 0)
                    {
                        for (int i = 0; i < AL.Count; i++)
                        {
                            Hashtable sData = (Hashtable)AL[i];
                            ArrayList subAL = (ArrayList)sData["HEADER"];

                            if (subAL.Count > 0)
                            {
                                for (int j = 0; j < subAL.Count; j++)
                                {
                                    Hashtable mData = (Hashtable)subAL[j];
                                    sData["TRANSFERNO"] = mData["TRANSFERNO"];

                                    // 查詢該會員是否有此集運單號和提單
                                    sql = $@"SELECT A.ID AS MID, A.STATUS, B.ID AS HID, B.TRANSFERNO, B.DEPOTSTATUS 
                                         FROM T_V_SHIPPING_M A
                                         LEFT JOIN T_V_SHIPPING_H B ON A.ID = B.SHIPPINGID_M
                                         WHERE SHIPPINGNO = @SHIPPINGNO AND TRANSFERNO = @TRANSFERNO";

                                    DataTable DT = DBUtil.SelectDataTable(sql, sData);
                                    if (DT.Rows.Count > 0)
                                    {
                                        mData["SHIPPINGIDM"] = DT.Rows[0]["MID"];
                                        mData["ID"] = DT.Rows[0]["HID"];

                                        string res = UpdateShippingCusHTrack(mData);

                                        if (res == "OK")
                                            UPDNum++;

                                    }
                                }

                                //更新集運單狀態
                                sData["STATUS"] = 3;//訂單狀態(已出貨)
                                UpdateShippingCusMState(sData);

                                SHIPPINGNO += (SHIPPINGNO == "" ? "" : ";") + sData["SHIPPINGNO"]?.ToString();
                            }
                        }

                        if (UPDNum > 0)
                        {
                            ts.Complete();                            
                            return new { status = 0, msg = "成功", error = "" };
                        }
                        else
                        {
                            return new { status = 99, msg = "失敗", error = SHIPPINGNO };
                        }

                    }
                    else
                    {
                        return new { status = 99, msg = "失敗", error = "無提單資料可更新！" };
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                    return new { status = 99, msg = "失敗", error = json.ToString() };
                }
            }
        }


        private string UpdateShippingCusMState(Hashtable sData)
        {

            sData["UPDBY"] = "SYSTEM";
            sData["UPDDATE"] = DateTime.Now;

            string sql = $@"UPDATE T_V_SHIPPING_M SET
                                   STATUS = @STATUS,
                                   UPDBY = @UPDBY,
                                   UPDDATE = @UPDDATE
                             WHERE SHIPPINGNO = @SHIPPINGNO";

            DBUtil.EXECUTE(sql, sData);

            return "OK";
        }

        private string UpdateShippingCusHState(Hashtable sData)
        {
            //備註:點收狀態說明
            if (sData["STATE"]?.ToString() == "1")
                sData["REMARK1"] = "已點收到";
            else if (sData["STATE"]?.ToString() == "2")
                sData["REMARK1"] = "未點收到";
            else if (sData["STATE"]?.ToString() == "3")
                sData["REMARK1"] = "有多餘的點收";
            else
                sData["REMARK1"] = "未收到倉庫端的點收資料";            

            string sql = $@"UPDATE T_V_SHIPPING_H SET
                                   DEPOTSTATUS = @STATE,
                                   REMARK1 = @REMARK1
                             WHERE ID = @ID";            

            DBUtil.EXECUTE(sql, sData);

            return "OK";
        }

        private string UpdateShippingCusHTrack(Hashtable sData)
        {
            //備註:出貨商
            sData["REMARK2"] = "嘉里大榮";
            
            string sql = $@"UPDATE T_V_SHIPPING_H SET
                                   TRACKINGNO = @TRACKINGNO,
                                   REMARK2 = @REMARK2
                             WHERE ID = @ID";

            DBUtil.EXECUTE(sql, sData);

            return "OK";
        }

    }

}