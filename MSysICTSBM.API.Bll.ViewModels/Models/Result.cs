using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class Result
    {
        public string status { get; set; }
        public string message { get; set; }
        public string gismessage { get; set; }
        public string messageMar { get; set; }
        public string giserrorMessages { get; set; }
        public bool isAttendenceOn { get; set; }
        public bool isAttendenceOff { get; set; }

        public string emptype { get; set; }

        public string applink { get; set; }

        public int houseid { get; set; }
        public bool IsExixts { get; set; }
    }
}
