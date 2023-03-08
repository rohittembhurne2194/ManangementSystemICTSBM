using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULBDocStatusVMDocData
    {
        public string DocName { get; set; }
        public string DocSubName { get; set; }
        public List<DataAll> DataAll { get; set; }
    }

    public class DataAll
    {
        public string DocSubNameNew { get; set; }

        public bool DocSentStatus { get; set; }
        public List<Send> SendData { get; set; }

        public bool DocDigCopyRecStatus { get; set; }
        public List<DigCopy> DCData { get; set; }

        public bool DocHardCopyRecStatus { get; set; }
        public List<HardCopy> HCData { get; set; }
    }

    public class Send
    {
      
        public string DocSentCreateUserName { get; set; }
        public string DocSentNote { get; set; }

        public DateTime? CreationDate { get; set; }
        public string DocSubName { get; set; }
        //public bool DocSentStatus { get; set; }
    }
    public class DigCopy
    {
       
        public string DocSentCreateUserName { get; set; }
        public string DocSentNote { get; set; }
        public DateTime? CreationDate { get; set; }
        public string DocSubName { get; set; }
    }

    public class HardCopy
    {
       
        public string DocSentCreateUserName { get; set; }
        public string DocSentNote { get; set; }
        public DateTime? CreationDate { get; set; }
        public string DocSubName { get; set; }
    }
   
}
