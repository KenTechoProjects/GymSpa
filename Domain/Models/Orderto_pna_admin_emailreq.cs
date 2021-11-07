using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Orderto_pna_admin_emailreq
    {
        public string orderid { get; set; }
        public string vendorname { get; set; }
        public string member_fnmae { get; set; }
        public string member_phone { get; set; }
        public string item { get; set; }
        public string service_category { get; set; }
        public string orderdate { get; set; }
        public string vendoremail { get; set; }
        public double totalpay { get; set; }
    }
    public class Orderto_pna_admin_emailreqNew
    {
        public string orderid { get; set; }
        public string vendorname { get; set; }
        public string member_fnmae { get; set; }
        public string member_phone { get; set; }
        public string item { get; set; }
        public string service_category { get; set; }
        public string orderdate { get; set; }
        public string vendoremail { get; set; }
        public double totalpay { get; set; }
        public Enums.SendEmailType sendEmailType { get; set; }
    }
}
