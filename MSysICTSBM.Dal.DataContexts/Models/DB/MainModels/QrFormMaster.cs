using System;
using System.Collections.Generic;

#nullable disable

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainModels
{
    public partial class QrFormMaster
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
