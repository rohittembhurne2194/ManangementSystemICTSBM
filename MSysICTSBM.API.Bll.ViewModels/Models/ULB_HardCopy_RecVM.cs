using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULB_HardCopy_RecVM
    {
        public int Id { get; set; }
        public int? ULBId { get; set; }
        public int? userId { get; set; }
        public bool? Agreement { get; set; }
        public int? AgreementUserId { get; set; }
        public DateTime? AgreementDate { get; set; }
        public bool? Banner { get; set; }
        public int? BannerUserId { get; set; }
        public DateTime? BannerDate { get; set; }
        public bool? Abhipray { get; set; }
        public int? AbhiprayUserId { get; set; }
        public DateTime? AbhiprayDate { get; set; }
        public bool? Disclaimer { get; set; }
        public int? DisclaimerUserId { get; set; }
        public DateTime? DisclaimerDate { get; set; }
        public bool? EntryBook { get; set; }
        public int? EntryBookUserId { get; set; }
        public DateTime? EntryBookDate { get; set; }
    }
}
