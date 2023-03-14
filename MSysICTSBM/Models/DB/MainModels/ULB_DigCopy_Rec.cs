﻿using System;
using System.Collections.Generic;

#nullable disable

namespace MSysICTSBM.Models.DB.MainModels
{
    public partial class ULB_DigCopy_Rec
    {
        public int Id { get; set; }
        public int? ULBId { get; set; }
        public int? DocSubID { get; set; }
        public bool? DocStatus { get; set; }
        public int? DocCreateUserId { get; set; }
        public DateTime? DocCreateDate { get; set; }
        public int? DocUpdateUserId { get; set; }
        public DateTime? DocUpdateDate { get; set; }
        public string Note { get; set; }
    }
}
