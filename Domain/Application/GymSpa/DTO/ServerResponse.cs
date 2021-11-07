using Domain.Models;

namespace Domain.Application.GymSpa.DTO
{
    public class ServerResponse : ResponseParam
    {
        public bool IsSuccessful { get; set; }
        public string Status { get; set; }
    }
}