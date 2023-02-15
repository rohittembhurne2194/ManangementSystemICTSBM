using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.Dal.DataContexts.Models.DB.MainSPModels
{
    public class sp_getULB_DocStatus_Result
    {
        public string AppName { get; set; }
        public string DocName { get; set; }

        public string DocSubName { get; set; }
        public Nullable<bool> DocSentStatus { get; set; }
        public string DocSentCreateUserName { get; set; }
        public string DocSentUpdateUserName { get; set; }
        public Nullable<DateTime> DocSentCreateDate { get; set; }
        public Nullable<DateTime> DocSentUpdateDate { get; set; }

        public string DocSentNote { get; set; }

        public Nullable<bool> DocDigCopyRecStatus { get; set; }

        public string DocDigCopyRecCreateUserName { get; set; }
        public string DocDigCopyRecUpdateUserName { get; set; }
        public Nullable<DateTime> DocDigCopyRecCreateDate { get; set; }
        public Nullable<DateTime> DocDigCopyRecUpdateDate { get; set; }

        public string DocDigCopyNote { get; set; }

        public Nullable<bool> DocHardCopyRecStatus { get; set; }

        public string DocHardCopyRecCreateUserName { get; set; }
        public string DocHardCopyRecUpdateUserName { get; set; }
        public Nullable<DateTime> DocHardCopyRecCreateDate { get; set; }
        public Nullable<DateTime> DocHardCopyRecUpdateDate { get; set; }

        public string DocHardCopyNote { get; set; }
    }
}
