using Domain.Models;
using System.Threading.Tasks;

namespace SharedService.Interface
{
    public interface ISharedService
    {
        Task<ResponseParam> GenerateQRCode(string data);

        Task<ResponseParam> GetMemberType();

        Task<ResponseParam> ConvertBase64toimage(ImaReq imaReq);
    }
}