using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin
{
    public class GeneralSettingsReq
    {
        [Required]
        public string superAdminSoftToken { get; set; }

        public double Dispatch_Commission { get; set; }
    }
}