using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TNShippingH
    {
        public long Id { get; set; }
        public long Accountid { get; set; }
        public string Oldtrasferno { get; set; }
        public string Trasferno { get; set; }
        public string Trackingno { get; set; }
        public string Trackingdesc { get; set; }
        public string Trackingremark { get; set; }
        public string PLength { get; set; }
        public string PWidth { get; set; }
        public string PHeight { get; set; }
        public string PWeight { get; set; }
        public string PSource { get; set; }
        public string PTrackingno { get; set; }
        public string Total { get; set; }
        public string Receiver { get; set; }
        public string ReceiverAddr { get; set; }
        public string Trackingtype { get; set; }
        public int Combinetype { get; set; }
        public int Status { get; set; }
        public string Paytype { get; set; }
        public int Paystatus { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public DateTime? Importdate { get; set; }
        public DateTime? Paydate { get; set; }
        public DateTime? Exportdate { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
