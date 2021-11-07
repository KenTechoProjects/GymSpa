using Domain.Application.SuperAdmin;
using Domain.Application.SuperAdmin.Login;
using Domain.Application.SuperMember.DTO;
using Domain.Application.Vendor;
using Domain.Models;
using System.Threading.Tasks;

namespace APICore.Application.Superadmin.Interface
{
    public interface ISuperAdminModuleService
    {
        Task<ResponseParam> ProfileSuperAdmin(SuperAdminReq superAdminReq);

        Task<ResponseParam> SuperAdminLogin(LoginReq loginReq);

        Task<ResponseParam> GetSuperAdminProfileByLoginToken(string Logintoken);

        Task<ResponseParam> SuperAdminUpdateProfile(SuperAdminUpdateProfileReq superAdminUpdateProfileReq);

        Task<ResponseParam> SuperAdmincreatestaff(SuperAdminCreateStaffReq superAdminCreateStaffReq);

        Task<ResponseParam> SuperAdmincreatesSuperMember(SuperMemberRegReq superMemberRegReq);

        Task<ResponseParam> SuperAdmindelstaff(string superAdminSoftToken, int superAdminUserID, int AdminstaffID);

        Task<ResponseParam> create_vendor(VendorRegReq vendorRegReq);
    }
}