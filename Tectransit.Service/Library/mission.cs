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
                        recordData["MAWBNO"] = DT.Rows[i]["SHIPPINGNO"];//主單號
                        
                        #region post資料處理

                        //只傳未入庫(未點收)的資料
                        sql = $@"SELECT MAWBNO, FLIGHTNUM AS FLIGHTNO, CLEARANCENO, TOTALWEIGHT, STATUS
                           　FROM T_V_SHIPPING_M
                           　WHERE MAWBNO = '{recordData["MAWBNO"]?.ToString()}' AND STATUS = 0
                           　GROUP BY MAWBNO, FLIGHTNUM, CLEARANCENO, TOTALWEIGHT, STATUS";

                        DataTable MasterDT = DBUtil.SelectDataTable(sql);
                        List<data> postData = new List<data>();

                        if (MasterDT.Rows.Count > 0)
                        {
                            int num = 0;
                            for (int j = 0; j < MasterDT.Rows.Count; j++)
                            {
                                data masterD = new data();
                                masterD.number = ++num;
                                masterD.mawbno = MasterDT.Rows[j]["MAWBNO"]?.ToString();
                                masterD.flightno = MasterDT.Rows[j]["FLIGHTNO"]?.ToString();
                                masterD.clearanceno = MasterDT.Rows[j]["CLEARANCENO"]?.ToString();
                                masterD.totalweight = MasterDT.Rows[j]["TOTALWEIGHT"]?.ToString();
                                masterD.total = DBUtil.GetSingleValue1($@"SELECT COUNT(TRANSFERNO) AS COL1 FROM T_V_SHIPPING_M WHERE CLEARANCENO = '{MasterDT.Rows[j]["CLEARANCENO"]}'");

                                sql = $@"SELECT A.ID AS HID, A.TRANSFERNO, B.WEIGHT, D.NAME, D.TAXID, D.PHONE, D.ADDR
                                     FROM T_V_SHIPPING_M A
                                     LEFT JOIN T_V_SHIPPING_H B ON A.ID = B.SHIPPINGID_M
                                     LEFT JOIN T_V_DECLARANT D  ON A.ID = D.SHIPPINGID_M
                                     WHERE A.CLEARANCENO = '{MasterDT.Rows[j]["CLEARANCENO"]}'
                                     ORDER BY A.TRANSFERNO";

                                DataTable hData = DBUtil.SelectDataTable(sql);
                                List<dataItems> itemsList = new List<dataItems>();
                                if (hData.Rows.Count > 0)
                                {
                                    for (int k = 0; k < hData.Rows.Count; k++)
                                    {
                                        dataItems item = new dataItems();
                                        item.transferno = hData.Rows[k]["TRANSFERNO"]?.ToString();
                                        item.weight = hData.Rows[k]["WEIGHT"]?.ToString();
                                        item.receiver = hData.Rows[k]["NAME"]?.ToString();
                                        item.receiveraddr = hData.Rows[k]["ADDR"]?.ToString();
                                        item.receiverphone = hData.Rows[k]["PHONE"]?.ToString();
                                        item.taxid = hData.Rows[k]["TAXID"]?.ToString();

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
                                                detail.quantity = Convert.ToDecimal(dData.Rows[m]["QUANTITY"]);
                                                detail.unit = dData.Rows[m]["UNIT"]?.ToString();
                                                detail.unitprice = Convert.ToDecimal(dData.Rows[m]["UNITPRICE"]);
                                                detail.origin = dData.Rows[m]["ORIGIN"]?.ToString();

                                                detailList.Add(detail);
                                            }
                                        }

                                        item.detail = detailList;
                                        itemsList.Add(item);
                                    }
                                }

                                masterD.items = itemsList;
                                postData.Add(masterD);
                            }
                            
                        }

                        #endregion

                        //執行拋轉
                        ShipmentTransfer objShip = new ShipmentTransfer();
                        if (postData.Count > 0)
                        {
                            res = objShip.getData(postData);
                            
                            //更新拋轉紀錄
                            string postjson = JsonConvert.SerializeObject(postData);
                            objShip.UpdateRecord(Convert.ToInt64(recordData["ID"]), postjson, res);

                        }
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
