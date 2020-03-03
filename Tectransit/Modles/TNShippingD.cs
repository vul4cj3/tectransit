using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TNShippingD
    {
        public long Id { get; set; }
        public string Product { get; set; }
        public string Producturl { get; set; }
        public string Unitprice { get; set; }
        public string Quantity { get; set; }
        public string Remark { get; set; }
        public long ShippingidM { get; set; }
        public long ShippingidH { get; set; }
    }
}
