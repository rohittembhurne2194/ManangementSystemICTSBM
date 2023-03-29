using System;
using System.Collections.Generic;

#nullable disable

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainModels
{
    public partial class DocSubMaster
    {
        public int Id { get; set; }
        public int? DocId { get; set; }
        public string DocSubName { get; set; }
        public DateTime? DocSubDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
