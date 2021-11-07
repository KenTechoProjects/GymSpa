using System;

namespace Domain.Application.SuperAdmin.AuthDTOs
{
    public class SuperAdminAuthResponse
    {
        public string username { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiry { get; set; }
        public string SoftToken { get; set; }
    }
}