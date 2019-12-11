using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TSDeclarant
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Taxid { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Addr { get; set; }
        public string IdphotoF { get; set; }
        public string IdphotoB { get; set; }
        public string Appointment { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
