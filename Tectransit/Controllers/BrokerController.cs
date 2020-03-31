using System;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Tectransit.Datas;
using Tectransit.Modles;

namespace Tectransit.Controllers
{
    [Route("api/broker/[action]")]
    public class BrokerController : Controller
    {
        private readonly TECTRANSITDBContext _context;
        BrokerHelper objBroker = new BrokerHelper();

        public BrokerController(TECTRANSITDBContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public dynamic GetShippingCusIMBRData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                var jsonData = JObject.FromObject(form);
                int pageIndex = jsonData.Value<int>("PAGE_INDEX");
                int pageSize = jsonData.Value<int>("PAGE_SIZE");
                JArray srhForm = jsonData.Value<JArray>("srhForm");
                Hashtable htData = new Hashtable();
                if (srhForm.Count > 0)
                {
                    JObject temp = (JObject)srhForm[0];
                    foreach (var t in temp)
                        htData[t.Key.ToUpper()] = t.Value?.ToString();
                }
                htData["_cuscode"] = Request.Cookies["_cuscode"];
                htData["_cusname"] = Request.Cookies["_cusname"];

                htData["IMBROKERID"] = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = @_cuscode", htData);

                if (!string.IsNullOrEmpty(htData["CRESDATE"]?.ToString()) && !string.IsNullOrEmpty(htData["CREEDATE"]?.ToString()))
                {
                    sWhere += $" AND (CREDATE BETWEEN '{htData["CRESDATE"]?.ToString()} 00:00:00' AND '{htData["CREEDATE"]?.ToString()} 23:59:59')";
                }

                return objBroker.GetBrokerData(sWhere, htData, pageIndex, pageSize);
                
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = 99, msg = "取得失敗！" };
            }
        }

        [HttpPost]
        public dynamic GetShippingCusEXBRData([FromBody] object form)
        {
            try
            {
                string sWhere = "";
                var jsonData = JObject.FromObject(form);
                int pageIndex = jsonData.Value<int>("PAGE_INDEX");
                int pageSize = jsonData.Value<int>("PAGE_SIZE");
                JArray srhForm = jsonData.Value<JArray>("srhForm");
                Hashtable htData = new Hashtable();
                if (srhForm.Count > 0)
                {
                    JObject temp = (JObject)srhForm[0];
                    foreach (var t in temp)
                        htData[t.Key.ToUpper()] = t.Value?.ToString();
                }
                
                htData["_cuscode"] = Request.Cookies["_cuscode"];
                htData["_cusname"] = Request.Cookies["_cusname"];

                htData["EXBROKERID"] = DBUtil.GetSingleValue1($@"SELECT ID AS COL1 FROM T_S_ACCOUNT WHERE USERCODE = @_cuscode", htData);

                if (!string.IsNullOrEmpty(htData["CRESDATE"]?.ToString()) && !string.IsNullOrEmpty(htData["CREEDATE"]?.ToString()))
                {
                    sWhere += $" AND (CREDATE BETWEEN '{htData["CRESDATE"]?.ToString()} 00:00:00' AND '{htData["CREEDATE"]?.ToString()} 23:59:59')";
                }

                return objBroker.GetBrokerData(sWhere, htData, pageIndex, pageSize);

            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = 99, msg = "取得失敗！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic ExportBroker(long id)
        {
            try
            {
                string result = "";
                Hashtable htData = new Hashtable();
                htData["ID"] = id;
                string SHIPPINGNO = DBUtil.GetSingleValue1(@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID AND STATUS = 0", htData);

                string file1 = DBUtil.GetSingleValue1(@"SELECT SHIPPINGFILE1 AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID AND STATUS = 0", htData);
                string file2 = DBUtil.GetSingleValue1(@"SELECT SHIPPINGFILE2 AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID AND STATUS = 0", htData);
                string filename = "";

                if (string.IsNullOrEmpty(file2))
                    filename = file1.Replace("/", @"\").Replace("res", @"tectransit\dist\tectransit");
                else
                    filename = file2.Replace("/", @"\").Replace("res", @"tectransit\dist\tectransit");
                
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), filename);

                string copyfile = $"CusBroker_{SHIPPINGNO}.xlsx";
                string copypath = Path.Combine(Directory.GetCurrentDirectory(), $@"tectransit\dist\tectransit\assets\temp\");

                if (!Directory.Exists(copypath))
                    Directory.CreateDirectory(copypath);

                //檢查同檔名是否已存在過--->移除
                if (System.IO.File.Exists(copypath + copyfile))
                    System.IO.File.Delete(copypath + copyfile);

                System.IO.File.Copy(templatePath, copypath + copyfile);
                FileInfo newFile = new FileInfo(copypath + copyfile);

                using (ExcelPackage ep = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = ep.Workbook.Worksheets[1];

                    #region excel 資料過程

                    string sql = $@"SELECT SHIPPINGNO, FLIGHTNUM, MAWBNO
                                    FROM T_V_SHIPPING_M
                                    WHERE ID = @ID AND STATUS = 0";

                    DataTable DT = DBUtil.SelectDataTable(sql, htData);
                    if (DT.Rows.Count > 0)
                    {
                        ws.Cells[1, 10].Value = DT.Rows[0]["MAWBNO"]?.ToString();
                        ws.Cells[1, 15].Value = DT.Rows[0]["FLIGHTNUM"]?.ToString();
                        
                    }

                    #endregion

                    ep.Save();

                    newFile = null;
                    htData = null;
                    ws.Dispose();

                    result = $"/res/assets/temp/{copyfile}";
                }

                return new { status = 0, msg = result };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = 99, msg = "轉出失敗！請洽相關人員！" };
            }
        }

        [HttpGet("{id}")]
        public dynamic ExportNewBroker(long id)
        {
            try
            {
                string result = "";
                Hashtable htData = new Hashtable();
                htData["ID"] = id;
                string SHIPPINGNO = DBUtil.GetSingleValue1(@"SELECT SHIPPINGNO AS COL1 FROM T_V_SHIPPING_M WHERE ID = @ID AND STATUS = 0", htData);

                string filename = "NewCusBroker.xlsx";
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), $@"tectransit\dist\tectransit\assets\doc\");
                string copyfile = $"NewCusBroker_{SHIPPINGNO}.xlsx";
                string copypath = Path.Combine(Directory.GetCurrentDirectory(), $@"tectransit\dist\tectransit\assets\temp\");

                if (!Directory.Exists(copypath))
                    Directory.CreateDirectory(copypath);

                //檢查同檔名是否已存在過--->移除
                if (System.IO.File.Exists(copypath + copyfile))
                    System.IO.File.Delete(copypath + copyfile);

                System.IO.File.Copy(templatePath + filename, copypath + copyfile);
                FileInfo newFile = new FileInfo(copypath + copyfile);

                using (ExcelPackage ep = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = ep.Workbook.Worksheets[1];

                    #region excel 資料過程

                    string sql = $@"SELECT DISTINCT A.SHIPPINGNO, A.FLIGHTNUM, A.MAWBNO, B.CLEARANCENO, B.BOXWEIGHT
                                    FROM T_V_SHIPPING_M A
                                    LEFT JOIN T_V_SHIPPING_H B ON A.ID = B.SHIPPINGID_M
                                    WHERE A.ID = @ID AND STATUS = 0";

                    DataTable DT = DBUtil.SelectDataTable(sql, htData);
                    if (DT.Rows.Count > 0)
                    {
                        ws.Cells[2, 2].Value = DateTime.Now.ToString("yyyyMMdd");
                        ws.Cells[2, 3].Value = DT.Rows[0]["FLIGHTNUM"]?.ToString();
                        ws.Cells[2, 4].Value = DT.Rows[0]["MAWBNO"]?.ToString();
                        ws.Cells[2, 6].Value = DT.Rows.Count;

                        ws.Cells[2, 2].Style.Font.Color.SetColor(Color.Red);
                        ws.Cells[2, 3].Style.Font.Color.SetColor(Color.Red);
                        ws.Cells[2, 4].Style.Font.Color.SetColor(Color.Red);
                        ws.Cells[2, 6].Style.Font.Color.SetColor(Color.Red);

                        int row = 4, rownum = 1;                        
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            ws.Cells[row, 1].Value = rownum;

                            string tempCNO = "";
                            if (!string.IsNullOrEmpty(DT.Rows[i]["CLEARANCENO"]?.ToString()))
                            {
                                int? len = DT.Rows[i]["CLEARANCENO"]?.ToString().IndexOf('*');
                                if (len > 0)
                                    tempCNO = DT.Rows[i]["CLEARANCENO"]?.ToString().Substring(0, Convert.ToInt32(len));
                                else
                                    tempCNO = DT.Rows[i]["CLEARANCENO"]?.ToString();
                            }

                            ws.Cells[row, 2].Value = tempCNO;
                            ws.Cells[row, 3].Value = 1;
                            ws.Cells[row, 4].Value = DT.Rows[i]["BOXWEIGHT"]?.ToString();

                            ws.Cells[row, 1].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[row, 2].Style.Font.Color.SetColor(Color.Red);
                            ws.Cells[row, 3].Style.Font.Color.SetColor(Color.Red);
                            ws.Cells[row, 4].Style.Font.Color.SetColor(Color.Red);

                            row++;
                            rownum++;
                        }
                    }
                    
                    #endregion

                    ep.Save();
                    
                    newFile = null;                    
                    htData = null;
                    ws.Dispose();

                    result = $"/res/assets/temp/{copyfile}";
                }

                return new { status = 0, msg = result };
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
                return new { status = 99, msg = "轉出失敗！請洽相關人員！" };
            }

        }
    }
}