using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
   public class ULBDocStatusVMNew
    {
        public string AppName { get; set; }
        public string DocName { get; set; }
        public int Id { get; set; }
        public List<SubDatum> SubData { get; set; }

    }

    public class SubDatum
    {
        
        public string DocSubName { get; set; }
        public bool DocSentStatus { get; set; }
        public bool DocDigCopyRecStatus { get; set; }

        public bool DocHardCopyRecStatus { get; set; }

        public string DocNameNew { get; set; }
        //public object DocSentCreateUserName { get; set; }
        //public object DocSentUpdateUserName { get; set; }
        //public object DocSentCreateDate { get; set; }
        //public object DocSentUpdateDate { get; set; }
        //public object DocSentNote { get; set; }
       
        //public object DocDigCopyRecCreateUserName { get; set; }
        //public object DocDigCopyRecUpdateUserName { get; set; }
        //public object DocDigCopyRecCreateDate { get; set; }
        //public object DocDigCopyRecUpdateDate { get; set; }
        //public object DocDigCopyNote { get; set; }

        //public object DocHardCopyRecCreateUserName { get; set; }
        //public object DocHardCopyRecUpdateUserName { get; set; }
        //public object DocHardCopyRecCreateDate { get; set; }
        //public object DocHardCopyRecUpdateDate { get; set; }
        //public object DocHardCopyNote { get; set; }


    }

}
