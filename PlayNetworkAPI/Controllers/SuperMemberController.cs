using APICore.Application.SuperMember.Interface;
using Domain.Application.SuperMember.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PlayNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperMemberController : ControllerBase
    {
        private readonly ISuperMemberService _superMemberService;

        public SuperMemberController(ISuperMemberService superMemberService)
        {
            _superMemberService = superMemberService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] SupermemberLoginReq loginReq)
        {
            var obj = await _superMemberService.Login(loginReq);
            return Ok(obj);
        }

        [HttpGet("GetProfileByLoginToken")]
        public async Task<IActionResult> GetProfileByLoginToken
        (string Logintoken)
        {
            var obj = await _superMemberService.GetProfileByLoginToken(Logintoken);
            return Ok(obj);
        }
    }
}