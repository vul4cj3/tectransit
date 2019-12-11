using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TNDeclarant
    {
        public long Id { get; set; }
        public long ShippingidH { get; set; }
        public long Declarantid { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
