using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class LoginModel
    {
        public LoginModel(string mSSV, string email)
        {
            MSSV = mSSV;
            Email = email;
        }

        public string MSSV { get; set; }
        public string Email { get; set; }
    }
}