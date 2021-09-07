using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class DetailLogin
    {
        private int status;
        private string notification;
        private string fullname;
        private string mssv;

        public DetailLogin(int status, string notification, string fullname, string mssv)
        {
            this.status = status;
            this.notification = notification;
            this.fullname = fullname;
            this.mssv = mssv;
        }

        public int Status { get => status; set => status = value; }
        public string Notification { get => notification; set => notification = value; }
        public string FullName { get => fullname; set => fullname = value; }
        public string MSSV { get => mssv; set => mssv = value; }
    }
}