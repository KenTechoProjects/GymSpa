namespace Domain.Application.QRCode
{
    public class QRCodeReq
    {
        public string CustomerName { get; set; }
        public double WalletBalance { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}