namespace Domain.Application.GymSpa.DTO
{
    public partial class ServicePymentDTO
    {
        public ServicePymentDTO()
        {
            Order = new OrderDTO();
        }

        public double TotalAmountPayable { get; set; }
        public virtual OrderDTO Order { get; set; }
        public string VendorCode { get; set; }
        public string PnaMemberId { get; set; }
    }
}