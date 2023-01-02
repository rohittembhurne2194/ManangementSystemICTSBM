using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULBDocStatusVM
    {
        public string AppName { get; set; }
        public string DocName { get; set; }
        public string DocSubName { get; set; }
        public Nullable<bool> DocSentStatus { get; set; }
        public string DocSentCreateUserName { get; set; }
        public string DocSentUpdateUserName { get; set; }
        public Nullable<DateTime> DocSentCreateDate { get; set; }
        public Nullable<DateTime> DocSentUpdateDate { get; set; }

        public Nullable<bool> DocDigCopyRecStatus { get; set; }

        public string DocDigCopyRecCreateUserName { get; set; }
        public string DocDigCopyRecUpdateUserName { get; set; }
        public Nullable<DateTime> DocDigCopyRecCreateDate { get; set; }
        public Nullable<DateTime> DocDigCopyRecUpdateDate { get; set; }
        public Nullable<bool> DocHardCopyRecStatus { get; set; }

        public string DocHardCopyRecCreateUserName { get; set; }
        public string DocHardCopyRecUpdateUserName { get; set; }
        public Nullable<DateTime> DocHardCopyRecCreateDate { get; set; }
        public Nullable<DateTime> DocHardCopyRecUpdateDate { get; set; }
    }
}
