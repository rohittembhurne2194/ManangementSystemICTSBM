using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULB_App_StatusVM
    {
        public int Id { get; set; }
        public int? ULBId { get; set; }
        public bool? CMSStatus { get; set; }
        public bool? AppStatus { get; set; }
        public int? userId { get; set; }
        public int? updateUserId { get; set; }
        public DateTime? CMSDate { get; set; }
        public DateTime? AppDate { get; set; }
    }
}
