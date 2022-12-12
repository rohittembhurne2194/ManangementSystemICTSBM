using System;
using System.Collections.Generic;
using System.Text;

namespace MSysICTSBM.API.Bll.ViewModels.Models
{
    public class EmployeeMasterVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public bool? IsActive { get; set; }
        public string IsActiveULB { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? lastModifyDateEntry { get; set; }
    }
}
