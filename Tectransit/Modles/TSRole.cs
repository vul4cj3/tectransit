using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSRole
    {
        public long Id { get; set; }
        public string Rolecode { get; set; }
        public string Roleseq { get; set; }
        public string Rolename { get; set; }
        public string Roledesc { get; set; }
        public bool Isenable { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
