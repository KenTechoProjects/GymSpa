using APICore.Application.Vendor.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PlayNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly Ivendorservice _ivendorservice;

        public VendorController(Ivendorservice ivendorservice)
        {
            _ivendorservice = ivendorservice;
        }

        [HttpPost("Create-product")]
        public async Task<IActionResult> create_product([FromForm] Create_nightlife_productReq create_Nightlife_ProductReq)
        {
            var obj = await _ivendorservice.Create_nightlife_product(create_Nightlife_ProductReq);
            return Ok(obj);
        }
    }
}