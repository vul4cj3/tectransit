using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tectransit.Datas;

namespace Tectransit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepotCheckController : ControllerBase
    {

        //倉庫點收API
        [HttpPost]
        public dynamic Post([FromBody]object json)
        {
            try
            {
                var jsonData = JObject.FromObject(json);
                JArray arrData = jsonData.Value<JArray>("json");

                #region 資料處理
                ArrayList AL = new ArrayList();
                for (int i = 0; i < arrData.Count; i++)
                {
                    JObject temp = (JObject)arrData[i];
                    Hashtable hData = new Hashtable();
                    foreach (var t in temp)
                    {
                        //Detail(product) data
                        if (t.Key == "detail")
                        {
                            JArray prdData = temp.Value<JArray>("detail");
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
                            hData["HEADER"] = subAL;

                        }
                        else
                            hData[(t.Key).ToUpper()] = t.Value?.ToString();

                    }
                    AL.Add(hData);
                }
                #endregion


                #region 更新提單狀態
                int UPDNum = 0;//有更新到的提單數
                string MAWBNO = "";
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

                                // 查詢主單號下的袋號提單
                                sql = $@"SELECT ID, TRANSFERNO, STATUS 
                                 FROM T_V_SHIPPING_M
                                 WHERE MAWBNO = @MAWBNO AND CLEARANCENO = @CLEARANCENO AND TRANSFERNO = @TRANSFERNO";

                                DataTable DT = DBUtil.SelectDataTable(sql, sData);
                                if (DT.Rows.Count > 0)
                                {
                                    mData["ID"] = DT.Rows[0]["ID"];
                                    //更新提單點收狀態
                                    string res = UpdateShippingCusState(mData);

                                    if (res == "OK")
                                        UPDNum++;
                                }
                            }
                        }

                        // 檢查是否有沒傳到的提單(全部改為已入庫/備註:未收到點收紀錄)
                        sql = $@"SELECT ID, TRANSFERNO, STATUS 
                                 FROM T_V_SHIPPING_M
                                 WHERE MAWBNO = @MAWBNO AND CLEARANCENO = @CLEARANCENO AND STATUS = 0";

                        DataTable DT_N = DBUtil.SelectDataTable(sql, sData);
                        if (DT_N.Rows.Count > 0)
                        {
                            for (int k = 0; k < DT_N.Rows.Count; k++)
                            {
                                Hashtable mData = new Hashtable();
                                mData["ID"] = DT_N.Rows[k]["ID"];
                                mData["STATE"] = "0";

                                string res = UpdateShippingCusState(mData);
                            }
                        }


                        MAWBNO = sData["MAWBNO"]?.ToString();
                    }

                    if (UPDNum > 0)
                        return new { status = 0, msg = "成功", error = "" };
                    else
                        return new { status = 99, msg = "失敗", error = MAWBNO };

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


        private string UpdateShippingCusState(Hashtable sData)
        {
            sData["STATUS"] = 1;//訂單狀態
            //備註:點收狀態說明
            if (sData["STATE"]?.ToString() == "1")
                sData["REMARK1"] = "已點收到";
            else if (sData["STATE"]?.ToString() == "2")
                sData["REMARK1"] = "未點收到";
            else if (sData["STATE"]?.ToString() == "3")
                sData["REMARK1"] = "有多餘的點收";
            else
                sData["REMARK1"] = "未收到倉庫端的點收資料";

            sData["UPDBY"] = "SYSTEM";
            sData["UPDDATE"] = DateTime.Now;

            string sql = $@"UPDATE T_V_SHIPPING_M SET
                                   STATUS = @STATUS,
                                   DEPOTSTATUS = @STATE,
                                   REMARK1 = @REMARK1,
                                   UPDBY = @UPDBY,
                                   UPDDATE = @UPDDATE
                             WHERE ID = @ID";

            DBUtil.EXECUTE(sql, sData);

            return "OK";
        }

    }

}