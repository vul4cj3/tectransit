using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSAclog
    {
        public long Id { get; set; }
        public string Usercode { get; set; }
        public string Username { get; set; }
        public string Position { get; set; }
        public string Target { get; set; }
        public string Message { get; set; }
        public DateTime? LogDate { get; set; }
    }
}
