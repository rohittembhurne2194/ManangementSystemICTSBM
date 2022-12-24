using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULBDocStatusVM
    {
        public string DocType { get; set; }
        public Nullable<bool> DocSend { get; set; }
        public Nullable<bool> DigCopyRec { get; set; }
        public Nullable<bool> HardCopyRec { get; set; }
    }
}
