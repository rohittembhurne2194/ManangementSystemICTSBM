using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULBStatusVM
    {
        public string formType { get; set; }
        public Nullable<bool> sent { get; set; }
        public Nullable<bool> prin { get; set; }
    }
}
