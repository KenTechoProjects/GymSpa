using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin
{
    public class InitiatePaymentReq
    {
        [Required]
        public string superAdminSoftToken { get; set; }

        public string BeneficiaryFullName { get; set; }
        public string SchemeCode { get; set; }
        public string SchemeType { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
    }
}