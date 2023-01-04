using System;
using System.Collections.Generic;

#nullable disable

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainModels
{
    public partial class QrPrinted
    {
        public int PrintId { get; set; }
        public int? ULBId { get; set; }
        public DateTime? PrintDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? HouseQty { get; set; }
        public int? HouseGreen { get; set; }
        public int? HouseBlue { get; set; }
        public int? DumpQty { get; set; }
        public int? StreetQty { get; set; }
        public int? LiquidQty { get; set; }
        public int? BannerAcrylic { get; set; }
        public int? DumpAcrylic { get; set; }
        public int? AbhiprayForm { get; set; }
        public int? DisclaimerForm { get; set; }
        public int? DataEntryBook { get; set; }
        public string Note { get; set; }
        public DateTime? UpdationDate { get; set; }
        public int? CreateUserId { get; set; }
        public int? UpdateUserId { get; set; }
    }
}
