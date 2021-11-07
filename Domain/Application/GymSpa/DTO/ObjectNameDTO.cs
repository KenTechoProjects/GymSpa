using System.Collections.Generic;

namespace Domain.Application.GymSpa.DTO
{
    public partial class ObjectNameDTO
    {
        public const string GYMSPA = "GYMSPA";
        public const string HOTEL_RESERVATION = "HOTEL_RESERVATION";
        public const string EVENT_TICKET = "EVENT_TICKET";

        public virtual List<string> GetObjectNames() => new List<string>(new string[] { GYMSPA, HOTEL_RESERVATION, EVENT_TICKET });
    }
}