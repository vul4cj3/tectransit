﻿using System;
using System.Collections.Generic;

namespace Tectransit.Modles
{
    public partial class TDFaqD
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public string Faqdseq { get; set; }
        public bool Istop { get; set; }
        public bool Isenable { get; set; }
        public DateTime? Credate { get; set; }
        public string Createby { get; set; }
        public DateTime? Upddate { get; set; }
        public string Updby { get; set; }
        public string Faqhid { get; set; }
    }
}
