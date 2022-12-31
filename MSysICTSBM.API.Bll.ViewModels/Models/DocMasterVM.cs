using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class DocMasterVM
    {
        public int Id { get; set; }
        public string DocName { get; set; }
        public DateTime? DocDate { get; set; }
    }
}
