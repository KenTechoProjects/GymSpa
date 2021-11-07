using System;

namespace Domain.Models
{
    public class MakeNightLifeOrderReq
    {
        public string Vendor_code { get; set; }
        public string item_code { get; set; }
        public string service_type { get; set; }
        public string demand_type { get; set; }
        public string MembershipID { get; set; }
        public int number_of_persons { get; set; }
        public string table_type { get; set; }
        public Nullable<DateTime> reservation_date { get; set; }
        public string service_category { get; set; }
    }
}