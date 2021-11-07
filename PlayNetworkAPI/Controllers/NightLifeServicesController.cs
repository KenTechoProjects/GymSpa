using APICore.Application.NightLife.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PlayNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NightLifeServicesController : ControllerBase
    {
        private readonly INightLifeService _nightLifeService;

        public NightLifeServicesController(INightLifeService nightLifeService)
        {
            _nightLifeService = nightLifeService;
        }

        [HttpGet("Get-clubs")]
        public async Task<IActionResult> GetNight_clubs()
        {
            var obj = await _nightLifeService.GetNight_clubs();
            return Ok(obj);
        }

        [HttpGet("Getallservice_byvendor")]
        public async Task<IActionResult> Getallservice_byvendor([FromQuery] string vendorcode)
        {
            var obj = await _nightLifeService.Getallservice_byvendor(vendorcode);
            return Ok(obj);
        }

        //[HttpGet("Get-allproducts")]
        //public async Task<IActionResult> Getallservice()
        //{
        //    var obj = await _nightLifeService.Getallservice();
        //    return Ok(obj);
        //}
        //[HttpGet("Get-productbyVendor")]
        //public async Task<IActionResult> Getallservice([FromQuery]string vendor_code)
        //{
        //    var obj = await _nightLifeService.Getserviceby_vendor(vendor_code);
        //    return Ok(obj);
        //}
        [HttpPost("Make_order")]
        public async Task<IActionResult> Make_order([FromBody] MakeNightLifeOrderReq makeOrderReq)
        {
            var obj = await _nightLifeService.Make_order(makeOrderReq);
            return Ok(obj);
        }

        [HttpPost("Make_order_payment")]
        public async Task<IActionResult> Make_order_payment([FromBody] Make_order_paymentReq make_Order_PaymentReq)
        {
            var obj = await _nightLifeService.Make_order_payment(make_Order_PaymentReq);
            return Ok(obj);
        }
    }
}