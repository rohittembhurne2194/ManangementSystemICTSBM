using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainSPModels
{
    public class sp_Get_ActiveULB_result
    {
        public int? Id { get; set; }
        public string AppName { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public int? ULBId { get; set; }
        public int? dtDiff { get; set; }

    }
}
