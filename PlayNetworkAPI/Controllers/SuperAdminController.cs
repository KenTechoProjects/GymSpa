using APICore.Application.Superadmin.Interface;
using Domain.Application.SuperAdmin;
using Domain.Application.SuperAdmin.Login;
using Domain.Application.SuperMember.DTO;
using Domain.Application.Vendor;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PlayNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminModuleService _superAdminModuleService;

        public SuperAdminController(ISuperAdminModuleService superAdminModuleService)
        {
            _superAdminModuleService = superAdminModuleService;
        }

        [HttpPost("profile-SuperAdmin")]
        public async Task<IActionResult> ProfileSuperAdmin([FromBody] SuperAdminReq superAdminReq)
        {
            var obj = await _superAdminModuleService.ProfileSuperAdmin(superAdminReq);
            return Ok(obj);
        }

        [HttpPost("SuperAdmin-login")]
        public async Task<IActionResult> SuperAdminLogin([FromBody] LoginReq loginReq)
        {
            var obj = await _superAdminModuleService.SuperAdminLogin(loginReq);
            return Ok(obj);
        }

        // Get: api/GetUserProfileByLoginToken
        [HttpGet("GetSuperAdminProfileByLoginToken")]
        public async Task<IActionResult> GetSuperAdminProfileByLoginToken
            (string Logintoken)
        {
            var obj = await _superAdminModuleService.GetSuperAdminProfileByLoginToken(Logintoken);
            return Ok(obj);
        }

        [HttpPost("SuperAdmin-updateprofile")]
        public async Task<IActionResult> SuperAdminUpdateProfile([FromForm] SuperAdminUpdateProfileReq superAdminUpdateProfileReq)
        {
            var obj = await _superAdminModuleService.SuperAdminUpdateProfile(superAdminUpdateProfileReq);
            return Ok(obj);
        }

        [HttpPost("SuperAdmin-createstaff")]
        public async Task<IActionResult> SuperAdmincreatestaff([FromBody] SuperAdminCreateStaffReq superAdminCreateStaffReq)
        {
            var obj = await _superAdminModuleService.SuperAdmincreatestaff(superAdminCreateStaffReq);
            return Ok(obj);
        }

        [HttpPost("SuperAdmin-supermember")]
        public async Task<IActionResult> SuperAdmincreatesSuperMember([FromBody] SuperMemberRegReq superMemberRegReq)
        {
            var obj = await _superAdminModuleService.SuperAdmincreatesSuperMember(superMemberRegReq);
            return Ok(obj);
        }

        [HttpGet("SuperAdmin-delstaff")]
        public async Task<IActionResult> SuperAdmindelstaff
        (string superAdminSoftToken, int superAdminUserID, int AdminstaffID)
        {
            var obj = await _superAdminModuleService.SuperAdmindelstaff(superAdminSoftToken, superAdminUserID, AdminstaffID);
            return Ok(obj);
        }

        [HttpPost("create_vendor")]
        public async Task<IActionResult> create_vendor([FromBody] VendorRegReq vendorRegReq)
        {
            var obj = await _superAdminModuleService.create_vendor(vendorRegReq);
            return Ok(obj);
        }
    }
}