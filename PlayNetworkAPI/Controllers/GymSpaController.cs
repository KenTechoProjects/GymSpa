using APICore.Application.GymSpa.Connectors;
using AutoMapper;
using Domain.Application.GymSpa.DTO;
using Domain.Application.GymSpa.Extensions;
using Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Service;

namespace PlayNetworkAPI.Controllers
{
    [Route("api/gym-spa")]
    [ApiController]
    [DisplayName("Gym and Spa Services")]
    public class GymSpaController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IGymSpaService<ServerResponse> _gymSpa;
        private readonly IMapper _mapper;

        public GymSpaController(IWebHostEnvironment hostEnvironment, IGymSpaService<ServerResponse> gymSpaService, IMapper mapper)
        {
            this._hostEnvironment = hostEnvironment;
            this._gymSpa = gymSpaService;
            this._mapper = mapper;
        }

        #region Create Record

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-dutyroaster")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateDutyRoaster([FromBody] DutyRoasterDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _mapper.Map<DutyRoasterDTOView, DutyRoasterDTO>(model);

                    var result = await _gymSpa.Add(data);
                    if (result.Code == "00")
                    {
                        return Created("api/gym-spa/create-dutyroaster", result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = model, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPost]
        [Route("book-appointment")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Active == true)
                    {
                        model.Closed = false;
                        model.Cancelled = false;
                    }
                    var result = _mapper.Map<AppointmentDTOView, AppointmentDTO>(model);
                    var data = await _gymSpa.BookAppointment(result);
                    if (data.Code == "00")
                    {
                        return Created("api/gym-spa/book-appointment", data);
                    }
                    else
                    {
                        return BadRequest(data);
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = model, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-product")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var imagePath = await _gymSpa.UploadFile(model.FileImage, model.Name, model.UploadType);
                    if (imagePath.Code == "00")
                    {
                        var data = _mapper.Map<ProductDTOView, ProductDTO>(model);
                        result = await _gymSpa.Add(data);
                        if (result.Code == "00")
                        {
                            return Created("api/gym-spa/create-product", result);
                        }
                        else
                        {
                            return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                        }
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { imagePath }, IsSuccessful = false, Message = imagePath.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("create-working-days-and-time")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateWorkingDays([FromBody] DurationDTOView[] model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();

                    var data = _mapper.Map<DurationDTOView[], WorkingDateDTO[]>(model);
                    result = await _gymSpa.SetWorkingDaysandTime(data);
                    if (result.Code == "00")
                    {
                        return Created("api/gym-spa/create-working-days-and-time", result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-working-duration")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateDuration([FromBody] DurationDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var data = _mapper.Map<DurationDTOView, DurationDTO>(model);

                    result = await _gymSpa.Add(data);
                    if (result.Code == "00")
                    {
                        return Created("api/gym-spa/create-working-duration", result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        //new Uri("api/gym-spa/")
        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-product-category")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateProductCategory([FromBody] ProductCategoryDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var data = _mapper.Map<ProductCategoryDTO>(model);

                    result = await _gymSpa.Add(data);
                    if (result.Code == "00")
                    {
                        return Created("api/gym-spa/create-product-category", result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPost]
        [Route("create-day-sale")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateDaySale([FromBody] SalesDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var data = _mapper.Map<SalesDTO>(model);

                    result = await _gymSpa.Add(data);
                    if (result.Code == "00")
                    {
                        return Created("api/gym-spa/create-day-sale", result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPost]
        [Route("create-order-detail")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetailDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var data = _mapper.Map<OrderDetailDTO>(model);

                    result = await _gymSpa.Add(data);
                    if (result.Code == "00")
                    {
                        return Created("api/gym-spa/create-other-detail", result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPost]
        [Route("create-vendor-staff")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateStaff([FromBody] StaffDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var guid = new Guid().ToString();

                    var path = await _gymSpa.UploadFile(model.ProfileImage, guid, model.UploadType = UploadType.Document);
                    if (path.Code == "00")
                    {
                        var result = new ServerResponse();
                        var data = _mapper.Map<StaffDTO>(model);

                        result = await _gymSpa.Add(data);
                        if (result.Code == "00")
                        {
                            return Created("api/gym-spa/create-vendor-staff", result);
                        }
                        else
                        {
                            return NotFound(new ServerResponse { Code = ResponseCodes.UNSUCCESSFUL, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                        }
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = path.Code, Data = new { path.Data }, IsSuccessful = false, Message = path.Message, RequestId = path.RequestId, Status = path.Status });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPost]
        [Route("create-vendor-stock")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateStock([FromBody] StockDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var data = _mapper.Map<StockDTO>(model);

                    result = await _gymSpa.Add(data);
                    if (result.Code == "00")
                    {
                        return Created("api/gym-spa/create-vendor-stock", result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = result.Code, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = result.RequestId, Status = result.Status });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPost]
        [Route("create-vendor-services")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateVendorServices([FromBody] BaseServiceDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var data = _mapper.Map<BaseServiceDTO>(model);

                    result = await _gymSpa.Add(data);
                    if (result.Code == "00")
                    {
                        return Created("api/gym-spa/create-vendor-services", result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = result.Code, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = result.RequestId, Status = result.Status });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPost]
        [Route("create-relate-vendor-to-services")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> JoinVendorToServices([FromBody] PnaVendorServiceDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var data = _mapper.Map<PnaVendorServiceDTO>(model);

                    result = await _gymSpa.Add(data);
                    if (result.Code == "00")
                    {
                        return Created("api/gym-spa/create-relate-vendor-to-services", result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = result.Code, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = result.RequestId, Status = result.Status });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPost]
        [Route("create-order")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTORequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();

                    var data = new ProductDTO();
                    var rs = await _gymSpa.CreateOrder(model, data);
                    if (rs != null)
                    {
                        result = (ServerResponse)rs;
                        if (result.Code == "00")
                        {
                            return Created("api/gym-spa/create-relate-vendor-to-services", result);
                        }
                        else
                        {
                            var tt = result != null ? result.Data : null;
                            return NotFound(new ServerResponse { Code = result.Code, Data = new { tt }, IsSuccessful = false, Message = result.Message, RequestId = result.RequestId, Status = result.Status });
                        }
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = result.Code, Data = null, IsSuccessful = false, Message = result.Message, RequestId = result.RequestId, Status = result.Status });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPost]
        [Route("print-order-payment-receipt")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status201Created)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> PrintReceipt(  PrintReceiptRequest model)
        {
            var result = new ServerResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    var data = new ProductDTO();
                    var rs = await _gymSpa.PrintReceipt(model);
                    if (rs != null)
                    {
                        result = rs;
                        if (result.Code == "00")
                        {
                            return Created("api/gym-spa/print-order-payment-receipt", result);
                        }
                        else
                        {
                            var tt = result != null ? result.Data : null;
                            return NotFound(new ServerResponse { Code = result.Code, Data = new { tt }, IsSuccessful = false, Message = result.Message, RequestId = result.RequestId, Status = result.Status });
                        }
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = result.Code, Data = null, IsSuccessful = false, Message = result.Message, RequestId = result.RequestId, Status = result.Status });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        #endregion Create Record

        #region Update Data

        /// <summary>
        ///
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approve-appointment/{appointmentId}/{vendorId}")]
        //[ProducesDefaultResponseType(typeof(ServerResponse))]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> ApproveAppointment(  long appointmentId, int vendorId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();

                    result = await _gymSpa.ApproveAppointment(appointmentId, vendorId);
                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpPatch]
        [Route("update-appointment")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> UpdateAppointment([FromBody] AppointmentDTOView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var data = _mapper.Map<AppointmentDTO>(model);

                    result = await _gymSpa.UpdateAppointment(data, data.AppointmentColumnUpdate);
                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

 

        [HttpPut]
        [Route("update-order/{orderId}")]
        //[ProducesDefaultResponseType(typeof(ServerResponse))]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDTORequests model,   long orderId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();
                    var mod = _mapper.Map<OrderDTO>(model);

                    var rs = await _gymSpa.EditOrder(mod, orderId);
                    if (result != null)
                    {
                        result = (ServerResponse)rs;
                        if (result.Code == "00")
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                        }
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = null, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        #endregion Update Data

        #region Delete Or Cancel data
[HttpPost]
        [Route("cancel-appointment/{appointmentId}/{vendorCode}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> CancelAppointment( long appointmentId, string vendorCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();

                    result = await _gymSpa.CancelAppointment(appointmentId, vendorCode);
                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        #endregion Delete Or Cancel data

        #region View Data

        [HttpGet]
        [Route("get-available-staffs/{vendorId}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> AvailableStaff(  int vendorId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _gymSpa.AvailableStaffs(vendorId);

                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = vendorId, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-total-appoinment-member/{vendorId}/{memberId}/{dateOfAppointment}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> TotalCostOsAppointment(  int vendorId, int memberId, DateTime dateOfAppointment)
        {
            try
            {
                if (dateOfAppointment != default(DateTime) && vendorId > 0 && memberId > 0)
                {
                    var result = await _gymSpa.TotalCostOfAnAppointment(vendorId, memberId, dateOfAppointment);
                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { vendorId, memberId, dateOfAppointment }, IsSuccessful = false, Message = "Please all field is required", RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("view-sales/{date}/{vendorCode}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> ViewSales(  DateTime date, string vendorCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = new ServerResponse();

                    result = await _gymSpa.PreviewSalesrecordsAndCommission(date, vendorCode);
                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { result }, IsSuccessful = false, Message = result.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                    }
                }
                else
                {
                    string err = string.Empty;
                    ModelState.Values.ToList().ForEach(p =>
                    {
                        err = string.Join(" | ", p.Errors);
                    });
                    return NotFound(new ServerResponse { Code = ResponseCodes.ERROR, Data = new { err }, IsSuccessful = false, Message = err, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        [Route("get-all-service-providers")]
        public async Task<IActionResult> GetServiceProviders()
        {
            try
            {
                var data = await _gymSpa.GetServiceProviders();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
          ;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        [Route("get-service-providers-services-product/{vendorsCode}")]
        public async Task<IActionResult> GetServiceByVendor(  string vendorsCode)
        {
            try
            {
                var data = await _gymSpa.Getallservice_by_vendor(vendorsCode);
                if (data.Code == "00")
                {
                    return Ok(data);
                }
                else
                {
                    return BadRequest(data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-available-booking-dates/{vendorCode}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> Availabledates(string vendorCode)
        {
            try
            {
                var result = await _gymSpa.AvailableDates(vendorCode);
                if (result.Code == "00")
                {
                    List<WorkingDateDTO> dates = (List<WorkingDateDTO>)result.Data;
                    return Ok(dates.Take(50));
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-all-appointments")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetAppointments()
        {
            try
            {
                var result = await _gymSpa.ViewAll(new AppointmentDTO());
                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-individual-appointment/{vendorId}/{appointmrntId}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetAppointment(  int vendorId, long appointmrntId)
        {
            try
            {
                var result = await _gymSpa.View(new AppointmentDTO(), appointmrntId, 0, 0, 0, vendorId);
                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-all-workng-days-ofa-vendor")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetAllWorokingDays()
        {
            try
            {
                var result = await _gymSpa.ViewAll(new WorkingDateDTO());
                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-individual-workng-days/{vendorId}/{datesId}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetIndividualWorkingDays(  int vendorId, long datesId)
        {
            try
            {
                var result = await _gymSpa.View(new WorkingDateDTO(), datesId, 0, 0, 0, vendorId);
                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-all-discount-levels")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetAllDisCountLevel()
        {
            try
            {
                var result = await _gymSpa.ViewAll(new DiscountLevelDTO());
                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-vendor-discount-level/{vendorId}/{discountLevelId}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetDiscountLevels(  int vendorId, long discountLevelId)
        {
            try
            {
                var result = await _gymSpa.View(new DiscountLevelDTO(), discountLevelId, 0, 0, 0, vendorId);
                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-all-orders")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var result = await _gymSpa.ViewAll(new OrderDTO());
                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-order-ofa-vendor/{vendorId}/{id)}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetOrderPerVendor(  int vendorId, long id)
        {
            try
            {
                var result = await _gymSpa.View(new OrderDTO(), id, 0, 0, 0, vendorId);
                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-order-member/{vendorId}/{orderId}/{memberId}/{orderDate}/{productId}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetOrderPerMemberr(  int vendorId, long orderId, string memberId, DateTime orderDate, long productId)
        {
            try
            {
                if (vendorId > 0 && orderId > 0 && !string.IsNullOrWhiteSpace(memberId) && orderDate != default(DateTime) && productId > 0)
                {
                    var result = await _gymSpa.GetMemberOrder(vendorId, orderId, memberId, orderDate);

                    if (result.Code == "00")
                    {
                        var orderDetail = await _gymSpa.GetMemberOrderDetail(orderId, memberId, orderDate);
                        if (orderDetail.Code == "00")
                        {
                            var GetAmountPayable = await _gymSpa.GetMemberOrderAmount((IEnumerable<OrderDetailDTO>)orderDetail.Data);
                            if (GetAmountPayable.Code == "00")
                            {
                                double amountPayable = (double)GetAmountPayable.Data;
                                var data = (OrderDTO)result.Data;
                                data.Total_payable_amount = amountPayable;

                                result.Data = data;
                                return Ok(result);
                            }
                            else
                            {
                                return BadRequest(GetAmountPayable);
                            }
                        }
                        else
                        {
                            return BadRequest(orderDetail);
                        }
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest(new ServerResponse { Code = ResponseCodes.EMPTYFIELD, Data = null, IsSuccessful = false, Message = "Some fields are empty", RequestId = null, Status = StatusCodesExtended.Status410InvalidObject.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-products-ofa-vendor/{vendorCode}/{productId}/{categoryId}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetProductForVendor(  string vendorCode, long productId, long categoryId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(vendorCode) && productId > 0 && categoryId > 0)
                {
                    var result = await _gymSpa.View(new ProductDTO(), productId, 0, 0, categoryId, 0, vendorCode);

                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest(new ServerResponse { Code = ResponseCodes.EMPTYFIELD, Data = null, IsSuccessful = false, Message = "Some fields are empty", RequestId = null, Status = StatusCodesExtended.Status410InvalidObject.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-all-products")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var result = await _gymSpa.ViewAll(new ProductDTO());

                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-productcategories-ofa-vendor/{vendorId}/{id}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetProductCategoriesForVendor(  int vendorId, long id)
        {
            try
            {
                if (vendorId > 0 && vendorId > 0)
                {
                    var result = await _gymSpa.View(new ProductCategoryDTO(), id, 0, 0, 0, vendorId);

                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest(new ServerResponse { Code = ResponseCodes.EMPTYFIELD, Data = null, IsSuccessful = false, Message = "Some fields are empty", RequestId = null, Status = StatusCodesExtended.Status410InvalidObject.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-all-product-categories")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetProductCategories()
        {
            try
            {
                var result = await _gymSpa.ViewAll(new ProductCategoryDTO());

                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-sale-of-a-vendor/{vendorId}/{id}/{orderId}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetSalesForVendor(  int vendorId, long id, long orderId)
        {
            try
            {
                if (vendorId > 0 && vendorId > 0)
                {
                    var result = await _gymSpa.View(new SalesDTO(), id, 0, orderId, 0, vendorId);

                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest(new ServerResponse { Code = ResponseCodes.EMPTYFIELD, Data = null, IsSuccessful = false, Message = "Some fields are empty", RequestId = null, Status = StatusCodesExtended.Status410InvalidObject.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-all-sales")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetSales()
        {
            try
            {
                var result = await _gymSpa.ViewAll(new SalesDTO());

                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-stocks-ofa-vendor/{vendorId}/{id}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetStocksForVendor(  int vendorId, long id)
        {
            try
            {
                if (vendorId > 0 && vendorId > 0)
                {
                    var result = await _gymSpa.View(new ProductCategoryDTO(), id, 0, 0, 0, vendorId);

                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest(new ServerResponse { Code = ResponseCodes.EMPTYFIELD, Data = null, IsSuccessful = false, Message = "Some fields are empty", RequestId = null, Status = StatusCodesExtended.Status410InvalidObject.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-all-stocks")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetStocks()
        {
            try
            {
                var result = await _gymSpa.ViewAll(new StockDTO());

                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-staffs-ofa-vendor/{vendorCode}/{staffId}")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetStaffsForVendor(  string vendorCode, string staffId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(vendorCode) && !string.IsNullOrWhiteSpace(staffId))
                {
                    var result = await _gymSpa.View(new StaffDTO(), 0, 0, 0, 0, 0, vendorCode, staffId);

                    if (result.Code == "00")
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest(new ServerResponse { Code = ResponseCodes.EMPTYFIELD, Data = null, IsSuccessful = false, Message = "Some fields are empty", RequestId = null, Status = StatusCodesExtended.Status410InvalidObject.ToString() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        [HttpGet]
        [Route("get-all-staffs")]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status200OK)]
        [ProducesResponseType(typeof(ServerResponse), StatusCodesExtended.Status400BadRequest)]
        public async Task<IActionResult> GetStaffs()
        {
            try
            {
                var result = await _gymSpa.ViewAll(new StaffDTO());

                if (result.Code == "00")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse { Code = ResponseCodes.ERROR, Data = ex, IsSuccessful = false, Message = ex.Message, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status500InternalServerError.ToString() });
            }
        }

        #endregion View Data
    }

    //public class ModelState_
    //{
    //    public static string ModelStateErrors()
    //    {
    //        string err = string.Empty;
    //        ModelState.Values.ToList().ForEach(p =>
    //        {
    //            err = string.Join(" | ", p.Errors);

    //        });
    //        return errors;
    //    }
    //}
}