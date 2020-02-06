using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TVDeclarant
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Taxid { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Addr { get; set; }
        public string IdphotoF { get; set; }
        public string IdphotoB { get; set; }
        public string Appointment { get; set; }
        public long ShippingidM { get; set; }
    }
}
