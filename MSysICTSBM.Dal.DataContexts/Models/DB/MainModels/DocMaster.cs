using System;
using System.Collections.Generic;

#nullable disable

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainModels
{
    public partial class DocMaster
    {
        public int Id { get; set; }
        public string DocName { get; set; }
        public DateTime? DocDate { get; set; }
    }
}
