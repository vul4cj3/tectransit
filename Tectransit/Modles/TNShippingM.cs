using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TNShippingM
    {
        public long Id { get; set; }
        public long? Accountid { get; set; }
        public string Stationcode { get; set; }
        public string Shippingno { get; set; }
        public string Trackingno { get; set; }
        public string Trackingdesc { get; set; }
        public string Trackingremark { get; set; }
        public string PLength { get; set; }
        public string PWidth { get; set; }
        public string PHeight { get; set; }
        public string PWeight { get; set; }
        public string PValueprice { get; set; }
        public string Mawbno { get; set; }
        public string Clearanceno { get; set; }
        public string Hawbno { get; set; }
        public string Total { get; set; }
        public string Totalprice { get; set; }
        public string Receiver { get; set; }
        public string ReceiverAddr { get; set; }
        public string Receiverphone { get; set; }
        public bool Ismultreceiver { get; set; }
        public string Trackingtype { get; set; }
        public int Status { get; set; }
        public string Paytype { get; set; }
        public int Paystatus { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public DateTime? Paydate { get; set; }
        public DateTime? Exportdate { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
