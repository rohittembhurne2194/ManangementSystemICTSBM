using System;
using System.Collections.Generic;

#nullable disable

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainModels
{
    public partial class ULB_App_Status
    {
        public int Id { get; set; }
        public int? ULBId { get; set; }
        public bool? CMSStatus { get; set; }
        public bool? AppStatus { get; set; }
        public int? CMSUserId { get; set; }
        public int? AppUserId { get; set; }
        public DateTime? CMSDate { get; set; }
        public DateTime? AppDate { get; set; }
    }
}
