using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSSequencecode
    {
        public long Id { get; set; }
        public string Startcode { get; set; }
        public string Endcode { get; set; }
        public string Firstcode { get; set; }
        public string Nextcode { get; set; }
        public string Codename { get; set; }
        public string Codedesc { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
