using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TVShippingH
    {
        public long Id { get; set; }
        public string Clearanceno { get; set; }
        public string Transferno { get; set; }
        public string Trackingno { get; set; }
        public string Depotstatus { get; set; }
        public string Receiver { get; set; }
        public string Receiveraddr { get; set; }
        public string Receiverphone { get; set; }
        public string Weight { get; set; }
        public string Totalitem { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public long ShippingidM { get; set; }
    }
}
