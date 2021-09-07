using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication2.Models
{
    public partial class Account
    {
        public int No { get; set; }
        public string MSSV { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? Score { get; set; }
    }
}
