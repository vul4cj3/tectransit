using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tectransit.Service.Library;
using static Tectransit.Service.Library.Tools;
using System.Web.Script.Serialization;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace Tectransit.Service.Bussiness
{
    public class TRANSDEPOT:mission
    {
        public TRANSDEPOT() : base("TRANSDEPOT")
        {
        }

        public override void process()
        {
            writeLog("=======" + DateTime.Now.ToString() + "=======", base._Name);
            writeLog("開始執行", base._Name);
            Task();
            writeLog("結束執行", base._Name);
        }
    }

    public class ShipmentTransfer
    {
        public result getData(List<data> datalist)
        {
            try
            {
                string sUrl = $@"http://192.168.11.158:90/Areas/Express/Express.asmx/PostExpressDataJSON";
                var postData = JsonConvert.SerializeObject(datalist);

                result objResponse = new result();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                
                string postParams = "json=" + postData;

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
                        var xml = XDocument.Parse(temp);
                        
                        objResponse = JsonConvert.DeserializeObject<result>(xml.Root.Value);                        
                    }
                }
                return objResponse;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                result res = null;
                return res;
            }
        }

        //更新拋轉紀錄
        public void UpdateRecord(long id, string postjson, result res)
        {
            Hashtable sData = new Hashtable();
            sData["ID"] = id;
            sData["ACTIVE"] = res.status == 0 ? 1 : 2; //1:拋轉成功/2:拋轉失敗
            sData["APIURL"] = "http://192.168.11.158:90/Areas/Express/Express.asmx/PostExpressDataJSON";
            sData["SENDDATA"] = postjson; //傳送json資料(request)            
            sData["STATUS"] = res.status; //回傳結果:狀態            
            sData["MSG"] = res.msg; //回傳結果:狀態說明            
            sData["RESPONSEDATA"] = res.errormsg;//回傳結果:99-紀錄errmsg
            sData["UPDDATE"] = DateTime.Now;//更新時間

            string sql = $@"UPDATE T_S_DEPOTRECORD SET
                                 ACTIVE = @ACTIVE,
                                 APIURL = @APIURL,
                                 SENDDATA = @SENDDATA,
                                 STATUS = @STATUS,
                                 MSG = @MSG,
                                 RESPONSEDATA = @RESPONSEDATA,
                                 UPDDATE = @UPDDATE                                 
                            WHERE ID = @ID";

            DBUtil.EXECUTE(sql, sData);
        }

    }

    //Postdata
    public class data
    {
        public int number { get; set; }
        public string mawbno { get; set; }
        public string flightno { get; set; }
        public string clearanceno { get; set; }
        public string totalweight { get; set; }
        public string total { get; set; }
        public List<dataItems> items { get; set; }

    }

    public class dataItems
    {
        public string transferno { get; set; }
        public string weight { get; set; }
        public string receiver { get; set; }
        public string receiveraddr { get; set; }
        public string receiverphone { get; set; }
        public string taxid { get; set; }
        public List<dataDetail> detail { get; set; }

    }

    public class dataDetail
    {
        public string product { get; set; }
        public decimal quantity { get; set; }
        public string unit { get; set; }
        public decimal unitprice { get; set; }
        public string origin { get; set; }

    }

    //回傳結果
    public class result
    {
        public int status { get; set; }
        public string msg { get; set; }
        public string errormsg { get; set; }

    }

}
