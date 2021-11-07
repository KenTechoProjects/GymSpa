namespace Domain.Application.GymSpa.DTO
{
    public class Order_To_Pna_Admin_Email_Req
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
}