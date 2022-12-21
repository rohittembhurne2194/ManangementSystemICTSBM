using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainSPModels
{
    public class sp_getULB_statusById_Result
    {
        public string formType { get; set; }
        public Nullable<bool> sent { get; set; }
        public Nullable<bool> prin { get; set; }
    }
}
