﻿using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TVShippingM
    {
        public long Id { get; set; }
        public long Accountid { get; set; }
        public string Shippingno { get; set; }
        public string Mawbno { get; set; }
        public string Flightnum { get; set; }
        public string Total { get; set; }
        public string Totalweight { get; set; }
        public int Trackingtype { get; set; }
        public string Shippercompany { get; set; }
        public string Shipper { get; set; }
        public string Receivercompany { get; set; }
        public string Receiver { get; set; }
        public string Receiverzipcode { get; set; }
        public string Receiveraddr { get; set; }
        public string Receiverphone { get; set; }
        public string Taxid { get; set; }
        public bool Ismultreceiver { get; set; }
        public int Status { get; set; }
        public string Storecode { get; set; }
        public string Mawbfile { get; set; }
        public string Shippingfile1 { get; set; }
        public string Shippingfile2 { get; set; }
        public string Brokerfile1 { get; set; }
        public string Brokerfile2 { get; set; }
        public long Imbrokerid { get; set; }
        public long Exbrokerid { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public string Mawbdate { get; set; }
        public DateTime? Paydate { get; set; }
        public DateTime? Exportdate { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
