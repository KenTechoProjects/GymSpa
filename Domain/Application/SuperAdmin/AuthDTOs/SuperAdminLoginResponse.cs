namespace Domain.Application.SuperAdmin.AuthDTOs
{
    public class SuperAdminLoginResponse
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string phonenumber { get; set; }
        public string user_type { get; set; }
        public string Email { get; set; }
        public int roleid { get; set; }
        public string role { get; set; }
        public string profile_pic { get; set; }
        public string SoftToken { get; set; }
    }
}