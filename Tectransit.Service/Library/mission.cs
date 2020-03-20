using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tectransit.Service.Library.Tools;
using Tectransit.Service.Bussiness;
using Newtonsoft.Json;

namespace Tectransit.Service.Library
{
    public abstract class mission
    {
        public string _Name = "";
        public bool IsRunning = false;
        public delegate void myCallback(string s);
        private myCallback callback0;

        public mission(string Name)
        {
            _Name = Name;
        }

        public void runn()
        {
            try
            {
                IsRunning = true;
                process();
                //callback0(_Name);
            }
            catch (Exception ex)
            {
                writeLog(ex.ToString());
            }
            finally
            {
                IsRunning = false;
            }
        }

        public void add(myCallback callback1)
        {
            callback0 = callback1;
        }

        public abstract void process();


        protected void Task()
        {
            TransportData();
        }

        protected void Task2()
        {
            TransportEcoData();
        }

        /// <summary>
        ///廠商匯入&異動資料(未入庫)拋轉到厚生倉
        /// </summary>
        protected void TransportData()
        {
            try
            {
                //抓取需要拋轉的紀錄(廠商)
                string sql = $@"SELECT * FROM T_S_DEPOTRECORD 
                            WHERE TYPE = 2 AND ACTIVE IN (0, 2)
                            ORDER BY CREDATE";

                DataTable DT = DBUtil.SelectDataTable(sql);
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        result res = new result();
                        Hashtable recordData = new Hashtable();
                        recordData["ID"] = DT.Rows[i]["ID"];//拋轉紀錄ID
                        recordData["SHIPPINGNO"] = DT.Rows[i]["SHIPPINGNO"];//集運單號
                        
                        #region post資料處理

                        //只傳未入庫(未點收)的資料
                        sql = $@"SELECT ID, ACCOUNTID, SHIPPINGNO, MAWBNO, FLIGHTNUM AS FLIGHTNO,
                                        TOTAL, TOTALWEIGHT, STATUS, ISMULTRECEIVER, RECEIVER, TAXID, RECEIVERPHONE, RECEIVERADDR
                           　    FROM T_V_SHIPPING_M
                           　    WHERE SHIPPINGNO = '{recordData["SHIPPINGNO"]?.ToString()}' AND STATUS = 0";

                        DataTable MasterDT = DBUtil.SelectDataTable(sql);

                        List<data> postData = new List<data>();
                        if (MasterDT.Rows.Count > 0)
                        {
                            data masterData = new data();
                            masterData.accountid = Convert.ToInt64(MasterDT.Rows[0]["ACCOUNTID"]);
                            masterData.accountname = DBUtil.GetSingleValue1($@"SELECT COMPANYNAME AS COL1 FROM T_S_ACCOUNT WHERE ID = {MasterDT.Rows[0]["ACCOUNTID"]?.ToString()}");
                            masterData.shippingno = MasterDT.Rows[0]["SHIPPINGNO"]?.ToString();
                            masterData.mawbno = MasterDT.Rows[0]["MAWBNO"]?.ToString();
                            masterData.flightno = MasterDT.Rows[0]["FLIGHTNO"]?.ToString();
                            masterData.total = MasterDT.Rows[0]["TOTAL"]?.ToString();
                            masterData.totalweight = MasterDT.Rows[0]["TOTALWEIGHT"]?.ToString();
                            
                            sql = $@"SELECT A.ID AS HID, A.CLEARANCENO, A.TRANSFERNO, A.WEIGHT, A.TOTALITEM, A.RECEIVER, A.TAXID, A.RECEIVERPHONE, A.RECEIVERADDR
                                     FROM T_V_SHIPPING_H A
									 WHERE A.SHIPPINGID_M = {MasterDT.Rows[0]["ID"]?.ToString()}
                                     ORDER BY A.CLEARANCENO";

                            DataTable hData = DBUtil.SelectDataTable(sql);
                            List<dataItems> itemsList = new List<dataItems>();
                            if (hData.Rows.Count > 0)
                            {
                                for (int k = 0; k < hData.Rows.Count; k++)
                                {
                                    dataItems item = new dataItems();
                                    item.clearanceno = hData.Rows[k]["CLEARANCENO"]?.ToString();
                                    item.transferno = hData.Rows[k]["TRANSFERNO"]?.ToString();
                                    item.weight = hData.Rows[k]["WEIGHT"]?.ToString();
                                    item.totalitem = hData.Rows[k]["TOTALITEM"]?.ToString();
                                    if (Convert.ToBoolean(MasterDT.Rows[0]["ISMULTRECEIVER"]) == true)
                                    {
                                        item.receiver = hData.Rows[k]["RECEIVER"]?.ToString();
                                        item.receiveraddr = hData.Rows[k]["RECEIVERADDR"]?.ToString();
                                        item.receiverphone = hData.Rows[k]["RECEIVERPHONE"]?.ToString();
                                        item.taxid = hData.Rows[k]["TAXID"]?.ToString();
                                    }
                                    else
                                    {
                                        item.receiver = MasterDT.Rows[0]["RECEIVER"]?.ToString();
                                        item.receiveraddr = MasterDT.Rows[0]["RECEIVERADDR"]?.ToString();
                                        item.receiverphone = MasterDT.Rows[0]["RECEIVERPHONE"]?.ToString();
                                        item.taxid = MasterDT.Rows[0]["TAXID"]?.ToString();
                                    }
                                    
                                    sql = $@"SELECT PRODUCT, QUANTITY, UNITPRICE, UNIT, ORIGIN
									         FROM T_V_SHIPPING_D
									         WHERE SHIPPINGID_H = {hData.Rows[k]["HID"]}";
                                    DataTable dData = DBUtil.SelectDataTable(sql);
                                    List<dataDetail> detailList = new List<dataDetail>();
                                    if (dData.Rows.Count > 0)
                                    {
                                        for (int m = 0; m < dData.Rows.Count; m++)
                                        {
                                            dataDetail detail = new dataDetail();
                                            detail.product = dData.Rows[m]["PRODUCT"]?.ToString();
                                            detail.quantity = Convert.ToInt32(dData.Rows[m]["QUANTITY"]);
                                            detail.unit = dData.Rows[m]["UNIT"]?.ToString();
                                            detail.unitprice = Convert.ToInt32(dData.Rows[m]["UNITPRICE"]);
                                            detail.origin = dData.Rows[m]["ORIGIN"]?.ToString();

                                            detailList.Add(detail);
                                        }
                                    }

                                    item.detail = detailList;
                                    itemsList.Add(item);
                                }

                                masterData.items = itemsList;
                                postData.Add(masterData);
                            }

                            //執行拋轉
                            ShipmentTransfer objShip = new ShipmentTransfer();
                            if (postData != null)
                            {
                                res = objShip.getData(postData);

                                //更新拋轉紀錄
                                string postjson = JsonConvert.SerializeObject(postData);
                                objShip.UpdateRecord(Convert.ToInt64(recordData["ID"]), postjson, res);

                                writeLog("拋轉成功！");

                            }

                        }
                        #endregion


                    }
                }
                else { writeLog("沒有需要拋轉的資料！"); }
            }
            catch (Exception ex)
            {
                writeLog(ex.Message.ToString());
                writeLog("拋轉失敗！");
            }
        }

        /// <summary>
        ///廠商託運單號(嘉里大榮)拋轉到台空貨況
        /// </summary>
        protected void TransportEcoData()
        {
            try
            {
                //抓取需要拋轉的紀錄(廠商)
                string sql = $@"SELECT * FROM T_S_TECTRACKRECORD 
                            WHERE TYPE = 2 AND ACTIVE IN (0, 2)
                            ORDER BY CREDATE";

                DataTable DT = DBUtil.SelectDataTable(sql);
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Ecoresult res = new Ecoresult();
                        Hashtable recordData = new Hashtable();
                        recordData["ID"] = DT.Rows[i]["ID"];//拋轉紀錄ID
                        recordData["SHIPPINGNO"] = DT.Rows[i]["SHIPPINGNO"];//集運單號

                        #region Post資料處理
                        List<Ecodata> postData = new List<Ecodata>();
                        string[] SNOList = recordData["SHIPPINGNO"]?.ToString().Split(';');
                        if (SNOList.Length > 0)
                        {
                            for (int j = 0; j < SNOList.Length; j++)
                            {
                                sql = $@"SELECT A.SHIPPINGNO, FORMAT(A.UPDDATE, 'yyyy-MM-dd HH:mm:ss') AS SHIPPINGDATE, B.TRACKINGNO 
                                         FROM T_V_SHIPPING_M A 
                                         LEFT JOIN T_V_SHIPPING_H B ON A.ID = B.SHIPPINGID_M
                                         WHERE SHIPPINGNO = '{SNOList[j]}' AND B.TRACKSTATUS IN ('0')";

                                DataTable MasterDT = DBUtil.SelectDataTable(sql);                                
                                if (MasterDT.Rows.Count > 0)
                                {
                                    for (int k = 0; k < MasterDT.Rows.Count; k++)
                                    {
                                        if (!string.IsNullOrEmpty(MasterDT.Rows[k]["TRACKINGNO"]?.ToString()))
                                        {
                                            Ecodata masterData = new Ecodata();
                                            masterData.TRACKINGNO = MasterDT.Rows[k]["TRACKINGNO"]?.ToString();
                                            masterData.CUSTOMERID = "2"; //1:颿達/2:嘉里大榮

                                            //貨況細項
                                            List<EcoItems> DetailList = new List<EcoItems>();
                                            EcoItems detailData = new EcoItems();
                                            detailData.TWDATE = MasterDT.Rows[k]["SHIPPINGDATE"]?.ToString();
                                            detailData.TWPLACE = "桃園";
                                            detailData.TWSTATUS = "已清關";
                                            detailData.ENDATE = detailData.TWDATE;
                                            detailData.ENPLACE = "Taoyuan";
                                            detailData.ENSTATUS = "the clearance of goods";


                                            DetailList.Add(detailData);
                                            masterData.DETAIL = DetailList;

                                            postData.Add(masterData);
                                        }
                                    }
                                }

                            }

                            //執行拋轉
                            EcoTransfer objEco = new EcoTransfer();
                            if (postData != null)
                            {
                                res = objEco.getData(postData);

                                //更新拋轉紀錄
                                string postjson = JsonConvert.SerializeObject(postData);
                                objEco.UpdateTectrackRecord(Convert.ToInt64(recordData["ID"]), postjson, res);

                                //拋轉成功--->更新託運單狀態(避免已建立的託運單又被重拋)
                                if (res.status == 0)
                                {
                                    for (int m = 0; m < postData.Count; m++)
                                    {
                                        Hashtable htData = new Hashtable();
                                        htData["TRACKINGNO"] = postData[m].TRACKINGNO;
                                        htData["TRACKSTATUS"] = "1";
                                        objEco.UpdateShippingCusH(htData);
                                    }
                                }

                                writeLog("拋轉成功！");

                            }

                        }
                        #endregion
                    }
                }
                else { writeLog("沒有需要拋轉的資料！"); }
            }
            catch (Exception ex)
            {
                writeLog(ex.Message.ToString());
                writeLog("拋轉失敗！");
            }
        }
    }
}
