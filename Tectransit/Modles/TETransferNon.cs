﻿using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TETransferNon
    {
        public long Id { get; set; }
        public string Stationcode { get; set; }
        public string Transferno { get; set; }
        public string Transfercompany { get; set; }
        public string Accountname { get; set; }
        public string PLength { get; set; }
        public string PWidth { get; set; }
        public string PHeight { get; set; }
        public string PWeight { get; set; }
        public string PValueprice { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
    }
}
