using Dapper;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using QRCoder;
using SharedService.Interface;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;

namespace SharedService.Services
{
    public class SharedServiceExtented : ISharedService
    {
        private readonly Logger _logger;
        private readonly IConfiguration _configuration;
        private readonly BaseUrls _baseUrls;

        public SharedServiceExtented(Logger logger, IConfiguration configuration, IOptions<BaseUrls> baseUrls)
        {
            _logger = logger;
            _configuration = configuration;
            _baseUrls = baseUrls.Value;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DB_A57DC4_PNADb"));
            }
        }

        public async Task<ResponseParam> GenerateQRCode(string data)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{data}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                // Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.DarkRed, Color.PaleGreen, true);
                //Set color by using Color-class types
                //Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.DarkRed, Color.PaleGreen, true);
                //Set color by using HTML hex color notation
                //Bitmap qrCodeImage = qrCode.GetGraphic(20, "#000ff0", "#0ff000");
                //Set logo in center of QR-code MainMart ico.png
                Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.FromArgb(197, 118, 8), Color.White, (Bitmap)Bitmap.FromFile("Resources/Images/playnet.PNG"));
                int width = 250; // width of the Qr Code
                int height = 250; // height of the Qr Code
                Bitmap resize = new Bitmap(qrCodeImage, new Size(width, height));
                string fileGuid = Guid.NewGuid().ToString().Substring(0, 4);

                qrCodeData.SaveRawData("Resources/QRCodeImages/file-" + fileGuid + ".qrr", QRCodeData.Compression.Uncompressed);

                //QRCodeData qrCodeData1 = new QRCodeData("wwwroot/qrr/file-" + fileGuid + ".qrr", QRCodeData.Compression.Uncompressed);
                var bitmapBytes = BitmapToBytes(resize); //Convert bitmap into a byte array
                                                         // return File(bitmapBytes, "image/jpeg");
                                                         // return Ok(Convert.ToBase64String(bitmapBytes));
                                                         //Return as file result
                                                         //var obj = await _sharedService.GenerateQRCode(qRCodeReq);
                                                         //return Ok(obj);
                var sendbase64 = Convert.ToBase64String(bitmapBytes);

                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "QRCode to Base64", sendbase64);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][GenerateQRcode][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        // This method is for converting bitmap into a byte array
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public async Task<ResponseParam> GetMemberType()
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                // Query the database for a requested

                using (IDbConnection conn = Connection)
                {
                    string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_MemberType]";

                    var Sys_Rolesresults = await conn.QueryAsync(selectQuery, new
                    {
                    });
                    int row = Sys_Rolesresults.Count();
                    if (row >= 1)
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Success", Sys_Rolesresults);
                    }
                    else if (row <= 1)
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "No Record found");
                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Oops. Something went wrong. Please try again later");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[MM][GetAllRoles][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> ConvertBase64toimage(ImaReq imaReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                var bytes = Convert.FromBase64String(imaReq.base64image);// a.base64image
                                                                         //or full path to file in temp location
                                                                         //var filePath = Path.GetTempFileName();

                // full path to file in current project location
                var folderName = Path.Combine("Resources", "Images");
                string filedir = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = $"{imaReq.MembershipID}.png";
                Debug.WriteLine(filedir);
                Debug.WriteLine(Directory.Exists(filedir));
                if (!Directory.Exists(filedir))
                { //check if the folder exists;
                    Directory.CreateDirectory(filedir);
                }
                string file = Path.Combine(filedir, fileName);
                var fullPath = Path.Combine(filedir, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                Debug.WriteLine(file);
                //Debug.WriteLine(File.Exists(file));

                if (bytes.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                }
                var getprofilepicUrl = _baseUrls.ProfilepictureUrl + fileName;
                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "SUCCESS", getprofilepicUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][Convertbase64string][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }
    }
}