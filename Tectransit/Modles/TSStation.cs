using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSStation
    {
        public long Id { get; set; }
        public string Stationcode { get; set; }
        public string Stationname { get; set; }
        public string Countrycode { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Stationseq { get; set; }
        public string Remark { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
