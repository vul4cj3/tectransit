﻿using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TVShippingD
    {
        public long Id { get; set; }
        public string Product { get; set; }
        public string Producturl { get; set; }
        public string Unitprice { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public string Origin { get; set; }
        public string Remark { get; set; }
        public long ShippingidM { get; set; }
        public long ShippingidH { get; set; }
    }
}
