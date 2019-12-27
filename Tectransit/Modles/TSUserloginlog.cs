using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSUserloginlog
    {
        public long Id { get; set; }
        public string Usercode { get; set; }
        public string Username { get; set; }
        public string Hostname { get; set; }
        public string Hostip { get; set; }
        public DateTime? LoginDate { get; set; }
    }
}
