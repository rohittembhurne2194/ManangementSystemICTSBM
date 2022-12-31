using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULB_HardCopy_RecVM
    {
        public int Id { get; set; }
        public int? ULBId { get; set; }
        public int? DocSubID { get; set; }
        public bool? DocStatus { get; set; }
        public int? userId { get; set; }
        public int? DocCreateUserId { get; set; }
        public string DocCreateUserName { get; set; }
        public DateTime? DocCreateDate { get; set; }
        public int? DocUpdateUserId { get; set; }
        public string DocUpdateUserName { get; set; }
        public DateTime? DocUpdateDate { get; set; }
        public string Note { get; set; }

    }
}
