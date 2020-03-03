using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TETransferD
    {
        public long Id { get; set; }
        public string Product { get; set; }
        public string Producturl { get; set; }
        public string Unitprice { get; set; }
        public string Quantity { get; set; }
        public string Remark { get; set; }
        public long TransferidM { get; set; }
        public long TransferidH { get; set; }
    }
}
