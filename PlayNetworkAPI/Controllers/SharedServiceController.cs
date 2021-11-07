using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using SharedService.Interface;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace PlayNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedServiceController : ControllerBase
    {
        private readonly ISharedService _sharedService;

        public SharedServiceController(ISharedService sharedService)
        {
            _sharedService = sharedService;
        }

        [HttpGet("GetMemberType")]
        public async Task<IActionResult> GetMemberType()
        {
            var response = await _sharedService.GetMemberType();
            return Ok(response);
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                return stream.ToArray();
            }
        }

        [HttpPost("ConvertBase64toimage")]
        public async Task<IActionResult> ConvertBase64toimage([FromBody] ImaReq imaReq)
        {
            var response = await _sharedService.ConvertBase64toimage(imaReq);
            return Ok(response);
        }
    }
}