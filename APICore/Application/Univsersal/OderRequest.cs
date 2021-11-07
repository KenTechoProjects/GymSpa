using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.Univsersal
{
   public class OderRequest
    {
 
            public string Vendor_code { get; set; }
            public string Item_code { get; set; }
            public string Service_type { get; set; }
            public string Demand_type { get; set; }
            public string MembershipID { get; set; }
            public int Number_of_persons { get; set; }
            public string Table_type { get; set; }
            public Nullable<DateTime> GymSpa_Date { get; set; }
            public string Service_category { get; set; }


        
    }
}
