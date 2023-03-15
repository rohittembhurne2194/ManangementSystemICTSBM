using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
   public  class ULBFormStatusVMNew
    {
        public string AppName { get; set; }
        public string FormName { get; set; }
        public int FormId { get; set; }
        public List<FormDataAll> DataAll { get; set; }
    }

    public class FormDataAll
    {
        public string FormNameNew { get; set; }

        public bool PrintStatus { get; set; }
        public List<Print> PrintData { get; set; }

        public bool SendStatus { get; set; }
        public List<Sent> SendData { get; set; }

        public bool ReceiveStatus { get; set; }
        public List<Receive> ReceiveData { get; set; }
    }

    public class Print
    {
        public Nullable<int> Id { get; set; }
        public string UserName { get; set; }
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

        public DateTime? CreationDate { get; set; }
        public string FormName { get; set; }

    }
    public class Sent
    {
        public Nullable<int> Id { get; set; }
        public string UserName { get; set; }
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
        public DateTime? CreationDate { get; set; }
        public string FormName { get; set; }
    }

    public class Receive
    {
        public Nullable<int> Id { get; set; }
        public string UserName { get; set; }
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
        public DateTime? CreationDate { get; set; }
        public string FormName { get; set; }
    }
}
