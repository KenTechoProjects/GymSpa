using System.Collections.Generic;

namespace Domain.Application.SuperAdmin.AuthDTOs
{
    public class AdminStaffsList
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string user_type { get; set; }
        public string Email { get; set; }
        public string profile_pic { get; set; }
        public int roleid { get; set; }
    }

    public class AdminStaffsListResponse
    {
        public IList<AdminStaffsList> AdminStaffsList { get; set; }
    }
}