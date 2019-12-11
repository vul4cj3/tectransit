using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSButton
    {
        public long Id { get; set; }
        public string Buttoncode { get; set; }
        public string Buttonseq { get; set; }
        public string Buttonname { get; set; }
        public string Buttondesc { get; set; }
        public string Buttontype { get; set; }
        public string Buttonevent { get; set; }
        public string Iconclass { get; set; }
        public string Menucode { get; set; }
        public bool Isenable { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
