using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SystemUtility.Sys_Logs
{
    public class Sys_logReq
    {
        [Required]
        public string Usertype { get; set; }

        [Required]
        public string channel { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public string UserID { get; set; }
    }
}