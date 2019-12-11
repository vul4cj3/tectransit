using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSUser
    {
        public long Id { get; set; }
        public string Usercode { get; set; }
        public string Userpassword { get; set; }
        public string Userseq { get; set; }
        public string Username { get; set; }
        public string Userdesc { get; set; }
        public bool Isenable { get; set; }
        public int? Logincount { get; set; }
        public DateTime? Lastlogindate { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
