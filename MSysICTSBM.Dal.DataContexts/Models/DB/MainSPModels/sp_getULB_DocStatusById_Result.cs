using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainSPModels
{
    public class sp_getULB_DocStatusById_Result
    {
        public string DocType { get; set; }
        public Nullable<bool> DocSend { get; set; }
        public Nullable<bool> DigCopyRec { get; set; }
        public Nullable<bool> HardCopyRec { get; set; }
    }
}
