using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULB_DetailVM
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
        public string CreateUserName { get; set; }

        public string UpdateUserName { get; set; }
    }
}
