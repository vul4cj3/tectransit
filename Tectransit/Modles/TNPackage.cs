using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TNPackage
    {
        public long Id { get; set; }
        public long ShippingidH { get; set; }
        public string Packtype { get; set; }
        public string Packname { get; set; }
        public string Packurl { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
