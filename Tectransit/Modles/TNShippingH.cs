﻿using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TNShippingH
    {
        public long Id { get; set; }
        public string Transferno { get; set; }
        public string Boxno { get; set; }
        public string Receiver { get; set; }
        public string Receiveraddr { get; set; }
        public string Remark { get; set; }
        public long ShippingidM { get; set; }
    }
}
