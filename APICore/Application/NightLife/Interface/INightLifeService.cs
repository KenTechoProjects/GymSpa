using Domain.Models;
using System.Threading.Tasks;

namespace APICore.Application.NightLife.Interface
{
    public interface INightLifeService
    {
        Task<ResponseParam> GetNight_clubs();

        Task<ResponseParam> Getallservice_byvendor(string vendorcode);

        Task<ResponseParam> Getallservice();

        Task<ResponseParam> Getserviceby_vendor(string vendor_code);

        Task<ResponseParam> Make_order(MakeNightLifeOrderReq makeOrderReq);

        Task<ResponseParam> Make_order_payment(Make_order_paymentReq make_Order_PaymentReq);

        Task<ResponseParam> Send_emailnotification_vendor(Orderto_pna_admin_emailreq orderto_Pna_Admin_Emailreq);

        // Task<ResponseParam> Send_emailpaymentnotification_vendor(Orderto_pna_admin_emailreq orderto_Pna_Admin_Emailreq);
        Task<ResponseParam> Send_emailnotification_pnaadmin(Orderto_pna_admin_emailreq orderto_Pna_Admin_Emailreq);
    }
}