using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin
{
    public class SuperAdminCreateStaffReq
    {
        [Required]
        public string AdminSoftToken { get; set; }

        [Required]
        public string fullname { get; set; }

        [Required]
        public string user_type { get; set; }

        [Required]
        public int AdminUserID { get; set; }
    }
}