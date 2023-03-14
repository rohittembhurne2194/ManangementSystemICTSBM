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

        public string UserName { get; set; }
        public string HouseQty { get; set; }

        public DateTime? CreationDate { get; set; }
        public string FormName { get; set; }

    }
    public class Sent
    {

        public string UserName { get; set; }
        public string HouseQty { get; set; }
        public DateTime? CreationDate { get; set; }
        public string FormName { get; set; }
    }

    public class Receive
    {

        public string UserName { get; set; }
        public string HouseQty { get; set; }
        public DateTime? CreationDate { get; set; }
        public string FormName { get; set; }
    }
}
