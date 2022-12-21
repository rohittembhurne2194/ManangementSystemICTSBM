using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class ULBFormStatusVM
    {
        public ULBFormStatus QrCode { get; set; } = new ULBFormStatus();
        public ULBFormStatus Banners { get; set; } = new ULBFormStatus();
        public ULBFormStatus Abhipray { get; set; } = new ULBFormStatus();
        public ULBFormStatus Disclaimer { get; set; } = new ULBFormStatus();
        public ULBFormStatus EntryBook { get; set; } = new ULBFormStatus();
    }
    public class ULBFormStatus
    {
        public List<QrPrintedVM> Printed { get; set; } = new List<QrPrintedVM>();
        public List<QrSentVM> Sent { get; set; } = new List<QrSentVM>();
        public List<QrReceiveVM> Received { get; set; } = new List<QrReceiveVM>();
    }
}
