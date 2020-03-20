using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSTectrackrecord
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public int Active { get; set; }
        public string Shippingno { get; set; }
        public string Apiurl { get; set; }
        public string Senddata { get; set; }
        public string Status { get; set; }
        public string Msg { get; set; }
        public string Responsedata { get; set; }
        public string Remark { get; set; }
        public DateTime? Credate { get; set; }
        public DateTime? Upddate { get; set; }
    }
}
