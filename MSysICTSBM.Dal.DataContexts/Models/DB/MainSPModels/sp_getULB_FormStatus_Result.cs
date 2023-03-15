using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainSPModels
{
    public class sp_getULB_FormStatus_Result
    {
        public string AppName { get; set; }
        public string FormName { get; set; }
        public string Name { get; set; }

        public Nullable<DateTime> CreationDate { get; set; }
        public Nullable<bool> PrintStatus { get; set; }
        public Nullable<bool> SendStatus { get; set; }
        public Nullable<bool> ReceiveStatus { get; set; }

        public Nullable<int> Id { get; set; }
        public Nullable<int> HouseQty { get; set; }
        public Nullable<int> HouseBlue { get; set; }
        public Nullable<int> HouseGreen { get; set; }
        public Nullable<int> DumpQty { get; set; }
        public Nullable<int> StreetQty { get; set; }
        public Nullable<int> LiquidQty { get; set; }

        public Nullable<int> BannerAcrylic { get; set; }
        public Nullable<int> DumpAcrylic { get; set; }

        public Nullable<int> AbhiprayForm { get; set; }
        public Nullable<int> DisclaimerForm { get; set; }
        public Nullable<int> DataEntryBook { get; set; }



    }
}
