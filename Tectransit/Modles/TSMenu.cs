using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSMenu
    {
        public long Id { get; set; }
        public string Menucode { get; set; }
        public string Parentcode { get; set; }
        public string Menuurl { get; set; }
        public string Menuseq { get; set; }
        public string Menuname { get; set; }
        public string Menudesc { get; set; }
        public string Iconurl { get; set; }
        public bool Isback { get; set; }
        public bool Isvisible { get; set; }
        public bool Isenable { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
