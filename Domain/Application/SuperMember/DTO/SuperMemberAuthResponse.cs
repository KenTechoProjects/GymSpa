using System;

namespace Domain.Application.SuperMember.DTO
{
    public class SuperMemberAuthResponse
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}