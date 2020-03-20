using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tectransit.Service.Library;
using static Tectransit.Service.Library.Tools;

namespace Tectransit.Service.Bussiness
{
    public class TRANSTECECO : mission
    {
        public TRANSTECECO() : base("TRANSTECECO")
        {
        }

        public override void process()
        {
            writeLog("=======" + DateTime.Now.ToString() + "=======", base._Name);
            writeLog("開始執行", base._Name);
            Task2();
            writeLog("結束執行", base._Name);
        }
    }

    public class EcoTransfer
    {
        public Ecoresult getData(List<Ecodata> datalist)
        {
            try
            {
                string sUrl = $@"http://e-commerce.t3ex-tec.com/Api/TransitCheckApi/GetTrackingData";
                var postData = JsonConvert.SerializeObject(datalist);

                Ecoresult objResponse = new Ecoresult();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                request.ContentType = "application/json";
                request.Method = "POST";

                string postParams = postData;

                byte[] byteArray = Encoding.UTF8.GetBytes(postParams);//要發送的字串轉為byte[]

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                }

                using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
                {

                    if (reader != null)
                    {
                        string temp = reader.ReadToEnd();

                        objResponse = JsonConvert.DeserializeObject<Ecoresult>(temp);
                    }
                }
                return objResponse;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                Ecoresult res = null;
                return res;
            }
        }

        //更新拋轉紀錄
        public void UpdateTectrackRecord(long id, string postjson, Ecoresult res)
        {
            Hashtable sData = new Hashtable();
            sData["ID"] = id;
            sData["ACTIVE"] = res.status == 0 ? 1 : 2; //1:拋轉成功/2:拋轉失敗/3:其他
            if (res.status == 99 && res.error.IndexOf("託運單號已存在") > 0)
            {
                sData["ACTIVE"] = 3;
                sData["REMARK"] = "台空貨況已存在相同單號，不再進行拋轉";
            }
            sData["APIURL"] = "http://e-commerce.t3ex-tec.com/Api/TransitCheckApi/GetTrackingData";
            sData["SENDDATA"] = postjson; //傳送json資料(request)            
            sData["STATUS"] = res.status; //回傳結果:狀態            
            sData["MSG"] = res.msg; //回傳結果:狀態說明            
            sData["RESPONSEDATA"] = res.error;//回傳結果:99-紀錄errmsg
            sData["UPDDATE"] = DateTime.Now;//更新時間

            string sql = $@"UPDATE T_S_TECTRACKRECORD SET
                                 ACTIVE = @ACTIVE,
                                 APIURL = @APIURL,
                                 SENDDATA = @SENDDATA,
                                 STATUS = @STATUS,
                                 MSG = @MSG,
                                 RESPONSEDATA = @RESPONSEDATA,
                                 REMARK = @REMARK,
                                 UPDDATE = @UPDDATE                                 
                            WHERE ID = @ID";

            DBUtil.EXECUTE(sql, sData);
        }

        //更新託運單拋轉狀態
        public void UpdateShippingCusH(Hashtable sData)
        {
            string sql = $@"UPDATE T_V_SHIPPING_H SET
                                 TRACKSTATUS = @TRACKSTATUS                                
                            WHERE TRACKINGNO = @TRACKINGNO AND DEPOTSTATUS = '1'";

            DBUtil.EXECUTE(sql, sData);
        }

    }
}
