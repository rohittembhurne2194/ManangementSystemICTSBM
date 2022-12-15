using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ActiveULBVM
    {
        public int? Id { get; set; }
        public string AppName { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public int? ULBId { get; set; }
        public int? dtDiff { get; set; }
    }
}
