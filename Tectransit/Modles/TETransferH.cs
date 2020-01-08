using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TETransferH
    {
        public long Id { get; set; }
        public long Accountid { get; set; }
        public string Stationcode { get; set; }
        public string Trasferno { get; set; }
        public string Trasfercompany { get; set; }
        public string PLength { get; set; }
        public string PWidth { get; set; }
        public string PHeight { get; set; }
        public string PWeight { get; set; }
        public string PValueprice { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
