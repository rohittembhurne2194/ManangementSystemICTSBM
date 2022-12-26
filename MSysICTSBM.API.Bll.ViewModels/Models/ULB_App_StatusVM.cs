using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULB_App_StatusVM
    {
        public int Id { get; set; }
        public int? ULBId { get; set; }
        public int? UserId { get; set; }
        public bool? CMSStatus { get; set; }
        public bool? AppStatus { get; set; }
        public int? CMSUserId { get; set; }
        public int? AppUserId { get; set; }
        public DateTime? CMSDate { get; set; }
        public DateTime? AppDate { get; set; }
        public string CMSUserName { get; set; }
        public string AppUserName { get; set; }
    }
}
