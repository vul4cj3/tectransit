using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TVShippingM
    {
        public long Id { get; set; }
        public long Accountid { get; set; }
        public string Shippingno { get; set; }
        public string Trackingno { get; set; }
        public string Mawbno { get; set; }
        public string Trasferno { get; set; }
        public string Total { get; set; }
        public int Trackingtype { get; set; }
        public string Receiver { get; set; }
        public string Receiceraddr { get; set; }
        public bool Ismultreceiver { get; set; }
        public int Status { get; set; }
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
