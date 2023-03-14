using System;
using System.Collections.Generic;

#nullable disable

namespace MSysICTSBM.Models.DB.MainModels
{
    public partial class ULB_Detail
    {
        public int Id { get; set; }
        public long? AppID { get; set; }
        public string AppName { get; set; }
        public long? House_property { get; set; }
        public long? Dump_property { get; set; }
        public long? Liquid_property { get; set; }
        public long? Street_property { get; set; }
        public bool? IsActive { get; set; }
        public long? CreateUserid { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUserid { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
