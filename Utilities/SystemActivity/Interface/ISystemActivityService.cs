using Domain.Application.SystemUtility.Sys_Logs;
using Domain.Models;
using System.Threading.Tasks;

namespace Utilities.SystemActivity.Interface
{
    public interface ISystemActivityService
    {
        Task<ResponseParam> sys_Log(Sys_logReq sys_LogReq);

        Task<ResponseParam> JsonRequestLog(object data);

        Task<ResponseParam> JsonErrorLog(object data);
    }
}