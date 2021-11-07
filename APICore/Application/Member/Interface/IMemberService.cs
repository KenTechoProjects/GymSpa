using Domain.Application.Member.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace APICore.Application.Member.Interface
{
    public interface IMemberService
    {
        Task<ResponseParam> MemberReg(RegReq regReq);

        Task<ResponseParam> MemberLogin(MLoginReq loginReq);

        Task<ResponseParam> GetProfileByLoginToken(string Logintoken);

        Task<ResponseParam> UpdateAccountprofile(UpdateAccountprofileReq updateAccountprofileReq);

        Task<ResponseParam> UpdateAccountsecurity(UpdateBuyerAccountsecurityReq updateAccountsecurityReq);

        Task<ResponseParam> SetupTransactionpin(TransactionPinSetupReq transactionPinSetupReq);

        Task<ResponseParam> ChangeTransactionpin(ChangeTransactionPinReq changeTransactionPinReq);

        Task<ResponseParam> TransferWalletToWallet(Wallet2WalletTransferReq wallet2WalletTransferReq);

        Task<ResponseParam> GetWalletBalance(string WalletId);

        Task<ResponseParam> WalletNameEnquiry(WalletNameEnqReq walletNameEnqReq);

        Task<ResponseParam> Changepassword(ChangePasswordReq cooperatorChangePasswordReq);

        Task<ResponseParam> Restpassword(string Email);

        Task<ResponseParam> FundWalletExtention(fundWalletReq fundWalletReq);

        Task<ResponseParam> InitializeFundWallet(InitializeReq initializeReq);

        Task<ResponseParam> WalletTransactionHistory(WalletTransactionHistory walletTransactionHistory);

        Task<ResponseParam> Debit_Wallet(Debit_WalletReq debit_WalletRe);

        Task<ResponseParam> reversal_Wallet(ReversalFundReq reversalFundReq);

        Task<ResponseParam> payMembershipSubcription(payMembershipSubcription payMembershipSubcription);

        Task<ResponseParam> InitializeMembershipSubcription(InitializeReq initializeReq);

        Task<ResponseParam> Validate_activationcode(GetActivationcodeReq getActivationcodeReq);

        Task<ResponseParam> Resend_activationcode(Resend_activationcodeReq resend_ActivationcodeReq);

        Task<ResponseParam> Profilepictureupload(string Email, IFormFile profile_pic);

        Task<ResponseParam> debit_PNA(Debit_PNAreq debit_PNAreq);
    }
}