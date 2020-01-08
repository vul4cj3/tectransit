using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TNShippingD
    {
        public long Id { get; set; }
        public string Packname { get; set; }
        public string Packurl { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public long ShippingidH { get; set; }
    }
}
