using APICore.Application.Member.Interface;
using Domain.Application.Member.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PlayNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembershipController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> MemberReg([FromBody] RegReq regReq)
        {
            var obj = await _memberService.MemberReg(regReq);
            return Ok(obj);
        }

        [HttpPost("Validate_activationcode")]
        public async Task<IActionResult> Validate_activationcode([FromBody] GetActivationcodeReq getActivationcodeReq)
        {
            var obj = await _memberService.Validate_activationcode(getActivationcodeReq);
            return Ok(obj);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> MemberLogin([FromBody] MLoginReq loginReq)
        {
            var obj = await _memberService.MemberLogin(loginReq);
            return Ok(obj);
        }

        [HttpPost("pay-MembershipSubscription")]
        public async Task<IActionResult> payMembershipSubcription([FromBody] payMembershipSubcription payMembershipSubcription)
        {
            var obj = await _memberService.payMembershipSubcription(payMembershipSubcription);
            return Ok(obj);
        }

        [HttpPost("Initialize-MembershipSubscription")]
        public async Task<IActionResult> InitializeMembershipSubcription([FromBody] InitializeReq initializeReq)
        {
            var obj = await _memberService.InitializeMembershipSubcription(initializeReq);
            return Ok(obj);
        }

        [HttpPost("Setup-Transactionpin")]
        public async Task<IActionResult> SetupTransactionpin([FromBody] TransactionPinSetupReq transactionPinSetupReq)
        {
            var obj = await _memberService.SetupTransactionpin(transactionPinSetupReq);
            return Ok(obj);
        }

        [HttpPost("Change-Transactionpin")]
        public async Task<IActionResult> ChangeTransactionpin([FromBody] ChangeTransactionPinReq changeTransactionPinReq)
        {
            var obj = await _memberService.ChangeTransactionpin(changeTransactionPinReq);
            return Ok(obj);
        }

        [HttpPost("Wallet-Transfer")]
        public async Task<IActionResult> TransferWalletToWallet([FromBody] Wallet2WalletTransferReq wallet2WalletTransferReq)
        {
            var obj = await _memberService.TransferWalletToWallet(wallet2WalletTransferReq);
            return Ok(obj);
        }

        [HttpGet("GetWalletBalance")]
        public async Task<IActionResult> GetWalletBalance([FromQuery] string WalletId)
        {
            var obj = await _memberService.GetWalletBalance(WalletId);
            return Ok(obj);
        }

        [HttpPost("WalletNameEnquiry")]
        public async Task<IActionResult> WalletNameEnquiry([FromBody] WalletNameEnqReq walletNameEnqReq)
        {
            var obj = await _memberService.WalletNameEnquiry(walletNameEnqReq);
            return Ok(obj);
        }

        [HttpPost("Changepassword")]
        public async Task<IActionResult> Changepassword([FromBody] ChangePasswordReq cooperatorChangePasswordReq)
        {
            var obj = await _memberService.Changepassword(cooperatorChangePasswordReq);
            return Ok(obj);
        }

        [HttpGet("Restpassword")]
        public async Task<IActionResult> Restpassword([FromQuery] string Email)
        {
            var obj = await _memberService.Restpassword(Email);
            return Ok(obj);
        }

        [HttpPost("Fund-Wallet")]
        public async Task<IActionResult> FundWalletExtention([FromBody] fundWalletReq fundWalletReq)
        {
            var obj = await _memberService.FundWalletExtention(fundWalletReq);
            return Ok(obj);
        }

        [HttpPost("Initialize-FundWallet")]
        public async Task<IActionResult> InitializeFundWallet([FromBody] InitializeReq initializeReq)
        {
            var obj = await _memberService.InitializeFundWallet(initializeReq);
            return Ok(obj);
        }

        [HttpPost("Wallet-TransactionHistory")]
        public async Task<IActionResult> WalletTransactionHistory([FromBody] WalletTransactionHistory walletTransactionHistory)
        {
            var obj = await _memberService.WalletTransactionHistory(walletTransactionHistory);
            return Ok(obj);
        }

        [HttpPost("Resend-activationcode")]
        public async Task<IActionResult> Resend_activationcode([FromBody] Resend_activationcodeReq resend_ActivationcodeReq)
        {
            var obj = await _memberService.Resend_activationcode(resend_ActivationcodeReq);
            return Ok(obj);
        }

        [HttpPost("upload-profilepic")]
        public async Task<IActionResult> Profilepictureupload([FromForm] string Email, IFormFile profile_pic)
        {
            var obj = await _memberService.Profilepictureupload(Email, profile_pic);
            return Ok(obj);
        }

        [HttpPost("Update-Accountprofile")]
        public async Task<IActionResult> UpdateAccountprofile([FromBody] UpdateAccountprofileReq updateAccountprofileReq)
        {
            var obj = await _memberService.UpdateAccountprofile(updateAccountprofileReq);
            return Ok(obj);
        }

        //[HttpPost("debit_PNA")]
        //public async Task<IActionResult> debit_PNA([FromBody]Debit_PNAreq debit_PNAreq)
        //{
        //    var obj = await _memberService.debit_PNA(debit_PNAreq);
        //    return Ok(obj);
        //}
        //[HttpPost("reversal_Wallet")]
        //public async Task<IActionResult> reversal_Wallet([FromBody]ReversalFundReq reversalFundReq)
        //{
        //    var obj = await _memberService.reversal_Wallet(reversalFundReq);
        //    return Ok(obj);
        //}
    }
}