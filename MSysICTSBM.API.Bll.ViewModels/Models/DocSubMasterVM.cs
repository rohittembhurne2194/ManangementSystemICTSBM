using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class DocSubMasterVM
    {
        public int Id { get; set; }
        public int? DocId { get; set; }
        public string DocSubName { get; set; }
        public DateTime? DocSubDate { get; set; }
    }
}
