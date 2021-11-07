using Domain.Application.GymSpa.DTO;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Utilities.Service
{
    public class CustomFunctions
    {
        public static string GetLongMonth(DateTime serverTime)
        {
            var logmonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            return logmonth;
        }

        public static string GetLongDay(DateTime serverTime)
        {
            var logmonth = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
            return logmonth;
        }

        public static DateTime GetLongDate(DateTime serverTime)
        {
            // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            return logdate;
        }

        public static string RequestID()
        {
            var requestId = Helper.GenerateUniqueId(7);
            return requestId.ToString();
        }

        public static string ReferalCode()
        {
            string ReferralCode = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 5);

            return ReferralCode;
        }

        public static string GeneratePassword()
        {
            string password = Guid.NewGuid().ToString("N").ToLower()
                        .Replace("1", "").Replace("o", "").Replace("0", "")
                        .Substring(0, 10);

            return password;
        }

        public static string OrderId()
        {
            string OrderID = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 7);

            return OrderID;
        }

        public static T ConvertObject<T>(dynamic source, Type dest)
        {
            return Convert.ChangeType(source, dest);
        }

        public static class ConvertAll
        {
            public static AppointmentDTO GetAppointment(dynamic source)
            {
                return ConvertObject<AppointmentDTO>(source, typeof(AppointmentDTO));
            }

            public static WorkingDateDTO GetWorkingDate(dynamic source)
            {
                return ConvertObject<WorkingDateDTO>(source, typeof(WorkingDateDTO));
            }

            public static DiscountLevelDTO GetDiscountLevel(dynamic source)
            {
                return ConvertObject<DiscountLevelDTO>(source, typeof(DiscountLevelDTO));
            }

            public static OrderDTO GetOrder(dynamic source)
            {
                return ConvertObject<OrderDTO>(source, typeof(OrderDTO));
            }

            public static OrderDetailDTO GetOrderDetail(dynamic source)
            {
                return ConvertObject<OrderDetailDTO>(source, typeof(OrderDetailDTO));
            }

            public static PNAMember GetPNAMember(dynamic source)
            {
                return ConvertObject<PNAMember>(source, typeof(PNAMember));
            }

            public static PnaVendorDTO GetPnaVendor(dynamic source)
            {
                return ConvertObject<PnaVendorDTO>(source, typeof(PnaVendorDTO));
            }

            public static ProductDTO GetProduct(dynamic source)
            {
                return ConvertObject<ProductDTO>(source, typeof(ProductDTO));
            }

            public static ProductCategoryDTO GetProductCategory(dynamic source)
            {
                return ConvertObject<ProductCategoryDTO>(source, typeof(ProductCategoryDTO));
            }

            public static SalesDTO GetSales(dynamic source)
            {
                return ConvertObject<SalesDTO>(source, typeof(SalesDTO));
            }

            public static StaffDTO GetStaff(dynamic source)
            {
                return ConvertObject<StaffDTO>(source, typeof(StaffDTO));
            }

            public static Type GetStock(dynamic source)
            {
                return ConvertObject<StockDTO>(source, typeof(StockDTO));
            }

            public static bool CheckForSpecialCharacters(string input)
            {
                Regex rex = new Regex("^[a-z0-9 ]+$", RegexOptions.IgnoreCase);
                Boolean result = rex.IsMatch(input);
                return result;
            }
        }
    }
}