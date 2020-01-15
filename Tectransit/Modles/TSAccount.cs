using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSAccount
    {
        public long Id { get; set; }
        public string Usercode { get; set; }
        public string Userpassword { get; set; }
        public string Userseq { get; set; }
        public string Username { get; set; }
        public string Userdesc { get; set; }
        public string Companyname { get; set; }
        public string Rateid { get; set; }
        public string Warehouseno { get; set; }
        public string Email { get; set; }
        public string Taxid { get; set; }
        public string IdphotoF { get; set; }
        public string IdphotoB { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Addr { get; set; }
        public bool Isenable { get; set; }
        public int? Logincount { get; set; }
        public DateTime? Lastlogindate { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
