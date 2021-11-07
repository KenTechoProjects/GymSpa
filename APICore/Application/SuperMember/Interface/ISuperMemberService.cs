using Domain.Application.SuperMember.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APICore.Application.SuperMember.Interface
{
    public interface ISuperMemberService
    {
        Task<ResponseParam> Login(SupermemberLoginReq loginReq);
        Task<ResponseParam> GetProfileByLoginToken(string Logintoken);
    }
}
