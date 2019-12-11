using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSRank
    {
        public long Id { get; set; }
        public int Ranktype { get; set; }
        public string Rankcode { get; set; }
        public string Rankseq { get; set; }
        public string Rankname { get; set; }
        public string Rankdesc { get; set; }
        public bool Isenable { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
